//
//  BatteryTool.h
//  BatteryDemo
//
//  Created by kang on 2017/9/1.
//  Copyright © 2017年 kangxq. All rights reserved.
//

#import <Foundation/Foundation.h>

@interface BatteryTool : NSObject


/**
 获取电池电量：当前电量百分比*100
 
 @return 电量
 */
+ (int) getCurrentBatteryLevel;

@end
