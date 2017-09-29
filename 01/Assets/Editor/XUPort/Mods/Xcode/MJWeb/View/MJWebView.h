//
//  MJWebView.h
//  Unity-iPhone
//
//  Created by developer on 2017/8/13.
//
//

#import <UIKit/UIKit.h>
#import "UnityInterface.h"

//typedef NS_ENUM(NSInteger, MJWebViewTransitionEdge) {
//    MJWebViewTransitionEdgeNone,
//    MJWebViewTransitionEdgeTop,
//    MJWebViewTransitionEdgeLeft,
//    MJWebViewTransitionEdgeBottom,
//    MJWebViewTransitionEdgeRight
//};


@protocol MJWebViewDelegate <NSObject>

@optional
- (void) dismissMJWebViewController;

@end

static NSString *MJWebViewDoneButtonTitle = nil;

@interface MJWebViewToolBar : UIToolbar
@property (nonatomic, retain) UIBarButtonItem *btnNext;
@property (nonatomic, retain) UIBarButtonItem *btnBack;
@property (nonatomic, retain) UIBarButtonItem *btnReload;
@property (nonatomic, retain) UIBarButtonItem *btnDone;
@end



@interface MJWebSpinner : UIView
@property (nonatomic, retain) UIActivityIndicatorView *indicator;
@property (nonatomic, retain) UILabel *textLabel;
-(id) initWithFrame:(CGRect)frame;
-(void) show;
-(void) hide;
@end


@interface MJWebView : UIWebView
@property (nonatomic, retain) MJWebViewToolBar *toolBar;
@property (nonatomic, retain) MJWebSpinner *spinner;
@property (nonatomic, assign) UIEdgeInsets insets;

@property (nonatomic, assign) BOOL showSpinnerWhenLoading;
@property (nonatomic, copy) NSString *currentUrl;

@property (nonatomic, retain) NSMutableArray *schemes;
@property (nonatomic, retain) NSMutableDictionary *headers;

@property (nonatomic, assign) BOOL viewAnimating;
@property (nonatomic, assign) BOOL openLinkInExternalBrowser;
@property (nonatomic, weak) id<MJWebViewDelegate> mjDelegate;

-(id) initWithFrame:(CGRect)frame;
-(void) btnDonePressed:(id)sender;
-(void) updateToolBtn;
//-(void) changeToInsets:(UIEdgeInsets)insets targetOrientation:(ScreenOrientation)orientation;
-(void) setBounces:(BOOL)bounces;

@end
