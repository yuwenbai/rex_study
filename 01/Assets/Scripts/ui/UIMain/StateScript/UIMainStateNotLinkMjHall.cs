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
    public class UIMainStateNotLinkMjHall : UIMainStateBase
    {
        public override UIMain.EnumUIMainState SelfState
        {
            get
            {
                return UIMain.EnumUIMainState.NotLinkMjHall;
            }
        }

        public override bool SetData(GEnum.NamedEvent genum, object[] data)
        {
            switch (genum)
            {
                //推荐列表
                case GEnum.NamedEvent.SysData_MjHall_RecommendResultRsp:
                    List<MjRoom> roomList = MemoryData.MjHallData.MjHallListRecommend;
                    if (roomList != null && roomList.Count > 5)
                    {
                        roomList = roomList.GetRange(0,5);
                    }

                    this.UI.ReferralScript.SetData(roomList, null, null);
                    this.UI.ReferralScript.RefreshUI(SelfState);
                    break;
            }
            return false;
        }
        public override void RefreshUI()
        {
            UI.RoomId = UI.CurrMjHalll.RoomID;
            this.UI.PlayInfoScript.SetData(OnGoBackClick, null, UI.CurrMjHalll);
            this.UI.CenterScript.SetData(null);
            this.UI.TopScript.SetData(this.UI.CurrMjHalll);
            this.UI.DownScript.SetData(this.UI.CurrMjHalll);
            this.UI.RightButtonScript.SetData(null);
            this.UI.RefreshUI(SelfState);
        }

        private void OnGoBackClick()
        {
            ChangeState(new UIMainStateLinkMain(), this.UI);
        }
    }
}