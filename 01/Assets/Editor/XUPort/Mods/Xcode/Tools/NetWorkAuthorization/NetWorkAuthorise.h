//
//  NetWorkAuthorise.h
//  Unity-iPhone
//
//  Created by kang on 2017/8/25.
//
//

#import <Foundation/Foundation.h>

typedef NS_ENUM(NSInteger,NetWorkType) {
    NetWorkNone = 0,
    NetWork2G,
    NetWork3G,
    NetWork4G,
    NetWorkLTE,
    NetWorkWIFI
};


typedef NS_ENUM(NSInteger,WIFIStrenthType) {
    WIFIStrenthNone,
    WIFIStrenthWeak,
    WIFIStrenthNormal,
    WIFIStrenthStrong,
};


@interface NetWorkAuthorise : NSObject

/**
 检查 statusBar 网络状态
 @param NetWorkType 网络类型
 */
+ (NetWorkType) statusBarNetworkingState;


/**
 获取WiFi信号强度
 @return WiFi信号强度
 */
+ (WIFIStrenthType) getWIFIStrength;

@end
