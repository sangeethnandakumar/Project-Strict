using System;
using System.Collections.Generic;
using System.Text;

namespace Strict.Components
{
    public class Rule
    {
        public string X { get; set; }
        public string Description { get; set; }
        public string Sniplet { get; set; }
        public string Regex { get; set; }
        public string CommandSample { get; set; }
        public List<Set> Sets { get; set; }
    }
}
