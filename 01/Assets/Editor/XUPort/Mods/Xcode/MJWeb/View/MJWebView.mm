//
//  MJWebView.m
//  Unity-iPhone
//
//  Created by developer on 2017/8/13.
//
//

#import "MJWebView.h"
#import "MJWebViewManager.h"
#import "UnityInterface.h"


@implementation MJWebViewToolBar

@end

@implementation MJWebSpinner
-(id) initWithFrame:(CGRect)frame {
    self = [super initWithFrame:frame];
    if (self) {
        self.backgroundColor = [UIColor colorWithRed:0 green:0 blue:0 alpha:0.5];
        self.clipsToBounds = YES;
        self.layer.cornerRadius = 10.0;
        
        _indicator = [[UIActivityIndicatorView alloc] initWithActivityIndicatorStyle:UIActivityIndicatorViewStyleWhiteLarge];
        _indicator.frame = (CGRect){ frame.size.width / 2 - _indicator.frame.size.width / 2,
            frame.size.height / 2 - _indicator.frame.size.height / 2 - 10,
            _indicator.bounds.size.width,
            _indicator.bounds.size.height};
        [self addSubview:_indicator];
        
        _textLabel = [[UILabel alloc] initWithFrame:CGRectMake(0, frame.size.height - 22 * 2, frame.size.width, 22)];
        _textLabel.backgroundColor = [UIColor clearColor];
        _textLabel.textColor = [UIColor whiteColor];
        _textLabel.adjustsFontSizeToFitWidth = YES;
        _textLabel.textAlignment = NSTextAlignmentCenter;
        _textLabel.text = @"Loading...";
        [self addSubview:_textLabel];
        
        UITapGestureRecognizer *tap = [[UITapGestureRecognizer alloc] initWithTarget:self action:@selector(hide)];
        [self addGestureRecognizer:tap];
        
    }
    return self;
}

-(void) show {
    self.hidden = NO;
    [self.indicator startAnimating];
}

-(void) hide {
    [self.indicator stopAnimating];
    self.hidden = YES;
}

@end




@implementation MJWebView

-(id) initWithFrame:(CGRect)frame {
    self = [super initWithFrame:frame];
    if (self) {
        CGRect toolBarFrame = CGRectMake(0, frame.size.height - 44, frame.size.width, 44);
        _toolBar = ({
            MJWebViewToolBar *toolBar = [[MJWebViewToolBar alloc] initWithFrame:toolBarFrame];
            toolBar.backgroundColor = [UIColor whiteColor];
            
            UIBarButtonItem *back = [[UIBarButtonItem alloc] initWithBarButtonSystemItem:UIBarButtonSystemItemRewind target:self action:@selector(goBack)];
            UIBarButtonItem *forward = [[UIBarButtonItem alloc] initWithBarButtonSystemItem:UIBarButtonSystemItemFastForward target:self action:@selector(goForward)];
            UIBarButtonItem *reload = [[UIBarButtonItem alloc] initWithBarButtonSystemItem:UIBarButtonSystemItemRefresh target:self action:@selector(btnReloadPressed:)];
            UIBarButtonItem *space = [[UIBarButtonItem alloc] initWithBarButtonSystemItem:UIBarButtonSystemItemFlexibleSpace target:self action:nil];
            
            UIBarButtonItem *done;
            if (MJWebViewDoneButtonTitle) {
                done = [[UIBarButtonItem alloc] initWithTitle:MJWebViewDoneButtonTitle style:UIBarButtonItemStylePlain target:self action:@selector(btnDonePressed:)];
            } else {
                done = [[UIBarButtonItem alloc] initWithBarButtonSystemItem:UIBarButtonSystemItemDone target:self action:@selector(btnDonePressed:)];
            }
            
            toolBar.items = @[back,forward,reload,space,done];
            
            toolBar.btnBack = back;
            toolBar.btnNext = forward;
            toolBar.btnReload = reload;
            toolBar.btnDone = done;
            
            toolBar.hidden = YES;
            
            toolBar;
        });
        
        _schemes = [[NSMutableArray alloc] initWithObjects:@"uniwebview", nil];
        _headers = [NSMutableDictionary dictionary];
        
        _showSpinnerWhenLoading = YES;
        
//        _spinner = ({
//            
//            spinner;
//        });
        
        MJWebSpinner *spinner = [[MJWebSpinner alloc] initWithFrame:CGRectMake(frame.size.width / 2 - 65, frame.size.height / 2 - 65, 130, 130)];
        [spinner hide];
        
        _spinner = spinner;
        
        [self setBounces:NO];
        [self updateToolBtn];
        
    }
    return self;
}

-(void)setBounces:(BOOL)bounces {
    UIScrollView* sv = nil;
    for(UIView* view in self.subviews){
        if([view isKindOfClass:[UIScrollView class]]){
            sv = (UIScrollView*)view;
            sv.bounces = bounces;
        }
    }
}


-(void) btnDonePressed:(id)sender {
//    [[MJWebViewManager sharedManager] webViewDone:self];
}

-(void) btnReloadPressed:(id)sender {
    if (!self.loading) {
        [self reload];
    } else {
        NSLog(@"UniWebView can not reload because some content is being loading right now.");
    }
}

-(void) updateToolBtn {
    self.toolBar.btnBack.enabled = [self canGoBack];
    self.toolBar.btnNext.enabled = [self canGoForward];
}

//-(void)changeToInsets:(UIEdgeInsets)insets targetOrientation:(ScreenOrientation)orientation {
//    UIView *unityView = UnityGetGLViewController().view;
//    CGRect viewRect = unityView.frame;
//    
//    if (orientation == landscapeLeft || orientation == landscapeRight) {
//        if (IOS_Version < 8.0 ) {
//            viewRect = CGRectMake(viewRect.origin.x, viewRect.origin.y, viewRect.size.height, viewRect.size.width);
//            self.toolBar.frame = CGRectMake(0, unityView.frame.size.width - 44, unityView.frame.size.height, 44);
//            self.spinner.frame = CGRectMake(unityView.frame.size.height / 2 - 65, unityView.frame.size.width / 2 - 65, 130, 130);
//        } else {
//            self.toolBar.frame = CGRectMake(0, unityView.frame.size.height - 44, unityView.frame.size.width, 44);
//            self.spinner.frame = CGRectMake(unityView.frame.size.width / 2 - 65, unityView.frame.size.height / 2 - 65, 130, 130);
//        }
//    } else {
//        self.toolBar.frame = CGRectMake(0, unityView.frame.size.height - 44, unityView.frame.size.width, 44);
//        self.spinner.frame = CGRectMake(unityView.frame.size.width / 2 - 65, unityView.frame.size.height / 2 - 65, 130, 130);
//    }
//    
//    CGRect f = CGRectMake(insets.left,
//                          insets.top,
//                          viewRect.size.width - insets.left - insets.right,
//                          viewRect.size.height - insets.top - insets.bottom);
//    self.frame = f;
//    self.insets = insets;
//}



@end
