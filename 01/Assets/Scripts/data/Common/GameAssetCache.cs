using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace projectQ
{
    public class GameAssetCache : SingletonTamplate<GameAssetCache>
    {
        #region Prefab-----------------------------

        /// <summary>
        /// 音乐总控制prefab
        /// </summary>
        public const string Prefab_MusicCtrl_Path = "UIPrefab/System/MusicCtrl";
        /// <summary>
        /// 单个音效prefab
        /// </summary>
        public const string Prefab_MusicWavList_Path = "UIPrefab/System/MusicWavList";
        /// <summary>
        /// 资源加载总控制prefab
        /// </summary>
        public const string Prefab_BundleBox_Path = "UIPrefab/System/BundleBox";
        /// <summary>
        /// 发送消息的小Loading条prefab
        /// </summary>
        public const string Prefab_SendLoading_Path = "UIPrefab/UILoadingSend/UILoadingSend";
        /// <summary>
        /// Bundle版本更新弹出框
        /// </summary>
        public const string Prefab_MessageBox_Path = "UIPrefab/System/MessageBox/MessageBox";
        /// <summary>
        /// Bundle版本更新下载apk显示框
        /// </summary>
        public const string Prefab_BundleDownApk_Path = "UIPrefab/System/BundleDownApk/BundleDownApk";
        /// <summary>
        /// 地区选择小框
        /// </summary>
        public const string Prefab_UIMapSelect_Path = "UIPrefab/UIMap/UIMapSelect";

        #endregion---------------------------------

        #region 图片-------------------------------

        /// <summary>
        /// 默认活动图片
        /// </summary>
        public const string Texture_Activity_Path = "Texture/Tex_Common/Texture_Activity";
        /// <summary>
        /// 默认头像图片
        /// </summary>
        public const string Texture_Hand_Path = "Texture/Tex_Common/Texture_head_01";
        /// <summary>
        /// 麻将馆大厅背景图
        /// </summary>
        public const string Texture_MainMahjongBg_Path = "Texture/Tex_Common/Tex_MainMahjongBg";
        /// <summary>
        /// 普通大厅背景图
        /// </summary>
        public const string Texture_MainBg_Path = "Texture/Tex_Common/Tex_MainBg";
        /// <summary>
        /// 背景图02
        /// </summary>
        public const string Texture_MainBg02_Path = "Texture/Tex_Common/Tex_MainBg02";

        #endregion---------------------------------

        #region Music(音乐)------------------------

        /// <summary>
        /// 369麻将-大厅音乐
        /// </summary>
        public const string M_Back_01_Path = "Music/Music_Back/soundbgm_music1";

        #endregion---------------------------------

        #region Sound(音效)------------------------

        /// <summary>
        /// 路径基础路径
        /// </summary>
        public const string M_Sound_Path = "Music/Music_Sound/";

        #endregion---------------------------------

        #region Voice(语音)------------------------

        /// <summary>
        /// 普通话
        /// </summary>
        public const string M_Voice_Mandarin_Path = "Music/Music_Voice/Mandarin/";
        /// <summary>
        /// 方言 - 河南
        /// </summary>
        public const string M_Voice_Localism_HeNan_Path = "Music/Music_Voice/Localism_HeNan/";
        /// <summary>
        /// 方言 - 山东
        /// </summary>
        public const string M_Voice_Localism_ShanDong_Path = "Music/Music_Voice/Localism_ShanDong/";

        #endregion---------------------------------

        #region AnimationCurve---------------------

        public const string LotteryAnimation = "Animation/UILottery/LotteryAnimation";

        #endregion---------------------------------

        #region 通过枚举获取音乐路径---------------
        
        public string MusicSave_FilePath(MusicDownTypeEnum downType)
        {
            string savePath = "";

            switch (downType)
            {
                case MusicDownTypeEnum.MusicDown_Bg:
                    savePath = GameConfig.Instance.MusicBgLocal_Path;
                    break;
                case MusicDownTypeEnum.MusicDown_Sound:
                    savePath = GameConfig.Instance.MusicSoundLocal_Path;
                    break;
                case MusicDownTypeEnum.MusicDown_VoiceMandarin:
                    savePath = GameConfig.Instance.MusicMandarinLocal_Path;
                    break;
                case MusicDownTypeEnum.MusicDown_VoiceHeNan:
                    savePath = GameConfig.Instance.MusicHeNanLocal_Path;
                    break;
            }

            return savePath;
        }

        #endregion---------------------------------

        public static AnimationCurveAsset LoadAnimationCurve(string path)
        {
            AnimationCurveAsset asset = Resources.Load(path) as AnimationCurveAsset;
            return asset;
        }
    }
}
