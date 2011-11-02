using System.Collections.Generic;
using System.Xml;

namespace deprojectreferencer
{
    public interface IAssemblyReferenceConverter
    {
        void Convert(XmlDocument projectFile, XmlNamespaceManager namespaceManager, IEnumerable<ProjectReference> projectReferences);
    }

    public class AssemblyReferenceConverter : IAssemblyReferenceConverter
    {
        private readonly string _msbuildNamespace;

        public AssemblyReferenceConverter(string msbuildNamespace)
        {
            _msbuildNamespace = msbuildNamespace;
        }

        public void Convert(XmlDocument projectFile, XmlNamespaceManager namespaceManager, IEnumerable<ProjectReference> projectReferences)
        {
            XmlNode itemGroup;
            if ((itemGroup = projectFile.SelectSingleNode("/msb:Project/msb:ItemGroup[msb:Reference]", namespaceManager)) == null)
            {
                itemGroup = projectFile.CreateElement("ItemGroup", _msbuildNamespace);
                projectFile.DocumentElement.AppendChild(itemGroup);
            }

            foreach(var projectReference in projectReferences)
            {
                var newReference = CreateNewReference(projectFile, projectReference);
                itemGroup.AppendChild(newReference);
            }
        }

        private XmlElement CreateNewReference(XmlDocument projectFile, ProjectReference projectReference)
        {
            var hintPath = projectFile.CreateElement("HintPath", _msbuildNamespace);
            hintPath.InnerText = string.Format(@"..\..\..\build\{0}\{0}.dll", projectReference.Name);

            var newReference = projectFile.CreateElement("Reference", _msbuildNamespace);
            newReference.SetAttribute("Include", projectReference.Name);

            newReference.AppendChild(hintPath);
            return newReference;
        }
    }
}