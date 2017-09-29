//
//  MJALocation.h
//  LocationDemo
//
//  Created by developer on 2017/8/15.
//  Copyright © 2017年 kang. All rights reserved.
//

#import <Foundation/Foundation.h>
#import <AMapLocationKit/AMapLocationKit.h>

@interface MJALocation : NSObject

@property (nonatomic,copy) void(^locationBlock)(NSString *longitude,NSString *latitude);
@property (nonatomic,copy) void(^addressBlock)(NSString *address);
@property (nonatomic, strong) AMapLocationManager *locationManager;

- (void) setting;
- (void) start;
- (void) stop;
@end
