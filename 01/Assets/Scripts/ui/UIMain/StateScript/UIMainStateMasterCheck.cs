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
    public class UIMainStateMasterCheck : UIMainStateBase
    {
        public override UIMain.EnumUIMainState SelfState
        {
            get
            {
                return UIMain.EnumUIMainState.MasterCheck;
            }
        }

        public override void RefreshUI()
        {
            ModelNetWorker.Instance.MyFMjRoomReq(MemoryData.PlayerData.MyPlayerModel.PlayerDataBase.UserID);
            this.UI.LoadUIMain("UICuratorList",MemoryData.PlayerData.MyPlayerModel.PlayerDataBase.OwnerRoomID);

            UI.RoomId = MemoryData.PlayerData.MyPlayerModel.PlayerDataBase.OwnerRoomID;
            UI.CurrMjHalll = MemoryData.MjHallData.GetMjHallById(UI.RoomId);

            this.UI.PlayInfoScript.SetData(OnGoBackClick, null, UI.CurrMjHalll);
            this.UI.CenterScript.SetData(null);
            this.UI.TopScript.SetData(UI.CurrMjHalll);
            this.UI.DownScript.SetData(UI.CurrMjHalll);
            this.UI.RightButtonScript.SetData(null);
            this.UI.RefreshUI(SelfState);
        }

        private void OnGoBackClick()
        {
            _R.ui.CloseUI("UICuratorList");
            ChangeState(new UIMainStateMaster(), this.UI);
        }
    }
}