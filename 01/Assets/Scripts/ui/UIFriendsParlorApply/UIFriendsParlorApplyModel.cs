/**
 * @Author JEFF
 *
 *
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace projectQ
{
    public class UIFriendsParlorApplyModel : UIModelBase {
        private UIFriendsParlorApply UI
        {
            get { return _ui as UIFriendsParlorApply; }
        }
        //发送好友麻将馆介绍
        public void OnSendRoomDescReq()
        {
            EventDispatcher.FireSysEvent(GEnum.NamedEvent.EGetGameData, GEnum.NamedGameData.EC2SMessageRoomDesc);
            //ModleNetWorker.Instance.FMjRoomDescInfoReq();
        }

        public void OnSearchHall(string searchKey)
        {
            EventDispatcher.FireSysEvent(GEnum.NamedEvent.EGetGameData, GEnum.NamedGameData.EC2SMessageRoomSearch,searchKey,0,Msg.FMjSortType.FMjSort_RenQi);
            //ModleNetWorker.Instance.FMjRoomSearchReq(searchKey,0, Msg.FMjSortType.FMjSort_Null);
        }

        #region override
        protected override GEnum.NamedEvent[] FocusNetWorkData()
        {
            return new GEnum.NamedEvent[]
            {
                GEnum.NamedEvent.SysData_MjHall_DescUpdate,
                GEnum.NamedEvent.SysData_MjHall_SearchResultRsp,
            };
        }
        protected override void OnNetWorkDataCallBack(GEnum.NamedEvent msgEnum, object[] data)
        {
            switch(msgEnum)
            {
                case GEnum.NamedEvent.SysData_MjHall_DescUpdate:
                    UI.RefreshInfo(data[0] as string);
                    break;
                case GEnum.NamedEvent.SysData_MjHall_SearchResultRsp:
                    UI.ReceiveSearchResult(data[0] as List<MjRoom>);
                    break;
            }
        }
        #endregion

        private void Start()
        {
            string info = MemoryData.MjHallData.MjHallDescInfo;
            if (string.IsNullOrEmpty(info))
                OnSendRoomDescReq();
            else
                UI.RefreshInfo(info);
        }

    }
}
