/****************************************************
*
*  上传Ftp文件基类
*  作者: lyb
*  日期：2017年7月25日
*
*****************************************************/

using System.IO;
using System.Xml;
using UnityEditor;
using System.Collections.Generic;

namespace PlugProject
{
    public class BundleConfigData
    {
        /// <summary>
        /// 资源服务器
        /// 【0】外网 , 【1】内网
        /// </summary>
        public string BundleType;
        /// <summary>
        /// 【0】本地资源加载 , 【1】服务器资源加载
        /// </summary>
        public string IsDown;
        /// <summary>
        /// 资源服务器地址
        /// </summary>
        public string ServerSite;
        /// <summary>
        /// 资源加载配置文件路径
        /// </summary>
        public string ServerSettingUrl;
    }

    /// <summary>
    /// 服务器版本xml文件的id，以此来区分要获取的是哪块的数据
    /// </summary>
    public enum XmlMessageTypeEnum
    {
        downUrl,
        musicUrl,
        textureUrl,
        xmlUrl,
    }

    /// <summary>
    /// 当前解析的是版本号xml还是md5码xml
    /// </summary>
    public enum DataTypeEnum
    {
        Data_Version,
        Data_Md5,
    }

    /// <summary>
    /// 通过枚举获取服务器上的一个路径
    /// </summary>
    public enum PathServerEnum
    {
        /// <summary>
        /// 版本记录xml服务器路径
        /// </summary>
        Path_VersionXml,
        /// <summary>
        /// Md5值xml服务器路径
        /// </summary>
        Path_Md5Xml,
        /// <summary>
        /// 音乐记录xml服务器路径
        /// </summary>
        Path_MusicXml,
        /// <summary>
        /// xml文件服务器路径
        /// </summary>
        Path_AssetsXml,
        /// <summary>
        /// Texture文件服务器路径
        /// </summary>
        Path_AssetsTexture,
        /// <summary>
        /// 背景音乐文件服务器路径
        /// </summary>
        Path_AssetsMusicBg,
        /// <summary>
        /// 音效文件服务器路径
        /// </summary>
        Path_AssetsMusicSound,
        /// <summary>
        /// 普通话语音文件服务器路径
        /// </summary>
        Path_AssetsMusicPuTong,
        /// <summary>
        /// 河南话语音文件服务器路径
        /// </summary>
        Path_AssetsMusicHeNan,
    }

    public class BaseUpLoading
    {
        private static List<BundleConfigData> _ConfigData = new List<BundleConfigData>();
        public static List<BundleConfigData> ConfigData
        {
            get { return _ConfigData; }
        }

        #region 解析本地xml文件获取资源服务器地址 ------------------------

        /// <summary>
        /// 获取资源配置文件的url地址
        /// </summary>
        public static void UpLoading_XmlConfigInit()
        {
            XmlDocument xmlDoc = new XmlDocument();
            XmlReaderSettings set = new XmlReaderSettings();
            set.IgnoreComments = true;
            XmlReader reader = XmlReader.Create(UpLoadingData.BundleConfigPath, set);
            xmlDoc.Load(reader);
            reader.Close();

            XmlNodeList xmlNodeList = xmlDoc.SelectSingleNode("objects").ChildNodes;

            foreach (XmlNode xnode in xmlNodeList)
            {
                BundleConfigData configData = new BundleConfigData();

                for (int i = 0; i < xnode.ChildNodes.Count; i++)
                {
                    switch (xnode.ChildNodes[i].Name)
                    {
                        default: break;
                        case "BundleType":
                            {
                                configData.BundleType = xnode.ChildNodes[i].InnerText;
                            }
                            break;
                        case "IsDown":
                            {
                                configData.IsDown = xnode.ChildNodes[i].InnerText;
                            }
                            break;
                        case "ServerSite":
                            {
                                configData.ServerSite = xnode.ChildNodes[i].InnerText;
                            }
                            break;
                        case "ServerSettingUrl":
                            {
                                configData.ServerSettingUrl = xnode.ChildNodes[i].InnerText;
                            }
                            break;
                    }
                }

                ConfigData.Add(configData);
            }
        }

        #endregion ------------------------------------------------

        #region lyb 获取xml中的数据--------------------------------

        /// <summary>
        /// 读取Xml数据
        /// </summary>
        public static void UpLoading_XmlLoad(string xmlStr = "")
        {
            XmlDocument xmlDoc = new XmlDocument();

            xmlDoc.LoadXml(xmlStr);

            XmlNode node = xmlDoc.SelectSingleNode("objects");

            XmlElement ele = (XmlElement)node;

            if (ele != null && ele.Attributes["version"] != null)
            {
                string version = ele.Attributes["version"].Value;
                version = string.IsNullOrEmpty(version) ? string.Empty : version.Trim();

                UpLoadingData.Version_Server = version;
            }

            XmlNodeList xmlNodeList = xmlDoc.SelectSingleNode("objects").ChildNodes;

            foreach (XmlElement element in xmlNodeList)
            {
                if (element.GetAttribute("id").Equals("xmlUrl") || element.GetAttribute("id").Equals("textureUrl"))
                {
                    foreach (XmlElement nodes in element.ChildNodes)
                    {
                        string xmlUrl = nodes.GetAttribute("name") + "@" + nodes.InnerText;

                        UpLoadingData.XmlList_Server.Add(xmlUrl);
                    }
                }

                if (element.GetAttribute("id").Equals("UpXml") || element.GetAttribute("id").Equals("UpTexture"))
                {
                    foreach (XmlElement nodes in element.ChildNodes)
                    {
                        string xmlMd5 = nodes.GetAttribute("name") + "@" + nodes.InnerText;

                        UpLoadingData.XmlMd5List_Server.Add(xmlMd5);
                    }
                }
            }
        }

        #endregion ------------------------------------------------

        #region 获取messageid名字 ---------------------------------

        /// <summary>
        /// 根据传过来的枚举获得xml文件中的messageid名字
        /// </summary>
        public static string UpLoad_MessageStr(XmlMessageTypeEnum messageEnum)
        {
            string mName = "";

            switch (messageEnum)
            {
                case XmlMessageTypeEnum.downUrl:
                    mName = "downUrl";
                    break;
                case XmlMessageTypeEnum.musicUrl:
                    mName = "musicUrl";
                    break;
                case XmlMessageTypeEnum.textureUrl:
                    mName = "textureUrl";
                    break;
                case XmlMessageTypeEnum.xmlUrl:
                    mName = "xmlUrl";
                    break;
            }

            return mName;
        }

        #endregion ------------------------------------------------

        #region 检测当前初始化是否成功 ----------------------------

        public static bool IsInitSucc()
        {
            if (File.Exists(UpLoadingData.UpLoadingFilePath))
            {
                return true;
            }

            return false;
        }

        #endregion ------------------------------------------------

        #region 获取指定目录下的所有的文件 ------------------------

        public static List<FileInfo> UpLoadingFileAllGet(string uPath)
        {
            List<FileInfo> upFileList = new List<FileInfo>();

            DirectoryInfo dirInfo = new DirectoryInfo(uPath);

            FileInfo[] fileList = dirInfo.GetFiles();

            for (int i = 0; i < fileList.Length; i++)
            {
                if (fileList[i].Extension != ".meta")
                {
                    upFileList.Add(fileList[i]);
                }
            }

            return upFileList;
        }

        #endregion ------------------------------------------------

        #region 出错的话弹框提示 ----------------------------------

        public static void UpLoading_ErrorBox(string eStr)
        {
            EditorUtility.DisplayDialog("错误", eStr, "确定");
        }

        #endregion ------------------------------------------------
    }
}