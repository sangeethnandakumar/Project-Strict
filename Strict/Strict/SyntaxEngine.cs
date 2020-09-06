using Strict.Components;
using Strict.Structure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Strict
{
    class SyntaxEngine
    {
        public List<Rule> GenerateValidators(Command command)
        {
            foreach(var rule in command.Rules)
            {
                var regex = "^";
                var commandSample = "";
                foreach(var set in rule.Sets)
                {
                    switch(set.Inherit)
                    {
                        case "drivers":
                            regex += command.Drivers.Where(x => x.X == set.Value).FirstOrDefault().Value;
                            commandSample += command.Drivers.Where(x => x.X == set.Value).FirstOrDefault().Value;
                            break;
                        case "commandset":
                            regex += "[ ]+";
                            commandSample += " ";
                            regex += command.CommandSets.Where(x => x.X == set.Value).FirstOrDefault().Value;
                            commandSample += command.CommandSets.Where(x => x.X == set.Value).FirstOrDefault().Value;
                            break;
                        case "flags":
                            var flag = "";
                            commandSample += " ";
                            foreach(var option in set.Options)
                            {
                                if(option.Optional==false)
                                {
                                    var flagValue = command.Flags.Where(x=>x.X==option.Value).FirstOrDefault().Value;
                                    flag += $"[ ]+{flagValue}";
                                    commandSample += $"{flagValue} ";
                                }
                            }
                            flag += "(";
                            for (var i = 0; i < set.Options.Count; i++)
                            {
                                if(set.Options[i].Optional==true)
                                {
                                    flag += $"[ ]+{command.Flags.Where(x => x.X == set.Options[i].Value).FirstOrDefault().Value}";
                                    commandSample += $"{command.Flags.Where(x => x.X == set.Options[i].Value).FirstOrDefault().Value} ";
                                    flag += (i != set.Options.Count - 1) ? "|" : "";
                                }                                
                            }
                            regex += $"{flag})*";
                            break;
                        case "values":                            
                            regex += "[ ]+";
                            commandSample += " ";
                            var valProps = command.Values.Where(x => x.X == set.Value).FirstOrDefault();
                            if(valProps.Kind == "name")
                            {
                                commandSample += "<FILENAME>";
                                regex += $"[a-zA-Z]+";
                            }
                            break;
                    }                   
                }
                regex += "$";
                rule.Regex = regex;
                rule.CommandSample = commandSample;
            }
            return command.Rules;
        }
    }
}
