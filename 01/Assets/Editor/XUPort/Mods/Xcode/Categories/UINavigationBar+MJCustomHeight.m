//
//  UINavigationBar+MJCustomHeight.m
//  Unity-iPhone
//
//  Created by developer on 2017/8/13.
//
//

#import "UINavigationBar+MJCustomHeight.h"
#import <objc/runtime.h>

static char const *const MJNavigationBarHeightKey = "MJNavigationBarHeightKey";

@implementation UINavigationBar (MJCustomHeight)

//- (void)setHeight:(CGFloat)height
//{
//    objc_setAssociatedObject(self, MJNavigationBarHeightKey, @(height), OBJC_ASSOCIATION_RETAIN_NONATOMIC);
//}
//
//- (NSNumber *)height
//{
//    return objc_getAssociatedObject(self, MJNavigationBarHeightKey);
//}
//
//- (CGSize)sizeThatFits:(CGSize)size
//{
//    CGSize newSize;
//    
//    if (self.height) {
//        newSize = CGSizeMake(self.superview.bounds.size.width, [self.height floatValue]);
//    } else {
//        newSize = [super sizeThatFits:size];
//    }
//    
//    return newSize;
//}


@end
