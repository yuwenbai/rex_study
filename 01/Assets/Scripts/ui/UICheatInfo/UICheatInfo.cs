/**
* @Author YQC
*
*
*/
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace projectQ
{
    public class UICheatInfo : UIViewBase
    {
        public UICheatInfoModel Model
        {
            get { return _model as UICheatInfoModel; }
        }

        /// <summary>
        /// 作弊类型
        /// </summary>
        public enum EnumCheatType
        {
            /// <summary>
            /// 严重的
            /// </summary>
            Serious,

            /// <summary>
            /// 警告
            /// </summary>
            Warning,

            /// <summary>
            /// 正常
            /// </summary>
            Normal
        }

        public GameObject Root;
        public UISprite SpriteBg;
        public UIGrid Grid;
        public UILabel LabelIpInfo;
        public UILabel LabelLocationInfo;
        public UILabel LabelLocationEmptyInfo;
        
        public TweenPosition TweenPos;

        public EnumCheatType CurrState = EnumCheatType.Normal;

        public void RefreshUI()
        {
            Model.RefreshData();
            this.CurrState = GetState();
            RefreshBg(this.CurrState);
            RefreshLabel();
        }

        private EnumCheatType GetState()
        {
            if (Model.IpCheate && Model.LocationCheate)
                return EnumCheatType.Serious;
            if (Model.IpCheate || Model.LocationCheate)
                return EnumCheatType.Warning;
            return EnumCheatType.Normal;
        }

        /// <summary>
        /// 设置背景图
        /// </summary>
        private void RefreshBg(EnumCheatType type)
        {
            switch(type)
            {
                case EnumCheatType.Serious:
                    this.SpriteBg.spriteName = "prepare_bj_hongbj";
                    break;
                case EnumCheatType.Warning:
                    this.SpriteBg.spriteName = "prepare_bj_huangbj";
                    break;
                case EnumCheatType.Normal:
                    this.SpriteBg.spriteName = "prepare_bj_lvbj";
                    break;
            }
            
        }

        /// <summary>
        /// 设置数据
        /// </summary>
        private void RefreshLabel()
        {
            this.LabelIpInfo.text = Model.IpText.ToString();

            if (Model.LocationText.Length == 0)
            {
                UITools.SetActive(this.LabelLocationInfo, false);
            }
            else
            {
                UITools.SetActive(this.LabelLocationInfo, true);
                this.LabelLocationInfo.text = Model.LocationText.ToString();
            }

            if (Model.LocationEmptyText.Length == 0)
            {
                UITools.SetActive(this.LabelLocationEmptyInfo, false);
            }
            else
            {
                UITools.SetActive(this.LabelLocationEmptyInfo, true);
                this.LabelLocationEmptyInfo.text = Model.LocationEmptyText.ToString();
            }
            this.Grid.Reposition();
            StartCoroutine(GridReposition());
        }
        private IEnumerator GridReposition()
        {
            yield return null;
            this.Grid.Reposition();
            yield return null;
            this.Grid.Reposition();
        }

        private Coroutine coroutine;
        public void PlayAnimation(bool isForward,float def=0f)
        {
            if(def > 0f)
            {
                if (coroutine != null)
                    StopCoroutine(coroutine);

                coroutine = StartCoroutine(AnimationCorutine(def));
            }
            if(isForward)
            {
                Root.SetActive(true);
                EventDelegate.Remove(TweenPos.onFinished, OnAnimationFinished);
                TweenPos.PlayForward();
            }
            else
            {
                EventDelegate.Add(TweenPos.onFinished, OnAnimationFinished);
                TweenPos.PlayReverse();
            }
        }

        private void OnAnimationFinished()
        {
            Root.SetActive(false);
            EventDelegate.Remove(TweenPos.onFinished, OnAnimationFinished);
        }
        private IEnumerator AnimationCorutine(float time)
        {
            yield return new WaitForSeconds(time);
            PlayAnimation(false);
        }

        public void SetShow(bool isShow)
        {
            if (coroutine != null)
                StopCoroutine(coroutine);
            this.SpriteBg.cachedTransform.localPosition = isShow ? TweenPos.to : TweenPos.from;
        }


        #region Event
        private void OnBgClick(GameObject go)
        {
            this.PlayAnimation(false);
        }
        #endregion
        #region override
        public override void Init()
        {
            UIEventListener.Get(SpriteBg.cachedGameObject).onClick = OnBgClick;
        }

        public override void OnHide()
        {
        }

        public override void OnShow()
        {
            this.RefreshUI();
        }
        #endregion
    }
}