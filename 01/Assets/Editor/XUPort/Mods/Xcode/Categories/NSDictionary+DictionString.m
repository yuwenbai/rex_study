//
//  NSDictionary+DictionString.m
//  Unity-iPhone
//
//  Created by developer on 2017/7/25.
//
//

#import "NSDictionary+DictionString.h"

@implementation NSDictionary (DictionString)


/**
 字符串字典对象转化为 json string

 @return Json string
 */
- (NSString *) dictionaryToString {
    
    NSError *parseError = nil;
    NSData *jsonData = [NSJSONSerialization dataWithJSONObject:self options:NSJSONWritingPrettyPrinted error:&parseError];
    return [[NSString alloc] initWithData:jsonData encoding:NSUTF8StringEncoding];
}



/**
 char *字符串转化为字典对象

 @param message (char *)
 @return NSDictionary
 */
+ (NSDictionary *) dictionWithCharString:(const char *) message {

    NSLog(@"xcode orign data:%s",message);
   
    NSString *str=[NSString stringWithCString:message encoding:NSUTF8StringEncoding];
    
    NSLog(@"==xcode ==str:%@",str);
    NSData *data=[NSData dataWithBytes:message length:[str length]];
    NSDictionary *json = [NSJSONSerialization JSONObjectWithData:data options:NSJSONReadingMutableContainers error:nil];
    
    if (!json) {
        NSLog(@"json parse failed \r\n");
        return nil;
    }
    return json;
}

@end
