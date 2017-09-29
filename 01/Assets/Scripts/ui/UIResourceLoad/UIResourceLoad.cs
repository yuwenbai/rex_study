/**
* @Author lyb
*  Loading 进度条界面
*
*/

using System.Collections;
using UnityEngine;

namespace projectQ
{
    public class UIResourceLoad : UIViewBase
    {
        /// <summary>
        /// 背景图
        /// </summary>
        public UITexture Tex_Bg;
        /// <summary>
        /// 如果进入麻将馆则把该obj隐藏
        /// </summary>
        public GameObject LoadObj01;
        /// <summary>
        /// 如果进入麻将馆则把该obj显示
        /// </summary>
        public GameObject LoadObj02;
        /// <summary>
        /// Tips
        /// </summary>
        public GameObject TipsObj;
        /// <summary>
        /// 进度条
        /// </summary>
        public UISlider SliderObj;
        /// <summary>
        /// 抵制不良游戏，拒绝盗
        /// </summary>
        public UILabel NoticeLab;
        /// <summary>
        /// 京公网安备1102379781297812号
        /// </summary>
        public UILabel RecordLab;
        /// <summary>
        /// 进度条百分比
        /// </summary>
        public UILabel SliderValueLab;
        /// <summary>
        /// 资源名字显示
        /// </summary>
        public UILabel NameShowLab;
        /// <summary>
        /// 进度条特效
        /// </summary>
        public GameObject SliderEffect;

        /// <summary>
        /// 打开面板传递过来的参数
        /// </summary>
        private string pushStr = "";
        /// <summary>
        /// 根据不同的情景选择不同的背景图
        /// 0 登录界面 1 进麻将馆界面
        /// </summary>
        private int pushValue = 0;

        private Coroutine changeValue;

        /*
        void OnEnable(){}
        void OnDisable(){}
        */

        #region override-------------------------------------------------

        /// <summary>
        /// 打开面板，传递参数，先调用该方法
        /// </summary>
        public override void OnPushData(object[] data)
        {
            if (data != null && data.Length > 0)
            {
                if (data[0].GetType() == typeof(string))
                {
                    pushStr = data[0] as string;
                }

                if (data.Length > 1)
                {
                    pushValue = (int)data[1];
                }
            }
        }

        public override void Init()
        {
#if __DEBUG
            //SliderValueLab.gameObject.SetActive(true);
#else
            //SliderValueLab.gameObject.SetActive(false);
#endif
            Load_Init();
        }

        public override void OnHide() { }

        public override void OnShow()
        {
            MemoryData.GameStateData.BigLoadingActive = true;
        }

        void OnEnable()
        {
            EventDispatcher.AddEvent(EventKey.Bundle_LoadingNameShow_Event, C2CResourceNameShow);
            EventDispatcher.AddEvent(EventKey.Bundle_LoadingSliderValueShow_Event, C2CResourceSliderValueShow);
        }

        void OnDisable()
        {
            MemoryData.GameStateData.BigLoadingActive = false;
            EventDispatcher.RemoveEvent(EventKey.Bundle_LoadingNameShow_Event, C2CResourceNameShow);
            EventDispatcher.RemoveEvent(EventKey.Bundle_LoadingSliderValueShow_Event, C2CResourceSliderValueShow);
        }

        #endregion-------------------------------------------------------

        #region 初始化方法-----------------------------------------------

        void Load_Init()
        {
            Init_Text();

            bool isBol1 = pushValue == 0 ? true : false;
            LoadObj01.SetActive(isBol1);

            bool isBol2 = pushValue == 0 ? false : true;
            LoadObj02.SetActive(isBol2);

            string bgTexPath = pushValue == 0 ? "Texture_Loading_01" : "Texture_Loading_02";
            Texture bgTex = ResourcesDataLoader.Load<Texture>("Texture/Tex_Loading/" + bgTexPath);
            Tex_Bg.mainTexture = bgTex;

            if (SliderObj.value <= 0)
            {
                SliderEffect.SetActive(false);
            }
        }

        /// <summary>
        /// 文本初始化
        /// </summary>
        void Init_Text()
        {
            string noticeStr = MemoryData.XmlData.XmlBuildDataText_Get(GTextsEnum.G_Text_102);
            if (noticeStr != "")
            {
                NoticeLab.text = noticeStr;
            }

            string recordStr = MemoryData.XmlData.XmlBuildDataText_Get(GTextsEnum.G_Text_101);
            if (recordStr != "")
            {
                RecordLab.text = recordStr.Replace("\n", "，");
            }
        }

        #endregion-------------------------------------------------------

        #region Common --------------------------------------------------

        /// <summary>
        /// 当前下载的资源进度显示
        /// </summary>
        public void C2CResourceSliderValueShow(object[] data)
        {
            float sValue = (float)data[0];

            OnChangeValue(1.0f, sValue * 100.0f);
        }

        /// <summary>
        /// 当前资源正在下载的资源名字
        /// </summary>
        public void C2CResourceNameShow(object[] data)
        {
            NameShowLab.text = (string)data[0];
        }

        #endregion-------------------------------------------------------

        #region 修改界面load条百分比-------------------------------------

        /// <summary>
        /// 修改
        /// </summary>
        public void OnChangeValue(float time, float taget)
        {
            if (float.IsNaN(taget))
            {
                taget = 0f;
            }

            //如果传入的值小于等于当前的值 则抛弃掉
            if (SliderObj.value * 100 > taget)
            {
                return;
            }

            SliderEffect.SetActive(true);

            if (changeValue != null)
            {
                StopCoroutine(changeValue);
            }

            changeValue = StartCoroutine(ChangeValue(time, taget / 100.0f));
        }

        /// <summary>
        /// 改变百分比
        /// </summary>
        IEnumerator ChangeValue(float time, float taget)
        {
            float tempTime = 0f;
            float oldValue = SliderObj.value;

            while (time > tempTime && SliderObj.value <= 1f)
            {
                tempTime += Time.deltaTime;

                SliderObj.value = Mathf.Lerp(oldValue, taget, tempTime / time);
                yield return null;
            }

            SliderObj.value = taget;

            if (taget == 1)
            {
                this.Close();
            }

            if (changeValue != null)
            {
                StopCoroutine(changeValue);
            }
            yield break;
        }

        #endregion------------------------------------------------------
    }
}