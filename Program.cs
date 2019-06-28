using System;
using colapsar_cs.models;
using System.Collections.Generic;
using System.Linq;

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

            var n1 = new Node(1, "n1");
            var n2 = new Node(2, "n2");
            var n3 = new Node(3, "n3");
            var n4 = new Node(4, "n4");

            var e1 = new Edge(n1, n2);
            var e2 = new Edge(n1, n3);
            var e3 = new Edge(n3, n1);
            var e4 = new Edge(n2, n4);

            ((List<Node>)n1.Neighbors()).ForEach(Console.WriteLine);
        }
    }
}
