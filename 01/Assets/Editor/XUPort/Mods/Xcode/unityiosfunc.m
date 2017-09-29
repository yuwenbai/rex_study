

#import "UnityDelegateAPI.h"

#if defined (_cplusplus)
extern "C"
{
#endif
    
    
    /**
     请求微信登录
     */
    void IOSWXLogin()
    {
        [[UnityDelegate shareInstance] requestLogin];
    }
    
    /**
     检测微信安装
     */
    void IOSWXInstalled()
    {
        [[UnityDelegate shareInstance] checkWXInstalled];
    }
    
    void GetOpenPara()
    {
        [[UnityDelegate shareInstance] getOpenPara];
    }
    
    
    /**
     微信分享

     @param sharingMsg 分享信息
     */
    void WXSharing(const char * sharingMsg)
    {
        [[UnityDelegate shareInstance] shareMessage:sharingMsg];
    }
    
    
    
    /**
     复制string到剪切板

     @param message
     */
    void PutStringToClipboard(const char * message)
    {
        [[UnityDelegate shareInstance] copyStringToClipboard:message];
    }
    
    
    /**
     微信支付
     
     @param message 支付信息
     */
    void IOSWXPay(const char * message)
    {
        [[UnityDelegate shareInstance] requestPay:message];
    }
    
    
    // 切换竖屏
    void chageToPortraitOrientation() {
        [[UnityDelegate shareInstance] chageToPortraitOrientation];
    }

    // 切换横屏
    void changeToOrientationLandscape() {
         [[UnityDelegate shareInstance] changeToOrientationLandscape];
    }
    
    // 切换屏幕方向
    void ChageToOrientationByUnity(int orientaion) {
        [[UnityDelegate shareInstance] chageToOrientation:orientaion];
        
    }
    
    
    /**
     检测iOS设备定位服务

     @return
     */
    bool IsOpenLocationService () {
        return [[UnityDelegate shareInstance] isOpenLocationService];
        
    }
    
    
    /**
     检测iOS设备定位授权

     @return
     */
    bool IsAuthoriseLocationService () {
        return [[UnityDelegate shareInstance] isLocationServiceAuthorizationed];;
    }
    

    void OpenWebViewController (const char * url) {
        NSString *urlString = [NSString stringWithUTF8String:url];
        [[UnityDelegate shareInstance] openWebViewController:urlString];
        
    }
    
    
    /**
     开始获取GPS信息
     */
    void UnityRequestStartUpdateGPS() {
        [[UnityDelegate shareInstance] startUpdateGPS];
        
    }
    
    
    /**
     停止获取GPS信息
     */
    void UnityRequestStopUpdateGPS() {
        [[UnityDelegate shareInstance] stopUpdateGPS];
        
    }
    
    
    /**
      获取网络类型

     @return
     */
    int UnityRequestNetworkState () {
        return  [[UnityDelegate shareInstance] getNetworkState];
        
    }
    
    
    /**
     获取WiFi信号强度

     @return
     */
    int UnityRequestWiFiStrenth () {
        return  [[UnityDelegate shareInstance] getWiFiSignalStrenth];
        
    }

    
    /**
     获取当前电量

     @return
     */
    int UnityRequestBatteryLevel() {
        return  [[UnityDelegate shareInstance] getBatteryLevel];
        
    }
    
    
#if defined (_cplusplus)
}
#endif
