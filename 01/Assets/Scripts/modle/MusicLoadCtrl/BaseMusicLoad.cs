/****************************************************
*
*  音乐、音效控制脚本基类
*  作者: lyb
*  日期：2017年7月25日
*
*****************************************************/

using UnityEngine;
using System.Collections.Generic;

namespace projectQ
{
    public abstract class BaseMusicLoad : MonoBehaviour
    {
        /// <summary>
        /// 正在播放的音效列表
        /// </summary>
        private Dictionary<string, GameObject> _MWavList = new Dictionary<string, GameObject>();
        public Dictionary<string, GameObject> MWavList
        {
            get { return _MWavList; }
            set { _MWavList = value; }
        }

        #region 获取音乐---------------------------

        public AudioClip MusicAudio_Set(string mPath)
        {
            string[] values = mPath.Split(new char[] { '/' });

            string mName = values[values.Length - 1];

            AudioClip audio = null;

            if (GameConfig.Instance.IsDown == "1")
            {
                if (GameConfig.Instance.DownMusicDic.ContainsKey(mName))
                {
                    audio = GameConfig.Instance.DownMusicDic[mName];
                }
                else
                {
                    audio = ResourcesDataLoader.Load<AudioClip>(mPath);
                }
            }
            else
            {
                audio = ResourcesDataLoader.Load<AudioClip>(mPath);
            }

            return audio;
        }

        #endregion---------------------------------

        #region 通过枚举获取音效路径---------------

        public string SoundAudio_Set(GEnum.SoundEnum sEnum)
        {
            string sName = "";

            if (sEnum != GEnum.SoundEnum.btn_null)
            {
                sName = MemoryData.XmlData.XmlBuildDataSound_Get(sEnum);

                if (string.IsNullOrEmpty(sName))
                {
                    QLoger.LOG(" #[音效]# 没有找到相应的音效返回");
                    return "";
                }
            }
            else
            {
                QLoger.LOG(" #[音效]# btn_null 无效音效");
                return "";
            }

            return GameAssetCache.M_Sound_Path + sName;
        }

        #endregion---------------------------------

        #region 工具方法---------------------------

        /// <summary>
        /// 把本地所有的音效全部删除
        /// </summary>
        public void Music_StopAll()
        {
            foreach (GameObject mObj in MWavList.Values)
            {
                AudioSource aSource = mObj.GetComponent<AudioSource>();

                if (aSource != null)
                {
                    aSource.volume = 0;
                }
            }
        }

        /// <summary>
        /// 停止音效
        /// path -- 需要停止的音效路径
        /// </summary>
        public void Music_Stop(string path)
        {
            if (MWavList.ContainsKey(path))
            {
                GameObject obj = MWavList[path];
                GameObject.Destroy(obj);
                Music_Delete(path);
            }
        }

        /// <summary>
        /// 往列表中添加正在播放的音频路径
        /// </summary>
        public void Music_Add(string path, GameObject mObj)
        {
            if (!MWavList.ContainsKey(path))
            {
                MWavList.Add(path, mObj);
            }
        }

        /// <summary>
        /// 删除列表中有的音频路径
        /// </summary>
        public void Music_Delete(string path)
        {
            if (MWavList.ContainsKey(path))
            {
                MWavList.Remove(path);
                Music_StopDelegate(path);
            }
        }

        /// <summary>
        /// 音乐播放完毕之后的行为动作展示
        /// 比如该音乐播完播动画之类的
        /// </summary>
        void Music_StopDelegate(string path)
        {
            GameDelegateCache.C2CMusicStopDelegate(path);
        }

        #endregion---------------------------------

        #region 获取当前地区的相关数据-------------

        /// <summary>
        /// 获取该地区下的
        /// areaId 人身上的地区ID
        /// voice 如果该类不为空 则返回该类对应数据
        /// </summary>
        public string Music_AreaPathGet(MusicVoice voice = null)
        {
            var deskInfo = MemoryData.DeskData.GetOneDeskInfo(MjDataManager.Instance.MjData.curUserData.selfDeskID);
            int regionId = MemoryData.MahjongPlayData.GetMahjongPlayByConfigId(deskInfo.mjGameConfigId).RegionID;

            if (regionId == -1)
            {
                return "";
            }

            string regionName = MemoryData.XmlData.XmlBuildDataRegion_Get(regionId);

            string musicVoice = "";
            switch (regionName)
            {
                case "河南省":
                    if (voice != null)
                    {
                        musicVoice = voice.Localism_HeNan;
                    }
                    else
                    {
                        musicVoice = GameAssetCache.M_Voice_Localism_HeNan_Path;
                    }
                    break;
                case "山东省":
                    if (voice != null)
                    {
                        musicVoice = voice.Localism_ShanDong;
                    }
                    else
                    {
                        musicVoice = GameAssetCache.M_Voice_Localism_ShanDong_Path;
                    }
                    break;
                default:
                    break;
            }

            return musicVoice;
        }

        #endregion---------------------------------
    }
}