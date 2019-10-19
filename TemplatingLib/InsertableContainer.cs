using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TemplatingLib
{
    public class InsertableContainer : IInsertable
    {
        public List<IInsertable> Insertables { get; set; }
        public bool AutoIndentAndNewline { get; set; }
        public string NewLine { get; set; }

        public InsertableContainer()
        {
            Insertables = new List<IInsertable>();
            AutoIndentAndNewline = false;
            NewLine = Environment.NewLine;
        }

        public InsertableContainer(IEnumerable<IInsertable> insertables)
        {
            Insertables = new List<IInsertable>(insertables);
            AutoIndentAndNewline = false;
            NewLine = Environment.NewLine;
        }

        public void Insert(TextWriter output, int currentIndentation)
        {
            for (int i = 0; i < Insertables.Count; i++)
            {
                if (AutoIndentAndNewline && i > 0)
                {
                    output.Write("".PadLeft(currentIndentation));
                }

                Insertables[i].Insert(output, currentIndentation);

                if (AutoIndentAndNewline && i != Insertables.Count - 1)
                {
                    output.Write(NewLine);
                }
            }
        }
    }
}
