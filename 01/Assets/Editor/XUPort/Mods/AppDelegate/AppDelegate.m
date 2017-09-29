#import "UnityAppController.h"
#import "UnityAppController+ViewHandling.h"
#import "UnityAppController+Rendering.h"
#import "iPhone_Sensors.h"

#import <CoreGraphics/CoreGraphics.h>
#import <QuartzCore/QuartzCore.h>
#import <QuartzCore/CADisplayLink.h>
#import <Availability.h>

#import <OpenGLES/EAGL.h>
#import <OpenGLES/EAGLDrawable.h>
#import <OpenGLES/ES2/gl.h>
#import <OpenGLES/ES2/glext.h>

#include <mach/mach_time.h>

// MSAA_DEFAULT_SAMPLE_COUNT was moved to iPhone_GlesSupport.h
// ENABLE_INTERNAL_PROFILER and related defines were moved to iPhone_Profiler.h
// kFPS define for removed: you can use Application.targetFrameRate (30 fps by default)
// DisplayLink is the only run loop mode now - all others were removed

#include "CrashReporter.h"

#import "UI/OrientationSupport.h"
#import "UI/UnityView.h"
#import "UI/Keyboard.h"
#import "UI/SplashScreen.h"
#import "Unity/InternalProfiler.h"
#import "Unity/DisplayManager.h"
#import "Unity/EAGLContextHelper.h"
#import "Unity/GlesHelper.h"
#import "PluginBase/AppDelegateListener.h"
#import "WXApi.h"
#import "NSString+K_ParamJsonString.h"
#import "UnityInterface.h"
#import "WecahtManager.h"
#import "UnityDelegateAPI.h"

// Set this to 1 to force single threaded rendering
#define UNITY_FORCE_DIRECT_RENDERING 0

bool	_ios42orNewer			= false;
bool	_ios43orNewer			= false;
bool	_ios50orNewer			= false;
bool	_ios60orNewer			= false;
bool	_ios70orNewer			= false;
bool	_ios80orNewer			= false;
bool	_ios81orNewer			= false;
bool	_ios82orNewer			= false;
bool	_ios83orNewer			= false;
bool	_ios90orNewer			= false;
bool	_ios91orNewer			= false;
bool	_ios100orNewer			= false;

// was unity rendering already inited: we should not touch rendering while this is false
bool	_renderingInited		= false;
// was unity inited: we should not touch unity api while this is false
bool	_unityAppReady			= false;
// see if there's a need to do internal player pause/resume handling
//
// Typically the trampoline code should manage this internally, but
// there are use cases, videoplayer, plugin code, etc where the player
// is paused before the internal handling comes relevant. Avoid
// overriding externally managed player pause/resume handling by
// caching the state
bool	_wasPausedExternal		= false;
// should we skip present on next draw: used in corner cases (like rotation) to fill both draw-buffers with some content
bool	_skipPresent			= false;
// was app "resigned active": some operations do not make sense while app is in background
bool	_didResignActive		= false;

// was startUnity scheduled: used to make startup robust in case of locking device
static bool	_startUnityScheduled	= false;
static bool _displayLinkDestroyed = false;
static bool _trampolineShutDown = false;

bool	_supportsMSAA			= false;


@implementation UnityAppController

@synthesize unityView				= _unityView;
@synthesize unityDisplayLink		= _unityDisplayLink;

@synthesize rootView				= _rootView;
@synthesize rootViewController		= _rootController;
@synthesize mainDisplay				= _mainDisplay;
@synthesize renderDelegate			= _renderDelegate;
@synthesize quitHandler				= _quitHandler;

#if !UNITY_TVOS
@synthesize interfaceOrientation	= _curOrientation;
#endif

- (id)init
{
    if( (self = [super init]) )
    {
        // due to clang issues with generating warning for overriding deprecated methods
        // we will simply assert if deprecated methods are present
        // NB: methods table is initied at load (before this call), so it is ok to check for override
        NSAssert(![self respondsToSelector:@selector(createUnityViewImpl)],
                 @"createUnityViewImpl is deprecated and will not be called. Override createUnityView"
                 );
        NSAssert(![self respondsToSelector:@selector(createViewHierarchyImpl)],
                 @"createViewHierarchyImpl is deprecated and will not be called. Override willStartWithViewController"
                 );
        NSAssert(![self respondsToSelector:@selector(createViewHierarchy)],
                 @"createViewHierarchy is deprecated and will not be implemented. Use createUI"
                 );
    }
//    wxController=[[WXController alloc]init];
    return self;
}


- (void)setWindow:(id)object		{}
- (UIWindow*)window					{ return _window; }


- (void)shouldAttachRenderDelegate	{}
- (void)preStartUnity				{}


- (void)startUnity:(UIApplication*)application
{
    NSAssert(_unityAppReady == NO, @"[AppDelegate startUnity:] called after Unity has been initialized");
    
    UnityInitApplicationGraphics(UNITY_FORCE_DIRECT_RENDERING);
    
    // we make sure that first level gets correct display list and orientation
    [[DisplayManager Instance] updateDisplayListInUnity];
    
    UnityLoadApplication();
    Profiler_InitProfiler();
    
    [self showGameUI];
    [self createDisplayLink];
    
    UnitySetPlayerFocus(1);
}

extern "C" void UnityDestroyDisplayLink()
{
    if (!_displayLinkDestroyed)
    {
        [GetAppController() destroyDisplayLink];
        _displayLinkDestroyed = true;
    }
}

extern "C" void UnityShutdownTrampoline()
{
    if (!_trampolineShutDown)
    {
        if (UnityGetMainWindow().rootViewController == UnityGetGLViewController())
            UnityGetMainWindow().rootViewController = nil;
        
        [UnityGetGLView() removeFromSuperview];
        
        UnityDestroyDisplayLink();
        _trampolineShutDown = true;
        _unityAppReady = false;
    }
}

extern "C" void UnityRequestQuit()
{
    _didResignActive = true;
    if (GetAppController().quitHandler)
        GetAppController().quitHandler();
    else
        exit(0);
}

//#pragma mark - Orientations
//#pragma mark 屏幕方向
//#if !UNITY_TVOS
//- (NSUInteger)application:(UIApplication*)application supportedInterfaceOrientationsForWindow:(UIWindow*)window
//{
//    // UIInterfaceOrientationMaskAll
//    // it is the safest way of doing it:
//    // - GameCenter and some other services might have portrait-only variant
//    //     and will throw exception if portrait is not supported here
//    // - When you change allowed orientations if you end up forbidding current one
//    //     exception will be thrown
//    // Anyway this is intersected with values provided from UIViewController, so we are good
//    return   (1 << UIInterfaceOrientationPortrait) | (1 << UIInterfaceOrientationPortraitUpsideDown)
//    | (1 << UIInterfaceOrientationLandscapeRight) | (1 << UIInterfaceOrientationLandscapeLeft);
//}
//#endif
//    
//
//#pragma mark - Notification
//#pragma mark 通知相关
//    
//#if !UNITY_TVOS
//- (void)application:(UIApplication*)application didReceiveLocalNotification:(UILocalNotification*)notification
//{
//    AppController_SendNotificationWithArg(kUnityDidReceiveLocalNotification, notification);
//    UnitySendLocalNotification(notification);
//}
//#endif
//
//#if UNITY_USES_REMOTE_NOTIFICATIONS
//- (void)application:(UIApplication*)application didReceiveRemoteNotification:(NSDictionary*)userInfo
//{
//    AppController_SendNotificationWithArg(kUnityDidReceiveRemoteNotification, userInfo);
//    UnitySendRemoteNotification(userInfo);
//}
//
//- (void)application:(UIApplication*)application didRegisterForRemoteNotificationsWithDeviceToken:(NSData*)deviceToken
//{
//    AppController_SendNotificationWithArg(kUnityDidRegisterForRemoteNotificationsWithDeviceToken, deviceToken);
//    UnitySendDeviceToken(deviceToken);
//}
//
//#if !UNITY_TVOS
//- (void)application:(UIApplication *)application didReceiveRemoteNotification:(NSDictionary *)userInfo fetchCompletionHandler:(void (^)(UIBackgroundFetchResult result))handler
//{
//    AppController_SendNotificationWithArg(kUnityDidReceiveRemoteNotification, userInfo);
//    UnitySendRemoteNotification(userInfo);
//    if (handler)
//    {
//        handler(UIBackgroundFetchResultNoData);
//    }
//}
//#endif
//
//- (void)application:(UIApplication*)application didFailToRegisterForRemoteNotificationsWithError:(NSError*)error
//{
//    AppController_SendNotificationWithArg(kUnityDidFailToRegisterForRemoteNotificationsWithError, error);
//    UnitySendRemoteNotificationError(error);
//}
//#endif
//
//    
//#pragma mark - URL 
//#pragma mark 微信等相关处理
//    
//- (BOOL)application:(UIApplication*)application openURL:(NSURL*)url sourceApplication:(NSString*)sourceApplication annotation:(id)annotation
//{
//    NSLog(@"sourceApp:%@",sourceApplication);
//    NSMutableArray* keys	= [NSMutableArray arrayWithCapacity:3];
//    NSMutableArray* values	= [NSMutableArray arrayWithCapacity:3];
//    
//    auto addItem = [&](NSString* key, id value)
//    {
//        [keys addObject:key];
//        [values addObject:value ? value : [NSNull null]];
//    };
//    
//    addItem(@"url", url);
//    addItem(@"sourceApplication", sourceApplication);
//    addItem(@"annotation", annotation);
//    
//    NSDictionary* notifData = [NSDictionary dictionaryWithObjects:values forKeys:keys];
//    AppController_SendNotificationWithArg(kUnityOnOpenURL, notifData);
//    
//    // 微信跳转
//    if ([sourceApplication containsString:sourceApplication]) {
//        return [WXApi handleOpenURL:url delegate:[WecahtManager shareInstance]];
//    }else {
//        return YES;
//    }
//    //    return  [WXApi handleOpenURL:url delegate:[WXApiManager sharedManager]];
//}
//    
//
///**
// 这个方法是用于从微信返回第三方App
//
// @param application
// @param url
// @return
// */
//- (BOOL)application:(UIApplication *)application handleOpenURL:(NSURL *)url {
//    NSLog(@"befor iOS9 url:%@",url);
//    NSString *urlString = url.absoluteString;
//    if ([urlString hasPrefix:@"wx8820c0bb95c6fc96"]) {
//        BOOL result = [WXApi handleOpenURL:url delegate:[WecahtManager shareInstance]];
//        return result;
//    }else if ([urlString hasPrefix:@"sljsappxac://com.slj.gamemj"]) {
//        
//        [[UnityDelegate shareInstance] sendUnityMessageTo:UnityRoot  sel:UnityFunction methodKey:Untiy_OpenPara params:[urlString paramJsonString]];
//        return YES;
//    }
//    return YES;
//}
//
////iOS9以后走的是这个方法
//-(BOOL)application:(UIApplication *)app openURL:(NSURL *)url options:(NSDictionary<NSString *,id> *)options{
//    NSLog(@"after iOS9 url:%@",url);
//    NSString *urlString = url.absoluteString;
//    if ([urlString hasPrefix:@"wx8820c0bb95c6fc96"]) {
//        BOOL result = [WXApi handleOpenURL:url delegate:[WecahtManager shareInstance]];
//        return result;
//    }else if ([urlString hasPrefix:@"sljsappxac://com.slj.gamemj"]) {
//        
//        [[UnityDelegate shareInstance] sendUnityMessageTo:UnityRoot  sel:UnityFunction methodKey:Untiy_OpenPara params:[urlString paramJsonString]];
//        return YES;
//    }
//    
//    return YES;
//}
//    
//- (BOOL)application:(UIApplication *)application continueUserActivity:(NSUserActivity *)userActivity restorationHandler:(void(^)(NSArray * __nullable restorableObjects))restorationHandler   {
//
//    return YES;
//}
//    
//
//    
//    
//#pragma mark - run
//#pragma mark app启动入口
//    
//-(BOOL)application:(UIApplication*)application willFinishLaunchingWithOptions:(NSDictionary*)launchOptions
//{
//    return YES;
//}
//
//- (BOOL)application:(UIApplication*)application didFinishLaunchingWithOptions:(NSDictionary*)launchOptions
//{
//    ::printf("-> applicationDidFinishLaunching()\n");
//    
//    // send notfications
//#if !UNITY_TVOS
//    if(UILocalNotification* notification = [launchOptions objectForKey:UIApplicationLaunchOptionsLocalNotificationKey])
//        UnitySendLocalNotification(notification);
//    
//    if(NSDictionary* notification = [launchOptions objectForKey:UIApplicationLaunchOptionsRemoteNotificationKey])
//        UnitySendRemoteNotification(notification);
//    
//    if ([UIDevice currentDevice].generatesDeviceOrientationNotifications == NO)
//        [[UIDevice currentDevice] beginGeneratingDeviceOrientationNotifications];
//#endif
//    
//    //检测scheme启动app
//    if (NSURL *url = [launchOptions objectForKey:UIApplicationLaunchOptionsURLKey]) {
//        NSString *urlString = url.absoluteString;
//        if ([urlString hasPrefix:@"sljsappxac://com.slj.gamemj"]) {
//            [[NSUserDefaults standardUserDefaults] setObject:[urlString paramJsonString] forKey:@"schemes"];
//            [[NSUserDefaults standardUserDefaults] synchronize];
//        }
//    }
//    
//    //APP启动检查定位服务和授权
//    [[UnityDelegate shareInstance] isOpenLocationService];
//    
//    //APP启动时开启GPS定位
//    [[UnityDelegate shareInstance] startUpdateGPS];
//    
//    //APP启动检查网络状态
//    [[UnityDelegate shareInstance] checkNetWorkState];
//    
//    
//    UnityInitApplicationNoGraphics([[[NSBundle mainBundle] bundlePath] UTF8String]);
//    
//    [self selectRenderingAPI];
//    [UnityRenderingView InitializeForAPI:self.renderingAPI];
//    
//    _window			= [[UIWindow alloc] initWithFrame:[UIScreen mainScreen].bounds];
//    _unityView		= [self createUnityView];
//    
//    [DisplayManager Initialize];
//    _mainDisplay	= [DisplayManager Instance].mainDisplay;
//    [_mainDisplay createWithWindow:_window andView:_unityView];
//    
//    [self createUI];
//    [self preStartUnity];
//    
//    // if you wont use keyboard you may comment it out at save some memory
//    [KeyboardDelegate Initialize];
//    
//    //向微信注册
//    [WXApi registerApp:@"wx8820c0bb95c6fc96" enableMTA:YES];
//    
//    //向微信注册支持的文件类型
//    UInt64 typeFlag = MMAPP_SUPPORT_TEXT | MMAPP_SUPPORT_PICTURE | MMAPP_SUPPORT_LOCATION | MMAPP_SUPPORT_VIDEO |MMAPP_SUPPORT_AUDIO | MMAPP_SUPPORT_WEBPAGE | MMAPP_SUPPORT_DOC | MMAPP_SUPPORT_DOCX | MMAPP_SUPPORT_PPT | MMAPP_SUPPORT_PPTX | MMAPP_SUPPORT_XLS | MMAPP_SUPPORT_XLSX | MMAPP_SUPPORT_PDF;
//    
//    [WXApi registerAppSupportContentFlag:typeFlag];
//    
//    return YES;
//}

- (void) launchInitUnity {
    
    UnityInitApplicationNoGraphics([[[NSBundle mainBundle] bundlePath] UTF8String]);
    
    [self selectRenderingAPI];
    [UnityRenderingView InitializeForAPI:self.renderingAPI];
    
    _window			= [[UIWindow alloc] initWithFrame:[UIScreen mainScreen].bounds];
    _unityView		= [self createUnityView];
    
    [DisplayManager Initialize];
    _mainDisplay	= [DisplayManager Instance].mainDisplay;
    [_mainDisplay createWithWindow:_window andView:_unityView];
    
    [self createUI];
    [self preStartUnity];
    
    // if you wont use keyboard you may comment it out at save some memory
    [KeyboardDelegate Initialize];
}



#pragma mark - App State
#pragma mark app状态切换
- (void)applicationDidEnterBackground:(UIApplication*)application
{
    ::printf("-> applicationDidEnterBackground()\n");
    [[UnityDelegate shareInstance] SendUnityApplicationOnPause];
}

- (void)applicationWillEnterForeground:(UIApplication*)application
{
    ::printf("-> applicationWillEnterForeground()\n");
    
    // applicationWillEnterForeground: might sometimes arrive *before* actually initing unity (e.g. locking on startup)
    if(_unityAppReady)
    {
        // if we were showing video before going to background - the view size may be changed while we are in background
        [GetAppController().unityView recreateGLESSurfaceIfNeeded];
    }
    [[UnityDelegate shareInstance] SendUnityApplicationOnResume];
}

- (void)applicationDidBecomeActive:(UIApplication*)application
{
    ::printf("-> applicationDidBecomeActive()\n");
    
    [self removeSnapshotView];
    
    if(_unityAppReady)
    {
        if(UnityIsPaused() && _wasPausedExternal == false)
        {
            UnityWillResume();
            UnityPause(0);
        }
        UnitySetPlayerFocus(1);
    }
    else if(!_startUnityScheduled)
    {
        _startUnityScheduled = true;
        [self performSelector:@selector(startUnity:) withObject:application afterDelay:0];
    }
    
    _didResignActive = false;
}

- (void)removeSnapshotView
{
    // do this on the main queue async so that if we try to create one
    // and remove in the same frame, this always happens after in the same queue
    dispatch_async(dispatch_get_main_queue(), ^{
        if(_snapshotView)
        {
            [_snapshotView removeFromSuperview];
            _snapshotView = nil;
        }
    });
}

- (void)applicationWillResignActive:(UIApplication*)application
{
    ::printf("-> applicationWillResignActive()\n");
    
    if(_unityAppReady)
    {
        UnitySetPlayerFocus(0);
        
        _wasPausedExternal = UnityIsPaused();
        if (_wasPausedExternal == false)
        {
            // do pause unity only if we dont need special background processing
            // otherwise batched player loop can be called to run user scripts
            int bgBehavior = UnityGetAppBackgroundBehavior();
            if(bgBehavior == appbgSuspend || bgBehavior == appbgExit)
            {
                // Force player to do one more frame, so scripts get a chance to render custom screen for minimized app in task manager.
                // NB: UnityWillPause will schedule OnApplicationPause message, which will be sent normally inside repaint (unity player loop)
                // NB: We will actually pause after the loop (when calling UnityPause).
                UnityWillPause();
                [self repaint];
                UnityPause(1);
                
                // this is done on the next frame so that
                // in the case where unity is paused while going
                // into the background and an input is deactivated
                // we don't mess with the view hierarchy while taking
                // a view snapshot (case 760747).
                dispatch_async(dispatch_get_main_queue(), ^{
                    // if we are active again, we don't need to do this anymore
                    if (!_didResignActive)
                    {
                        return;
                    }
                    
                    _snapshotView = [self createSnapshotView];
                    if(_snapshotView)
                        [_rootView addSubview:_snapshotView];
                });
            }
        }
    }
    
    _didResignActive = true;
}

#pragma mark - memory managemnet
#pragma mark 内存管理
    
- (void)applicationDidReceiveMemoryWarning:(UIApplication*)application
{
    ::printf("WARNING -> applicationDidReceiveMemoryWarning()\n");
}

- (void)applicationWillTerminate:(UIApplication*)application
{
    ::printf("-> applicationWillTerminate()\n");
    
    Profiler_UninitProfiler();
    UnityCleanup();
    
    extern void SensorsCleanup();
    SensorsCleanup();
}

@end

#pragma mark - other method
#pragma mark  其他方法
void AppController_SendNotification(NSString* name)
{
    [[NSNotificationCenter defaultCenter] postNotificationName:name object:GetAppController()];
}
void AppController_SendNotificationWithArg(NSString* name, id arg)
{
    [[NSNotificationCenter defaultCenter] postNotificationName:name object:GetAppController() userInfo:arg];
}
void AppController_SendUnityViewControllerNotification(NSString* name)
{
    [[NSNotificationCenter defaultCenter] postNotificationName:name object:UnityGetGLViewController()];
}

extern "C" UIWindow*			UnityGetMainWindow()		{ return GetAppController().mainDisplay.window; }
extern "C" UIViewController*	UnityGetGLViewController()	{ return GetAppController().rootViewController; }
extern "C" UIView*				UnityGetGLView()			{ return GetAppController().unityView; }
extern "C" ScreenOrientation	UnityCurrentOrientation()	{ return GetAppController().unityView.contentOrientation; }



bool LogToNSLogHandler(LogType logType, const char* log, va_list list)
{
    NSLogv([NSString stringWithUTF8String:log], list);
    return true;
}

void UnityInitTrampoline()
{
#if ENABLE_CRASH_REPORT_SUBMISSION
    SubmitCrashReportsAsync();
#endif
    InitCrashHandling();
    
    NSString* version = [[UIDevice currentDevice] systemVersion];
    
    // keep native plugin developers happy and keep old bools around
    _ios42orNewer = true;
    _ios43orNewer = true;
    _ios50orNewer = true;
    _ios60orNewer = true;
    _ios70orNewer = [version compare: @"7.0" options: NSNumericSearch] != NSOrderedAscending;
    _ios80orNewer = [version compare: @"8.0" options: NSNumericSearch] != NSOrderedAscending;
    _ios81orNewer = [version compare: @"8.1" options: NSNumericSearch] != NSOrderedAscending;
    _ios82orNewer = [version compare: @"8.2" options: NSNumericSearch] != NSOrderedAscending;
    _ios83orNewer = [version compare: @"8.3" options: NSNumericSearch] != NSOrderedAscending;
    _ios90orNewer = [version compare: @"9.0" options: NSNumericSearch] != NSOrderedAscending;
    _ios91orNewer = [version compare: @"9.1" options: NSNumericSearch] != NSOrderedAscending;
    _ios100orNewer = [version compare: @"10.0" options: NSNumericSearch] != NSOrderedAscending;
    
    // Try writing to console and if it fails switch to NSLog logging
    ::fprintf(stdout, "\n");
    if(::ftell(stdout) < 0)
        UnitySetLogEntryHandler(LogToNSLogHandler);
}

