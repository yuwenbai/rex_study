/**
 * @Author JEFF
 *
 *
 */

using System;
using System.Collections;
using System.Collections.Generic;

namespace projectQ
{
    public class MjRoom
    {
        public int RoomID;
        private string _roomName;
        public string RoomName
        {
            set { _roomName = CommonTools.Tools_NameRegexPlus(value,string.Empty); }
            get { return _roomName; }
        }
        public string RoomDesc;
        public string CreateTime;
        public int CurMemberNum;
        public int MaxMemberNum;
        public int RegionID;	    // 地区
        public int CityID;
        public string FounderName;  // 创始人名称
        public string PhoneNo;
        public string WeiXinNo;     // 馆主手机号微信号
        public int OnlineNum;	    // 麻将馆在线人数
        public string AdText;       //麻将馆宣言
        public int CommisionMoney;  //麻将馆佣金
        public bool isTuiJian;      //是否推荐麻将馆

        public string BoardID;        //麻将馆的牌匾
        public int RankType;        // 麻将馆排序标记：火1、爆2、促3、金4
        public int AgentID;        // 代理系统ID

        public List<int> roomDeskIDList = new List<int>();  //麻将馆内桌子ID集合


        public static MjRoom ProtoToData(Msg.FMjRoom proto)
        {
            MjRoom data = new MjRoom();
            data.RoomID = proto.RoomID;
            data.RoomName = proto.RoomName;
            data.RoomDesc = proto.RoomDesc;
            data.CreateTime = proto.CreateTime;
            data.CurMemberNum = proto.CurMemberNum;
            data.MaxMemberNum = proto.MaxMemberNum;
            data.RegionID = proto.RegionID;
            data.CityID = proto.CityID;
            data.FounderName = proto.FounderName;
            data.PhoneNo = proto.PhoneNo;
            data.WeiXinNo = proto.WeiXinNo;
            data.OnlineNum = proto.OnlineNum;
            data.isTuiJian = proto.IsTuijian;
            if (string.IsNullOrEmpty(proto.AdText))
                data.AdText = "诚信麻将 诚信娱乐 [FF0000FF]拒绝赌博[-] 线下实体店 xxx市xxx街道";
            else
                data.AdText = proto.AdText;
            data.CommisionMoney = proto.CommisionMoney;
            data.BoardID = proto.BoardID;
            data.RankType = proto.RankType;
            data.AgentID = proto.AgentID;

            return data;
        }
    }

    public class SysMjRoomData
    {
        public enum EnumSort
        {

        }

        private string _mjHallDescInfo;
        public string MjHallDescInfo
        {
            set
            {
                _mjHallDescInfo = value;
                EventDispatcher.FireSysEvent(GEnum.NamedEvent.SysData_MjHall_DescUpdate, _mjHallDescInfo);

            }
            get
            {
                return _mjHallDescInfo;
            }
        }

        //推荐麻将馆列表
        private List<MjRoom> _mjHallListRecommend;
        public List<MjRoom> MjHallListRecommend
        {
            set
            {
                _mjHallListRecommend = value;
                this.AddMjHallMap(_mjHallListRecommend);
                EventDispatcher.FireSysEvent(GEnum.NamedEvent.SysData_MjHall_RecommendResultRsp, _mjHallListRecommend);
            }
            get { return _mjHallListRecommend; }
        }


        //搜索结果麻将馆列表
        private List<MjRoom> _mjHallList;
        public List<MjRoom> MjHallList
        {
            set
            {
                _mjHallList = value;
                this.AddMjHallMap(_mjHallList);
                EventDispatcher.FireSysEvent(GEnum.NamedEvent.SysData_MjHall_SearchResultRsp, _mjHallList);
            }
            get
            {
                return _mjHallList;
            }
        }

        public void AddMjHallListValue(string key, List<MjRoom> values,bool isFinish)
        {
            if (_mjHallList == null)
            {
                _mjHallList = new List<MjRoom>();
            }

            for (int i = 0; i < values.Count; i++)
            {
                bool isHave = false;
                for (int n = 0; n < _mjHallList.Count; n++)
                {
                    if (values[i].RoomID == _mjHallList[n].RoomID)
                        isHave = true;
                }

                if (!isHave)
                    _mjHallList.Add(values[i]);
            }

            this.AddMjHallMap(_mjHallList);
            if (isFinish)
            {
                List<MjRoom> result = new List<MjRoom>();
                if (string.IsNullOrEmpty(key))
                    result = _mjHallList;
                else
                    result = values;
                EventDispatcher.FireSysEvent(GEnum.NamedEvent.SysData_MjHall_SearchResultRsp, result);
            }
        }

        /// <summary>
        /// 全部麻将馆
        /// </summary>
        private Dictionary<int, MjRoom> MjHallMap = new Dictionary<int, MjRoom>();

        /// <summary>
        /// 添加麻将馆数据
        /// </summary>
        /// <param name="info"></param>
        public void AddMjHallMap(MjRoom info)
        {
            if (MjHallMap.ContainsKey(info.RoomID))
                MjHallMap[info.RoomID] = info;
            else
                MjHallMap.Add(info.RoomID, info);
        }

        private void AddMjHallMap(List<MjRoom> list)
        {
            for (int i = 0; i < list.Count; i++)
            {
                AddMjHallMap(list[i]);
            }
        }

        /// <summary>
        /// 取得麻将馆数据 根据麻将馆ID
        /// </summary>
        /// <param name="hallId"></param>
        public MjRoom GetMjHallById(int hallId)
        {
            if (MjHallMap.ContainsKey(hallId))
            {
                return MjHallMap[hallId];
            }
            return null;
        }

        /// <summary>
        /// 取得所有麻将馆
        /// </summary>
        public List<MjRoom> GetMjHallAll()
        {
            if (_mjHallList != null && _mjHallList.Count > 0)
                return _mjHallList;
            return _mjHallListRecommend;
        }

        /// <summary>
        /// 根据ID获取麻将馆内的桌子ID
        /// </summary>
        /// <param name="roomID"></param>
        /// <returns></returns>
        public List<int> GetMjHallDeskList(int roomID)
        {
            return this.GetMjHallById(roomID).roomDeskIDList;
        }

        public void AddOrUpdateHallDeskList(int roomID, List<int> deskID)
        {
            if (deskID != null && deskID.Count > 0)
            {
                for (int i = 0; i < deskID.Count; i++)
                {
                    AddOrUpdateHallDeskList(roomID, deskID[i]);
                }
            }
        }

        public void AddOrUpdateHallDeskList(int roomID, int deskID)
        {
            List<int> deskIDList = this.GetMjHallById(roomID).roomDeskIDList;
            if (!deskIDList.Contains(deskID))
            {
                deskIDList.Add(deskID);
            }
        }

        public void RefreshHallDeskList(int roomID, List<int> deskID)
        {
            List<int> deskIDList = this.GetMjHallById(roomID).roomDeskIDList;
            deskIDList.Clear();
            deskIDList.AddRange(deskID);
        }


        public void RefreshHallDeskList(int roomID, int deskID)
        {
            List<int> deskIDList = this.GetMjHallById(roomID).roomDeskIDList;
            deskIDList.Clear();
            deskIDList.Add(deskID);
        }

        public void RemoveHallDeskList(int roomID, List<int> deskID)
        {
            if (deskID != null && deskID.Count > 0)
            {
                for (int i = 0; i < deskID.Count; i++)
                {
                    RemoveHallDeskList(roomID, deskID[i]);
                }
            }
        }

        public void RemoveHallDeskList(int roomID, int deskID)
        {
            List<int> deskIDList = this.GetMjHallById(roomID).roomDeskIDList;
            if (deskIDList.Contains(deskID))
            {
                deskIDList.Remove(deskID);
            }
        }
        
    }
}
