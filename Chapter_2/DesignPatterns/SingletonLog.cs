using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesignPatterns
{
    /// <summary>
    /// C# in Depth by Jon Skeet
    /// Implementing the Singleton Pattern in C#
    /// http://csharpindepth.com/Articles/General/Singleton.aspx
    /// Fourth version - not quite as lazy, but thread-safe without using locks
    /// 
    ///  As you can see, this is really is extremely simple - but why is it thread-safe and how lazy is it? Well, static constructors in C# are specified to execute only when an instance of the class is created or a static member is referenced, and to execute only once per AppDomain. Given that this check for the type being newly constructed needs to be executed whatever else happens, it will be faster than adding extra checking as in the previous examples. There are a couple of wrinkles, however:
    ///  - It's not as lazy as the other implementations. In particular, if you have static members other than Instance, the first reference to those members will involve creating the instance. This is corrected in the next implementation.
    ///  - There are complications if one static constructor invokes another which invokes the first again.Look in the.NET specifications (currently section 9.5.3 of partition II) for more details about the exact nature of type initializers - they're unlikely to bite you, but it's worth being aware of the consequences of static constructors which refer to each other in a cycle.
    ///  - The laziness of type initializers is only guaranteed by.NET when the type isn't marked with a special flag called beforefieldinit. Unfortunately, the C# compiler (as provided in the .NET 1.1 runtime, at least) marks all types which don't have a static constructor(i.e.a block which looks like a constructor but is marked static) as beforefieldinit.I now have an article with more details about this issue.Also note that it affects performance, as discussed near the bottom of the page.
    /// </summary>
    public sealed class Singleton
    {
        private static readonly Singleton logger = new Singleton();

        // Explicit static constructor to tell C# compiler
        // not to mark type as beforefieldinit
        static Singleton()
        {
        }

        private Singleton()
        {
        }

        public void Log(string message)
        {
            System.Diagnostics.Trace.TraceInformation(message);
        }

        public readonly static Singleton Instance = logger;
    }
}
