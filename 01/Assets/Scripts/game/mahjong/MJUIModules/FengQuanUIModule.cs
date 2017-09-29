/**
 * @Author HaiLong.Zhang
 *
 *
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using projectQ;
using MahjongPlayType;
namespace projectQ
{
    public partial class MahjongUIManager
    {
        public void RegisterModuleEvent_FengQuanUIModule()
        {
            _MahjongEventHelper.AddEvent(MJEnum.MjFengQuanEvent.MJFQ_ShowUI.ToString(), LoadFengQuan);
            _MahjongEventHelper.AddEvent(MJEnum.MjFengQuanEvent.MJFQ_ClearUI.ToString(), RefreshFengQuan);
            _AnimEventHelper.AddEvent(MJEnum.MjFengQuanEvent.MJFQ_ShowUI.ToString(), LoadFengQuan);
        }


        private void LoadFengQuan(object[] obj)
        {
            int type = -1;
            int deskID = -1;
            MjDataManager.Instance.FengQuanGetData(out type, out deskID);
            SetFengQuanDeskShow(type, deskID);
        }


        private void SetFengQuanDeskShow(int identify, int deskID)
        {
            if (_desk != null)
            {
                _desk.ShowFengQuanIdentifyling(identify, deskID);
            }
        }

        private void RefreshFengQuan(object[] obj)
        {
            if (_desk != null)
            {
                _desk.RefreshFengQuanIdentifyling();
            }
        }
    }

    public class FengQuanUIModule : SpecialUIModule
    {
        public override void AddEvents()
        {

        }

        public override void ReconnectedPreparedUIManager()
        {
            EventDispatcher.FireEvent(MJEnum.MjFengQuanEvent.MJFQ_ShowUI.ToString());
        }

        public override void ClearUI()
        {
            EventDispatcher.FireEvent(MJEnum.MjFengQuanEvent.MJFQ_ClearUI.ToString());
        }
    }
}
