//
//  UnityDelegate+GPS.h
//  Unity-iPhone
//
//  Created by kang on 2017/9/15.
//
//

#import "UnityDelegate.h"

@interface UnityDelegate (GPS)

#pragma mark - GPS定位

- (BOOL) isOpenLocationService;

- (BOOL) isLocationServiceAuthorizationed;

- (void) startUpdateGPS;

- (void) stopUpdateGPS;

/**
 向Unity发送GPS信息
 
 @param locationDic： 经纬度，时间戳（毫秒），地址
 */
- (void) sendUnityGPSMessage:(NSDictionary *)locationDic;

/**
 Unity请求GPS时返回GPS信息
 
 @param locationDic ：经纬度、时间戳（毫秒）、地址
 */
- (void) returnUnityGPSMessage:(NSDictionary *)locationDic;

@end
