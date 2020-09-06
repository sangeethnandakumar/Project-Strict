using System;
using System.Collections.Generic;
using System.Text;

namespace Strict.Components
{
    public class Set
    {
        public string Inherit { get; set; }
        public string Value { get; set; }
        public List<SubSet> Options { get; set; }

        public Set()
        {
            Options = new List<SubSet>();
        }
    }
}
