//
//  UIImage+ScaleImage.h
//  Unity-iPhone
//
//  Created by kang on 2017/9/14.
//
//

#import <UIKit/UIKit.h>

@interface UIImage (ScaleImage)

/**
 生成微信分享缩略图
 @return 二进制缩略图数据
 */
- (NSData *) getImageData;

+ (UIImage *)scaleImage:(UIImage *)image toScale:(float)scaleSize;

+ (UIImage *)getLaunchImageName;
@end
