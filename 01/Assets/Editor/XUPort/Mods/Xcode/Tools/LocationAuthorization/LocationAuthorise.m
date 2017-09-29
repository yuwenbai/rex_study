//
//  LocationAuthorise.m
//  Unity-iPhone
//
//  Created by User on 2017/8/25.
//
//

#import "LocationAuthorise.h"
#import <CoreLocation/CoreLocation.h>

@implementation LocationAuthorise

// 请求网络状态
+ (BOOL) requestLocationAuthorizationState {
    
    if ([self isOpenLocationService]) {
        
        return [self isLocationServiceAuthorizationed];
    }
    
    return NO;
}



/**
 检查定位服务开启状态
 */
+ (BOOL) isOpenLocationService {
    
    BOOL status = YES;
    if (![CLLocationManager locationServicesEnabled]) {
        double v = [[[UIDevice currentDevice] systemVersion] doubleValue];
        if (v >= 10.0) {
            UIAlertView *alert = [[UIAlertView alloc]initWithTitle:@"定位服务未开启" message:@"定位服务未开启，请进入系统【设置】>【隐私】>【定位服务】中打开定位开关，并允许369互娱麻将使用定位服务" delegate:nil cancelButtonTitle:@"知道啦~" otherButtonTitles:nil, nil];
            [alert show];
        }else {
            UIAlertView *alertios9 = [[UIAlertView alloc]initWithTitle:@"定位服务未开启" message:@"定位服务未开启，请进入系统【设置】>【隐私】>【定位服务】中打开定位开关，并允许369互娱麻将使用定位服务" delegate:self cancelButtonTitle:@"取消" otherButtonTitles:@"设置", nil];
            alertios9.tag = 100;
            [alertios9 show];
        }
        
        return NO;
    }
    return status;
}


/**
 检查定位服务授权状态
 */
+ (BOOL) isLocationServiceAuthorizationed {
    
    BOOL status = NO;
    if (([CLLocationManager authorizationStatus] == kCLAuthorizationStatusAuthorizedWhenInUse
         || [CLLocationManager authorizationStatus] == kCLAuthorizationStatusAuthorizedAlways)) {
        
        status = YES;
    } else if ([CLLocationManager authorizationStatus] == kCLAuthorizationStatusNotDetermined){
        status = NO;
    } else {
        UIAlertView *alertios9 = [[UIAlertView alloc]initWithTitle:@"定位服务未授权" message:@"定位服务未授权，请进入系统【设置】>【隐私】>【定位服务】中打开隐私开关，并允许369互娱麻将使用定位服务" delegate:self cancelButtonTitle:@"取消" otherButtonTitles:@"设置", nil];
        alertios9.tag = 101;
        [alertios9 show];
    }
    
    return status;
}


#pragma mark - alertView delegate

+ (void) alertView:(UIAlertView *)alertView clickedButtonAtIndex:(NSInteger)buttonIndex {
    
    // 授权alert view
    if (alertView.tag == 100) {
        
        if (buttonIndex == 1) {
            NSURL *url = [NSURL URLWithString:@"prefs:root=LOCATION_SERVICES"];
            if ([[UIApplication sharedApplication] canOpenURL:url]) {
                // 系统小于10的时候，打开定位界面
                [[UIApplication sharedApplication] openURL:url];
            } else {
                // 系统大于10的时候直接打开当前App的设置界面
                [[UIApplication sharedApplication] openURL:[NSURL URLWithString:UIApplicationOpenSettingsURLString]];
            }
        }
    }else if (alertView.tag == 101) {
        
        if (buttonIndex == 1) {
            
            [[UIApplication sharedApplication] openURL:[NSURL URLWithString:UIApplicationOpenSettingsURLString]];
        }
    }
}


//+ (BOOL) isOpenLocationService {
//    
//    BOOL status = NO;
//    if ([CLLocationManager locationServicesEnabled]){
//        //        status = YES;
//        return [self isLocationServiceAuthorizationed];
//        //        UIAlertView *alert = [[UIAlertView alloc]initWithTitle:@"定位已开启" message:@"定位已开启，继续使用" delegate:[UnityDelegate shareInstance] cancelButtonTitle:@"知道啦~" otherButtonTitles:nil, nil];
//        //        [alert show];
//        
//    }else {
//        double v = [[[UIDevice currentDevice] systemVersion] doubleValue];
//        if (v >= 10.0) {
//            UIAlertView *alert = [[UIAlertView alloc]initWithTitle:@"定位服务未开启" message:@"定位服务未开启，请进入系统【设置】>【隐私】>【定位服务】中打开定位开关，并允许369互娱麻将使用定位服务" delegate:[UnityDelegate shareInstance] cancelButtonTitle:@"知道啦~" otherButtonTitles:nil, nil];
//            [alert show];
//        }else {
//            UIAlertView *alertios9 = [[UIAlertView alloc]initWithTitle:@"定位服务未开启" message:@"定位服务未开启，请进入系统【设置】>【隐私】>【定位服务】中打开定位开关，并允许369互娱麻将使用定位服务" delegate:[UnityDelegate shareInstance] cancelButtonTitle:@"取消" otherButtonTitles:@"设置", nil];
//            alertios9.tag = 100;
//            [alertios9 show];
//        }
//        //        UIAlertView *alertios9 = [[UIAlertView alloc]initWithTitle:@"定位服务未开启" message:@"定位服务未开启，请进入系统【设置】>【隐私】>【定位服务】中打开定位开关，并允许369互娱麻将使用定位服务" delegate:[UnityDelegate shareInstance] cancelButtonTitle:@"取消" otherButtonTitles:@"设置", nil];
//        //        alertios9.tag = 100;
//        //        [alertios9 show];
//        
//    }
//    return status;
//}
//
//
//+ (BOOL) isLocationServiceAuthorizationed {
//    
//    BOOL status = NO;
//    if (([CLLocationManager authorizationStatus] == kCLAuthorizationStatusAuthorizedWhenInUse
//         || [CLLocationManager authorizationStatus] == kCLAuthorizationStatusAuthorizedAlways)) {
//        status = YES;
//        //        UIAlertView *alert = [[UIAlertView alloc]initWithTitle:@"已授权" message:@"已授权定位服务，请继续使用" delegate:[UnityDelegate shareInstance] cancelButtonTitle:@"知道啦~" otherButtonTitles:nil, nil];
//        //        [alert show];
//        
//    } else {
//        
//        UIAlertView *alertios9 = [[UIAlertView alloc]initWithTitle:@"定位服务未授权" message:@"定位服务未授权，请进入系统【设置】>【隐私】>【定位服务】中打开隐私开关，并允许369互娱麻将使用定位服务" delegate:[UnityDelegate shareInstance] cancelButtonTitle:@"取消" otherButtonTitles:@"设置", nil];
//        alertios9.tag = 101;
//        [alertios9 show];
//        
//        
//        //            double v = [[[UIDevice currentDevice] systemVersion] doubleValue];
//        //            if (v >= 10.0) {
//        //                UIAlertView *alert = [[UIAlertView alloc]initWithTitle:@"定位服务未授权" message:@"定位服务未授权，请进入系统【设置】>【隐私】>【定位服务】中打开隐私开关，并允许369互娱麻将使用定位服务" delegate:[UnityDelegate shareInstance] cancelButtonTitle:@"知道啦~" otherButtonTitles:nil, nil];
//        //                alert.tag = 101;
//        //                [alert show];
//        //            }else {
//        //                UIAlertView *alertios9 = [[UIAlertView alloc]initWithTitle:@"定位服务未授权" message:@"定位服务未授权，请进入系统【设置】>【隐私】>【定位服务】中打开隐私开关，并允许369互娱麻将使用定位服务" delegate:[UnityDelegate shareInstance] cancelButtonTitle:@"取消" otherButtonTitles:@"设置", nil];
//        //                alertios9.tag = 101;
//        //                [alertios9 show];
//        //            }
//        
//        
//    }
//    
//    return status;
//}

@end
