//
//  UnityDelegate.h
//  Unity-iPhone
//  Unity的单例类，用于处理Unity端的函数调用
//  Created by developer on 2017/7/25.
//
//

#import <Foundation/Foundation.h>
#import "UnityConstant.h"


@interface UnityDelegate : NSObject

+ (instancetype) shareInstance;

/**
 发送Unity消息
 
 @param unityObj Unity消息响应对象
 @param unitySEL Unity响应消息方法
 @param unityMenthodKey Unity响应消息方法的key
 @param paramString 消息参数
 */
- (void) sendUnityMessageTo:(NSString *)unityObj sel:(NSString *)unitySEL methodKey:(NSString *)unityMenthodKey params:(NSString *)paramString;


/**
 发送Unity消息
 
 @param unityObj Unity消息响应对象
 @param unitySEL Unity响应消息方法
 */
- (void) sendUnityMessageTo:(NSString *)unityObj
                        sel:(NSString *)unitySEL
                methodValue:(NSString *)unityMenthodValue
                  enumValue:(NSString *)enumValue
                 paramValue:(NSString *)paramString;





@end
