/****************************************************
*
*  音乐、音效控制脚本
*  作者: lyb
*  日期：2017年7月25日
*
*****************************************************/

using UnityEngine;
using System.Collections.Generic;

namespace projectQ
{
    [RequireComponent(typeof(AudioSource))]
    public class MusicCtrl : BaseMusicLoad
    {
        public static void LOG(LogType t, string p, params object[] v)
        {
            QLoger.LOG(typeof(MusicCtrl), t, p, v);
        }

        private static MusicCtrl _Instance;
        public static MusicCtrl Instance
        {
            get { return _Instance; }
        }

        void Awake()
        {
            _Instance = this;
        }

        void OnDestroy()
        {
            _Instance = null;
            MWavList.Clear();
        }

        public AudioSource musicMp3;

        /// <summary>
        /// 音效音量
        /// </summary>
        private float soundVolume = 1.0f;

        void Start()
        {
            musicMp3 = GetComponent<AudioSource>();
        }

        #region 音乐相关---------------------------------------------------------

        /// <summary>
        /// 切换背景音乐
        /// </summary>
        public void Music_BackChangePlay(string path)
        {
            string[] values = path.Split(new char[] { '/' });

            if (musicMp3.clip != null)
            {
                if (musicMp3.clip.name.Equals(values[values.Length - 1]))
                {
                    // 切换的背景音乐是一样的  不予执行
                    return;
                }
            }

            AudioClip audio = MusicAudio_Set(path);
            musicMp3.clip = audio;
            musicMp3.Play();

            Music_BackVolumeChange(true);
        }

        /// <summary>
        /// 设置背景音乐的大小
        /// 是否设置背景音乐的大小
        /// </summary>
        public void Music_BackVolumeChange(bool isOpen)
        {
            // 2 开启 1 关闭
            int optionValue = PlayerPrefsTools.GetInt("OPTION_MUSIC");
            if (optionValue == 2)
            {
                if (isOpen)
                {
                    musicMp3.volume = 1.0f;
                }
                else
                {
                    musicMp3.volume = 0;
                }
            }
            else
            {
                musicMp3.volume = 0;
            }
        }

        /// <summary>
        /// 设置音效的大小
        /// </summary>
        public void Music_SoundVolumeChange(bool isOpen)
        {
            soundVolume = isOpen == true ? 1.0f : 0;

            if (!isOpen)
            {
                Music_StopAll();
            }
        }

        #endregion---------------------------------------------------------------

        #region 音效相关---------------------------------------------------------

        /// <summary>
        /// 播放音效的调用接口
        /// path -- 播放的音效路径        
        /// </summary>
        public void Music_SoundPlay(GEnum.SoundEnum soundEnum)
        {
            string soundPath = SoundAudio_Set(soundEnum);

            if (soundPath == "")
            {
                return;
            }

            // 音效 2 开启 1 关闭
            int optionEffect = PlayerPrefsTools.GetInt("OPTION_EFFECT");
            if (optionEffect == 1)
            {
                return;
            }

            Music_SoundCreat(soundPath);
        }

        /// <summary>
        /// 创建一个音乐播放器。。播放该音乐
        /// isLoop -- 是否需要循环播放
        /// volume -- 音量大小
        /// </summary>
        void Music_SoundCreat(string path, bool isLoop = false)
        {
            if (!MWavList.ContainsKey(path))
            {
                GameObject prefabObj = ResourcesDataLoader.Load<GameObject>(GameAssetCache.Prefab_MusicWavList_Path);
                GameObject go = NGUITools.AddChild(gameObject, prefabObj);
                MusicWavList list = go.GetComponent<MusicWavList>();
                list.LoadMusicPlay(path, isLoop, soundVolume);
            }
        }

        #endregion---------------------------------------------------------------

        #region 语音相关---------------------------------------------------------

        /// <summary>
        /// 播放语音的调用接口
        /// musicId -- 对应数据表中的ID字段
        /// </summary>
        public void Music_VoicePlay(int musicId, long uid = -1)
        {
            string vPath = Music_VoiceNameGet(musicId);

            Music_VoiceCreat(vPath, uid);
        }

        /// <summary>
        /// 获取数据表中的语音文件
        /// </summary>
        private string Music_VoiceNameGet(int voiceId)
        {
            string voicePath = "";

            List<BaseXmlBuild> buildList = MemoryData.XmlData.XmlBuildDataDic["MusicVoice"];

            foreach (BaseXmlBuild build in buildList)
            {
                MusicVoice voice = (MusicVoice)build;
                if (voice.SoundTypeID == voiceId.ToString())
                {
                    // 普通话 2 开启 1 关闭
                    int optionLanguage = PlayerPrefsTools.GetInt("OPTION_LANGUAGE");
                    if (optionLanguage == 2)
                    {
                        //普通话
                        voicePath = GameAssetCache.M_Voice_Mandarin_Path + Music_VoiceNameRandom(voice.Mandarin);
                    }
                    else
                    {
                        //方言
                        voicePath = Music_AreaPathGet(voice);
                        if (voicePath == "")
                        {
                            voicePath = GameAssetCache.M_Voice_Mandarin_Path + Music_VoiceNameRandom(voice.Mandarin);
                        }
                        else
                        {
                            voicePath = Music_AreaPathGet() + Music_VoiceNameRandom(voicePath);
                        }
                    }
                }
            }
            return voicePath;
        }

        /// <summary>
        /// 如果是多个值，则随机循环找到一个
        /// </summary>
        /// <returns></returns>
        private string Music_VoiceNameRandom(string mName)
        {
            string musicName = mName;

            string[] values = mName.Split(new char[] { ':' });

            if (values.Length >= 2)
            {
                //多个音乐，随机一个出来
                int num = Random.Range(0, values.Length);

                musicName = values[num];
            }

            return musicName;
        }

        /// <summary>
        /// 播放语音音效
        /// vPath -- 播放的音效路径
        /// </summary>
		public void Music_VoiceCreat(string vPath, long uid = -1)
        {
            if (vPath == "")
            {
                return;
            }

            // 音效 2 开启 1 关闭
            int optionEffect = PlayerPrefsTools.GetInt("OPTION_EFFECT");
            if (optionEffect == 1)
            {
                return;
            }

            //判断男女 _1 男的  _2 女的
            string sexStr = "_1";
            int sex = -1;

            //通过ID获取性别
            if (uid != -1)
            {
                var u = MemoryData.PlayerData.CheckModel(uid);
                if (u != null)
                {
                    sex = u.PlayerDataBase.Sex;
                }
                else
                {
                    LOG(LogType.EError, "获取到客户端没有数据的ID {0}", uid);
                }
            }
            else
            {
                sex = MemoryData.PlayerData.MyPlayerModel.PlayerDataBase.Sex;
            }

            if (sex > 0)
            {
                sexStr = "_" + sex.ToString();
            }
            else
            {
                LOG(LogType.EError, "使用默认性别{1}语音播放 {0}", uid, sexStr);
            }

            vPath = vPath + sexStr;

            Music_SoundCreat(vPath);
        }

        #endregion---------------------------------------------------------------
    }
}