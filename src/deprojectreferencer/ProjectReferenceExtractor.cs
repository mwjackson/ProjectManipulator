using System.Collections.Generic;
using System.Linq;
using System.Xml;

namespace deprojectreferencer
{
    public class ProjectReferenceExtractor
    {
        public IEnumerable<ProjectReference> Extract(XmlDocument projectFile, XmlNamespaceManager namespaceManager)
        {
            return projectFile
                .SelectNodes("/msb:Project/msb:ItemGroup/msb:ProjectReference", namespaceManager)
                .Cast<XmlNode>()
                .Select(pr => new ProjectReference(pr.SelectSingleNode("msb:Name", namespaceManager).InnerText, pr.Attributes["Include"].Value))
                .ToList();
        }
    }
}