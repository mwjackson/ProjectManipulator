using System;
using System.IO;

namespace deprojectreferencer
{
    class Program
    {

        static int Main(string[] args)
        {
            try
            {
                new ProjectManipulator().Go(args);
                return 0;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                Console.WriteLine();
                PrintUsage();
                return 1;
            }
        }

        private static void PrintUsage()
        {
            Console.WriteLine(File.ReadAllText("readme.txt"));
        }
    }
}
