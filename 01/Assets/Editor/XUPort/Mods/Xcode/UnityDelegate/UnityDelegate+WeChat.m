//
//  UnityDelegate+WeChat.m
//  Unity-iPhone
//
//  Created by kang on 2017/9/14.
//
//

#import "UnityDelegate+WeChat.h"
#import "WecahtManager.h"

@implementation UnityDelegate (WeChat)



/**
 请求微信登录
 */
- (void) requestLogin {
    [[WecahtManager shareInstance] requestLogin];
}


/**
 检查微信安装
 */
- (void) checkWXInstalled {
    [[WecahtManager shareInstance] checkWXInstalled];
}


/**
 微信分享

 @param shareMsg 分享信息
 */
- (void) shareMessage:(const char * ) shareMsg {
    [[WecahtManager shareInstance] shareMessage:shareMsg];
}


/**
 请求微信支付

 @param orderMsg 订单信息
 */
- (void) requestPay:(const char *) orderMsg {
    [[WecahtManager shareInstance] requestPay:orderMsg];
    
}

@end
