/**
 * @Author rexzhao
 * 回放
 */

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace projectQ
{
    public class UIReplayCtrl : UIViewBase
    {
        public UIReplayCtrlModel Model
        {
            get { return _model as UIReplayCtrlModel; }
        }


        /// <summary>
        /// 停止回放并退出
        /// </summary>
        public GameObject StopReplayBtn;

        /// <summary>
        /// 重播按钮
        /// </summary>
        public GameObject ReplayButton;
        /// <summary>
        /// 暂停按钮
        /// </summary>
        public GameObject PauseReplayBtn;

        /// <summary>
        /// 播放按钮
        /// </summary>
        public GameObject PlayReplayBtn;

        /// <summary>
        /// 加速
        /// </summary>
        public GameObject ChangeSpeedBtn;

        /// <summary>
        /// 加速显示
        /// </summary>
        /// 
        public GameObject SpeedStateIcon;

        public UISprite FullScreenSprite;


        [Tooltip("打开的动画")]
        public UIPlayAnimation OpenAnimationReplay;

        #region override ------------------------------------------------------------

        private bool isCanOpen = true;
        public bool IsCanOpen
        {
            set { isCanOpen = value; }
            get { return isCanOpen; }
        }
        public bool _PanelOpen = true;
        public bool PanelOpen
        {
            set
            {
                _PanelOpen = value;
            }
            get { return _PanelOpen; }
        }

        public int aspeed = 1;
        public override void Init()
        {
            //ShowMask();
            UIEventListener.Get(StopReplayBtn).onClick = OnStopReplayBtnClick;
            UIEventListener.Get(ReplayButton).onClick = OnReplayButtonClick;
            UIEventListener.Get(PauseReplayBtn).onClick = OnPauseReplayBtnClick;
            UIEventListener.Get(PlayReplayBtn).onClick = OnPlayReplayBtnClick;
            UIEventListener.Get(ChangeSpeedBtn).onClick = OnChangeSpeedBtnClick;
            UIEventListener.Get(FullScreenSprite.gameObject).onClick = OnScreenClick;
            EventDelegate.Add(this.OpenAnimationReplay.onFinished, OnFinished);

            UpdateSpeedState();

            this.IsMask = false;
            this.PanelOpen = false;
            PauseReplayBtn.SetActive(true);
            PlayReplayBtn.SetActive(false);

        }
        public override void OnShow()
        {
            
        }

        public override void OnHide() { }

        public override void GoBack()
        {
            this.OnStopReplayBtnClick(null);
        }
        #endregion-------------------------------------------------------------------

        #region 按钮点击回调方法-----------------------------------------------------

        /// <summary>
        /// 点击关闭按钮
        /// </summary>
        void OnStopReplayBtnClick(GameObject button)
        {
            //TODO
            //Debug.Log("rextest onCloseBtnClick");
            EventDispatcher.FireSysEvent(GEnum.NamedEvent.SysUI_OpenMain);
            FakeReplayManager.Instance.ReplayState = false;
            FakeReplayManager.Instance.StopReplay(true);
        }
        /// <summary>
        /// 点击重播按钮
        /// </summary>
        void OnReplayButtonClick(GameObject button)
        {
            //TODO
            //Debug.Log("rextest OnReplayButtonClick");
            PauseReplayBtn.SetActive(true);
            PlayReplayBtn.SetActive(false);
            FakeReplayManager.Instance.ReplayAgain();
        }
        /// <summary>
        /// 点击暂停按钮
        /// </summary>
        void OnPauseReplayBtnClick(GameObject button)
        {
            //TODO
            //Debug.Log("rextest OnPauseReplayBtnClick");
            PauseReplayBtn.SetActive(false);
            PlayReplayBtn.SetActive(true);
            FakeReplayManager.Instance.PauseReplay(true);
        }
        /// <summary>
        /// 点击播放按钮
        /// </summary>
        void OnPlayReplayBtnClick(GameObject button)
        {
            //TODO
            //Debug.Log("rextest OnPauseReplayBtnClick");
            PauseReplayBtn.SetActive(true);
            PlayReplayBtn.SetActive(false);
            FakeReplayManager.Instance.PauseReplay(false);
        }
        /// <summary>
        /// 点击加速按钮
        /// </summary>
        void OnChangeSpeedBtnClick(GameObject button)
        {
            //TODO
            FakeReplayManager.Instance.ChangeSpeed();
        }
        public void UpdateSpeedState()
        {
            if (Time.timeScale > 1)
            {
                SpeedStateIcon.SetActive(true);
            }
            else
            {
                SpeedStateIcon.SetActive(false);
            }
        }
        /// <summary>
        /// 动画
        /// </summary>
        /// <param name="go"></param>
        private void OnScreenClick(GameObject go)
        {
            OnScreenpenClick(!PanelOpen);
        }
        /// <summary>
        /// UI打开关闭动画
        /// </summary>
        /// <param name="isOpen"></param>
        /// <returns></returns>
        public void OnScreenpenClick(bool isOpen)
        {
            if (isCanOpen && isOpen != PanelOpen)
            {
                OpenAnimationReplay.clipName = PanelOpen ? "ReplayCtrlFadeOut" : "ReplayCtrlFadeIn";
                //foreach (AnimationState state in OpenAnimationReplay.target.GetComponent<Animation>())
                //{
                //    state.speed = this.aspeed;
                //}
                if (PanelOpen)
                {
                    OpenAnimationReplay.Play(true,false);
                }
                else
                {
                    OpenAnimationReplay.Play(true,false);
                }
            }
        }
        public void OnReplayOver()
        {
            if (!_PanelOpen)
            {
                _PanelOpen = true;
                OpenAnimationReplay.clipName = "ReplayCtrlFadeIn";
                OpenAnimationReplay.Play(true, false);
            }
        }
        /// <summary>
        /// 动画结束
        /// </summary>
        private void OnFinished()
        {
            _PanelOpen = !_PanelOpen;
        }
        #endregion-------------------------------------------------------------------
    }
}