using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Security;

namespace AssemblyReader
{
    /// <summary>
    /// https://docs.microsoft.com/en-us/dotnet/api/system.reflection.assembly?view=netcore-2.1
    /// </summary>
    class Program
    {
        static void ShowUsage()
        {
            Console.WriteLine("Usage: version [options] [path]");

            Console.WriteLine("Options:");
            Console.WriteLine("\t --a | --assembly");
            Console.WriteLine("\t --p | --product");
            Console.WriteLine("\t --f | --file");
            Console.WriteLine("\t --all");

            Console.WriteLine("Path:");
            Console.WriteLine("\t The path to the assembly");
        }

        const int MAX_PATH = 256;
        static void Main(string[] args)
        {
            if (args.Length != 2)
            {
                ShowUsage();
                return;
            }

            var option = args[0];

            var assemblyPath = args[1];

            //TODO: validation
            //.exe and .dll
            if (string.IsNullOrWhiteSpace(assemblyPath))
            {
                Console.WriteLine($"Assembly path is missing");
                return;
            }

            try
            {
                var assembly = Assembly.LoadFrom(assemblyPath);
                //TODO: check this methods
                //var assembly = Assembly.LoadFile(assemblyPath);

                //var an = AssemblyName.GetAssemblyName(assemblyPath);
                //var assembly = Assembly.Load(an);

                if (assembly.FullName.Length > Program.MAX_PATH)
                {
                    Console.WriteLine($"Max length for namespace exceeded max value of {Program.MAX_PATH}");
                    return;
                }

                if (IsOptionAll(option) || IsOptionAssembly(option))
                {
                    string assemblyVersion = assembly.GetName().Version.ToString();
                    Console.WriteLine($"Assembly version: {assemblyVersion}");
                }

                if (IsOptionAll(option) || IsOptionProduct(option))
                {
                    var versionInfo = FileVersionInfo.GetVersionInfo(assembly.Location);
                    Console.WriteLine($"Product version: {versionInfo.ProductVersion}");
                }

                if (IsOptionAll(option) || IsOptionFile(option))
                {
                    var versionInfo = FileVersionInfo.GetVersionInfo(assembly.Location);
                    Console.WriteLine($"File version: {versionInfo.FileVersion}");
                }
            }
            catch (FileNotFoundException ex)
            {
                Debug.WriteLine(ex.GetType().FullName);
                Debug.WriteLine(ex.StackTrace);

                Console.WriteLine(ex.Message);
            }
            catch (FileLoadException ex)
            {
                Debug.WriteLine(ex.GetType().FullName);
                Debug.WriteLine(ex.StackTrace);

                Console.WriteLine("A file that was found could not be loaded.");
            }
            catch (BadImageFormatException ex)
            {
                Debug.WriteLine(ex.GetType().FullName);
                Debug.WriteLine(ex.StackTrace);

                Console.WriteLine(ex.Message);
            }
            catch (SecurityException ex)
            {
                Debug.WriteLine(ex.GetType().FullName);
                Debug.WriteLine(ex.StackTrace);

                Console.WriteLine(ex.Message);
            }
            //catch (PathTooLongException ex)
            //{
            //    Debug.WriteLine(ex.GetType().FullName);
            //    Debug.WriteLine(ex.Message);

            //    Console.WriteLine(ex.Message);
            //}

            Console.ReadKey(true);
        }

        private static bool IsOptionAssembly(string option)
        {
            var values = new string[] { "--assembly", "--a" };
            return Array.Exists(values, v => v == option);

            //return string.Equals(option, "--assembly", StringComparison.OrdinalIgnoreCase);
        }

        private static bool IsOptionProduct(string option)
        {
            var values = new string[] { "--product", "--p" };
            return Array.Exists(values, v => v == option);

            //return string.Equals(option, "--product", StringComparison.OrdinalIgnoreCase);
        }

        private static bool IsOptionFile(string option)
        {
            var values = new string[] { "--file", "--f" };
            return Array.Exists(values, v => v == option);

            //return string.Equals(option, "--file", StringComparison.OrdinalIgnoreCase);
        }

        private static bool IsOptionAll(string option)
        {
            var values = new string[] { "--all" };
            return Array.Exists(values, v => v == option);

            //return string.Equals(option, "--all", StringComparison.OrdinalIgnoreCase);
        }
    }
}
