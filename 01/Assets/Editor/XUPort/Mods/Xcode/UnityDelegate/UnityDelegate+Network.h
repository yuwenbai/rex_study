//
//  UnityDelegate+Network.h
//  Unity-iPhone
//
//  Created by kang on 2017/9/15.
//
//

#import "UnityDelegate.h"

@interface UnityDelegate (Network)

/**
 检查网络状态接口
 */
- (void) checkNetWorkState;

/**
 获取当前网络类型
 
 @return
 */
- (int) getNetworkState;

/**
 获取WiFi信号强度
 
 @return
 */
- (int) getWiFiSignalStrenth ;

/**
 获取当前电量：电量（0~1.0） *100
 
 @return
 */
- (int) getBatteryLevel ;


@end
