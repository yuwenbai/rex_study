//
//  WechatRequestHandler.h
//  Unity-iPhone
//
//  Created by kang on 2017/9/14.
//
//

#import <Foundation/Foundation.h>
#import "WXApi.h"

@interface WechatRequestHandler : NSObject

+ (void) responseWechatReq:(BaseReq *)req;

+ (void)managerDidRecvGetMessageReq:(GetMessageFromWXReq *)request;

+ (void)managerDidRecvShowMessageReq:(ShowMessageFromWXReq *)request;

+ (void)managerDidRecvLaunchFromWXReq:(LaunchFromWXReq *)request;

@end
