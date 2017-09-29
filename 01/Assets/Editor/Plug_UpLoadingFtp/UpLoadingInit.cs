/****************************************************
*
*  上传配置文件初始化
*  读取本地xml获取资源服务器地址
*  把资源服务器上的配置文件下载下来
*  作者: lyb
*  日期：2017年7月25日
*
*****************************************************/

using UnityEngine;
using System.IO;

namespace PlugProject
{
    public class UpLoadingInit : BaseUpLoading
    {
        #region 初始化Xml文件获取资源服务器配置文件地址 -----------

        public static void Init(string typeStr = "0")
        {
            UpLoadingData.UpLoading_Clear();

            UpLoading_XmlConfigInit();

            foreach (BundleConfigData cData in ConfigData)
            {
                if (cData.BundleType.Equals(typeStr))
                {
                    UpLoadingData.ServerSiteUrl = cData.ServerSite;
                    UpLoadingData.ServerSettingUrl = cData.ServerSettingUrl;
                }
            }

            if (!Directory.Exists(UpLoadingData.UpLoadingPath))
            {
                Directory.CreateDirectory(UpLoadingData.UpLoadingPath);
            }

            //下载版本配置文件
            string serverVersionPath = UpLoadingPath.UpLoading_PathGet(PathServerEnum.Path_VersionXml);
            using (System.Net.WebClient wc = new System.Net.WebClient())
            {
                wc.DownloadFile(serverVersionPath, UpLoadingData.UpLoadingFilePath);
            }

            //下载Md5值配置文件
            string serverMd5Path = UpLoadingPath.UpLoading_PathGet(PathServerEnum.Path_Md5Xml);
            using (System.Net.WebClient wc = new System.Net.WebClient())
            {
                wc.DownloadFile(serverMd5Path, UpLoadingData.UpLoadingMd5FilePath);
            }

            //下载音乐配置文件
            string serverMusicPath = UpLoadingPath.UpLoading_PathGet(PathServerEnum.Path_MusicXml);
            using (System.Net.WebClient wc = new System.Net.WebClient())
            {
                wc.DownloadFile(serverMusicPath, UpLoadingData.UpLoadingMusicFilePath);
            }

            UpLoadingFind.UpLoading_DataInit(DataTypeEnum.Data_Version);

            Debug.Log(" #[上传Ftp工具]# UpLoadingInit - 服务器配置文件下载完毕");
        }

        #endregion ------------------------------------------------    
    }
}