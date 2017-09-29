/****************************************************
*
*  更新配置文件里的数据
*  支持 添加，修改 功能
*  作者: lyb
*  日期：2017年7月25日
*
*****************************************************/

using UnityEngine;

namespace PlugProject
{
    public class UpLoadingUpdate : BaseUpLoading
    {
        #region 配置文件数据更新 -------------------------------------

        /// <summary>
        /// xml数据更新
        /// mEnum 要更新的组的枚举
        /// xName 要更新的数据的名字
        /// xVersion 要更新的版本号
        /// </summary>
        public static void UpLoading_DataUpdate(XmlMessageTypeEnum mEnum, string xName, int xVersion)
        {
            if (!IsInitSucc())
            {
                UpLoading_ErrorBox(" #[UpLoadingUpdate]# 未初始化,请先初始化 ");
                return;
            }

            if (mEnum == XmlMessageTypeEnum.xmlUrl)
            {
                Tool_UpLoadingXml.UpLoadingMd5_XmlUpdate(xName);
            }
            else
            {
                Tool_UpLoadingXml.UpLoadingMd5_TextureUpdate(xName);
            }

            string messageStr = UpLoad_MessageStr(mEnum);

            string mContent = Tool_XmlOperation.Xml_Find(UpLoadingData.UpLoadingFilePath, messageStr, xName);

            if (!string.IsNullOrEmpty(mContent))
            {
                string[] values = mContent.Split(new char[] { '?' });

                string targetStr = values[0] + "?" + xVersion.ToString();

                Tool_XmlOperation.Xml_Update(UpLoadingData.UpLoadingFilePath, xName, "", targetStr);

                Debug.Log(" #[上传Ftp工具]# UpLoadingUpdate - 更新 【 " + xName + "】 数据表完毕");
            }
            else
            {
                Debug.Log(" #[上传Ftp工具]# UpLoadingUpdate - 没有找到相关的数据信息, 添加进Xml文件");

                UpLoading_DataAdd(mEnum, xName, xVersion);
            }
        }

        #endregion ----------------------------------------------------

        #region 配置文件数据添加 --------------------------------------

        /// <summary>
        /// xml数据添加
        /// </summary>
        private static void UpLoading_DataAdd(XmlMessageTypeEnum mEnum, string xName, int xVersion)
        {
            if (!IsInitSucc())
            {
                UpLoading_ErrorBox(" #[UpLoadingUpdate]# 未初始化,请先初始化 ");
                return;
            }

            string messageStr = UpLoad_MessageStr(mEnum);

            string mContent = Tool_XmlOperation.Xml_Find(UpLoadingData.UpLoadingFilePath, messageStr, xName);

            if (string.IsNullOrEmpty(mContent))
            {
                string tDoc = "";
                if (mEnum == XmlMessageTypeEnum.xmlUrl)
                {
                    tDoc = UpLoad_XmlAdd(messageStr, xName, xVersion);
                }
                else if (mEnum == XmlMessageTypeEnum.textureUrl)
                {
                    tDoc = UpLoad_TextureAdd(messageStr, xName, xVersion);
                }

                Tool_XmlOperation.Xml_Add(UpLoadingData.UpLoadingFilePath, messageStr, xName, tDoc);

                Debug.Log(" #[上传Ftp工具]# UpLoadingUpdate - 添加 【 " + xName + "】 数据完毕");
            }
            else
            {
                UpLoading_ErrorBox(" #[UpLoadingUpdate]# 该数据存在，不可重复添加 ");
            }
        }

        /// <summary>
        /// xml数据添加
        /// </summary>
        private static string UpLoad_XmlAdd(string mId, string xName, int xVersion)
        {
            string tDoc = string.Format("DownXmlAssets/{0}.zip.dat?{1}", xName, xVersion);

            return tDoc;
        }

        /// <summary>
        /// texture数据添加
        /// </summary>
        private static string UpLoad_TextureAdd(string mId, string xName, int xVersion)
        {
            string tDoc = string.Format("DownTextureAssets/{0}.png?{1}", xName, xVersion);

            return tDoc;
        }

        #endregion ----------------------------------------------------

        #region 配置文件版本号修改 ------------------------------------

        public static void UpLoading_VersionUpdate(string newVersion)
        {
            Tool_XmlOperation.Xml_UpdateVersion(UpLoadingData.UpLoadingFilePath, newVersion);
        }

        #endregion ----------------------------------------------------
    }
}