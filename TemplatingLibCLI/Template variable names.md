## Templates
Various templates can be specified, both globally and per type.

| Template | Description |
| --- | --- |
| FileTemplate | Top level template for the output file layout |
| DefaultTypeTemplates | Type templates to use by default |
| CustomTypeTemplates | Type templates to use for specific types. Type attribute determines type it is for |
| WriteCall | Function call to a type's write function to write a field of that type |
| ReadCall | Function call to a type's read function to read a field of that type |
| WriteFunction | Function to write a type |
| ReadFunction | Function to read a type |
| TypeTemplate | The template for the entire type/object declaration |
| TypeFieldTemplate | The template for a single field in the type/object declaration |
| TypeNameTemplate | Template for turning an obj_name and module_name into a full type name |


## Template variables
These are the variables that are available in the templates. Some depend on context, such as obj_name which in the case of TypeNameTemplate refers to the name of the object we want the type name for.

| Variable | Description |
| --- | --- |
| module_name | Name of the module (protocol) being generated |
| obj_name | Name of the object (message or type) being generated |
| obj_type | Type of the object (output of TypeNameTemplate) being generated |
| field_type | Type of an object's field (output of TypeNameTemplate) |
| field_name | Name of an object's field |
| field_byte_offset | Offset of the field from the start of the object in bytes |
| field_bit_offset | Offset of the field inside the field starting byte in bits |
| read_fields | List of field reading calls inside a read function |
| write_fields | List of field writing calls inside a write function |
| fields | List of fields of the object being generated |
| functions | List of read and write functions of the object being generated |
| constants | List of constants (empty for now) |
| objects | List of objects |
