/**
* @Author Xin.Wang
* 胡漂 跟漂的UI模块
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
        public void RegisterModuleEvent_HuPiaoGenPiaoUIModule()
        {
            _MahjongEventHelper.AddEvent(MJEnum.MjHuPiaoGenPiaoEvent.HPGP_LogicToUIEvent.ToString(), HPGP_LogicToUIEvent);
            //_AnimEventHelper.AddEvent(MJEnum.MjHuPiaoGenPiaoEvent.HPGP_LogicToUIAnim.ToString(), HPGP_LogicToUIAnim);
            // _MahjongEventHelper.AddEvent(GEnum.NamedEvent.EMjControlClickOpaction, HPGP_UIClickEvent);           //点击操作按钮
        }


        private void HPGP_UIClickEvent(object[] vars)
        {
            EnumMjOpAction opaction = (EnumMjOpAction)vars[0];

            if (opaction == EnumMjOpAction.MjOp_HuPiaoGenPiao)
            {

            }
        }


        /// <summary>
        /// 表现逻辑层
        /// 刷新UI展示
        /// </summary>
        /// <param name="vars"></param>
        private void HPGP_LogicToUIEvent(object[] vars)
        {
            bool needAnim = (bool)vars[0];
            if (needAnim)
            {
                int selfSeatID = MjDataManager.Instance.MjData.curUserData.selfSeatID;
                int curShowSeat = (int)vars[1];
                int uiID = CardHelper.GetMJUIPosByServerPos(curShowSeat, selfSeatID);
                ClearControll();
                ShowTipsBase(new List<int>() { uiID }, EnumMjOpAction.MjOp_HuPiaoGenPiao, 0, ConstDefine.Mj_Anim2d_BaseTips);
            }

            //refresh number
            HPGP_UIToDeskRefresh();
        }

        /// <summary>
        /// 表现动画层
        /// </summary>
        /// <param name="vars"></param>
        private void HPGP_LogicToUIAnim(object[] vars)
        {
            int seatID = (int)vars[0];

        }


        /// <summary>
        /// 刷新
        /// </summary>
        private void HPGP_UIToDeskRefresh()
        {
            List<int> seatIDList = MjDataManager.Instance.GetAllSeatIDCurDesk(false);
            int selfSeatID = MjDataManager.Instance.MjData.curUserData.selfSeatID;
            for (int i = 0; i < seatIDList.Count; i++)
            {
                int curSeatID = seatIDList[i];
                int curUISeat = CardHelper.GetMJUIPosByServerPos(curSeatID, selfSeatID);
                int curValue = MjDataManager.Instance.GetHuPiaoGenPiaoValue(curSeatID);

                HPGP_UIToDeskSetValue(curUISeat, curValue);
            }
        }


        private void HPGP_UIToDeskSetValue(int uiSeatID, int value)
        {
            if (value == 0)
            {
                return;
            }

            if (_desk != null)
            {
                _desk.XiaPaoXiaPao(uiSeatID, EnumMjSpecialCheck.MjBaseCheckType_XiaPiao, value, false, true);
            }
        }
    }


}


public class HuPiaoGenPiaoUIModule : SpecialUIModule
{
    public override void AddEvents()
    {

    }

    public override void ReconnectedPreparedUIManager()
    {
        if (MjDataManager.Instance.CheckHuPiaoGenPiaoData())
        {
            //检测当前有该玩法
            EventDispatcher.FireEvent(MJEnum.MjHuPiaoGenPiaoEvent.HPGP_LogicToUIEvent.ToString(), false);
        }
    }

}
