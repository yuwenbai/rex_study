//
//  NSString+K_ParamJsonString.m
//  Unity-iPhone
//
//  Created by developer on 2017/7/25.
//
//

#import "NSString+K_ParamJsonString.h"


@implementation NSString (K_ParamJsonString)

/**
 获取url链接中的参数，并转换为字符串
 @param url url链接
 @return json字符串
 */
- (NSString *) paramJsonString{
    
    NSMutableDictionary *params = [[NSMutableDictionary alloc]init];
    //    NSString *urlString = @"369mj://?v_a=1&v_pa=10033474&v_pb=&v_pc=&v_pd=&v_pe=&v_j=";
    NSArray *array = [self componentsSeparatedByString:@"?"];
    if (array.count >1) {
        NSString *paramsString = array[1];
        NSArray *paramItems = [paramsString componentsSeparatedByString:@"&"];
        
        for (NSString *paramItem in paramItems) {
            NSString *paramKey;
            NSString *paramValue;
            
            NSRange range = [paramItem rangeOfString:@"="];
            if (range.location == NSNotFound) {
                paramKey = [paramItem substringToIndex:paramItem.length-1];
                paramValue = @"";
            }else {
                paramKey = [paramItem substringToIndex:range.location];
                paramValue = [paramItem substringFromIndex:range.location +1];
            }
            
            NSLog(@"key:%@ value:%@",paramKey,paramValue);
            [params setObject:paramValue forKey:paramKey];
        }
        
        NSLog(@"param:%@",params);
    }
    
    return [params dictionaryToString];
}

@end
