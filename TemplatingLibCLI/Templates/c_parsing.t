
#include "%obj_name%.h"
#include <stdint.h>

int32_t Parse%obj_name%(%struct_name%* %obj_name%, const uint8_t * buffer)
{
  %parse_fields%

  return %obj_size%;
}