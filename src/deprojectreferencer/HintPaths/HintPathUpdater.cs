using System.Linq;
using System.Xml;

namespace deprojectreferencer.HintPaths
{
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