using System;
using System.Collections.Generic;
using System.Text;

namespace Strict.Components
{
    public class CommandData
    {
        public string Command { get; set; }
        public Kind Kind { get; set; }
    }

    public enum Kind
    {
        DRIVER,
        COMMANDSET,
        FLAG,
        VALUE
    }
}
