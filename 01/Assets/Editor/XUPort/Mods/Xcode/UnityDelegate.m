//
//  UnityDelegate.m
//  Unity-iPhone
//
//  Created by developer on 2017/7/25.
//
//

#import "UnityDelegate.h"
#import "NSDictionary+DictionString.h"

#if defined (_cplusplus)
extern "C"
{
#endif
    
    void UnitySendMessage(const char *, const char *, const char *);
    
    //     UIWindow*			UnityGetMainWindow()		{ return GetAppController().mainDisplay.window; }
    //     UIViewController*	UnityGetGLViewController()	{ return GetAppController().rootViewController; }
    //     UIView*			UnityGetGLView()			{ return GetAppController().unityView; }
    //     ScreenOrientation	UnityCurrentOrientation()	{ return GetAppController().unityView.contentOrientation; }
#if defined (_cplusplus)
}
#endif

@interface UnityDelegate()

@end

static UnityDelegate * unityDelegate = nil;
@implementation UnityDelegate

#pragma mark - init
+ (instancetype)allocWithZone:(struct _NSZone *)zone
{
    static dispatch_once_t onceToken;
    dispatch_once(&onceToken, ^{
        unityDelegate = [super allocWithZone:zone];
    });
    return unityDelegate;
}

+ (instancetype) shareInstance {
    return [self allocWithZone:nil];
}

- (id) copyWithZone:(NSZone *)zone;{
    return self;
}

#pragma mark - Unity Methods

/**
 发送Unity消息

 @param unityObj Unity消息响应对象
 @param unitySEL Unity响应消息方法
 @param unityMenthodKey Unity响应消息方法的key
 @param paramString 消息参数
 */
- (void) sendUnityMessageTo:(NSString *)unityObj sel:(NSString *)unitySEL methodKey:(NSString *)unityMenthodKey params:(NSString *)paramString {
    
    // 字典转换成Json字符串
    NSDictionary * paramsDic = @{
                                 Unity_MethodKey:unityMenthodKey,
                                 Unity_EnumKey:@"",
                                 Unity_ParamKey:paramString
                                };
    NSString *param = [paramsDic dictionaryToString];
    UnitySendMessage([unityObj UTF8String], [unitySEL UTF8String], [param UTF8String]);
    
}


/**
 发送Unity消息
 
 @param unityObj Unity消息响应对象
 @param unitySEL Unity响应消息方法
 */
- (void) sendUnityMessageTo:(NSString *)unityObj
                        sel:(NSString *)unitySEL
                methodValue:(NSString *)unityMenthodValue
                  enumValue:(NSString *)enumValue
                 paramValue:(NSString *)paramString {
    
    // 字典转换成Json字符串
    NSDictionary * paramsDic = @{
                                 Unity_MethodKey:unityMenthodValue,
                                 Unity_EnumKey:enumValue,
                                 Unity_ParamKey:paramString
                                 };
    NSString *param = [paramsDic dictionaryToString];
    UnitySendMessage([unityObj UTF8String], [unitySEL UTF8String], [param UTF8String]);
    
}





@end
