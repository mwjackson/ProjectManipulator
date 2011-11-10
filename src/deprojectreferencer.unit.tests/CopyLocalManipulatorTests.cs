using System.Linq;
using System.Xml;
using NUnit.Framework;

namespace deprojectreferencer.unit.tests
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
        public void Should_add_copy_local_reference_as_a_child_element()
        {
            XmlNode reference = _projectFile.CreateElement("Reference", _namespaceManager.DefaultNamespace);

            new CopyLocalManipulator(_namespaceManager).SetFalse(_projectFile, new[] { reference });

            Assert.That(reference.FirstChild.Name, Is.EqualTo(@"Private"));
            Assert.That(reference.FirstChild.InnerText, Is.EqualTo(@"False"));
        } 

        [Test]
        public void Should_not_add_if_already_exists() {
            XmlNode reference = _projectFile.CreateElement("Reference", _namespaceManager.DefaultNamespace);

            new CopyLocalManipulator(_namespaceManager).SetFalse(_projectFile, new[] { reference });
            new CopyLocalManipulator(_namespaceManager).SetFalse(_projectFile, new[] { reference });

            var privateNodes = reference.SelectNodes("Private", _namespaceManager).Cast<XmlNode>();

            Assert.That(privateNodes.Count(), Is.EqualTo(1));
        }

        [Test]
        public void Should_set_false_if_true()
        {
            XmlNode reference = _projectFile.CreateElement("Reference", _namespaceManager.DefaultNamespace);
            var privateNode = _projectFile.CreateElement("Private", _namespaceManager.DefaultNamespace);
            privateNode.InnerText = "True";
            reference.AppendChild(privateNode);

            new CopyLocalManipulator(_namespaceManager).SetFalse(_projectFile, new[] { reference });

            Assert.That(reference.FirstChild.Name, Is.EqualTo(@"Private"));
            Assert.That(reference.FirstChild.InnerText, Is.EqualTo(@"False"));
        }

        [Test]
        public void Should_set_false_for_every_node_in_list()
        {
            XmlNode reference1 = _projectFile.CreateElement("Reference", _namespaceManager.DefaultNamespace);
            XmlNode reference2 = _projectFile.CreateElement("Reference", _namespaceManager.DefaultNamespace);
            XmlNode reference3 = _projectFile.CreateElement("Reference", _namespaceManager.DefaultNamespace);
            var references = new[] {reference1, reference2, reference3};

            new CopyLocalManipulator(_namespaceManager).SetFalse(_projectFile, references);

            var falsePrivateNodes = references
                .SelectMany(x => x.SelectNodes("Private", _namespaceManager).Cast<XmlNode>())
                .Where(node => node.InnerText == "False");

            Assert.That(falsePrivateNodes.Count(), Is.EqualTo(3));
        }   
    }
}