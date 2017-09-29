/****************************************************
*
*  资源加载基类
*  作者: lyb
*  日期：2017年7月25日
*
*****************************************************/

using System.IO;
using System.Xml;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace projectQ
{
    public enum XmlLoadEnum
    {
        /// <summary>
        /// 服务器上的xml
        /// </summary>
        XmlLoad_Server,
        /// <summary>
        /// 客户端本地的xml
        /// </summary>
        XmlLoad_Client,
    }

    public enum MusicDownTypeEnum
    {
        /// <summary>
        /// 初始化第一次下载
        /// </summary>
        MusicDown_Null,
        /// <summary>
        /// 背景音乐下载
        /// </summary>
        MusicDown_Bg,
        /// <summary>
        /// 音效下载
        /// </summary>
        MusicDown_Sound,
        /// <summary>
        /// 普通话下载
        /// </summary>
        MusicDown_VoiceMandarin,
        /// <summary>
        /// 河南方言下载
        /// </summary>
        MusicDown_VoiceHeNan,
    }

    public abstract class BaseResourceLoad : MonoBehaviour
    {
        #region lyb 获取xml中的数据--------------------------------------

        /// <summary>
        /// 读取Xml数据
        /// </summary>
        public void Xml_Load(XmlLoadEnum xmlLoad, string xmlStr = "")
        {
            XmlDocument xmlDoc = new XmlDocument();

            if (xmlLoad == XmlLoadEnum.XmlLoad_Server)
            {
                DebugPro.LogBundle(" #[资源加载]# - 解析服务器上的Xml ");

                xmlDoc.LoadXml(xmlStr);
            }
            else
            {
                DebugPro.LogBundle(" #[资源加载]# - 解析客户端上的Xml ");

                XmlReaderSettings set = new XmlReaderSettings();
                set.IgnoreComments = true;
                XmlReader reader = XmlReader.Create(GameConfig.Instance.Xml_ClientPath, set);
                try
                {
                    //本地XML如果是坏的直接报错停掉所以 try catch捕捉 并退出
                    xmlDoc.Load(reader);
                }
                catch (System.Exception ex)
                {
                    QLoger.ERROR(ex.ToString());
                    return;
                }
                finally
                {
                    if (reader != null)
                    {
                        reader.Close();
                    }
                }
            }

            XmlNode node = xmlDoc.SelectSingleNode("objects");

            XmlElement ele = (XmlElement)node;

            if (ele != null && ele.Attributes["version"] != null)
            {
                string version = ele.Attributes["version"].Value;
                version = string.IsNullOrEmpty(version) ? string.Empty : version.Trim();


                if (xmlLoad == XmlLoadEnum.XmlLoad_Server)
                {
                    GameConfig.Instance.Version_Server = version;
                }
                else
                {
                    GameConfig.Instance.Version_Client = version;
                }
            }

            XmlNodeList xmlNodeList = xmlDoc.SelectSingleNode("objects").ChildNodes;

            foreach (XmlElement element in xmlNodeList)
            {
                if (element.GetAttribute("id").Equals("textureUrl"))
                {
                    foreach (XmlElement nodes in element.ChildNodes)
                    {
                        string texUrl = nodes.GetAttribute("name") + "@" + nodes.InnerText;
                        if (xmlLoad == XmlLoadEnum.XmlLoad_Server)
                        {
                            GameConfig.Instance.TextureList_Server.Add(texUrl);
                        }
                        else
                        {
                            GameConfig.Instance.TextureList_Client.Add(texUrl);
                        }
                    }
                }

                if (element.GetAttribute("id").Equals("xmlUrl"))
                {
                    foreach (XmlElement nodes in element.ChildNodes)
                    {
                        string xmlUrl = nodes.GetAttribute("name") + "@" + nodes.InnerText;
                        if (xmlLoad == XmlLoadEnum.XmlLoad_Server)
                        {
                            GameConfig.Instance.XmlList_Server.Add(xmlUrl);
                        }
                        else
                        {
                            GameConfig.Instance.XmlList_Client.Add(xmlUrl);
                        }
                    }
                }

                if (element.GetAttribute("id").Equals("downUrl"))
                {
                    foreach (XmlElement nodes in element.ChildNodes)
                    {
                        if (nodes.GetAttribute("name").Equals("AppDownUrl"))
                        {
                            GameConfig.Instance.AppDownUrl = nodes.InnerText;
                        }

                        if (nodes.GetAttribute("name").Equals("ApkDownUrl"))
                        {
                            GameConfig.Instance.ApkDownUrl = nodes.InnerText;
                        }

                        if (nodes.GetAttribute("name").Equals("ApkMd5"))
                        {
                            GameConfig.Instance.ApkMd5 = nodes.InnerText;
                        }
                    }
                }

                if (element.GetAttribute("id").Equals("musicUrl"))
                {
                    foreach (XmlElement nodes in element.ChildNodes)
                    {
                        if (nodes.GetAttribute("name").Equals("MusicXml"))
                        {
                            GameConfig.Instance.MusicXml_ServerUrl = GameConfig.Instance.Resource_ServerPath + nodes.InnerText;
                        }
                    }
                }
            }
        }

        #endregion ------------------------------------------------------

        #region lyb 获取音乐xml中的数据----------------------------------

        /// <summary>
        /// 读取音乐相关Xml数据
        /// </summary>
        public void Xml_MusicLoad(XmlLoadEnum xmlLoad, string xmlStr = "")
        {
            XmlDocument xmlDoc = new XmlDocument();

            if (xmlLoad == XmlLoadEnum.XmlLoad_Server)
            {
                DebugPro.LogBundle(" #[资源加载-音乐]# - 解析服务器上的音乐Xml ");

                xmlDoc.LoadXml(xmlStr);
            }
            else
            {
                DebugPro.LogBundle(" #[资源加载-音乐]# - 解析客户端上的音乐Xml ");

                XmlReaderSettings set = new XmlReaderSettings();
                set.IgnoreComments = true;
                XmlReader reader = XmlReader.Create(GameConfig.Instance.MusicXml_ClientPath, set);
                try
                {
                    //本地XML如果是坏的直接报错停掉所以 try catch捕捉 并退出
                    xmlDoc.Load(reader);
                }
                catch (System.Exception ex)
                {
                    QLoger.ERROR(ex.ToString());
                    return;
                }
                finally
                {
                    if (reader != null)
                    {
                        reader.Close();
                    }
                }
            }

            XmlNodeList xmlNodeList = xmlDoc.SelectSingleNode("objects").ChildNodes;

            foreach (XmlElement element in xmlNodeList)
            {
                if (element.GetAttribute("id").Equals("MusicBack"))
                {
                    foreach (XmlElement nodes in element.ChildNodes)
                    {
                        string urlStr = nodes.GetAttribute("name") + "@" + nodes.InnerText;

                        if (xmlLoad == XmlLoadEnum.XmlLoad_Server)
                        {
                            GameConfig.Instance.MusicBgList_Server.Add(urlStr);
                        }
                        else
                        {
                            GameConfig.Instance.MusicBgList_Client.Add(urlStr);
                        }
                    }
                }

                if (element.GetAttribute("id").Equals("MusicSound"))
                {
                    foreach (XmlElement nodes in element.ChildNodes)
                    {
                        string urlStr = nodes.GetAttribute("name") + "@" + nodes.InnerText;

                        if (xmlLoad == XmlLoadEnum.XmlLoad_Server)
                        {
                            GameConfig.Instance.MusicSoundList_Server.Add(urlStr);
                        }
                        else
                        {
                            GameConfig.Instance.MusicSoundList_Client.Add(urlStr);
                        }
                    }
                }

                if (element.GetAttribute("id").Equals("MusicMandarin"))
                {
                    foreach (XmlElement nodes in element.ChildNodes)
                    {
                        string urlStr = nodes.GetAttribute("name") + "@" + nodes.InnerText;

                        if (xmlLoad == XmlLoadEnum.XmlLoad_Server)
                        {
                            GameConfig.Instance.MusicMandarinList_Server.Add(urlStr);
                        }
                        else
                        {
                            GameConfig.Instance.MusicMandarinList_Client.Add(urlStr);
                        }
                    }
                }

                if (element.GetAttribute("id").Equals("MusicHeNan"))
                {
                    foreach (XmlElement nodes in element.ChildNodes)
                    {
                        string urlStr = nodes.GetAttribute("name") + "@" + nodes.InnerText;

                        if (xmlLoad == XmlLoadEnum.XmlLoad_Server)
                        {
                            GameConfig.Instance.MusicHeNanList_Server.Add(urlStr);
                        }
                        else
                        {
                            GameConfig.Instance.MusicHeNanList_Client.Add(urlStr);
                        }
                    }
                }
            }
        }

        #endregion ------------------------------------------------------

        #region lyb 检测本地数据表和本地资源的合法性---------------------

        /// <summary>
        /// 检测本地图片资源是否合法
        /// checkList 检测的列表
        /// checkPath 检测的地址
        /// </summary>
        public bool ResourceCorrect_Check(ArrayList checkList, string checkPath, string fileType)
        {
            //是否需要重新下载资源
            bool isDown = false;

            if (checkList.Count <= 0)
            {
                isDown = true;
            }

            foreach (string urlStr in checkList)
            {
                string[] values = urlStr.Split(new char[] { '@' });

                string path = checkPath + "/" + values[0] + fileType;

                if (!File.Exists(path))
                {
                    isDown = true;

                    break;
                }
            }

            return isDown;
        }

        /// <summary>
        /// 检查版本号
        /// 前两位大版本更新验证
        /// </summary>
        public bool CheckVersion()
        {
            List<int> serverVersion = GetVersionArr(GameConfig.Instance.Version_Server);
            List<int> localVersion = GetVersionArr(Application.version);

            if (localVersion == null || serverVersion == null || localVersion.Count < 2 || serverVersion.Count < 2)
            {
                return false;
            }

            if (serverVersion[0] > localVersion[0])
            {
                return false;
            }
            else if (serverVersion[0] == localVersion[0])
            {
                if (serverVersion[1] > localVersion[1])
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// 检查第三位版本号
        /// 游戏内自更新验证 1.2.0.1279
        /// </summary>
        public bool CheckVersionSelfUpdate()
        {
            List<int> serverVersion = GetVersionArr(GameConfig.Instance.Version_Server);
            List<int> localVersion = GetVersionArr(Application.version);

            if (localVersion == null || serverVersion == null || localVersion.Count < 2 || serverVersion.Count < 2)
            {
                return false;
            }

            if (localVersion[2] == 0)
            {
                if (serverVersion[3] != localVersion[3])
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
            else if (serverVersion[2] != localVersion[2])
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// 字符串转换为数组
        /// </summary>
        /// <param name="versionStr"></param>
        /// <returns></returns>
        private List<int> GetVersionArr(string versionStr)
        {
            List<int> result = new List<int>();
            if (!string.IsNullOrEmpty(versionStr))
            {
                versionStr = versionStr.Trim();
                string[] temp = versionStr.Split('.');
                if (temp.Length > 0)
                {
                    for (int i = 0; i < temp.Length; i++)
                    {
                        int num = 0;
                        if (int.TryParse(temp[i], out num))
                        {
                            result.Add(num);
                        }
                        else
                        {
                            result.Add(0);
                        }
                    }
                }
            }
            return result;
        }

        #endregion ------------------------------------------------------

        #region lyb 监测当前的资源是否需要更新---------------------------

        /// <summary>
        /// 监测当前的资源是否需要更新
        /// </summary>
        public string ResourceUpdate_Check(ArrayList resourceList, string rName, string rDoc)
        {
            foreach (string urlStr in resourceList)
            {
                string[] values = urlStr.Split(new char[] { '@' });

                string name = values[0];
                string doc = values[1];

                if (name.Equals(rName))
                {
                    string[] docValues = rDoc.Split(new char[] { '?' });
                    if (int.Parse(docValues[1]) < 0)
                    {
                        DebugPro.LogBundle(" #[资源加载]# - 配置为强制更新，资源名字是 = " + name);

                        return name;
                    }

                    //名字相同比较资源地址
                    if (doc.Equals(rDoc))
                    {
                        return "";
                    }
                    else
                    {
                        DebugPro.LogBundle(" #[资源加载]# - 资源有更新，更新资源名字是 = " + name);

                        return name;
                    }
                }
            }

            return "New";
        }

        #endregion ------------------------------------------------------

        #region lyb 资源下载出错，弹框提示-------------------------------

        public void BundleErrorPrompt(string error)
        {
            WindowUIManager.Instance.CreateOrAddWindow(WindowUIType.SystemPopupWindow, "资源更新",
                error, new string[] { "确定" }, delegate (int index)
                 {
                     if (index == 0)
                     {
                         EventDispatcher.FireEvent(EventKey.Bundle_Restart_Event);
                     }
                 });
        }

        #endregion ------------------------------------------------------
    }
}