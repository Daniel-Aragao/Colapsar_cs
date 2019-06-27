using System;

namespace colapsar_cs
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("The current time is " + DateTime.Now);
            Console.WriteLine("Hello World!");
            var o1 = new Object();
            var o2 = new Object();
            Console.WriteLine(o1.GetHashCode());
            Console.WriteLine(o2.GetHashCode());
        }
    }
}
