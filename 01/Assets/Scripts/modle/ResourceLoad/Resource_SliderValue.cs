/****************************************************
*
*  资源加载的时候loading条显示进度
*  作者: lyb
*  日期：2017年7月25日
*
*****************************************************/

namespace projectQ
{
    /// <summary>
    /// 当前资源加载的步骤
    /// </summary>
    public enum BundleStepEnum
    {
        /// <summary>
        /// 下载图片
        /// </summary>
        Bundle_TextureDown,
        /// <summary>
        /// 加载图片
        /// </summary>
        Bundle_TextureLoad,
        /// <summary>
        /// 下载Xml
        /// </summary>
        Bundle_XmlDown,
        /// <summary>
        /// Xml解密
        /// </summary>
        Bundle_XmlUnEncrypt,
        /// <summary>
        /// Xml解压缩
        /// </summary>
        Bundle_XmlUnZip,
        /// <summary>
        /// 音乐下载
        /// </summary>
        Bundle_MusicDown,
    }

    public class Resource_SliderValue : BaseResourceLoad
    {
        private float Step_TextureDown = 0.1f;
        private float Step_TextureLoad = 0.1f;
        private float Step_XmlDown = 0.15f;
        private float Step_XmlUnEncrypt = 0.1f;
        private float Step_XmlUnZip = 0.1f;
        private float Step_MusicDown = 0.15f;

        void OnEnable()
        {
            EventDispatcher.AddEvent(EventKey.Bundle_SliderValue_Event, Bundle_SliderValueShow);
            EventDispatcher.AddEvent(EventKey.Bundle_SliderValueShow_Event, Bundle_SliderValueSmoothShow);
        }

        void OnDisable()
        {
            EventDispatcher.RemoveEvent(EventKey.Bundle_SliderValue_Event, Bundle_SliderValueShow);
            EventDispatcher.RemoveEvent(EventKey.Bundle_SliderValueShow_Event, Bundle_SliderValueSmoothShow);
        }

        #region 资源加载进度计算----------------------

        void Bundle_SliderValueSmoothShow(object[] data)
        {
            BundleStepEnum stepEnum = (BundleStepEnum)data[0];

            float nValue = 0;

            switch (stepEnum)
            {
                case BundleStepEnum.Bundle_TextureDown:
                    nValue = Step_TextureDown;
                    break;
                case BundleStepEnum.Bundle_TextureLoad:
                    nValue = Step_TextureLoad + Step_TextureDown;
                    break;
                case BundleStepEnum.Bundle_XmlDown:
                    nValue = Step_XmlDown + Step_TextureDown + Step_TextureLoad;
                    break;
                case BundleStepEnum.Bundle_XmlUnEncrypt:
                    nValue = Step_XmlUnEncrypt + Step_TextureDown + Step_TextureLoad + Step_XmlDown;
                    break;
                case BundleStepEnum.Bundle_XmlUnZip:
                    nValue = Step_XmlUnZip + Step_TextureDown + Step_TextureLoad + Step_XmlDown + Step_XmlUnEncrypt;
                    break;
                case BundleStepEnum.Bundle_MusicDown:
                    nValue = Step_MusicDown + Step_TextureDown + Step_TextureLoad + Step_XmlDown + Step_XmlUnEncrypt + Step_XmlUnZip;
                    break;
            }

            DebugPro.LogBundle(" 当前资源的进度显示 = " + stepEnum + " 数值= " + nValue);

            EventDispatcher.FireEvent(EventKey.Bundle_LoadingSliderValueShow_Event, nValue);
        }

        #endregion -----------------------------------

        #region 资源加载进度计算----------------------

        void Bundle_SliderValueShow(object[] data)
        {
            BundleStepEnum stepEnum = (BundleStepEnum)data[0];
            float downNum = (float)((int)data[1]);
            float allNum = (float)((int)data[2]);

            float sValue = 0;

            switch (stepEnum)
            {
                case BundleStepEnum.Bundle_TextureDown:
                    sValue = (downNum / allNum) * Step_TextureDown;
                    break;
                case BundleStepEnum.Bundle_TextureLoad:
                    sValue = ((downNum / allNum) * Step_TextureLoad) + Step_TextureDown;
                    break;
                case BundleStepEnum.Bundle_XmlDown:
                    sValue = ((downNum / allNum) * Step_XmlDown) + Step_TextureDown + Step_TextureLoad;
                    break;
                case BundleStepEnum.Bundle_XmlUnEncrypt:
                    sValue = ((downNum / allNum) * Step_XmlUnEncrypt) + Step_TextureDown + Step_TextureLoad + Step_XmlDown;
                    break;
                case BundleStepEnum.Bundle_XmlUnZip:
                    sValue = ((downNum / allNum) * Step_XmlUnZip) + Step_TextureDown + Step_TextureLoad + Step_XmlDown + Step_XmlUnEncrypt;
                    break;
                case BundleStepEnum.Bundle_MusicDown:
                    sValue = ((downNum / allNum) * Step_MusicDown) + Step_TextureDown + Step_TextureLoad + Step_XmlDown + Step_XmlUnEncrypt + Step_XmlUnZip;
                    break;
            }

            //QLoger.LOG( LogType.ELog," 当前资源的进度显示 = " + stepEnum + " 数值= " + sValue);
            //EventDispatcher.FireEvent(EventKey.Bundle_LoadingSliderValueShow_Event, nValue);
        }

        #endregion -----------------------------------
    }
}