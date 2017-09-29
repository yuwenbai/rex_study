//
//  WecahtSingleton.h
//  Unity-iPhone
//
//  Created by kang on 2017/9/14.
//
//

#import <Foundation/Foundation.h>
#import "WXApi.h"

@protocol WechatAuthAPIDelegate<NSObject>
@optional

- (void)onAuthGotQrcode:(UIImage *)image;  //得到二维码
- (void)onQrcodeScanned;    //二维码被扫描
- (void)onAuthFinish:(int)errCode AuthCode:(NSString *)authCode;    //成功登录

@end

@interface WecahtManager : NSObject<WXApiDelegate>

+ (instancetype) shareInstance;

/**
 检测微信安装
 */
-(void) checkWXInstalled;

-(void) shareMessage:( const char *)message;

-(void) requestPay:(const char *)orderMessage;

/**
 请求登录
 */
- (void) requestLogin;

@end
