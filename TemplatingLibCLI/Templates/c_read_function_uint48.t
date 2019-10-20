void %module_name%_%obj_name%_Read(%module_name%_%obj_name%_t* %obj_name%, const uint8_t* Buffer)
{
  *%obj_name% = (Buffer[0] << 40) | (Buffer[1] << 32) | (Buffer[2] << 24) | (Buffer[3] << 16) | (Buffer[4] << 8) | Buffer[5];
}
