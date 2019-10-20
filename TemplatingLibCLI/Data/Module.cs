using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TemplatingLibCLI.Data
{
    public class Module
    {
        public string Name { get; }
        public Dictionary<string, Type> Types { get; }

        public Module(string name)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
            Types = new Dictionary<string, Type>();
        }

        public void AddType(Type type)
        {
            Types.Add(type.Name, type);
        }

        public Type GetType(string name)
        {
            if (Types.TryGetValue(name, out Type result))
            {
                return result;
            }
            return null;
        }
    }
}
