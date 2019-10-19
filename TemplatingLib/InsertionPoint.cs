using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TemplatingLib
{
    class InsertionPoint
    {
        public int Column { get; }
        public int Line { get; }
        public string Name { get; }

        public InsertionPoint(int column, int line, string name)
        {
            Column = column;
            Line = line;
            Name = name ?? throw new ArgumentNullException(nameof(name));
        }
    }
}
