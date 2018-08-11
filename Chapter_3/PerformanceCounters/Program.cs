using System;
using System.Diagnostics;
using System.Threading;

class Type
{
    public Type(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentNullException(nameof(name));

        _name = name;
    }

    public bool IsCpu =>
        string.Equals(_name, CPU, StringComparison.OrdinalIgnoreCase);

    public bool IsMemory =>
        string.Equals(_name, MEMORY, StringComparison.OrdinalIgnoreCase);

    private string _name;
    private readonly static string CPU = "cpu";
    private readonly static string MEMORY = "memory";
}

public class Program
{
    public static void Main(string[] args)
    {
        var type = args[0];
        string instanceName = null;
        if (args.Length == 2)
        {
            instanceName = args[1];
        }

        var counter = GetPerformanceCounter(type, instanceName);
        ReadValuesOfPerformanceCounters(counter);
    }

    private static PerformanceCounter GetPerformanceCounter(string type, string instanceName)
    {
        var instanceNameExists = !string.IsNullOrWhiteSpace(instanceName);
        var typeObj = new Type(type);

        if (instanceNameExists)
        {
            if (typeObj.IsCpu)
            {
                return new PerformanceCounter("Process", "% Processor Time", instanceName, true);
            }
            else if (typeObj.IsMemory)
            {
                //Working Set - Private
                //amount of memory used by a process that cannot be shared among other processes
                return new PerformanceCounter("Process", "Working Set - Private", instanceName, true);
            }
            else
            {
                throw new ArgumentException("Type not supported");
            }
        }
        else
        {
            if (typeObj.IsCpu)
            {
                return new PerformanceCounter("Processor", "% Processor Time", "_Total");
            }
            else if (typeObj.IsMemory)
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
        Console.WriteLine($"Category: {counter.CategoryName}");
        Console.WriteLine($"Instance: {counter.InstanceName}");
        Console.WriteLine($"Counter name: {counter.CounterName}");

        Console.WriteLine(new string('-', 15));
        while (true)
        {
            Console.WriteLine($"Value: {counter.NextValue()}");
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