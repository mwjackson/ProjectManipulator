using System.IO;
using System.Xml;
using FakeItEasy;
using NUnit.Framework;
using deprojectreferencer.ProjectReferences;

namespace deprojectreferencer.unit.tests.ProjectReferences
{
    [TestFixture]
    public class DeProjectReferencerTests
    {
        private const string MSBUILD_NAMESPACE = "http://schemas.microsoft.com/developer/msbuild/2003";
     
        private IProjectReferenceExtractor _projectReferenceExtractor;
        private IAssemblyReferenceConverter _assemblyReferenceConverter;
        private IProjectReferenceDeleter _projectReferenceDeleter;

        [Test, Category("AcceptanceTest")]
        public void Deprojectreferencing_should_match_expected_output()
        {
            string expected = File.ReadAllText(@"samples\expected.csproj");

            // to not interact with the other tests
            File.Copy(@"samples\project.csproj", @"samples\acceptance.csproj", true);

            new DeProjectReferencer(new ProjectReferenceExtractor(), new AssemblyReferenceConverter(MSBUILD_NAMESPACE), new ProjectReferenceDeleter()).Dereference(@"samples\acceptance.csproj");

            string dereferenced = File.ReadAllText(@"samples\acceptance.csproj");

            Assert.That(dereferenced, Is.EqualTo(expected));
        }

        [SetUp]
        public void Setup() {
            _projectReferenceExtractor = A.Fake<IProjectReferenceExtractor>();
            _assemblyReferenceConverter = A.Fake<IAssemblyReferenceConverter>();
            _projectReferenceDeleter = A.Fake<IProjectReferenceDeleter>();
        }

        [Test]
        public void Deprojectreferencing_should_extract_the_project_references()
        {

            new DeProjectReferencer(_projectReferenceExtractor, _assemblyReferenceConverter, _projectReferenceDeleter).Dereference(@"samples\acceptance.csproj");

            A.CallTo(() => _projectReferenceExtractor.Extract(A<XmlDocument>.Ignored, A<XmlNamespaceManager>.Ignored)).MustHaveHappened();
        }

        [Test]
        public void Deprojectreferencing_should_convert_the_project_references()
        {
            var projectReferences = new[] {new ProjectReference("name", "path")};
            A.CallTo(() => _projectReferenceExtractor.Extract(A<XmlDocument>.Ignored, A<XmlNamespaceManager>.Ignored))
                .Returns(projectReferences);

            new DeProjectReferencer(_projectReferenceExtractor, _assemblyReferenceConverter, _projectReferenceDeleter).Dereference(@"samples\acceptance.csproj");

            A.CallTo(() => _assemblyReferenceConverter.Convert(A<XmlDocument>.Ignored, A<XmlNamespaceManager>.Ignored, projectReferences)).MustHaveHappened();
        }

        [Test]
        public void Deprojectreferencing_should_delete_the_project_references()
        {
            var projectReferences = new[] { new ProjectReference("name", "path") };
            A.CallTo(() => _projectReferenceExtractor.Extract(A<XmlDocument>.Ignored, A<XmlNamespaceManager>.Ignored))
                .Returns(projectReferences);

            new DeProjectReferencer(_projectReferenceExtractor, _assemblyReferenceConverter, _projectReferenceDeleter).Dereference(@"samples\acceptance.csproj");

            A.CallTo(() => _projectReferenceDeleter.Delete(A<XmlDocument>.Ignored, A<XmlNamespaceManager>.Ignored)).MustHaveHappened();
        }
    }
}