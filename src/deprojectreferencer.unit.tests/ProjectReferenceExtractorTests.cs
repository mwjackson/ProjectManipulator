using System.Collections.Generic;
using System.Linq;
using System.Xml;
using NUnit.Framework;

namespace deprojectreferencer.unit.tests
{
    [TestFixture]
    public class ProjectReferenceExtractorTests
    {
        private XmlDocument _xmlDocument;
        private XmlNamespaceManager _namespaceManager;
        private const string MSBUILD_NAMESPACE = "http://schemas.microsoft.com/developer/msbuild/2003";

        [SetUp]
        public void Setup() {
            _xmlDocument = new XmlDocument();
            _xmlDocument.Load(@"samples\project.csproj");

            _namespaceManager = new XmlNamespaceManager(_xmlDocument.NameTable);
            _namespaceManager.AddNamespace("msb", MSBUILD_NAMESPACE);
        }

        [Test]
        public void Extracting_project_references_should_use_the_right_xpath() {
            var projects = new ProjectReferenceExtractor().Extract(_xmlDocument, _namespaceManager);
            
            Assert.That(projects.Count(), Is.EqualTo(2));
        }

        [Test]
        public void Extracting_project_references_should_map_them_to_project_references() {
            var projects = new ProjectReferenceExtractor().Extract(_xmlDocument, _namespaceManager);
            
            Assert.That(projects, Is.InstanceOf<IEnumerable<ProjectReference>>());
        }

        [Test]
        public void Extracting_when_there_are_no_project_references_should_not_error()
        {
            _xmlDocument = new XmlDocument();
            _xmlDocument.Load(@"samples\project_with_no_project_references.csproj");

            _namespaceManager = new XmlNamespaceManager(_xmlDocument.NameTable);
            _namespaceManager.AddNamespace("msb", MSBUILD_NAMESPACE);

            Assert.That(() => new ProjectReferenceExtractor().Extract(_xmlDocument, _namespaceManager), Throws.Nothing);
        }
    }
}
