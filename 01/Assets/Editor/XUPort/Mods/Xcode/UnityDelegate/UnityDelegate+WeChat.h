//
//  UnityDelegate+WeChat.h
//  Unity-iPhone
//
//  Created by kang on 2017/9/14.
//
//

#import "UnityDelegate.h"

@interface UnityDelegate (WeChat)

/**
 检测微信安装
 */
- (void) checkWXInstalled;

- (void) shareMessage:( const char *)message;

- (void) requestPay:(const char *)orderMessage;

- (void) requestLogin;

@end
