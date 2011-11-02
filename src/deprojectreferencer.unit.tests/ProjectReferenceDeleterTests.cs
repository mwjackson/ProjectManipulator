using System.Xml;
using NUnit.Framework;

namespace deprojectreferencer.unit.tests
{
    [TestFixture]
    public class ProjectReferenceDeleterTests
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
        public void Should_remove_the_project_reference_itemgroup_from_the_document()
        {
            new ProjectReferenceDeleter().Delete(_projectFile, _namespaceManager);

            XmlNode projectReferenceItemGroup = _projectFile.SelectSingleNode("/msb:Project/msb:ItemGroup[msb:ProjectReference]", _namespaceManager);
            
            Assert.That(projectReferenceItemGroup, Is.Null);
        }

        [Test]
        public void Should_not_error_if_project_reference_item_group_doesnt_exist()
        {
            _projectFile = new XmlDocument();
            _projectFile.Load(@"samples\project_with_no_project_references.csproj");

            _namespaceManager = new XmlNamespaceManager(_projectFile.NameTable);
            _namespaceManager.AddNamespace("msb", MSBUILD_NAMESPACE);

            Assert.That(() => new ProjectReferenceDeleter().Delete(_projectFile, _namespaceManager), Throws.Nothing);
        }
    }
}