/**
* @Author Xin.Wang
* 亮一张的逻辑功能层
*
*/
using System.Collections;
using System.Collections.Generic;
using projectQ;
using MahjongPlayType;



public class LiangyiLogicModule : SpecialLogicModule
{
    public override void AddEvents()
    {
        m_EventHelper.AddEvent(MJEnum.MjLiangyiEvent.MJLY_LogicShowUI.ToString(), EventShowUI);
        m_EventHelper.AddEvent(MJEnum.MjLiangyiEvent.MJLY_LogicUpdateUI.ToString(), EventUpdateUI);
        m_EventHelper.AddEvent(MJEnum.MjLiangyiEvent.MJLY_LogicSendData.ToString(), EventSendData);
    }


    private void EventShowUI(object[] obj)
    {
        AnimPlayManager.Instance.PlayAnimCenter(MahjongTimeConfig.Instance.MjSystemTime.Time_Notime, MJEnum.MjLiangyiEvent.MJLY_AnimShowUI.ToString(), true);
    }


    private void EventUpdateUI(object[] obj)
    {
        int seatID = (int)obj[0];

        AnimPlayManager.Instance.PlayAnimCenter(MahjongTimeConfig.Instance.MjSystemTime.Time_Notime, MJEnum.MjLiangyiEvent.MJLY_AnimUpdateUI.ToString(), seatID);
    }

    private void EventSendData(object[] obj)
    {
        int curDesk = MjDataManager.Instance.MjData.curUserData.selfDeskID;
        int seatID = MjDataManager.Instance.MjData.curUserData.selfSeatID;
        MjLiangyiData data = MjDataManager.Instance.MjData.ProcessData.processLiangYi.GetADataBySeat(seatID);
        ModelNetWorker.Instance.MjReqLiangYiZhang(curDesk, seatID, data.chooseIndex + 1, data.chooseCard);
    }

    public override void ClearUp()
    {
    }

}
