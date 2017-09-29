/**
* @Author Xin.Wang
* 游戏固有的基本流程表现层
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
        public void RegisterModuleEvent_BasicViewModule()
        {
            _AnimEventHelper.AddEvent(MJEnum.ProcessBasicEnum.PROBASIC_Roll_LogictoView.ToString(), AnimRoll);                                              //转色子
            _AnimEventHelper.AddEvent(MJEnum.ProcessBasicEnum.PROBASIC_SPECIAL_LogicToView.ToString(), AnimOpenSpecial);                                    //定混
            _AnimEventHelper.AddEvent(MJEnum.ProcessBasicEnum.PROBASIC_SPECIALShowCard_LogicToView.ToString(), PROBASIC_SPECIALShowCard_LogicToView);       //定混UI展示
        }

        #region 色子
        /// <summary>
        /// 摇色子
        /// </summary>
        /// <param name="vars"></param>
        public void AnimRoll(object[] vars)
        {
            List<int> rollList = (List<int>)vars[0];
            int seatID = (int)vars[1];

            if (rollList != null && rollList.Count > 0)
            {
                _static.PlayRoll(rollList, null);
                EventDispatcher.FireEvent(MJEnum.MusicEnum.ME_PlaySoundFX.ToString(), GEnum.SoundEnum.desk_shaizi);
            }
        }

        #endregion

        #region 定混

        public void AnimOpenSpecial(object[] vars)
        {
            MjStandingPlateData standingData = MjDataManager.Instance.GetCurOpenSpecialStanding();

            if (standingData != null)
            {
                int offset = standingData.standingOffset;
                int mjCode = standingData.standingMjCode;
                bool canGet = standingData.standingIsCanget;
                bool isFromBegin = standingData.standingIsFromBegin;

                if (offset >= 0)
                {
                    SetOpenAcardInCode(offset, true, mjCode, canGet, isFromBegin);
                }

                AnimOpenSpecialCall();
            }
        }


        private void AnimOpenSpecialCall()
        {
            EnumMjSpecialType specialType = MjDataManager.Instance.GetCurOpenSpecialType();
            List<int> changeCode = MjDataManager.Instance.GetCurOpenSpecialChangeList();

            if (specialType != EnumMjSpecialType.Null)
            {
                List<int> uiSeatList = MjDataManager.Instance.GetAllUiSeatCurDesk(false);
                for (int i = 0; i < uiSeatList.Count; i++)
                {
                    _ui3D.SetCardAfterSpecial(uiSeatList[i], specialType, changeCode, true);
                }

                int curDesk = MjDataManager.Instance.MjData.curUserData.selfDeskID;
                int gameType = MemoryData.DeskData.GetOneDeskInfo(curDesk).mjGameType;

                _uiHun = LoadUISelf<UIMahjongHun>(GameConst.path_MahjongHun, new StructUIAnchor(), false, specialType, changeCode, gameType, 2.0f);
            }
        }


        private void PROBASIC_SPECIALShowCard_LogicToView(object[] vars)
        {
            bool needAnim = (bool)vars[0];

            EnumMjSpecialType specialType = MjDataManager.Instance.GetCurOpenSpecialType();
            List<int> changeCode = MjDataManager.Instance.GetCurOpenSpecialChangeList();

            if (specialType != EnumMjSpecialType.Null && changeCode != null && changeCode.Count > 0)
            {
                int curDesk = MjDataManager.Instance.MjData.curUserData.selfDeskID;
                int gameType = MemoryData.DeskData.GetOneDeskInfo(curDesk).mjGameType;

                int specialTypeNum = (int)specialType;
                if (specialType == EnumMjSpecialType.Hun ||
                    specialType == EnumMjSpecialType.Hun_Caishen ||
                    specialType == EnumMjSpecialType.Hun_Laizi ||
                    specialType == EnumMjSpecialType.Hun_JinPai)
                {
                    specialTypeNum = 1;
                }

                string showCode = CardHelper.GetHunStrName(specialTypeNum, gameType);
                SetHun(changeCode, showCode, needAnim);
            }
        }

        #endregion
    }
}


public class MjProcessBasicView : BaseUIModule
{
    public override void AddEvents()
    {



    }

}
