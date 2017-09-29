/****************************************************
*
*  游戏全局变量定义
*  作者: lyb
*  日期：2017年7月25日
*
*****************************************************/

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace projectQ
{
    public class GameConfig : SingletonTamplate<GameConfig>
    {
        #region lyb 通用----------------------------------------------------------

        public string merchant_id = "c697c188d13bcea8418a9f6a";

        /// <summary>
        /// 特效当前是否显示状态记录
        /// </summary>
        public bool IsEffectOpen = true;

        #endregion ---------------------------------------------------------------

        #region lyb 进入游戏前的资源加载相关--------------------------------------

        /// <summary>
        /// 当前是否要走资源加载
        /// 0，本地资源加载 1，服务器资源加载
        /// </summary>
        public string IsDown = "";
        /// <summary>
        /// 从本地xml文件中读取到的路径信息
        /// </summary>
        public string Xml_ServerUrl = "";
        /// <summary>
        /// 服务器资源地址
        /// </summary>
        public string Resource_ServerPath = "";
        /// <summary>
        /// 服务器上的版本号
        /// </summary>
        public string Version_Server = "";
        /// <summary>
        /// 客户端上的版本号
        /// </summary>
        public string Version_Client = "";
        /// <summary>
        /// App下载地址
        /// </summary>
        public string AppDownUrl = "";
        /// <summary>
        /// Apk下载地址
        /// </summary>
        public string ApkDownUrl = "";
        /// <summary>
        /// Apk - Md5值
        /// </summary>
        public string ApkMd5 = "";
        /// <summary>
        /// 服务器上音乐资源配置文件地址
        /// </summary>
        public string MusicXml_ServerUrl = "";

        /// <summary>
        /// Apk保存完整路径
        /// </summary>
        //public string Apk_ClientPath = PathHelper.PersistentPath + "/369mj_alpha_1.0.0.apk";

        /// <summary>
        /// Apk保存路径
        /// </summary>
        public string ApkLocal_Path = PathHelper.PersistentPath + "/DownApkAssets/";

        /// <summary>
        /// Apk包名
        /// </summary>
        public string ApkLocal_Name = "";

        /// <summary>
        /// 本地Xml版本文件地址
        /// </summary>
        public string Xml_ClientPath = PathHelper.PersistentPath + "/version.xml";
        /// <summary>
        /// 客户端图片资源地址
        /// </summary>
        public string ResourceLocal_Path = PathHelper.PersistentPath + "/DownTextureAssets";       
        /// <summary>
        /// 服务器上的图片链接地址列表
        /// </summary>
        public ArrayList TextureList_Server = new ArrayList();
        /// <summary>
        /// 客户端上的图片链接地址列表
        /// </summary>
        public ArrayList TextureList_Client = new ArrayList();
        /// <summary>
        /// 更新处理完成的资源列表
        /// </summary>
        public Dictionary<string, Texture2D> DownResourceDic = new Dictionary<string, Texture2D>();


        /// <summary>
        /// 客户端Xml资源地址
        /// </summary>
        public string XmlLocal_Path = PathHelper.PersistentPath + "/DownXmlAssets";
        /// <summary>
        /// 服务器上的Xml文件链接地址列表
        /// </summary>
        public ArrayList XmlList_Server = new ArrayList();
        /// <summary>
        /// 客户端上的Xml文件链接地址列表
        /// </summary>
        public ArrayList XmlList_Client = new ArrayList();
        /// <summary>
        /// 客户端解压Xml资源地址
        /// </summary>
        public string XmlUnZip_Path = PathHelper.PersistentPath + "/XmlUnZipAssets";


        /// <summary>
        /// 本地音乐Xml版本文件地址
        /// </summary>
        public string MusicXml_ClientPath = PathHelper.PersistentPath + "/MusicXml.xml";
        /// <summary>
        /// 客户端背景音乐资源地址
        /// </summary>
        public string MusicBgLocal_Path = PathHelper.PersistentPath + "/DownMusicAssets/Music_Back";
        /// <summary>
        /// 服务器上的背景音乐链接地址列表
        /// </summary>
        public ArrayList MusicBgList_Server = new ArrayList();
        /// <summary>
        /// 客户端上的背景音乐链接地址列表
        /// </summary>
        public ArrayList MusicBgList_Client = new ArrayList();


        /// <summary>
        /// 客户端音效资源地址
        /// </summary>
        public string MusicSoundLocal_Path = PathHelper.PersistentPath + "/DownMusicAssets/Music_Sound";
        /// <summary>
        /// 服务器上的音效链接地址列表
        /// </summary>
        public ArrayList MusicSoundList_Server = new ArrayList();
        /// <summary>
        /// 客户端上的音效链接地址列表
        /// </summary>
        public ArrayList MusicSoundList_Client = new ArrayList();


        /// <summary>
        /// 客户端普通话语音资源地址
        /// </summary>
        public string MusicMandarinLocal_Path = PathHelper.PersistentPath + "/DownMusicAssets/Music_Voice/MusicVoice_Mandarin";
        /// <summary>
        /// 服务器上的音效链接地址列表
        /// </summary>
        public ArrayList MusicMandarinList_Server = new ArrayList();
        /// <summary>
        /// 客户端上的音效链接地址列表
        /// </summary>
        public ArrayList MusicMandarinList_Client = new ArrayList();


        /// <summary>
        /// 客户端河南话语音资源地址
        /// </summary>
        public string MusicHeNanLocal_Path = PathHelper.PersistentPath + "/DownMusicAssets/Music_Voice/MusicVoice_HeNan";
        /// <summary>
        /// 服务器上的音效链接地址列表
        /// </summary>
        public ArrayList MusicHeNanList_Server = new ArrayList();
        /// <summary>
        /// 客户端上的音效链接地址列表
        /// </summary>
        public ArrayList MusicHeNanList_Client = new ArrayList();

        /// <summary>
        /// 音乐缓存列表
        /// </summary>
        public Dictionary<string, AudioClip> DownMusicDic = new Dictionary<string, AudioClip>();

        /// <summary>
        /// 资源加载的相关数据清理
        /// </summary>
        public void BundleDataClear()
        {
            IsDown = "";
            Xml_ServerUrl = "";
            Resource_ServerPath = "";
            Version_Server = "";
            Version_Client = "";
            TextureList_Server = new ArrayList();
            TextureList_Client = new ArrayList();
            DownResourceDic = new Dictionary<string, Texture2D>();
            XmlList_Server = new ArrayList();
            XmlList_Client = new ArrayList();

            MusicBgList_Server = new ArrayList();
            MusicBgList_Client = new ArrayList();
            MusicSoundList_Server = new ArrayList();
            MusicSoundList_Client = new ArrayList();
            MusicMandarinList_Server = new ArrayList();
            MusicMandarinList_Client = new ArrayList();
            MusicHeNanList_Server = new ArrayList();
            MusicHeNanList_Client = new ArrayList();
        }

        #endregion ---------------------------------------------------------------

        #region lyb 微信头像相关数据存储------------------------------------------

        /// <summary>
        /// 微信头像下载,本地保存路径
        /// </summary>
        public string WeChat_HeadTexturePath = PathHelper.PersistentPath + "/WeChatHead";
        /// <summary>
        /// 微信头像下载保存数据的xml文件
        /// </summary>
        public string WeChat_HeadTextureXmlPath = PathHelper.PersistentPath + "/WeChatHead/WeChatHeadTexture.xml";

        #endregion ---------------------------------------------------------------        
    }
}
