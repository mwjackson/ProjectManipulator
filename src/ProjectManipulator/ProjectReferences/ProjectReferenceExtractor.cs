using System.Collections.Generic;
using System.Linq;
using System.Xml;

namespace ProjectManipulator.ProjectReferences
{
    public interface IProjectReferenceExtractor
    {
        IEnumerable<ProjectReference> Extract(XmlDocument projectFile, XmlNamespaceManager namespaceManager);
    }

    public class ProjectReferenceExtractor : IProjectReferenceExtractor
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