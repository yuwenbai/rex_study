using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DaYingJiaItem : MonoBehaviour
{
    public UILabel m_Label_Name;
    public UILabel m_Label_Time;
    public UILabel m_Label_Numb;

    public void InitItem(string name, int time, int costNum)
    {
        m_Label_Name.text = name;

        System.DateTime startTime = System.TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1));
        if (time > 0 && m_Label_Time != null)
        {
            System.DateTime dt = startTime.AddSeconds(time);
            m_Label_Time.text = string.Format("{0}", dt.ToString("H:mm:ss"));//入桌 
        }

        if (costNum > 0)
            m_Label_Numb.text = string.Format("桌卡 -{0}", costNum);
        else
            m_Label_Numb.text = "";
    }
}
