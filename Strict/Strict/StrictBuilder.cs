using Strict.Components;
using Strict.Structure;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;

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

        public Command ParseCommand(string command)
        {
            bool isMatch = false;
            foreach(var rule in Schema.Rules)
            {
                Regex regex = new Regex(rule.Regex);
                Match match = regex.Match(command);
                if (match.Success)
                {
                    return null;
                }
            }
            if(!isMatch)
            {
                Console.WriteLine("Unable to recognise the following command");
            }
            return null;
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
                if(set.Attributes["inherit"]?.InnerText.ToLower() != "flags")
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
                        OptionSets = ParseOptionSets(set)
                    });
                }                
            }
            return sets;
        }

        private List<OptionSet> ParseOptionSets(XmlNode set)
        {
            var optionSets = new List<OptionSet>();
            //Gather optionsets
            foreach (XmlNode optionSet in set.ChildNodes)
            {
                optionSets.Add(new OptionSet
                {
                    OrderEnforce = bool.Parse(optionSet.Attributes["orderenforce"]?.InnerText),
                    Optional = bool.Parse(optionSet.Attributes["optional"]?.InnerText),
                    SubSets = ParseSubSets(optionSet)
                });
            }
            return optionSets;
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
