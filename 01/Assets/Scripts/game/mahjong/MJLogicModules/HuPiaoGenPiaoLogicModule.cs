/**
 * @Author Xin.Wang
 * 胡漂 跟漂 的逻辑模块
 *
 */

using System.Collections;
using System.Collections.Generic;
using projectQ;
using MahjongPlayType;

public class HuPiaoGenPiaoLogicModule : SpecialLogicModule
{
    public override void AddEvents()
    {
        m_EventHelper.AddEvent(MJEnum.MjHuPiaoGenPiaoEvent.HPGP_DataToLogicShowEffect.ToString(), HPGP_LogicShowUI);
        m_EventHelper.AddEvent(MJEnum.MjHuPiaoGenPiaoEvent.HPGP_UIToLogicSendData.ToString(), HPGP_UIToLogicSendData);
    }


    /// <summary>
    /// 逻辑 展示漂胡结果
    /// </summary>
    /// <param name="obj"></param>
    private void HPGP_LogicShowUI(object[] obj)
    {
        int seatID = (int)obj[0];

        //逻辑
        EventDispatcher.FireEvent(MJEnum.MjHuPiaoGenPiaoEvent.HPGP_LogicToUIEvent.ToString(), true, seatID);
        //特效
        //AnimPlayManager.Instance.PlayAnim(seatID, MahjongTimeConfig.Instance.MjSystemTime.Time_NormalTime, MJEnum.MjHuPiaoGenPiaoEvent.HPGP_LogicToUIAnim.ToString(), seatID);
        //声音

    }


    /// <summary>
    /// 发送数据
    /// </summary>
    /// <param name="obj"></param>
    private void HPGP_UIToLogicSendData(object[] obj)
    {
        int selfSeatID = MjDataManager.Instance.MjData.curUserData.selfSeatID;
        int mjCode = MjDataManager.Instance.GetPaikouHu();
        int deskID = MjDataManager.Instance.MjData.curUserData.selfDeskID;

        ModelNetWorker.Instance.MjReqPiaoHu(deskID, selfSeatID, mjCode);
    }

}
