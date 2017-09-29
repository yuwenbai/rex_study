//
//  UIApplication+CurrentViewController.m
//  Unity-iPhone
//
//  Created by developer on 2017/7/25.
//
//

#import "UIApplication+CurrentViewController.h"

@implementation UIApplication (CurrentViewController)


/**
  获取当前屏幕显示的viewcontroller

 @return 返回屏幕最上层的ViewController
 */
- (UIViewController *)getCurrentVC
{
    UIViewController *result = nil;
    
    UIWindow * window = [self keyWindow];
    if (window.windowLevel != UIWindowLevelNormal)
    {
        NSArray *windows = [[UIApplication sharedApplication] windows];
        for(UIWindow * tmpWin in windows)
        {
            if (tmpWin.windowLevel == UIWindowLevelNormal)
            {
                window = tmpWin;
                break;
            }
        }
    }
    
    UIView *frontView = [[window subviews] objectAtIndex:0];
    id nextResponder = [frontView nextResponder];
    
    if ([nextResponder isKindOfClass:[UIViewController class]])
        result = nextResponder;
    else
        result = window.rootViewController;
    
    return result;
}
@end
