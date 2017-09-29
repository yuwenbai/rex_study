/**
 * @Author HaiLong.Zhang
 *
 *
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using projectQ;
using MahjongPlayType;
public class FengQuanLogicModule : SpecialLogicModule
{
    public override void AddEvents()
    {
        EventDispatcher.AddEvent(MJEnum.MjFengQuanEvent.MJFQ_LogicNotify.ToString(), NotifyFengQuan);
    }


    private void NotifyFengQuan(object[] obj)
    {
        AnimPlayManager.Instance.PlayAnimCenter(MahjongTimeConfig.Instance.MjSystemTime.Time_NormalTime, MJEnum.MjFengQuanEvent.MJFQ_ShowUI.ToString());
    }

}
