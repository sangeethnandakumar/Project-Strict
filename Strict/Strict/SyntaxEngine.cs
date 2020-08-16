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
                foreach(var set in rule.Sets)
                {
                    switch(set.Inherit)
                    {
                        case "drivers":
                            regex += command.Drivers.Where(x => x.X == set.Value).FirstOrDefault().Value;
                            break;
                        case "commandset":
                            regex += " ";
                            regex += command.CommandSets.Where(x => x.X == set.Value).FirstOrDefault().Value;                            
                            break;
                        case "flags":                            
                            foreach(var optionset in set.OptionSets)
                            {
                                var flag = "(";
                                for (var i= 0; i < optionset.SubSets.Count; i++)
                                {
                                    flag += $" {command.Flags.Where(x => x.X == optionset.SubSets[i].Value).FirstOrDefault().Value}";
                                    flag += (i != optionset.SubSets.Count - 1) ? "|" : "";
                                }
                                regex += $"{flag})*";
                            }                            
                            break;
                        case "values":
                            regex += " ";
                            var valProps = command.Values.Where(x => x.X == set.Value).FirstOrDefault();
                            if(valProps.Kind == "name")
                            {
                                regex += $"[a-zA-Z]+";
                            }
                            break;
                    }                   
                }
                regex += "$";
                rule.Regex = regex;
            }
            return command.Rules;
        }
    }
}
