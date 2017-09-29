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
    public class UIMainStateLinkedMjHall : UIMainStateBase
    {
        public override UIMain.EnumUIMainState SelfState
        {
            get
            {
                return UIMain.EnumUIMainState.LinkedMjHall;
            }
        }

        public override bool SetData(GEnum.NamedEvent genum, object[] data)
        {
            switch (genum)
            {
                case GEnum.NamedEvent.SysData_User_MjHallInfoUpdate:
                    if((long)data[0] == MemoryData.UserID)
                    {
                        RefreshUI();
                    }
                    break;
                case GEnum.NamedEvent.SysData_MjHall_SearchResultRsp:
                    RefreshUI();
                    break;
            }
            return false;
        }

        public override void RefreshUI()
        {
            //进入麻将馆
            UI.RoomId = MemoryData.PlayerData.MyPlayerModel.PlayerDataBase.MjRoomId;

            UI.CurrMjHalll = MemoryData.MjHallData.GetMjHallById(UI.RoomId);
            this.UI.PlayInfoScript.SetData(OnGoBackClick, null , UI.CurrMjHalll);
            this.UI.CenterScript.SetData(null);
            this.UI.TopScript.SetData(UI.CurrMjHalll);
            this.UI.DownScript.SetData(UI.CurrMjHalll);
            this.UI.RightButtonScript.SetData(null);
            this.UI.RefreshUI(SelfState);
        }

        private void OnGoBackClick()
        {
            ChangeState(new UIMainStateLinkMain(), this.UI);
        }
    }
}