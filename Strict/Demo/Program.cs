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

            read:
            var command = Console.ReadLine();
            //Extract Command Rule & Command Data
            var commandRule = cliBuilder.ParseRule(command);
            var commandData = cliBuilder.ParseCommand(command, commandRule);
            goto read;
        }
    }
}
