/**
* @Author Xin.Wang
*
*
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace projectQ
{
    public class UIMahjongCloseState : UIViewBase
    {
        public override void Init()
        {
            EventDispatcher.AddEvent(GEnum.NamedEvent.EMjControlCloseStateUpdate, UpdateCloseState);
            IniState();
        }

        public override void OnHide()
        {

        }

        public override void OnShow()
        {

        }

        protected override void OnClose()
        {
            EventDispatcher.RemoveEvent(GEnum.NamedEvent.EMjControlCloseStateUpdate, UpdateCloseState);
        }


        public override void OnPushData(object[] data)
        {
            shouldCloseAuto = (bool)data[0];
            this.isClose = (bool)data[1];
            this.showTime = (float)data[2];

        }

        private bool shouldCloseAuto = false;
        private float showTime = 0f;
        //private int timeUpdate = -1;
        private bool isClose = false;

        private long[] userID = null;

        private float timerValue = 0f;
        private bool isTimer = false;

        public UILabel label_Title = null;
        public UILabel label_Tips = null;
        public UILabel label_Time = null;

        public UIMahjongCloseStatePlayer[] playerInfo = null;

        public void SetTipAndTitle(string tips, string title)
        {
            label_Title.text = title;
            label_Tips.text = tips;
        }

        public void IniState()
        {
            int selfSeatID = MjDataManager.Instance.MjData.curUserData.selfSeatID;
            this.userID = MjDataManager.Instance.GetAllPlayerIDByDeskID(MjDataManager.Instance.MjData.curUserData.selfDeskID);

            for (int i = 0; i < userID.Length; i++)
            {
                PlayerDataModel item = MemoryData.PlayerData.get(userID[i]);
                if (item.playerDataMj != null)
                {
                    int curSeatID = item.playerDataMj.seatID;
                    int state = MjDataManager.Instance.GetCloseState(curSeatID);
                    string headUrl = item.PlayerDataBase.HeadURL;

                    playerInfo[curSeatID - 1].IniState(state, headUrl);
                }
            }
            if(label_Time!=null)
            {
                //timerValue = 60;  //服务器暂时没支持，暂定60s
                timerValue = showTime;
            }
            if(timerValue>0)
            {
                isTimer = true;
            }
            if (shouldCloseAuto)
            {
                StartCoroutine(IEAutoClose());
            }
        }
        void Update()
        {
            if (isTimer)
            {
                timerValue -= Time.deltaTime;
                string tStr = string.Format("{0:d2}", (int)timerValue);
                label_Time.text = string.Format("{0}", "（" + tStr + "S）");
                if (timerValue <= 0)
                {
                    isTimer = false;
                }
            }
        }
        private void UpdateCloseState(object[] vars)
        {
            bool state = (bool)vars[0];
            if (state)
            {
                int seatID = (int)vars[1];
                UpdateState(seatID);
                float timer = (float)vars[2];
                UpdateTimerValue(timer);
            }
            else
            {
                this.isClose = (bool)vars[1];
                this.showTime = (float)vars[2];
                StartCoroutine(IEAutoClose());
            }

        }

        private void UpdateState(int seatID)
        {
            int state = MjDataManager.Instance.GetCloseState(seatID);
            playerInfo[seatID - 1].UpdateState(state);
        }

        private void UpdateTimerValue(float timer)
        {
            if(label_Time!=null)
            {
                timerValue = timer;
            }
        }

        private IEnumerator IEAutoClose()
        {
            ClearCloseMessage();
            yield return new WaitForSeconds(showTime);

            CloseFire();
            Close();
        }


        private void ClearCloseMessage()
        {
            MjDataManager.Instance.SetCloseEnd();
        }

        private void CloseFire()
        {
            EventDispatcher.FireEvent(GEnum.NamedEvent.EMjControlCloseStateEnd, isClose);
        }

    }

}




