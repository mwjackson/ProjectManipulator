using System.Xml;

namespace deprojectreferencer.ProjectReferences
{
    public interface IProjectReferenceDeleter
    {
        void Delete(XmlDocument projectFile, XmlNamespaceManager namespaceManager);
    }

    public class ProjectReferenceDeleter : IProjectReferenceDeleter
    {
        public void Delete(XmlDocument projectFile, XmlNamespaceManager namespaceManager)
        {
            var projectReferenceItemGroup = projectFile.SelectSingleNode("/msb:Project/msb:ItemGroup[msb:ProjectReference]", namespaceManager);
            if (projectReferenceItemGroup == null) return;
            projectReferenceItemGroup.ParentNode.RemoveChild(projectReferenceItemGroup);
        }
    }
}