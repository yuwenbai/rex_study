//
//  UnityDelegate+Orientation.m
//  Unity-iPhone
//
//  Created by kang on 2017/9/15.
//
//

#import "UnityDelegate+Orientation.h"
#import "UIApplication+CurrentViewController.h"
#import "UnityAppController+ViewHandling.h"
#import "UnityAppController+Transition.h"
#import "OrientationSupport.h"
#import <objc/runtime.h>

#import "MJNavigationController.h"
#import "MJWebViewController.h"

@implementation UnityDelegate (Orientation)

// 切换竖屏
- (void) chageToPortraitOrientation {
//    [(UnityAppController*)GetAppController() orientUnity:UIInterfaceOrientationPortrait];
}

// 切换横屏
- (void) changeToOrientationLandscape {
//    [(UnityAppController*)GetAppController() orientUnity:UIDeviceOrientationLandscapeLeft];
}

// 切换屏幕
- (void) chageToOrientation:(UIInterfaceOrientation)orientaion {
    
    if (orientaion == UIDeviceOrientationUnknown) {
        return;
    }
    
    //    [(UnityAppController*)GetAppController() orientUnity:orientaion];
    UIViewController *unityController = [(UnityAppController*)GetAppController() createUnityViewControllerForOrientation:orientaion];
    //    [(UnityAppController*)GetAppController() pressentToViewController:unityController];
    UnityAppController *appDelegate = (UnityAppController*)GetAppController();
    //    if ([appDelegate respondsToSelector:@selector(transitionToViewController:)]) {
    //        [appDelegate performSelector:@selector(transitionToViewController:) withObject:unityController];
    //    }
    
    SEL selector = NSSelectorFromString(@"transitionToViewController:");
    //    ((void (*)(id, SEL))[obj methodForSelector:selector])(obj, selector);
    IMP imp = [appDelegate methodForSelector:selector];
    void (*func)(id, SEL, UIViewController*) = (void *)imp;
    func(appDelegate, selector,unityController);
    
    [UIViewController attemptRotationToDeviceOrientation];
    [[UIApplication sharedApplication] setStatusBarOrientation:orientaion];
}

@end
