using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TemplatingLib
{
    public class ParsingException : Exception
    {
        public int Line { get; }
        public int Column { get; }

        public ParsingException(int line, int column, string message) : base(message)
        {
            Column = column;
            Line = line;
        }
    }
}
