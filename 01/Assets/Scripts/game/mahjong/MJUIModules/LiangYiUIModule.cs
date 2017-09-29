/**
* @Author Xin.Wang
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
        public void RegisterModuleEvent_LiangYiUIModule()
        {
            _MahjongEventHelper.AddEvent(MJEnum.MjLiangyiEvent.MJLY_AnimShowUI.ToString(), MJLiangYiAnimShow);
            _AnimEventHelper.AddEvent(MJEnum.MjLiangyiEvent.MJLY_AnimShowUI.ToString(), MJLiangYiAnimShow);
            _AnimEventHelper.AddEvent(MJEnum.MjLiangyiEvent.MJLY_AnimUpdateUI.ToString(), MJLYAnimUpdateUI);
            _MahjongEventHelper.AddEvent(GEnum.NamedEvent.EMjControlStataChooseOne, EMjControlStataChooseOne);     //亮一按钮状态
        }

        /// <summary>
        /// 亮一按钮状态
        /// </summary>
        /// <param name="obj"></param>
        private void EMjControlStataChooseOne(object[] obj)
        {
            bool state = (bool)obj[0];
            int chooseIndex = (int)obj[1];
            int chooseCardID = (int)obj[2];

            //set choose btn 
            MjDataManager.Instance.LiangyiUISetChooseState(state, chooseCardID, chooseIndex);
            if (_uiBtnChoose)
            {
                _uiBtnChoose.RefreshBtnStateLiangyi(state);
            }
        }


        private void MJLiangYiAnimShow(object[] vars)
        {
            bool needAnim = (bool)vars[0];

            EnumMjSelectSubType curSelectType = MjDataManager.Instance.MjData.ProcessData.processLiangYi.selectSubType;
            int selfSeatID = MjDataManager.Instance.MjData.curUserData.selfSeatID;

            bool needUpdate = false;
            bool justSelf = false;
            bool needShowResult = false;
            switch (curSelectType)
            {
                case EnumMjSelectSubType.MjSelectSubType_WAIT_NoSelect:
                    {
                        //阶段一
                        needUpdate = true;
                        //show UI
                        _uiBtnChoose = LoadUISelf<UIMahjongBtnChoose>(GameConst.path_MahjongBtnChoose, new StructUIAnchor(), false,
                     EnumMjSpecialCheck.MjBaseCheckType_LiangYi);
                        ShowTipsSpecial(true, EnumMjSpecialCheck.MjBaseCheckType_LiangYi);
                        SetClickState(0, true);
                        SetLiangYiIni();
                    }
                    break;
                case EnumMjSelectSubType.MjSelectSubType_WAIT_Select:
                    {
                        //阶段二 
                        needUpdate = true;
                        justSelf = true;
                        needShowResult = true;

                        //close UI
                        CloseUISelf<UIMahjongBtnChoose>(_uiBtnChoose);
                        ShowTipsSpecial(true, EnumMjSpecialCheck.MjBaseCheckType_LiangYi);
                    }
                    break;
                case EnumMjSelectSubType.MjSelectSubType_RESULT_Select:
                    {
                        //阶段三
                        needShowResult = true;
                        CloseUISelf<UIMahjongBtnChoose>(_uiBtnChoose);
                        ShowTipsSpecial(false, EnumMjSpecialCheck.MjBaseCheckType_LiangYi);
                    }
                    break;
            }

            if (needUpdate)
            {
                List<int> seatIDs = MjDataManager.Instance.GetAllSeatIDCurDesk(true);
                ShowUpdateResult(seatIDs, needAnim);
            }

            if (needShowResult)
            {
                SetResult(justSelf, needAnim);
            }
        }

        private void SetLiangYiIni()
        {
            if (_ui3D)
            {
                _ui3D.SetHandLiangYiIni(4);
            }
        }

        /// <summary>
        /// 更新结果
        /// </summary>
        /// <param name="obj"></param>
        private void MJLYAnimUpdateUI(object[] obj)
        {
            int curSeatID = (int)obj[0];
            int selfSeatID = MjDataManager.Instance.MjData.curUserData.selfSeatID;
            if (curSeatID == selfSeatID)
            {
                //自己逻辑
                SetResult(true, true);
            }
            else
            {
                ShowUpdateResult(new List<int>() { curSeatID }, true);
            }

        }

        private void ShowUpdateResult(List<int> seatIDs, bool needAnim)
        {
            //展示Update 
            int selfSeatID = MjDataManager.Instance.MjData.curUserData.selfSeatID;
            for (int i = 0; i < seatIDs.Count; i++)
            {
                int curSeat = seatIDs[i];
                int curUIID = CardHelper.GetMJUIPosByServerPos(curSeat, selfSeatID);
                bool haveChoose = MjDataManager.Instance.MjData.ProcessData.processLiangYi.CheckChooseState(curSeat);
                if (haveChoose)
                {
                    this.UpdateTipsSpecail(curUIID, EnumMjSpecialCheck.MjBaseCheckType_LiangYi);
                }
            }
        }


        private void SetResult(bool justSelf, bool needAnim)
        {
            List<int> showSeatID = new List<int>();
            int selfSeatID = MjDataManager.Instance.MjData.curUserData.selfSeatID;
            //展示结果
            if (justSelf)
            {
                //show result self
                showSeatID.Add(selfSeatID);
            }
            else
            {
                showSeatID = MjDataManager.Instance.GetAllSeatIDCurDesk(needAnim);
            }

            for (int i = 0; i < showSeatID.Count; i++)
            {
                int curSeat = showSeatID[i];
                int cardID = MjDataManager.Instance.MjData.ProcessData.processLiangYi.GetChooseCard(curSeat);
                if (cardID > 0)
                {
                    int curUISeat = CardHelper.GetMJUIPosByServerPos(curSeat, selfSeatID);
                    if (_ui3D)
                    {
                        _ui3D.SetHandLiangYi(curUISeat, cardID);
                    }
                }
            }
        }

    }
}


public class LiangYiUIModule : SpecialUIModule
{
    public override void AddEvents()
    {

    }

    public override void ReconnectedPreparedUIManager()
    {
        if (MjDataManager.Instance.MjData.ProcessData.processLiangYi.curISReconned)
        {
            //走重连
            EventDispatcher.FireEvent(MJEnum.MjLiangyiEvent.MJLY_AnimShowUI.ToString(), false);
        }
    }
}
