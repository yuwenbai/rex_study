

using projectQ.chectKey;
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

    public class UICheatKeyListModel : UIModelBase {
        private UICheatKeyList UI
        {
            get { return this._ui as UICheatKeyList; }
        }
        #region override
        protected override GEnum.NamedEvent[] FocusNetWorkData()
        {
            return new GEnum.NamedEvent[] {
                GEnum.NamedEvent.SysUI_CheatKeyDataUpdata
            };
        }
        protected override void OnNetWorkDataCallBack(GEnum.NamedEvent msgEnum, object[] data)
        {
            switch(msgEnum)
            {
                case GEnum.NamedEvent.SysUI_CheatKeyDataUpdata:
                    UI.Refresh();
                    break;
            }
        }
        #endregion
    }
}