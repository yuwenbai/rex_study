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
    public class UICuratorListModel : UIModelBase
    {
        public UICuratorList UI
        {
            get {
                return this._ui as UICuratorList;
            }
        }

        protected override GEnum.NamedEvent[] FocusNetWorkData()
        {
            return new GEnum.NamedEvent[] {
               GEnum.NamedEvent.SysData_MjHall_CheckMjHall,
               GEnum.NamedEvent.SysData_MjHall_MjHallUpdate
            };
        }


        protected override void OnNetWorkDataCallBack(GEnum.NamedEvent msgEnum, object[] data)
        {
            switch (msgEnum)
            {
                case GEnum.NamedEvent.SysData_MjHall_CheckMjHall:
                    {
                        int roomID = (int)data[0];
                        UI.OnRefreshData(roomID);
                    }
                    break;
                case GEnum.NamedEvent.SysData_MjHall_MjHallUpdate:
                    {
                        int deskCount = (int)data[0];
                        int members = (int)data[1];
                        int onlineNum = (int)data[2];

                        UI.OnUpdateMessage(deskCount, members, onlineNum);
                    }
                    break;
            }
        }





    }

}


