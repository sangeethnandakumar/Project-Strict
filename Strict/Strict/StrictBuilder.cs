using Colorful;
using Strict.Components;
using Strict.Structure;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;
using Console = Colorful.Console;

namespace Strict
{
    public class StrictBuilder
    {
        private string schema;
        public readonly Command Schema;
        private readonly XmlDocument doc;

        public StrictBuilder(string schema)
        {
            this.schema = schema;
            //Read the XML data
            doc = new XmlDocument();
            doc.Load(schema);
            //Schema will be parsed to memory objects
            Schema = ParseSchema();
            SyntaxEngine engine = new SyntaxEngine();
            //Generate RegeX
            Schema.Rules = engine.GenerateValidators(Schema);
        }

        public CommandData ParseCommand(string command, Rule commandRule)
        {
            var cmd = new CommandData();
            //foreach(var set in commandRule.Sets)
            //{
            //    if(set.Inherit == "flags")
            //    {
            //        foreach(var flag in set.Options)
            //        {
            //            //cmd.Flags.Add(new Flag { X = flag.Value });
            //        }
            //    }
            //    else if (set.Inherit == "values")
            //    {
            //        //cmd.Values.Add(new Value { X = set.Value });
            //    }
            //}
            return cmd;
        }

        public Rule ParseRule(string command)
        {
            //Check for reserver commands
            if (command.ToLower() == "help")
            {
                Console.WriteLine("");
                Console.WriteLine("Supported Commands On This CLI", Color.White);
                Console.WriteLine("------------------------------", Color.White);
                foreach (var rule in Schema.Rules)
                {
                    string clitext = "{0} : {1} ({2})";
                    Formatter[] colors = new Formatter[]
                    {
                        new Formatter(rule.CommandSample, Color.DeepPink),
                        new Formatter(rule.Description, Color.White),
                        new Formatter(rule.Sniplet, Color.Yellow)
                    };
                    Console.WriteLineFormatted(clitext, Color.Gray, colors);                    
                }
                Console.WriteLine();
                return null;
            }
            else if (command.ToLower() == "clear")
            {
                Console.Clear();
                return null;
            }
            else if (string.IsNullOrWhiteSpace(command))
            {                
                Console.WriteLine("Type in a command to execute", Color.Plum);
                return null;
            }
            else
            {
                bool isMatch = false;
                foreach (var rule in Schema.Rules)
                {
                    Console.WriteLine($"Rule: {rule.Regex}");
                    Regex regex = new Regex(rule.Regex);
                    Match match = regex.Match(command);
                    if (match.Success)
                    {
                        Console.WriteLine(rule.Description, Color.Plum);
                        return rule;
                    }
                }
                if (!isMatch)
                {
                    string clitext = "Unable to resolve the command: '{0}'.\nCheck if the syntax is correct or type '{1}' to see the list of commands, the CLI supports.";
                    Formatter[] colors = new Formatter[]
                    {
                        new Formatter(command, Color.DeepPink),
                        new Formatter("help", Color.DeepPink)
                    };
                    Console.WriteLineFormatted(clitext, Color.Gray, colors);
                }
                return null;
            }
        }

        private Command ParseSchema()
        {
            var command = new Command();

            //Gather drivers
            XmlNode drivers = doc.DocumentElement.SelectSingleNode("/strict/drivers");
            foreach (XmlNode driver in drivers.ChildNodes)
            {
                command.Drivers.Add(new Driver
                {
                    X = driver.Attributes["x"]?.InnerText,
                    Value = driver.InnerText,
                });
            }

            //Gather commandsets
            XmlNode commandsets = doc.DocumentElement.SelectSingleNode("/strict/commandset");
            foreach (XmlNode commandset in commandsets.ChildNodes)
            {
                command.CommandSets.Add(new CommandSet
                {
                    X = commandset.Attributes["x"]?.InnerText,
                    Value = commandset.InnerText,
                });
            }

            //Gather flags
            XmlNode flags = doc.DocumentElement.SelectSingleNode("/strict/flags");
            foreach (XmlNode flag in flags.ChildNodes)
            {
                command.Flags.Add(new Flag
                {
                    X = flag.Attributes["x"]?.InnerText,
                    Value = flag.InnerText,
                });
            }

            //Gather values
            XmlNode values = doc.DocumentElement.SelectSingleNode("/strict/values");
            foreach (XmlNode value in values.ChildNodes)
            {
                command.Values.Add(new Value
                {
                    X = value.Attributes["x"]?.InnerText,
                    AllowSpace = bool.Parse(value.Attributes["allowspace"]?.InnerText),
                    AllowWildChar = bool.Parse(value.Attributes["allowwildchar"]?.InnerText),
                    Kind = value.Attributes["kind"]?.InnerText
                });
            }

            //Gather rules
            XmlNode rules = doc.DocumentElement.SelectSingleNode("/strict/rules");
            foreach (XmlNode rule in rules.ChildNodes)
            {
                command.Rules.Add(new Rule
                {
                    X = rule.Attributes["x"]?.InnerText,
                    Description = rule.Attributes["desc"]?.InnerText,
                    Sniplet = rule.Attributes["snip"]?.InnerText,
                    Sets = ParseSets(rule)
                });
            }

            return command;
        }

        private List<Set> ParseSets(XmlNode rule)
        {
            var sets = new List<Set>();
            //Gather sets
            foreach (XmlNode set in rule.ChildNodes)
            {
                if (set.Attributes["inherit"]?.InnerText.ToLower() != "flags")
                {
                    sets.Add(new Set
                    {
                        Inherit = set.Attributes["inherit"]?.InnerText,
                        Value = set.InnerText,
                    });
                }
                else
                {
                    sets.Add(new Set
                    {
                        Inherit = set.Attributes["inherit"]?.InnerText,
                        Value = set.InnerText,
                        Options = ParseSubSets(set)
                    });
                }
            }
            return sets;
        }

        private List<SubSet> ParseSubSets(XmlNode optionSet)
        {
            var subSets = new List<SubSet>();
            //Gather subsets
            foreach (XmlNode subset in optionSet.ChildNodes)
            {
                subSets.Add(new SubSet
                {
                    Optional = bool.Parse(subset.Attributes["optional"]?.InnerText),
                    Value = subset.InnerText
                });
            }
            return subSets;
        }

    }
}
