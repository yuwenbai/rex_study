/****************************************************
*
*  上传Ftp数据类脚本
*  存储所有用到的数据
*  作者: lyb
*  日期：2017年7月25日
*
*****************************************************/

using System.IO;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace PlugProject
{
    public class UpLoadingData
    {
        /// <summary>
        /// 【本地】 配置文件的xml地址
        /// </summary>
        public static string BundleConfigPath = Application.dataPath + "/Resources/ClientConfig/BundleConfig.xml";
        /// <summary>
        /// 【本地】 UpLoadingMd5Xml 文件存放路径
        /// </summary>
        public static string UpLoadingMd5Path = Application.dataPath + "/Resources/ClientConfig/UpLoadingMd5.xml";
        /// <summary>
        /// 【本地】上传的xml文件的路径
        /// </summary>
        public static string UpXmlPath = Application.dataPath + "/Resources/ClientConfig/XmlZip";
        /// <summary>
        /// 【本地】上传的Texture文件的路径
        /// </summary>
        public static string UpTexturePath = Application.dataPath + "/Resources/Texture/TextureUpLoading";
        /// <summary>
        /// 【本地】需要上传的xml文件列表
        /// </summary>
        public static List<FileInfo> UpLoadingFileList = new List<FileInfo>();
        /// <summary>
        /// 【本地】需要上传的Texture文件列表
        /// </summary>
        public static List<FileInfo> UpLoadingTextureFileList = new List<FileInfo>();

        /// <summary>
        /// 【服务器】根路径
        /// </summary>
        public static string ServerSiteUrl = "";
        /// <summary>
        /// 【服务器】版本配置文件路径
        /// </summary>
        public static string ServerSettingUrl = "";
        /// <summary>
        /// 【服务器】上的版本号
        /// </summary>
        public static string Version_Server = "";
        /// <summary>
        /// 【服务器】上的Xml文件链接地址列表以及图片的链接地址
        /// </summary>
        public static ArrayList XmlList_Server = new ArrayList();
        /// <summary>
        /// 【服务器】上的Xml文件Md5值存储
        /// </summary>
        public static ArrayList XmlMd5List_Server = new ArrayList();


        /// <summary>
        /// 【上传Ftp工具】资源存放,本地路径
        /// </summary>
        public static string UpLoadingPath = Application.persistentDataPath + "/UpLoading";
        /// <summary>
        /// 【上传Ftp工具】从服务器上下载下来的版本配置存放本地路径
        /// </summary>
        public static string UpLoadingFilePath = Application.persistentDataPath + "/UpLoading/version.xml";
        /// <summary>
        /// 【上传Ftp工具】从服务器上下载下来的Md5值配置存放本地路径
        /// </summary>
        public static string UpLoadingMd5FilePath = Application.persistentDataPath + "/UpLoading/UpLoadingMd5.xml";
        /// <summary>
        /// 【上传Ftp工具】从服务器上下载下来的音乐配置存放本地路径
        /// </summary>
        public static string UpLoadingMusicFilePath = Application.persistentDataPath + "/UpLoading/MusicXml.xml";

        /// <summary>
        /// 完成功能后清理数据
        /// </summary>
        public static void UpLoading_Clear()
        {
            UpLoadingFileList = new List<FileInfo>();
            UpLoadingTextureFileList = new List<FileInfo>();
            XmlList_Server = new ArrayList();
            XmlMd5List_Server = new ArrayList();
        }
    }
}