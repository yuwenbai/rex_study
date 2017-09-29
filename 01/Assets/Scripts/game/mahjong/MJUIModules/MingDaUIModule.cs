using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using projectQ;
using MahjongPlayType;

namespace projectQ
{
    public partial class MahjongUIManager
    {
        private UIMahjongMingDaBtn _MingDa;
        public void RegisterModuleEvent_MingDaUIModule()
        {
            _MahjongEventHelper.AddEvent(MJEnum.MingDaEvents.MD_ShowUI.ToString(), LoadUIMingDa);
            _MahjongEventHelper.AddEvent(MJEnum.MingDaEvents.MD_CloseUI.ToString(), CloseUIMingDa);
            _MahjongEventHelper.AddEvent(MJEnum.MingDaEvents.MD_ShowResult.ToString(), UIMingDaShowResult);
            _MahjongEventHelper.AddEvent(MJEnum.MingDaEvents.MD_ShowRefresh.ToString(), UIRefreshShowUI);
        }

        private void LoadUIMingDa(object[] param)
        {
            if (!NullHelper.IsObjectIsNull(_MingDa))
            {
                CloseUIMingDa(param);
            }
            _MingDa = LoadUISelf<UIMahjongMingDaBtn>(PrefabPathDefine.UIMingDa, new StructUIAnchor(), false);
            if (NullHelper.IsObjectIsNull(_MingDa))
            {
                return;
            }

            MahjongPlayType.MjMingDaData data = MjDataManager.Instance.GetMainPlayer();
            if (data == null)
                return;
            MjMingDaData.CommonData commondata = data.GetCommonData(MjXuanPiaoData.EnumCommonType.MingDa);
            if (commondata != null)
            {
                _MingDa.RefreshUI(EnumMjSpecialCheck.MjBaseCheckType_MingDa, commondata.isSelect, commondata.chooseVlaueList);
                ShowTipsSpecial(data.SelectState != EnumMjSelectSubType.MjSelectSubType_RESULT_Select, EnumMjSpecialCheck.MjBaseCheckType_MingDa);
            }

            RefreshMingDaTag(data.SelectState);
        }

        private void RefreshMingDaTag(EnumMjSelectSubType curType)
        {
            int tarValue = 1;
            MahjongPlayType.MjMingDaData mainData = MjDataManager.Instance.GetMainPlayer();
            if (mainData == null)
                return;
            if (curType  == EnumMjSelectSubType.MjSelectSubType_WAIT_NoSelect || curType == EnumMjSelectSubType.MjSelectSubType_WAIT_Select)
            {
                if (mainData.SelectState == EnumMjSelectSubType.MjSelectSubType_WAIT_Select)
                {
                    MjMingDaData.CommonData commondata = mainData.GetCommonData(MjXuanPiaoData.EnumCommonType.MingDa);
                    if (commondata.curChooseValue == tarValue)
                    {
                        int uiSeatID = CardHelper.GetMJUIPosByServerPos(mainData.SeatID, MjDataManager.Instance.MjData.curUserData.selfSeatID);
                        ShowMingDa(uiSeatID, true, !MjDataManager.Instance.isOutMingDa);
                    }
                    return;
                }
            }
            else
            {
                List<int> list = MjDataManager.Instance.GetAllSeatIDCurDesk(false);
                for (int i = 0; i < list.Count; i++)
                {
                    MahjongPlayType.MjMingDaData data = MjDataManager.Instance.GetPlayerBySeatID(list[i]);
                    MjMingDaData.CommonData commondata = data.GetCommonData(MjXuanPiaoData.EnumCommonType.MingDa);
                    if(commondata.curChooseValue == tarValue)
                    {
                        int uiSeatID = CardHelper.GetMJUIPosByServerPos(list[i], MjDataManager.Instance.MjData.curUserData.selfSeatID);
                        ShowMingDa(uiSeatID, true, !MjDataManager.Instance.isOutMingDa);
                    }
                }
            }
        }

        private void CloseUIMingDa(object[] param)
        {
            if (NullHelper.IsObjectIsNull(_MingDa))
            {
                return;
            }
            CloseUISelf<UIMahjongMingDaBtn>(_MingDa);
        }

        private void UIMingDaShowResult(object[] param)
        {
            if (NullHelper.IsObjectIsNull(_MingDa))
            {
                return;
            }
            _MingDa.RefreshResultUI();
        }

        private void UIRefreshShowUI(object[] param)
        {
            if (NullHelper.IsObjectIsNull(_MingDa))
            {
                return;
            }

            MahjongPlayType.MjMingDaData mainData = MjDataManager.Instance.GetMainPlayer();
            if (mainData == null)
                return;
            if (mainData.SelectState == EnumMjSelectSubType.MjSelectSubType_RESULT_Select)
            {
                ShowTipsSpecial(false, EnumMjSpecialCheck.MjBaseCheckType_MingDa);
                return;
            }

            int seatid = (int)param[0];
            MahjongPlayType.MjMingDaData data = MjDataManager.Instance.GetPlayerBySeatID(seatid);
            MjMingDaData.CommonData commondata = data.GetCommonData(MjXuanPiaoData.EnumCommonType.MingDa);
            int uiSeatID = CardHelper.GetMJUIPosByServerPos(seatid, MjDataManager.Instance.MjData.curUserData.selfSeatID);

            if (seatid == MjDataManager.Instance.MjData.curUserData.selfSeatID)
            {
                _MingDa.RefreshUI(EnumMjSpecialCheck.MjBaseCheckType_MingDa, commondata.isSelect, commondata.chooseVlaueList);
                if (commondata.curChooseValue == 1)
                    ShowMingDa(uiSeatID, true, true);
            }
            else
            {
                if (commondata != null)
                {
                    if (commondata.isSelect)
                        UpdateTipsSpecail(uiSeatID, EnumMjSpecialCheck.MjBaseCheckType_MingDa);
                }
            }
        }

        private void ShowMingDa(int uiseatID, bool isTing, bool anim)
        {
            if (_desk != null)
            {
                _desk.ShowCenterUp(uiseatID, EnumMjSpecialCheck.MjBaseCheckType_MingDa, anim);
            }
        }

    }
}

public class MingDaUIModule : SpecialUIModule
{
    public override void AddEvents()
    {
        m_EventHelper.AddEvent(MJEnum.MingDaEvents.MD_CloseUI.ToString(), ClearUIAlert);
        //m_EventHelper.AddEvent(MJEnum.MingDaEvents.MD_ShowOtherPlayer.ToString(), UIAlertTipResult);
        m_EventHelper.AddEvent(MJEnum.MingDaEvents.MD_ClickBtn.ToString(), UIClickBtn);
    }

    private void ClearUIAlert(object[] param)
    {
    }

    private void UIAlertTipResult(object[] param)
    {
    }

    private void UIClickBtn(object[] param)
    {
        int selfSeatID = MjDataManager.Instance.MjData.curUserData.selfSeatID;
        MjMingDaData data = MjDataManager.Instance.GetMainPlayer();
        int value = (int)param[0];

        if (data != null)
        {
            List<int> rulerID = new List<int>();
            List<int> valueList = new List<int>();
            for (int i = 0; i < data.Data.Count; i++)
            {
                int ru = (int)data.Data[i].enumType;
                rulerID.Add(ru);
                if (value >= 0)
                {
                    data.Data[i].curChooseValue = value;
                    valueList.Add(value);
                }
                else
                {
                    int va = data.Data[i].curChooseValue;
                    valueList.Add(va);
                }
            }

            MjDataManager.Instance.MjData.ProcessData.processMingDaCard.SendMingDaSelect(MjDataManager.Instance.MjData.curUserData.selfDeskID, selfSeatID, rulerID, valueList);
        }
    }

    public override void ReconnectedPreparedUIManager()
    {
        if (MjDataManager.Instance.isOutMingDa)
        {
            MjMingDaData data = MjDataManager.Instance.GetMainPlayer();
            if (data != null)
            {
                EventDispatcher.FireEvent(MJEnum.MingDaEvents.MD_LogicNotify.ToString(), (int)data.SelectState);
            }
        }
    }
}
