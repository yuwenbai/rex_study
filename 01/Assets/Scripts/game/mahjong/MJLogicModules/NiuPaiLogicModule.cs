/**
 * @Author JEFF
 *
 *
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using projectQ;
public class NiuPaiLogicModule : SpecialLogicModule
{
    public override void AddEvents()
    {
        base.m_EventHelper.AddEvent(MJEnum.NiuPaiEvent.NPE_LogicNiuPaiNotify.ToString(), LogicNiuPaiNotify);
        base.m_EventHelper.AddEvent(MJEnum.NiuPaiEvent.NPE_LogicRepNiuPai.ToString(), LogicRepNiuPai);
        base.m_EventHelper.AddEvent(MJEnum.NiuPaiEvent.NPE_LogicResponse.ToString(), LogicResponse);
        base.m_EventHelper.AddEvent(MJEnum.NiuPaiEvent.NPE_LogicReconnect.ToString(), NiuPaiLogicReconnect); 
    }
    private void LogicNiuPaiNotify(object[] param)
    {
        MahjongPlayType.NotifyNiuPai data = MjDataManager.Instance.MjData.ProcessData.processNiuPai.NotifyNiuPaiData;
        if (NullHelper.IsObjectIsNull(data))
        {
            return;
        }
        if (data.SeatID == MjDataManager.Instance.MjData.curUserData.selfSeatID)
        {
            AnimPlayManager.Instance.PlayAnim(MjDataManager.Instance.MjData.curUserData.selfSeatID, 0, MJEnum.NiuPaiEvent.NPE_NiuPaiAnim.ToString());
        }
        else
        {
            DebugPro.DebugInfo("LogicNiuPaiNotify : selfSeatID:", MjDataManager.Instance.MjData.curUserData.selfSeatID, "serverSeatID,", data.SeatID);
        }
    }

    private void LogicRepNiuPai(object[] param)
    {
        //关闭ui,上传数据
        EventDispatcher.FireEvent(MJEnum.NiuPaiEvent.NPE_CloseNiuPaiUI.ToString());
        ModelNetWorker.Instance.NiuPaiRequest();
    }

    private void LogicResponse(object[] param)
    {
       
        MahjongPlayType.ResponseNiuPai data = MjDataManager.Instance.MjData.ProcessData.processNiuPai.ResponseNiuPai;
        if (NullHelper.IsObjectIsNull(data))
        {
            return;
        }
        if (data.SelectedID == 2)
        {//用户选择的是过
            return;
        }
        else
        {
            //EventDispatcher.FireEvent(MJEnum.NiuPaiEvent.NPE_UINiuPaiShowFx.ToString());
            if (!data.IsNiuPai)
            {//等待补花消息下发
                return;
            }
            else
            {//往碰杠区进行扭牌
                EventDispatcher.FireEvent(MJEnum.NiuPaiEvent.NPE_UILogicNiu.ToString());
            }
        }
    }

    private void NiuPaiLogicReconnect(object[] param)
    {
        EventDispatcher.FireEvent(MJEnum.NiuPaiEvent.NPE_UINiuPaiReconnect.ToString());
    }
    public override void ClearUp()
    {
        // ClearUITips();
    }
}