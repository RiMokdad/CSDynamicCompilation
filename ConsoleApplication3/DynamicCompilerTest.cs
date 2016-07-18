using System;
using System.CodeDom.Compiler;

namespace MyDynamiCompiler
{
    public static class DynamicCompilerTest
    {
        public static string COMMON_PATH = "../../Common/";
        public static void CompileCode()
        {
            CompilerParameters parameters = new CompilerParameters();
            parameters.GenerateExecutable = false;
            parameters.OutputAssembly = "AutoGen.dll";
            parameters.GenerateInMemory = true;

            //update Debug Folder
            UpdateDebugFolder("B.cs", COMMON_PATH);
            UpdateDebugFolder("C.cs", COMMON_PATH);
            UpdateDebugFolder("A.cs", COMMON_PATH);

            //Compile
            var results = CodeDomProvider.CreateProvider("CSharp").CompileAssemblyFromFile(parameters, "B.cs");
            var results2 = CodeDomProvider.CreateProvider("CSharp").CompileAssemblyFromFile(parameters, "C.cs");
            var results3 = CodeDomProvider.CreateProvider("CSharp").CompileAssemblyFromFile(parameters, "A.cs");

            DisplayErrors(results);
            DisplayErrors(results2);
            DisplayErrors(results3);

            //Run assembly : 
            var type = results.CompiledAssembly.GetType("CSDynamicCompilation.B");
            var type2 = results2.CompiledAssembly.GetType("CSDynamicCompilation.C");
            var type3 = results3.CompiledAssembly.GetType("CSDynamicCompilation.A");
            var obj = Activator.CreateInstance(type);
            var obj2 = Activator.CreateInstance(type2);
            var obj3 = Activator.CreateInstance(type3);

            //Display and verify results
            var stringOutput = type.GetMethod("StringTest").Invoke(obj, new object[] { });
            Console.WriteLine(stringOutput);
            object[] parameter = new object[] { 2 };
            var voidOutput = type.GetMethod("VoidTest").Invoke(obj, parameter);
            Console.WriteLine(voidOutput);
            var intOutput = type2.GetMethod("IntTest").Invoke(obj2, new object[] { });
            Console.WriteLine(intOutput);
            var stringOutputA = type3.GetMethod("StringTestA").Invoke(obj3, new object[] { });
            Console.WriteLine(stringOutputA);
        }

        public static void DisplayErrors(CompilerResults results)
        {
            if (results.Errors.Count > 0)
            {
                string textBox = "";
                foreach (CompilerError CompErr in results.Errors)
                {
                    textBox += "Line number " + CompErr.Line + ", Error Number: " + CompErr.ErrorNumber
                                    + ", '" + CompErr.ErrorText + ";" + Environment.NewLine + Environment.NewLine;
                }
                Console.WriteLine(textBox);
            }
        }

        public static void UpdateDebugFolder(string fileName, string sourcePath, string targetPath = "./")
        {
            // Use Path class to manipulate file and directory paths.
            string sourceFile = System.IO.Path.Combine(sourcePath, fileName);
            string destFile = System.IO.Path.Combine(targetPath, fileName);

            // To copy a file to another location and 
            // overwrite the destination file if it already exists.
            System.IO.File.Copy(sourceFile, destFile, true);
        }
    }
}
 

