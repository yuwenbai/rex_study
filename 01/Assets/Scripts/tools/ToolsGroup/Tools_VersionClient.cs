/****************************************************
*
*  获取更改客户端存储的版本号
*  作者: lyb
*  日期：2017年7月25日
*
*****************************************************/

using System.Xml;
using UnityEngine;

public class Tools_VersionClient
{
    public static string VersionClientPath = "ClientConfig/Version_Client";

    #region 配置文件版本号获取-------------------------------------

    public static string Versionclient_Find()
    {
        string version = "";

        XmlDocument xmlDoc = new XmlDocument();
        string xmldata = Resources.Load(VersionClientPath).ToString();
        xmlDoc.LoadXml(xmldata);

        XmlNode node = xmlDoc.SelectSingleNode("objects");

        XmlElement ele = (XmlElement)node;

        if (ele != null && ele.Attributes["version"] != null)
        {
            version = ele.Attributes["version"].Value;
            version = string.IsNullOrEmpty(version) ? string.Empty : version.Trim();
        }

        return version;
    }

    #endregion ----------------------------------------------------
}
