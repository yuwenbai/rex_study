using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using projectQ;

namespace projectQ
{
    public partial class MahjongUIManager
    {
        public void RegisterModuleEvent_DuanMenUIModule()
        {
            _MahjongEventHelper.AddEvent(MJEnum.MjDuanMenEvent.MjDM_ShowUI.ToString(), MjDuanMenShow);
        }

        private void MjDuanMenShow(object[] param)
        {
            List<int> list = MjDataManager.Instance.GetAllSeatIDCurDesk(false);
            for (int i = 0; i < list.Count; i++)
            {
                MahjongPlayType.MjPlayerOutCardLimit data = MjDataManager.Instance.GetOutCardLimitBySeatID(list[i]);

                if(data!= null)
                {
                    int uiSeatID = CardHelper.GetMJUIPosByServerPos(list[i], MjDataManager.Instance.MjData.curUserData.selfSeatID);
                    _desk.ShowDuanMen(uiSeatID, MjDataManager.Instance.GetLockTypeName(data.lackType));
                }
            }
        }
    }
}

public class DuanMenUIModule : SpecialUIModule
{
    public override void AddEvents()
    {

    }

    public override void ReconnectedPreparedUIManager()
    {
        EventDispatcher.FireEvent(MJEnum.MjDuanMenEvent.MjDM_LogicNotify.ToString());
    }
}
