using Newtonsoft.Json;
using Strict;
using System;

namespace Demo
{
    class Program
    {
        static void Main(string[] args)
        {
            //Build command schema => This will generate dynamic regex patterns to syntax match arguments and create models as command components
            var schema = @"C:\Users\sangeeth.CORP\Desktop\Projects\ProjectStrict\Strict\Demo\strict.xml";
            var cliBuilder = new StrictBuilder(schema);

            var command = Console.ReadLine();
            var commandRule = cliBuilder.ParseCommand(command);



            Console.WriteLine("");
            Console.WriteLine(JsonConvert.SerializeObject(commandRule));

            Console.Read();
        }
    }
}
