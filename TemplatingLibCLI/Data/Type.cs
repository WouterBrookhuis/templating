using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TemplatingLib;

namespace TemplatingLibCLI.Data
{
    public class Type
    {
        public Module Module { get; }
        public string Name { get; }
        public bool IsBasicType { get; }
        public int SizeInBits { get; }
        public List<Field> Fields { get; }

        public Type(Module module, string name, int sizeInBits)
        {
            Module = module ?? throw new ArgumentNullException(nameof(module));
            Name = name ?? throw new ArgumentNullException(nameof(name));
            SizeInBits = sizeInBits;
            IsBasicType = true;
            Fields = new List<Field>();
        }

        public Type(Module module, string name, List<Field> fields)
        {
            Module = module ?? throw new ArgumentNullException(nameof(module));
            Name = name ?? throw new ArgumentNullException(nameof(name));
            Fields = fields ?? throw new ArgumentNullException(nameof(fields));
            SizeInBits = fields.Select(f => f.Type.SizeInBits).Sum();
            IsBasicType = false;
        }
    }
}
