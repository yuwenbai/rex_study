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
    public class UIMainStateNone : UIMainStateBase
    {
        public override UIMain.EnumUIMainState SelfState
        {
            get
            {
                return UIMain.EnumUIMainState.None;
            }
        }

        public override void RefreshUI()
        {
            UI.RoomId = 0;

            //切换到馆主界面
            if (MemoryData.PlayerData.MyPlayerModel.PlayerDataBase.OwnerRoomID > 0)
            {
                ChangeState(new UIMainStateMaster(),this.UI);
            }
            //切换到绑定界面 1 未关联 2 关联 3 解除关联中
            else if (MemoryData.PlayerData.MyPlayerModel.PlayerDataBase.BindRoomState == 2 
                || MemoryData.PlayerData.MyPlayerModel.PlayerDataBase.BindRoomState == 3)
            {
                ChangeState(new UIMainStateLinkedMjHall(), this.UI);
            }
            else
            {
                ChangeState(new UIMainStateNormalMain(), this.UI);
            }
        }
    }
}