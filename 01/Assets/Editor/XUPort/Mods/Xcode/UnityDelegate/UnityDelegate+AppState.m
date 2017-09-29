//
//  UnityDelegate+AppState.m
//  Unity-iPhone
//
//  Created by kang on 2017/9/14.
//
//

#import "UnityDelegate+AppState.h"
#import "UnityConstant.h"
#import "NSDictionary+DictionString.h"

@implementation UnityDelegate (AppState)

-(void)SendUnityApplicationOnPause
{
//    // 字典转换成Json字符串
//    NSDictionary * onPauseStr = @{Unity_MethodKey:Untiy_LIFECYCLE,
//                                  Unity_EnumKey:@"",
//                                  Unity_ParamKey:Untiy_OnPause};
//    NSString *param = [onPauseStr dictionaryToString];
//    UnitySendMessage([UnityRoot UTF8String], [UnityFunction UTF8String], [param UTF8String]);
    
    
    [self sendUnityMessageTo:UnityRoot
                         sel:UnityFunction
                 methodValue:Untiy_LIFECYCLE
                   enumValue:@""
                  paramValue:Untiy_OnPause];
    
}

-(void)SendUnityApplicationOnResume
{
//    // 字典转换成Json字符串
//    NSDictionary * onPauseStr = @{Unity_MethodKey:Untiy_LIFECYCLE,
//                                  Unity_EnumKey:@"",
//                                  Unity_ParamKey:Untiy_OnResume};
//    NSString *param = [onPauseStr dictionaryToString];
//    UnitySendMessage([UnityRoot UTF8String], [UnityFunction UTF8String], [param UTF8String]);
    
    [self sendUnityMessageTo:UnityRoot
                         sel:UnityFunction
                 methodValue:Untiy_LIFECYCLE
                   enumValue:@""
                  paramValue:Untiy_OnResume];
}


@end
