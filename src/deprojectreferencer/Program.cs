using System;
using System.Xml;

namespace deprojectreferencer
{
    class Program
    {
        private const string MSBUILD_NAMESPACE = "http://schemas.microsoft.com/developer/msbuild/2003";
        
        static void Main(string[] args)
        {
            try
            {
                //CreateDeProjectReferencer().Dereference(args[0]);
                var projectPath = args[0];
                var projectFile = new XmlDocument();
                projectFile.Load(projectPath);
                new CopyLocalManipulator(MSBUILD_NAMESPACE).SetFalse(projectFile);
                projectFile.Save(projectPath);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        private static DeProjectReferencer CreateDeProjectReferencer()
        {
            return new DeProjectReferencer(new ProjectReferenceExtractor(), new AssemblyReferenceConverter(MSBUILD_NAMESPACE), new ProjectReferenceDeleter());
        }
    }
}
