//
//  LocationAuthorise.h
//  Unity-iPhone
//
//  Created by User on 2017/8/25.
//
//

#import <Foundation/Foundation.h>

@protocol LocationAuthorizationDelegate <NSObject>

@optional
// 跳转定位服务开启页面
- (void) forwardSettingLocationService;
// 跳转应用设置页面
- (void) forwardSettingLocationAuthorization;

@end

@interface LocationAuthorise : NSObject



// 请求网络状态
+ (BOOL) requestLocationAuthorizationState;


@end
