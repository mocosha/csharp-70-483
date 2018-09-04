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
        static bool IsOptionAssembly(string option)
        {
            return string.Equals(option, "assembly", StringComparison.OrdinalIgnoreCase);
        }

        static bool IsOptionProduct(string option)
        {
            return string.Equals(option, "product", StringComparison.OrdinalIgnoreCase);
        }

        static bool IsOptionFile(string option)
        {
            return string.Equals(option, "file", StringComparison.OrdinalIgnoreCase);
        }

        static bool IsOptionAll(string option)
        {
            return string.Equals(option, "all", StringComparison.OrdinalIgnoreCase);
        }

        const int MAX_PATH = 256;
        static void Main(string[] args)
        {
            if (args.Length != 2)
            {
                Console.WriteLine("Usage: version [options] [path]");

                Console.WriteLine("Options:");
                Console.WriteLine("\t assembly");
                Console.WriteLine("\t product");
                Console.WriteLine("\t file");
                Console.WriteLine("\t all");

                Console.WriteLine("Path:");
                Console.WriteLine("\t The path to the assembly");

                return;
            }

            var option = args[0];

            var assemblyPath = args[1];

            //TODO: validation
            //.exe and .dll
            if (string.IsNullOrWhiteSpace(assemblyPath))
            {

            }

            try
            {
                var assembly = Assembly.LoadFrom(assemblyPath);

                if (assembly.FullName.Length > Program.MAX_PATH)
                {
                    Console.WriteLine("");
                    return;
                }

                //TODO: check this methods
                //var assembly = Assembly.LoadFile(assemblyPath);

                //var an = AssemblyName.GetAssemblyName(assemblyPath);
                //var assembly = Assembly.Load(an);

                if (IsOptionAll(option) || IsOptionAssembly(option))
                {
                    string assemblyVersion = assembly.GetName().Version.ToString();
                    Console.WriteLine($"Assembly version: {assemblyVersion}");
                }
                else if (IsOptionAll(option) || IsOptionProduct(option))
                {
                    string productVersion = FileVersionInfo.GetVersionInfo(assembly.Location).ProductVersion;
                    Console.WriteLine($"Product version: {productVersion}");
                }
                else if (IsOptionAll(option) || IsOptionFile(option))
                {
                    string fileVersion = FileVersionInfo.GetVersionInfo(assembly.Location).FileVersion;
                    Console.WriteLine($"File version: {fileVersion}");
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

#if DEBUG
            Console.ReadKey(true);
#endif

        }
    }
}
