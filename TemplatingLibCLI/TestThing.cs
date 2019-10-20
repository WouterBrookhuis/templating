using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TemplatingLib;

namespace TemplatingLibCLI
{
    class TestThing
    {
        public List<Data.Module> FromProtocol(Messages.Protocol protocol)
        {
            var modules = new List<Data.Module>();
            var types = new Dictionary<string, Data.Type>();

            var protocolModule = new Data.Module(protocol.Name);
            modules.Add(protocolModule);

            foreach(var protocolType in protocol.Types)
            {
                BuildType(protocol, protocolType, protocolModule, 0);
            }

            foreach(var protocolMessage in protocol.Messages)
            {
                var messageAsType = new Messages.Type
                {
                    Fields = protocolMessage.Fields,
                    Name = protocolMessage.Name,
                    NumBits = 0
                };
                BuildType(protocol, messageAsType, protocolModule, 0);
            }

            return modules;
        }

        public void BuildType(Messages.Protocol protocol, Messages.Type protoType, Data.Module module, int depth)
        {
            if (module.GetType(protoType.Name) != null)
            {
                return;
            }

            if(depth == 100)
            {
                // Save to assume this is nested waaaay too deep
                throw new Exception($"Too much type nesting in protocol {protocol.Name}");
            }
            
            if(protoType.Fields != null && protoType.Fields.Count > 0)
            {
                // Ensure all types of our fields exist
                int bitOffset = 0;
                var fields = new List<Data.Field>();
                foreach(var field in protoType.Fields)
                {
                    var fieldType = protocol.Types.FirstOrDefault(t => t.Name == field.Type);
                    if(fieldType == null)
                    {
                        throw new Exception($"Missing type {field.Type} in protocol {protocol.Name}");
                    }
                    BuildType(protocol, fieldType, module, depth + 1);

                    var type = module.GetType(field.Type);
                    fields.Add(new Data.Field(type, field.Name, bitOffset / 8, bitOffset % 8));
                    bitOffset += type.SizeInBits;
                }
                // Create our type
                module.AddType(new Data.Type(module, protoType.Name, fields));
            }
            else
            {
                // Create our type
                module.AddType(new Data.Type(module, protoType.Name, protoType.NumBits));
            }
        }

        public void WriteModule(TextWriter output, TemplateLibrary library, Data.Module module)
        {
            var baseDict = new Dictionary<string, IInsertable>()
            {
                { "module_name", new StringLiteral(module.Name) },
            };

            var objectsContainer = new InsertableContainer()
            {
                AutoIndentAndNewline = true
            };

            foreach(var type in module.Types.Values)
            {
                if (!library.CustomTypeTemplates.TryGetValue(type.Name, out TemplateLibrary.TypeTemplates typeTemplates))
                {
                    typeTemplates = library.GenericTypeTemplates;
                }

                var typeDict = new NestedDictionary<string, IInsertable>(baseDict)
                {
                    { "obj_name", new StringLiteral(type.Name) },
                };

                // Fields
                var fieldsContainer = new InsertableContainer()
                {
                    AutoIndentAndNewline = true
                };
                foreach(var field in type.Fields)
                {
                    var fieldDict = new NestedDictionary<string, IInsertable>(typeDict)
                    {
                        { "field_type", new TemplateInsert(library.TypeNameTemplate, new NestedDictionary<string, IInsertable>(baseDict)
                        {
                            { "obj_name", new StringLiteral(field.Type.Name) }
                        }) },
                        { "field_name", new StringLiteral(field.Name) },
                    };
                    fieldsContainer.Insertables.Add(new TemplateInsert(typeTemplates.TypeFieldTemplate, fieldDict));
                }
                typeDict.Add("fields", fieldsContainer);

                // Functions
                var functionsContainer = new InsertableContainer()
                {
                    AutoIndentAndNewline = true
                };

                // Read and write functions for this type

                var readCallContainer = new InsertableContainer()
                {
                    AutoIndentAndNewline = true
                };

                var writeCallContainer = new InsertableContainer()
                {
                    AutoIndentAndNewline = true
                };

                foreach(var field in type.Fields)
                {
                    var fieldReadWriteCallDict = new NestedDictionary<string, IInsertable>(typeDict)
                    {
                        { "field_type", new TemplateInsert(library.TypeNameTemplate, new NestedDictionary<string, IInsertable>(baseDict)
                        {
                            { "obj_name", new StringLiteral(field.Type.Name) }
                        }) },
                        { "field_name", new StringLiteral(field.Name) },
                        { "field_byte_offset", new StringLiteral(field.ByteOffset.ToString()) },
                        { "field_bit_offset", new StringLiteral(field.BitOffset.ToString()) },
                    };

                    if(!library.CustomTypeTemplates.TryGetValue(field.Type.Name, out TemplateLibrary.TypeTemplates fieldTypeTemplates))
                    {
                        fieldTypeTemplates = library.GenericTypeTemplates;
                    }

                    readCallContainer.Insertables.Add(new TemplateInsert(fieldTypeTemplates.ReadCall, fieldReadWriteCallDict));
                    writeCallContainer.Insertables.Add(new TemplateInsert(fieldTypeTemplates.WriteCall, fieldReadWriteCallDict));
                }


                var readFuncDict = new NestedDictionary<string, IInsertable>(typeDict)
                {
                    { "read_fields", readCallContainer }
                };
                functionsContainer.Insertables.Add(new TemplateInsert(typeTemplates.ReadFunction, readFuncDict));

                var writeFuncDict = new NestedDictionary<string, IInsertable>(typeDict)
                {
                    { "write_fields", writeCallContainer }
                };
                functionsContainer.Insertables.Add(new TemplateInsert(typeTemplates.WriteFunction, writeFuncDict));

                typeDict.Add("functions", functionsContainer);

                // Write out
                objectsContainer.Insertables.Add(new TemplateInsert(typeTemplates.TypeTemplate, typeDict));
            }

            var fileDict = new NestedDictionary<string, IInsertable>(baseDict)
            {
                { "constants", new StringLiteral($"#define MODULE_NAME {module.Name}") },
                { "objects", objectsContainer }
            };

            library.FileTemplate.Apply(output, fileDict, 0);
        }
    }
}
