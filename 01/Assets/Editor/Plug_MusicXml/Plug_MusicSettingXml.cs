/****************************************************
*
*  创建音乐bundle文件的配置Xml文件
*  作者: lyb
*  日期：2017年7月25日
*
*****************************************************/

using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

namespace PlugProject
{
    public class Plug_MusicSettingXml : MusicXmlHelper
    {
        private static string MusicXmlPath = Application.dataPath + "/Resources/Music/MusicXml.xml";
        private static string MusicBackNodeName = "MusicBack";
        private static string MusicSoundNodeName = "MusicSound";
        private static string MusicMandarinNodeName = "MusicMandarin";
        private static string MusicHeNanNodeName = "MusicHeNan";

        private static string NodeBackValue = "music/music_back/";
        private static string NodeSoundValue = "music/music_sound/";
        private static string NodeMandarinValue = "music/music_voice/localism_henan/";
        private static string NodeHeNanValue = "music/music_voice/mandarin/";

        private static string MusicVersion = "1005";

        public static void MusicXmlCreat()
        {
            MusicXmlInit();
        }

        #region 创建音乐Xml文件 -----------------------------------

        private static void MusicXmlInit()
        {
            if (File.Exists(MusicXmlPath))
            {
                File.Delete(MusicXmlPath);
            }

            Tool_XmlOperation.Xml_Creat(MusicXmlPath);
            Tool_XmlOperation.Xml_CreatElement(MusicXmlPath, MusicBackNodeName);

            MusicBackXml_Add();
        }

        #endregion ------------------------------------------------

        #region 读取音乐文件 --------------------------------------

        private static void MusicBackXml_Add()
        {
            MusicXmlAdd(Music_BackPath, MusicBackNodeName, NodeBackValue);

            MusicSoundXml_Add();
        }

        private static void MusicSoundXml_Add()
        {
            Tool_XmlOperation.Xml_CreatElement(MusicXmlPath, MusicSoundNodeName);

            MusicXmlAdd(Music_SoundPath, MusicSoundNodeName, NodeSoundValue);

            MusicMandarinXml_Add();
        }

        private static void MusicMandarinXml_Add()
        {
            Tool_XmlOperation.Xml_CreatElement(MusicXmlPath, MusicMandarinNodeName);

            MusicXmlAdd(Music_MandarinPath, MusicMandarinNodeName, NodeMandarinValue);

            MusicHeNanXml_Add();
        }

        private static void MusicHeNanXml_Add()
        {
            Tool_XmlOperation.Xml_CreatElement(MusicXmlPath, MusicHeNanNodeName);

            MusicXmlAdd(Music_HeNanPath, MusicHeNanNodeName, NodeHeNanValue);

            AssetDatabase.Refresh();

            PlugMenuItemMain.PlugLog_Show(" #[Plug_MusicSettingXml]# 创建完毕 ");
        }

        public static void MusicXmlAdd(string mPath, string nodeName, string nodePath)
        {
            List<FileInfo> musicFileList = MusicFileAllGet(mPath);

            foreach (FileInfo fInfo in musicFileList)
            {
                string fName = fInfo.Name.Substring(0, fInfo.Name.LastIndexOf('.'));

                Tool_XmlOperation.Xml_Add(MusicXmlPath, nodeName, fName, nodePath + fName + ".unity3d?" + MusicVersion);
            }
        }

        #endregion ------------------------------------------------
    }
}