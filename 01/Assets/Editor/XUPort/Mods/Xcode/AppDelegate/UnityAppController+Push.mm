//
//  UnityAppController+Push.m
//  Unity-iPhone
//
//  Created by kang on 2017/9/20.
//
//

#import "UnityAppController+Push.h"
//#include "AppDelegateListener.h"
#import "PluginBase/AppDelegateListener.h"


@implementation UnityAppController (Push)

+ (void) load {
    
    NSLog(@"Push load method has been call");
}

#pragma mark - Notification
#pragma mark 通知相关

#if !UNITY_TVOS
- (void)application:(UIApplication*)application didReceiveLocalNotification:(UILocalNotification*)notification
{
    AppController_SendNotificationWithArg(kUnityDidReceiveLocalNotification, notification);
    UnitySendLocalNotification(notification);
}
#endif

#if UNITY_USES_REMOTE_NOTIFICATIONS
- (void)application:(UIApplication*)application didReceiveRemoteNotification:(NSDictionary*)userInfo
{
    AppController_SendNotificationWithArg(kUnityDidReceiveRemoteNotification, userInfo);
    UnitySendRemoteNotification(userInfo);
}

- (void)application:(UIApplication*)application didRegisterForRemoteNotificationsWithDeviceToken:(NSData*)deviceToken
{
    AppController_SendNotificationWithArg(kUnityDidRegisterForRemoteNotificationsWithDeviceToken, deviceToken);
    UnitySendDeviceToken(deviceToken);
}

#if !UNITY_TVOS
- (void)application:(UIApplication *)application didReceiveRemoteNotification:(NSDictionary *)userInfo fetchCompletionHandler:(void (^)(UIBackgroundFetchResult result))handler
{
    AppController_SendNotificationWithArg(kUnityDidReceiveRemoteNotification, userInfo);
    UnitySendRemoteNotification(userInfo);
    if (handler)
    {
        handler(UIBackgroundFetchResultNoData);
    }
}
#endif

- (void)application:(UIApplication*)application didFailToRegisterForRemoteNotificationsWithError:(NSError*)error
{
    AppController_SendNotificationWithArg(kUnityDidFailToRegisterForRemoteNotificationsWithError, error);
    UnitySendRemoteNotificationError(error);
}
#endif



@end
