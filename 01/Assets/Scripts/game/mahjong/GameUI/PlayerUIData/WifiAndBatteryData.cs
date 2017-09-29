using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using projectQ;

public class WifiAndBatteryData
{
    private static WifiAndBatteryData m_Instance;

    public static WifiAndBatteryData Instance
    {
        get
        {
            if (m_Instance == null)
                m_Instance = new WifiAndBatteryData();
            return m_Instance;
        }
    }
    private WifiAndBatteryData()
    {
        EventDispatcher.AddEvent(GEnum.EnumWifiAndBattery.Data_NetType.ToString(), SetNetType);
        EventDispatcher.AddEvent(GEnum.EnumWifiAndBattery.Data_Battery.ToString(), SetBatteryType);
        EventDispatcher.AddEvent(GEnum.EnumWifiAndBattery.Data_Wifi.ToString(), SetWifiType);
    }

    private int m_NetType = -1;
    private int m_Battery = -1;
    private int m_Wifi = -1;

    public int netType
    {
        get
        {
            return m_NetType;
        }
    }

    public int battery
    {
        get
        {
            return m_Battery;
        }
    }

    public int wifi
    {
        get
        {
            return m_Wifi;
        }
    }

    private void SetNetType(object[] values)
    {
        string value = values[0].ToString();

        int type = int.Parse(value);
        m_NetType = type;

        EventDispatcher.FireEvent(GEnum.EnumWifiAndBattery.Refresh_NetType.ToString());
    }

    private void SetWifiType(object[] values)
    {
        string value = values[0].ToString();

        int num = int.Parse(value);
        m_Wifi = num;

        EventDispatcher.FireEvent(GEnum.EnumWifiAndBattery.Refresh_Wifi.ToString());
    }

    private void SetBatteryType(object[] values)
    {
        string value = values[0].ToString();

        int num = int.Parse(value);
        m_Battery = num;

        EventDispatcher.FireEvent(GEnum.EnumWifiAndBattery.Refresh_Battery.ToString());
    }
}
