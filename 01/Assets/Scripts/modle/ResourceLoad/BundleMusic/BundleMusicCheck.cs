/****************************************************
*
*  音乐检测下载
*  作者: lyb
*  日期：2017年7月25日
*
*****************************************************/

using System.IO;
using System.Collections;

namespace projectQ
{
    public class BundleMusicCheck : BaseResourceLoad
    {
        /// <summary>
        /// 当前下载列表
        /// </summary>
        private ArrayList downMusicList = new ArrayList();

        void Start() { }

        /// <summary>
        /// 音乐下载初始化
        /// mType 下载的音乐类型
        /// </summary>
        public void MusicDown_CheckInit(MusicDownTypeEnum mType)
        {
            downMusicList = new ArrayList();

            if (mType == MusicDownTypeEnum.MusicDown_Bg)
            {
                MusicDown_Check(mType, GameConfig.Instance.MusicBgList_Server, GameConfig.Instance.MusicBgList_Client,
                    GameConfig.Instance.MusicBgLocal_Path);
            }
            else if (mType == MusicDownTypeEnum.MusicDown_Sound)
            {
                MusicDown_Check(mType, GameConfig.Instance.MusicSoundList_Server, GameConfig.Instance.MusicSoundList_Client,
                    GameConfig.Instance.MusicSoundLocal_Path);
            }
            else if (mType == MusicDownTypeEnum.MusicDown_VoiceMandarin)
            {
                MusicDown_Check(mType, GameConfig.Instance.MusicMandarinList_Server, GameConfig.Instance.MusicMandarinList_Client,
                    GameConfig.Instance.MusicMandarinLocal_Path);
            }
            else if (mType == MusicDownTypeEnum.MusicDown_VoiceHeNan)
            {
                MusicDown_Check(mType, GameConfig.Instance.MusicHeNanList_Server, GameConfig.Instance.MusicHeNanList_Client,
                    GameConfig.Instance.MusicHeNanLocal_Path);
            }
        }

        /// <summary>
        /// 音乐检测
        /// mServerList 服务器资源列表
        /// mClientList 客户端资源列表
        /// mLocalPath 客户端资源路径
        /// </summary>
        void MusicDown_Check(MusicDownTypeEnum downType, ArrayList mServerList, ArrayList mClientList, string mLocalPath)
        {
            downMusicList = mServerList;

            bool isMDown = ResourceCorrect_Check(mClientList, mLocalPath, ".unity3d");

            BundleMusicDown musicDown = Flow_MusicDown();

            EventDispatcher.FireEvent(EventKey.Bundle_SliderValueShow_Event, BundleStepEnum.Bundle_MusicDown);

            if (isMDown)
            {
                DebugPro.LogBundle(" #[资源加载-音乐]# BundleMusicCheck - 客户端上资源与本地存储资源不匹配，重新更新下载 ");

                Tools_FileOperation.Files_DeleteAll(mLocalPath);

                if (File.Exists(GameConfig.Instance.MusicXml_ClientPath))
                {
                    File.Delete(GameConfig.Instance.MusicXml_ClientPath);
                }

                musicDown.MusicDown_Init(downType, mServerList);
            }
            else
            {
                DebugPro.LogBundle(" #[资源加载-音乐]# BundleMusicCheck - 客户端上有资源与本地存储资源进行对比看是否需要更新 ");

                musicDown.MusicDown_Init(downType, mServerList, mClientList);
            }
        }

        #region lyb 进入下一步音乐下载的流程----------------------

        private BundleMusicDown Flow_MusicDown()
        {
            BundleMusicDown musicDown = gameObject.GetComponent<BundleMusicDown>();

            if (musicDown == null)
            {
                musicDown = gameObject.AddComponent<BundleMusicDown>();
            }

            return musicDown;
        }

        #endregion -----------------------------------------------
    }
}