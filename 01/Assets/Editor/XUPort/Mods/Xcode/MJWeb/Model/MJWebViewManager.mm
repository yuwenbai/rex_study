//
//  MJWebViewManager.m
//  Unity-iPhone
//
//  Created by developer on 2017/8/13.
//
//

#import "MJWebViewManager.h"
#import "MJWebView.h"
#include "OrientationSupport.h"
#import "UnityInterface.h"
#import "MJWebViewController.h"
#import "MJNavigationController.h"
#import "UnityAppController.h"


#define SCREEN_W [UIScreen mainScreen].bounds.size.width
#define SCREEN_H [UIScreen mainScreen].bounds.size.height

NSString * const UWVPlayerWillExitFullScreenNoti = @"UIMoviePlayerControllerWillExitFullscreenNotification";
NSString * const UWVPlayerDidEnterFullScreenNoti = @"UIMoviePlayerControllerDidEnterFullscreenNotification";


@interface NSUserDefaults(UnRegisterDefaults)
- (void)uwv_unregisterDefaultForKey:(NSString *)defaultName;
@end

@implementation NSUserDefaults (UnRegisterDefaults)

- (void)uwv_unregisterDefaultForKey:(NSString *)defaultName {
    NSDictionary *registeredDefaults = [[NSUserDefaults standardUserDefaults] volatileDomainForName:NSRegistrationDomain];
    if ([registeredDefaults objectForKey:defaultName] != nil) {
        NSMutableDictionary *mutableCopy = [NSMutableDictionary dictionaryWithDictionary:registeredDefaults];
        [mutableCopy removeObjectForKey:defaultName];
        [self uwv_replaceRegisteredDefaults:mutableCopy];
    }
}

- (void)uwv_replaceRegisteredDefaults:(NSDictionary *)dictionary {
    [[NSUserDefaults standardUserDefaults] setVolatileDomain:dictionary forName:NSRegistrationDomain];
}

@end


@interface MJWebViewManager() {
    NSMutableDictionary *_webViewDic;
    ScreenOrientation _orientationBeforeFullScreen;
    BOOL _multipleOrientation;
}

- (void)webViewDone:(MJWebView *)webView;
@end


@implementation MJWebViewManager

#pragma mark - init
+ (instancetype)allocWithZone:(struct _NSZone *)zone
{
    static dispatch_once_t onceToken;
    static MJWebViewManager *mjWebViewManager = nil;
    dispatch_once(&onceToken, ^{
        mjWebViewManager = [super allocWithZone:zone];
        mjWebViewManager->_webViewDic = [[NSMutableDictionary alloc]init];
    });
    return mjWebViewManager;
}

+ (instancetype) sharedManager {
    return [self allocWithZone:nil];
}

- (id) copyWithZone:(NSZone *)zone;{
    return self;
}

//-(instancetype) init {
//    self = [super init];
//    if (self) {
//        _webViewDic = [[NSMutableDictionary alloc] init];
//        [[NSNotificationCenter defaultCenter] addObserver:self selector:@selector(videoExitFullScreen:) name:UWVPlayerWillExitFullScreenNoti object:nil];
//        [[NSNotificationCenter defaultCenter] addObserver:self selector:@selector(videoEnterFullScreen:) name:UWVPlayerDidEnterFullScreenNoti object:nil];
//        [self checkOrientationSupport];
//    }
//    return self;
//}

#pragma mark - MJWebView

- (void) openWebViewControllerWithWebView:(MJWebView *)webView {
    
    MJWebViewController *webViewController = [[MJWebViewController alloc]init];
    [webViewController setWebView:webView];
    webView.mjDelegate = webViewController;
    
    MJNavigationController *navViewController = [[MJNavigationController alloc]initWithRootViewController:webViewController];
    UnityAppController *appDelegate = (UnityAppController*)GetAppController();
    [appDelegate.rootViewController presentViewController:navViewController animated:YES completion:nil];
    
}


-(MJWebView *) webViewWithName:(NSString *)name {
    return [_webViewDic objectForKey:name];
}

-(void) checkOrientationSupport {
    NSArray *arr = [[[NSBundle mainBundle] infoDictionary] objectForKey:@"UISupportedInterfaceOrientations"];
    __block BOOL portraitOrientation = NO;
    __block BOOL landspaceOrientation = NO;
    
    [arr enumerateObjectsUsingBlock:^(NSString *orientation, NSUInteger idx, BOOL *stop) {
        if ([orientation rangeOfString:@"Portrait"].location != NSNotFound) {
            portraitOrientation = YES;
        } else if ([orientation rangeOfString:@"Landscape"].location != NSNotFound) {
            landspaceOrientation = YES;
        }
        
        if (portraitOrientation && landspaceOrientation) {
            _multipleOrientation = YES;
            *stop = YES;
        }
    }];
}

-(void) addManagedWebView:(MJWebView *)webView forName:(NSString *)name {
    if (![_webViewDic objectForKey:name]) {
        [_webViewDic setObject:webView forKey:name];
    } else {
        NSLog(@"Duplicated name. Something goes wrong: %@", name);
    }
}

-(void) addManagedWebViewName:(NSString *)name insets:(UIEdgeInsets)insets {
    
    NSLog(@"WebView name:%@",name);
    
    MJWebView *webView = [[MJWebView alloc] initWithFrame:CGRectMake(0, 0, SCREEN_W, SCREEN_H-64)];
//    webView.mediaPlaybackRequiresUserAction = NO;
    webView.delegate = self;
    [self addManagedWebView:webView forName:name];
    [self openWebViewControllerWithWebView:webView];
    NSLog(@"WebView name:%@",name);
}

-(void) changeWebViewName:(NSString *)name insets:(UIEdgeInsets)insets {
//    MJWebView *webView = [_webViewDic objectForKey:name];
//    [self changeWebView:webView insets:insets];
}

-(void) changeWebView:(MJWebView *)webView insets:(UIEdgeInsets)insets {
//    [webView changeToInsets:insets targetOrientation:UnityCurrentOrientation()];
}

-(void) webviewName:(NSString *)name beginLoadURL:(NSString *)urlString {
    MJWebView *webView = [_webViewDic objectForKey:name];
    urlString=[urlString stringByAddingPercentEscapesUsingEncoding:NSUTF8StringEncoding];
    NSURL *url = [NSURL URLWithString:urlString];
    NSMutableURLRequest *request = [NSMutableURLRequest requestWithURL:url];
    
    for (NSString *key in webView.headers.allKeys) {
        [request setValue:webView.headers[key] forHTTPHeaderField:key];
    }
    
    [webView loadRequest:request];
}

-(void) webViewNameReload:(NSString *)name {
    MJWebView *webView = [_webViewDic objectForKey:name];
    [webView reload];
}

-(void) webViewNameStop:(NSString *)name {
    MJWebView *webView = [_webViewDic objectForKey:name];
    if ([webView isLoading]) {
        [webView stopLoading];
    }
}

-(void) webViewNameCleanCache:(NSString *)name {
    MJWebView *webView = [_webViewDic objectForKey:name];
    [[NSURLCache sharedURLCache] removeCachedResponseForRequest:webView.request];
    [[NSURLCache sharedURLCache] removeAllCachedResponses];
}

-(void) webViewNameCleanCookie:(NSString *)name forKey:(NSString *)key {
    
    NSHTTPCookie *cookie;
    NSHTTPCookieStorage *cookieJar = [NSHTTPCookieStorage sharedHTTPCookieStorage];
    
    if (key.length) {
        NSLog(@"Removing cookie for %@", key);
        for (cookie in [cookieJar cookies]) {
            if ([cookie.name isEqualToString:key]) {
                [cookieJar deleteCookie:cookie];
                NSLog(@"Found cookie for %@, removed.", key);
            }
        }
    } else {
        NSLog(@"Removing all cookies");
        for (cookie in [cookieJar cookies]) {
            [cookieJar deleteCookie:cookie];
        }
    }
    
    [[NSUserDefaults standardUserDefaults] synchronize];
}

-(void) webViewName:(NSString *)name show:(BOOL)show fade:(BOOL)fade direction:(MJWebViewTransitionEdge)direction duration:(float)duration {
    
    MJWebView *webView = [_webViewDic objectForKey:name];
    
    if (webView.viewAnimating) {
        NSLog(@"Trying to animate but another transition animation is not finished yet. Ignore this one.");
        return;
    }
    
    if (fade || direction != MJWebViewTransitionEdgeNone) {
        webView.viewAnimating = YES;
        
        webView.alpha = 1.0;
        if (fade) {
            webView.alpha = show ? 0.0 : 1.0;
        }
        
        int x, y;
        CGRect rect = webView.frame;
        CGSize screenSize = UnityGetGLView().bounds.size;
        switch (direction) {
            case MJWebViewTransitionEdgeTop:
                x = rect.origin.x; y = -rect.size.height;
                break;
            case MJWebViewTransitionEdgeLeft:
                x = -rect.size.width; y = rect.origin.y;
                break;
            case MJWebViewTransitionEdgeBottom:
                x = rect.origin.x; y = screenSize.height;
                break;
            case MJWebViewTransitionEdgeRight:
                x = screenSize.width; y = rect.origin.y;
                break;
            default:
                x = rect.origin.x; y = rect.origin.y;
                break;
        }
        
        CGRect destinationRect;
        if (show) {
            webView.hidden = NO;
            destinationRect = webView.frame;
            webView.frame = CGRectMake(x, y, webView.frame.size.width, webView.frame.size.height);
        } else {
            destinationRect = CGRectMake(x, y, webView.frame.size.width, webView.frame.size.height);
        }
        
        [UIView animateWithDuration:duration animations:^{
            webView.alpha = fade ? (show ? 1.0 : 0.0) : 1.0;
            webView.frame = destinationRect;
        } completion:^(BOOL finished) {
            
            if (!show) {
                webView.hidden = YES;
            }
            
            webView.viewAnimating = NO;
            UnitySendMessage([name UTF8String], show ? "ShowTransitionFinished" : "HideTransitionFinished", "");
        }];
        
    } else {
        webView.hidden = !show;
        webView.alpha = 1.0;
        UnitySendMessage([name UTF8String], show ? "ShowTransitionFinished" : "HideTransitionFinished", "");
    }
    
    if (!show) {
        [webView endEditing:YES];
        [webView.spinner hide];
    }
}

-(void) removeWebViewName:(NSString *)name {
    MJWebView *webView = [_webViewDic objectForKey:name];
    webView.delegate = nil;
    [webView removeFromSuperview];
    [webView.toolBar removeFromSuperview];
    [webView.spinner removeFromSuperview];
    [_webViewDic removeObjectForKey:name];
}

-(void) updateBackgroundWebViewName:(NSString *)name transparent:(BOOL)transparent {
    UIColor *color = transparent ? [UIColor clearColor] : [UIColor whiteColor];
    [self setWebViewBackgroundColorName:name color:color];
}

-(void) setWebViewBackgroundColorName:(NSString *)name color:(UIColor *)color {
    MJWebView *webView = [_webViewDic objectForKey:name];
    CGFloat red = 0.0, green = 0.0, blue = 0.0, alpha =0.0;
    [color getRed:&red green:&green blue:&blue alpha:&alpha];
    
    webView.opaque = NO;
    webView.backgroundColor = color;
    
    for (UIView* subView in [webView subviews]) {
        if ([subView isKindOfClass:[UIScrollView class]]) {
            for (UIView* shadowView in [subView subviews]) {
                if ([shadowView isKindOfClass:[UIImageView class]]) {
                    [shadowView setHidden:true];
                }
            }
        }
    }
}

-(void) webViewName:(NSString *)name showToolBarAnimate:(BOOL)animate {
    MJWebView *webView = [_webViewDic objectForKey:name];
    if (webView.toolBar.hidden) {
        if (animate) {
            CGRect oldFrame = webView.toolBar.frame;
            webView.toolBar.frame = CGRectOffset(oldFrame, 0, oldFrame.size.height);
            webView.toolBar.hidden = NO;
            [UIView animateWithDuration:0.4 animations:^{
                webView.toolBar.frame = oldFrame;
            }];
        } else {
            webView.toolBar.hidden = NO;
        }
    }
}

-(void) webViewName:(NSString *)name hideToolBarAnimate:(BOOL)animate {
    MJWebView *webView = [_webViewDic objectForKey:name];
    if (!webView.toolBar.hidden) {
        if (animate) {
            CGRect oldFrame = webView.toolBar.frame;
            [UIView animateWithDuration:0.4 animations:^{
                webView.toolBar.frame = CGRectOffset(oldFrame, 0, oldFrame.size.height);
            } completion:^(BOOL finished) {
                webView.toolBar.hidden = YES;
                webView.toolBar.frame = oldFrame;
            }];
        } else {
            webView.toolBar.hidden = YES;
        }
    }
}

-(bool) canGoBackWebViewName:(NSString *)name {
    MJWebView *webView = [_webViewDic objectForKey:name];
    return [webView canGoBack] ? true : false;
}

-(bool) canGoForwardWebViewName:(NSString *)name {
    MJWebView *webView = [_webViewDic objectForKey:name];
    return [webView canGoForward] ? true : false;
}

-(void) goBackWebViewName:(NSString *)name {
    MJWebView *webView = [_webViewDic objectForKey:name];
    [webView goBack];
}

-(void) goForwardWebViewName:(NSString *)name {
    MJWebView *webView = [_webViewDic objectForKey:name];
    [webView goForward];
}

-(void) webViewName:(NSString *)name setZoomEnable:(BOOL)enable {
    MJWebView *webView = [_webViewDic objectForKey:name];
    webView.scalesPageToFit = enable;
}

-(void) webViewName:(NSString *)name setBounces:(BOOL)bounces {
    MJWebView *webView = [_webViewDic objectForKey:name];
    [webView setBounces:bounces];
}

-(void) webViewName:(NSString *)name loadHTMLString:(NSString *)htmlString baseURLString:(NSString *)baseURL {
    MJWebView *webView = [_webViewDic objectForKey:name];
    [webView loadHTMLString:htmlString baseURL:[NSURL URLWithString:baseURL]];
}

-(void) webViewName:(NSString *)name evaluatingJavaScript:(NSString *)javaScript shouldCallBack:(BOOL)callBack {
    NSDictionary *info = @{@"js": javaScript, @"callback": @(callBack), @"name": name};
    [self performSelectorOnMainThread:@selector(performJavaScript:) withObject:info waitUntilDone:NO];
}

-(void) performJavaScript:(NSDictionary *)info {
    
    NSString *name = info[@"name"];
    NSString *js = info[@"js"];
    BOOL callback = [info[@"callback"] boolValue];
    
    MJWebView *webView = [_webViewDic objectForKey:info[@"name"]];
    NSString *result = [webView stringByEvaluatingJavaScriptFromString:js];
    if (callback) {
        UnitySendMessage([name UTF8String], "EvalJavaScriptFinished", [result UTF8String]);
    }
}

-(void) webViewName:(NSString *)name setSpinnerShowWhenLoading:(BOOL)show {
    MJWebView *webView = [_webViewDic objectForKey:name];
    webView.showSpinnerWhenLoading = show;
}

-(void) webViewName:(NSString *)name setSpinnerText:(NSString *)text {
    MJWebView *webView = [_webViewDic objectForKey:name];
    if (text) {
        webView.spinner.textLabel.text = text;
    }
}

-(NSString *) webViewName:(MJWebView *)webView {
    NSString *webViewName = [[_webViewDic allKeysForObject:webView] lastObject];
    if (!webViewName) {
        NSLog(@"Did not find the webview: %@",webViewName);
    }
    return webViewName;
}

- (void)webViewName:(NSString *)name addUrlScheme:(NSString *)scheme {
    MJWebView *webView = [_webViewDic objectForKey:name];
    if (![webView.schemes containsObject:scheme]) {
        [webView.schemes addObject:scheme];
    }
}

- (void)webViewName:(NSString *)name removeUrlScheme:(NSString *)scheme {
    MJWebView *webView = [_webViewDic objectForKey:name];
    if ([webView.schemes containsObject:scheme]) {
        [webView.schemes removeObject:scheme];
    }
}

- (void)webViewDidStartLoad:(MJWebView *)webView {
    if (webView.showSpinnerWhenLoading && !webView.hidden) {
        [webView.spinner show];
    }
}

- (void)webViewDidFinishLoad:(MJWebView *)webView {
    [webView.spinner hide];
    NSString *webViewName = [self webViewName:webView];
    [webView updateToolBtn];
    
    webView.currentUrl = webView.request.mainDocumentURL.absoluteString;
    UnitySendMessage([webViewName UTF8String], "LoadComplete", "");
}


- (void)webView:(MJWebView *)webView didFailLoadWithError:(NSError *)error {
    [webView.spinner hide];
    NSString *webViewName = [self webViewName:webView];
    [webView updateToolBtn];
    
    webView.currentUrl = webView.request.mainDocumentURL.absoluteString;
    UnitySendMessage([webViewName UTF8String], "LoadComplete", [error.localizedDescription UTF8String]);
}

- (void)webViewDone:(MJWebView *)webView {
    [webView.spinner hide];
    NSString *webViewName = [self webViewName:webView];
    UnitySendMessage([webViewName UTF8String], "WebViewDone", "");
}

-(NSString *) webViewNameGetCurrentUrl:(NSString *)name {
    MJWebView *webView = [_webViewDic objectForKey:name];
    return webView.currentUrl ?: @"";
}


#pragma mark - webView Delegate
-(BOOL)webView:(MJWebView *)webView shouldStartLoadWithRequest:(NSURLRequest *)request navigationType:(UIWebViewNavigationType)navigationType {
    
//    webView.currentUrl = [NSString stringWithFormat:@"%@",request.URL];
    NSLog(@"webView url:%@",[request.URL absoluteString]);
    
    NSString *webViewName = [self webViewName:webView];
    __block BOOL canResponse = NO;
    [webView.schemes enumerateObjectsUsingBlock:^(NSString *scheme, NSUInteger idx, BOOL *stop) {
        if ([[request.URL absoluteString] rangeOfString:[scheme stringByAppendingString:@"://"]].location == 0) {
            canResponse = YES;
            *stop = YES;
        }
    }];
    
    if (canResponse) {
        NSString *rawMessage = [NSString stringWithFormat:@"%@",request.URL];
        if ([rawMessage hasPrefix:@"uniwebview://close"] || [rawMessage isEqualToString:@"uniwebview://root?action=close"]) {
            [webView.mjDelegate dismissMJWebViewController];
            [self closeWebView:webView];
            return NO;
        }
        UnitySendMessage([webViewName UTF8String], "ReceivedMessage", [rawMessage UTF8String]);
        return NO;
    } else {
        if (navigationType == UIWebViewNavigationTypeLinkClicked && webView.openLinkInExternalBrowser) {
            [[UIApplication sharedApplication] openURL:request.URL];
            return NO;
        } else {
            UnitySendMessage([webViewName UTF8String], "LoadBegin", [request.URL.absoluteString UTF8String]);
        }
    }
    
    return YES;
}






-(void)scrollViewDidScroll:(UIScrollView *)scrollView
{
    CGPoint point = scrollView.contentOffset;
    if (point.x > 0) {
        scrollView.contentOffset = CGPointMake(0, point.y);//这里不要设置为CGPointMake(0, 0)，这样我们在文章下面左右滑动的时候，就跳到文章的起始位置，不科学
    }
}

#pragma mark - webView Unity

-(void) videoEnterFullScreen:(NSNotification *)noti {
    UIInterfaceOrientation toInterfaceOrientation = [UIApplication sharedApplication].statusBarOrientation;
    _orientationBeforeFullScreen = ConvertToUnityScreenOrientation(toInterfaceOrientation);
}

-(void) videoExitFullScreen:(NSNotification *)noti {
    
    ScreenOrientation orientation = portrait;
    
    if (_multipleOrientation) {
        UIInterfaceOrientation toInterfaceOrientation = [UIApplication sharedApplication].statusBarOrientation;
        orientation = ConvertToUnityScreenOrientation(toInterfaceOrientation);
    } else {
        orientation = UnityCurrentOrientation();
    }
    
    if (_orientationBeforeFullScreen == landscapeLeft || _orientationBeforeFullScreen == landscapeRight) {
        if (orientation == portrait) {
            orientation = _orientationBeforeFullScreen;
        } else {
            orientation = portrait;
        }
    }
    
//    [_webViewDic enumerateKeysAndObjectsUsingBlock:^(id key, id obj, BOOL *stop) {
////        MJWebView *webView = (MJWebView *)obj;
////        [webView changeToInsets:webView.insets targetOrientation:orientation];
//    }];
}

-(NSString *) webViewGetUserAgent:(NSString *)name {
    MJWebView *webView = [_webViewDic objectForKey:name];
    return [webView stringByEvaluatingJavaScriptFromString:@"window.navigator.userAgent"];
}

-(void) webViewSetUserAgent:(NSString *)userAgent {
    [[NSUserDefaults standardUserDefaults] uwv_unregisterDefaultForKey:@"UserAgent"];
    
    if (userAgent.length != 0) {
        NSDictionary *dictionary = [NSDictionary dictionaryWithObjectsAndKeys:userAgent, @"UserAgent", nil];
        [[NSUserDefaults standardUserDefaults] registerDefaults:dictionary];
    }
}

-(float) webViewNameGetAlpha:(NSString *)name {
    MJWebView *webView = [_webViewDic objectForKey:name];
    return webView.alpha;
}

-(void) webViewName:(NSString *)name setAlpha:(float)alpha {
    MJWebView *webView = [_webViewDic objectForKey:name];
    webView.alpha = alpha;
}

-(void) webViewName:(NSString *)name setValue:(NSString *)value forHeaderField:(NSString *)key {
    MJWebView *webView = [_webViewDic objectForKey:name];
    if (value.length == 0) {
        [webView.headers removeObjectForKey:key];
    } else if (key.length != 0) {
        webView.headers[key] = value;
    }
}

-(void) webViewName:(NSString *)name setVerticalScrollBarShow:(BOOL)show {
    MJWebView *webView = [_webViewDic objectForKey:name];
    webView.scrollView.showsVerticalScrollIndicator = show;
}

-(void) webViewName:(NSString *)name setHorizontalScrollBarShow:(BOOL)show {
    MJWebView *webView = [_webViewDic objectForKey:name];
    webView.scrollView.showsHorizontalScrollIndicator = show;
}

#pragma mark - 

- (void) goBackWebView:(MJWebView *) webView {
    
    NSString *rawMessage = [NSString stringWithFormat:@"%@",@"mjwebView://root?action=close"];
    NSString *webViewName = [self webViewName:webView];
    UnitySendMessage([webViewName UTF8String], "ReceivedMessage", [rawMessage UTF8String]);
    
}

- (void) closeWebView:(MJWebView *) webView {

    NSString *rawMessage = [NSString stringWithFormat:@"%@",@"mjwebView://root?action=close"];
    NSString *webViewName = [self webViewName:webView];
    UnitySendMessage([webViewName UTF8String], "ReceivedMessage", [rawMessage UTF8String]);
}

@end
