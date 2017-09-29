/****************************************************
*
*  游戏内事件枚举罗列
*  作者: lyb
*  日期：2017年7月25日
*
*****************************************************/

namespace projectQ
{
    public partial class EventKey
    {
        //=====================资源加载相关

        /// <summary>
        /// 资源加载重新开始
        /// </summary>
        public const string Bundle_Restart_Event = "Bundle_Restart_Event";
        /// <summary>
        /// 资源加载开始
        /// </summary>
        public const string Bundle_Load_Event = "Bundle_Load_Event";
        /// <summary>
        /// 资源加载通知当前加载的资源名字
        /// </summary>
        public const string Bundle_LoadName_Event = "Bundle_LoadName_Event";
        /// <summary>
        /// 发送给loading界面显示的文字
        /// </summary>
        public const string Bundle_LoadingNameShow_Event = "Bundle_LoadingNameShow_Event";
        /// <summary>
        /// 资源下载完毕
        /// </summary>
        public const string Bundle_ResourceDownFinish_Event = "Bundle_ResourceDownFinish_Event";
        /// <summary>
        /// 资源解密，解压完毕
        /// </summary>
        public const string Bundle_UnEncryZipFinish_Event = "Bundle_UnEncryZipFinish_Event";
        /// <summary>
        /// 资源加载的loading条进度显示
        /// </summary>
        public const string Bundle_SliderValue_Event = "Bundle_SliderValue_Event";
        /// <summary>
        /// 资源加载的loading条进度平滑显示
        /// </summary>
        public const string Bundle_SliderValueShow_Event = "Bundle_SliderValueShow_Event";
        /// <summary>
        /// 发送给loading界面显示的进度
        /// </summary>
        public const string Bundle_LoadingSliderValueShow_Event = "Bundle_LoadingSliderValueShow_Event";
        /// <summary>
        /// 本地头像加载完成
        /// </summary>
        public const string Bundle_HeadTextureLoadFinish_Event = "Bundle_HeadTextureLoadFinish_Event";
        /// <summary>
        /// 音乐音效下载开始
        /// </summary>
        public const string Bundle_MusicDownBegin_Event = "Bundle_MusicDownBegin_Event";
        /// <summary>
        /// 音乐音效下载完毕
        /// </summary>
        public const string Bundle_MusicDownFinish_Event = "Bundle_MusicDownFinish_Event";
        /// <summary>
        /// 初始化完的音乐加载到内存
        /// </summary>
        public const string Bundle_MusicDownAddMemory_Event = "Bundle_MusicDownAddMemory_Event";

        //=====================资源加载相关
    }
}