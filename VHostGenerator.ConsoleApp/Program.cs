using System;
using System.Reflection;

namespace VHostGenerator.ConsoleApp
{
    internal static class Program
    {
        private static void Main(string[] args)
        {
            Console.WriteLine("+----------------------------------------+");
            Console.WriteLine("+            V-HOST GENERATOR            +");
            Console.WriteLine($"+                 v{Assembly.GetEntryAssembly().GetName().Version.ToString(3)}                 +");
            Console.WriteLine("+            by Share IT Code            +");
            Console.WriteLine("+               MIT Licence              +");
            Console.WriteLine("+----------------------------------------+");
        }
    }
}