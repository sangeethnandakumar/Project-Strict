using System;
using System.Collections.Generic;
using System.Text;

namespace Strict.Components
{
    public class Set
    {
        public string Inherit { get; set; }
        public string Value { get; set; }
        public List<OptionSet> OptionSets { get; set; }

        public Set()
        {
            OptionSets = new List<OptionSet>();
        }
    }
}
