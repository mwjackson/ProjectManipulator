using System.Collections.Generic;
using System.Linq;
using System.Xml;

namespace ProjectManipulator.CopyLocal
{
    public class CopyLocalManipulator
    {
        private readonly string _msBuildNamespace;

        public CopyLocalManipulator(string msBuildNamespace)
        {
            _msBuildNamespace = msBuildNamespace;
        }

        public void SetFalse(XmlDocument projectFile)
        {
            var namespaceManager = new XmlNamespaceManager(projectFile.NameTable);
            namespaceManager.AddNamespace("msb", _msBuildNamespace);

            var assemblyReferences = projectFile.SelectNodes("/msb:Project/msb:ItemGroup/msb:Reference", namespaceManager).Cast<XmlNode>();
            var projectReferences = projectFile.SelectNodes("/msb:Project/msb:ItemGroup/msb:ProjectReference", namespaceManager).Cast<XmlNode>();

            var allReferences = new List<XmlNode>();
            allReferences.AddRange(assemblyReferences);
            allReferences.AddRange(projectReferences);

            foreach (var reference in allReferences)
            {
                var privateNodes = reference.SelectNodes("msb:Private", namespaceManager).Cast<XmlNode>();
                if (privateNodes.Any())
                {
                    Toggle(privateNodes.First());
                    continue;
                }

                var privateNode = projectFile.CreateElement("Private", _msBuildNamespace);
                privateNode.InnerText = "False";
                reference.AppendChild(privateNode);
            }
        }

        private void Toggle(XmlNode privateNode)
        {
            privateNode.InnerText = "False";
        }
    }
}