//
//  UnityDelegate+GPS.m
//  Unity-iPhone
//
//  Created by kang on 2017/9/15.
//
//

#import "UnityDelegate+GPS.h"
#import "NSDictionary+DictionString.h"
#import "AuthorizationManager.h"
#import "AMapLocation.h"

#define Untiy_GPSIsOpen @"CheckGPSIsOpen"
#define Untiy_GPSDataChanged @"GPSServerDataChanged"
#define Untiy_GPSDataRet @"GetGPSServerDataRet"


@implementation UnityDelegate (GPS)

#pragma mark - GPS定位

- (BOOL) isOpenLocationService {
    
    return [AuthorizationManager isLocationAvailable];
}

- (BOOL) isLocationServiceAuthorizationed {
    return [AuthorizationManager isLocationAvailable];
}


- (void) startUpdateGPS {
    
    [[AMapLocation shareInstance] start];
    
}

- (void) stopUpdateGPS {
    
    [[AMapLocation shareInstance] stop];
}


/**
 向Unity发送GPS信息
 
 @param locationDic： 经纬度，时间戳（毫秒），地址
 */
- (void) sendUnityGPSMessage:(NSDictionary *)locationDic {
    
    NSString *locationParamsString = [locationDic dictionaryToString];
    [self sendUnityMessageTo:UnityRoot
                      sel:UnityFunction
              methodValue:Untiy_GPSDataChanged
                enumValue:@""
               paramValue:locationParamsString];
}


/**
 Unity请求GPS时返回GPS信息
 
 @param locationDic ：经纬度、时间戳（毫秒）、地址
 */
- (void) returnUnityGPSMessage:(NSDictionary *)locationDic {
    NSString *locationParamsString = [locationDic dictionaryToString];
//    // 字典转换成Json字符串
//    NSDictionary * paramsDic = @{
//                                 Unity_MethodKey:Untiy_GPSDataRet,
//                                 Unity_EnumKey:@"",
//                                 Unity_ParamKey:locationParamsString
//                                 };
//    NSString *param = [paramsDic dictionaryToString];
//    UnitySendMessage([UnityRoot UTF8String], [UnityFunction UTF8String], [param UTF8String]);
    
    [self sendUnityMessageTo:UnityRoot
                         sel:UnityFunction
                 methodValue:Untiy_GPSDataRet
                   enumValue:@""
                  paramValue:locationParamsString];
}

@end
