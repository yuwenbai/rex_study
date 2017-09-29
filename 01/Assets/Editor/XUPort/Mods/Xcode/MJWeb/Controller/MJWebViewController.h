//
//  MJWebViewController.h
//  JSCallOCDemo
//
//  Created by developer on 2017/8/10.
//  Copyright © 2017年 kang. All rights reserved.
//

#import <UIKit/UIKit.h>
#import <WebKit/WebKit.h>
#import "MJWebView.h"

@interface MJWebViewController : UIViewController <MJWebViewDelegate>

@property (nonatomic, copy) NSString *url;
@property (nonatomic, strong) MJWebView *webView;

- (instancetype) initWithUrl:(NSString *)url;

- (instancetype) initWithWebView:(MJWebView *)webView;

- (void) dismissMJWebViewController;

- (void) cleanCache;
- (void) cleanCookie;
- (void) load;

@end
