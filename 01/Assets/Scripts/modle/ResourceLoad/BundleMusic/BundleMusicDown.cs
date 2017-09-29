/****************************************************
*
*  下载服务器上的音乐资源
*  作者: lyb
*  日期：2017年7月25日
*
*****************************************************/

using System.IO;
using UnityEngine;
using System.Collections;

namespace projectQ
{
    public class BundleMusicDown : BaseResourceLoad
    {
        private int music_downNum = 0;
        private int music_allNum;
        private bool isMusicDown = false;

        /// <summary>
        /// 当前下载音乐资源的类型
        /// </summary>
        private MusicDownTypeEnum downType;
        /// <summary>
        /// 当前下载列表
        /// </summary>
        private ArrayList downMusicList = new ArrayList();
        /// <summary>
        /// 不同平台下的资源下载路径
        /// </summary>
        private string downServerPath;

        void Start() { }

        void Update()
        {
            if (isMusicDown)
            {
                if (music_downNum >= music_allNum)
                {
                    DebugPro.LogBundle(" #[资源加载-音乐]# BundleMusicDown - 服务器音乐资源下载完毕 ");

                    isMusicDown = false;

                    MusicDown_Finish();
                }
            }
        }

        #region lyb 资源下载完毕-----------------------------------------

        /// <summary>
        /// 资源下载完成
        /// </summary>
        void MusicDown_Finish()
        {
            using (System.Net.WebClient wc = new System.Net.WebClient())
            {
                wc.DownloadFile(GameConfig.Instance.MusicXml_ServerUrl, GameConfig.Instance.MusicXml_ClientPath);
            }

            DebugPro.LogBundle(" #[资源加载-音乐]# 下载完毕。。。");

            EventDispatcher.FireEvent(EventKey.Bundle_MusicDownAddMemory_Event, downType);

            EventDispatcher.FireEvent(EventKey.Bundle_MusicDownFinish_Event, downType);
        }

        #endregion ------------------------------------------------------

        /// <summary>
        /// 音乐下载初始化 - 直接下载
        /// mDownList 下载列表
        /// </summary>
        public void MusicDown_Init(MusicDownTypeEnum mType, ArrayList mDownList)
        {
            if (isMusicDown)
            {
                DebugPro.LogBundle(" #[资源加载-音乐]# 正在下载中。。。");
                return;
            }

            downType = mType;
            downMusicList = new ArrayList();
            downMusicList = mDownList;

            MusicDown_Begin();
        }

        /// <summary>
        /// 音乐下载初始化 - 服务器资源和客户端资源对比更新下载
        /// mDownList 下载列表
        /// </summary>
        public void MusicDown_Init(MusicDownTypeEnum mType, ArrayList mServerList, ArrayList mClientList)
        {
            downType = mType;
            downMusicList = new ArrayList();

            Music_ServerCompareClient(mServerList, mClientList);
        }

        #region lyb Music文件对比----------------------------------------

        /// <summary>
        /// 服务器和客户端资源比对
        /// </summary>
        public void Music_ServerCompareClient(ArrayList musicServerList, ArrayList musicClientList)
        {
            foreach (string urlStr in musicServerList)
            {
                string[] values = urlStr.Split(new char[] { '@' });

                string checkName = ResourceUpdate_Check(musicClientList, values[0], values[1]);

                if (!string.IsNullOrEmpty(checkName))
                {
                    //说明在本地列表中发现了需要更新的资源或者新添加的资源

                    string path = GameAssetCache.Instance.MusicSave_FilePath(downType) + "/" + checkName + ".unity3d";

                    if (File.Exists(path))
                    {
                        File.Delete(path);
                    }

                    if (checkName.Equals("New"))
                    {
                        DebugPro.LogBundle(" #[资源加载-音乐]# BundleMusicDown - 新的音乐下载 = " + values[0]);
                    }
                    else
                    {
                        DebugPro.LogBundle(" #[资源加载-音乐]# BundleMusicDown - 需要更新下载的音乐 = " + checkName);
                    }

                    downMusicList.Add(urlStr);
                }
            }

            if (downMusicList.Count > 0)
            {
                DebugPro.LogBundle(" #[资源加载-音乐]# BundleMusicDown - 音乐有更新,进入下载流程 ");

                MusicDown_Begin();
            }
            else
            {
                DebugPro.LogBundle(" #[资源加载-音乐]# BundleMusicDown - 音乐没有更新,进入游戏 ");

                MusicDown_Finish();
            }
        }

        #endregion ------------------------------------------------------

        #region lyb Music文件下载----------------------------------------

        /// <summary>
        /// 音乐资源开始下载
        /// </summary>
        void MusicDown_Begin()
        {
            downServerPath = GameConfig.Instance.Resource_ServerPath + "AssetBundles/Windows/";
#if UNITY_ANDROID
            downServerPath = GameConfig.Instance.Resource_ServerPath + "AssetBundles/Android/";
#elif UNITY_IPHONE
            downServerPath = GameConfig.Instance.Resource_ServerPath + "AssetBundles/IOS/";
#endif

            music_downNum = 0;

            music_allNum = downMusicList.Count;

            isMusicDown = true;

            MusicDown_ServerLoad();
        }

        /// <summary>
        /// 获取Url地址
        /// </summary>
        void MusicDown_ServerLoad()
        {
            if (music_downNum < music_allNum)
            {
                string[] values = downMusicList[music_downNum].ToString().Split(new char[] { '@' });

                EventDispatcher.FireEvent(EventKey.Bundle_LoadName_Event, 8, values[0]);

                StartCoroutine(DownLoadMusic(downServerPath + values[1], values[0]));
            }
        }

        IEnumerator DownLoadMusic(string mUrl, string mName)
        {
            WWW www = new WWW(mUrl);

            yield return www;

            if (www.isDone && www.error == null)
            {
                byte[] data = www.bytes;

                string fileName = GameAssetCache.Instance.MusicSave_FilePath(downType) + "/" + mName + ".unity3d";

                File.WriteAllBytes(fileName, data);

                music_downNum++;

                EventDispatcher.FireEvent(EventKey.Bundle_SliderValue_Event,
                        BundleStepEnum.Bundle_MusicDown, music_downNum, music_allNum);

                MusicDown_ServerLoad();
            }
            else
            {
                QLoger.LOG(LogType.EError, " 请您稍后重试[m3] www.error = " + www.error + "当前的Url = " + mUrl);

                BundleErrorPrompt(ResourceConfig.Bundle_Text506);
            }
        }

        #endregion ------------------------------------------------------
    }
}