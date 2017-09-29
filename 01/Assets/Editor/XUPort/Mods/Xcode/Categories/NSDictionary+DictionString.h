//
//  NSDictionary+DictionString.h
//  Unity-iPhone
//
//  Created by developer on 2017/7/25.
//
//

#import <Foundation/Foundation.h>

@interface NSDictionary (DictionString)

/**
 字符串字典对象转化为 json string
 
 @return Json string
 */
- (NSString *) dictionaryToString;


/**
 char *字符串转化为字典对象
 
 @param message (char *)
 @return NSDictionary
 */
+ (NSDictionary *) dictionWithCharString:(const char *) message;


@end
