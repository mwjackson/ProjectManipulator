using System;
using System.Xml;
using deprojectreferencer.CopyLocal;
using deprojectreferencer.HintPaths;
using deprojectreferencer.ProjectReferences;

namespace deprojectreferencer
{
    internal class ProjectManipulator
    {
        private const string MSBUILD_NAMESPACE = "http://schemas.microsoft.com/developer/msbuild/2003";

        public void Go(params string[] args)
        {
            var projectPath = args[1];
            var projectFile = new XmlDocument();
            projectFile.Load(projectPath);
            switch(args[0])
            {
                case "/p":
                    {
                        CreateDeProjectReferencer().Dereference(projectPath); 
                        break;
                    }
                case "/cl":
                    {
                        new CopyLocalManipulator(MSBUILD_NAMESPACE).SetFalse(projectFile);
                        projectFile.Save(projectPath); 
                        break;
                    }
                case "/hp":
                    {
                        new HintPathUpdater(MSBUILD_NAMESPACE, new HintPathLookup()).Update(projectFile);
                        break;
                    }
                default:
                    {
                        throw new ArgumentException(string.Format("Switch not recognised: {0}", args[0]));
                    }
            }
        }

        private static DeProjectReferencer CreateDeProjectReferencer()
        {
            return new DeProjectReferencer(new ProjectReferenceExtractor(), new AssemblyReferenceConverter(MSBUILD_NAMESPACE), new ProjectReferenceDeleter());
        }
    }
}