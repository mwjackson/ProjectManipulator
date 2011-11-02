using System.Xml;

namespace deprojectreferencer
{
    public class ProjectReferenceDeleter
    {
        public void Delete(XmlDocument projectFile, XmlNamespaceManager namespaceManager)
        {
            var projectReferenceItemGroup = projectFile.SelectSingleNode("/msb:Project/msb:ItemGroup[msb:ProjectReference]", namespaceManager);
            if (projectReferenceItemGroup == null) return;
            projectReferenceItemGroup.ParentNode.RemoveChild(projectReferenceItemGroup);
        }
    }
}