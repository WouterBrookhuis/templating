using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TemplatingLib
{
    public class ParsingOptions
    {
        public static ParsingOptions Default = new ParsingOptions
        {
            InsertionPointPrefix = '%',
            InsertionPointPostfix = '%',
            NewLine = Environment.NewLine,
        };

        public char InsertionPointPrefix { get; set; }
        public char InsertionPointPostfix { get; set; }
        public string NewLine { get; set; }
    }
}
