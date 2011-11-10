using System.Collections.Generic;
using System.Linq;
using System.Xml;

namespace deprojectreferencer
{
    public class CopyLocalManipulator
    {
        private readonly XmlNamespaceManager _namespaceManager;

        public CopyLocalManipulator(XmlNamespaceManager namespaceManager)
        {
            _namespaceManager = namespaceManager;
        }

        public void SetFalse(XmlDocument projectFile, IEnumerable<XmlNode> references)
        {
            var x = projectFile.NamespaceURI;
            foreach (var reference in references)
            {
                var privateNodes = reference.SelectNodes("Private", _namespaceManager).Cast<XmlNode>();
                if (privateNodes.Any())
                {
                    Toggle(privateNodes.First());
                    return;
                }

                var privateNode = projectFile.CreateElement("Private", _namespaceManager.DefaultNamespace);
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