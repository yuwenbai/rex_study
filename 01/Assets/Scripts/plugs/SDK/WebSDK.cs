/**
 * @Author JEFF
 *
 *
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using projectQ;

public class WebSDKParams : SDKParams
{

    public WebSDKParams(string linkConfKey)
    {
        LinkConf linkConf = SDKManager.Instance.GetDataByKey(linkConfKey);
        _url = linkConf.Url;
    }


}
public class WebSDK : SDKBase
{
    public void OpenUrl(LinkConf linkConf, SDKParams param)
    {
        WebSDKParams paramSdk = param as WebSDKParams;
        if (paramSdk == null)
        {
            QLoger.ERROR("WebSDKParams is null");
            return;
        }
        //打开WEB页面
        string url = linkConf.Url;
        if (url == null || url.Trim().Length <= 0)
        {
            WindowUIManager.Instance.CreateTip("当前功能出错，请联系客服，谢谢！");
            return;
        }

#if UNITY_ANDROID
        	OpenWebUrl(paramSdk.URL);
#elif UNITY_IPHONE
//			OpenWebUrl(paramSdk.URL);
		IOSManager.Instance.OpenWebView(_errorMessage,paramSdk.URL);
#else
            WindowUIManager.Instance.CreateTip("当前功能出错，请联系客服，谢谢！");
            QLoger.LOG(string.Format("当前功能出错，请联系客服，谢谢！{0}", url));
#endif
    }
    #region WebView界面的开启和关闭 -------------------------------------------
    #region unity跟Android交互接口 --------------------------------------------
    /// <summary>
    /// 打开一个WebView界面
    /// </summary>
    private void OpenWebUrl(string url)
    {
#if UNITY_ANDROID || UNITY_IOS
        AndroidManager.Instance.OpenWebViewVar(_errorMessage, url);
#else
        QLoger.LOG("OpenWebUrl Falil  : " + url);
#endif
    }

    /// <summary>
    /// 关闭所有的WebView界面
    /// </summary>
    public void HiddenWebView()
    {
#if UNITY_ANDROID || UNITY_IOS
        AndroidManager.Instance.HiddenWebView();
#else
        QLoger.LOG("HiddenWebView Falil");
#endif
    }

    public bool IsOpenWebView()
    {
        return AndroidManager.Instance.IsOpenWebView();
    }
    #endregion ----------------------------------------------------------------
    #endregion ----------------------------------------------------------------
}
