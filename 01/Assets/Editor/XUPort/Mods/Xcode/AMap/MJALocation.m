//
//  MJALocation.m
//  LocationDemo
//
//  Created by developer on 2017/8/15.
//  Copyright © 2017年 kang. All rights reserved.
//

#import "MJALocation.h"


#define DefaultLocationTimeout 10
#define DefaultReGeocodeTimeout 5

@interface MJALocation()<AMapLocationManagerDelegate>
{
    
}
@property (nonatomic, copy) AMapLocatingCompletionBlock completionBlock;

//@property (nonatomic, strong) UILabel *displayLabel;

@end

@implementation MJALocation

- (id) init {
    
    self = [super init];
    if (self) {
    }
    
    return self;
}

- (void) setting {
    
    [self initCompleteBlock];
    [self configLocationManager];
    
}

- (void)configLocationManager
{
    self.locationManager = [[AMapLocationManager alloc] init];
    
    [self.locationManager setDelegate:self];
    
    //设置期望定位精度
    [self.locationManager setDesiredAccuracy:kCLLocationAccuracyBest];
    
    //设置不允许系统暂停定位
    [self.locationManager setPausesLocationUpdatesAutomatically:NO];
    
    //设置允许在后台定位
    [self.locationManager setAllowsBackgroundLocationUpdates:NO];
    
    //设置定位超时时间
    [self.locationManager setLocationTimeout:DefaultLocationTimeout];
    
    //设置逆地理超时时间
    [self.locationManager setReGeocodeTimeout:DefaultReGeocodeTimeout];
}


- (void)initCompleteBlock
{
    __weak MJALocation *weakSelf = self;
    self.completionBlock = ^(CLLocation *location, AMapLocationReGeocode *regeocode, NSError *error)
    {
//        NSLog(@"高德定位完成一次");
        if (error != nil && error.code == AMapLocationErrorLocateFailed)
        {
            //定位错误：此时location和regeocode没有返回值，不进行annotation的添加
            NSLog(@"定位错误:{%ld - %@};", (long)error.code, error.localizedDescription);
            return;
        }
        else if (error != nil
                 && (error.code == AMapLocationErrorReGeocodeFailed
                     || error.code == AMapLocationErrorTimeOut
                     || error.code == AMapLocationErrorCannotFindHost
                     || error.code == AMapLocationErrorBadURL
                     || error.code == AMapLocationErrorNotConnectedToInternet
                     || error.code == AMapLocationErrorCannotConnectToHost))
        {
            //逆地理错误：在带逆地理的单次定位中，逆地理过程可能发生错误，此时location有返回值，regeocode无返回值，进行annotation的添加
            NSLog(@"逆地理错误:{%ld - %@};", (long)error.code, error.localizedDescription);
        }
        else if (error != nil && error.code == AMapLocationErrorRiskOfFakeLocation)
        {
            //存在虚拟定位的风险：此时location和regeocode没有返回值，不进行annotation的添加
            NSLog(@"存在虚拟定位的风险:{%ld - %@};", (long)error.code, error.localizedDescription);
            return;
        }
        else
        {
            //没有错误：location有返回值，regeocode是否有返回值取决于是否进行逆地理操作，进行annotation的添加
        }
        
        //修改label显示内容
        if (regeocode)
        {
            NSString *address = [NSString stringWithFormat:@"%@ \n %@-%@-%.2fm", regeocode.formattedAddress,regeocode.citycode, regeocode.adcode, location.horizontalAccuracy];
            if (weakSelf.addressBlock) {
                weakSelf.addressBlock(address);
            }
//            [weakSelf.displayLabel setText:[NSString stringWithFormat:@"%@ \n %@-%@-%.2fm", regeocode.formattedAddress,regeocode.citycode, regeocode.adcode, location.horizontalAccuracy]];
        }
//        else
//        {
//            
////            [weakSelf.displayLabel setText:[NSString stringWithFormat:@"lat:%f;lon:%f \n accuracy:%.2fm", location.coordinate.latitude, location.coordinate.longitude, location.horizontalAccuracy]];
//        }
        
        NSString *longitudeString = [NSString stringWithFormat:@"%f",location.coordinate.longitude];
        NSString *latitudeString = [NSString stringWithFormat:@"%f",location.coordinate.latitude];
        if (weakSelf.locationBlock) {
            weakSelf.locationBlock(longitudeString, latitudeString);
        }
    };
}




- (void)requestLocationAndGeocode
{
    //进行单次带逆地理定位请求
    [self.locationManager requestLocationWithReGeocode:YES completionBlock:self.completionBlock];
}


- (void)requestLocAction
{
    //进行单次定位请求
    [self.locationManager requestLocationWithReGeocode:NO completionBlock:self.completionBlock];
}


#pragma mark - delegate

/**
 *  @brief 连续定位回调函数.注意：如果实现了本方法，则定位信息不会通过amapLocationManager:didUpdateLocation:方法回调。
 *  @param manager 定位 AMapLocationManager 类。
 *  @param location 定位结果。
 *  @param reGeocode 逆地理信息。
 */
- (void)amapLocationManager:(AMapLocationManager *)manager didUpdateLocation:(CLLocation *)location reGeocode:(AMapLocationReGeocode *)reGeocode {

    // 纬度
    CLLocationDegrees latitude = location.coordinate.latitude;
    // 经度
    CLLocationDegrees longitude = location.coordinate.longitude;
//    NSLog(@"%@",[NSString stringWithFormat:@"%lf", location.coordinate.longitude]);
    
    NSString *longitudeString = [NSString stringWithFormat:@"%f",longitude];
    NSString *latitudeString = [NSString stringWithFormat:@"%f",latitude];
    if (self.locationBlock) {
        self.locationBlock(longitudeString, latitudeString);
    }
}



- (void) start {
    //停止定位
    [self.locationManager startUpdatingLocation];
    [self.locationManager setDelegate:self];
}

- (void) stop {
    
    //停止定位
    [self.locationManager stopUpdatingLocation];
    [self.locationManager setDelegate:nil];
}

@end
