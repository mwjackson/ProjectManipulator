using System.IO;
using System.Linq;
using System.Xml;
using NUnit.Framework;

namespace deprojectreferencer.unit.tests.HintPaths
{
    public class HintPathUpdaterTests
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
        public void Should_update_replace_wonga_hint_path_with_service_directory()
        {
            var commonPath = @"..\..\..\build\common\Wonga.Common.Data.dll";

            var outputProjectFile = new HintPathUpdater(MSBUILD_NAMESPACE).Update(_projectFile);

            string updatedPath = outputProjectFile.SelectNodes("/msb:Project/msb:ItemGroup/msb:Reference/msb:HintPath", _namespaceManager)[1].InnerText;

            Assert.That(updatedPath, Is.EqualTo(commonPath));
        }

        [Test]
        public void Should_not_replace_hintPath_for_libs() {
            string originalPath = _projectFile.SelectNodes("/msb:Project/msb:ItemGroup/msb:Reference/msb:HintPath", _namespaceManager)[0].InnerText;

            var outputProjectFile = new HintPathUpdater(MSBUILD_NAMESPACE).Update(_projectFile);

            string updatedPath = outputProjectFile.SelectNodes("/msb:Project/msb:ItemGroup/msb:Reference/msb:HintPath", _namespaceManager)[0].InnerText;

            Assert.That(updatedPath, Is.EqualTo(originalPath));
        }

        [Test]
        public void Should_replace_hint_path_correctly_for_each_common() {
            var commonPath = @"..\..\..\build\common";

            var outputProjectFile = new HintPathUpdater(MSBUILD_NAMESPACE).Update(_projectFile);

            string wongaCommonDataPath = outputProjectFile.SelectNodes("/msb:Project/msb:ItemGroup/msb:Reference/msb:HintPath", _namespaceManager)[1].InnerText;
            string wongaCommonUtilsPath = outputProjectFile.SelectNodes("/msb:Project/msb:ItemGroup/msb:Reference/msb:HintPath", _namespaceManager)[2].InnerText;

            Assert.That(wongaCommonDataPath, Is.StringContaining(commonPath));
            Assert.That(wongaCommonUtilsPath, Is.StringContaining(commonPath));
        }

        [Test]
        public void Should_replace_hint_path_correctly_for_each_service() {
            Assert.Fail("pending");
        }
    }

    public class HintPathUpdater
    {
        private readonly string _msbuildNamespace;

        public HintPathUpdater(string msbuildNamespace)
        {
            _msbuildNamespace = msbuildNamespace;
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
                
                var fileName = new FileInfo(oldPath).Name;
                var newPath = string.Format(@"..\..\..\build\common\{0}", fileName);
                hintPath.InnerText = newPath;
            }

            return projectFile;
        }
    }
}