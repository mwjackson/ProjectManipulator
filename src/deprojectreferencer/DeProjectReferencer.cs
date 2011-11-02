using System.Linq;
using System.Xml;

namespace deprojectreferencer
{
    public class DeProjectReferencer
    {
        private const string MSBUILD_NAMESPACE = "http://schemas.microsoft.com/developer/msbuild/2003";

        private readonly IProjectReferenceExtractor _projectReferenceExtractor;
        private readonly IAssemblyReferenceConverter _assemblyReferenceConverter;
        private readonly IProjectReferenceDeleter _projectReferenceDeleter;

        public DeProjectReferencer(IProjectReferenceExtractor projectReferenceExtractor, IAssemblyReferenceConverter assemblyReferenceConverter, IProjectReferenceDeleter projectReferenceDeleter)
        {
            _projectReferenceExtractor = projectReferenceExtractor;
            _assemblyReferenceConverter = assemblyReferenceConverter;
            _projectReferenceDeleter = projectReferenceDeleter;
        }

        public void Dereference(string projectFilePath)
        {
            var projectFile = new XmlDocument();
            projectFile.Load(projectFilePath);

            var namespaceManager = new XmlNamespaceManager(projectFile.NameTable);
            namespaceManager.AddNamespace("msb", MSBUILD_NAMESPACE);

            var projectReferences = _projectReferenceExtractor.Extract(projectFile, namespaceManager);

            if (projectReferences.Count() == 0) return;

            _assemblyReferenceConverter.Convert(projectFile, namespaceManager, projectReferences);
            _projectReferenceDeleter.Delete(projectFile, namespaceManager);

            projectFile.Save(projectFilePath);
        }
    }
}