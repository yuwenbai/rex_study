using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using projectQ;

public class WifiAndBattery : MonoBehaviour
{
    #region wifi组件
    public Transform WifiObj;
    private Transform m_Wifi1;
    private Transform m_Wifi2;
    private Transform m_Wifi3;
    #endregion

    #region 电池组件
    public Transform BatteryObj;
    private Transform m_Battery1;
    private Transform m_Battery2;
    private Transform m_Battery3;
    private Transform m_Battery4;
    private Transform m_Battery5;
    #endregion

    #region 网络信号
    public Transform NetworkObj;
    private Transform m_Network1;
    private Transform m_Network2;
    private Transform m_Network3;
    private Transform m_Network4;
    private Transform m_NoNetMark;
    private UISprite m_MarkSp;
    #endregion

    public UILabel TimerLab;
    
    private void InitObj()
    {
        if (WifiObj != null)
        {
            m_Wifi1 = WifiObj.Find("Wifi1");
            m_Wifi2 = WifiObj.Find("Wifi2");
            m_Wifi3 = WifiObj.Find("Wifi3");

            WifiObj.gameObject.SetActive(false);
        }

        if (BatteryObj != null)
        {
            m_Battery1 = BatteryObj.Find("Battery1");
            m_Battery2 = BatteryObj.Find("Battery2");
            m_Battery3 = BatteryObj.Find("Battery3");
            m_Battery4 = BatteryObj.Find("Battery4");
            m_Battery5 = BatteryObj.Find("Battery5");

            BatteryObj.gameObject.SetActive(false);
        }

        if (NetworkObj != null)
        {
            m_Network1 = NetworkObj.Find("Network1");
            m_Network2 = NetworkObj.Find("Network2");
            m_Network3 = NetworkObj.Find("Network3");
            m_Network4 = NetworkObj.Find("Network4");
            m_NoNetMark = NetworkObj.Find("NoMark");
            Transform markObj = NetworkObj.Find("Mark");
            if (markObj != null)
                m_MarkSp = markObj.GetComponent<UISprite>();

            NetworkObj.gameObject.SetActive(false);
        }

        if (WifiAndBatteryData.Instance == null)
            return;
    }

    private void OnEnable()
    {
        EventDispatcher.AddEvent(GEnum.EnumWifiAndBattery.Refresh_NetType.ToString(), CheckNetType);
        EventDispatcher.AddEvent(GEnum.EnumWifiAndBattery.Refresh_Battery.ToString(), ShowBattery);
        EventDispatcher.AddEvent(GEnum.EnumWifiAndBattery.Refresh_Wifi.ToString(), ShowWifi);

        InitObj();
    } 

    private void Start()
    {
        //TimerLab.text = "发送消息";
#if UNITY_ANDROID && !UNITY_EDITOR
        SDKManager.Instance.GetNetWorkType();
        SDKManager.Instance.GetBattery();
#endif
#if UNITY_IOS && !UNITY_EDITOR
        //CheckNetType(new object[] {
        //    IOSManager.Instance.getNetworkState().ToString()
        //});
        //ShowBattery(new object[] {
        //    IOSManager.Instance.getBatteryLevel().ToString()
        //});
        EventDispatcher.FireEvent(GEnum.EnumWifiAndBattery.Data_NetType.ToString(), IOSManager.Instance.getNetworkState().ToString());
        EventDispatcher.FireEvent(GEnum.EnumWifiAndBattery.Data_Battery.ToString(), IOSManager.Instance.getBatteryLevel().ToString());
#endif
        ShowTimer();
    }

    private void CheckNetType(object[] datas)
    {

        if (m_Network1 == null)
        {
            InitObj();
        }

        if (m_Network1 == null)
        {
            return;
        }

        m_NetType type = (m_NetType)WifiAndBatteryData.Instance.netType;

        bool isNet = type != m_NetType.NETWORN_WIFI;//type != m_NetType.NETWORN_NONE && type != m_NetType.NETWORN_MOBILE && 

        if (NetworkObj.gameObject.activeSelf != isNet)
            NetworkObj.gameObject.SetActive(isNet);

        m_NoNetMark.gameObject.SetActive(isNet && type != m_NetType.NETWORN_2G && type == m_NetType.NETWORN_3G && type == m_NetType.NETWORN_4G);
        m_MarkSp.gameObject.SetActive(isNet && (type == m_NetType.NETWORN_2G || type == m_NetType.NETWORN_3G || type == m_NetType.NETWORN_4G));

        if (WifiObj.gameObject.activeSelf != (type == m_NetType.NETWORN_WIFI))
            WifiObj.gameObject.SetActive(type == m_NetType.NETWORN_WIFI);

        switch (type)
        {
            case m_NetType.NETWORN_2G:
                m_MarkSp.spriteName = "xinhao_2g";
                break;
            case m_NetType.NETWORN_3G:
                m_MarkSp.spriteName = "xinhao_3g";
                break;
            case m_NetType.NETWORN_4G:
                m_MarkSp.spriteName = "xinhao_4g";
                break;
            case m_NetType.NETWORN_WIFI:
#if UNITY_ANDROID && !UNITY_EDITOR
                SDKManager.Instance.GetWifiLevel();
#endif
#if UNITY_IOS && !UNITY_EDITOR
        EventDispatcher.FireEvent(GEnum.EnumWifiAndBattery.Data_Wifi.ToString(), IOSManager.Instance.getWiFiStrenth());
                //ShowWifi(new object[] {
                // IOSManager.Instance.getWiFiStrenth()
                //});
#endif
                break;
            //case m_NetType.NETWORN_NONE:
            //case m_NetType.NETWORN_MOBILE:
            //    break;
            default:
                m_NoNetMark.gameObject.SetActive(true);
                m_MarkSp.gameObject.SetActive(false);
                break;
        }

        if (m_MarkSp.gameObject.activeSelf)
            m_MarkSp.MakePixelPerfect();
    }

    private void ShowWifi(object[] datas)
    {
        if (m_Wifi1 == null)
            InitObj();

        if (m_Wifi1 == null)
        {
            return;
        }

        WifiObj.gameObject.SetActive(true);
        
        ChangeWifi(WifiAndBatteryData.Instance.wifi);
    }

    private void ChangeWifi(int num)
    {
        if (m_Wifi1.gameObject.activeSelf != num > (int)m_WifiNum.Wifi_4)
            m_Wifi1.gameObject.SetActive(num > (int)m_WifiNum.Wifi_4);

        if (m_Wifi2.gameObject.activeSelf != num > (int)m_WifiNum.Wifi_3)
            m_Wifi2.gameObject.SetActive(num > (int)m_WifiNum.Wifi_3);

        if (m_Wifi3.gameObject.activeSelf != num > (int)m_WifiNum.Wifi_2)
            m_Wifi3.gameObject.SetActive(num > (int)m_WifiNum.Wifi_2);
    }

    private void ShowBattery(object[] datas)
    {
        if (m_Battery1 == null)
            InitObj();

        if (m_Battery1 == null)
        {
            return;
        }

        BatteryObj.gameObject.SetActive(true);
        
        ChangeBattery(WifiAndBatteryData.Instance.battery);
    }

    private void ChangeBattery(int num)
    {
        m_Battery1.gameObject.SetActive(num > 90);
        m_Battery2.gameObject.SetActive(num > 70);
        m_Battery3.gameObject.SetActive(num > 50);
        m_Battery4.gameObject.SetActive(num > 30);
        m_Battery5.gameObject.SetActive(num > 10);
    }

    bool showTime = true;
    int m_FuncIndex = -1;

    private void ShowTimer()
    {
        if (TimerLab == null)
            return;
        TimerLab.text = System.DateTime.Now.ToShortTimeString().ToString();
        m_FuncIndex = TimeTick.Instance.SetAction(CallBack, 1);
    }

    private void CallBack()
    {
        if (m_FuncIndex > 0)
            TimeTick.Instance.RemoveAction(m_FuncIndex);
        if (showTime)
            Start();
    }

    private enum m_WifiNum
    {
        Wifi_None,              //wifi无
        Wifi_2,                 //wifi差
        Wifi_3,                 //wifi中
        Wifi_4,                 //wifi好
        Wifi_5,                 //wifi满
    }

    private enum m_NetType
    {
        NETWORN_NONE = 0,       //没有网
        NETWORN_2G,             //2G
        NETWORN_3G,             //3G
        NETWORN_4G,             //4G
        NETWORN_WIFI,           //wifi
        NETWORN_MOBILE          //erro
    }
}
