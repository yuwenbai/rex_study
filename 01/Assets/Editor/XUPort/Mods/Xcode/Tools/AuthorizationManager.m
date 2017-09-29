//
//  AuthorizationManager.m
//  Unity-iPhone
//
//  Created by developer on 2017/8/8.
//
//

#import "AuthorizationManager.h"
#import <CoreLocation/CoreLocation.h>
#import "UnityDelegateAPI.h"
#import "NetWorkAuthorise.h"
#import "LocationAuthorise.h"
#import "BatteryTool.h"


#define IOS_VERSION_10 (NSFoundationVersionNumber > NSFoundationVersionNumber_iOS_9_x_Max)?(YES):(NO)
#define IOS_VERSION  [UIDevice currentDevice].systemVersion;
@implementation AuthorizationManager

/**
 定位是否可用
 */
+ (BOOL) isLocationAvailable {
    
    return [LocationAuthorise requestLocationAuthorizationState];
}


/**
 检查网络状态
 */
+ (void) checkNetworkState {

    NetWorkType type = [NetWorkAuthorise statusBarNetworkingState];
    if (type == NetWorkNone) {
        UIAlertView *alert = [[UIAlertView alloc]initWithTitle:@"无网络" message:@"无网络，请进入系统【设置】>【无线局域网】或者【蜂窝移动网络】中设置网络，并允许369互娱麻将使用网络" delegate:nil cancelButtonTitle:@"知道啦~" otherButtonTitles:nil, nil];
        alert.tag = 101;
        [alert show];
        
    }
    
}


/**
 获取当前网络类型

 @return
 */
+ (int) getNetworkState {
    
    NetWorkType type = [NetWorkAuthorise statusBarNetworkingState];
     NSLog(@"网络类型：%ld",type);
    if (type == NetWorkLTE) {
        return 3;
    }else if( type == NetWorkWIFI) {
        return 4;
    }
    
    return type;
}



/**
 获取WiFi信号强度

 @return
 */
+ (int) getWiFiSignalStrenth {
    
    WIFIStrenthType type = [NetWorkAuthorise getWIFIStrength];
    NSLog(@"信号强度：%ld",type);
    return type;
}

/**
 获取当前电量：电量（0~1.0） *100
 
 @return
 */
+ (int) getBatteryLevel {

    return [BatteryTool getCurrentBatteryLevel];
    
}





@end
