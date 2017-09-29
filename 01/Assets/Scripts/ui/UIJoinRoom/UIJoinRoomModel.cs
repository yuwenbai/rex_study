using System;
using System.Collections;
using System.Collections.Generic;
using Msg;
using UnityEngine;

namespace projectQ
{
    public class UIJoinRoomModel : UIModelBase
    {
        private UIJoinRoom UI
        {
            get { return _ui as UIJoinRoom; }
        }

        protected override GEnum.NamedEvent[] FocusNetWorkData()
        {
            return new GEnum.NamedEvent[] {
                GEnum.NamedEvent.EMahjongJoinDesk
            };
        }
        protected override void OnNetWorkDataCallBack(GEnum.NamedEvent msgEnum, object[] data)
        {
            switch(msgEnum)
            {
                case GEnum.NamedEvent.EMahjongJoinDesk:
                    UI.SendPasswordCallback((int)data[0]);
                    break;
            }
        }
    }
}
