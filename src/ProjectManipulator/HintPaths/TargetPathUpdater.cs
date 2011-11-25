using System;
using System.Configuration;
using System.Linq;
using System.Xml;

namespace ProjectManipulator.HintPaths
{
    public class TargetPathUpdater
    {
        private readonly string _msbuildNamespace;

        public TargetPathUpdater(string msbuildNamespace)
        {
            _msbuildNamespace = msbuildNamespace;
        }

        public XmlDocument Update(string projectPath)
        {
            var projectFile = new XmlDocument();
            projectFile.Load(projectPath);

            var namespaceManager = new XmlNamespaceManager(projectFile.NameTable);
            namespaceManager.AddNamespace("msb", _msbuildNamespace);

            var projectTargets = projectFile.SelectNodes("/msb:Project/msb:Import", namespaceManager).Cast<XmlNode>();

            foreach (var projectTarget in projectTargets)
            {
                var oldPath = projectTarget.Attributes["Project"].Value;
                if (string.IsNullOrEmpty(oldPath)) continue;
                if (!oldPath.ToLower().Contains(@"integration.testing.target")) continue;

                string newPath;
                if (oldPath.ToLower().Contains(@"..\..\..\..\build"))
                    newPath = @"$(ProjectDir)..\..\..\..\buildsolutions\Wonga.Ops\Integration.Testing.target";
                else
                    newPath = @"$(ProjectDir)..\..\..\buildsolutions\Wonga.Ops\Integration.Testing.target";

                projectTarget.Attributes["Project"].Value = newPath;

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