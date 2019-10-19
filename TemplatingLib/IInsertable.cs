using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TemplatingLib
{
    public interface IInsertable
    {
        void Insert(TextWriter output, int currentIndentation);
    }
}
