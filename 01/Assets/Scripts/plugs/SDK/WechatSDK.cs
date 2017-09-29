/**
 * @Author JEFF
 *
 *
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using projectQ;
public enum ShareTypeEnum
{
    /// <summary>
    /// Null
    /// </summary>
    Null,
    /// <summary>
    /// 分享到朋友圈
    /// </summary>
    WXSceneTimeline,
    /// <summary>
    /// 分享到会话
    /// </summary>
    WXSceneSession
}
public class WechatSDK : SDKBase
{

    /// <summary>
    /// 登录接口
    /// </summary>
    public  void WX_Login()
    {
#if UNITY_ANDROID && !UNITY_EDITOR
        AndroidManager.Instance.LogIn();
#elif UNITY_IPHONE
		IOSManager.Instance.LogIn();
		#endif
    }
    /// <summary>
    /// 微信分享
    /// </summary>
    /// <param name="funcName"></param>
    /// <param name="shareParams"></param>
    public void WX_Share(string funcName, SDKParams shareParams)
    {
        WXShareParams param = shareParams as WXShareParams;
        if (param == null)
        {
            QLoger.ERROR("WX_Share WXShareParams is null");
            return;
        }
        //微信分享
        string urlStr = param.URL;
        string titleStr = param.Title;
        string descStr = param.Desc;
        int wxType = (int)param.WXType; //分享类型：1.朋友圈 2好友 3.收藏
        int shareType = (int)param.WXShareType; //分享内容：1.文字 2图片 3.音乐
        string bitMapStr = param.BitMapStr;
        string key = param.KEY;
        QLoger.LOG("WXShare url:" + urlStr);
        QLoger.LOG("WXShare title:" + titleStr);
        QLoger.LOG("WXShare desc:" + descStr);
        QLoger.LOG("WXShare type:1.friendCircle 2.friends 3.collect:" + wxType);
        QLoger.LOG("WXShare shareType:1.text 2.picture 3.audio:" + shareType);
        QLoger.LOG("WXShare bitMapStr:" + bitMapStr);
        QLoger.LOG("WXShare key:" + key);
        this.WX_ShareToWeChat(urlStr, titleStr, descStr, bitMapStr, wxType, shareType,key);
    }
    private void WX_ShareToWeChat(string url, string title, string desc, string bitMap, int type, int shareType,string key)
    {
        //发送分享数据
        SDKData.ExWxShare share = new SDKData.ExWxShare();
        share.url = url;
        share.title = title;
        share.description = desc;
        share.type = type;
        share.strPicByte = bitMap;
        share.shareType = shareType;
        share.key = key;
#if UNITY_ANDROID
        AndroidManager.Instance.UnityCallExchange("ShareWebPage", share.ToString());
#endif
#if UNITY_IPHONE
		IOSManager.Instance.IOSWXSharing(share.ToString());
#endif
    }

    /// <summary>
    /// 检测是否安装了微信
    /// </summary>
    public void WX_Installed()
    {
#if UNITY_ANDROID && !UNITY_EDITOR
        AndroidManager.Instance.WxInstalled();
#elif UNITY_IOS&&!UNITY_EDITOR
		IOSManager.Instance.CheckWXInstalled();
#endif
    }
}
