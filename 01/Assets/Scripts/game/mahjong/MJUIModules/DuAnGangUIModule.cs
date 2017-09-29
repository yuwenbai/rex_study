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
namespace projectQ
{
    public partial class MahjongUIManager
    {
        private UIDuAnGang _DuAnGang;
        public void RegisterModuleEvent_DuAnGangUIModule()
        {
            _MahjongEventHelper.AddEvent(MJEnum.DuAnGangEvents.DAG_OpenUIDuAnGang.ToString(), LoadUIDuAnGang);
            _MahjongEventHelper.AddEvent(MJEnum.DuAnGangEvents.DAG_CloseUIDuAnGang.ToString(), CloseUIDuAnGang);
            _MahjongEventHelper.AddEvent(MJEnum.DuAnGangEvents.DAG_UIDuAnGangShowResult.ToString(), UIDuAnGangShowResult);
        }
        private void LoadUIDuAnGang(object[] param)
        {
            if (!NullHelper.IsObjectIsNull(_DuAnGang))
            {
                CloseUIDuAnGang(param);
            }
            _DuAnGang = LoadUISelf<UIDuAnGang>(PrefabPathDefine.UIDuAnGang, new StructUIAnchor(), false);
            if (NullHelper.IsObjectIsNull(_DuAnGang))
            {
                return;
            }
            _DuAnGang.RefreshUI();
        }
        private void CloseUIDuAnGang(object[] param)
        {
            if (NullHelper.IsObjectIsNull(_DuAnGang))
            {
                return;
            }
            CloseUISelf<UIDuAnGang>(_DuAnGang);
        }

        private void UIDuAnGangShowResult(object[] param)
        {
            if (NullHelper.IsObjectIsNull(_DuAnGang))
            {
                return;
            }
            _DuAnGang.RefreshResultUI();
        }
    }
}

public class DuAnGangUIModule : SpecialUIModule
{
    public override void AddEvents()
    {
        m_EventHelper.AddEvent(MJEnum.DuAnGangEvents.DAG_ClearUIAlert.ToString(), ClearUIAlert);
        m_EventHelper.AddEvent(MJEnum.DuAnGangEvents.DAG_UIAlertTipResult.ToString(), UIAlertTipResult);
        m_EventHelper.AddEvent(MJEnum.DuAnGangEvents.DAG_UIAlertTip.ToString(), UIAlertTip);
    }
    private void ClearUIAlert(object[] param)
    {
        MahjongPlayOprateDuAnGang duAnGangData = MjDataManager.Instance.MjData.ProcessData.processDuAnGang;
        if (NullHelper.IsObjectIsNull(duAnGangData) || NullHelper.IsObjectIsNull(duAnGangData.NotifyDuAnGangData))
        {
            return;
        }
        EventDispatcher.FireEvent(MJEnum.DuAnGangEvents.DAG_UIAlert.ToString(), duAnGangData.NotifyDuAnGangData.SeatID, "");
    }
    private void UIAlertTipResult(object[] param)
    {
        MahjongPlayOprateDuAnGang duAnGangData = MjDataManager.Instance.MjData.ProcessData.processDuAnGang;
        if (NullHelper.IsObjectIsNull(duAnGangData) || NullHelper.IsObjectIsNull(duAnGangData.RspDuAnGangData))
        {
            return;
        }
        MahjongPlayType.RspDuAnGangData data = duAnGangData.RspDuAnGangData;
        if (data.ResultCode == 1)
        {//成功            
            EventDispatcher.FireEvent(MJEnum.DuAnGangEvents.DAG_UIAlert.ToString(), data.SeatID, "");
        }
        else if (duAnGangData.RspDuAnGangData.ResultCode == 3)
        {//失败
            EventDispatcher.FireEvent(MJEnum.DuAnGangEvents.DAG_UIAlert.ToString(), data.SeatID, "");
        }
        else
        {//过
            EventDispatcher.FireEvent(MJEnum.DuAnGangEvents.DAG_UIAlert.ToString(), data.SeatID, UIConstStringDefine.GiveUpDuAnGang);
        }
    }
    private void UIAlertTip(object[] param)
    {
        MahjongPlayOprateDuAnGang duAnGangData = MjDataManager.Instance.MjData.ProcessData.processDuAnGang;
        if (NullHelper.IsObjectIsNull(duAnGangData) || NullHelper.IsObjectIsNull(duAnGangData.NotifyDuAnGangData))
        {
            return;
        }
        MahjongPlayType.NotifyDuAnGangData data = duAnGangData.NotifyDuAnGangData;
        if (data.SeatID != MjDataManager.Instance.MjData.curUserData.selfSeatID)
        {
            EventDispatcher.FireEvent(MJEnum.DuAnGangEvents.DAG_UIAlert.ToString(), data.SeatID, UIConstStringDefine.PlayingDuAnGang);
        }
    }
    public override void ReconnectedPreparedUIManager()
    {
        EventDispatcher.FireEvent(MJEnum.DuAnGangEvents.DAG_LogicNotify.ToString());
    }

}
