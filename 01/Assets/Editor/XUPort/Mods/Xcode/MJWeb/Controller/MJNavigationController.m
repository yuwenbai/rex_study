//
//  MJNavigationController.m
//  JSCallOCDemo
//
//  Created by developer on 2017/8/10.
//  Copyright © 2017年 kang. All rights reserved.
//

#import "MJNavigationController.h"
#import "UINavigationBar+MJCustomHeight.h"

@interface MJNavigationController ()

@end
//设置颜色
#define setColor(r, g, b) [UIColor colorWithRed:(r)/255.0 green:(g)/255.0 blue:(b)/255.0 alpha:1.0] //优聚投
//导航栏色调
//#define yjtColor setColor(100, 33, 213)
#define yjtColor setColor(0, 0, 0)
@implementation MJNavigationController

- (void)viewDidLoad {
    [super viewDidLoad];
    
    self.navigationBar.barTintColor = yjtColor;
    //    [[UINavigationBar appearance] setBarTintColor:yjtColor];
//    [self.navigationBar setBackgroundImage:[UIImage imageNamed:@"nav_bg"] forBarMetrics:UIBarMetricsDefault];
    //    [[UINavigationBar appearance] setBarTintColor:[UIColor colorWithPatternImage:[UIImage imageNamed:@"navigationbar"]]];
    //去除导航栏下方的横线
    //    [self.navigationBar setBackgroundImage:[UIImage imageWithColor:yjtColor]
    //                       forBarPosition:UIBarPositionAny
    //                           barMetrics:UIBarMetricsDefault];
    [self.navigationBar setShadowImage:[UIImage new]];
    [self.navigationBar setTitleTextAttributes:@{NSFontAttributeName:[UIFont systemFontOfSize:19],NSForegroundColorAttributeName:[UIColor whiteColor]}];
    
    //  导航栏半透明属性设置为NO,阻止导航栏遮挡view
    self.navigationBar.translucent = NO;
    
    
//    [self.navigationBar setHeight:36];
    //调整导航栏按钮的位置
//    UIBarButtonItem *leftItem = self.navigationItem.leftBarButtonItem;
//    [leftItem setBackgroundVerticalPositionAdjustment:-28 forBarMetrics:UIBarMetricsDefault];
    
//    UIBarButtonItem *rightItem = self.navigationItem.rightBarButtonItem;
//    [rightItem setBackgroundVerticalPositionAdjustment:8 forBarMetrics:UIBarMetricsDefault];
    
}


//- (BOOL) prefeprefersStatusBarHidden {
//
//    return true;
//}

- (UIStatusBarStyle) preferredStatusBarStyle
{
    
//    UIViewController* topVC = self.topViewController;
    
    return UIStatusBarStyleLightContent;
    
}

- (UIInterfaceOrientationMask)supportedInterfaceOrientations
{
    return 1 << UIInterfaceOrientationPortrait;
}

- (void)didReceiveMemoryWarning {
    [super didReceiveMemoryWarning];
    // Dispose of any resources that can be recreated.
}



@end
