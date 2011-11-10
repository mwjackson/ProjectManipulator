using System.Xml;

namespace deprojectreferencer
{
    public class OutputPathSetter
    {
        private readonly XmlNamespaceManager _namespaceManager;

        public OutputPathSetter(XmlNamespaceManager namespaceManager)
        {
            _namespaceManager = namespaceManager;
        }

        public XmlDocument Set(XmlDocument projectFile)
        {
            var assemblyName = projectFile.SelectSingleNode("/msb:Project/msb:PropertyGroup/msb:AssemblyName", _namespaceManager).InnerText;
            var path = string.Format(@"..\..\build\{0}\", assemblyName);

            var releasePropertyGroup = projectFile.SelectSingleNode("/msb:Project/msb:PropertyGroup[msb:OutputPath][contains(@Condition,'Release')]", _namespaceManager);
            releasePropertyGroup.SelectSingleNode("msb:OutputPath", _namespaceManager).InnerText = path;
            return projectFile;
        }
    }
}