
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using projectQ;
using MahjongPlayType;
namespace projectQ
{
    public partial class MahjongUIManager
    {
        private UINiuPai _UINiuPai;
        private UINiuPai _UIBuNiu;
        public void RegisterModuleEvent_NiuPaiUIModule()
        {
            // _MahjongEventHelper.AddEvent(MJEnum.NiuPaiEvent.NPE_ShowNiuPaiUI.ToString(), ShowNiuPaiUI);
            _MahjongEventHelper.AddEvent(MJEnum.NiuPaiEvent.NPE_ShowBuNiuPaiUI.ToString(), ShowNiuPaiUI);
            _MahjongEventHelper.AddEvent(MJEnum.NiuPaiEvent.NPE_CloseBuNiuPaiUI.ToString(), CloseNiuPaiUI);
            _MahjongEventHelper.AddEvent(MJEnum.NiuPaiEvent.NPE_CloseNiuPaiUI.ToString(), CloseNiuPaiUI);
            _MahjongEventHelper.AddEvent(MJEnum.NiuPaiEvent.NPE_UILogicNiu.ToString(), UILogicNiu);
            _MahjongEventHelper.AddEvent(MJEnum.NiuPaiEvent.NPE_UINiuPaiReconnect.ToString(), UINiuPaiReconnect);
            _MahjongEventHelper.AddEvent(MJEnum.NiuPaiEvent.NPE_UINiuPaiShowFx.ToString(), ShowUIClickFx);
            _AnimEventHelper.AddEvent(MJEnum.NiuPaiEvent.NPE_NiuPaiAnim.ToString(), NiuPaiAnim);
        }
        private void NiuPaiAnim(object[] param)
        {
            ShowNiuPaiUI(null);
        }
        private void ShowUIClickFx(object[] param)
        {
            int uiseatID = CardHelper.GetMJUIPosByServerPos(MjDataManager.Instance.MjData.ProcessData.processNiuPai.NotifyNiuPaiData.SeatID, MjDataManager.Instance.MjData.curUserData.selfSeatID);
            this.ShowTipsBase(new List<int>() { uiseatID }, EnumMjOpAction.MjOp_NiuPaiBuHua, 0, ConstDefine.Mj_Anim2d_BaseTips);
        }
        private void UILogicNiu(object[] param)
        {
            int uiseatID = CardHelper.GetMJUIPosByServerPos(MjDataManager.Instance.MjData.ProcessData.processNiuPai.NotifyNiuPaiData.SeatID, MjDataManager.Instance.MjData.curUserData.selfSeatID);
            List<NiuPaiGroupItem> niuPaiData = MjDataManager.Instance.MjData.ProcessData.processNiuPai.ResponseNiuPai.Data;
            if (NullHelper.IsObjectIsNull(niuPaiData))
            {
                return;
            }
            for (int i = 0; i < niuPaiData.Count; i++)
            {
                //最后一组做动画，因为动画之前，先把这组牌隐藏，动画之后只会把最后一组进行显示
                bool needAnim = true;// i == niuPaiData.Count - 1 ? true : false;
                _ui3D.seatUIArray[uiseatID].CreatePengGangZoneCardsResult(niuPaiData[i].MjCodes, needAnim, EnumMjCodeType.Code_Niu, EnumMjControlerShowType.ShowTypeMidle);
            }
        }
        private void ShowNiuPaiUI(object[] param)
        {
            if (!NullHelper.IsObjectIsNull(_UINiuPai))
            {
                CloseUISelf<UINiuPai>(_UINiuPai);
            }
            _UINiuPai = LoadUISelf<UINiuPai>(PrefabPathDefine.UINiuPai, new StructUIAnchor(), false);
            if (NullHelper.IsObjectIsNull(_UINiuPai))
            {
                return;
            }
            // _UINiuPai.RefreshUI();
        }
        private void ShowBuNiuUI(object[] param)
        {
            if (!NullHelper.IsObjectIsNull(_UIBuNiu))
            {
                CloseUISelf<UINiuPai>(_UIBuNiu);
            }
            _UIBuNiu = LoadUISelf<UINiuPai>(PrefabPathDefine.UINiuPai, new StructUIAnchor(), false);
            if (NullHelper.IsObjectIsNull(_UIBuNiu))
            {
                return;
            }
            // _UINiuPai.RefreshUI();
        }
        private void CloseNiuPaiUI(object[] param)
        {
            if (!NullHelper.IsObjectIsNull(_UINiuPai))
            {
                CloseUISelf<UINiuPai>(_UINiuPai);
            }
        }
        private void CloseBuNiuPaiUI(object[] param)
        {
            if (!NullHelper.IsObjectIsNull(_UIBuNiu))
            {
                CloseUISelf<UINiuPai>(_UIBuNiu);
            }
        }
        private void UINiuPaiReconnect(object[] param)
        {
            List<MahjongPlayType.NiuPaiHistoryRecords> totalRecords = MjDataManager.Instance.MjData.ProcessData.processNiuPai.HistoryRecords;
            if (totalRecords == null)
            {
                return;
            }
            for (int j = 0; j < totalRecords.Count; j++)
            {
                MahjongPlayType.NiuPaiHistoryRecords records = totalRecords[j];
                if (NullHelper.IsObjectIsNull(records))
                {
                    return;
                }
                if (records.DeskID != MjDataManager.Instance.MjData.curUserData.selfDeskID)
                {
                    DebugPro.DebugError("桌号错误：", records.DeskID);
                    continue;
                }
                if (NullHelper.IsInvalidIndex(0, records.NiuPaiCountRecords))
                {
                    return;
                }
                int recordsCount = records.NiuPaiCountRecords[0];
                records.NiuPaiCountRecords.RemoveAt(0);
                List<NiuPaiGroupItem> niuPaiRecordItem = new List<NiuPaiGroupItem>();
                for (int i = 0; i < recordsCount; i++)
                {
                    if (records.NiuPaiData.Count <= 0)
                    {
                        break;
                    }
                    niuPaiRecordItem.Add(records.NiuPaiData[0]);
                    records.NiuPaiData.RemoveAt(0);
                }

                int uiseatID = CardHelper.GetMJUIPosByServerPos(records.SeatID, MjDataManager.Instance.MjData.curUserData.selfSeatID);
                for (int i = 0; i < niuPaiRecordItem.Count; i++)
                {
                    _ui3D.seatUIArray[uiseatID].CreatePengGangZoneCardsResult(niuPaiRecordItem[i].MjCodes, false, EnumMjCodeType.Code_Niu, EnumMjControlerShowType.ShowTypeMidle);
                }
            }

        }
    }
}

public class NiuPaiUIModule : SpecialUIModule
{
    public override void AddEvents()
    {

    }

    public override void ReconnectedPreparedUIManager()
    {

    }

}

