//
//  AppDelegate+Transition.m
//  Unity-iPhone
//
//  Created by developer on 2017/8/6.
//
//

#import "UnityAppController+Transition.h"
//#import "UnityView.h"

@implementation UnityAppController (Transition)


- (void) transitionToNewViewController:(UIViewController *)toViewController {
   
    _rootController.view	= nil;
    toViewController.view	= _rootView;
    _rootController = toViewController;
    _window.rootViewController = toViewController;
    
    [_rootView layoutSubviews];
}

//- (void) pressentToViewController:(UIViewController *)toViewController {
//    _rootController.view	= nil;
//    toViewController.view	= _rootView;
//    [_rootController presentViewController:toViewController animated:NO completion:nil];
//}

@end
