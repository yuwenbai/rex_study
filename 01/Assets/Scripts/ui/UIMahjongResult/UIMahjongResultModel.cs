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
    public class UIMahjongResultModel : UIModelBase
    {
        public UIMahjongResult UI
        {
            get
            {
                return this._ui as UIMahjongResult;
            }
        }

        protected override GEnum.NamedEvent[] FocusNetWorkData()
        {
            return new GEnum.NamedEvent[] {
                GEnum.NamedEvent.SysData_MjHall_MjDeskViewRecord,
                //GEnum.NamedEvent.EMjControlCloseMaskUI
            };
        }

        protected override void OnNetWorkDataCallBack(GEnum.NamedEvent msgEnum, object[] data)
        {
            switch (msgEnum)
            {
                case GEnum.NamedEvent.SysData_MjHall_MjDeskViewRecord:
                    {
                        int deskID = (int)data[0];
                        UI.RefreshData(deskID);
                    }
                    break;
                    //case GEnum.NamedEvent.EMjControlCloseMaskUI:
                    //    {
                    //        UI.Close();
                    //    }
                    //    break;
            }
        }


    }

}

