/**
 * @Author JEFF
 *
 *
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using projectQ;

namespace projectQ
{
    public partial class MahjongUIManager
    {
        private UIMahjongNachi _UIMahjongNachi;
        private UIMahjongNaChiChoose _UIMahjongNaChiChoose;
        private UIPlayingTips _UIPlayingTips;
        public void RegisterModuleEvent_GangHouNaChiUIModule()
        {
            _MahjongEventHelper.AddEvent(MJEnum.GangHouNaChi.GHNC_OpenUINaChi.ToString(), OpenUINaChi);
            _MahjongEventHelper.AddEvent(MJEnum.GangHouNaChi.GHNC_CloseUINaChi.ToString(), CloseUINaChi);
            _MahjongEventHelper.AddEvent(MJEnum.GangHouNaChi.GHNC_OpenUINaChiChoose.ToString(), OpenUINaChiChoose);
            _MahjongEventHelper.AddEvent(MJEnum.GangHouNaChi.GHNC_CloseUINaChiChoose.ToString(), CloseUINaChiChoose);
            _MahjongEventHelper.AddEvent(MJEnum.GangHouNaChi.GHNC_OpenUINaChiTips.ToString(), OpenUINaChiTips);
            _MahjongEventHelper.AddEvent(MJEnum.GangHouNaChi.GHNC_CloseUINaChiTips.ToString(), CloseUINaChiTips);
            _MahjongEventHelper.AddEvent(MJEnum.GangHouNaChi.GHNC_ClearUI.ToString(), GHNC_ClearUI);
        }
        private void OpenUINaChi(object[] param)
        {
            if (!NullHelper.IsObjectIsNull(_UIMahjongNachi))
            {
                CloseUINaChi(param);
            }
            _UIMahjongNachi = LoadUISelf<UIMahjongNachi>(PrefabPathDefine.GangHouNaChiUI, new StructUIAnchor(), false);
            if (NullHelper.IsObjectIsNull(_UIMahjongNachi))
            {
                return;
            }

            _UIMahjongNachi.RefreshUI();

        }
        private void CloseUINaChi(object[] param)
        {
            if (NullHelper.IsObjectIsNull(_UIMahjongNachi))
            {
                return;
            }
            CloseUISelf<UIMahjongNachi>(_UIMahjongNachi);
        }
        private void OpenUINaChiChoose(object[] param)
        {
            if (!NullHelper.IsObjectIsNull(_UIMahjongNaChiChoose))
            {
                CloseUINaChiChoose(param);
            }
            _UIMahjongNaChiChoose = LoadUISelf<UIMahjongNaChiChoose>(PrefabPathDefine.GangHouNaChiChooseUI, new StructUIAnchor(), false);
            if (NullHelper.IsObjectIsNull(_UIMahjongNaChiChoose))
            {
                return;
            }
            _UIMahjongNaChiChoose.RefreshUI();
        }
        private void CloseUINaChiChoose(object[] param)
        {
            if (NullHelper.IsObjectIsNull(_UIMahjongNaChiChoose))
            {
                return;
            }
            CloseUISelf<UIMahjongNaChiChoose>(_UIMahjongNaChiChoose);
        }
        private void OpenUINaChiTips(object[] param)
        {
            if (NullHelper.IsObjectIsNull(param))
            {
                return;
            }
            int seatID = (int)param[0];
            if (!NullHelper.IsObjectIsNull(_UIPlayingTips))
            {
                CloseUINaChiTips(param);
            }
            _UIPlayingTips = LoadUISelf<UIPlayingTips>(PrefabPathDefine.PlayingTips, new StructUIAnchor(), false);
            if (NullHelper.IsObjectIsNull(_UIPlayingTips))
            {
                return;
            }

            long userID = MjDataManager.Instance.GetCurrentUserIDBySeatID(seatID);
            string nickName = MemoryData.PlayerData.get(userID).PlayerDataBase.Name;
            string msg = string.Format(UIConstStringDefine.GangHouNaChiTips, nickName);
            _UIPlayingTips.RefreshUI(msg, UIPlayingTipsType.TipsWithThreeMarks, Vector2.zero);
        }
        private void CloseUINaChiTips(object[] param)
        {
            if (NullHelper.IsObjectIsNull(_UIPlayingTips))
            {
                return;
            }
            CloseUISelf<UIPlayingTips>(_UIPlayingTips);
        }

        private void GHNC_ClearUI(object[] param)
        {
            CloseUINaChi(param);
        }


    }
}

public class GangHouNaChiUIModule : SpecialUIModule
{
    public override void AddEvents()
    {

    }
    public override void ReconnectedPreparedUIManager()
    {
        EventDispatcher.FireEvent(MJEnum.GangHouNaChi.GHNC_LogicNotify.ToString());
    }
    public override void ClearUI()
    {
        base.ClearUI();
        EventDispatcher.FireEvent(MJEnum.GangHouNaChi.GHNC_ClearUI.ToString());
    }

}
