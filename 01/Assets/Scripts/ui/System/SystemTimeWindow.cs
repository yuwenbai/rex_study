using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using projectQ.animation;

namespace projectQ
{
    public class SystemTimeWindow : WindowUIBase
    {
        /// <summary>
        /// 标题
        /// </summary>
        public UILabel LabelTitle;
        /// <summary>
        /// 内容文本
        /// </summary>
        public UILabel LabelContent;
        /// <summary>
        /// 确定按钮
        /// </summary>
        public GameObject BtnOk;
        /// <summary>
        /// 确定按钮文字
        /// </summary>
        public UILabel LabelBtnOk;
        /// <summary>
        /// 取消按钮
        /// </summary>
        public GameObject BtnCancel;
        /// <summary>
        /// 取消按钮文字
        /// </summary>
        public UILabel LabelBtnCancel;
        /// <summary>
        /// 动画
        /// </summary>
        public AnimationItem AnimItem;

        //按钮点击事件
        private System.Action<int> onbtnClick = null;

        /// <summary>
        /// 倒计时时间
        /// </summary>
        private float timeValue = -1;
        /// <summary>
        /// 倒计时开始
        /// </summary>
        private bool isTime = false;

        private WindowUIData windowData;

        #region overide process fun -------------------------------------------

        //窗体初始化
        public override void Init(Action<int> clickClose)
        {
            base.Init(clickClose);
        }

        public override void SetData(WindowUIData wData)
        {
            windowData = wData;

            LabelTitle.text = windowData.titleText;
            NGUITools.SetActive(LabelTitle.gameObject, windowData.titleText != null);

            LabelContent.text = windowData.contentText;
            NGUITools.SetActive(LabelContent.gameObject, windowData.contentText != null);

            if (windowData.buttonTexts != null)
            {
                LabelBtnCancel.text = windowData.buttonTexts[0];
                LabelBtnOk.text = windowData.buttonTexts[1];
            }

            onbtnClick = windowData.btnCall;

            this.timeValue = windowData.timeValue;

            base.SetData(windowData);
        }

        public override void Open(GameObject parent = null)
        {
            _R.ui.PlayAnimation(AnimItem, true, (isOk) =>
            {
                if(isOk)
                {
                    NGUITools.SetActive(selfObj, true);
                    base.Open(parent);

                    OnTimeBegin();
                }
            });
        }

        public override void Close()
        {
            base.Close();
        }

        #endregion -----------------------------------------------------------

        #region 实现方法 -----------------------------------------------------

        void Update()
        {
            if (isTime)
            {
                timeValue -= Time.deltaTime;

                string tStr = string.Format("{0:d2}", (int)timeValue);
                if (windowData.timeLeftShow)
                {
                    LabelBtnCancel.text = windowData.buttonTexts[0] + "(" + tStr + ")";
                }
                else
                {
                    LabelBtnOk.text = windowData.buttonTexts[1] + "(" + tStr + ")";
                }

                if (timeValue <= 0)
                {
                    isTime = false;

                    int returnIndex = windowData.timeLeftShow ? 1 : 0;
                    OnBtnClick(returnIndex);
                }
            }
        }

        /// <summary>
        /// 点击按钮事件调用
        /// </summary>
        private void OnBtnClick(int index)
        {
            _R.ui.PlayAnimation(AnimItem, false, (isOk) =>
            {
                if(isOk)
                {
                    if (onbtnClick != null)
                    {
                        onbtnClick(index);
                    }
                    this.Close();
                }
            });
        }

        /// <summary>
        /// 倒计时开始
        /// </summary>
        private void OnTimeBegin()
        {
            if (timeValue > 0)
            {
                isTime = true;
                //QLoger.LOG(" 倒计时开始 ++++++++++++++++++++++++ ");
            }
        }

        #endregion -----------------------------------------------------------

        #region overide event func -------------------------------------------

        public override void onInit()
        {
            UIEventListener.Get(BtnOk).onClick = delegate (GameObject go)
            {
                OnBtnClick(0);
            };

            UIEventListener.Get(BtnCancel).onClick = delegate (GameObject go)
            {
                OnBtnClick(1);
            };

            base.CreateMask(WindowMaskEffcType.None);

            base.onInit();
        }

        public override void onSetData()
        {
            base.onSetData();
        }

        public override void onOpen()
        {
            base.onOpen();
        }

        public override void onClose()
        {
            base.onClose();
        }

        #endregion -----------------------------------------------------------
    }
}