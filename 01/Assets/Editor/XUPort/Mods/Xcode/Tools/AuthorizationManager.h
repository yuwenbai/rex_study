//
//  AuthorizationManager.h
//  Unity-iPhone
//
//  Created by developer on 2017/8/8.
//
//

#import <Foundation/Foundation.h>

@interface AuthorizationManager : NSObject


/**
 定位是否可用
 */
+ (BOOL) isLocationAvailable;

/**
 检查网络状态
 */
+ (void) checkNetworkState;


/**
 获取当前网络类型
 
 @return
 */
+ (int) getNetworkState;


/**
 获取WiFi信号强度
 
 @return
 */
+ (int) getWiFiSignalStrenth;


/**
 获取当前电量：电量（0~1.0） *100
 
 @return
 */
+ (int) getBatteryLevel;

@end
