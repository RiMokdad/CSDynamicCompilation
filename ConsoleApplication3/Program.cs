
using MyDynamiCompiler;

namespace CSDynamicCompilation
{
    using System;

    class Program
    {
        public static string DEBUG_PATH = "./";
        public static string COMMON_PATH = "../../Common/";
        static void Main(string[] args)
        {
            DynamiCompiler.ExecuteMethode("CSDynamicCompilation", "A.cs", COMMON_PATH, DEBUG_PATH, "StringTestA", new object[] { });
            Console.ReadLine();
        }
    }
}
