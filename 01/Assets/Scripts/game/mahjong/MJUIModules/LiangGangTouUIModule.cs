using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using projectQ;

namespace projectQ
{
    public partial class MahjongUIManager
    {
        public void RegisterModuleEvent_LiangGangTouUIModule()
        {
            _AnimEventHelper.AddEvent(MJEnum.MjLiangGangTouEvent.MJLGT_ShowUi.ToString(), MjLiangGangTouShow);
            _MahjongEventHelper.AddEvent(MJEnum.MjLiangGangTouEvent.MJLGT_CloseUI.ToString(), MjLiangGangTouClose);

        }

        private void MjLiangGangTouShow(object[] param)
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

            var data = MjDataManager.Instance.GetMjLiangGangTouData();
            if (data != null)
            {
                _UIMahjongNachi.RefreshUI(data.cardList);
            }
            else
            {
                CloseUINaChi(param);
            }
        }

        private void MjLiangGangTouClose(object[] param)
        {
            CloseUINaChi(param);
        }

    }
}

public class LiangGangTouUIModule : SpecialUIModule
{
    public override void AddEvents()
    {

    }

    public override void ReconnectedPreparedUIManager()
    {
        if (MjDataManager.Instance.CheckPlayDataOprateContain(MjPlayOprateType.OPRATE_LIANGGANGTOU))
        {
            EventDispatcher.FireEvent(MJEnum.MjLiangGangTouEvent.MJLGT_LogicNotify.ToString());
        }
    }

    public override void ClearUI()
    {
        EventDispatcher.FireEvent(MJEnum.MjLiangGangTouEvent.MJLGT_CloseUI.ToString());
    }
}