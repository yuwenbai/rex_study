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
    public class UIMainStateNormalMain : UIMainStateBase
    {
        public override UIMain.EnumUIMainState SelfState
        {
            get
            {
                return UIMain.EnumUIMainState.NormalMain;
            }
        }

        public override bool SetData(GEnum.NamedEvent genum, object[] data)
        {
            switch (genum)
            {
                case GEnum.NamedEvent.SysData_User_MjHallInfoUpdate:
                    if ((long)data[0] == MemoryData.UserID)
                    {
                        if(MemoryData.PlayerData.MyPlayerModel.PlayerDataBase.BindRoomID > 0)
                        {
                            ChangeState(new UIMainStateLinkedMjHall(),this.UI);
                        }
                    }
                    break;
                //推荐列表
                case GEnum.NamedEvent.SysData_MjHall_RecommendResultRsp:
                    RefreshReferral();
                    break;
            }
            return false;
        }
        public override void RefreshUI()
        {
            UI.RoomId = 0;

            if (MemoryData.MjHallData.MjHallListRecommend == null)
            {
                ModelNetWorker.Instance.FMjRoomTuijianReq(null);
            }
            else
            {
                RefreshReferral();
            }
            this.UI.PlayInfoScript.SetData(null, null, null);
            this.UI.CenterScript.SetData(null);
            this.UI.TopScript.SetData(null);
            this.UI.DownScript.SetData(null);
            this.UI.RightButtonScript.SetData(null);
            
            this.UI.RefreshUI(SelfState);
        }
        private void RefreshReferral()
        {
            List<MjRoom> roomList = MemoryData.MjHallData.MjHallListRecommend;
            if (roomList != null && roomList.Count > 5)
            {
                roomList = roomList.GetRange(0, 5);
            }
            this.UI.ReferralScript.SetData(roomList, null, null);
            this.UI.ReferralScript.RefreshUI(SelfState);
        }

    }
}