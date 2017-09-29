//
//  WechatRequestHandler.m
//  Unity-iPhone
//
//  Created by kang on 2017/9/14.
//
//

#import "WechatRequestHandler.h"

@implementation WechatRequestHandler

+ (void) responseWechatReq:(BaseReq *)req {

    if ([req isKindOfClass:[GetMessageFromWXReq class]]) {
        GetMessageFromWXReq *getMessageReq = (GetMessageFromWXReq *)req;
        [self managerDidRecvGetMessageReq:getMessageReq];
        
    } else if ([req isKindOfClass:[ShowMessageFromWXReq class]]) {
        ShowMessageFromWXReq *showMessageReq = (ShowMessageFromWXReq *)req;
        [self managerDidRecvShowMessageReq:showMessageReq];
        
    } else if ([req isKindOfClass:[LaunchFromWXReq class]]) {
        LaunchFromWXReq *launchReq = (LaunchFromWXReq *)req;
        [self managerDidRecvLaunchFromWXReq:launchReq];
    }
}

+ (void)managerDidRecvGetMessageReq:(GetMessageFromWXReq *)req {
    // 微信请求App提供内容， 需要app提供内容后使用sendRsp返回
    //  NSString *strTitle = [NSString stringWithFormat:@"微信请求App提供内容"];
    //  NSString *strMsg = [NSString stringWithFormat:@"openID: %@", req.openID];
    
}

+ (void)managerDidRecvShowMessageReq:(ShowMessageFromWXReq *)req {
    WXMediaMessage *msg = req.message;
    
    //显示微信传过来的内容
    // NSString *strTitle = [NSString stringWithFormat:@"微信请求App显示内容"];
    NSString *strMsg = nil;
    
    if ([msg.mediaObject isKindOfClass:[WXAppExtendObject class]]) {
        WXAppExtendObject *obj = msg.mediaObject;
        strMsg = [NSString stringWithFormat:@"openID: %@, 标题：%@ \n描述：%@ \n附带信息：%@ \n文件大小:%lu bytes\n附加消息:%@\n", req.openID, msg.title, msg.description, obj.extInfo, (unsigned long)obj.fileData.length, msg.messageExt];
    }
    else if ([msg.mediaObject isKindOfClass:[WXTextObject class]]) {
        WXTextObject *obj = msg.mediaObject;
        strMsg = [NSString stringWithFormat:@"openID: %@, 标题：%@ \n描述：%@ \n内容：%@\n", req.openID, msg.title, msg.description, obj.contentText];
    }
    else if ([msg.mediaObject isKindOfClass:[WXImageObject class]]) {
        WXImageObject *obj = msg.mediaObject;
        strMsg = [NSString stringWithFormat:@"openID: %@, 标题：%@ \n描述：%@ \n图片大小:%lu bytes\n", req.openID, msg.title, msg.description, (unsigned long)obj.imageData.length];
    }
    else if ([msg.mediaObject isKindOfClass:[WXLocationObject class]]) {
        WXLocationObject *obj = msg.mediaObject;
        strMsg = [NSString stringWithFormat:@"openID: %@, 标题：%@ \n描述：%@ \n经纬度：lng:%f_lat:%f\n", req.openID, msg.title, msg.description, obj.lng, obj.lat];
    }
    else if ([msg.mediaObject isKindOfClass:[WXFileObject class]]) {
        WXFileObject *obj = msg.mediaObject;
        strMsg = [NSString stringWithFormat:@"openID: %@, 标题：%@ \n描述：%@ \n文件类型：%@ 文件大小:%lu\n", req.openID, msg.title, msg.description, obj.fileExtension, (unsigned long)obj.fileData.length];
    }
    else if ([msg.mediaObject isKindOfClass:[WXWebpageObject class]]) {
        WXWebpageObject *obj = msg.mediaObject;
        strMsg = [NSString stringWithFormat:@"openID: %@, 标题：%@ \n描述：%@ \n网页地址：%@\n", req.openID, msg.title, msg.description, obj.webpageUrl];
    }
    
}

+ (void)managerDidRecvLaunchFromWXReq:(LaunchFromWXReq *)req {
    //    WXMediaMessage *msg = req.message;
    
    //从微信启动App
    //  NSString *strTitle = [NSString stringWithFormat:@"从微信启动"];
    // NSString *strMsg = [NSString stringWithFormat:@"openID: %@, messageExt:%@", req.openID, msg.messageExt];
    
}
@end
