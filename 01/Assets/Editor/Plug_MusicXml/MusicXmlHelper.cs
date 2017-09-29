/****************************************************
*
*  创建音乐bundle文件的配置Xml文件的工具类
*  作者: lyb
*  日期：2017年7月25日
*
*****************************************************/

using System.IO;
using UnityEngine;
using System.Collections.Generic;

namespace PlugProject
{
    public class MusicXmlHelper
    {
        public static string Music_BackPath = Application.dataPath + "/Resources/Music/Music_Back";
        public static string Music_SoundPath = Application.dataPath + "/Resources/Music/Music_Sound";
        public static string Music_MandarinPath = Application.dataPath + "/Resources/Music/Music_Voice/Mandarin";
        public static string Music_HeNanPath = Application.dataPath + "/Resources/Music/Music_Voice/Localism_HeNan";

        #region 获取指定目录下的所有的文件 ------------------------

        public static List<FileInfo> MusicFileAllGet(string mPath)
        {
            List<FileInfo> musicFileList = new List<FileInfo>();

            DirectoryInfo dirInfo = new DirectoryInfo(mPath);

            FileInfo[] fileList = dirInfo.GetFiles();

            for (int i = 0; i < fileList.Length; i++)
            {
                if (fileList[i].Extension == ".ogg" || fileList[i].Extension == ".mp3")
                {
                    musicFileList.Add(fileList[i]);
                }
            }

            return musicFileList;
        }

        #endregion ------------------------------------------------
    }
}