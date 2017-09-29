/**
 * @Author JEFF
 *
 *
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using projectQ;

public class GangHouNaChiLogicModule : SpecialLogicModule
{
    public override void ClearUp()
    {

    }

    public override void AddEvents()
    {
        m_EventHelper.AddEvent(MJEnum.GangHouNaChi.GHNC_LogicNotify.ToString(), GangHouNaChiNotify);//整个拿吃状态通知
        m_EventHelper.AddEvent(MJEnum.GangHouNaChi.GHNC_LogicUploadResult.ToString(), UploadNachiResult);//关闭tips消息
        m_EventHelper.AddEvent(MJEnum.GangHouNaChi.GHNC_LogicResponseResult.ToString(), LogicResponseResult);  //拿吃结果服务器消息返回处理
    }

    private void GangHouNaChiNotify(object[] param)
    {
        MahjongPlayType.GangHouNaChiNotifyData data = MjDataManager.Instance.MjData.ProcessData.processGangHouNaChi.GangHouNaChiNotifyData;

        if (NullHelper.IsObjectIsNull(data))
        {
            DebugPro.DebugWarning("GangHouNaChiNotifyData is Null");
            return;
        }
        if (data.MjCodeList == null || data.MjCodeList.Count <= 0)
        {
            DebugPro.DebugError("GangHouNaChiNotifyData MjCodeList is null");
            return;
        }
        EventDispatcher.FireEvent(MJEnum.GangHouNaChi.GHNC_OpenUINaChi.ToString());

        if (data.SelectSubType == (int)EnumMjSelectSubType.MjSelectSubType_WAIT_NoSelect)
        {//需要杠后拿吃
            if (data.SeatID == MjDataManager.Instance.MjData.curUserData.selfSeatID)
            {//自己选牌
                EventDispatcher.FireEvent(MJEnum.GangHouNaChi.GHNC_OpenUINaChiChoose.ToString());
            }
            else
            {
                EventDispatcher.FireEvent(MJEnum.GangHouNaChi.GHNC_OpenUINaChiTips.ToString(), data.SeatID);
            }
        }
        else 
        {
            DebugPro.DebugInfo("GangHouNaChi SelectSubType: no show");
        }

    }
    private void UploadNachiResult(object[] result)
    {
        //关闭ui
        //上传结果
        EventDispatcher.FireEvent(MJEnum.GangHouNaChi.GHNC_CloseUINaChiChoose.ToString());
        EventDispatcher.FireEvent(MJEnum.GangHouNaChi.GHNC_CloseUINaChiTips.ToString());
        MahjongPlayType.MahjongPlayOperateGangHouNaChi nachiData = MjDataManager.Instance.MjData.ProcessData.processGangHouNaChi;
        if (NullHelper.IsObjectIsNull(nachiData))
        {
            return;
        }
        MahjongPlayType.GangHouNaChiUploadData uploadData = nachiData.GangHouNaChiUploadData;
        if (NullHelper.IsObjectIsNull(uploadData))
        {
            return;
        }
        ModelNetWorker.Instance.MjReqGameingNaChiResult(uploadData.DeskID, uploadData.SeatID, uploadData.MjCode, uploadData.MjIndex);
    }

    private void LogicResponseResult(object[] result)
    {
        EventDispatcher.FireEvent(MJEnum.GangHouNaChi.GHNC_CloseUINaChiTips.ToString());
    }
   
}
