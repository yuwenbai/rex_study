//
//  UnityWebViewFunc.m
//  Unity-iPhone
//
//  Created by developer on 2017/8/13.
//
//

#import <Foundation/Foundation.h>
#import "MJWebViewManager.h"
#import "MJWebView.h"
#import "UnityAppController.h"
#import "UnityInterface.h"

// Helper method to create C string copy
NSString* MJWebViewMakeNSString (const char* string) {
    if (string) {
        return [NSString stringWithUTF8String: string];
    } else {
        return [NSString stringWithUTF8String: ""];
    }
}

char* MJWebViewMakeCString(NSString *str) {
    const char* string = [str UTF8String];
    if (string == NULL) {
        return NULL;
    }
    
    char* res = (char*)malloc(strlen(string) + 1);
    strcpy(res, string);
    return res;
}


//#if defined (_cplusplus)
extern "C"
{
    void _UniWebViewInit(const char *name, int top, int left, int bottom, int right) {
        UIEdgeInsets insets = UIEdgeInsetsMake(top, left, bottom, right);
        [[MJWebViewManager sharedManager] addManagedWebViewName:MJWebViewMakeNSString(name) insets:insets];
    }
    
    void _UniWebViewChangeInsets(const char *name, int top, int left, int bottom, int right) {
//        UIEdgeInsets insets = UIEdgeInsetsMake(top, left, bottom, right);
//        [[MJWebViewManager sharedManager] changeWebViewName:MJWebViewMakeNSString(name) insets:insets];
    }
    
    void _UniWebViewLoad(const char *name, const char *url) {
        [[MJWebViewManager sharedManager] webviewName:MJWebViewMakeNSString(name)
                                          beginLoadURL:MJWebViewMakeNSString(url)];
    }
    
    void _UniWebViewReload(const char *name) {
        [[MJWebViewManager sharedManager] webViewNameReload:MJWebViewMakeNSString(name)];
    }
    
    void _UniWebViewStop(const char *name) {
        [[MJWebViewManager sharedManager] webViewNameStop:MJWebViewMakeNSString(name)];
    }
    
    void _UniWebViewShow(const char *name, bool fade, int direction, float duration) {
        [[MJWebViewManager sharedManager] webViewName:MJWebViewMakeNSString(name) show:YES fade:fade direction:(MJWebViewTransitionEdge)direction duration:duration];
    }
    
    void _UniWebViewHide(const char *name, bool fade, int direction, float duration) {
        [[MJWebViewManager sharedManager] webViewName:MJWebViewMakeNSString(name) show:NO fade:fade direction:(MJWebViewTransitionEdge)direction duration:duration];
    }
    
    void _UniWebViewCleanCache(const char *name) {
        [[MJWebViewManager sharedManager] webViewNameCleanCache:MJWebViewMakeNSString(name)];
    }
    
    void _UniWebViewCleanCookie(const char *name, const char *key) {
        [[MJWebViewManager sharedManager] webViewNameCleanCookie:MJWebViewMakeNSString(name) forKey:MJWebViewMakeNSString(key)];
    }
    
    void _UniWebViewDestroy(const char *name) {
        [[MJWebViewManager sharedManager] removeWebViewName:MJWebViewMakeNSString(name)];
    }
    
    void _UniWebViewTransparentBackground(const char *name, BOOL transparent) {
        [[MJWebViewManager sharedManager] updateBackgroundWebViewName:MJWebViewMakeNSString(name) transparent:transparent];
    }
    
    void _UniWebViewSetBackgroundColor(const char *name, float r, float g, float b, float a) {
        UIColor *color = [UIColor colorWithRed:r green:g blue:b alpha:a];
        [[MJWebViewManager sharedManager] setWebViewBackgroundColorName:MJWebViewMakeNSString(name) color:color];
    }
    
    void _UniWebViewShowToolBar(const char *name, bool animate) {
        [[MJWebViewManager sharedManager] webViewName:MJWebViewMakeNSString(name) showToolBarAnimate:animate];
    }
    void _UniWebViewHideToolBar(const char *name, bool animate) {
        [[MJWebViewManager sharedManager] webViewName:MJWebViewMakeNSString(name) hideToolBarAnimate:animate];
    }
    
    bool _UniWebViewCanGoBack(const char *name) {
        return [[MJWebViewManager sharedManager] canGoBackWebViewName:MJWebViewMakeNSString(name)];
//        return NO;
    }
    
    bool _UniWebViewCanGoForward(const char *name) {
        return [[MJWebViewManager sharedManager] canGoForwardWebViewName:MJWebViewMakeNSString(name)];
//         return NO;
    }
    
    void _UniWebViewGoBack(const char *name) {
        [[MJWebViewManager sharedManager] goBackWebViewName:MJWebViewMakeNSString(name)];
    }
    
    void _UniWebViewGoForward(const char *name) {
        [[MJWebViewManager sharedManager] goForwardWebViewName:MJWebViewMakeNSString(name)];
    }
    
    void _UniWebViewLoadHTMLString(const char *name, const char *html, const char *baseUrl) {
        [[MJWebViewManager sharedManager] webViewName:MJWebViewMakeNSString(name)
                                        loadHTMLString:MJWebViewMakeNSString(html)
                                         baseURLString:MJWebViewMakeNSString(baseUrl)];
    }
    
    void _UniWebViewSetBounces(const char *name, bool bounces) {
        [[MJWebViewManager sharedManager] webViewName:MJWebViewMakeNSString(name) setBounces:bounces];
    }
    
    void _UniWebViewEvaluatingJavaScript(const char *name, const char *javascript, BOOL callback) {
        NSString *webViewName = MJWebViewMakeNSString(name);
        NSString *jsString = MJWebViewMakeNSString(javascript);
        NSLog(@"webViewName:%@, eval js:%@",webViewName,jsString);
        [[MJWebViewManager sharedManager] webViewName:webViewName evaluatingJavaScript:jsString shouldCallBack:callback];
    }
    
    void _UniWebViewSetSpinnerShowWhenLoading(const char *name, bool show) {
        [[MJWebViewManager sharedManager] webViewName:MJWebViewMakeNSString(name)
                             setSpinnerShowWhenLoading:show];
    }
    
    void _UniWebViewSetSpinnerText(const char *name, const char *text) {
        [[MJWebViewManager sharedManager] webViewName:MJWebViewMakeNSString(name)
                                        setSpinnerText:MJWebViewMakeNSString(text)];
    }
    
    const char *_UniWebViewGetCurrentUrl(const char *name) {
        return MJWebViewMakeCString([[MJWebViewManager sharedManager] webViewNameGetCurrentUrl:MJWebViewMakeNSString(name)]);
        return "";
    }
    
    void _UniWebViewSetZoomEnable(const char *name, bool enable) {
        [[MJWebViewManager sharedManager] webViewName:MJWebViewMakeNSString(name) setZoomEnable:enable];
    }
    
    void _UniWebViewAddUrlScheme(const char *name, const char *scheme) {
        [[MJWebViewManager sharedManager] webViewName:MJWebViewMakeNSString(name)
                                          addUrlScheme:MJWebViewMakeNSString(scheme)];
    }
    
    void _UniWebViewRemoveUrlScheme(const char *name, const char *scheme) {
        [[MJWebViewManager sharedManager] webViewName:MJWebViewMakeNSString(name)
                                       removeUrlScheme:MJWebViewMakeNSString(scheme)];
    }
    
    int _UniWebViewScreenHeight() {
        if (IOS_Version < 8.0) {
            if (UnityCurrentOrientation() == landscapeLeft || UnityCurrentOrientation() == landscapeRight) {
                return UnityGetGLViewController().view.frame.size.width;
            } else {
                return UnityGetGLViewController().view.frame.size.height;
            }
        } else {
            return UnityGetGLViewController().view.frame.size.height;
        }
//        return 0;
    }
//
    int _UniWebViewScreenWidth() {
        if (IOS_Version < 8.0) {
            if (UnityCurrentOrientation() == landscapeLeft || UnityCurrentOrientation() == landscapeRight) {
                return UnityGetGLViewController().view.frame.size.height;
            } else {
                return UnityGetGLViewController().view.frame.size.width;
            }
        } else {
            return UnityGetGLViewController().view.frame.size.width;
        }
//        return 0;
    }
    
    int _UniWebViewScreenScale() {
        return (int)[[UIScreen mainScreen] scale];
    }
    
    const char * _UniWebViewGetUserAgent(const char *name) {
        return MJWebViewMakeCString([[MJWebViewManager sharedManager] webViewGetUserAgent:MJWebViewMakeNSString(name)]);
        return "";
    }
    
    void _UniWebViewSetUserAgent(const char *userAgent) {
        [[MJWebViewManager sharedManager] webViewSetUserAgent:MJWebViewMakeNSString(userAgent)];
    }
    
    float _UniWebViewGetAlpha(const char *name) {
        return [[MJWebViewManager sharedManager] webViewNameGetAlpha:MJWebViewMakeNSString(name)];
//        return 0.0f;
    }
    
    void _UniWebViewSetAlpha(const char *name, float alpha) {
        [[MJWebViewManager sharedManager] webViewName:MJWebViewMakeNSString(name)
                                              setAlpha:alpha];
    }
    
    void _UniWebViewSetHeaderField(const char *name, const char *key, const char *value) {
        [[MJWebViewManager sharedManager] webViewName:MJWebViewMakeNSString(name)
                                              setValue:MJWebViewMakeNSString(value)
                                        forHeaderField:MJWebViewMakeNSString(key)
         ];
    }
    
    void _UniWebViewSetVerticalScrollBarShow(const char *name, BOOL show) {
        [[MJWebViewManager sharedManager] webViewName:MJWebViewMakeNSString(name) setVerticalScrollBarShow:show];
    }
    
    void _UniWebViewSetHorizontalScrollBarShow(const char *name, BOOL show) {
        [[MJWebViewManager sharedManager] webViewName:MJWebViewMakeNSString(name) setHorizontalScrollBarShow:show];
    }
    
    
    bool _UniWebViewGetOpenLinksInExternalBrowser(const char *name) {
        MJWebView *webView = [[MJWebViewManager sharedManager] webViewWithName:MJWebViewMakeNSString(name)];
        return webView.openLinkInExternalBrowser;
//        return NO;
    }
    
    void _UniWebViewSetOpenLinksInExternalBrowser(const char *name, BOOL value) {
        MJWebView *webView = [[MJWebViewManager sharedManager] webViewWithName:MJWebViewMakeNSString(name)];
        webView.openLinkInExternalBrowser = value;
    }
    
    void _UniWebViewSetDoneButtonText(const char *text) {
        NSString *title = MJWebViewMakeNSString(text);
        if (title.length == 0) {
            MJWebViewDoneButtonTitle = nil;
        } else {
            MJWebViewDoneButtonTitle = title;
        }
    }
    
    
//#if defined (_cplusplus)
}
//#endif

