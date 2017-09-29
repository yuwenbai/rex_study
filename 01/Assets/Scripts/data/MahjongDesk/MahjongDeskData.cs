/**
 * @Author Xin.Wang
 *
 *
 */

using System.Collections;
using System.Collections.Generic;

namespace projectQ
{
    #region MjDeskInfo
    public class MjDeskInfo
    {
        public int deskID;                                              //桌子ID
        public int bouts;                                               //当前进行局数
        public int rounds;                                              //总共进行局数
        public int maxDouble;                                           //封顶番数
        public int mjGameType;                                   //当前玩法类型
        public int mjGameConfigId;                              //玩法唯一ID
        public MahjongPlay.MahjongPlayType MjType;              //玩法类型
        //public List<int> mjRulerList;                       //规则列表
        public List<RulerItem> mjRulerSelected;

        public bool viewScore;                                          //是否允许查看战绩
        public string DeskAdvert;                                       //桌子当前的宣言、说明
        public long ownerUserID;
        public List<long> playerIdList = new List<long>();       //当前桌玩家ID信息
        public MjDeskInfo()
        { }

        public MjDeskInfo(int gameConfigId, int bouts, int maxDouble, int gameType, List<int> rulerList, int deskID, int rounds, bool view, string memo,List<RulerItem> rulerSelected,long OwnerUserID)
        {
            this.mjGameConfigId = gameConfigId;
            this.bouts = bouts;
            this.maxDouble = maxDouble;
            //this.mjRulerList = rulerList;
            this.mjGameType = gameType;
            this.deskID = deskID;
            this.rounds = rounds;
            this.viewScore = view;
            this.DeskAdvert = memo;
            this.mjRulerSelected = rulerSelected;
            this.ownerUserID = OwnerUserID;
        }

        public MjDeskInfo(MahjongPlay play,int deskId)
        {
            var selectPlay = MemoryData.MahjongPlayData.GetSelected(play);
            this.mjGameConfigId = play.ConfigId;
            this.bouts = 0;
            this.maxDouble = 0;
            //this.mjRulerList = selectPlay.SelectedList;
            this.mjGameType = play.PlayId;
            this.deskID = deskId;
            this.rounds = 0;
            this.viewScore = selectPlay.ViewScore;
            this.DeskAdvert = string.Empty;
            this.mjRulerSelected = selectPlay.SelectedItemList;
        }
        public void SetPlayerInfo(long userID)
        {
            if (!playerIdList.Contains(userID))
            {
                playerIdList.Add(userID);
            }
        }

      
        public long GetPlayerIdBySeatId(int seatID)
        {
            long userID = 0;
            if (seatID < 1 || seatID > 4)
            {
                QLoger.ERROR("SetPlayerInfo SeatID is error ! + seatID = {0}", seatID);
                return 0;
            }
            if (playerIdList == null)
            {
                return 0;
            }
            for (int i = 0; i < playerIdList.Count; i++)
            {
                if (seatID == MemoryData.PlayerData.get(playerIdList[i]).playerDataMj.seatID)
                {
                    userID = playerIdList[i];
                }
            }
            return userID;
        }

        //public MjPlayerInfo GetPlayerInfoBySeat(int seatID)
        //{
        //    if (seatID < 1 || seatID > 4)
        //    {
        //        QLoger.ERROR("SetPlayerInfo SeatID is error ! + seatID = {0}", seatID);
        //        return null;
        //    }
        //    if (playerIdList == null)
        //    {
        //        return null;
        //    }
        //    MjPlayerInfo info = null;
        //    for (int i = 0; i < playerIdList.Count; i++)
        //    {
        //        if (seatID == MemoryData.PlayerData.get(playerIdList[i]).playerDataMj.seatID)
        //        {
        //            info = MemoryData.PlayerData.get(playerIdList[i]).playerDataMj;
        //        }
        //    }

        //    return info;
        //}

        //public MjPlayerInfo[] GetAllPlayerInfo()
        //{
        //    if (playerIdList == null)
        //    {
        //        return null;
        //    }
        //    MjPlayerInfo[] infos = new MjPlayerInfo[4];
        //    for (int i = 0; i < playerIdList.Count; i++)
        //    {
        //        infos[i] = MemoryData.PlayerData.get(playerIdList[i]).playerDataMj;
        //    }
        //    return infos;
        //}

        public long[] GetAllPlayerID()
        {
            if (playerIdList == null)
            {
                return null;
            }
            long[] playerIDList = playerIdList.ToArray();
            return playerIDList;
        }


        public void DeletePlayerInfo(int seatID)
        {
            if (seatID < 1 || seatID > 4)
            {
                QLoger.ERROR("SetPlayerInfo SeatID is error ! + seatID = {0}", seatID);
                return;
            }
            if (playerIdList == null)
            {
                return;
            }

            for (int i = 0; i < playerIdList.Count; i++)
            {
                if (seatID == MemoryData.PlayerData.get(System.Convert.ToInt64(playerIdList[i])).playerDataMj.seatID)
                {
                    playerIdList.RemoveAt(i);
                }
            }

        }
    }

    #endregion



    public class MahjongDeskData
    {
        private Dictionary<string, MjDeskInfo> _dicDeskData = new Dictionary<string, MjDeskInfo>();
        private MjDeskInfo _tempDeskData = null;
        public MjDeskInfo TempDeskData
        {
            set { _tempDeskData = value; }
            get { return _tempDeskData; }
        }

        //根据桌子ID 获取单一deskInfo
        public MjDeskInfo GetOneDeskInfo(int deskID)
        {
            return GetOneDeskInfo(deskID.ToString());
        }

        public MjDeskInfo GetOneDeskInfo(string deskID)
        {
            MjDeskInfo info = null;
            _dicDeskData.TryGetValue(deskID, out info);
            return info;
        }


        //根据桌子ID添加或更新桌子info
        public void AddOrUpdateDeskInfo(int deskID, MjDeskInfo deskInfo)
        {
            AddOrUpdateDeskInfo(deskID.ToString(), deskInfo);
        }

        public void AddOrUpdateDeskInfo(string deskID, MjDeskInfo deskInfo)
        {
            if (_dicDeskData.ContainsKey(deskID))
            {
                _dicDeskData[deskID] = deskInfo;
            }
            else
            {
                _dicDeskData.Add(deskID, deskInfo);
            }
        }

        //根据桌子ID删除deskinfo
        public void RemoveDeskInfo(int deskID)
        {
            RemoveDeskInfo(deskID.ToString());
        }

        public void RemoveDeskInfo(string deskID)
        {
            if (_dicDeskData.ContainsKey(deskID))
            {
                _dicDeskData.Remove(deskID);
            }
        }
    }


    #region 内存数据
    public partial class MKey
    {
        public const string USER_MAHJONG_DESK_DATA = "USER_MAHJONG_DESK_DATA";
    }

    public partial class MemoryData
    {
        static public MahjongDeskData DeskData
        {
            get
            {
                MahjongDeskData itemData = MemoryData.Get<MahjongDeskData>(MKey.USER_MAHJONG_DESK_DATA);
                if (itemData == null)
                {
                    itemData = new MahjongDeskData();
                    MemoryData.Set(MKey.USER_MAHJONG_DESK_DATA, itemData);
                }
                return itemData;

            }
        }

    }


    #endregion

}

