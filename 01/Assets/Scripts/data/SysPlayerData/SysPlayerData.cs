/**
 * @Author Xin.Wang
 *
 *
 */

using System.Collections;
using System.Collections.Generic;


namespace projectQ
{
    public class PlayerDataBase : System.IComparable<PlayerDataBase>
    {
        public long UserID;

        #region 用户基本信息
        private string _name;
        public string Name
        {
            set
            {
                _name = CommonTools.Tools_NameRegexPlus(value, string.Empty);
                _name = CommonTools.Substring(_name, 10);
            }
            get
            {
                if (_name == null)
                    return string.Empty;
                return _name;
            }
        }
        public int Level;
        public int RegionID;
        public string PhoneNo;
        public int Money;
        public int Diamond;
        public int BindTickets;         // 绑定桌卡：只可自己消耗，不可转让
        public int Tickets;             // 普通桌卡：正常使用、转让
        private string _headUrl;
        public string HeadURL
        {
            set
            {
                DebugPro.Log(DebugPro.EnumLog.HeadUrl, "头像用户基本信息 ", value);
                System.Diagnostics.Debug.Assert(string.IsNullOrEmpty(value));
                if (value != _headUrl && value.Length > 0)
                    _headUrl = value;
            }
            get
            {
                return string.IsNullOrEmpty(_headUrl) ? Name : _headUrl;
            }
        }

        public string RealName;         //真实姓名
        public string IDCard;           //身份证号
        public int LoginTime;           //上次登录时间
        public bool TodayFirstLogin;    //是否今日登录
        public int lotteryTimes;        //抽奖次数 0-可以抽 抽一次累加
        public string InviteCode;       //自己的邀请码
        public string BeInviteeCode;    //被邀请的邀请人的邀请码
        public string BeInviteeName;    //被邀请的邀请人的名字
        public bool isAddFriend;        //是否被添加好友
        public int DeskViewRecordDeskId; //回放记录的当前DeskId
        public string Account;           //玩家账号

        private int _Sex = 1;           //1:男  2:女
        public int Sex
        {
            set { _Sex = value == 0 ? 1 : value; }
            get { return _Sex; }
        }
        
        public string _clientIp;        //客户端IP
        public string ClientIp
        {
            set
            {
                _clientIp = value;
            }
            get { return _clientIp; }
        }
        public float Latitude;      //维度信息
        public float Longitude;     //精度信息

        //总桌卡数量 通常用这个
        public int TotalTicket
        {
            get { return BindTickets + Tickets; }
        }
        #endregion

        #region 麻将馆信息
        private int _bindRoomID; // 关联的麻将馆ID
        public int BindRoomID
        {
            set
            {
                if (_ownerRoomID > 0)
                {
                    _bindRoomID = 0;
                }
                else
                {
                    _bindRoomID = value;
                }
            }
            get
            {
                return _bindRoomID;
            }
        }     
        public int OwnerDeskID;     // 桌主自己的桌ID
        private int _ownerRoomID;     // 馆主自己的麻将馆ID
        public int OwnerRoomID
        {
            set
            {
                if (value > 0)
                    _bindRoomID = 0;
                _ownerRoomID = value;
            }
            get { return _ownerRoomID; }
        }
        public int TotalBouts;      // 历史总盘数
        public int WinBouts;        // 历史获胜记录数
        public int huPaiType;       // 最大牌型
        public List<BestMjRecord> MjRecords;
        public List<MjPlayTimes> MjTimes;
        public int BindRoomState;       // 绑定状态 1 未关联 2已关联 3解除关联中
        public int TerminateRoomTime;   // 解除绑定时间
        private bool isOnline = true;           //是否在线
        public bool IsOnline
        {
            get { return isOnline; }
            set
            {
                isOnline = value;
            }
        }

        public int MyAgentID;      // 棋牌室馆长自己的代理ID
        public int AgentID;      // 玩家的推广人ID


        #endregion

        /// <summary>
        /// 是否有麻将馆
        /// </summary>
        /// <returns></returns>
        public bool IsHaveMjRoom()
        {
            return BindRoomID > 0 || OwnerRoomID > 0;
        }

        /// <summary>
        /// 取得麻将馆ID 不管是否绑定还是馆主
        /// </summary>
        public int MjRoomId
        {
            get
            {
                return OwnerRoomID > 0 ? OwnerRoomID : BindRoomID;
            }
        }

        public PlayerDataBase(long userId)
        {
            this.UserID = userId;
        }
        public PlayerDataBase()
        {

        }

        public virtual int CompareTo(PlayerDataBase other)
        {
            int result = this.Name.CompareTo(other.Name);
            return -1 * result;
        }


    }

    public class MjPlayerInfo
    {
        public long userID;                            //用户UserID
        public int score;                               //用户当前分数
        public int seatID;                              //用户座位ID
        public bool hasReady;                           //用户是否准备
        public int winBouts;                            //用户赢的局数 

        public Dictionary<int, BestMjRecord> bestRecordDic = new Dictionary<int, BestMjRecord>();

        #region 属性
        public int SeatID
        {
            get
            {
                if (seatID > 0 && seatID < 5)
                {
                    return seatID;
                }
                else
                {
                    DebugPro.DebugError("seatID error:", seatID);
                    return -1;
                }
            }
        }
        #endregion

        public void SetBestRecord(BestMjRecord record)
        {
            int type = record.mjType;
            if (bestRecordDic.ContainsKey(type))
            {
                bestRecordDic[type] = record;
            }
            else
            {
                bestRecordDic.Add(type, record);
            }
        }


        public MjPlayerInfo()
        { }

        public MjPlayerInfo(long userID, int score, int seatID, bool hasReady)
        {
            this.userID = userID;
            //this.headUrl = headUrl;
            //this.level = level;
            //this.nickName = nickName;
            this.score = score;
            this.seatID = seatID;
            this.hasReady = hasReady;
        }
    }

    public class ResultPlayerInfo
    {
        public int seatID;                          //当前玩家座位号
        public int Score;                           //当前玩家总的积分
        public int WinBouts;                        //当前玩家总的赢局


        public ResultPlayerInfo(int seatID, int score, int winbouts)
        {
            this.seatID = seatID;
            this.Score = score;
            this.WinBouts = winbouts;
        }
    }

    public class FriendPlayerInfo
    {
        public int hotPoint;    //热度
        public int playTime;    //与我玩的局数
        public int state;       //好友状态 1 离线 2游戏中 3牌桌等待中 4空闲
        public bool IsTemp = true;     //临时好友 需要同意才能

        public FriendPlayerInfo()
        {
            IsTemp = true;
        }
    }


    public class ClienteleDataState
    {
        private e_ClienteleState m_ClienteleState;
        public e_ClienteleState ClienteleState
        {
            get
            {
                return m_ClienteleState;
            }
        }
        public void SetGameState(int state)
        {
            m_ClienteleState = (e_ClienteleState)state;
        }

        public enum e_ClienteleState
        {
            outLine = 0,
            onLine,
            isPlay,
        }

        private int m_CurDeskID;
        public int deskID
        {
            get
            {
                return m_CurDeskID;
            }
        }
        public void SetCurDeskID(int id)
        {
            m_CurDeskID = id;
        }
    }


    public class PlayerDataModel
    {
        #region PlayerDataBase
        private PlayerDataBase _playerDataBase = new PlayerDataBase();
        public PlayerDataBase PlayerDataBase
        {
            get
            {
                return _playerDataBase;
            }
        }

        public void SetPlayerDataBase(PlayerDataBase dataBase)
        {
            _playerDataBase = dataBase;
        }

        #endregion

        #region PlayerDataMj
        public MjPlayerInfo playerDataMj = null;
        public void SetMjPlayerInfoData(MjPlayerInfo playerData, long userID, string headUrl, int level, string nickName, int sex, string ClientIp)
        {
            PlayerDataBase.UserID = System.Convert.ToInt64(userID);
            PlayerDataBase.HeadURL = headUrl;
            PlayerDataBase.Level = level;
            PlayerDataBase.Name = nickName;
            PlayerDataBase.Sex = sex;
            PlayerDataBase.ClientIp = ClientIp;
            playerDataMj = playerData;

            System.Text.StringBuilder sb = new System.Text.StringBuilder("=============SetMjPlayerInfoData=============").Append(userID).AppendLine();
            sb.Append("HeadURL___").Append(headUrl).AppendLine();
            sb.Append("Level___").Append(level).AppendLine();
            sb.Append("Name___").Append(nickName).AppendLine();
            sb.Append("Sex___").Append(sex).AppendLine();
            DebugPro.Log(DebugPro.EnumLog.MemoryData, sb.ToString());
        }
        #endregion

        //data friend
        #region 好友
        public FriendPlayerInfo _playerDataFriend = null;
        public FriendPlayerInfo FriendPlayerInfo
        {
            get
            {
                if (_playerDataFriend == null)
                    _playerDataFriend = new FriendPlayerInfo();
                return _playerDataFriend;
            }
        }

        #endregion

        //data other

        #region 客户状态信息，馆主查看客户用
        public ClienteleDataState _clienteleDataState = null;
        public ClienteleDataState ClienteleDataState
        {
            get
            {
                if (_clienteleDataState == null)
                    _clienteleDataState = new ClienteleDataState();
                return _clienteleDataState;
            }
        }
        #endregion

        #region Proto
        /// <summary>
        /// 设置用户麻将馆信息
        /// </summary>
        /// <param name="rsp"></param>
        public void SetMjRoomPlayerInfo(Msg.FMjRoomPlayerInfo rsp)
        {
            PlayerDataBase.BindRoomID = rsp.BindRoomID;
            PlayerDataBase.OwnerRoomID = rsp.OwnerRoomID;
            PlayerDataBase.OwnerDeskID = rsp.OwnerDeskID;
            PlayerDataBase.WinBouts = rsp.WinBouts;
            PlayerDataBase.TotalBouts = rsp.TotalBouts;
            PlayerDataBase.huPaiType = rsp.HuPaiType;
            PlayerDataBase.BindRoomState = rsp.BindRoomState;
            PlayerDataBase.TerminateRoomTime = rsp.TerminateRoomTime;
            PlayerDataBase.MyAgentID = rsp.MyAgentID;

            //麻将馆信息本来就不带有性别不能在这里强制设置 改为数据类
            //PlayerDataBase.Sex = 1; //JEFF 这里创建的用户信息没有性别 ， 先给男，保证不错

            if (rsp.MjTimes != null)
            {
                PlayerDataBase.MjTimes = new List<MjPlayTimes>();
                List<Msg.MjPlayTimes> msgPlayTimes = rsp.MjTimes.MjTimes;
                if (msgPlayTimes != null && msgPlayTimes.Count > 0)
                {
                    for (int i = 0; i < msgPlayTimes.Count; i++)
                    {
                        MjPlayTimes data = new MjPlayTimes(msgPlayTimes[i].MjType, msgPlayTimes[i].WinBouts, msgPlayTimes[i].TotalBouts);
                        PlayerDataBase.MjTimes.Add(data);
                    }
                }
            }
            if (rsp.MjRecords != null)
            {
                PlayerDataBase.MjRecords = new List<BestMjRecord>();
                for (int i = 0; i < rsp.MjRecords.Count; i++)
                {
                    List<Msg.MjPai> originalCodeList = rsp.MjRecords[i].MjList;
                    List<MjPai> mjCodeList = new List<MjPai>();

                    if (originalCodeList != null && originalCodeList.Count > 0)
                    {
                        for (int j = 0; j < originalCodeList.Count; j++)
                        {
                            MjPai code = new MjPai(originalCodeList[j].MjCode, originalCodeList[j].CodeType);
                            mjCodeList.Add(code);
                        }
                    }
                    List<Msg.MjPai> originalSpecialList = rsp.MjRecords[i].MjSpecialList;
                    List<MjPai> specialList = new List<MjPai>();
                    if (originalSpecialList != null && originalSpecialList.Count > 0)
                    {
                        for (int j = 0; j < originalSpecialList.Count; j++)
                        {
                            MjPai specialCode = new MjPai(originalSpecialList[j].MjCode, originalSpecialList[j].CodeType);
                        }
                    }
                    var bestRecord = new BestMjRecord(rsp.MjRecords[i].MjType, rsp.MjRecords[i].OddsCount, mjCodeList, specialList, rsp.MjRecords[i].scoreChange, rsp.MjRecords[i].PaiType);
                    PlayerDataBase.MjRecords.Add(bestRecord);
                }
            }


            System.Text.StringBuilder sb = new System.Text.StringBuilder("=============用户麻将相关信息更新=============").Append(rsp.UserID).AppendLine();
            sb.Append("BindRoomID___").Append(rsp.BindRoomID).AppendLine();
            sb.Append("OwnerRoomID___").Append(rsp.OwnerRoomID).AppendLine();
            sb.Append("OwnerDeskID___").Append(rsp.OwnerDeskID).AppendLine();
            sb.Append("WinBouts___").Append(rsp.WinBouts).AppendLine();
            sb.Append("TotalBouts___").Append(rsp.TotalBouts).AppendLine();
            sb.Append("HuPaiType___").Append(rsp.HuPaiType).AppendLine();
            sb.Append("BindRoomState___").Append(rsp.BindRoomState).AppendLine();
            sb.Append("TerminateRoomTime___").Append(rsp.TerminateRoomTime).AppendLine();
            sb.AppendLine("============= 用户麻将相关信息更新结束 =============");
            DebugPro.Log(DebugPro.EnumLog.MemoryData, sb.ToString());
        }
        /// <summary>
        /// 设置用户基本信息
        /// </summary>
        /// <param name="userInfo"></param>
        public void SetUserInfo(Msg.UserInfo userInfo)
        {
            //^\w+?\s([a-z]+).*?\s(\w+) =.*
            //PlayerDataBase.$2 = userInfo.$2;
            PlayerDataBase.UserID = userInfo.UserID;
            PlayerDataBase.Name = userInfo.Name;
            PlayerDataBase.Level = userInfo.Level;
            PlayerDataBase.TotalBouts = userInfo.TotalBouts;
            PlayerDataBase.WinBouts = userInfo.WinBouts;
            PlayerDataBase.RegionID = userInfo.RegionID;
            PlayerDataBase.PhoneNo = userInfo.PhoneNo;
            PlayerDataBase.Money = userInfo.Money;
            PlayerDataBase.Diamond = userInfo.Diamond;
            PlayerDataBase.BindTickets = userInfo.BindTickets;
            PlayerDataBase.Tickets = userInfo.Tickets;
            PlayerDataBase.HeadURL = userInfo.HeadURL;
            PlayerDataBase.RealName = userInfo.RealName;
            PlayerDataBase.IDCard = userInfo.IDCard;
            PlayerDataBase.LoginTime = userInfo.LoginTime;
            PlayerDataBase.TodayFirstLogin = userInfo.TodayFirstLogin;
            PlayerDataBase.lotteryTimes = userInfo.LotteryTimes;
            PlayerDataBase.InviteCode = userInfo.InviteCode;            
            PlayerDataBase.BeInviteeCode = userInfo.BeInviteeCode;
            PlayerDataBase.BeInviteeName = userInfo.BeInviteeName;
            PlayerDataBase.Sex = userInfo.Sex;
            PlayerDataBase.ClientIp = userInfo.ClientIp;
            PlayerDataBase.AgentID = userInfo.AgentID;
            PlayerDataBase.Account = userInfo.Account;

            if (userInfo.UserID == MemoryData.UserID)
            {
                var gpsData = GPSManager.Instance.GetNewGpsData();
                PlayerDataBase.Longitude = gpsData == null ? 0f : gpsData.Longitude;
                PlayerDataBase.Latitude = gpsData == null ? 0f : gpsData.Latitude;
            }
            else
            {
                PlayerDataBase.Longitude = userInfo.PosX;
                PlayerDataBase.Latitude = userInfo.PosZ;
            }

            PlayerDataBase.DeskViewRecordDeskId = -1;



            System.Text.StringBuilder sb = new System.Text.StringBuilder("=============用户信息更新=============").Append(userInfo.UserID).AppendLine();
            sb.Append("Name___").Append(userInfo.Name).AppendLine();
            sb.Append("Level___").Append(userInfo.Level).AppendLine();
            sb.Append("TotalBouts___").Append(userInfo.TotalBouts).AppendLine();
            sb.Append("WinBouts___").Append(userInfo.WinBouts).AppendLine();
            sb.Append("RegionID___").Append(userInfo.RegionID).AppendLine();
            sb.Append("PhoneNo___").Append(userInfo.PhoneNo).AppendLine();
            sb.Append("Money___").Append(userInfo.Money).AppendLine();
            sb.Append("Diamond___").Append(userInfo.Diamond).AppendLine();
            sb.Append("BindTickets___").Append(userInfo.BindTickets).AppendLine();
            sb.Append("Tickets___").Append(userInfo.Tickets).AppendLine();
            sb.Append("HeadURL___").Append(userInfo.HeadURL).AppendLine();
            sb.Append("RealName___").Append(userInfo.RealName).AppendLine();
            sb.Append("IDCard___").Append(userInfo.IDCard).AppendLine();
            sb.Append("LoginTime___").Append(userInfo.LoginTime).AppendLine();
            sb.Append("TodayFirstLogin___").Append(userInfo.TodayFirstLogin).AppendLine();
            sb.Append("LotteryTimes___").Append(userInfo.LotteryTimes).AppendLine();
            sb.Append("InviteCode___").Append(userInfo.InviteCode).AppendLine();
            sb.Append("BeInviteeCode___").Append(userInfo.BeInviteeCode).AppendLine();
            sb.Append("Sex___").Append(userInfo.Sex).AppendLine();
            sb.Append("ClientIp___").Append(userInfo.ClientIp).AppendLine();
            sb.Append("PosX___").Append(userInfo.PosX).AppendLine();
            sb.Append("PosZ___").Append(userInfo.PosZ).AppendLine();
            sb.AppendLine("============= 用户信息更新结束 =============");
            DebugPro.Log(DebugPro.EnumLog.MemoryData, sb.ToString());
        }


        public void SetProtoMyFriend(Msg.MyFriend rsp)
        {
            PlayerDataBase.Name = rsp.Name;
            PlayerDataBase.HeadURL = rsp.HeadURL;
            PlayerDataBase.WinBouts = rsp.WinBouts;
            PlayerDataBase.OwnerDeskID = rsp.DeskID;

            FriendPlayerInfo.state = (int)rsp.State;
            FriendPlayerInfo.hotPoint = rsp.Credits;
            FriendPlayerInfo.IsTemp = rsp.IsTemp;
            System.Text.StringBuilder sb = new System.Text.StringBuilder("=============好友数据更新=============").Append(rsp.UserID).AppendLine();
            sb.Append("Name:").Append(rsp.Name).AppendLine();
            sb.Append("HeadURL:").Append(rsp.HeadURL).AppendLine();
            sb.Append("WinBouts:").Append(rsp.WinBouts).AppendLine();
            sb.Append("DeskID:").Append(rsp.DeskID).AppendLine();
            sb.Append("State:").Append(rsp.State).AppendLine();
            sb.Append("Credits:").Append(rsp.Credits).AppendLine();
            sb.Append("IsTemp:").Append(rsp.IsTemp).AppendLine();
            sb.AppendLine("============= 好友数据更新结束 =============");
            DebugPro.Log(DebugPro.EnumLog.MemoryData, sb.ToString());
        }
        #endregion

    }



    public class SysPlayerData
    {
        private IDictionary<string, PlayerDataModel> _sysPlayerDic = new Dictionary<string, PlayerDataModel>();

        public IDictionary<string, PlayerDataModel> SysPlayerDic
        {
            get
            {
                return _sysPlayerDic;
            }
        }

        /// <summary>
        /// 初始化个人信息
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        public PlayerDataModel SetMyPlayer(long userID)
        {
            _myPlayerModel = get(userID);
            return _myPlayerModel;
        }
        private PlayerDataModel _myPlayerModel = null;
        /// <summary>
        /// 快速获取个人信息
        /// </summary>
        public PlayerDataModel MyPlayerModel
        {
            get
            {
                return _myPlayerModel;
            }
        }

        /// <summary>
        /// 个人UserID
        /// </summary>
        public long MyUserID
        {
            get
            {
                return _myPlayerModel == null ? 0 : _myPlayerModel.PlayerDataBase.UserID;
            }
        }



        /// <summary>
        /// 获取用户信息(不存在默认new)
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        public PlayerDataModel get(long userID)
        {
            return get(userID.ToString());
        }

        /// <summary>
        /// 获取用户信息(不存在默认new)
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        public PlayerDataModel get(string userID)
        {
            PlayerDataModel model = null;
            if (!_sysPlayerDic.ContainsKey(userID))
            {
                model = new PlayerDataModel();
                long userId;
                if (long.TryParse(userID, out userId))
                {
                    model.PlayerDataBase.UserID = userId;
                    _sysPlayerDic.Add(userID, model);
                }
            }
            else
            {
                model = _sysPlayerDic[userID];
            }
            return model;
        }


        /// <summary>
        /// 检查用户信息(不存在不默认new)
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        public PlayerDataModel CheckModel(long userID)
        {
            return CheckModel(userID.ToString());
        }
        /// <summary>
        /// 检查用户信息(不存在不默认new)
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        public PlayerDataModel CheckModel(string userID)
        {
            PlayerDataModel model = null;
            _sysPlayerDic.TryGetValue(userID, out model);
            return model;
        }


    }


    public partial class MKey
    {
        public const string SYS_PLAYER_DATA = "SYS_PLAYER_DATA";
    }

    public partial class MemoryData
    {
        static public SysPlayerData PlayerData
        {
            get
            {
                SysPlayerData itemData = MemoryData.Get<SysPlayerData>(MKey.SYS_PLAYER_DATA);
                if (itemData == null)
                {
                    itemData = new SysPlayerData();
                    MemoryData.Set(MKey.SYS_PLAYER_DATA, itemData);
                }
                return itemData;
            }
        }
    }


}


