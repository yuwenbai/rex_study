//
//  MJWebViewManager.h
//  Unity-iPhone
//
//  Created by developer on 2017/8/13.
//
//

#import <Foundation/Foundation.h>

#define IOS_Version [[[UIDevice currentDevice] systemVersion] doubleValue]

typedef NS_ENUM(NSInteger, MJWebViewTransitionEdge) {
    MJWebViewTransitionEdgeNone,
    MJWebViewTransitionEdgeTop,
    MJWebViewTransitionEdgeLeft,
    MJWebViewTransitionEdgeBottom,
    MJWebViewTransitionEdgeRight
};

@class MJWebView;

@interface MJWebViewManager : NSObject <UIWebViewDelegate>

+ (instancetype) sharedManager;

-(MJWebView *) webViewWithName:(NSString *)name;

-(void) checkOrientationSupport;
-(void) addManagedWebView:(MJWebView *)webView forName:(NSString *)name ;
-(void) addManagedWebViewName:(NSString *)name insets:(UIEdgeInsets)insets ;

-(void) changeWebViewName:(NSString *)name insets:(UIEdgeInsets)insets ;

-(void) changeWebView:(MJWebView *)webView insets:(UIEdgeInsets)insets ;

-(void) webviewName:(NSString *)name beginLoadURL:(NSString *)urlString ;

-(void) webViewNameReload:(NSString *)name ;
-(void) webViewNameStop:(NSString *)name ;

-(void) webViewNameCleanCache:(NSString *)name ;

-(void) webViewNameCleanCookie:(NSString *)name forKey:(NSString *)key;

-(void) webViewName:(NSString *)name show:(BOOL)show fade:(BOOL)fade direction:(MJWebViewTransitionEdge)direction duration:(float)duration;

-(void) removeWebViewName:(NSString *)name ;

-(void) updateBackgroundWebViewName:(NSString *)name transparent:(BOOL)transparent ;

-(void) setWebViewBackgroundColorName:(NSString *)name color:(UIColor *)color ;
-(void) webViewName:(NSString *)name showToolBarAnimate:(BOOL)animate ;

-(void) webViewName:(NSString *)name hideToolBarAnimate:(BOOL)animate ;
-(bool) canGoBackWebViewName:(NSString *)name ;
-(bool) canGoForwardWebViewName:(NSString *)name ;
-(void) goBackWebViewName:(NSString *)name ;
-(void) goForwardWebViewName:(NSString *)name ;

-(void) webViewName:(NSString *)name setZoomEnable:(BOOL)enable ;
-(void) webViewName:(NSString *)name setBounces:(BOOL)bounces ;
-(void) webViewName:(NSString *)name loadHTMLString:(NSString *)htmlString baseURLString:(NSString *)baseURL ;

-(void) webViewName:(NSString *)name evaluatingJavaScript:(NSString *)javaScript shouldCallBack:(BOOL)callBack ;
-(void) performJavaScript:(NSDictionary *)info;

-(void) webViewName:(NSString *)name setSpinnerShowWhenLoading:(BOOL)show ;
-(void) webViewName:(NSString *)name setSpinnerText:(NSString *)text ;

-(NSString *) webViewName:(MJWebView *)webView ;

- (void)webViewName:(NSString *)name addUrlScheme:(NSString *)scheme ;

- (void)webViewName:(NSString *)name removeUrlScheme:(NSString *)scheme ;

- (void)webViewDidStartLoad:(MJWebView *)webView ;
- (void)webViewDidFinishLoad:(MJWebView *)webView ;

- (void)webView:(MJWebView *)webView didFailLoadWithError:(NSError *)error ;

- (void)webViewDone:(MJWebView *)webView ;

-(NSString *) webViewNameGetCurrentUrl:(NSString *)name ;


#pragma mark - webView Unity

-(void) videoEnterFullScreen:(NSNotification *)noti ;
-(NSString *) webViewGetUserAgent:(NSString *)name;

-(void) webViewSetUserAgent:(NSString *)userAgent ;

-(float) webViewNameGetAlpha:(NSString *)name;
-(void) webViewName:(NSString *)name setAlpha:(float)alpha ;
-(void) webViewName:(NSString *)name setValue:(NSString *)value forHeaderField:(NSString *)key ;

-(void) webViewName:(NSString *)name setVerticalScrollBarShow:(BOOL)show ;
-(void) webViewName:(NSString *)name setHorizontalScrollBarShow:(BOOL)show ;

#pragma mark -
- (void) goBackWebView:(MJWebView *) webView ;
- (void) closeWebView:(MJWebView *) webView ;
@end
