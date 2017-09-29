//
//  UnityAppController+Orientations.m
//  Unity-iPhone
//
//  Created by kang on 2017/9/20.
//
//

#import "UnityAppController+Orientations.h"

@implementation UnityAppController (Orientations)

#pragma mark - Orientations
#pragma mark 屏幕方向
#if !UNITY_TVOS
- (NSUInteger)application:(UIApplication*)application supportedInterfaceOrientationsForWindow:(UIWindow*)window
{
    // UIInterfaceOrientationMaskAll
    // it is the safest way of doing it:
    // - GameCenter and some other services might have portrait-only variant
    //     and will throw exception if portrait is not supported here
    // - When you change allowed orientations if you end up forbidding current one
    //     exception will be thrown
    // Anyway this is intersected with values provided from UIViewController, so we are good
    return   (1 << UIInterfaceOrientationPortrait) | (1 << UIInterfaceOrientationPortraitUpsideDown)
    | (1 << UIInterfaceOrientationLandscapeRight) | (1 << UIInterfaceOrientationLandscapeLeft);
}
#endif

@end
