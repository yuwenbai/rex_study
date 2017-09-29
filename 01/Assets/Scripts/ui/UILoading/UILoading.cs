

using System;
/**
* @Author YQC
*
*
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace projectQ
{
    public class UILoading : UIViewBase
    {
        public enum EnumLoadingType
        {
            Show,Empty
        }


        [Tooltip("根 开启关闭的都是此")]
        public GameObject Root;

        [Tooltip("Loading中的Label")]
        public UILabel LabelContent;

        //[Tooltip("Loading背景")]
        //public UISprite SpriteMask;
        //private float MaskDefaultAlpha = 1f;
        public GameObject GoShow;
        public GameObject GoEmpty;

        [Tooltip("延迟关闭时间")]
        public float DelayHideTime = 0.5f;

        [Tooltip("透明Loading最长时间")]
        public float EmptyTime = 3f;

        private bool isShow = false;

        private EnumLoadingType currLoadingType = EnumLoadingType.Show;
        private float emptyStartTime = 0f;

        public void ShowLoading()
        {
            if(this.currLoadingType == EnumLoadingType.Empty)
            {
                GameDelegateCache.Instance.InvokeMethodEvent -= UIOnUpdata;
                GameDelegateCache.Instance.InvokeMethodEvent += UIOnUpdata;
            }

            isShow = true;
            Root.SetActive(true);

            GoShow.SetActive(currLoadingType == EnumLoadingType.Show);
            GoEmpty.SetActive(currLoadingType == EnumLoadingType.Empty);

            MemoryData.GameStateData.SmallLoadingActive = true;
        }
        public void ShowLoadingEmpty()
        {
            emptyStartTime = Time.realtimeSinceStartup;
            this.currLoadingType = EnumLoadingType.Empty;
           
            ShowLoading();
        }
        public void HideLoading()
        {
            this.SetLabelContent(string.Empty);
            isShow = false;
            Root.SetActive(false);
            this.currLoadingType = EnumLoadingType.Show;
            MemoryData.GameStateData.SmallLoadingActive = false;
        }

        public void DelayHideLoading()
        {
            isShow = false;
            StartCoroutine(CoroutineHide());
        }

        private IEnumerator CoroutineHide()
        {
            yield return new WaitForSeconds(DelayHideTime);
            if(!isShow)
                HideLoading();
        }

        public void SetLabelContent(string content)
        {
            this.LabelContent.text = content;
        }

        public void UIOnUpdata()
        {
            
            if(isShow && currLoadingType == EnumLoadingType.Empty)
            {
                if(Time.realtimeSinceStartup - emptyStartTime > EmptyTime)
                {
                    currLoadingType = EnumLoadingType.Show;
                    GameDelegateCache.Instance.InvokeMethodEvent -= UIOnUpdata;

                    this.ShowLoading();
                }
            }
        }

        #region override

        public override void Init()
        {
            HideLoading();
        }

        public override void OnHide()
        {
        }

        public override void OnShow()
        {
        }

        #endregion
    }
}
