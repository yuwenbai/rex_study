/**
* @Author Hailong.Zhang
*
*
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using projectQ.animation;

namespace projectQ
{
    public class UIMahjongDismissTips : UIViewBase
    {
        #region 重写基类方法
        public override void Init()
        {
            EventDispatcher.AddEvent(GEnum.NamedEvent.EMjControlDismissTipsUpdate, UpdateDismissTips);
            EventDispatcher.AddEvent(GEnum.NamedEvent.EMjControlDismissTipsClose, CloseDismissTips);

            InitData();
        }
        public override void OnHide()
        {
            
        }
        public override void OnShow()
        {
            ShowInfo();
        }
        protected override void OnClose()
        {
            EventDispatcher.RemoveEvent(GEnum.NamedEvent.EMjControlDismissTipsUpdate, UpdateDismissTips);
            EventDispatcher.RemoveEvent(GEnum.NamedEvent.EMjControlDismissTipsClose, CloseDismissTips);
        }
        public override void OnPushData(object[] data)
        {
            strSeat = (string)data[0];
            closeTime = (int)data[1];
            timeType = (int)data[2];
        }
        #endregion

        [HideInInspector]
        public GameObject selfObj = null;

        private string strSeat = "";

        private int closeTime = -1;

        private int timeType = -1;

        private float timerValue = 0f;

        private bool isTimer = false;

        public UILabel Label_Tips = null;

        public UILabel Label_ConfirmTime = null;

        public UILabel Label_CancellTime = null;

        public GameObject BtnConfirm = null;

        public GameObject BtnCancell = null;

        public AnimationItem animItem = null;
        

        private void InitData()
        {
            if(selfObj==null)
            {
                selfObj = this.gameObject;
            }
            NGUITools.SetActive(selfObj, true);
            if (Label_Tips != null)
            {
                NGUITools.SetActive(Label_Tips.gameObject, strSeat != null);
                Label_Tips.text = string.Format("{0}", strSeat);
            }
            if (timeType == 1 && Label_ConfirmTime != null)
            {
                NGUITools.SetActive(Label_ConfirmTime.gameObject, closeTime != null);
                timerValue = closeTime;
            }
            else if (timeType == 0 && Label_CancellTime != null)
            {
                NGUITools.SetActive(Label_CancellTime.gameObject, closeTime != null);
                timerValue = closeTime;
            }
            
            InitBtnEvent();
        }

        private void UpdateDismissTips(object[] vars)
        {
            strSeat = (string)vars[0];
            closeTime = (int)vars[1];
            timeType = (int)vars[2];
            UpdateDismissTipsData(strSeat, closeTime, timeType);
            isTimer = true;
        }

        private void UpdateDismissTipsData(string strSeat,int closeTime,int timeType)
        {
            if (Label_Tips != null)
            {
                Label_Tips.text = string.Format("{0}", strSeat);
            }
            if (timeType == 1 && Label_ConfirmTime != null)
            {
                timerValue = closeTime;
            }
            else if (timeType == 0 && Label_CancellTime != null)
            {
                timerValue = closeTime;
            }
        }
        private void CloseDismissTips(object[] vars)
        {
            CloseSelf();
        }
        private void ShowInfo()
        {
            OnTimeBegin();
        }
        void Update()
        {
            if (isTimer)
            {
                timerValue -= Time.deltaTime;
                string tStr = string.Format("{0:d2}", (int)timerValue);
                if (timeType == 1)
                {
                    Label_ConfirmTime.text = string.Format("{0}", "（" + tStr + "S）");
                }
                else
                {
                    Label_CancellTime.text = string.Format("{0}", "（" + tStr + "S）");
                }
                if (timerValue <= 0)
                {
                    isTimer = false;
                    OnClickClose(timeType);
                }
            }
        }

        private void OnTimeBegin()
        {
            _R.ui.PlayAnimation(animItem, true, (isok) =>
                {
                    if(isok)
                    {
                        if (timerValue > 0)
                        {
                            isTimer = true;
                        }
                    }
                });
            
        }
        private void InitBtnEvent()
        {
            UIEventListener.Get(BtnConfirm).onClick = delegate(GameObject go)
            {
                OnBtnClick(0);
            };
            UIEventListener.Get(BtnCancell).onClick = delegate(GameObject go)
            {
                OnBtnClick(1);
            };
        }

        
        private void OnBtnClick(int index)
        {
            _R.ui.PlayAnimation(animItem, false, (isOK) =>
                {
                    if (isOK)
                    {
                        OnClickClose(index);
                        CloseSelf();
                    }
                });
        }
        private void OnClickClose(int index)
        {
            EventDispatcher.FireEvent(GEnum.NamedEvent.EMjControlSendCloseAnwser, index);
        }

        private void CloseSelf()
        {
            base.Close();
        }
    }
}