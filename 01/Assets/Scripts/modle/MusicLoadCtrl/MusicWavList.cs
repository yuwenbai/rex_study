/****************************************************
*
*  控制单个音效播放脚本，用完即销毁
*  作者: lyb
*  日期：2017年7月25日
*
*****************************************************/

using UnityEngine;

namespace projectQ
{
    public class MusicWavList : BaseMusicLoad
    {
        public AudioSource musicWav;
        private bool isPlayBol = false;
        private string musicPath = "";

        void Awake()
        {
            musicWav = gameObject.GetComponent<AudioSource>();
        }

        void Start() { }

        void Update()
        {
            if (isPlayBol)
            {
                if (!musicWav.isPlaying)
                {
                    MusicCtrl.Instance.Music_Delete(musicPath);
                    GameObject.DestroyImmediate(gameObject);
                }
            }
        }

        void OnDestroy()
        {
            musicWav = null;
        }

        /// <summary>
        /// 播放音频
        /// </summary>
        public void LoadMusicPlay(string path, bool isLoop, float volume)
        {
            musicPath = path;

            AudioClip audio = MusicAudio_Set(path);
            musicWav.clip = audio;
            musicWav.loop = isLoop;
            musicWav.volume = volume;
            MusicCtrl.Instance.Music_Add(musicPath, gameObject);
            musicWav.Play();
            isPlayBol = true;
        }
    }
}