using System.Collections.Generic;
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
            XmlNode reference = _projectFile.CreateElement("Reference");

            new CopyLocalManipulator(_projectFile).SetFalse(new[] {reference });

            Assert.That(reference.FirstChild.Name, Is.EqualTo(@"Private"));
            Assert.That(reference.FirstChild.InnerText, Is.EqualTo(@"False"));
        } 

        [Test]
        public void Should_not_add_if_already_exists() {
            XmlNode reference = _projectFile.CreateElement("Reference");

            new CopyLocalManipulator(_projectFile).SetFalse(new[] { reference });
            new CopyLocalManipulator(_projectFile).SetFalse(new[] { reference });

            var privateNodes = reference.SelectNodes("Private").Cast<XmlNode>();

            Assert.That(privateNodes.Count(), Is.EqualTo(1));
        }

        [Test]
        public void Should_set_false_if_true()
        {
            XmlNode reference = _projectFile.CreateElement("Reference");
            var privateNode = _projectFile.CreateElement("Private");
            privateNode.InnerText = "True";
            reference.AppendChild(privateNode);

            new CopyLocalManipulator(_projectFile).SetFalse(new[] { reference });

            Assert.That(reference.FirstChild.Name, Is.EqualTo(@"Private"));
            Assert.That(reference.FirstChild.InnerText, Is.EqualTo(@"False"));
        }

        [Test]
        public void Should_set_false_for_every_node_in_list()
        {
            XmlNode reference1 = _projectFile.CreateElement("Reference");
            XmlNode reference2 = _projectFile.CreateElement("Reference");
            XmlNode reference3 = _projectFile.CreateElement("Reference");
            var references = new[] {reference1, reference2, reference3};

            new CopyLocalManipulator(_projectFile).SetFalse(references);

            var falsePrivateNodes = references
                .SelectMany(x => x.SelectNodes("Private").Cast<XmlNode>())
                .Where(node => node.InnerText == "False");

            Assert.That(falsePrivateNodes.Count(), Is.EqualTo(3));
        }   
    }

    public class CopyLocalManipulator
    {
        private readonly XmlDocument _projectFile;

        public CopyLocalManipulator(XmlDocument projectFile)
        {
            _projectFile = projectFile;
        }

        public void SetFalse(IEnumerable<XmlNode> references)
        {
            foreach (var reference in references)
            {
                var privateNodes = reference.SelectNodes("Private").Cast<XmlNode>();
                if (privateNodes.Any())
                {
                    Toggle(privateNodes.First());
                    return;
                }

                var privateNode = _projectFile.CreateElement("Private");
                privateNode.InnerText = "False";
                reference.AppendChild(privateNode);
            }
        }

        private void Toggle(XmlNode privateNode)
        {
            privateNode.InnerText = "False";
        }
    }
}