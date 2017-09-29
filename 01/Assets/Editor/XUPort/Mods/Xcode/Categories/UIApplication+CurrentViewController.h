//
//  UIApplication+CurrentViewController.h
//  Unity-iPhone
//
//  Created by developer on 2017/7/25.
//
//

#import <UIKit/UIKit.h>

@interface UIApplication (CurrentViewController)

/**
 获取当前屏幕显示的viewcontroller
 
 @return 返回屏幕最上层的ViewController
 */
- (UIViewController *)getCurrentVC;

@end
