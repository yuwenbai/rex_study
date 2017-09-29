//
//  WechatReqTool.h
//  Unity-iPhone
//
//  Created by kang on 2017/9/14.
//
//

#import <Foundation/Foundation.h>

@interface WechatReqTool : NSObject

+ (void) shareMessage:( const char *)message;

//+ (void)SendUrlToWX:(NSString*)title
//        description:(NSString *)desc
//             urlstr:(NSString *)url
//               type:(int)sharetype;
//
//+ (void)SendImageToWX:(NSString *)image
//                 type:(int)sharetype
//                scale:(float) rate;

+ (void) requestPay:(const char *)data;

/**
 请求登录
 */
+ (void) requestLogin;

@end
