using System;
using System.Collections.Generic;
using System.Text;

namespace Strict.Components
{
    public class OptionSet
    {
        public bool OrderEnforce { get; set; }
        public bool Optional { get; set; }
        public List<SubSet> SubSets { get; set; }

        public OptionSet()
        {
            SubSets = new List<SubSet>();
        }
    }
}
