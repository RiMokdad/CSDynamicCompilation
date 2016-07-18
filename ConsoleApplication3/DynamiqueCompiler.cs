using System;
using System.CodeDom.Compiler;

namespace MyDynamiCompiler
{
    public static class DynamiCompiler
    {
        /// <summary>
        /// Compiles the specified file to "AutoGen.dll" and displays the compilation errors if there are any.
        /// </summary>
        /// <param name="fileName"> The file to be compiled. </param>
        /// <param name="sourcePath"> The source path of the file. </param>
        /// <param name="targetPath"> The target path, by default it must be the Debug folder. </param>
        /// <returns> The result of the compilation. </returns>
        public static CompilerResults Compile(string fileName, string sourcePath, string targetPath)
        {
            CompilerParameters parameters = new CompilerParameters();
            parameters.GenerateExecutable = false;
            parameters.OutputAssembly = "AutoGen.dll";
            parameters.GenerateInMemory = true;

            UpdateDebugFolder(fileName, sourcePath, targetPath);

            var results = CodeDomProvider.CreateProvider("CSharp").CompileAssemblyFromFile(parameters, fileName);
            DisplayErrors(results);

            return results;
        }

        /// <summary>
        /// The compiled file must be in the Debug. 
        /// This method moves the updated file from the worksâce folder to the Debug folder.
        /// Hence we compile the most recent file each time.
        /// </summary>
        /// <param name="fileName"> The file to be compiled. </param>
        /// <param name="sourcePath"> The workspace folder. </param>
        /// <param name="targetPath"> The folder where the compilation takes place, by default it's the Debug folder. </param>
        public static void UpdateDebugFolder(string fileName, string sourcePath, string targetPath)
        {
            string sourceFile = System.IO.Path.Combine(sourcePath, fileName);
            string destFile = System.IO.Path.Combine(targetPath, fileName);

            System.IO.File.Copy(sourceFile, destFile, true);
        }

        public static void DisplayErrors(CompilerResults results)
        {
            string textBox = "";
            foreach (CompilerError CompErr in results.Errors)
            {
                textBox += "Line number " + CompErr.Line + ", Error Number: " + CompErr.ErrorNumber
                                + ", '" + CompErr.ErrorText + ";" + Environment.NewLine + Environment.NewLine;
            }
            Console.Write(textBox);
        }

        /// <summary>
        /// Compiles a file and executes a specific method in this file.
        /// </summary>
        /// <param name="fileNamespace"> The namespace or the name of the project containing the file. </param>
        /// <param name="fileName"> The file to be compiled. </param>
        /// <param name="sourcePath"> The workspace folder of the file. </param>
        /// <param name="targetPath"> The folder where the compilation takes place, by default it's the Debug folder. </param>
        /// <param name="methodName"> The method to be executed. </param>
        /// <param name="methodParameters"> The parameters of this method. If it doesn't have any, pass an empty array of object and NOT a null. </param>
        public static void ExecuteMethode(string fileNamespace, string fileName, string sourcePath, string targetPath, string methodName, object[] methodParameters)
        {
            var results = Compile(fileName, sourcePath, targetPath);

            //remove extension from fileName so that stringType looks like : fileNamespace.className 
            string stringType = fileNamespace + "." + fileName.Remove(fileName.Length - 3);
            var type = results.CompiledAssembly.GetType(stringType);
            var obj = Activator.CreateInstance(type);

            var stringOutput = type.GetMethod(methodName).Invoke(obj, methodParameters);
            Console.WriteLine(stringOutput);
        }
    }
}
