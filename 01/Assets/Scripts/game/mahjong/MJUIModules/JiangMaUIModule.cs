using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using projectQ;

namespace projectQ
{
    public partial class MahjongUIManager
    {
        public void RegisterModuleEvent_JiangMaUIModule()
        {
            _AnimEventHelper.AddEvent(MJEnum.MjJiangMaEvent.MJJM_AnimationOne.ToString(), MJJiangMaAnimationOne);
            _AnimEventHelper.AddEvent(MJEnum.MjJiangMaEvent.MJJM_AnimationTwo.ToString(), MJJiangMaAnimationTwo);

        }

        private void MJJiangMaAnimationOne(object[] pro)
        {
            EnumMjOpenMaType openType = (EnumMjOpenMaType)pro[0];
            List<MjHorse> horseResult = (List<MjHorse>)pro[1];

            _uiOpenMa = LoadUISelf<UIOpenMa>(GameConst.path_MahjongOpenMa, new StructUIAnchor(), false, openType, horseResult);
        }

        private void MJJiangMaAnimationTwo(object[] pro)
        {
            EnumMjOpenMaType openType = (EnumMjOpenMaType)pro[0];
            List<int> showSeatID = (List<int>)pro[1];

            if (showSeatID != null && showSeatID.Count > 0)
            {
                int selfSeatID = MjDataManager.Instance.MjData.curUserData.selfSeatID;
                for (int i = 0; i < showSeatID.Count; i++)
                {
                    int showUISeatID = CardHelper.GetMJUIPosByServerPos(showSeatID[i], selfSeatID);
                    //showFly
                    Transform transP = GetHeadTransform(showUISeatID);

                    LoadUISelf<UIInnerGameAnim>(GameConst.path_MahjongInnerGameAnim, new StructUIAnchor(), false, openType, showUISeatID, transP.position);
                }
            }
        }
    }
}

public class JiangMaUIModule : SpecialUIModule
{
    public override void AddEvents()
    {

    }

    public override void ReconnectedPreparedUIManager()
    {
    }
}
