//
//  UIImage+ScaleImage.m
//  Unity-iPhone
//
//  Created by kang on 2017/9/14.
//
//

#import "UIImage+ScaleImage.h"

@implementation UIImage (ScaleImage)

/**
 生成微信分享缩略图
 @return 二进制缩略图数据
 */
- (NSData *) getImageData {
    
    UIImage *iconImg = self;
    NSData *data = UIImageJPEGRepresentation(iconImg, 1);
    
    int i = 1;
    while (data.length > 32*1000) {
        data = UIImageJPEGRepresentation(iconImg, 1-i/10);
        i++;
    }
    return  data;
}


+ (UIImage *)scaleImage:(UIImage *)image toScale:(float)scaleSize
{
    UIGraphicsBeginImageContext(CGSizeMake(image.size.width * scaleSize, image.size.height * scaleSize));
    [image drawInRect:CGRectMake(0, 0, image.size.width * scaleSize, image.size.height * scaleSize)];
    UIImage *scaledImage = UIGraphicsGetImageFromCurrentImageContext();
    UIGraphicsEndImageContext();
    return scaledImage;
}


+ (UIImage *)getLaunchImageName {
    NSDictionary *infoDict = [[NSBundle mainBundle] infoDictionary];
    //获取app中所有icon名字数组
    NSArray *iconsArr = infoDict[@"CFBundleIcons"][@"CFBundlePrimaryIcon"][@"CFBundleIconFiles"];
    //取最后一个icon的名字
    NSString *iconLastName = [iconsArr lastObject];
    UIImage *startImageView=nil;
    startImageView = [UIImage imageNamed:iconLastName];//AppIcon57x57
    return startImageView;
}


@end
