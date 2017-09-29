

using System;
/**
* @Author JEFF
*
*
*/
using System.Collections;
using System.Collections.Generic;
using Msg;
using UnityEngine;
namespace projectQ
{
    public class UIParlorListModel : UIModelBase
    {
        private UIParlorList UI
        {
            get { return base._ui as UIParlorList; }
        }

        private string searchKey = null;
        private int typeId;
        private List<ParlorItemData> CitySortList = null;
        [HideInInspector]
        public FMjSortType SortType = FMjSortType.FMjSort_RenQi;
        public List<ParlorItemData> priendDataList = null;
        public List<ParlorItemData> searchPriendDataList = null;
        public int cityId;

        public List<ParlorItemData> SetHallList(List<MjRoom> parlor)
        {
            var result = new List<ParlorItemData>(parlor.Count);
            for (int i = 0; i < parlor.Count; ++i)
            {
                ParlorItemData data = new ParlorItemData();
                data.hall = parlor[i];
                data.index = i;
                data.OnClick = UI.OnParlorItemClick;
                result.Add(data);
            }
            CitySortList = result;
            return result;
        }

        ////临时添加的检测城市选单有没有麻将馆的方法，调用比较混乱，可能有风险
        public bool CheckIsHaveRoom(int RegionID)
        {
            for (int i = 0; i < CitySortList.Count; i++)
            {
                int regid = UI.sortCity.cityData.CheckRegionIDByCityID(CitySortList[i].hall.RegionID);
                if (regid == RegionID)
                    return true;
            }

            return false;
        }

        public void OnSendSearch(string searchKey)
        {
            this.searchKey = searchKey;
            EventDispatcher.FireSysEvent(GEnum.NamedEvent.EGetGameData, GEnum.NamedGameData.EC2SMessageRoomSearch, searchKey, cityId, SortType);
            //ModleNetWorker.Instance.FMjRoomSearchReq(searchKey,0, SortType);

        }
        public void OnSendSearch()
        {
            EventDispatcher.FireSysEvent(GEnum.NamedEvent.EGetGameData, GEnum.NamedGameData.EC2SMessageRoomSearch, "", cityId, SortType);
            //ModleNetWorker.Instance.FMjRoomSearchReq("", cityId, SortType);
        }


        private int m_RememberCityID = -1;

        public List<ParlorItemData> RefreshSortList(List<MjRoom> parlor)
        {
            SetHallList(parlor);
            return SetCityId(-1);
        }

        public List<ParlorItemData> SetCityId(int cityID = -1)
        {
            if (m_RememberCityID == -1 && cityID != -1)
                m_RememberCityID = cityID;

            if (m_RememberCityID == -1 && cityID == -1)
                cityID = 0;

            if (cityID == -1 && m_RememberCityID != -1)
                cityID = m_RememberCityID;

            m_RememberCityID = cityID;

            cityId = cityID;

            List<MjRoom> room = MemoryData.MjHallData.MjHallList;
            List<MjRoom> lookRoom = new List<MjRoom>();

            for (int i = 0; i < room.Count; i++)
            {
                int regid = UI.sortCity.cityData.CheckRegionIDByCityID(room[i].RegionID);
                if (regid == cityID || cityId == 0)
                    lookRoom.Add(room[i]);
            }

            priendDataList = SetHallList(lookRoom);
            UI.RefreshFriendMJParlor();
            return priendDataList;
        }
        public void SetSortType(string str)
        {
            switch (str)
            {
                case "综合排行":
                    this.SortType = FMjSortType.FMjSort_RenQi;

                    break;
                case "热度排行"://桌卡销量
                    this.SortType = FMjSortType.FMjSort_ReDu;

                    break;
                case "人气排行"://在线人数
                    this.SortType = FMjSortType.FMjSort_RenQi;

                    break;
                case "推荐排行"://推荐

                    this.SortType = FMjSortType.FMjSort_TuiJian;
                    break;

            }
        }
        public List<ParlorItemData> SortByType()
        {
            List<ParlorItemData> sortList = priendDataList;
            if (sortList == null)
            {
                return null;
            }
            if (SortType == FMjSortType.FMjSort_Null)
            {

            }
            else if (SortType == FMjSortType.FMjSort_RenQi)
            {
              
                ParlorItemData temp = null;
                for (int i = 0; i < sortList.Count; i++)
                {
                    for (int j = 0; j < sortList.Count; j++)
                    {
                        if (sortList[j].hall.OnlineNum < sortList[i].hall.OnlineNum)
                        {
                            temp = sortList[j];
                            sortList[j] = sortList[i];
                            sortList[i] = temp;
                        }
                    }
                }
            }
            else if (SortType == FMjSortType.FMjSort_ReDu)
            {
                ParlorItemData temp = null;
                for (int i = 0; i < sortList.Count; i++)
                {
                    for (int j = 0; j < sortList.Count; j++)
                    {
                        if (sortList[j].hall.CommisionMoney < sortList[i].hall.CommisionMoney)
                        {
                            temp = sortList[j];
                            sortList[j] = sortList[i];
                            sortList[i] = temp;
                        }
                    }
                }
            }
            else if (SortType == FMjSortType.FMjSort_TuiJian)
            {
                List<ParlorItemData> isTuiJian = new List<ParlorItemData>();
                List<ParlorItemData> isntTuiJian = new List<ParlorItemData>();
                foreach (var item in priendDataList)
                {
                    if (item.hall.isTuiJian)
                    {
                        isTuiJian.Add(item);
                    }
                    else
                    {
                        isntTuiJian.Add(item);
                    }
                }

                ParlorItemData temp = null;
                for (int i = 0; i < isTuiJian.Count; i++)
                {
                    for (int j = 0; j < isTuiJian.Count; j++)
                    {
                        if (isTuiJian[j].hall.OnlineNum < isTuiJian[i].hall.OnlineNum)
                        {
                            temp = isTuiJian[j];
                            isTuiJian[j] = isTuiJian[i];
                            isTuiJian[i] = temp;
                        }
                    }
                }
                ParlorItemData temp2 = null;
                for (int i = 0; i < isntTuiJian.Count; i++)
                {
                    for (int j = 0; j < isntTuiJian.Count; j++)
                    {
                        if (isntTuiJian[j].hall.OnlineNum < isntTuiJian[i].hall.OnlineNum)
                        {
                            temp2 = isntTuiJian[j];
                            isntTuiJian[j] = isntTuiJian[i];
                            isntTuiJian[i] = temp2;
                        }
                    }
                }
                sortList.Clear();
                foreach (var item in isTuiJian)
                {
                    sortList.Add(item);
                }
                foreach (var item in isntTuiJian)
                {
                    sortList.Add(item);
                }
            }
            return sortList;

        }
        private void Awake()
        {
            SortType = FMjSortType.FMjSort_RenQi;
        }
        #region override
        protected override GEnum.NamedEvent[] FocusNetWorkData()
        {
            return new GEnum.NamedEvent[]{
                GEnum.NamedEvent.SysData_MjHall_SearchResultRsp
            };
        }

        protected override void OnNetWorkDataCallBack(GEnum.NamedEvent msgEnum, object[] data)
        {
            switch (msgEnum)
            {
                case GEnum.NamedEvent.SysData_MjHall_SearchResultRsp:
                    {
                        
                        if (!string.IsNullOrEmpty(this.searchKey))
                        {
                            List<MjRoom> list = data[0] as List<MjRoom>;
                            if (list != null && list.Count > 0)//MemoryData.MjHallData.MjHallList.Count > 0)
                            {
                                this.searchPriendDataList = this.SetHallList(list);// MemoryData.MjHallData.MjHallList);
                                UI.OnOpenSearch();
                            }
                            else
                            {
                                UI.LoadTip("您搜索的棋牌室不存在");
                            }
                           
                        }
                        else
                        {
                            //UI.LoadTip("请输入正确的内容");
                        
                            this.priendDataList = this.RefreshSortList(MemoryData.MjHallData.MjHallList);
                            UI.RefreshFriendMJParlor();
                        }
                        this.searchKey = "";
                    }
                    break;
            }
        }
        #endregion
    }
}