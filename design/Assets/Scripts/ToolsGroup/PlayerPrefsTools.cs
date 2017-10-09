/****************************************************
*
*  往本地存储数据
*  作者: lyb
*  日期：2017年7月25日
*
*****************************************************/

using System.IO;
using UnityEngine;
using System.Xml.Serialization;

public class PlayerPrefsTools
{
    #region set

    public static void SetInt(string key, int value, bool isSave = false)
    {
        PlayerPrefs.SetInt(key.ToString(), value);
        if (isSave) Save();
    }

    public static void SetFloat(string key, float value, bool isSave = false)
    {
        PlayerPrefs.SetFloat(key.ToString(), value);
        if (isSave) Save();
    }

    public static void SetString(string key, string value, bool isSave = false)
    {
        PlayerPrefs.SetString(key.ToString(), value);
        if (isSave) Save();
    }

    public static void SetObject<T>(string key, T obj, bool isSave = false)
    {
        XmlSerializer serializer = new XmlSerializer(typeof(T));
        StringWriter sw = new StringWriter();
        serializer.Serialize(sw, obj);
        PlayerPrefs.SetString(key.ToString(), sw.ToString());
        if (isSave) Save();
    }

    #endregion

    #region get

    public static int GetInt(string key)
    {
        return PlayerPrefs.GetInt(key.ToString(), 0);
    }

    public static float GetFloat(string key)
    {
        return PlayerPrefs.GetFloat(key.ToString(), 0.0f);
    }

    public static string GetString(string key)
    {
        return PlayerPrefs.GetString(key.ToString(), string.Empty);
    }

    public static T GetObject<T>(string key)
    {
        if (PlayerPrefs.HasKey(key.ToString()))
        {
            XmlSerializer serializer = new XmlSerializer(typeof(T));
            StringReader sr = new StringReader(PlayerPrefs.GetString(key.ToString()));
            return (T)serializer.Deserialize(sr);
        }
        else
        {
            return default(T);
        }
    }

    #endregion

    #region del

    public static void DeleteKey(string key)
    {
        PlayerPrefs.DeleteKey(key.ToString());
    }

    public static void DeleteAll()
    {
        PlayerPrefs.DeleteAll();
    }

    #endregion

    public static void Save()
    {
        PlayerPrefs.Save();
    }
}
