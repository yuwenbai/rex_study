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

    public class UIMahjongGameStart : UIViewBase
    {
        public override void Init()
        {

        }

        public override void OnHide()
        {

        }

        public override void OnShow()
        {
            PlayAndShowAnim();
        }

        public override void OnPushData(object[] data)
        {
            if (data.Length > 0)
            {
                this.userID = (long[])data[0];
                this.closeTime = (float)data[1];
            }

        }


        public UITexture[] headArray = null;
        public UILabel[] nickNameArray = null;


        private long[] userID = null;
        private float closeTime = 0f;


        public void PlayAndShowAnim()
        {
            //if (userID != null && userID.Length > 0)
            //{
            //    int selfSeatID = MjDataManager.Instance.MjData.curUserData.selfSeatID;

            //    PlayerDataModel[] model = new PlayerDataModel[4];
            //    for (int i = 0; i < userID.Length; i++)
            //    {
            //        PlayerDataModel item = MemoryData.PlayerData.get(userID[i]);
            //        if (item.playerDataMj != null)
            //        {
            //            int uiSeat = CommonTools.GetMJUIPosByServerPos(item.playerDataMj.seatID, selfSeatID);
            //            nickNameArray[uiSeat].text = item.PlayerDataBase.Name;

            //            DebugPro.Log(DebugPro.EnumLog.HeadUrl, "UIMahjongGameStart__PlayAndShowAnim", item.PlayerDataBase.HeadURL);
            //            DownHeadTexture.Instance.WeChat_HeadTextureGet(item.PlayerDataBase.HeadURL, null, headArray[uiSeat]);
            //        }
            //    }
            //}
            //play

        }

        public void SetClose()
        {
            if(this.gameObject.activeSelf)
                StartCoroutine(IECloseSelf());
        }

        private IEnumerator IECloseSelf()
        {
            yield return new WaitForSeconds(closeTime);
            EventDispatcher.FireSysEvent(GEnum.NamedEvent.SysData_CheatInfo_Open, true);
            this.Close();
        }
    }

}
