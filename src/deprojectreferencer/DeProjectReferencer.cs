using System.Linq;
using System.Xml;

namespace deprojectreferencer
{
    public class DeProjectReferencer
    {
        private const string MSBUILD_NAMESPACE = "http://schemas.microsoft.com/developer/msbuild/2003";
        
        private readonly string _projectFilePath;
        
        private readonly ProjectReferenceExtractor _projectReferenceExtractor;
        private readonly AssemblyReferenceConverter _assemblyReferenceConverter;
        private readonly ProjectReferenceDeleter _projectReferenceDeleter;

        public DeProjectReferencer(string projectFilePath)
        {
            _projectFilePath = projectFilePath;
            _projectReferenceExtractor = new ProjectReferenceExtractor();
            _assemblyReferenceConverter = new AssemblyReferenceConverter(MSBUILD_NAMESPACE);
            _projectReferenceDeleter = new ProjectReferenceDeleter();
        }

        public void Dereference()
        {
            var projectFile = new XmlDocument();
            projectFile.Load(_projectFilePath);

            var namespaceManager = new XmlNamespaceManager(projectFile.NameTable);
            namespaceManager.AddNamespace("msb", MSBUILD_NAMESPACE);

            var projectReferences = _projectReferenceExtractor.Extract(projectFile, namespaceManager);

            if (projectReferences.Count() == 0) return;

            _assemblyReferenceConverter.Convert(projectFile, namespaceManager, projectReferences);
            _projectReferenceDeleter.Delete(projectFile, namespaceManager);

            projectFile.Save(_projectFilePath);
        }
    }
}