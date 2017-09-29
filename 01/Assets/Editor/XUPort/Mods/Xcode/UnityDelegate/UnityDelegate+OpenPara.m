//
//  UnityDelegate+OpenPara.m
//  Unity-iPhone
//
//  Created by kang on 2017/9/14.
//
//

#import "UnityDelegate+OpenPara.h"
#import "UnityConstant.h"
#import "NSDictionary+DictionString.h"

@implementation UnityDelegate (OpenPara)

- (void) getOpenPara {
    
    NSString *params = [[NSUserDefaults standardUserDefaults] objectForKey:@"schemes"];
    if (params == nil || [params isEqualToString:@""]) {
        params = @"";
    }
    
    [self sendUnityMessageTo:UnityRoot
                         sel:UnityFunction
                 methodValue:Untiy_OpenPara
                   enumValue:@""
                  paramValue:params];
    
    [[NSUserDefaults standardUserDefaults] removeObjectForKey:@"schemes"];
    [[NSUserDefaults standardUserDefaults] synchronize];
    
}

@end
