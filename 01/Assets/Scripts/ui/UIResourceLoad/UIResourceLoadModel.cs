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
    public class UIResourceLoadModel : UIModelBase
    {
        private UIResourceLoad UI
        {
            get { return base._ui as UIResourceLoad; }
        }

        #region override

        protected override GEnum.NamedEvent[] FocusNetWorkData()
        {
            return new GEnum.NamedEvent[]
            {
                GEnum.NamedEvent.SysUI_Loading_Change,
            };
        }

        protected override void OnNetWorkDataCallBack(GEnum.NamedEvent msgEnum, object[] data)
        {
            switch (msgEnum)
            {
                case GEnum.NamedEvent.SysUI_Loading_Change:
                    UI.OnChangeValue((float)data[0], (float)data[1]);
                    break;
            }
        }

        #endregion
    }
}