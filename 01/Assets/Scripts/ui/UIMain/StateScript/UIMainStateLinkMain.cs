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
    public class UIMainStateLinkMain : UIMainStateBase
    {
        public override UIMain.EnumUIMainState SelfState
        {
            get
            {
                return UIMain.EnumUIMainState.LinkMain;
            }
        }

        public override void RefreshUI()
        {
            UI.RoomId = 0;
            this.UI.PlayInfoScript.SetData(null, null, null);
            this.UI.CenterScript.SetData(null);
            this.UI.TopScript.SetData(null);
            this.UI.DownScript.SetData(null);
            this.UI.RightButtonScript.SetData(null);

            this.UI.RefreshUI(SelfState);
        }
    }
}