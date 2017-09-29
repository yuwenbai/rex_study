//
//  UnityAppController+LaunchingWithOptions.m
//  Unity-iPhone
//
//  Created by kang on 2017/9/20.
//
//

#import "UnityAppController+LaunchOptions.h"
#import "UnityDelegateAPI.h"
#import "NSString+K_ParamJsonString.h"
#import "WXApi.h"
#import "UI/UnityView.h"
#import "UI/Keyboard.h"
#import "WecahtManager.h"
//#import "JPush.h"
//#import "MagicWindow.h"



@implementation UnityAppController (LaunchOptions)

-(BOOL)application:(UIApplication*)application willFinishLaunchingWithOptions:(NSDictionary*)launchOptions
{
    return YES;
}

- (BOOL)application:(UIApplication*)application didFinishLaunchingWithOptions:(NSDictionary*)launchOptions
{
    ::printf("-> applicationDidFinishLaunching()\n");
    
    NSLog(@"LaunchingWithOptions");
    
    // send notfications
#if !UNITY_TVOS
    if(UILocalNotification* notification = [launchOptions objectForKey:UIApplicationLaunchOptionsLocalNotificationKey])
        UnitySendLocalNotification(notification);
    
    if(NSDictionary* notification = [launchOptions objectForKey:UIApplicationLaunchOptionsRemoteNotificationKey])
        UnitySendRemoteNotification(notification);
    
    if ([UIDevice currentDevice].generatesDeviceOrientationNotifications == NO)
        [[UIDevice currentDevice] beginGeneratingDeviceOrientationNotifications];
#endif
    
    //检测scheme启动app
    [self checkSchemesLaunch:launchOptions];
    
    //APP启动检查定位服务和授权
    [[UnityDelegate shareInstance] isOpenLocationService];
    
    //APP启动时开启GPS定位
    [[UnityDelegate shareInstance] startUpdateGPS];
    
    //APP启动检查网络状态
    [[UnityDelegate shareInstance] checkNetWorkState];
    
    [self launchInitUnity];
    
    // 注册微信
    [WecahtManager shareInstance];
    
    return YES;
}


 //检测scheme启动app
-  (void) checkSchemesLaunch:(NSDictionary*)launchOptions {

   
    if (NSURL *url = [launchOptions objectForKey:UIApplicationLaunchOptionsURLKey]) {
        NSString *urlString = url.absoluteString;
        if ([urlString hasPrefix:@"sljsappxac://com.slj.gamemj"]) {
            [[NSUserDefaults standardUserDefaults] setObject:[urlString paramJsonString] forKey:@"schemes"];
            [[NSUserDefaults standardUserDefaults] synchronize];
        }
    }
}


@end
