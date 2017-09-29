//
//  GaoLocation.m
//  LocationDemo
//
//  Created by developer on 2017/8/16.
//  Copyright © 2017年 kang. All rights reserved.
//

#import "AMapLocation.h"
#import <AMapFoundationKit/AMapFoundationKit.h>
#import <AMapLocationKit/AMapLocationKit.h>
#import "UnityDelegateAPI.h"


const static NSString *APIKey = @"e59ff2fe0bec617d3806c86d2607dfad";

@interface AMapLocation() <AMapLocationManagerDelegate>
{
    BOOL _isUpdating;
    CLLocation *_cacheLocation;
    
}
@property (nonatomic, strong) AMapLocationManager *locationManager;
@property (nonatomic, strong) NSMutableDictionary *locationDic;
@end


@implementation AMapLocation

#pragma mark - init
+ (instancetype)allocWithZone:(struct _NSZone *)zone
{
    static AMapLocation *aMaplocation = nil;
    static dispatch_once_t onceToken;
    dispatch_once(&onceToken, ^{
        aMaplocation = [super allocWithZone:zone];
        [aMaplocation setting];
        
    });
    return aMaplocation;
}

+ (instancetype) shareInstance {
    return [self allocWithZone:nil];
}

- (id) copyWithZone:(NSZone *)zone;{
    return self;
}


#pragma mark - AMap setting

- (void) setting {

    [self configAPIKey];
    
    [self configLocationManager];
}

- (void) configAPIKey {
    [AMapServices sharedServices].apiKey = (NSString *)APIKey;
}

- (void) configLocationManager
{
    self.locationManager = [[AMapLocationManager alloc] init];
    self.locationManager.distanceFilter = 10;
    
    //设置期望定位精度
    [self.locationManager setDesiredAccuracy:kCLLocationAccuracyBest];
    
    //设置不允许系统暂停定位
    [self.locationManager setPausesLocationUpdatesAutomatically:NO];
    
    //设置允许在后台定位
    [self.locationManager setAllowsBackgroundLocationUpdates:NO];
    [self.locationManager setLocatingWithReGeocode:YES];
    
}
    

#pragma mark - method

- (void) setCacheLocation:(CLLocation *)cacheLocation {
    if (_cacheLocation != cacheLocation) {
        _cacheLocation = cacheLocation;
    }
}

- (void) setLocationDic:(NSMutableDictionary *)locationDic {

    if (locationDic != _locationDic) {
        _locationDic = locationDic;
    }
}

//- (void) locationDic {
//
//    if (!_locationDic) {
//        
//    }
//}

- (void) start {
    
    if (!_isUpdating) {
        [self.locationManager startUpdatingLocation];
        [self.locationManager setDelegate:self];
    }
    
    if (_locationDic) {
        [[UnityDelegate shareInstance] returnUnityGPSMessage:_locationDic];
    }
}

- (void) stop {
    
    _isUpdating = NO;
    [self.locationManager stopUpdatingLocation];
    [self.locationManager setDelegate:nil];
}

    

/**
 向Unity发送GPS信息

 @param location 经、纬度、海拔、时间戳
 @param reGeocode 地址码
 */
- (void) sendLocationMessageToUnity:(CLLocation *) location reGeocode:(AMapLocationReGeocode *)reGeocode {

    NSString *longitudeString = [NSString stringWithFormat:@"%f",location.coordinate.longitude];
    NSString *latitudeString = [NSString stringWithFormat:@"%f",location.coordinate.latitude];
    NSString *address = @"";
    //时间戳13位：精确到毫秒
    NSString *timestamp = [NSString stringWithFormat:@"%ld",(long)[location.timestamp timeIntervalSince1970]*1000];
    
    if (reGeocode)
    {
        address = reGeocode.formattedAddress;
    }
    
    
    NSLog(@"location:{lat:%f; lon:%f; accuracy:%f} address:%@", location.coordinate.latitude, location.coordinate.longitude, location.horizontalAccuracy,address);
    NSMutableDictionary *locationDic = [[NSMutableDictionary alloc]init];
    [locationDic setObject:longitudeString forKey:@"longitude"];
    [locationDic setObject:latitudeString forKey:@"latitude"];
    [locationDic setObject:@{@"k":address} forKey:@"address"];
    [locationDic setObject:timestamp forKey:@"time"];
    [locationDic setObject:@"" forKey:@"locType"];
    [locationDic setObject:@"" forKey:@"status"];

    // 缓存GPS信息
    [self setLocationDic:locationDic];
    // 发送
    [[UnityDelegate shareInstance] sendUnityGPSMessage:locationDic];

}
    
#pragma mark - delegate

//- (void)amapLocationManager:(AMapLocationManager *)manager didUpdateLocation:(CLLocation *)location {
//
//// 缓存GPS信息
//[self setCacheLocation:location];
//// 发送GPS信息
//[self sendLocationMessageToUnity:location reGeocode:nil];
//}


// 实现了当前方法就不会调用- (void)amapLocationManager:(AMapLocationManager *)manager didUpdateLocation:(CLLocation *)location方法
- (void)amapLocationManager:(AMapLocationManager *)manager didUpdateLocation:(CLLocation *)location reGeocode:(AMapLocationReGeocode *)reGeocode
{
    // 缓存GPS信息
    [self setCacheLocation:location];
    // 发送GPS信息
    [self sendLocationMessageToUnity:location reGeocode:reGeocode];
    
}





@end
