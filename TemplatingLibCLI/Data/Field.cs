using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TemplatingLibCLI.Data
{
    public class Field
    {
        /// <summary>
        /// The type of this field.
        /// </summary>
        public Type Type { get; }
        /// <summary>
        /// The name of this field.
        /// </summary>
        public string Name { get; }
        /// <summary>
        /// The offset in bytes from the start of the encapsulating structure where this field starts.
        /// </summary>
        public int ByteOffset { get; }
        /// <summary>
        /// The offset within the starting byte in bits where this field starts.
        /// </summary>
        public int BitOffset { get; }

        public Field(Type type, string name, int byteOffset, int bitOffset)
        {
            Type = type ?? throw new ArgumentNullException(nameof(type));
            Name = name ?? throw new ArgumentNullException(nameof(name));
            ByteOffset = byteOffset;
            BitOffset = bitOffset;
        }
    }
}
