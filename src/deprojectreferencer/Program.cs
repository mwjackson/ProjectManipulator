using System;

namespace deprojectreferencer
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                new DeProjectReferencer(args[0]).Dereference();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
    }
}
