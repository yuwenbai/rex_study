//
//  WechatReqTool.m
//  Unity-iPhone
//
//  Created by kang on 2017/9/14.
//
//

#import "WechatReqTool.h"
#import "WXApi.h"
#import "UIImage+ScaleImage.h"
#import "UIApplication+CurrentViewController.h"
#import "WecahtManager.h"
#import "NSDictionary+DictionString.h"


static NSString *kAuthScope = @"snsapi_message,snsapi_userinfo,snsapi_friend,snsapi_contact";
static NSString *kAuthOpenID = @"0c806938e2413ce73eef92cc3";
static NSString *kAuthState = @"xxx";

@implementation WechatReqTool

+ (void) shareMessage:( const char *)message
{
    NSDictionary *data=[NSDictionary dictionWithCharString:message];
    NSString *url =[[data objectForKey:@"url"] copy];
    NSString * title =[[data objectForKey:@"title"] copy];
    NSString *des= [[data objectForKey:@"description"] copy];
    NSString* strPic = [[data objectForKey:@"strPicByte"] copy];
    NSString *typestr = [[data objectForKey:@"type"] copy];
    id sharetypestr = [[data objectForKey:@"shareType"] copy];
    int type=[typestr intValue];
    int sharetype=[sharetypestr intValue];
    NSLog(@"Wechat message:%@",data);
    NSLog(@"title is:%@",title);
    NSLog(@"des is:%@",des);
    NSLog(@"url is:%@",url);
    NSLog(@"strPic is:%@, length:",strPic);
    NSLog(@"type is :%d",type);
    NSLog(@"sharetype is :%d",sharetype);
    
    if(sharetype==1) {//网页分享
        [self shareURL:[title copy] description:[des copy] urlstr:[url copy] type:type];
    }else if(sharetype==2) {//图片分享
        float rate=[[url copy] floatValue];
        [self shanreImage:[strPic copy] type:type scale:rate];
    }
    return ;
}

+ (void) shareURL:(NSString*)title
        description:(NSString *)desc
             urlstr:(NSString *)url
               type:(int)sharetype
{
    NSLog(@"=====SendUrlToWX=send url:%@",url);
    
    // 设置分享消息的标题和概要
    WXMediaMessage *mediamessage=[WXMediaMessage message];
    mediamessage.title=title;
    mediamessage.description=desc;
    
    // 设置分享的图片
    UIImage *thumbImage = [UIImage imageNamed:@"Icon_share80.png"];
    [mediamessage setThumbData:[thumbImage getImageData]];
    [mediamessage setThumbImage:thumbImage];
    
    // 设置分享的网页链接
    WXWebpageObject *webpageobj=[WXWebpageObject object];
    webpageobj.webpageUrl=url;
    //    WXAppExtendObject *webpageobj=[WXAppExtendObject object];
    //    webpageobj.url=url;
    mediamessage.mediaObject=webpageobj;
    
    // 发送消息
    SendMessageToWXReq *req=[[SendMessageToWXReq alloc]init];
    req.bText=NO;
    req.message=mediamessage;
    if(sharetype==2)
    {
        req.scene=WXSceneSession;
    }
    else
    {
        req.scene=WXSceneTimeline;
    }
    [WXApi sendReq:req];
    NSLog(@"=====SendUrlToWX=send succ");
}


+ (void) shanreImage:(NSString *)image
                 type:(int)sharetype
                scale:(float) rate
{
    //200*200
    //在这里获取应用程序Documents文件夹里的文件及文件夹列表http://write.blog.csdn.net/postedit
    NSArray *documentPaths = NSSearchPathForDirectoriesInDomains(NSDocumentDirectory, NSUserDomainMask, YES);
    NSLog(@"文件夹的所有文件:%@",documentPaths);
    NSString *documentDir = [documentPaths objectAtIndex:0];
    
    NSString *filePath = [[documentDir  stringByAppendingString:@"/screencapture.jpg"]copy];
    NSLog(@"获取第一个文件，并且拼接图片名称之后的路径:%@",filePath);
    
    NSData *imageData = [NSData dataWithContentsOfFile:filePath];
    UIImage *originImage = [[UIImage alloc] initWithContentsOfFile:[filePath copy]];
    UIImage *scaledimage= [UIImage scaleImage:originImage toScale:0.3];
    
    
    WXMediaMessage *message = [WXMediaMessage message];
    [message setThumbImage:scaledimage];
    
    
    WXImageObject *imageobject=[WXImageObject object];
    imageobject.imageData=imageData;
    message.mediaObject=imageobject;
    
    SendMessageToWXReq *req=[[SendMessageToWXReq alloc] init];
    req.bText=NO;
    req.message=message;
    req.scene=WXSceneSession;
    [WXApi sendReq:req];
    
}


+ (void) requestPay:(const char *)data
{
    NSDictionary *paydata=[NSDictionary dictionWithCharString:data];
    NSString * partnerid=[[paydata objectForKey:@"partnerId"]copy];
    NSString * prepayid=[[paydata objectForKey:@"prepayId"] copy];
    NSString * noncestr=[[paydata objectForKey:@"nonceStr"] copy];
    NSString * packageValue=[[paydata objectForKey:@"packageValue"] copy];
    NSString *sign=[[paydata objectForKey:@"sign"] copy];
    NSMutableString *stamp  = [[paydata objectForKey:@"timeStamp"]copy];
    NSLog(@"====partnerid:%@",partnerid);
    NSLog(@"====prepayid:%@",prepayid);
    NSLog(@"====noncestr:%@",noncestr);
    NSLog(@"====packageValue:%@",packageValue);
    NSLog(@"====sign:%@",sign);
    //调起微信支付
    PayReq* req             = [[PayReq alloc] init];
    req.partnerId           = partnerid;
    req.prepayId            =prepayid ;
    req.nonceStr            = noncestr;
    req.timeStamp           = stamp.intValue;
    req.package             = packageValue;
    req.sign                =sign ;
    [WXApi sendReq:req];
}


/**
 请求登录
 */
+ (void) requestLogin
{
    NSLog(@"sendAuthRequestScope");
    UIViewController *vc=[[UIApplication sharedApplication] getCurrentVC];
    
    SendAuthReq* req = [[SendAuthReq alloc] init];
    req.scope = kAuthScope; // @"post_timeline,sns"
    req.state = kAuthState;
    req.openID = kAuthOpenID;
    
    [WXApi sendAuthReq:req
        viewController:vc
              delegate:[WecahtManager shareInstance]];
    return;
}



@end
