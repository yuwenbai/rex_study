//
//  UnityDelegate+Clipboard.h
//  Unity-iPhone
//
//  Created by kang on 2017/9/15.
//
//

#import "UnityDelegate.h"

@interface UnityDelegate (Clipboard)

- (void) copyStringToClipboard:(const char *)inputStr;

@end
