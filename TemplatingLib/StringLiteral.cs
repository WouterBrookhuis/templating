using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TemplatingLib
{
    public class StringLiteral : IInsertable
    {
        private string _value;

        public string Value
        {
            get => _value;
            set
            {
                if (value == null)
                {
                    _value = string.Empty;
                }
                else
                {
                    _value = value;
                }
            }
        }

        public StringLiteral(string value)
        {
            _value = value ?? throw new ArgumentNullException(nameof(value));
        }

        public void Insert(TextWriter output, int currentIndentation)
        {
            output.Write(_value);
        }
    }
}
