//
//  BatteryTool.m
//  BatteryDemo
//
//  Created by kang on 2017/9/1.
//  Copyright © 2017年 kangxq. All rights reserved.
//

#import "BatteryTool.h"
#import <UIKit/UIKit.h>
#import <objc/runtime.h>

@implementation BatteryTool

/**
 获取电池电量：当前电量百分比*100
 @return 电量
 */

+ (int) getCurrentBatteryLevel {
    
    return [self getDeviceBatteryLevel];
}



/**
 获取状态栏电池电量，只有显示状态栏的时候才能刷新状态
 @return 电量
 */
+ (int) getStatuBarBatteryLevel {

    UIApplication *app = [UIApplication sharedApplication];
    NSArray *children = [[[app valueForKeyPath:@"statusBar"] valueForKeyPath:@"foregroundView"] subviews];
    
    int level= 0;
    for (id child in children) {
        if ([child isKindOfClass:[NSClassFromString(@"UIStatusBarBatteryItemView") class]]) {
            level = [[child valueForKeyPath:@"capacity"] intValue];
        }
    }
    return level;
}


/**
 获取设备电池电量，有误差，iOS7以5%来递变，到iOS9以%1递变，还是会有误差
 @return 电量
 */
+ (int) getDeviceBatteryLevel {

    [UIDevice currentDevice].batteryMonitoringEnabled = YES;
    double level = [UIDevice currentDevice].batteryLevel;
    NSLog(@"电量：%%%.0f",level*100);
    return level *100;
}




@end
