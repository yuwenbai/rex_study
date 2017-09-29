using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using projectQ;

public class LiangGangTouLogicModule : SpecialLogicModule
{
    public override void ClearUp()
    {

    }

    public override void AddEvents()
    {
        m_EventHelper.AddEvent(MJEnum.MjLiangGangTouEvent.MJLGT_LogicNotify.ToString(), LiangGangTouNotify);
    }

    private void LiangGangTouNotify(object[] param)
    {
        AnimPlayManager.Instance.PlayAnimCenter(MahjongTimeConfig.Instance.MjSystemTime.Time_Notime, MJEnum.MjLiangGangTouEvent.MJLGT_ShowUi.ToString());
    }
}
