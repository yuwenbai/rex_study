//
//  WechatResponseHandler.h
//  Unity-iPhone
//
//  Created by kang on 2017/9/14.
//
//

#import <Foundation/Foundation.h>
#import "WXApi.h"

@interface WechatResponseHandler : NSObject

+ (void) responseWechatResp:(BaseResp *)resp;

//+ (void)managerDidRecvMessageResponse:(SendMessageToWXResp *)response;
//
//+ (void)managerDidRecvAuthResponse:(SendAuthResp *)response;
//
//+ (void)managerDidRecvAddCardResponse:(AddCardToWXCardPackageResp *)response;
//
//+ (void)managerDidRecvChooseCardResponse:(WXChooseCardResp *)response;
//
//+ (void)managerDidRecvChooseInvoiceResponse:(WXChooseInvoiceResp *)response;
//
//+ (void)managerDidRecvPayResponse:(PayResp *)response;

@end
