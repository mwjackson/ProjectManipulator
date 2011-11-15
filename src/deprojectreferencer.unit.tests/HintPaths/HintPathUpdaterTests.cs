using System.Linq;
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

        [SetUp]
        public void Setup()
        {
            _projectFile = new XmlDocument();
            _projectFile.Load(@"samples\project.csproj");

            _namespaceManager = new XmlNamespaceManager(_projectFile.NameTable);
            _namespaceManager.AddNamespace("msb", MSBUILD_NAMESPACE);

            _hintPathLookup = A.Fake<IHintPathLookup>();
        }

        [Test]
        public void Should_update_replace_wonga_hint_path_with_service_directory()
        {
            var commonPath = @"..\..\..\build\common\Wonga.Common.Data.dll";

            var outputProjectFile = new HintPathUpdater(MSBUILD_NAMESPACE, _hintPathLookup).Update(_projectFile);

            string updatedPath = outputProjectFile.SelectNodes("/msb:Project/msb:ItemGroup/msb:Reference/msb:HintPath", _namespaceManager)[1].InnerText;

            Assert.That(updatedPath, Is.EqualTo(commonPath));
        }

        [Test]
        public void Should_not_replace_hintPath_for_libs() {
            string originalPath = _projectFile.SelectNodes("/msb:Project/msb:ItemGroup/msb:Reference/msb:HintPath", _namespaceManager)[0].InnerText;

            var outputProjectFile = new HintPathUpdater(MSBUILD_NAMESPACE, _hintPathLookup).Update(_projectFile);

            string updatedPath = outputProjectFile.SelectNodes("/msb:Project/msb:ItemGroup/msb:Reference/msb:HintPath", _namespaceManager)[0].InnerText;

            Assert.That(updatedPath, Is.EqualTo(originalPath));
        }

        [Test]
        public void Should_replace_hint_path_correctly_for_each_common() {
            const string commonPath = @"..\..\..\build\common";

            var outputProjectFile = new HintPathUpdater(MSBUILD_NAMESPACE, _hintPathLookup).Update(_projectFile);

            string wongaCommonDataPath = outputProjectFile.SelectNodes("/msb:Project/msb:ItemGroup/msb:Reference/msb:HintPath", _namespaceManager)[1].InnerText;
            string wongaCommonUtilsPath = outputProjectFile.SelectNodes("/msb:Project/msb:ItemGroup/msb:Reference/msb:HintPath", _namespaceManager)[2].InnerText;

            Assert.That(wongaCommonDataPath, Is.StringContaining(commonPath));
            Assert.That(wongaCommonUtilsPath, Is.StringContaining(commonPath));
        }

        [Test]
        public void Should_replace_hint_path_for_each_service()
        {
            new HintPathUpdater(MSBUILD_NAMESPACE, _hintPathLookup).Update(_projectFile);

            A.CallTo(() => _hintPathLookup.For(A<string>.Ignored)).MustHaveHappened(Repeated.Exactly.Times(2));
        }
    }

    public class HintPathUpdater
    {
        private readonly string _msbuildNamespace;
        private readonly IHintPathLookup _hintPathLookup;

        public HintPathUpdater(string msbuildNamespace, IHintPathLookup hintPathLookup)
        {
            _msbuildNamespace = msbuildNamespace;
            _hintPathLookup = hintPathLookup;
        }

        public XmlDocument Update(XmlDocument projectFile)
        {
            var namespaceManager = new XmlNamespaceManager(projectFile.NameTable);
            namespaceManager.AddNamespace("msb", _msbuildNamespace);

            var hintPaths = projectFile.SelectNodes("/msb:Project/msb:ItemGroup/msb:Reference/msb:HintPath", namespaceManager).Cast<XmlNode>();

            foreach(var hintPath in hintPaths)
            {
                var oldPath = hintPath.InnerText;
                if (oldPath.Contains(@"..\..\..\lib")) continue;

                var newPath = _hintPathLookup.For(oldPath);
                hintPath.InnerText = newPath;
            }

            return projectFile;
        }
    }
}