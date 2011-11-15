using System;
using System.IO;
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
            if (!File.Exists(args[1])) ThrowArgumentException(args);

            var projectPath = args[1];
            var projectFile = new XmlDocument();
            projectFile.Load(projectPath);
            Console.WriteLine(projectPath);
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
                    new HintPathUpdater(MSBUILD_NAMESPACE, new HintPathLookup()).Update(projectPath);
                    break;
                }
                default:
                {
                    ThrowArgumentException(args);
                    break;
                }
            }
        }

        private static void ThrowArgumentException(string[] args)
        {
            throw new ArgumentException(string.Format("Arguments not recognised: {0}", string.Join(" ", args)));
        }

        private static DeProjectReferencer CreateDeProjectReferencer()
        {
            return new DeProjectReferencer(new ProjectReferenceExtractor(), new AssemblyReferenceConverter(MSBUILD_NAMESPACE), new ProjectReferenceDeleter());
        }
    }
}