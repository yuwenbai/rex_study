/****************************************************
*
*  生成UpLoading的xml配置文件
*  作者: lyb
*  日期：2017年7月25日
*
*****************************************************/

using System.IO;
using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

namespace PlugProject
{
    public class Tool_UpLoadingXml : BaseUpLoading
    {
        private static string xmlNodeName = "UpXml";
        private static string textureNodeName = "UpTexture";

        [MenuItem("Tools/ConfigTools/Plug_UpLoadingFtp/CreatUpLoadingMd5Xml")]
        public static void CreatUpLoadingMd5Xml()
        {
            UpLoadingFind.UpLoading_DataInit(DataTypeEnum.Data_Version);

            //创建md5的xml
            UpLoadingMd5Xml_Creat();
        }

        [MenuItem("Tools/ConfigTools/Plug_UpLoadingFtp/UpLoadingMd5Xml")]
        public static void UpLoadingMd5Xml()
        {
            //更新xml的Md5
            List<FileInfo> xmlFileList = UpLoadingFileAllGet(UpLoadingData.UpXmlPath);

            if (xmlFileList.Count > 0)
            {
                foreach (FileInfo fInfo in xmlFileList)
                {
                    string[] fNames = fInfo.Name.Split(new char[] { '.' });

                    UpLoadingMd5_XmlUpdate(fNames[0]);
                }
            }
            else
            {
                UpLoading_ErrorBox(" #[Tool_UpLoadingXml]# 没有生成可以上传的xml文件 ");
            }
        }

        [MenuItem("Tools/ConfigTools/Plug_UpLoadingFtp/UpLoadingMd5Texture")]
        public static void UpLoadingMd5Texture()
        {
            //更新Texture的Md5
            List<FileInfo> textureFileList = UpLoadingFileAllGet(UpLoadingData.UpTexturePath);

            if (textureFileList.Count > 0)
            {
                foreach (FileInfo fInfo in textureFileList)
                {
                    string[] fNames = fInfo.Name.Split(new char[] { '.' });

                    UpLoadingMd5_TextureUpdate(fNames[0]);
                }
            }
            else
            {
                UpLoading_ErrorBox(" #[Tool_UpLoadingXml]# 没有可以上传的Texture文件 ");
            }
        }

        #region 创建UpLoadingXml文件 ------------------------------

        public static void UpLoadingMd5Xml_Creat()
        {
            if (File.Exists(UpLoadingData.UpLoadingMd5Path))
            {
                File.Delete(UpLoadingData.UpLoadingMd5Path);
            }

            Tool_XmlOperation.Xml_Creat(UpLoadingData.UpLoadingMd5Path);
            Tool_XmlOperation.Xml_CreatElement(UpLoadingData.UpLoadingMd5Path, xmlNodeName);
            Tool_XmlOperation.Xml_CreatElement(UpLoadingData.UpLoadingMd5Path, textureNodeName);

            AssetDatabase.Refresh();
        }

        #endregion ------------------------------------------------

        #region 更新xml的Md5值 ------------------------------------

        public static void UpLoadingMd5_XmlUpdate(string xName)
        {
            string mContent = Tool_XmlOperation.Xml_Find(UpLoadingData.UpLoadingMd5Path, xmlNodeName, xName);

            string md5Str = UpLoadingFind.UpLoading_Md5LocalGet(XmlMessageTypeEnum.xmlUrl, xName);

            if (!string.IsNullOrEmpty(mContent))
            {
                Tool_XmlOperation.Xml_Update(UpLoadingData.UpLoadingMd5Path, xName, "", md5Str);
            }
            else
            {
                Tool_XmlOperation.Xml_Add(UpLoadingData.UpLoadingMd5Path, xmlNodeName, xName, md5Str);
            }

            AssetDatabase.Refresh();
        }

        #endregion ------------------------------------------------

        #region 更新texture的Md5值 --------------------------------

        public static void UpLoadingMd5_TextureUpdate(string tName)
        {
            string mContent = Tool_XmlOperation.Xml_Find(UpLoadingData.UpLoadingMd5Path, textureNodeName, tName);

            string md5Str = UpLoadingFind.UpLoading_Md5LocalGet(XmlMessageTypeEnum.textureUrl, tName);

            if (!string.IsNullOrEmpty(mContent))
            {
                Tool_XmlOperation.Xml_Update(UpLoadingData.UpLoadingMd5Path, tName, "", md5Str);
            }
            else
            {
                Tool_XmlOperation.Xml_Add(UpLoadingData.UpLoadingMd5Path, textureNodeName, tName, md5Str);
            }

            AssetDatabase.Refresh();
        }

        #endregion ------------------------------------------------
    }
}