/**
 * @Author JEFF
 *
 *
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace projectQ
{
    public class UICreateRoomRuleModel : UIModelBase {
        private UICreateRoomRule UI
        {
            get { return this._ui as UICreateRoomRule; }
        }

        public MahjongPlayData PlayData;

        #region override
        protected override GEnum.NamedEvent[] FocusNetWorkData()
        {
            return new GEnum.NamedEvent[]
            {
                 GEnum.NamedEvent.SysData_User_RoomCard_Update,
                GEnum.NamedEvent.SysData_MJPlayData_Update,
                GEnum.NamedEvent.EMahjongJoinDesk,
                GEnum.NamedEvent.SysData_Region_Update,
                
                //GEnum.NamedEvent.EMahjongNewDesk,
            };
        }
        protected override void OnNetWorkDataCallBack(GEnum.NamedEvent msgEnum, object[] data)
        {
            switch(msgEnum)
            {
                case GEnum.NamedEvent.SysData_MJPlayData_Update:
                    {
                        PlayData = MemoryData.MahjongPlayData;
                        UI.RefreshUI();
                    }
                    break;

                //玩家自己加入desk
                case GEnum.NamedEvent.EMahjongJoinDesk:
                    {
                        int resultCode = (int) data[0];
                        if (resultCode == 0)
                        {
                            UI.Close();
                        }
                    }
                    break;
                case GEnum.NamedEvent.SysData_User_RoomCard_Update:
                    {
                        UI.RefreshUI();
                    }
                    break;
                case GEnum.NamedEvent.SysData_Region_Update:
                    {
                        UI.RefreshUI();
                    }
                    break;
            }
        }
        #endregion

        private void Awake()
        {
            PlayData = MemoryData.MahjongPlayData;
        }
    }
}