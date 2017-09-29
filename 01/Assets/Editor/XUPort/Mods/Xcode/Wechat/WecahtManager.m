//
//  WecahtSingleton.m
//  Unity-iPhone
//
//  Created by kang on 2017/9/14.
//
//

#import "WecahtManager.h"
#import "WechatResponseHandler.h"
#import "WechatRequestHandler.h"
#import "WechatReqTool.h"
#import "UnityConstant.h"
#import "NSDictionary+DictionString.h"

@implementation WecahtManager

#pragma mark - LifeCycle
+ (instancetype)allocWithZone:(struct _NSZone *)zone
{
    static WecahtManager * wecahtInstance = nil;
    static dispatch_once_t onceToken;
    dispatch_once(&onceToken, ^{
        wecahtInstance = [super allocWithZone:zone];
        [wecahtInstance registApp];
    });
    return wecahtInstance;
}

+ (instancetype) shareInstance {
    return [self allocWithZone:nil];
}

- (id) copyWithZone:(NSZone *)zone;{
    return self;
}

// 注册微信
- (void) registApp {

    //向微信注册
    [WXApi registerApp:@"wx8820c0bb95c6fc96" enableMTA:YES];
    
    //向微信注册支持的文件类型
    UInt64 typeFlag = MMAPP_SUPPORT_TEXT | MMAPP_SUPPORT_PICTURE | MMAPP_SUPPORT_LOCATION | MMAPP_SUPPORT_VIDEO |MMAPP_SUPPORT_AUDIO | MMAPP_SUPPORT_WEBPAGE | MMAPP_SUPPORT_DOC | MMAPP_SUPPORT_DOCX | MMAPP_SUPPORT_PPT | MMAPP_SUPPORT_PPTX | MMAPP_SUPPORT_XLS | MMAPP_SUPPORT_XLSX | MMAPP_SUPPORT_PDF;
    
    [WXApi registerAppSupportContentFlag:typeFlag];
}

#pragma mark - WXApiDelegate
- (void)onResp:(BaseResp *)resp {
    [WechatResponseHandler responseWechatResp:resp];
}


- (void)onReq:(BaseReq *)req {
    [WechatRequestHandler responseWechatReq:req];
}

#pragma mark - WechatAuthAPIDelegate
//得到二维码
- (void)onAuthGotQrcode:(UIImage *)image
{
    NSLog(@"onAuthGotQrcode");
}

//二维码被扫描
- (void)onQrcodeScanned
{
    NSLog(@"onQrcodeScanned");
}

//成功登录
- (void)onAuthFinish:(int)errCode AuthCode:(NSString *)authCode
{
    NSLog(@"onAuthFinish");
}


#pragma mark - private

/**
 检测微信安装
 */
-(void) checkWXInstalled {
    NSLog(@"CheckWXInstalled");
    if([WXApi isWXAppInstalled])
    {
        NSLog(@"通知unity 微信已经安装了");
        // 字典转换成Json字符串
        NSDictionary * isWXInstalledDict = @{Unity_MethodKey:Unity_enum_wxInstalled,
                                             Unity_EnumKey:@"",
                                             Unity_ParamKey:@"true"};
        //通知unity 微信已经安装了
        NSString *paramsStr=[isWXInstalledDict dictionaryToString];
        UnitySendMessage([UnityRoot UTF8String], [UnityFunction UTF8String], [paramsStr UTF8String]);
    }
    else
    {
        NSLog(@"通知unity 微信没有安装");
        // 字典转换成Json字符串
        NSDictionary * isWXInstalledDict = @{Unity_MethodKey:Unity_enum_wxInstalled,
                                             Unity_EnumKey:@"",
                                             Unity_ParamKey:@"false"};
        //通知unity 微信没有安装
        NSString *paramsStr=[isWXInstalledDict dictionaryToString];
        UnitySendMessage([UnityRoot UTF8String], [UnityFunction UTF8String], [paramsStr UTF8String]);
        
    }
    return;
}

- (void) requestLogin {
    [WechatReqTool requestLogin];
}

-(void) shareMessage:( const char *)message {
    [WechatReqTool shareMessage:message];
}


-(void) requestPay:(const char *)orderMessage {
    [WechatReqTool requestPay:orderMessage];
}


@end
