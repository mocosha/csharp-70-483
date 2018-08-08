using System;
using System.Diagnostics;
using System.Threading;

public class Program
{
    private readonly static string CPU = "cpu";
    private readonly static string MEMORY = "memory";

    public static void Main(string[] args)
    {
        var type = args[0];
        string instanceName = null;
        if (args.Length == 2)
        {
            instanceName = args[1];
        }

        var pc = GetPerformanceCounter(type, instanceName);
        ReadValuesOfPerformanceCounters(pc);
    }

    private static PerformanceCounter GetPerformanceCounter(string type, string instanceName)
    {
        var isInstance = !string.IsNullOrWhiteSpace(instanceName);

        if (isInstance)
        {
            if (string.Equals(type, CPU, StringComparison.OrdinalIgnoreCase))
            {
                return new PerformanceCounter("Process", "% Processor Time", instanceName, true);
            }
            else if (string.Equals(type, MEMORY, StringComparison.OrdinalIgnoreCase))
            {
                return new PerformanceCounter("Process", "Working Set - Private", instanceName, true);
            }
            else
            {
                throw new ArgumentException("Type not supported");
            }
        }
        else
        {
            if (string.Equals(type, CPU, StringComparison.OrdinalIgnoreCase))
            {
                return new PerformanceCounter("Processor", "% Processor Time", "_Total");
            }
            else if (string.Equals(type, MEMORY, StringComparison.OrdinalIgnoreCase))
            {
                return new PerformanceCounter("Memory", "Available MBytes");
            }
            else
            {
                throw new ArgumentException("Type not supported");
            }
        }
    }

    private static void ReadValuesOfPerformanceCounters(PerformanceCounter counter)
    {
        Console.WriteLine("Category: {0}", counter.CategoryName);
        Console.WriteLine("Instance: {0}", counter.InstanceName);
        Console.WriteLine("Counter name: {0}", counter.CounterName);

        Console.WriteLine(new string('-', 15));
        while (true)
        {
            Console.WriteLine("Value: {0}", counter.NextValue());
            //Console.WriteLine("Sample");
            //OutputSample(counter.NextSample());
            Thread.Sleep(2000);
        }
    }

    private static void OutputSample(CounterSample s)
    {
        Console.WriteLine(new string('+', 10));
        Console.WriteLine("Sample values - \r\n");
        Console.WriteLine("\tBaseValue        = " + s.BaseValue);
        Console.WriteLine("\tCounterFrequency = " + s.CounterFrequency);
        Console.WriteLine("\tCounterTimeStamp = " + s.CounterTimeStamp);
        Console.WriteLine("\tCounterType      = " + s.CounterType);
        Console.WriteLine("\tRawValue         = " + s.RawValue);
        Console.WriteLine("\tSystemFrequency  = " + s.SystemFrequency);
        Console.WriteLine("\tTimeStamp        = " + s.TimeStamp);
        Console.WriteLine("\tTimeStamp100nSec = " + s.TimeStamp100nSec);
        Console.WriteLine(new string('+', 10));
    }
}