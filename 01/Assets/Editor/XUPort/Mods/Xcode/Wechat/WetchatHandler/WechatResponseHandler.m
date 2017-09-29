//
//  WechatResponseHandler.m
//  Unity-iPhone
//
//  Created by kang on 2017/9/14.
//
//

#import "WechatResponseHandler.h"
#import "UnityConstant.h"
#import "NSDictionary+DictionString.h"
#import "UnityDelegateAPI.h"

@implementation WechatResponseHandler


+ (void) responseWechatResp:(BaseResp *)resp {
    
    if ([resp isKindOfClass:[SendMessageToWXResp class]]) {
        SendMessageToWXResp *messageResp = (SendMessageToWXResp *)resp;
        [self managerDidRecvMessageResponse:messageResp];
        
    } else if ([resp isKindOfClass:[SendAuthResp class]]) {
        SendAuthResp *authResp = (SendAuthResp *)resp;
        [self managerDidRecvAuthResponse:authResp];
        
    } else if ([resp isKindOfClass:[AddCardToWXCardPackageResp class]]) {
        AddCardToWXCardPackageResp *addCardResp = (AddCardToWXCardPackageResp *)resp;
        [self managerDidRecvAddCardResponse:addCardResp];
        
    } else if ([resp isKindOfClass:[WXChooseCardResp class]]) {
        WXChooseCardResp *chooseCardResp = (WXChooseCardResp *)resp;
        [self managerDidRecvChooseCardResponse:chooseCardResp];
        
    }else if ([resp isKindOfClass:[WXChooseInvoiceResp class]]){
        WXChooseInvoiceResp *chooseInvoiceResp = (WXChooseInvoiceResp *)resp;
        [self managerDidRecvChooseInvoiceResponse:chooseInvoiceResp];
        
    }else if([resp isKindOfClass:[PayResp class]]) {
        PayResp *payResp = (PayResp *)resp;
        [self managerDidRecvPayResponse:payResp];
    }
}


+ (void)managerDidRecvMessageResponse:(SendMessageToWXResp *)response {
    
}

+ (void)managerDidRecvAddCardResponse:(AddCardToWXCardPackageResp *)response {
    NSMutableString* cardStr = [[NSMutableString alloc] init];
    for (WXCardItem* cardItem in response.cardAry) {
        [cardStr appendString:[NSString stringWithFormat:@"code:%@ cardid:%@ cardext:%@ cardstate:%u\n",cardItem.encryptCode,cardItem.cardId,cardItem.extMsg,(unsigned int)cardItem.cardState]];
    }
    
}

+ (void)managerDidRecvChooseCardResponse:(WXChooseCardResp *)response {
    NSMutableString* cardStr = [[NSMutableString alloc] init];
    for (WXCardItem* cardItem in response.cardAry) {
        [cardStr appendString:[NSString stringWithFormat:@"cardid:%@, encryptCode:%@, appId:%@\n",cardItem.cardId,cardItem.encryptCode,cardItem.appID]];
    }
}

+ (void)managerDidRecvChooseInvoiceResponse:(WXChooseInvoiceResp *)response {
    NSMutableString* cardStr = [[NSMutableString alloc] init];
    for (WXInvoiceItem* cardItem in response.cardAry) {
        [cardStr appendString:[NSString stringWithFormat:@"cardid:%@, encryptCode:%@, appId:%@\n",cardItem.cardId,cardItem.encryptCode,cardItem.appID]];
    }
}


+ (void)managerDidRecvAuthResponse:(SendAuthResp *)response {
    NSString *strTitle = [NSString stringWithFormat:@"Auth登录结果"];
    
    NSString *code = response.code;
    if ([response.code isEqualToString:@""] ||response.code== nil) {
        return;
    }
    
    NSString *strMsg = [NSString stringWithFormat:@"code:%@,state:%@,errcode:%d", code, response.state, response.errCode];
    NSLog(@"%@",strTitle);
    NSLog(@"%@",strMsg);
    
    [[UnityDelegate shareInstance] sendUnityMessageTo:UnityRoot
                         sel:UnityFunction
                 methodValue:Untiy_TempCode
                   enumValue:@""
                  paramValue:code];
    
}


/**
 微信支付请求返回响应
 
 @param response
 */
+ (void)managerDidRecvPayResponse:(PayResp *)response {
    
    NSString *responseCode = @"";
    if (response.errCode != 0) {
        NSString *errorString = [NSString stringWithFormat:@"%d",response.errCode];
        responseCode = errorString;
        
    }
    
    [[UnityDelegate shareInstance] sendUnityMessageTo:UnityRoot
                                                  sel:UnityFunction
                                          methodValue:Unity_WXPay
                                            enumValue:@""
                                           paramValue:responseCode];
}

@end
