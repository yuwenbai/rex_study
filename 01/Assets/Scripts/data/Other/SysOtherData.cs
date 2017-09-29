

using System;
/**
* @Author YQC
*
*
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace projectQ
{
    /// <summary>
    /// 牌匾信息
    /// </summary>
    public class BoardData : IComparable<BoardData>
    {
        private static string Dir = "Texture/Tex_Board/";
        public string BoardKey;
        public string BoardFileName;
        public Color BoardColor;
        public Texture BoardTexture;

        public BoardData(string key,string fileName,Color col)
        {
            this.BoardKey = key;
            this.BoardFileName = fileName;
            this.BoardColor = col;
        }
        public string GetPath()
        {
            return Dir + BoardFileName;
        }

        public int CompareTo(BoardData other)
        {
            return this.BoardKey.CompareTo(other.BoardKey);
        }
    }

    /// <summary>
    /// 走马灯消息
    /// </summary>
    public class NoticeMsg
    {
        public int NoticeID;
        public Msg.NoticeTypeDef NoticeType;
        public int RegionID;    // 地区ID，-1为全国
        public int GameID;      // 麻将玩法，-1为全部
        public string Content;  // 公告内容

        public static NoticeMsg ProtoToData(Msg.NoticeMsg msg)
        {
            NoticeMsg result = new NoticeMsg();
            result.NoticeID = msg.NoticeID;
            result.NoticeType = msg.NoticeType;
            result.RegionID = msg.RegionID;
            result.GameID = msg.GameID;
            result.Content = msg.Content;
            return result;
        }
    }

    /// <summary>
    /// 作弊信息
    /// </summary>
    public class CheatInfo
    {
        public enum EnumWarningType
        {
            Ip = 1,
            Location = 2,
            LocationEmpty = 3,
        }
        public long UserId;

        public Dictionary<EnumWarningType, List<long>> CheatMap = null;

        //public bool IsCheat(List<long> ids)
        //{
        //    if (ids == null)
        //        return (IpCheatList != null && IpCheatList.Count > 0 ) || (LocationCheatList != null && LocationCheatList.Count > 0);
        //    else
        //    {
        //        for (int i = 0; i < ids.Count; i++)
        //        {
        //            if(ids[i] > 0)
        //            {
        //                if(IpCheatList != null && IpCheatList.IndexOf(ids[i]) > 0)
        //                {
        //                    return true;
        //                }
        //            }
        //        }
        //    }
        //    return false;
        //}

        public CheatInfo(long userId)
        {
            this.UserId = userId;
            CheatMap = new Dictionary<EnumWarningType, List<long>>();
        }

        public void SetUserIds(int type,List<long> userIds)
        {
            EnumWarningType enu = EnumWarningType.Ip;
            try
            {
                enu = (EnumWarningType)type;
            }
            catch (System.Exception ex)
            {
                DebugPro.Log(DebugPro.EnumLog.MemoryData, "作弊数据错误 EnumWarningType转换异常: type=",type);
            }
            //判断下与自己是否有关

            if(!this.CheatMap.ContainsKey(enu))
            {
                this.CheatMap.Add(enu, new List<long>());
            }

            for (int i = 0; i < userIds.Count; i++)
            {
                if (userIds[i] != UserId)
                    CheatMap[enu].Add(userIds[i]);
            }
            if (CheatMap.ContainsKey(enu) && CheatMap[enu].Count == 0)
                CheatMap.Remove(enu);
        }

        public List<long> GetCheatListByType(EnumWarningType type)
        {
            if (CheatMap == null || CheatMap.Count == 0 || !CheatMap.ContainsKey(type)) return null;

            return CheatMap[type];
        }
    }

    public class SysOtherData
    {
        private List<BoardData> BoardList;
        private BoardData CurrBoard;
        
        #region 牌匾
        public void AddBoard(BoardData data)
        {
            if (BoardList == null)
            {
                BoardList = new List<BoardData>();
            }
            BoardList.Add(data);
            BoardList.Sort();
        }
        public List<BoardData> GetBoardList()
        {
            if (BoardList == null)
                BoardList = new List<BoardData>();
            return BoardList;
        }
        public void SetCurrBoard(string key)
        {
            var board = BoardList.Find((temp) => {
                return temp.BoardKey == key;
            });
            if (board == null)
                board = BoardList[0];
            CurrBoard = board;
        }
        public BoardData GetCurrBoard()
        {
            if (CurrBoard == null && BoardList != null && BoardList.Count > 0)
                CurrBoard = BoardList[0];
            return CurrBoard;
        }
        public BoardData GetBoard(string key)
        {
            var board =this.BoardList.Find((temp) => {
                return temp.BoardKey == key;
            });
            if (board == null)
                board = this.BoardList[0];
            return board;
        }
        /// <summary>
        /// 切换下一张牌匾
        /// </summary>
        /// <returns></returns>
        public BoardData ChangeNextBoar()
        {
            int index = BoardList.IndexOf(CurrBoard);
            if (index == -1) index = 0;
            if(index == BoardList.Count - 1)
            {
                CurrBoard = BoardList[0];
            }
            else
            {
                CurrBoard = BoardList[index + 1];
            }
            return CurrBoard;
        }

        //Todo yqc 牌匾数据临时初始化
        public void InitBoard()
        {
            BoardList = new List<BoardData>();

            for (int i = 1;i <= 4; ++i)
            {
                var board = new BoardData(i.ToString(), "Board" + i, Color.blue);
                AddBoard(board);
            }
        }
        #endregion

        #region 走马灯
        private List<NoticeMsg> _noticeMsgList;
        public List<NoticeMsg> NoticeMsgList
        {
            get
            {
                if (_noticeMsgList == null)
                    _noticeMsgList = new List<NoticeMsg>();
                return _noticeMsgList;
            }
        }

        /// <summary>
        /// 添加走马灯信息
        /// </summary>
        /// <param name="data"></param>
        public void AddNoticeMsg(NoticeMsg data)
        {
            var index = NoticeMsgList.FindIndex((findData) =>
            {
                return findData.NoticeID == data.NoticeID;
            });
            if (index == -1)
                NoticeMsgList.Add(data);
            else
                NoticeMsgList[index] = data;
        }

        /// <summary>
        /// 取得走马灯信息 根据地区ID
        /// </summary>
        public List<NoticeMsg> GetNoticeListByRegionId(int regionID, Msg.NoticeTypeDef type = Msg.NoticeTypeDef.Notice_System)
        {
            List<NoticeMsg> result = new List<NoticeMsg>();
            for (int i = 0; i < NoticeMsgList.Count; i++)
            {
                if((NoticeMsgList[i].RegionID == regionID) && NoticeMsgList[i].NoticeType == type)
                {
                    result.Add(NoticeMsgList[i]);
                }
            }
            return result;
        }

        /// <summary>
        /// 根据玩法id获取跑马灯
        /// </summary>
        /// <param name="gameId"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public List<NoticeMsg> GetNoticeListByGameId(int gameId, Msg.NoticeTypeDef type = Msg.NoticeTypeDef.Notice_FMjRoom)
        {
            List<NoticeMsg> result = new List<NoticeMsg>();
            for (int i = 0; i < NoticeMsgList.Count; i++)
            {
                if ((NoticeMsgList[i].GameID == gameId || NoticeMsgList[i].GameID == -1) && NoticeMsgList[i].NoticeType == type)
                {
                    result.Add(NoticeMsgList[i]);
                }
            }
            return result;
        }

        #endregion

        //#region GPS提示
        //private bool IsCheckGps = false;
        //public void CheckGPSServer()
        //{
        //    if(!IsCheckGps)
        //    {
        //        IsCheckGps = true;
        //        GPSManager.Instance.GPSServeRequest();
        //    }
        //}
        //#endregion
        /* ping
        #region ping
        public int PingMaxCount = 3;
        public class PingClass {
            private int pingCount = 0;
            public int PingCount
            {
                set {
                    pingCount = value;
                }
                get {
                    return pingCount;
                }
            }
        }
        private PingClass pingData = new PingClass();
        public void SetPingCount(int count)
        {
            lock(pingData)
            {
                pingData.PingCount = count;
            }
        }
        public void AddPingCount()
        {
            lock (pingData)
            {
                ++pingData.PingCount;
            }
        }
        public int GetPingCount()
        {
            lock(pingData)
            {
                return pingData.PingCount;
            }
        }
        
        //上次Ping时间
        private float lastPingTime;
        public float LastPingTime
        {
            set { lastPingTime = value; }
            get { return lastPingTime; }
        }
        //网路延迟
        private float netDelay;
        public float NetDelay
        {
            set { netDelay = value; }
            get { return netDelay; }
        }
        //更新网络延迟
        public void UpdateNetDelay()
        {
            NetDelay = Time.realtimeSinceStartup - LastPingTime;
        }

        //注册ping
        public void RegistPing()
        {
            GameDelegateCache.Instance.InvokeMethodEvent -= Ping;
            GameDelegateCache.Instance.InvokeMethodEvent += Ping;
        }
        public void UnRegistPing()
        {
            GameDelegateCache.Instance.InvokeMethodEvent -= Ping;
        }
        private static void Ping()
        {
            if(MemoryData.OtherData.GetPingCount() > MemoryData.OtherData.PingMaxCount)
            {
                MemoryData.OtherData.UnRegistPing();
                MemoryData.OtherData.SetPingCount(0);
                LoginEnterNetWork.Instance.Reconnect();
                return;
            }

            if (MemoryData.OtherData != null && Time.realtimeSinceStartup - MemoryData.OtherData.LastPingTime >= 5.0f)
            {
                //QLoger.ERROR("发送ping");
                MemoryData.OtherData.LastPingTime = Time.realtimeSinceStartup;
                ModelNetWorker.Instance.PingCmd();
                MemoryData.OtherData.AddPingCount();
            }
        }
        #endregion
        */
        #region 作弊信息
        private Dictionary<long, CheatInfo> _cheateInfoMap = new Dictionary<long, CheatInfo>();

        private Dictionary<CheatInfo.EnumWarningType, List<long>> _cheatMap = new Dictionary<CheatInfo.EnumWarningType, List<long>>();

        private Dictionary<CheatInfo.EnumWarningType, List<List<long>>> _cheatMapNew = new Dictionary<CheatInfo.EnumWarningType, List<List<long>>>();

        [Obsolete("2017/07/16 已经过时 请使用 GetMyCheateInfoUserIdsNew")]
        public List<long> GetMyCheateInfoUserIds(CheatInfo.EnumWarningType type)
        {
            if(_cheatMap != null && _cheatMap.ContainsKey(type))
            {
                return _cheatMap[type];
            }
            return null;
        }
        public List<List<long>> GetMyCheateInfoUserIdsNew(CheatInfo.EnumWarningType type)
        {
            if (_cheatMapNew != null && _cheatMapNew.ContainsKey(type))
            {
                return _cheatMapNew[type];
            }
            return null;
        }
        public void AddCheatInfoByNetWork(List<Msg.WarningPlayerData> playerList)
        {
            _cheatMapNew.Clear();
            for (int i = 0; i < playerList.Count; i++)
            {
                var play = playerList[i];
                MemoryData.OtherData.AddCheatInfoByNetWork(play);

                CheatInfo.EnumWarningType enu = (CheatInfo.EnumWarningType)play.WarningType;
                if (!_cheatMapNew.ContainsKey(enu))
                {
                    _cheatMapNew.Add(enu, new List<List<long>>());
                }
                var tempMap = _cheatMapNew[enu];
                var newList = new List<long>();
                tempMap.Add(newList);
                for (int j = 0; j < play.UserList.Count; j++)
                {
                    newList.Add(play.UserList[j]);
                }
            }
            //for (int i = 0; i < playerList.Count; i++)
            //{
            //    var play = playerList[i];
            //    MemoryData.OtherData.AddCheatInfoByNetWork(play);

            //    //if (play.UserList.IndexOf(MemoryData.UserID) >= 0)
            //    //{
            //    //    //排除自己后 至少有一人位置信息为0
            //    //    if(play.WarningType == 3 && play.UserList.Count <= 1)
            //    //    {
            //    //        continue;
            //    //    }
            //    //    //排除自己后 至少有两人位置相近 或 ip相同
            //    //    if (play.WarningType != 3 && play.UserList.Count <= 2)
            //    //    {
            //    //        continue;
            //    //    }
            //    //}

            //    CheatInfo.EnumWarningType enu = (CheatInfo.EnumWarningType)play.WarningType;
            //    if(!_cheatMap.ContainsKey(enu))
            //    {
            //        _cheatMap.Add(enu, new List<long>());
            //    }
            //    var list = _cheatMap[enu];

            //    for (int j = 0; j < play.UserList.Count; j++)
            //    {
            //        long userId = play.UserList[j];
            //        if (list.IndexOf(userId) == -1 /*&& userId != MemoryData.UserID*/)
            //            list.Add(userId);
            //    }
            //}
            EventDispatcher.FireSysEvent(GEnum.NamedEvent.SysData_CheatInfo_Update);
        }
        /// <summary>
        /// 添加作弊信息通过网络数据
        /// </summary>
        /// <param name="data"></param>
        private void AddCheatInfoByNetWork(Msg.WarningPlayerData data)
        {
            if(data != null && data.UserList != null && data.UserList.Count > 0)
            {
                for (int i = 0; i < data.UserList.Count; i++)
                {
                    AddCheatInfo(data.UserList[i],data.UserList,data.WarningType);
                }
            }
        }

        private void AddCheatInfo(long userId,List<long> UserList,int type)
        {
            if (!_cheateInfoMap.ContainsKey(userId))
                _cheateInfoMap.Add(userId, new CheatInfo(userId));

            var data = _cheateInfoMap[userId];

            data.SetUserIds(type, UserList);
        }

        /// <summary>
        /// 取得作弊信息 根据用户ID
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public CheatInfo GetCheatInfo(long userId)
        {
            if (_cheateInfoMap.ContainsKey(userId))
                return _cheateInfoMap[userId];
            return null;
        }

        /// <summary>
        /// 取得多个作弊信息 根据牌桌ID
        /// </summary>
        /// <param name="deskId"></param>
        public List<CheatInfo> GetCheatInfosByDeskId(int deskId,bool isExcludeSelf)
        {
            List<CheatInfo> result = new List<CheatInfo>();

            if(deskId > 0)
            {
                var deskInfo = MemoryData.DeskData.GetOneDeskInfo(deskId);
                var playerIds = deskInfo.GetAllPlayerID();
                for (int i = 0; i < playerIds[i]; i++)
                {
                    if(playerIds[i] > 0 && (!isExcludeSelf || playerIds[i] != MemoryData.UserID))
                    {
                        var cheatData = this.GetCheatInfo(playerIds[i]);
                        if (cheatData != null)
                            result.Add(cheatData);
                    }
                }
            }
            return result;
        }

        //public Dictionary<CheatInfo.EnumWarningType,List<long>> GetCheatInfosByDeskId(int deskId)
        //{
        //    var result = new Dictionary<CheatInfo.EnumWarningType, List<long>>();
        //    var cheatInfos = GetCheatInfosByDeskId(deskId, true);
        //    for (int i = 0; i < cheatInfos.Count; i++)
        //    {
        //        foreach(var item in cheatInfos[i].CheatMap)
        //        {
        //            if(!result.ContainsKey(item.Key))
        //            {
        //                result.Add(item.Key, new List<long>());
        //            }

        //        }
        //    }
        //    return result;
        //}

        /// <summary>
        /// 清空作弊信息
        /// </summary>
        public void ClearCheatInfo()
        {
            _cheatMap.Clear();
            _cheatMapNew.Clear();
            _cheateInfoMap.Clear();
        }
        #endregion
    }
    #region 内存数据
    public partial class MKey
    {
        public const string SYS_OTHER_DATA = "SYS_OTHER_DATA";
    }

    public partial class MemoryData
    {
        static public SysOtherData OtherData
        {
            get
            {
                SysOtherData itemData = MemoryData.Get<SysOtherData>(MKey.SYS_OTHER_DATA);
                if (itemData == null)
                {
                    itemData = new SysOtherData();
                    MemoryData.Set(MKey.SYS_OTHER_DATA, itemData);
                }
                return itemData;
            }
        }
    }
    #endregion
}