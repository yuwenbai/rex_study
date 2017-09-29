using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using projectQ;

public class DuanMenLogicModule : SpecialLogicModule
{
    public override void ClearUp()
    {

    }

    public override void AddEvents()
    {
        m_EventHelper.AddEvent(MJEnum.MjDuanMenEvent.MjDM_LogicNotify.ToString(), LiangGangTouNotify);
    }

    private void LiangGangTouNotify(object[] param)
    {
        EventDispatcher.FireEvent(MJEnum.MjDuanMenEvent.MjDM_ShowUI.ToString());
    }
}
