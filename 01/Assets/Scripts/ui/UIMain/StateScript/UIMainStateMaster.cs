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
    public class UIMainStateMaster : UIMainStateBase
    {
        public override UIMain.EnumUIMainState SelfState
        {
            get
            {
                return UIMain.EnumUIMainState.Master;
            }
        }

        public override void RefreshUI()
        {
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
            ChangeState(new UIMainStateLinkMain(), this.UI);
        }
    }
}