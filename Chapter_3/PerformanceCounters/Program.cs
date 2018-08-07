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

    private static PerformanceCounter GetPerformanceCounter(string type, string instance)
    {
        var isInstance = !string.IsNullOrWhiteSpace(instance);

        if (isInstance)
        {
            if (string.Equals(type, CPU, StringComparison.OrdinalIgnoreCase))
            {
                return new PerformanceCounter("Process", "% Processor Time", instance, true);
            }
            else if (string.Equals(type, MEMORY, StringComparison.OrdinalIgnoreCase))
            {
                return new PerformanceCounter("Process", "Working Set - Private", instance, true);
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
        Console.WriteLine("   BaseValue        = " + s.BaseValue);
        Console.WriteLine("   CounterFrequency = " + s.CounterFrequency);
        Console.WriteLine("   CounterTimeStamp = " + s.CounterTimeStamp);
        Console.WriteLine("   CounterType      = " + s.CounterType);
        Console.WriteLine("   RawValue         = " + s.RawValue);
        Console.WriteLine("   SystemFrequency  = " + s.SystemFrequency);
        Console.WriteLine("   TimeStamp        = " + s.TimeStamp);
        Console.WriteLine("   TimeStamp100nSec = " + s.TimeStamp100nSec);
        Console.WriteLine(new string('+', 10));
    }
}