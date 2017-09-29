//
//  UnityDelegate+Network.m
//  Unity-iPhone
//
//  Created by kang on 2017/9/15.
//
//

#import "UnityDelegate+Network.h"
#import "AuthorizationManager.h"

@implementation UnityDelegate (Network)

/**
 检查网络状态接口
 */
- (void) checkNetWorkState {
    [AuthorizationManager checkNetworkState];
}


/**
 获取当前网络类型
 
 @return
 */
- (int) getNetworkState {
    
    return [AuthorizationManager getNetworkState];
}

/**
 获取WiFi信号强度
 
 @return
 */
- (int) getWiFiSignalStrenth {
    
    return [AuthorizationManager getWiFiSignalStrenth];
}

/**
 获取当前电量：电量（0~1.0） *100
 
 @return
 */
- (int) getBatteryLevel {
    
    return [AuthorizationManager getBatteryLevel];
}


@end
