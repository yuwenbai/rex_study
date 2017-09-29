using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace projectQ
{

    public class SystemTipPop : WindowUIBase  {

        #region component handle
        //内容文本
        public UISprite Bg;
        public UILabel labelContent;
        public TweenAlpha tweenAlpha;
        #endregion
        private static int MaxDepth;
        private int minDepth = 200;
        private int depthInterval = 3;
        private int currDepth;

        bool isShowTween;

        private void SetDepth()
        {
            if (MaxDepth <= minDepth)
                MaxDepth = minDepth;

            MaxDepth += depthInterval;



            Bg.depth = MaxDepth;
            labelContent.depth = MaxDepth + 1;
        }
        private void DelDepth()
        {
            if(Bg.depth == MaxDepth)
            {
                MaxDepth = minDepth;
            }
        }


        public void OnTweenFinished()
        {
            if(isShowTween)
            {
                isShowTween = false;
                tweenAlpha.delay = this.SelfData.StayTime;
                tweenAlpha.duration = 0.2f;
                tweenAlpha.PlayReverse();
            }
            else
            {
                this.Close();
            }
        }

        #region overide process fun

        //窗体初始化
        public override void Init(Action<int> clickClose)
        {
            EventDelegate.Add(tweenAlpha.onFinished, OnTweenFinished);
            base.Init(clickClose);
            SetDepth();
        }

        public override void SetData(WindowUIData windowData)
        {
            this.labelContent.text = windowData.contentText;
            base.SetData(windowData);
        }

        public override void Open(GameObject parent = null)
        {
            NGUITools.SetActive(selfObj, true);
            isShowTween = true;
            tweenAlpha.delay = 0;
            tweenAlpha.duration = 0.8f;
            tweenAlpha.PlayForward();
            base.Open(parent);
        }

        public override void Close()
        {
            DelDepth();
            EventDelegate.Remove(tweenAlpha.onFinished, OnTweenFinished);
            base.Close();
            GameObject.DestroyImmediate(selfObj);
        }

        #endregion


        #region overide event func

        public override void onInit()
        {
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
        #endregion


    }

}
