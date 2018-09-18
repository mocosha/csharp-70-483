using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Security;
/*
TODO:
    first param file path, second option with default (Company name, Assmbly version, File version) --all (+ product version...)

    use library for parsing arguments
    validate allowed arguments
    validate file type (.exe and .dll)

    target framework
    company name

    return error codes
    branding

    read assembly informations and put to dict
 */

namespace AssemblyReader
{
    /// <summary>
    /// read assembly - https://docs.microsoft.com/en-us/dotnet/api/system.reflection.assembly?view=netcore-2.1
    /// global tool - https://docs.microsoft.com/en-us/dotnet/core/tools/global-tools-how-to-create
    /// </summary>
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length != 2)
            {
                ShowUsage();
                return;
            }

            var option = args[0];

            var assemblyPath = args[1];

            if (string.IsNullOrWhiteSpace(assemblyPath))
            {
                Console.WriteLine($"Assembly path is missing");
                return;
            }

            try
            {
                var assembly = Assembly.LoadFrom(assemblyPath);

                //TODO: check these methods
                //var assembly = Assembly.LoadFile(assemblyPath);

                //var an = AssemblyName.GetAssemblyName(assemblyPath);
                //var assembly = Assembly.Load(an);

                if (assembly.FullName.Length > Program.MAX_PATH)
                {
                    Console.WriteLine($"Max length for namespace exceeded max value of {Program.MAX_PATH}");
                    return;
                }

                Console.WriteLine($"Full name: {assembly.FullName}");

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
        }

        const int MAX_PATH = 256;

        private static void ShowUsage()
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

        private static bool IsOptionAssembly(string option)
        {
            var values = new string[] { "--assembly", "--a" };
            return Array.Exists(values, v => v == option);
        }

        private static bool IsOptionProduct(string option)
        {
            var values = new string[] { "--product", "--p" };
            return Array.Exists(values, v => v == option);
        }

        private static bool IsOptionFile(string option)
        {
            var values = new string[] { "--file", "--f" };
            return Array.Exists(values, v => v == option);
        }

        private static bool IsOptionAll(string option)
        {
            var values = new string[] { "--all" };
            return Array.Exists(values, v => v == option);
        }
    }
}
