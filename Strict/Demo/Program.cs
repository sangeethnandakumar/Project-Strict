using Strict;
using System;

namespace Demo
{
    class Program
    {
        static void Main(string[] args)
        {
            //Build command schema => This will generate dynamic regex patterns to syntax match arguments and create models as command components
            var cliBuilder = new StrictBuilder(@"C:\Users\sangeeth.CORP\Desktop\strict.xml");

            var command = Console.ReadLine();

            var route = cliBuilder.ParseCommand(command);



            ////Check if command
            //foreach(var rule in cliBuilder.Schema.Rules)
            //{
            //    Console.WriteLine(rule.Regex);
            //}

            Console.Read();
        }
    }
}
