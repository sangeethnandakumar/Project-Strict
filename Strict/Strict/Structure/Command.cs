using Strict.Components;
using System;
using System.Collections.Generic;
using System.Text;

namespace Strict.Structure
{
    public class Command
    {
        public List<Driver> Drivers { get; set; }
        public List<CommandSet> CommandSets { get; set; }
        public List<Flag> Flags { get; set; }
        public List<Value> Values { get; set; }
        public List<Rule> Rules { get; set; }

        public Command()
        {
            Drivers = new List<Driver>();
            CommandSets = new List<CommandSet>();
            Flags = new List<Flag>();
            Values = new List<Value>();
            Rules = new List<Rule>();
        }
    }
}
