/****************************************************
*
*  时间格式化
*  作者: lyb
*  日期：2017年7月25日
*
*****************************************************/

using System;

public static class Tools_TimeFormat
{
    /// <summary>
    /// 服务器发过来的时间进行格式转化
    /// </summary>
    public static string ToTimeFormatString(this long seconds)
    {
        long timeLong = (System.DateTime.Now.Ticks - System.DateTime.UtcNow.Ticks) / 10000;
        DateTime time = new DateTime(1970, 1, 1);
        time = time.AddSeconds(seconds);
        time = time.AddMilliseconds(timeLong);
        string ret = time.ToString("yyyy/MM/dd @ HH:mm");
        return ret;
    }

    public static string ToSmartTimeFormatString(this int seconds)
    {
        string ret = "";
        if ((seconds / 86400) > 0)
        {
            ret = string.Format("{0:d}d {1:d2}:{2:d2}:{3:d2}", ((seconds) / 86400), ((seconds % 86400) / 3600), ((seconds % 3600) / 60), (seconds % 60));
        }
        else if ((((int)seconds) / 3600) > 0)
        {
            ret = string.Format("{0:d2}:{1:d2}:{2:d2}", (seconds / 3600), ((seconds % 3600) / 60), (seconds % 60));
        }
        else
        {
            ret = string.Format("{0:d2}:{1:d2}", ((seconds % 3600) / 60), (seconds % 60));
        }
        return ret;
    }

    /// <summary>
    /// 把时间格式化成xx单一的时间
    /// overTime 剩余时间
    /// </summary>
    public static string InitFormatTime(this long overTime)
    {
        long DayNum = overTime / 86400;
        if (DayNum > 0)
        {
            return string.Format("{0}天", DayNum);
        }

        long hoursNum = (overTime / 3600) % 3600;
        if (hoursNum > 0)
        {
            return string.Format("{0}小时", hoursNum);
        }

        long MinuteNum = (overTime / 60) % 60;
        if (MinuteNum > 0)
        {
            return string.Format("{0}分钟", MinuteNum);
        }

        long sNum = overTime % 60;
        if (sNum > 0)
        {
            return string.Format("{0}秒", sNum);
        }

        return string.Format("{0}秒", 0);
    }

    //时间转换年月日时分秒
    public static string ToTimeString(this long seconds)
    {
        long timeLong = (System.DateTime.Now.Ticks - System.DateTime.UtcNow.Ticks) / 10000;
        DateTime time = new DateTime(1970, 1, 1);
        time = time.AddSeconds(seconds);
        time = time.AddMilliseconds(timeLong);
        string ret = time.ToString("yyyy/MM/dd    HH:mm");
        return ret;
    }

}