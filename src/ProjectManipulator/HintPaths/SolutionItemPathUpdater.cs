using System;
using System.Configuration;
using System.Linq;
using System.Xml;

namespace ProjectManipulator.HintPaths
{
    public class SolutionItemPathUpdater
    {
        private readonly string _msbuildNamespace;

        public SolutionItemPathUpdater(string msbuildNamespace)
        {
            _msbuildNamespace = msbuildNamespace;
        }

        public XmlDocument Update(string projectPath)
        {
            var projectFile = new XmlDocument();
            projectFile.Load(projectPath);

            var namespaceManager = new XmlNamespaceManager(projectFile.NameTable);
            namespaceManager.AddNamespace("msb", _msbuildNamespace);

            var solutionItems = projectFile.SelectNodes("/msb:Project/msb:ItemGroup/msb:None", namespaceManager).Cast<XmlNode>();

            foreach (var solutionItem in solutionItems)
            {
                var oldPath = solutionItem.Attributes["Include"].Value;
                if (string.IsNullOrEmpty(oldPath)) continue;
                if (!oldPath.ToLower().Contains(@"..\..\..\build\wonga.api\wonga.api.xsd")) continue;

                var newPath = @"..\..\..\buildsolutions\Wonga.Common\Wonga.Api.xsd";
                solutionItem.Attributes["Include"].Value = newPath;

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