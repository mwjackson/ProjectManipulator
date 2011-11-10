using System.IO;
using System.Xml;
using NUnit.Framework;

namespace deprojectreferencer.unit.tests
{
    public class OutputPathSetterTests
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
        public void Should_set_the_output_path_only_for_release_configuration() {
            var resultDocument = new OutputPathSetter(_namespaceManager).Set(_projectFile);

            var releasePropertyGroup = resultDocument.SelectSingleNode("/msb:Project/msb:PropertyGroup[msb:OutputPath][contains(@Condition,'Release')]", _namespaceManager);

            var releasePath = releasePropertyGroup
                .SelectSingleNode("msb:OutputPath", _namespaceManager)
                .InnerText;
            Assert.That(releasePath, Is.StringContaining(@"..\..\build\"));
        }

        [Test]
        public void Should_not_set_the_output_path_only_for_other_configurations()
        {
            const string path = @"bin\Debug\";

            var resultDocument = new OutputPathSetter(_namespaceManager).Set(_projectFile);

            var debugPropertyGroup = resultDocument.SelectSingleNode("/msb:Project/msb:PropertyGroup[msb:OutputPath][contains(@Condition,'Debug')]", _namespaceManager);

            var debugPath = debugPropertyGroup
                .SelectSingleNode("msb:OutputPath", _namespaceManager)
                .InnerText;
            Assert.That(debugPath, Is.EqualTo(path));
        }

        [Test]
        public void Should_set_output_path_based_on_project_name()
        {
            const string path = @"..\..\build\Wonga.Risk.Handlers.CalculateCreditLimit\";

            var resultDocument = new OutputPathSetter(_namespaceManager).Set(_projectFile);

            var releasePropertyGroup = resultDocument.SelectSingleNode("/msb:Project/msb:PropertyGroup[msb:OutputPath][contains(@Condition,'Release')]", _namespaceManager);

            var releasePath = releasePropertyGroup
                .SelectSingleNode("msb:OutputPath", _namespaceManager)
                .InnerText;
            Assert.That(new DirectoryInfo(releasePath), Is.EqualTo(new DirectoryInfo(path)));
        }
    }
}