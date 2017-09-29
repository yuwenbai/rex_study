//
//  UnityDelegate+WebView.m
//  Unity-iPhone
//
//  Created by kang on 2017/9/15.
//
//

#import "UnityDelegate+WebView.h"

@implementation UnityDelegate (WebView)

//- (void) openWebViewController:(NSString *)url {
//
//    NSLog(@"open webView url:%@",url);
//
//    MJWebViewController *webViewController = [[MJWebViewController alloc]initWithUrl:url];
//
//    MJNavigationController *navViewController = [[MJNavigationController alloc]initWithRootViewController:webViewController];
//    UnityAppController *appDelegate = (UnityAppController*)GetAppController();
//    [appDelegate.rootViewController presentViewController:navViewController animated:YES completion:nil];
//
//}


- (void) openWebViewController:(NSString *)url {
    
    //    if ([url isEqualToString:@"123"]){
    //        UIAlertView *alert = [[UIAlertView alloc]initWithTitle:@"123" message:@"123" delegate:nil cancelButtonTitle:@"知道啦~" otherButtonTitles:nil, nil];
    //        [alert show];
    //    }else  {
    //        UIAlertView *alert = [[UIAlertView alloc]initWithTitle:@"NamedEvent" message:url delegate:nil cancelButtonTitle:@"知道啦~" otherButtonTitles:nil, nil];
    //        [alert show];
    //    }
    
}


//- (void) openWebViewControllerWithWebView:(MJWebView *)webView {
//
//    MJWebViewController *webViewController = [[MJWebViewController alloc]initWithWebView:webView];
//
//    MJNavigationController *navViewController = [[MJNavigationController alloc]initWithRootViewController:webViewController];
//    UnityAppController *appDelegate = (UnityAppController*)GetAppController();
//    [appDelegate.rootViewController presentViewController:navViewController animated:YES completion:nil];
//    
//    
//}

@end
