/****************************************************
*
*  查找服务器上的xml的数据
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
    public class UpLoadingFind : BaseUpLoading
    {
        #region 初始化调用 --------------------------------------------

        public static void UpLoading_DataInit(DataTypeEnum dType)
        {
            UpLoadingData.UpLoadingFileList = UpLoadingFileAllGet(UpLoadingData.UpXmlPath);
            UpLoadingData.UpLoadingTextureFileList = UpLoadingFileAllGet(UpLoadingData.UpTexturePath);
            
            if (dType == DataTypeEnum.Data_Version)
            {
                Tool_EditorCoroutineRunner.StartEditorCoroutine(UpLoading_VersionInit());
            }
            else
            {
                Tool_EditorCoroutineRunner.StartEditorCoroutine(UpLoading_Md5Init());
            }
        }

        /// <summary>
        /// 初始化服务器配置文件的版本号
        /// </summary>
        private static IEnumerator UpLoading_VersionInit()
        {
            WWW www = new WWW(UpLoadingSettingPath(UpLoadingData.UpLoadingFilePath));
            yield return www;

            if (www.isDone && www.error == null)
            {
                UpLoading_XmlLoad(www.text);

                UpLoading_DataInit(DataTypeEnum.Data_Md5);
            }
            else
            {
                UpLoading_ErrorBox(" #[UpLoadingFind]# www加载版本xml出错 www.error = " + www.error);
            }
        }

        /// <summary>
        /// 初始化服务器配置文件的Md5值
        /// </summary>
        private static IEnumerator UpLoading_Md5Init()
        {
            WWW www = new WWW(UpLoadingSettingPath(UpLoadingData.UpLoadingMd5FilePath));
            yield return www;

            if (www.isDone && www.error == null)
            {
                UpLoading_XmlLoad(www.text);

                Debug.Log(" #[上传Ftp工具]# UpLoadingFind - 初始化完毕");

                BuildUpdateEditor.CheckUpdateControl.GetInstance.IsInitFinish(true);
            }
            else
            {
                UpLoading_ErrorBox(" #[UpLoadingFind]# www加载Md5值xml出错 www.error = " + www.error);
            }
        }

        private static string UpLoadingSettingPath(string fPath)
        {
            string url = "file://" + fPath;

#if UNITY_EDITOR || UNITY_STANDALONE_WIN
            url = "file:///" + fPath;
#endif
            return url;
        }

        #endregion ----------------------------------------------------

        #region 查询服务器配置文件的版本号 ----------------------------

        /// <summary>
        /// 查询服务器配置文件的版本号
        /// xName 查询文件的名字
        /// </summary>
        public static string UpLoading_VersionGet(string xName)
        {
            string versionValue = "";

            if (UpLoadingData.XmlList_Server.Count <= 0)
            {
                UpLoading_ErrorBox(" #[UpLoadingFind]# 未获得Xml数据,请先初始化 ");
                return "";
            }

            foreach (string sValue in UpLoadingData.XmlList_Server)
            {
                string[] values = sValue.Split(new char[] { '@' });
                string[] verValue = values[1].Split(new char[] { '?' });

                if (values[0].Equals(xName))
                {
                    versionValue = verValue[1];
                    break;
                }
            }

            return versionValue;
        }

        #endregion ----------------------------------------------------

        #region 查询服务器配置文件的Md5值 -----------------------------

        /// <summary>
        /// 查询服务器配置文件的Md5值
        /// </summary>
        public static string UpLoading_Md5Get(string xName)
        {
            string md5Str = "";

            if (UpLoadingData.XmlMd5List_Server.Count <= 0)
            {
                UpLoading_ErrorBox(" #[UpLoadingFind]# 未获得Xml数据,请先初始化 ");
                return "";
            }

            foreach (string sValue in UpLoadingData.XmlMd5List_Server)
            {
                string[] values = sValue.Split(new char[] { '@' });

                if (values[0].Equals(xName))
                {
                    md5Str = values[1];
                    break;
                }
            }

            return md5Str;
        }

        #endregion ----------------------------------------------------

        #region 查询本地文件的Md5值 -----------------------------------

        public static string UpLoading_Md5LocalGet(XmlMessageTypeEnum mEnum, string xName)
        {
            List<FileInfo> UpLoadingFileList = new List<FileInfo>();

            if (mEnum == XmlMessageTypeEnum.xmlUrl)
            {
                UpLoadingFileList = UpLoadingData.UpLoadingFileList;
            }
            else if (mEnum == XmlMessageTypeEnum.textureUrl)
            {
                UpLoadingFileList = UpLoadingData.UpLoadingTextureFileList;
            }

            string md5Str = "";
            
            foreach (FileInfo fInfo in UpLoadingFileList)
            {
                string[] fNames = fInfo.Name.Split(new char[] { '.' });

                if (fNames[0].Equals(xName))
                {
                    md5Str = Tool_Md5.MD5_File(fInfo.FullName);

                    break;
                }
            }

            return md5Str;
        }

        #endregion ----------------------------------------------------
    }
}