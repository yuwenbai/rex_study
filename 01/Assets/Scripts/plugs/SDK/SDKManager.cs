/**
 * @author songqianqian
 * AndroidSDK提供的方法，按照如下方式调用
 * SDKManager.Instance.SDKFunction(FunctionEnumSDKMananger.APPLY_FOR_AGENT);
 * */

using System;
using LitJson;
using UnityEngine;
using System.Collections.Generic;
using projectQ;
using System.Text;

public class SDKManager : MonoBehaviour
{
    private WechatSDK _WechatSDK = new WechatSDK();
    private WebSDK _WebSDK = new WebSDK();
    private ClipBoardSDK _ClipboardSDK = new ClipBoardSDK();
    private static SDKManager _Instance;
    public static SDKManager Instance
    {
        get { return _Instance; }
    }

    private string _errorMessage;

    void Awake()
    {
        _Instance = this;
    }

    void OnDestroy()
    {
        _Instance = null;
    }

    #region 读取xml配置 -----------------------------------------

    private List<LinkConf> _linkConfList = new List<LinkConf>();

    /// <summary>
    /// 读取xml配置
    /// </summary>
    public void WebWeChatData_LoadXml()
    {
        if (_linkConfList.Count <= 0)
        {
            List<BaseXmlBuild> buildList = MemoryData.XmlData.XmlBuildDataDic["LinkConf"];
            foreach (BaseXmlBuild build in buildList)
            {
                LinkConf info = (LinkConf)build;
                _linkConfList.Add(info);
            }
        }
    }

    /// <summary>
    /// 通过key查找一条数据
    /// </summary>
    public LinkConf GetDataByKey(string key)
    {
        LinkConf data = new LinkConf();
        for (int i = 0; i < _linkConfList.Count; i++)
        {
            if (key == _linkConfList[i].Key)
            {
                data = _linkConfList[i];
                break;
            }
        }
        return data;
    }

    #endregion --------------------------------------------------

    /// <summary>
    /// 解析原生平台发过来的Json数据
    /// </summary>
    public void ResolutionJson(string str)
    {
        projectQ.QLoger.LOG(projectQ.LogType.ELog, "接送到JSON数据" + str);

        JsonData data = JsonMapper.ToObject(str);
        string key = data["k"].ToString();
        string v = data["v"].ToString();
        string e = data["e"].ToString();
        switch (key)
        {

            default: //默认发送的数据没有解析，直接LOG
#if __DEBUG
                //NGUIDebug.Log (str);
#endif
                break;
            case "MSG":
                {
#if __DEBUG
                    //NGUIDebug.Log (v);
#endif
                }
                break;

            case "LIFECYCLE":
                {
                    if ("onPause".Equals(v.Trim()))
                    {
                        _R.SetToSleep();
                    }
                    else if ("onResume".Equals(v.Trim()))
                    {
                        this.WX_OpenPara();
                        _R.SetToAwake();
                    }
                    else if ("onKeyBack".Equals(v.Trim()))
                    {
                        EventDispatcher.FireSysEvent(GEnum.NamedEvent.SysAndroid_GoBack_Button_Click);
                        DebugPro.DebugError("============onKeyBack====");
                    }
                }
                break;
            case "WXTempCode":
                {
                    EventDispatcher.FireSysEvent(GEnum.NamedEvent.SysUI_SmallLoading_Open, "WXLogin");

                    projectQ.QLoger.LOG(projectQ.LogType.ELog, " #[Unity - WXTempCode]# 登录成功 " + MemoryData.SDKData.Wx.Tcode);
                    if (MemoryData.SDKData.Wx.Tcode == "")
                    {
                        //服务器登陆失败，返回错误码的时候把该值置成空串
                        MemoryData.SDKData.Wx.Tcode = v;
                        projectQ.EventDispatcher.FireSysEvent(GEnum.NamedEvent.ELoginWXResult);
                    }
                }
                break;
            case "WXInstalled":
                {
                    projectQ.QLoger.LOG(projectQ.LogType.ELog, " #[Unity - WXInstalled]# 有无微信结果返回 ");
                    projectQ.EventDispatcher.FireSysEvent(GEnum.NamedEvent.EWXInstalledResult, v);
                }
                break;
            case "ShareWebPage":
                {
                    EventDispatcher.FireSysEvent(GEnum.NamedEvent.SysUI_SmallLoading_Close, "ShareWebPage");
                    if (e.Length > 0)
                    {
                        //失败
                        //WindowUIManager.Instance.CreateTip(string.Format("分享失败({0})", e));
                    }
                    else
                    {
                        projectQ.QLoger.LOG(" #[Unity - WXShareWebPage]# 分享成功 ");
                    }

                    /*
                    try{ MemoryData.SDKData.Wx.shareResult = (SDKData.WXCODE)int.Parse(v); }
                    catch{ MemoryData.SDKData.Wx.shareResult = SDKData.WXCODE.WX_SHARE_OK; }
                    */
                }
                break;
            case "OpenPara":
                {
                    if (v.Length > 0)
                    {
                        SDKData.ExUrlVars paraValue = SDKData.ExUrlVars.FromJson(v);

                        WXOpenParaEnum initKey = (WXOpenParaEnum)Enum.Parse(typeof(WXOpenParaEnum), paraValue.v_a);
                        string[] initValue = new string[6];
                        initValue[0] = paraValue.v_pa;
                        initValue[1] = paraValue.v_pb;
                        initValue[2] = paraValue.v_pc;
                        initValue[3] = paraValue.v_pd;
                        initValue[4] = paraValue.v_pe;
                        initValue[5] = paraValue.v_j;

                        MemoryData.InitData.SetDataValue(initKey, initValue);
                        EventDispatcher.FireSysEvent(GEnum.NamedEvent.WeiXinDataResponse);
                        projectQ.QLoger.LOG(" #[Unity - OpenPara]# paraValue.v_a =" + paraValue.v_a);
                    }
                }
                break;
            case "WXLogin":
                {
                    //EventDispatcher.FireSysEvent(GEnum.NamedEvent.SysUI_SmallLoading_Close, "WXLogin");
                    if (e.Length > 0)
                    {
                        if (e != "USER_CANCEL")
                        {
                            // 微信登录失败，排除用户自己取消的情况
                            WindowUIManager.Instance.CreateTip("微信授权登录失败");
                        }
                    }
                    else
                    {
                        projectQ.QLoger.LOG(" #[Unity - WXLogin]# 登录成功 ");
                    }
                }
                break;
            case "WXPay":
                {
                    EventDispatcher.FireSysEvent(GEnum.NamedEvent.SysUI_SmallLoading_Close, "WXPay");
                    if (!string.IsNullOrEmpty(e))
                    {
                        //失败
                        //WindowUIManager.Instance.CreateTip(string.Format("微信支付失败({0})", e));
                    }
                    else
                    {
                        projectQ.QLoger.LOG(" #[Unity - WXPay]# 支付成功 ");
                    }
                }
                break;
            case "GetDataFromClipboard":
                _ClipboardSDK.GetDataFromClipboard(v);
                break;
            case "CheckGPSIsOpen":
                GPSManager.Instance.GPSServeResponse("true" == v);
                break;
            //GPS 变化通知
            case "GPSServerDataChanged":
                {
                    var gpsData = AMapShareToGpsData(v);
                    if (gpsData != null)
                    {
                        GPSManager.Instance.AddGPSData(gpsData);
                    }
                }
                break;
            //GPS 请求返回
            case "GetGPSServerDataRet":
                {
                    var gpsData = AMapShareToGpsData(v);
                    if (gpsData != null)
                    {
                        GPSManager.Instance.AddGPSData(gpsData);
                    }
                }
                break;
            //wifi 请求返回
            case "GetWifiLevelRet":
                {
                    //TODO
                    EventDispatcher.FireEvent(GEnum.EnumWifiAndBattery.Data_Wifi.ToString(), v);
                }
                break;
            //電量 请求返回
            case "GetBatteryRet":
                {
                    //TODO
                    EventDispatcher.FireEvent(GEnum.EnumWifiAndBattery.Data_Battery.ToString(), v);
                }
                break;
            //获取网络类型
            case "GetNetWorkTypeRet":
                {
                    //TODO
                    EventDispatcher.FireEvent(GEnum.EnumWifiAndBattery.Data_NetType.ToString(), v);
                }
                break;
            case "GetGpsByAddressRet":
                //SDKData.AMapAddress gpsValue = SDKData.AMapAddress.FromJson(v);

                //projectQ.QLoger.LOG(" #[Unity - GetGPSServerDataRet]# " + gpsValue.latitude);
                //String aaa = gpsValue.latitude + " " + gpsValue.longitude + " status " + gpsValue.status;
                //GPSManager.Instance.SetInitGPSData(aaa);
                // GPSManager.Instance.GPSServeRequest();
                break;
        }
    }

    /// <summary>
    /// 安装apk
    /// </summary>
    public void InstallAPK(String data)
    {
#if UNITY_ANDROID && !UNITY_EDITOR
        AndroidManager.Instance.UnityCallExchange("InstallApk", data);
#elif UNITY_IOS
                //TODO
                //IOSManager.Instance.checkLocationServiceOpenStatus();
#endif
    }
    /// <summary>
    /// 获取电量
    /// </summary>
    public void GetBattery()
    {
#if UNITY_ANDROID && !UNITY_EDITOR
        AndroidManager.Instance.UnityCallExchange("GetBattery","");
#elif UNITY_IOS
                //TODO
                //IOSManager.Instance.checkLocationServiceOpenStatus();
#endif
    }
    /// <summary>
    /// 获取wifi
    /// </summary>
    /// 
    //function :    GetWifiLevel
    //return level:
    //    5   信号最好
    //    4   信号较好
    //    3   信号一般
    //    2   信号较差
    //    1   无

    //return level:
    public void GetWifiLevel()
    {
#if UNITY_ANDROID && !UNITY_EDITOR
        AndroidManager.Instance.UnityCallExchange("GetWifiLevel","");
#elif UNITY_IOS
        //TODO
        //IOSManager.Instance.checkLocationServiceOpenStatus();
#endif
    }
    /// <summary>
    /// 获取网络类型
    /// </summary>
    //   0 NETWORN_NONE
    //   1 NETWORN_2G
    //   2 NETWORN_3G
    //   3 NETWORN_4G
    //   4 NETWORN_WIFI
    //   5 NETWORN_MOBILE //异常情况返回5
    public void GetNetWorkType()
    {
#if UNITY_ANDROID && !UNITY_EDITOR
        AndroidManager.Instance.UnityCallExchange("GetNetWorkType","");
#elif UNITY_IOS
        //TODO
        //IOSManager.Instance.checkLocationServiceOpenStatus();
#endif
    }
    private GPSData AMapShareToGpsData(string v)
    {
        SDKData.AMapShare Value = SDKData.AMapShare.FromJson(v);
        if (Value == null) return null;

        var gpsData = new GPSData();
        //纬度
        float tempFloat = 0f;
        if (float.TryParse(Value.latitude, out tempFloat))
        {
            gpsData.Latitude = tempFloat;
        }
        else
        {
            gpsData.Latitude = 0f;
        }
        tempFloat = 0f;
        //经度
        if (float.TryParse(Value.longitude, out tempFloat))
        {
            gpsData.Longitude = tempFloat;
        }
        else
        {
            gpsData.Longitude = 0f;
        }
        //时间戳
        long tempLong = 0;
        if (long.TryParse(Value.time, out tempLong))
        {
            gpsData.Timestamp = tempLong;
        }
        //中文地址
        JsonData dataAddress = Value.address;
        if (dataAddress != null)
        {
            bool isExist = false;
            foreach (var ii in dataAddress.Keys)
            {
                if (ii == "k")
                {
                    isExist = true;
                    break;
                }
            }
            if (isExist)
            {
                gpsData.Address = dataAddress["k"].ToString();
            }
        }

        gpsData.locType = Value.locType;
        gpsData.status = Value.status;
        return gpsData;
    }

    /// <summary>
    /// 将文本放入系统剪切板里
    /// </summary>
    /// <param name="inputStr"></param>
    public void PutStringToClipboard(SDKData.ClipboardData data, System.Action<string> callback)
    {
        _ClipboardSDK.SendDataToClipboard(data, callback);
    }


    public void SDKFunction(int type, params object[] vars)
    {
        switch (type)
        {
            //case 1000:
            //    EventDispatcher.FireSysEvent(GEnum.NamedEvent.SysUI_SmallLoading_Open, "ShareWebPage");
            //    //微信分享
            //    this.WX_ShareToWeb("app://com.slj.gamemj?v_a=1&v_pa=1&v_pb=1&v_pc=1&v_pd=1&v_pe=1&v_j=1", "我是百度", "DESC", "", 2);
            //    break;
            case 5:
                //微信登陆
                EventDispatcher.FireSysEvent(GEnum.NamedEvent.SysUI_SmallLoading_Open, "WXLogin");
                StartCoroutine(UITools.WaitExcution(_WechatSDK.WX_Login, 0.5f));
                break;
            case 6:
                //微信支付
                EventDispatcher.FireSysEvent(GEnum.NamedEvent.SysUI_SmallLoading_Open, "WXPay");
                this.WX_Pay(vars[0] as string);
                break;
            case 10:
                //获取唤醒游戏的参数
                this.WX_OpenPara();
                break;
            case 15:
                //检测有没有微信
                _WechatSDK.WX_Installed();
                // this.WX_Installed();
                break;
            default:
                break;
        }
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="funcName"></param>
    /// <param name="vars"></param>
    public void SDKFunction(string funcName, SDKParams param)
    {
        LinkConf linkConf = GetDataByKey(funcName);
        //WebWeChat data = XmlWebWeChatData.GetDataByName(funcName);
        if (linkConf == null)
        {
            WindowUIManager.Instance.CreateTip("当前功能出错，请联系客服，谢谢！");
            return;
        }

        int type = int.Parse(linkConf.Type);

        switch (type)
        {
            case 2:
                _WebSDK.OpenUrl(linkConf, param);
                break;
            case 5:
                this._WechatSDK.WX_Login();
                break;
            case 3:
                {
                    Application.OpenURL(linkConf.Url);
                }
                break;
            case 1:
                _WechatSDK.WX_Share(funcName, param);
                break;
            default:
                break;
        }
    }


    /// <summary>
    /// 关闭所有的WebView界面
    /// </summary>
    public void HiddenWebView()
    {
        this._WebSDK.HiddenWebView();
    }

    public bool IsOpenWebView()
    {
        return this._WebSDK != null && this._WebSDK.IsOpenWebView();
    }

    public void InitWebView()
    {

#if UNITY_IPHONE
        AndroidManager.Instance.InitWebView();
#endif
    }


    /// <summary>
    /// 支付接口
    /// </summary>
    private void WX_Pay(string json)
    {
#if __DEBUG
        NGUIDebug.Log("微信支付" + json);
#endif

#if UNITY_ANDROID && !UNITY_EDITOR
		AndroidManager.Instance.WXPay(json);
#elif UNITY_IPHONE
		IOSManager.Instance.WXPay(json);
#endif
    }
    /// <summary>
    /// 唤醒游戏接口
    /// </summary>
    private void WX_OpenPara()
    {
#if UNITY_ANDROID && !UNITY_EDITOR
        AndroidManager.Instance.OpenPara();
#elif UNITY_IOS && !UNITY_EDITOR
		IOSManager.Instance.IOSGetOpenPara();
#endif
    }

    ///// <summary>
    ///// 应用GPS权限请求
    ///// </summary>
    //public void GPSAuthorityRequest()
    //{

    //}

    ///// <summary>
    ///// 应用GPS权限回复
    ///// </summary>
    ///// <param name="isOpen">是否开启</param>
    //public void GPSAuthorityResponse(bool isOpen)
    //{

    //}

    void OnGUI()
    {
#if __DEBUG
        /*
        if (GUI.Button(new Rect(10, 100, 240, 120), "******微信登陆******"))
        {
            this.SDKFunction(5);
        }

        if (GUI.Button(new Rect(10, 400, 240, 120), "******微信分享******"))
        {
            //this.SDKFunction("WEB_OPEN_AGENT_1", MemoryData.UserID);
            this.SDKFunction(1000);
        }

		if (GUI.Button (new Rect (10, 200, 120, 90), "WXPay")) 
        {
			Application.OpenURL("http://www.baidu.com");
		}
        */
#endif
    }
}

namespace projectQ
{
    public class SDKData
    {
        public enum WXCODE
        {
            WX_VOID = 0,

            WX_SHARE_OK = 100,
            WX_SHARE_CANSLE = 101,

            WX_END = 9999
        }

        public WXData Wx = new WXData();
        public class WXData
        {
            private string _tcode = "";
            public string Tcode
            {
                get { return _tcode; }
                set { _tcode = value; }
            }

            public string atoken;
            public string openid;

            public WXCODE shareResult;
        }
        /// <summary>
        /// 剪切板数据
        /// </summary>
        public class ClipboardData
        {
            public string data = "";
            public ClipboardData()
            {
            }
            public ClipboardData(string data)
            {
                this.data = data;
            }
            public static ClipboardData FromJson(String json)
            {
                JsonData data = JsonMapper.ToObject(json);
                ClipboardData clipboardData = new ClipboardData();
                clipboardData.data = data["data"].ToString();
                return clipboardData;
            }
            protected String toJson()
            {
                JsonData data = new JsonData();
                data["data"] = this.data;
                return data.ToJson();
            }
            public override String ToString()
            {
                return this.toJson();
            }
        }
        public class ExWxShare
        {
            public string key = "";
            public string url = "";
            public int type = 0;
            public string title = "";
            public string description = "";
            public string strPicByte = "";
            public int shareType = 0;
            public ExWxShare() { }

            public ExWxShare(string url, string title, string description, string strPicByte, int type, int shareType)
            {
                this.url = url;
                this.title = title;
                this.description = description;
                this.type = type;
                this.shareType = shareType;
            }

            public static ExWxShare FromJson(String json)
            {
                JsonData data = JsonMapper.ToObject(json);
                ExWxShare share = new ExWxShare();

                share.url = data["url"].ToString();
                share.title = data["title"].ToString();
                share.description = data["description"].ToString();
                share.strPicByte = data["strPicByte"].ToString();
                share.type = int.Parse(data["type"].ToString());
                share.shareType = int.Parse(data["shareType"].ToString());
                return share;
            }

            protected String toJson()
            {
                JsonData data = new JsonData();
                data["url"] = this.url;
                data["title"] = this.title;
                data["description"] = this.description;
                data["strPicByte"] = this.strPicByte;
                data["type"] = this.type;
                data["shareType"] = this.shareType;
                return data.ToJson();
            }

            public override String ToString()
            {
                return this.toJson();
            }
        }

        public class ExUrlVars
        {
            public string v_a;
            public string v_pa;
            public string v_pb;
            public string v_pc;
            public string v_pd;
            public string v_pe;
            public string v_j;

            public static ExUrlVars FromJson(String json)
            {
                JsonData data = JsonMapper.ToObject(json);
                ExUrlVars vars = new ExUrlVars();

                vars.v_a = data["v_a"].ToString();
                vars.v_pa = data["v_pa"].ToString();
                vars.v_pb = data["v_pb"].ToString();
                vars.v_pc = data["v_pc"].ToString();
                vars.v_pd = data["v_pd"].ToString();
                vars.v_pe = data["v_pe"].ToString();
                vars.v_j = data["v_j"].ToString();

                return vars;
            }

            protected string toJson()
            {
                JsonData data = new JsonData();
                data["v_a"] = this.v_a;
                data["v_pa"] = this.v_pa;
                data["v_pb"] = this.v_pb;
                data["v_pc"] = this.v_pc;
                data["v_pd"] = this.v_pd;
                data["v_pe"] = this.v_pe;
                data["v_j"] = this.v_j;

                return data.ToJson();
            }

            public string toString()
            {
                return this.toJson();
            }
        }

        public class AMapShare
        {
            public string latitude = "";
            public string longitude = "";
            public JsonData address;
            public string time = "";
            public string locType;
            public string status;
            //public string strPicByte = "";
            //public int shareType = 0;
            public AMapShare() { }

            //public ExWxShare(string url, string title, string description, string strPicByte, int type, int shareType)
            //{
            //    this.url = url;
            //    this.title = title;
            //    this.description = description;
            //    this.type = type;
            //    this.shareType = shareType;
            //}

            public static AMapShare FromJson(String json)
            {
                JsonData data = JsonMapper.ToObject(json);
                AMapShare share = new AMapShare();

                share.latitude = data["latitude"].ToString();
                share.longitude = data["longitude"].ToString();
                share.address = data["address"];
                share.time = data["time"].ToString();
                share.locType = data["locType"].ToString();
                share.status = data["status"].ToString();
                return share;
            }
        }
        public class AMapAddress
        {
            public string latitude = "";
            public string longitude = "";
            public string status = "";
            public string time = "";
            public string address = "";
            public string citycode = "";
            public AMapAddress() { }

            public static AMapAddress FromJson(String json)
            {
                JsonData data = JsonMapper.ToObject(json);
                AMapAddress share = new AMapAddress();

                share.latitude = data["latitude"].ToString();
                share.longitude = data["longitude"].ToString();
                share.status = data["status"].ToString();
                return share;
            }
            protected string toAddressJson()
            {
                JsonData data = new JsonData();
                data["address"] = this.address;
                data["citycode"] = this.citycode;

                return data.ToJson();
            }

            public string toAddressString()
            {
                return this.toAddressJson();
            }
        }

        public class InstallAPK
        {
            public string path = "";
            public string name = "";
            public InstallAPK() { }

            protected string toJson()
            {
                JsonData data = new JsonData();
                data["k"] = this.path;
                data["v"] = this.name;

                return data.ToJson();
            }

            public string toString()
            {
                return this.toJson();
            }
        }
    }

    partial class MKey
    {
        public const string SDK_DATA = "SDK_DATA";
    }

    partial class MemoryData
    {
        static public SDKData SDKData
        {
            get
            {
                SDKData data = MemoryData.Get<SDKData>(MKey.SDK_DATA);
                if (data == null)
                {
                    data = new SDKData();
                    MemoryData.Set(MKey.SDK_DATA, data);
                }
                return data;
            }
        }
    }

}