//
//  UnityDelegate+Clipboard.m
//  Unity-iPhone
//
//  Created by kang on 2017/9/15.
//
//

#import "UnityDelegate+Clipboard.h"
#import "NSDictionary+DictionString.h"


#define Untiy_Getdatafromclipboard @"GetDataFromClipboard"
@implementation UnityDelegate (Clipboard)

- (void) copyStringToClipboard:(const char *)inputStr
{
    NSDictionary *data=[NSDictionary dictionWithCharString:inputStr];
    NSString *messagedata =[[data objectForKey:@"data"] copy];
    UIPasteboard *pastboard=[UIPasteboard generalPasteboard];
    pastboard.string=messagedata;
    
//    NSDictionary * openParaStr = @{untiy_key:untiy_Getdatafromclipboard,
//                                   untiy_enum:@"",
//                                   untiy_value:messagedata};
//    NSString *dataToUnity=[self.jsonTool DictionaryToJsonString:openParaStr];
    
//    NSString *param = [openParaStr dictionaryToString];
//    UnitySendMessage([unityRoot UTF8String], [unityFunction UTF8String], [dataToUnity UTF8String]);
    
    [self sendUnityMessageTo:UnityRoot
                         sel:UnityFunction
                 methodValue:Untiy_Getdatafromclipboard
                   enumValue:@""
                  paramValue:messagedata];
}

@end
