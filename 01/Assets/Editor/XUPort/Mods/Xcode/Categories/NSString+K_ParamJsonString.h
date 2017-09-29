//
//  NSString+K_ParamJsonString.h
//  Unity-iPhone
//
//  Created by developer on 2017/7/25.
//
//

#import <Foundation/Foundation.h>
#import "NSDictionary+DictionString.h"

@interface NSString (K_ParamJsonString)

/**
 获取url链接中的参数，并转换为字符串
 
 @return json字符串
 */
- (NSString *) paramJsonString;


@end
