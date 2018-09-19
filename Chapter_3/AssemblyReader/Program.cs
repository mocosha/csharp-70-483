using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Security;

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
            var argsLen = args.Length;
            if (argsLen > 2)
            {
                ShowUsage();
                return;
            }

            var assemblyPath = args[0];
            var option = argsLen == 2 ? args[1] : "";

            if (string.IsNullOrWhiteSpace(assemblyPath))
            {
                Console.WriteLine($"Assembly path is missing");
                return;
            }

            try
            {
                Dictionary<string, string> infos = ReadAssemblyInfos(assemblyPath);

                switch (option)
                {
                    case "--all":
                        foreach (var item in infos)
                        {
                            Console.WriteLine($"{item.Key}: {item.Value}");
                        }
                        break;
                    case "--file":
                    case "--f":
                        Console.WriteLine($"FileVersion: {infos["FileVersion"]}");
                        break;
                    case "--product":
                    case "--p":
                        Console.WriteLine($"ProductVersion: {infos["ProductVersion"]}");
                        break;
                    case "--assembly":
                    case "--a":
                    default:
                        Console.WriteLine($"AssemblyVersion: {infos["AssemblyVersion"]}");
                        break;
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
            catch (PathTooLongException ex) //TODO: maybe this can be validate
            {
                Debug.WriteLine(ex.GetType().FullName);
                Debug.WriteLine(ex.Message);

                Console.WriteLine(ex.Message);
            }
        }

        private static Dictionary<string, string> ReadAssemblyInfos(string assemblyPath)
        {
            var assembly = Assembly.LoadFrom(assemblyPath);
            var versionInfo = FileVersionInfo.GetVersionInfo(assembly.Location);

            var assemblyInfos = new Dictionary<string, string>
                {
                    { "FullName", assembly.FullName },
                    { "TargetFramework", assembly.ImageRuntimeVersion},
                    { "CompanyName", versionInfo.CompanyName },
                    { "AssemblyVersion", assembly.GetName().Version.ToString() },
                    { "ProductVersion", versionInfo.ProductVersion },
                    { "FileVersion", versionInfo.FileVersion }
                };
            return assemblyInfos;
        }

        const int MAX_PATH = 256;

        private static void ShowUsage()
        {
            Console.WriteLine("Usage: version [path] [options]");

            Console.WriteLine("Path:");
            Console.WriteLine("\t The path to the assembly. Required.");

            Console.WriteLine("Options:");
            Console.WriteLine("\t Display different assembly information. Optional. Default is assembly version.");
            //Console.WriteLine("\t --a | --assembly");
            Console.WriteLine("\t --p | --product");
            Console.WriteLine("\t --f | --file");
            Console.WriteLine("\t --all");
        }
    }
}
