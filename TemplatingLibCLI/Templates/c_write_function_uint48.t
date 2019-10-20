void %module_name%_%obj_name%_Write(const %module_name%_%obj_name%_t* %obj_name%, uint8_t* Buffer)
{
  Buffer[0] = *%obj_name% >> 40;
  Buffer[1] = *%obj_name% >> 32;
  Buffer[2] = *%obj_name% >> 24;
  Buffer[3] = *%obj_name% >> 16;
  Buffer[4] = *%obj_name% >> 8;
  Buffer[5] = *%obj_name%;
}
