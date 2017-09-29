//
//  NetWorkAuthorise.m
//  Unity-iPhone
//
//  Created by kang on 2017/8/25.
//
//

#import "NetWorkAuthorise.h"
#import <CoreTelephony/CTCellularData.h>
#import "MJReachability.h"

typedef NS_ENUM(NSInteger,NetWorkSerViceType) {

    NetWorkSerViceNone,
    NetWorkSerViceCelluar,
    NetWorkSerViceWIFI,
    NetWorkSerViceALL
};

@implementation NetWorkAuthorise

/**
 是否开启网络权限
 
 @return
 */
+ (BOOL) isOpenNetWorkService {
    
    return true;
}

/**
 是否开启所有网络权限

 @return
 */
+ (BOOL) isOpenNetWorkALLService {

    
    return true;
}

/**
 是否开启WIFI权限

 @return
 */
+ (BOOL) isOpenNetWorkWIFIService {

    return true;
}


/**
 是否开启蜂窝网权限
 
 @return
 */
+ (BOOL) isOpenNetWorkCellularService {
    
    return true;
}


/**
 状态栏是由当前app控制的，首先获取当前app
 
 @return NetWorkType “wifi” 4G 3G 2G LTE None
 */
+ (NetWorkType) statusBarNetworkingState {
    
//    // 状态栏是由当前app控制的，首先获取当前app
//    UIApplication *app = [UIApplication sharedApplication];
//    NSArray *children = [[[app valueForKeyPath:@"statusBar"] valueForKeyPath:@"foregroundView"] subviews];
//    
//    NetWorkType type = 0;
//    for (id child in children) {
//        if ([child isKindOfClass:[NSClassFromString(@"UIStatusBarDataNetworkItemView") class]]) {
//            type = [[child valueForKeyPath:@"dataNetworkType"] intValue];
//        }
//    }
    
    NetWorkType type = 0;
    MJReachability *reachability = [MJReachability reachabilityWithHostName:@"www.baidu.com"];
    [reachability startNotifier];
    NetworkStatus status = [reachability currentReachabilityStatus];
    switch (status) {
        case NotReachable:
            type = NetWorkNone;
            NSLog(@"NetStatus:无网络");
            break;
        case ReachableViaWiFi:
            type = NetWorkWIFI;
            NSLog(@"NetStatus:WiFi");
            break;
        case ReachableVia2G:
            type = NetWork2G;
            NSLog(@"NetStatus:2G");
            break;
        case ReachableVia3G:
            type = NetWork3G;
            NSLog(@"NetStatus:3G");
            break;
        case ReachableVia4G:
            type = NetWork4G;
            NSLog(@"NetStatus:4G");
            break;
        case ReachableViaWWAN:
            type = NetWork2G;
            NSLog(@"NetStatus:WWAN");
            break;
            
        default:
            break;
    }


    return type;
}


/**
 获取WiFi信号强度

 @return
 */
+ (WIFIStrenthType) getWIFIStrength {
    
//    UIApplication *app = [UIApplication sharedApplication];
//    NSArray *subviews = [[[app valueForKey:@"statusBar"] valueForKey:@"foregroundView"] subviews];
//    NSString *dataNetworkItemView = nil;
//    
//    for (id subview in subviews) {
//        if([subview isKindOfClass:[NSClassFromString(@"UIStatusBarDataNetworkItemView") class]]) {
//            dataNetworkItemView = subview;
//            break;
//        }
//    }
//    
//    WIFIStrenthType signalStrength = [[dataNetworkItemView valueForKey:@"_wifiStrengthBars"] intValue];
//    NSLog(@"signal %ld", signalStrength);
    
    return WIFIStrenthStrong;
    
}





@end
