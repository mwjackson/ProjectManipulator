using System.Collections.Generic;
using System.Linq;
using System.Xml;
using NUnit.Framework;
using deprojectreferencer.CopyLocal;

namespace deprojectreferencer.unit.tests.CopyLocal
{
    public class CopyLocalManipulatorTests
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
        public void Should_not_error_if_no_references()
        {
            _projectFile = new XmlDocument();
            _projectFile.Load(@"samples\project_with_no_project_references.csproj");

            Assert.That(() => new CopyLocalManipulator(MSBUILD_NAMESPACE).SetFalse(_projectFile), Throws.Nothing);
        } 

        [Test]
        public void Should_add_copy_local_reference_as_a_child_element()
        {
            new CopyLocalManipulator(MSBUILD_NAMESPACE).SetFalse(_projectFile);

            XmlNode reference = _projectFile.SelectSingleNode("/msb:Project/msb:ItemGroup/msb:Reference", _namespaceManager);

            Assert.That(reference["Private"], Is.Not.Null);
            Assert.That(reference["Private"].InnerText, Is.EqualTo(@"False"));
        } 

        [Test]
        public void Should_not_add_if_already_exists() {
            new CopyLocalManipulator(MSBUILD_NAMESPACE).SetFalse(_projectFile);
            new CopyLocalManipulator(MSBUILD_NAMESPACE).SetFalse(_projectFile);

            var privateNodes = _projectFile
                .SelectSingleNode("/msb:Project/msb:ItemGroup/msb:Reference", _namespaceManager)
                .SelectNodes("msb:Private", _namespaceManager)
                .Cast<XmlNode>();

            Assert.That(privateNodes.Count(), Is.EqualTo(1));
        }

        [Test]
        public void Should_set_false_if_true()
        {
            _projectFile = new XmlDocument();
            _projectFile.Load(@"samples\project_with_copylocal_true.csproj");

            new CopyLocalManipulator(MSBUILD_NAMESPACE).SetFalse(_projectFile);

            XmlNode reference = _projectFile.SelectSingleNode("/msb:Project/msb:ItemGroup/msb:Reference", _namespaceManager);

            Assert.That(reference["Private"], Is.Not.Null);
            Assert.That(reference["Private"].InnerText, Is.EqualTo(@"False"));
        }

        [Test]
        public void Should_set_false_for_every_node_in_list()
        {
            new CopyLocalManipulator(MSBUILD_NAMESPACE).SetFalse(_projectFile);

            IEnumerable<XmlNode> references = _projectFile.SelectNodes("/msb:Project/msb:ItemGroup/msb:Reference", _namespaceManager).Cast<XmlNode>();

            var falsePrivateNodes = references
                .SelectMany(x => x.SelectNodes("msb:Private", _namespaceManager).Cast<XmlNode>())
                .Where(node => node.InnerText == "False");

            Assert.That(falsePrivateNodes.Count(), Is.EqualTo(3));
        }

        [Test]
        public void Should_add_copy_local_reference_as_a_child_element_for_project_references()
        {
            new CopyLocalManipulator(MSBUILD_NAMESPACE).SetFalse(_projectFile);
            
            XmlNode reference = _projectFile.SelectSingleNode("/msb:Project/msb:ItemGroup/msb:ProjectReference", _namespaceManager);

            Assert.That(reference["Private"], Is.Not.Null);
            Assert.That(reference["Private"].InnerText, Is.EqualTo(@"False"));
        }

        [Test]
        public void Should_set_false_if_true_for_project_references()
        {
            _projectFile = new XmlDocument();
            _projectFile.Load(@"samples\project_with_copylocal_true.csproj");

            new CopyLocalManipulator(MSBUILD_NAMESPACE).SetFalse(_projectFile);

            XmlNode reference = _projectFile.SelectSingleNode("/msb:Project/msb:ItemGroup/msb:ProjectReference", _namespaceManager);

            Assert.That(reference["Private"], Is.Not.Null);
            Assert.That(reference["Private"].InnerText, Is.EqualTo(@"False"));
        }
    }
}