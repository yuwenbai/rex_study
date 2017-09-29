/**
 * @Author JEFF
 *
 *
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using projectQ;
using MahjongPlayType;
public class DuAnGangLogicModule : SpecialLogicModule
{
    public override void AddEvents()
    {
        m_EventHelper.AddEvent(MJEnum.DuAnGangEvents.DAG_LogicNotify.ToString(), NotifyDuAnGang);
        m_EventHelper.AddEvent(MJEnum.DuAnGangEvents.DAG_RspNotify.ToString(), RspNotifyDuAnGang);
        m_EventHelper.AddEvent(MJEnum.DuAnGangEvents.DAG_LogicUploadData.ToString(), LogicUploadData);
    }


    private void NotifyDuAnGang(object[] param)
    {
        MahjongPlayType.NotifyDuAnGangData data = MjDataManager.Instance.MjData.ProcessData.processDuAnGang.NotifyDuAnGangData;
        if (NullHelper.IsObjectIsNull(data))
        {
            return;
        }
        if (!data.State)
        {
            EventDispatcher.FireEvent(MJEnum.DuAnGangEvents.DAG_CloseUIDuAnGang.ToString());
            EventDispatcher.FireEvent(MJEnum.DuAnGangEvents.DAG_ClearUIAlert.ToString());
        }
        else
        {//玩家自己展示对应的选择窗口
            //其他玩家给出提示
            EventDispatcher.FireEvent(MJEnum.DuAnGangEvents.DAG_OpenUIDuAnGang.ToString());
            EventDispatcher.FireEvent(MJEnum.DuAnGangEvents.DAG_UIAlertTip.ToString());

        }
    }

    private void RspNotifyDuAnGang(object[] param)
    {
        MahjongPlayOprateDuAnGang duAnGangData = MjDataManager.Instance.MjData.ProcessData.processDuAnGang;
        if (NullHelper.IsObjectIsNull(duAnGangData) || NullHelper.IsObjectIsNull(duAnGangData.RspDuAnGangData))
        {
            return;
        }
        EventDispatcher.FireEvent(MJEnum.DuAnGangEvents.DAG_UIDuAnGangShowResult.ToString());
        EventDispatcher.FireEvent(MJEnum.DuAnGangEvents.DAG_UIAlertTipResult.ToString()); 
        AnimPlayManager.Instance.PlayAnim(duAnGangData.RspDuAnGangData.SeatID, ConstDefine.DAG_HideDelayTime, GEnum.NameAnim.EMjAnimDuAnGang);

    }
    private void LogicUploadData(object[] param)
    {
        MahjongPlayOprateDuAnGang duAnGangData = MjDataManager.Instance.MjData.ProcessData.processDuAnGang;
        if (NullHelper.IsObjectIsNull(duAnGangData) || NullHelper.IsObjectIsNull(duAnGangData.UploadDuAnGangData))
        {
            return;
        }
        UploadDuAnGangData data = duAnGangData.UploadDuAnGangData;
        //通知逻辑层数据更新
        DebugPro.DebugInfo("上传赌暗杠结果：deskID:", data.DeskID, "seatID:", data.SeatID, "type(1赌;2过):", data.Type, "mjCode:", data.MjCode);
        ModelNetWorker.Instance.DuAnGangRequest(data.DeskID, data.SeatID, data.Type, data.MjCode);
    }
    public override void ClearUp()
    {
    }
}
