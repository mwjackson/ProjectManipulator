using System.Linq;
using System.Xml;
using NUnit.Framework;
using ProjectManipulator.ProjectReferences;

namespace ProjectManipulator.Unit.Tests.ProjectReferences
{
    [TestFixture]
    public class AssemblyReferenceConverterTests
    {
        private XmlDocument _projectFile;
        private XmlNamespaceManager _namespaceManager;
        private const string MSBUILD_NAMESPACE = "http://schemas.microsoft.com/developer/msbuild/2003";

        [SetUp]
        public void Setup()
        {
            _projectFile = new XmlDocument();
            _projectFile.Load(@"samples\project.csproj");

            _namespaceManager = new XmlNamespaceManager(_projectFile.NameTable);
            _namespaceManager.AddNamespace("msb", MSBUILD_NAMESPACE);
        }

        [Test]
        public void Should_add_an_itemgroup_if_doesnt_exist()
        {
            _projectFile = new XmlDocument();
            _projectFile.Load(@"samples\project_with_no_assembly_itemgroup.csproj");

            _namespaceManager = new XmlNamespaceManager(_projectFile.NameTable);
            _namespaceManager.AddNamespace("msb", MSBUILD_NAMESPACE);

            new AssemblyReferenceConverter(MSBUILD_NAMESPACE).Convert(_projectFile, _namespaceManager, new ProjectReference[0]);

            var itemGroups = _projectFile.SelectNodes("/msb:Project/msb:ItemGroup", _namespaceManager);
            
            Assert.That(itemGroups.Count, Is.EqualTo(2));
        }

        [Test]
        public void Should_add_an_assembly_reference_for_each_project_reference()
        {
            var projectReferences = new[] {new ProjectReference("name1", "path1"), new ProjectReference("name2", "path2")};

            new AssemblyReferenceConverter(MSBUILD_NAMESPACE).Convert(_projectFile, _namespaceManager, projectReferences);

            var assemblyReferences = _projectFile.SelectNodes("/msb:Project/msb:ItemGroup/msb:Reference", _namespaceManager);

            Assert.That(assemblyReferences.Count, Is.EqualTo(5));
        }

        [Test]
        public void Each_assembly_reference_should_be_of_the_correct_structure()
        {
            _projectFile = new XmlDocument();
            _projectFile.Load(@"samples\project_with_no_assembly_itemgroup.csproj");

            _namespaceManager = new XmlNamespaceManager(_projectFile.NameTable);
            _namespaceManager.AddNamespace("msb", MSBUILD_NAMESPACE);

            var projectReferences = new[] { new ProjectReference("name1", "path1") };

            new AssemblyReferenceConverter(MSBUILD_NAMESPACE).Convert(_projectFile, _namespaceManager, projectReferences);

            var assemblyReference = _projectFile.SelectNodes("/msb:Project/msb:ItemGroup/msb:Reference", _namespaceManager).Cast<XmlNode>().First();

            Assert.That(assemblyReference.Attributes["Include"].Value, Is.EqualTo("name1"));
            Assert.That(assemblyReference.FirstChild.Name, Is.EqualTo(@"HintPath"));
            Assert.That(assemblyReference.FirstChild.InnerText, Is.EqualTo(@"..\..\..\build\name1\name1.dll"));
        }
    }
}