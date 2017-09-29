/****************************************************
*
*  传过来的下载类型，先进行本地比对，确定下载还是更新
*  作者: lyb
*  日期：2017年7月25日
*
*****************************************************/

using System.IO;
using UnityEngine;
using System.Collections;

namespace projectQ
{
    public class BundleMusicInit : BaseResourceLoad
    {
        void Start()
        {
            BundleMusicDataInit();
        }

        #region lyb 读取服务器上的音乐bundle配置文件--------------

        void BundleMusicDataInit()
        {
            if (GameConfig.Instance.IsDown == "1")
            {
                //EventDispatcher.FireEvent(EventKey.Bundle_LoadName_Event, 1, "...");
                StartCoroutine(GetServerMusicXml());
            }
            else
            {
                EventDispatcher.FireEvent(EventKey.Bundle_MusicDownFinish_Event);
            }
        }

        IEnumerator GetServerMusicXml()
        {
            WWW www = new WWW(GameConfig.Instance.MusicXml_ServerUrl);
            yield return www;

            if (www.isDone && www.error == null)
            {
                LoadMusicXml_Server(www);
            }
            else
            {
                QLoger.LOG(LogType.EError, " 请您稍后重试[m1] www.error = " + www.error);

                BundleErrorPrompt(ResourceConfig.Bundle_Text507);
            }
        }

        /// <summary>
        /// 解析服务器上获得的xml
        /// </summary>
        void LoadMusicXml_Server(WWW www)
        {
            if (string.IsNullOrEmpty(www.text))
            {
                BundleErrorPrompt(ResourceConfig.Bundle_Text508);
                return;
            }

            Xml_MusicLoad(XmlLoadEnum.XmlLoad_Server, www.text);

            LocalFileMusic_Check();
        }

        #endregion -----------------------------------------------

        #region lyb 检测当前本地是否有已经下载好的音乐bundle文件--

        /// <summary>
        /// 初始化检测
        /// mType 音乐下载的类型
        /// </summary>
        void LocalFileMusic_Check()
        {
            this.Resource_MusicFileCheck();

            if (File.Exists(GameConfig.Instance.MusicXml_ClientPath))
            {
                DebugPro.LogBundle(" #[资源加载-音乐]# BundleMusicInit - 本地记录音乐版本信息文件存在 ");

                Xml_MusicLoad(XmlLoadEnum.XmlLoad_Client);
            }
            else
            {
                DebugPro.LogBundle(" #[资源加载-音乐]# BundleMusicInit - 本地记录音乐版本信息文件不存在 ");
            }

            EventDispatcher.FireEvent(EventKey.Bundle_MusicDownBegin_Event, MusicDownTypeEnum.MusicDown_Bg);
        }

        #endregion -----------------------------------------------

        #region lyb 检测本地文件夹是否存在，不存在则创建----------

        void Resource_MusicFileCheck()
        {
            if (!Directory.Exists(GameConfig.Instance.MusicBgLocal_Path))
            {
                Directory.CreateDirectory(GameConfig.Instance.MusicBgLocal_Path);
            }

            if (!Directory.Exists(GameConfig.Instance.MusicSoundLocal_Path))
            {
                Directory.CreateDirectory(GameConfig.Instance.MusicSoundLocal_Path);
            }

            if (!Directory.Exists(GameConfig.Instance.MusicMandarinLocal_Path))
            {
                Directory.CreateDirectory(GameConfig.Instance.MusicMandarinLocal_Path);
            }

            if (!Directory.Exists(GameConfig.Instance.MusicHeNanLocal_Path))
            {
                Directory.CreateDirectory(GameConfig.Instance.MusicHeNanLocal_Path);
            }
        }

        #endregion -----------------------------------------------
    }
}