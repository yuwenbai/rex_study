//
//  GaoLocation.h
//  LocationDemo
//
//  Created by developer on 2017/8/16.
//  Copyright © 2017年 kang. All rights reserved.
//

#import <Foundation/Foundation.h>

@interface AMapLocation : NSObject
{
    
}


+ (instancetype) shareInstance;
//@property (nonatomic,copy) void(^locationBlock)(NSString *longitude,NSString *latitude);
//@property (nonatomic,copy) void(^addressBlock)(NSString *address);

- (void) start;
- (void) stop;

@end
