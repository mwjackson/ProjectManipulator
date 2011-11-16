using System;
using System.Configuration;
using System.Linq;
using System.Xml;

namespace ProjectManipulator.HintPaths
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

        public XmlDocument Update(string projectPath)
        {
            var projectFile = new XmlDocument();
            projectFile.Load(projectPath);

            var namespaceManager = new XmlNamespaceManager(projectFile.NameTable);
            namespaceManager.AddNamespace("msb", _msbuildNamespace);

            var hintPaths = projectFile.SelectNodes("/msb:Project/msb:ItemGroup/msb:Reference/msb:HintPath", namespaceManager).Cast<XmlNode>();

            foreach(var hintPath in hintPaths)
            {
                var oldPath = hintPath.InnerText;
                if (string.IsNullOrEmpty(oldPath)) continue;
                if (oldPath.Contains(@"..\..\..\lib")) continue;

                var newPath = _hintPathLookup.For(oldPath, projectPath);
                hintPath.InnerText = newPath;

                Log(oldPath, newPath);
            }

            projectFile.Save(projectPath);
            return projectFile;
        }

        private void Log(string oldPath, string newPath)
        {
            if (Convert.ToBoolean(ConfigurationManager.AppSettings["debug"]))
            {
                Console.WriteLine("OldPath: {0}", oldPath);
                Console.WriteLine("NewPath: {0}", newPath);
            }
        }
    }
}