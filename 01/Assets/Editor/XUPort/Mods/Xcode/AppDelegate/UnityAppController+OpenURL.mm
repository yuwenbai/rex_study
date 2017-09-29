//
//  UnityAppController+OpenURL.m
//  Unity-iPhone
//
//  Created by kang on 2017/9/20.
//
//

#import "UnityAppController+OpenURL.h"
#import "WXApi.h"
#import "WecahtManager.h"
#import "UnityDelegateAPI.h"
#import "NSDictionary+DictionString.h"
#import "NSString+K_ParamJsonString.h"
#import "PluginBase/AppDelegateListener.h"
//#import "MWApi.h"

@implementation UnityAppController (OpenURL)


+ (void) load {
    NSLog(@"OpenURL load method has been call");
    
}



#pragma mark 微信等相关处理

- (BOOL)application:(UIApplication*)application openURL:(NSURL*)url sourceApplication:(NSString*)sourceApplication annotation:(id)annotation
{
    NSLog(@"sourceApp:%@",sourceApplication);
    NSMutableArray* keys	= [NSMutableArray arrayWithCapacity:3];
    NSMutableArray* values	= [NSMutableArray arrayWithCapacity:3];
    
    auto addItem = [&](NSString* key, id value)
    {
        [keys addObject:key];
        [values addObject:value ? value : [NSNull null]];
    };
    
    addItem(@"url", url);
    addItem(@"sourceApplication", sourceApplication);
    addItem(@"annotation", annotation);
    
    NSDictionary* notifData = [NSDictionary dictionaryWithObjects:values forKeys:keys];
    AppController_SendNotificationWithArg(kUnityOnOpenURL, notifData);
    
    // 微信跳转
    if ([sourceApplication containsString:sourceApplication]) {
        return [WXApi handleOpenURL:url delegate:[WecahtManager shareInstance]];
    }else {
        return YES;
    }
    
}


/**
 这个方法是用于从微信返回第三方App
 
 @param application
 @param url
 @return
 */
- (BOOL)application:(UIApplication *)application handleOpenURL:(NSURL *)url {
    NSLog(@"befor iOS9 url:%@",url);
    NSString *urlString = url.absoluteString;
    if ([urlString hasPrefix:@"wx8820c0bb95c6fc96"]) {
        BOOL result = [WXApi handleOpenURL:url delegate:[WecahtManager shareInstance]];
        return result;
    }else if ([urlString hasPrefix:@"sljsappxac://com.slj.gamemj"]) {
        
        [[UnityDelegate shareInstance] sendUnityMessageTo:UnityRoot  sel:UnityFunction methodKey:Untiy_OpenPara params:[urlString paramJsonString]];
        return YES;
    }
//    else if([urlString hasPrefix:@"sljsappxac://com.slj.gamemj"]) {
//        [MWApi routeMLink:url];
//    }
    return YES;
}

//iOS9以后走的是这个方法
-(BOOL)application:(UIApplication *)app openURL:(NSURL *)url options:(NSDictionary<NSString *,id> *)options{
    NSLog(@"after iOS9 url:%@",url);
    NSString *urlString = url.absoluteString;
    if ([urlString hasPrefix:@"wx8820c0bb95c6fc96"]) {
        BOOL result = [WXApi handleOpenURL:url delegate:[WecahtManager shareInstance]];
        return result;
    }else if ([urlString hasPrefix:@"sljsappxac://com.slj.gamemj"]) {
        
        [[UnityDelegate shareInstance] sendUnityMessageTo:UnityRoot  sel:UnityFunction methodKey:Untiy_OpenPara params:[urlString paramJsonString]];
        return YES;
    }
//    else if([urlString hasPrefix:@"sljsappxac://com.slj.gamemj"]) {
//        [MWApi routeMLink:url];
//    }
    
    return YES;
}


//- (BOOL)application:(UIApplication *)application continueUserActivity:(NSUserActivity *)userActivity restorationHandler:(void (^)(NSArray * _Nullable))restorationHandler
//{
//    //如果使用了Universal link ，此方法必写
//    return [MWApi continueUserActivity:userActivity];
//}
//
//+ (BOOL)continueUserActivity:(nonnull NSUserActivity *)userActivity {
//
//    return YES;
//}



@end
