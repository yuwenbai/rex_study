using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using projectQ;

public class UIMahjongBalanceNewModel : UIModelBase
{
    public UIMahjongBalanceNew UI
    {
        get { return _ui as UIMahjongBalanceNew; }
    }


    #region override
    protected override GEnum.NamedEvent[] FocusNetWorkData()
    {
        return new GEnum.NamedEvent[]
        {
            GEnum.NamedEvent.EMjBalanceDataRsp,
        };
    }

    protected override void OnNetWorkDataCallBack(GEnum.NamedEvent msgEnum, object[] data)
    {
        switch (msgEnum)
        {
            case GEnum.NamedEvent.EMjBalanceDataRsp:

                MjBalanceNew panelData = data[0] as MjBalanceNew;
                this.UI.RefreshPanelData(panelData);

                break;
        }
    }
    #endregion

}
