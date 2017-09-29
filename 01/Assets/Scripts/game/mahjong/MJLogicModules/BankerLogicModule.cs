/**
 * @Author JEFF
 *
 *
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using projectQ;
public class BankerLogicModule : SpecialLogicModule
{
    public override void AddEvents()
    {
        m_EventHelper.AddEvent(MJEnum.BankerPutAlert.BPA_BankerFirstTurn.ToString(), LogicUIAlert);
        m_EventHelper.AddEvent(MJEnum.BankerPutAlert.BPA_BankerTurnOver.ToString(), PutCard);
    }

    /// <summary>
    /// 庄家第一次出牌提示
    /// </summary>
    /// <param name="vars"></param>
    private void LogicUIAlert(object[] vars)
    {
        if (MjDataManager.Instance.MjData.ProcessData.processBankerFirstCard.IsFirstPut)
        {
            EventDispatcher.FireEvent(MJEnum.BankerPutAlert.BPA_OpenUIAlert.ToString());
        }
    }
    private void PutCard(object[] vars)
    {
        EventDispatcher.FireEvent(MJEnum.BankerPutAlert.BPA_CloseUIAlert.ToString());
    }

    public override void ClearUp()
    {
        // ClearUITips();
    }
}
