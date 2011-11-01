using System.Collections.Generic;
using System.Linq;
using System.Xml;

namespace deprojectreferencer
{
    internal class DeProjectReferencer
    {
        private const string MSBUILD_NAMESPACE = "http://schemas.microsoft.com/developer/msbuild/2003";
        
        private readonly string _projectFilePath;

        public DeProjectReferencer(string projectFilePath)
        {
            _projectFilePath = projectFilePath;
        }

        public void Dereference()
        {
            var projectFile = new XmlDocument();
            projectFile.Load(_projectFilePath);

            var namespaceManager = new XmlNamespaceManager(projectFile.NameTable);
            namespaceManager.AddNamespace("msb", MSBUILD_NAMESPACE);

            var projectReferences = ExtractProjectReferences(projectFile, namespaceManager);

            if (projectReferences.Count() == 0) return;

            ConvertToReferences(projectReferences, namespaceManager, projectFile);

            DeleteProjectReferences(namespaceManager, projectFile);

            SaveToFile(projectFile);
        }

        private void SaveToFile(XmlDocument projectFile)
        {
            projectFile.Save(_projectFilePath);
        }

        private void DeleteProjectReferences(XmlNamespaceManager namespaceManager, XmlDocument projectFile)
        {
            var projectReferenceItemGroup = projectFile.SelectSingleNode("/msb:Project/msb:ItemGroup[msb:ProjectReference]", namespaceManager);
            projectReferenceItemGroup.ParentNode.RemoveChild(projectReferenceItemGroup);
        }

        private void ConvertToReferences(IEnumerable<ProjectReference> projectReferences, XmlNamespaceManager namespaceManager, XmlDocument projectFile)
        {
            var referenceItemGroup = projectFile.SelectSingleNode("/msb:Project/msb:ItemGroup[msb:Reference]", namespaceManager);
            if (referenceItemGroup == null) referenceItemGroup = CreateReferenceItemGroup(projectFile);
            foreach (var projectReference in projectReferences)
            {
                var newReference = CreateNewReference(projectFile, projectReference);
                referenceItemGroup.AppendChild(newReference);
            }
        }

        private XmlElement CreateReferenceItemGroup(XmlDocument projectFile)
        {
            var itemGroup = projectFile.CreateElement("ItemGroup", MSBUILD_NAMESPACE);
            projectFile.DocumentElement.AppendChild(itemGroup);
            return itemGroup;
        }

        private IEnumerable<ProjectReference> ExtractProjectReferences(XmlDocument projectFile, XmlNamespaceManager namespaceManager)
        {
            var projectReferenceNodes = projectFile
                .SelectNodes("/msb:Project/msb:ItemGroup/msb:ProjectReference", namespaceManager)
                .Cast<XmlNode>()
                .ToArray();

            var projectReferences = projectReferenceNodes
                .Select(pr => new ProjectReference(pr.SelectSingleNode("msb:Name", namespaceManager).InnerText, pr.Attributes["Include"].Value))
                .ToList();
            return projectReferences;
        }

        private XmlElement CreateNewReference(XmlDocument projectFile, ProjectReference projectReference)
        {
            var hintPath = projectFile.CreateElement("HintPath", MSBUILD_NAMESPACE);
            hintPath.InnerText = string.Format(@"..\..\..\build\{0}\{0}.dll", projectReference.Name);

            var newReference = projectFile.CreateElement("Reference", MSBUILD_NAMESPACE);
            newReference.SetAttribute("Include", projectReference.Name);
            
            newReference.AppendChild(hintPath);
            return newReference;
        }
    }
}