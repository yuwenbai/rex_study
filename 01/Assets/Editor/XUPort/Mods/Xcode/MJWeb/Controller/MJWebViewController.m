//
//  MJWebViewController.m
//  JSCallOCDemo
//
//  Created by developer on 2017/8/10.
//  Copyright © 2017年 kang. All rights reserved.
//

#import "MJWebViewController.h"
#import "MJWebViewManager.h"

//#import "UIImage+iconfont.h"
//#include "Libraries/Plugins/iOS/UniWebView.mm"
//@class UniWebView;
@interface MJWebViewController ()

@property (nonatomic, strong) WKWebView *wkwebView;
@end


@implementation MJWebViewController

- (instancetype) initWithUrl:(NSString *)url {
    
    self = [super init];
    if(self) {
        self.url = url;
        NSLog(@"webview url:%@",url);
    }
    return self;
}

- (instancetype) initWithWebView:(MJWebView *)webView {
    
    self = [super init];
    if(self) {
        self.webView = webView;
    }
    return self;
}


- (void)viewDidLoad {
    [super viewDidLoad];
    [self setNavigationbar];
    [self setSubViews];
    
    self.view.backgroundColor = [UIColor whiteColor];
    
}

- (void)didReceiveMemoryWarning {
    [super didReceiveMemoryWarning];
    // Dispose of any resources that can be recreated.
}


- (UIInterfaceOrientationMask)supportedInterfaceOrientations
{
    return 1 << UIInterfaceOrientationPortrait;
}


- (void) setNavigationbar {
    
//    [ self.navigationItem.leftBarButtonItem setBackgroundVerticalPositionAdjustment:8 forBarMetrics:UIBarMetricsDefault];
//    [self.navigationItem.rightBarButtonItem setBackgroundVerticalPositionAdjustment:8 forBarMetrics:UIBarMetricsDefault];
    
//    UIImage *backimage = [UIImage imageNamed:@"nav_back"];
//    UIImage *closeImage = [UIImage imageNamed:@"nav_close"];
   
//    UIButton *backBtn = [UIButton buttonWithType:UIButtonTypeCustom];
//    backBtn.backgroundColor = [UIColor greenColor];
//    [backBtn setBackgroundImage:backimage forState:UIControlStateNormal];
//    [backBtn setFrame:CGRectMake(0, 0, 44, 44)];
////    [backBtn setImageEdgeInsets:UIEdgeInsetsMake(12, -5, 4, 33)];
//    [backBtn addTarget:self action:@selector(back:) forControlEvents:UIControlEventTouchUpInside];
//    
//    UIButton *closeBtn = [UIButton buttonWithType:UIButtonTypeCustom];
//    closeBtn.backgroundColor = [UIColor greenColor];
//    [closeBtn setBackgroundImage:closeImage forState:UIControlStateNormal];
//    [closeBtn setFrame:CGRectMake(0, 0, 44, 44)];
//    [closeBtn addTarget:self action:@selector(close:) forControlEvents:UIControlEventTouchUpInside];
//    
//    //修改方法
//    UIView *backView = [[UIView alloc] initWithFrame:CGRectMake(8.0, 0.0, 44.0, 44.0)];
//    backBtn.backgroundColor = [UIColor redColor];
//    [backView addSubview:backBtn];
//    
//    //修改方法
//    UIView *closeView = [[UIView alloc] initWithFrame:CGRectMake(8.0, 0.0, 44.0, 44.0)];
//    [closeView addSubview:closeBtn];
//    
//    UIBarButtonItem *backItem = [[UIBarButtonItem alloc]initWithCustomView:backView];
//    UIBarButtonItem *closeItem = [[UIBarButtonItem alloc]initWithCustomView:closeView];
    
    
    UIBarButtonItem *backItem = [[UIBarButtonItem alloc]initWithImage:[[UIImage imageNamed:@"nav_back_big"] imageWithRenderingMode:UIImageRenderingModeAlwaysOriginal] style:UIBarButtonItemStyleDone target:self action:@selector(back:)];
    UIBarButtonItem *closeItem = [[UIBarButtonItem alloc]initWithImage:[[UIImage imageNamed:@"nav_close_big"] imageWithRenderingMode:UIImageRenderingModeAlwaysOriginal] style:UIBarButtonItemStyleDone target:self action:@selector(close:)];
    
//    UIBarButtonItem *backItem = [[UIBarButtonItem alloc]initWithImage:[[UIImage imageNamed:@"nav_back"] imageWithRenderingMode:UIImageRenderingModeAlwaysOriginal] style:UIBarButtonItemStyleDone target:self action:@selector(back:)];
//    UIBarButtonItem *closeItem = [[UIBarButtonItem alloc]initWithImage:[[UIImage imageNamed:@"nav_close"] imageWithRenderingMode:UIImageRenderingModeAlwaysOriginal] style:UIBarButtonItemStyleDone target:self action:@selector(close:)];
    
    backItem.imageInsets = UIEdgeInsetsMake(0, -5, 0, -33);
    closeItem.imageInsets = UIEdgeInsetsMake(0, -23, 0, 5);
    self.navigationItem.leftBarButtonItem = backItem;
    self.navigationItem.rightBarButtonItem = closeItem;
    
//    NSString *titleStringIndex = [NSString stringWithFormat:@"客服%ld",self.navigationController.viewControllers.count];
//    self.title = titleStringIndex;
}

- (void) setSubViews {
    
//    if (self.url) {
//        self.webView = [[MJWebView alloc]initWithFrame:CGRectMake(0, 0, self.view.frame.size.width, self.view.frame.size.height-64)];
//        self.webView.currentUrl = self.url;
//    }
//    
    if (self.webView) {
        self.webView.frame = CGRectMake(0, 0, self.view.frame.size.width, self.view.frame.size.height-64);
        self.webView.delegate = [MJWebViewManager sharedManager];
//        self.webView.scrollView.contentInset = UIEdgeInsetsMake(0, 0, 0, 0);
//        self.webView.scalesPageToFit = NO;
//        self.webView.scrollView.showsHorizontalScrollIndicator = NO;
        [self.view addSubview:self.webView];
//        NSLog(@"setSubViews webView url:%@",self.webView.currentUrl);
//        NSURLRequest *request = [NSURLRequest requestWithURL:[NSURL URLWithString:self.webView.currentUrl]];
//        [self.webView loadRequest:request];
    }
    
    
//    UIButton *button = [UIButton buttonWithType:UIButtonTypeCustom];
//    button.frame = self.view.frame;
//    [button addTarget:self action:@selector(buttonClicked:) forControlEvents:UIControlEventTouchUpInside];
//    [button setTitle:@"下一页" forState:UIControlStateNormal];
//    [button setTitleColor:[UIColor blackColor] forState:UIControlStateNormal];
//    [self.webView addSubview:button];
    
}



- (void) back:(id)sender {

    if ([self.webView canGoBack] ) {
        [self.webView goBack];
    }else {
        [[MJWebViewManager sharedManager] goBackWebView:self.webView];
        [self dismissMJWebViewController];
    }
}

- (void) close:(id) sender {

    if (self.navigationController) {
         [[MJWebViewManager sharedManager] closeWebView:self.webView];
        [self dismissMJWebViewController];
    }
}


- (void) dismissMJWebViewController {
    
    [self.navigationController dismissViewControllerAnimated:YES completion:nil];
}

- (void) buttonClicked:(id) sender {
    
    MJWebViewController *webViewController = [[MJWebViewController alloc]init];
    [self.navigationController pushViewController:webViewController animated:YES];
}


#pragma mark - webView
//- (void) cleanCache {
//
//}
//- (void) cleanCookie {
//
//}
//- (void) load {
//
//}


@end
