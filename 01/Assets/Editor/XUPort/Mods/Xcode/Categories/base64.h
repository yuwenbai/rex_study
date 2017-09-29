//
//  base64.h
//  base64
//
//  Created by Yuriy Hao on 15-07-03.
//  Copyright 2015 iMobFun All Rights Reserved.
//
//  Permission is given to use this source code file, free of charge, in any
//  project, commercial or otherwise, entirely at your risk, with the condition
//  that any redistribution (in part or whole) of source code must retain
//  this copyright and permission notice. Attribution in compiled projects is
//  appreciated but not required.
//

#ifndef BASE64_DECODE_H
#define BASE64_DECODE_H

//#include <stdbool.h>
#include <ctype.h>
#include <stdio.h>
 
bool B64_Decode(char* dest, int* dest_size, const char* src, int src_size);
int B64_Encode(char* dest, int dest_size, const char* src, int src_size);

#endif // BASE_64_H
