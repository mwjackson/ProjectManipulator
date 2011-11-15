using System.Xml;
using FakeItEasy;
using NUnit.Framework;
using deprojectreferencer.HintPaths;

namespace deprojectreferencer.unit.tests.HintPaths
{
    public class HintPathUpdaterTests
    {
        private XmlDocument _projectFile;
        private XmlNamespaceManager _namespaceManager;
        private IHintPathLookup _hintPathLookup;
        private const string MSBUILD_NAMESPACE = "http://schemas.microsoft.com/developer/msbuild/2003";
        
        private const string ProjectPath = @"samples\project.csproj";

        [SetUp]
        public void Setup()
        {
            _projectFile = new XmlDocument();
            _projectFile.Load(ProjectPath);

            _namespaceManager = new XmlNamespaceManager(_projectFile.NameTable);
            _namespaceManager.AddNamespace("msb", MSBUILD_NAMESPACE);

            _hintPathLookup = A.Fake<IHintPathLookup>();
        }

        [Test]
        public void Should_update_replace_wonga_hint_path_with_service_directory()
        {
            const string expectedPath = @"..\..\..\build\common\Wonga.Common.Data.dll";
            A.CallTo(() => _hintPathLookup.For(A<string>.Ignored, A<string>.Ignored)).Returns(expectedPath);

            var outputProjectFile = new HintPathUpdater(MSBUILD_NAMESPACE, _hintPathLookup).Update(ProjectPath);

            string updatedPath = outputProjectFile.SelectNodes("/msb:Project/msb:ItemGroup/msb:Reference/msb:HintPath", _namespaceManager)[1].InnerText;

            Assert.That(updatedPath, Is.EqualTo(expectedPath));
        }

        [Test]
        public void Should_not_replace_hintPath_for_libs() {
            string originalPath = _projectFile.SelectNodes("/msb:Project/msb:ItemGroup/msb:Reference/msb:HintPath", _namespaceManager)[0].InnerText;

            var outputProjectFile = new HintPathUpdater(MSBUILD_NAMESPACE, _hintPathLookup).Update(ProjectPath);

            string updatedPath = outputProjectFile.SelectNodes("/msb:Project/msb:ItemGroup/msb:Reference/msb:HintPath", _namespaceManager)[0].InnerText;

            Assert.That(updatedPath, Is.EqualTo(originalPath));
        }

        [Test]
        public void Should_replace_hint_path_for_each_service()
        {
            new HintPathUpdater(MSBUILD_NAMESPACE, _hintPathLookup).Update(ProjectPath);

            A.CallTo(() => _hintPathLookup.For(A<string>.Ignored, A<string>.Ignored)).MustHaveHappened(Repeated.Exactly.Times(2));
        }
    }
}