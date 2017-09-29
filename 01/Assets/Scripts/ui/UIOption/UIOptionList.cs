/**
 * @Author lyb
 *
 *
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace projectQ
{
    public class UIOptionList : MonoBehaviour
    {
        public delegate void OptionListClickDelegate(int optionId);
        public OptionListClickDelegate OnClickCallBack;

        /// <summary>
        /// 当前设置按钮是否是打开的
        /// </summary>
        public bool IsOption;
        /// <summary>
        /// 设置ID
        /// </summary>
        public int OptionId;
        /// <summary>
        /// 设置按钮背景
        /// </summary>
        public GameObject OptionBtnSprBg;
        /// <summary>
        /// 设置按钮滑动小球
        /// </summary>
        public GameObject OptionBallSprBg;

        /// <summary>
        /// 滑动速度
        /// </summary>
        private float RemoveSpeed = 0.2f;

        private Vector3 ToggleOnTo = new Vector3(-43.0f, 0.0f, 0.0f);
        private Vector3 ToggleOffTo = new Vector3(43.0f, 0.0f, 0.0f);

        void Start()
        {
            //IsOption = true;
        }

        /// <summary>
        /// 设置按钮初始化
        /// </summary>
        public void OptionList_Init()
        {
            int optionValue = -1;

            switch (OptionId)
            {
                case 1:
                    optionValue = PlayerPrefsTools.GetInt("OPTION_MUSIC");
                    break;
                case 2:                    
                    optionValue = PlayerPrefsTools.GetInt("OPTION_EFFECT");
                    break;
                case 3:
                    optionValue = PlayerPrefsTools.GetInt("OPTION_SINGLE");
                    break;
                case 4:
                    optionValue = PlayerPrefsTools.GetInt("OPTION_LANGUAGE");
                    break;
            }

			if (optionValue == 2) {
				// 音效 2 开启
				OptionBtnSprBg.GetComponent<UISprite> ().spriteName = "option_Button_03";
				OptionBallSprBg.transform.localPosition = ToggleOnTo;

				IsOption = true;
			} else if (optionValue == 1) {
				// 音效 1 关闭
				OptionBtnSprBg.GetComponent<UISprite> ().spriteName = "option_Button_04";
				OptionBallSprBg.transform.localPosition = ToggleOffTo;

				IsOption = false;
			} else {
				QLoger.ERROR ("设置的默认值出错 " + OptionId);
			
			}
        }

        /// <summary>
        /// 设置按钮数值
        /// </summary>
        public void OptionList_SetValue()
        {
            if (!IsOption)
            {
                //QLoger.LOG(" 当前按钮是关闭的，打开 圆球推到左侧 ");

                TweenPosition tween = TweenPosition.Begin(OptionBallSprBg, RemoveSpeed, ToggleOnTo);
                tween.PlayForward();

                OptionBtnSprBg.GetComponent<UISprite>().spriteName = "option_Button_03";
                //OptionBallSprBg.GetComponent<UISprite>().spriteName = "option_Button_01";

                IsOption = true;
            }
            else
            {
                //QLoger.LOG(" 当前按钮是开启的，关闭 圆球推到右侧 ");

                TweenPosition tween = TweenPosition.Begin(OptionBallSprBg, RemoveSpeed, ToggleOffTo);
                tween.PlayForward();

                OptionBtnSprBg.GetComponent<UISprite>().spriteName = "option_Button_04";
                //OptionBallSprBg.GetComponent<UISprite>().spriteName = "option_Button_02";

                IsOption = false;
            }

            OptionSetValue();
        }

        /// <summary>
        /// 把设置数据保存在本地
        /// </summary>
        void OptionSetValue()
        {
            switch (OptionId)
            {
                case 1:
                    // 2 开启 1 关闭
                    PlayerPrefsTools.SetInt("OPTION_MUSIC", IsOption == true ? 2 : 1);
                    MusicCtrl.Instance.Music_BackVolumeChange(IsOption);
                    break;
                case 2:
                    PlayerPrefsTools.SetInt("OPTION_EFFECT", IsOption == true ? 2 : 1);
                    break;
                case 3:
                    PlayerPrefsTools.SetInt("OPTION_SINGLE", IsOption == true ? 2 : 1);
                    break;
                case 4:
                    PlayerPrefsTools.SetInt("OPTION_LANGUAGE", IsOption == true ? 2 : 1);
                    break;
            }
        }

        void OnClick()
        {
            if (OnClickCallBack != null)
            {
                OnClickCallBack(OptionId);
            }
        }
    }
}
