/****************************************************
*
*  把下载好的音乐加载到内存中
*  作者: lyb
*  日期：2017年7月25日
*
*****************************************************/

using UnityEngine;
using System.Collections;

namespace projectQ
{
    public class BundleMusicLoad : BaseResourceLoad
    {
        void Start() { }

        /// <summary>
        /// 把下载好的音乐加载到内存中
        /// </summary>
        public void MusicLoad_Init(MusicDownTypeEnum mType)
        {
            GameConfig.Instance.MusicBgList_Client = new ArrayList();
            GameConfig.Instance.MusicSoundList_Client = new ArrayList();
            GameConfig.Instance.MusicMandarinList_Client = new ArrayList();
            GameConfig.Instance.MusicHeNanList_Client = new ArrayList();

            Xml_MusicLoad(XmlLoadEnum.XmlLoad_Client);

            if (mType == MusicDownTypeEnum.MusicDown_Bg)
            {
                MusicLoad_AddMemory(mType, GameConfig.Instance.MusicBgList_Client);
            }
            else if (mType == MusicDownTypeEnum.MusicDown_Sound)
            {
                MusicLoad_AddMemory(mType, GameConfig.Instance.MusicSoundList_Client);
            }
            else if (mType == MusicDownTypeEnum.MusicDown_VoiceMandarin)
            {
                MusicLoad_AddMemory(mType, GameConfig.Instance.MusicMandarinList_Client);
            }
            else if (mType == MusicDownTypeEnum.MusicDown_VoiceHeNan)
            {
                MusicLoad_AddMemory(mType, GameConfig.Instance.MusicHeNanList_Client);
            }
        }

        /// <summary>
        /// 根据客户端列表读取bundle加载到内存
        /// </summary>
        void MusicLoad_AddMemory(MusicDownTypeEnum mType, ArrayList mClientList)
        {
            foreach (string urlStr in mClientList)
            {
                string[] values = urlStr.Split(new char[] { '@' });

                string fileName = GameAssetCache.Instance.MusicSave_FilePath(mType) + "/" + values[0] + ".unity3d";

                AssetBundle ab = AssetBundle.LoadFromFile(fileName);

                AudioClip audio = ab.LoadAsset(values[0]) as AudioClip;

                if (!GameConfig.Instance.DownMusicDic.ContainsKey(values[0]))
                {
                    GameConfig.Instance.DownMusicDic.Add(values[0], audio);
                }

                ab.Unload(false);
            }
        }
    }
}