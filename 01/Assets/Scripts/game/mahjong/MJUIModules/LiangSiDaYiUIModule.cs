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
        public void RegisterModuleEvent_LiangSiDaYiUIModule()
        {
            _MahjongEventHelper.AddEvent(MJEnum.MjLiangSiDaYiEvent.LSDY_UIShow3D.ToString(), MJLiangSiDaYiAnimShow);
        }


        private void MJLiangSiDaYiAnimShow(object[] obj)
        {
            int selfSeatID = MjDataManager.Instance.MjData.curUserData.selfSeatID;
            List<int> seatID = MjDataManager.Instance.GetAllSeatIDCurDesk(false);
            List<int> cardList = new List<int>();
            for (int i = 0; i < seatID.Count; i++)
            {
                int curSeatID = seatID[i];
                int curUISeatID = CardHelper.GetMJUIPosByServerPos(curSeatID, selfSeatID);
                bool canClick = false;
                if (curSeatID == selfSeatID)
                {
                    canClick = !MjDataManager.Instance.LSDY_CheckPutState();
                }
                cardList = MjDataManager.Instance.LSDY_GetCardList(curSeatID);

                if (_ui3D != null)
                {
                    _ui3D.SetLiangSiDaYi(curUISeatID, cardList, canClick);
                }
            }
        }
    }
}



public class LiangSiDaYiUIModule : SpecialUIModule
{
    public override void AddEvents()
    {

    }

    public override void ReconnectedPreparedUIManager()
    {
        bool contain = MjDataManager.Instance.LSDY_CheckPlayOprate();
        if (contain)
        {
            //EventDispatcher.FireEvent(MJEnum.MjLiangSiDaYiEvent.LSDY_UIShow3D.ToString(), false);
        }
    }

}
