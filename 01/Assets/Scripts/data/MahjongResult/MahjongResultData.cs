/**
 * @Author Xin.Wang
 *
 *
 */

using System.Collections;
using System.Collections.Generic;


namespace projectQ
{
    public class GameResultPlayer
    {
        public long userID;                        //当前玩家userID
        public string nickName;                     //当前玩家昵称
        public string HeadUrl;                      //当前玩家头像

        public int seatID;                          //当前玩家座位号
        public int Score;                           //当前玩家总的积分
        public int WinBouts;                        //当前玩家总的赢局


        public GameResultPlayer(long userID, string nickName, string headUrl)
        {
            this.userID = userID;
            this.nickName = nickName;
            this.HeadUrl = headUrl;
            DebugPro.LogWarning("头像 GameResultPlayer " + headUrl);
        }

        public void SetResultData(int seatID, int score, int winbouts)
        {
            this.seatID = seatID;
            this.Score = score;
            this.WinBouts = winbouts;
        }
    }

    public class GameResultCostData
    {
        public int seatID;
        public int costCardNum;                 //扣除房卡的数量（0不显示）
        public int joinRoomTime;                //加入房间时间
    }

    public enum EnumGameResultType
    {
        NormalType = 0,                 //房主付卡模式
        WinPayType,                     //赢家付卡模式
    }

    public class MjDaYingJiaTimeData
    {
        public GameResultCostData data;

        public string nickName;
        public int seatID;
        public bool isDaYingJia;
    }


    public class GameResult
    {
        public int deskID;                          //房间ID （唯一标识）
        public int gameType;                        //游戏类型 ID 对应多语言 
        public int gameTypeSub;                     //游戏类型 索引ID
        public int oddsCount;                       //封顶番数
        public int showType;                        //展示类型 1：普通（展示点炮以及胡等字样） 2：血流血战只展示分数

        public long ownerUserID;                    //桌主
        public int recordTime;                      //开局日期
        public int maxBouts;                        //最大局数
        public int showState;                       //显示类型 0 未读 , 1 已读 , 2 删除

        public List<MjBureauDetialInfo> bureauDetailList = new List<MjBureauDetialInfo>();
        public List<MjTitleInfo> titleList = new List<MjTitleInfo>();               //玩家总积分信息以及称号信息
        public List<GameResultPlayer> resultPlayerList = new List<GameResultPlayer>();
        public Dictionary<int, GameResultCostData> resultCostDic = new Dictionary<int, GameResultCostData>();       //结算玩家扣卡信息(seatID为Key）
        public EnumGameResultType resultType = EnumGameResultType.NormalType;

        public Dictionary<int, MjDaYingJiaTimeData> seeDaYingJiaDict = new Dictionary<int, MjDaYingJiaTimeData>();           //大结算点击查看显示

        public void AddOrUpdateCostData(GameResultCostData dataItem)
        {
            if (dataItem.costCardNum < 0)
            {
                dataItem.costCardNum = -dataItem.costCardNum;
            }
            int curSeatID = dataItem.seatID;
            if (resultCostDic.ContainsKey(curSeatID))
            {
                resultCostDic[curSeatID] = dataItem;
            }
            else
            {
                resultCostDic.Add(curSeatID, dataItem);
            }

            AddOrUpdateDaYingJiaDict(curSeatID, dataItem);
        }

        private void AddOrUpdateDaYingJiaDict(int curSeatID, GameResultCostData dataItem)
        {
            if (dataItem != null)// && data.titleList.Count > 0)
            {
                var data = GetTitleInfoBySeat(curSeatID);
                bool isDYJ = resultCostDic.ContainsKey(curSeatID) && data.titleList.Count > 0;
                GameResultPlayer resultData = null;
                for (int i = 0; i < resultPlayerList.Count; i++)
                {
                    if (resultPlayerList[i].seatID == curSeatID)
                    {
                        resultData = resultPlayerList[i];
                        break;
                    }
                }

                if (resultData == null)
                    return;
                MjDaYingJiaTimeData mjData = null;
                if (!seeDaYingJiaDict.ContainsKey(curSeatID))
                {
                    mjData = new MjDaYingJiaTimeData();
                }
                else
                {
                    mjData = seeDaYingJiaDict[curSeatID];
                }

                mjData.nickName = resultData.nickName;
                mjData.seatID = curSeatID;
                mjData.data = dataItem;
                mjData.isDaYingJia = isDYJ;
                seeDaYingJiaDict[curSeatID] = mjData;
            }
        }

        public void SetGameResultType(int resultType)
        {
            this.resultType = (EnumGameResultType)resultType;
        }

        public int selfSeatID;                                                  //当局游戏玩家个人的座位号 

        public GameResult(int deskID, int gametype, int gameTypeSub, int odds, int showtype, List<MjBureauDetialInfo> bureauInfo, List<MjTitleInfo> titleInfo, int selfSeatID)
        {
            SetDataBase(deskID, gametype, gameTypeSub, odds, showtype, bureauInfo, titleInfo, selfSeatID);
        }


        public void UpdateGameResult(int deskID, int gametype, int gameTypeSub, int odds, int showtype, List<MjBureauDetialInfo> bureauInfo, List<MjTitleInfo> titleInfo, int selfSeatID)
        {
            SetDataBase(deskID, gametype, gameTypeSub, odds, showtype, bureauInfo, titleInfo, selfSeatID);
        }


        private void SetDataBase(int deskID, int gametype, int gameTypeSub, int odds, int showtype, List<MjBureauDetialInfo> bureauInfo, List<MjTitleInfo> titleInfo, int selfSeatID)
        {
            this.deskID = deskID;
            this.gameType = gametype;
            this.gameTypeSub = gameTypeSub;
            this.oddsCount = odds;
            this.showType = showtype;
            this.bureauDetailList = bureauInfo;
            this.titleList = titleInfo;
            this.selfSeatID = selfSeatID;
        }


        public void SetPlayerInfo(List<GameResultPlayer> playerInfo, long userID)
        {
            int selfSeatID = -1;
            GameResultPlayer[] playerArray = new GameResultPlayer[5];
            for (int i = 0; i < playerInfo.Count; i++)
            {
                GameResultPlayer curInfo = playerInfo[i];
                playerArray[curInfo.seatID] = curInfo;

                if (curInfo.userID == userID)
                {
                    selfSeatID = curInfo.seatID;
                }

                resultPlayerList.Add(curInfo);
            }

            List<string> playerNameList = new List<string>();
            for (int i = 1; i < playerArray.Length; i++)
            {
                if (playerArray[i] != null)
                {
                    playerNameList.Add(playerArray[i].nickName);
                }
                else
                {
                    playerNameList.Add(string.Empty);
                }
            }

            for (int i = 0; i < bureauDetailList.Count; i++)
            {
                bureauDetailList[i].bestRecord.playerNameList = playerNameList;
                bureauDetailList[i].bestRecord.selfSeatID = selfSeatID;
            }

        }


        public MjTitleInfo GetTitleInfoBySeat(int seatID)
        {
            for (int i = 0; i < titleList.Count; i++)
            {
                if (titleList[i].seatID == seatID)
                {
                    return titleList[i];
                }
            }
            return null;
        }



    }


    public class MahjongResultData
    {
        private List<GameResult> _resultData = new List<GameResult>();

        public List<GameResult> ResultData
        {
            get
            {
                return _resultData;
            }
        }

        public GameResult get(int deskID)
        {
            if (_resultData != null && _resultData.Count > 0)
            {
                for (int i = 0; i < _resultData.Count; i++)
                {
                    if (_resultData[i].deskID == deskID)
                    {
                        return _resultData[i];
                    }
                }
            }
            return null;
        }


        public void AddOrUpdateData(GameResult resutData)
        {
            GameResult original = this.get(resutData.deskID);
            if (original != null)
            {
                this._resultData.Remove(original);
            }
            this._resultData.Add(resutData);
        }


        public void CheckAndUpdateData(GameResult resutData)
        {
            GameResult original = this.get(resutData.deskID);
            if (original == null)
            {
                AddOrUpdateData(resutData);
            }
            else
            {
                original.UpdateGameResult(resutData.deskID, resutData.gameType, resutData.gameTypeSub, resutData.oddsCount, resutData.showType, resutData.bureauDetailList, resutData.titleList, resutData.selfSeatID);
                original.maxBouts = resutData.maxBouts;
                original.recordTime = resutData.recordTime;
                original.ownerUserID = resutData.ownerUserID;
            }
        }


        public void SetResultCostData(int deskID, List<GameResultCostData> costList, int reusltType = 0)
        {
            if (costList != null && costList.Count > 0)
            {
                GameResult item = get(deskID);
                if (item != null)
                {
                    for (int i = 0; i < costList.Count; i++)
                    {
                        item.AddOrUpdateCostData(costList[i]);
                    }
                    item.SetGameResultType(reusltType);
                }
            }


        }

    }

    #region 内存存储
    public partial class MKey
    {
        public const string USER_MAHJONG_RESULT_DATA = "USER_MAHJONG_RESULT_DATA";
    }

    public partial class MemoryData
    {
        static public MahjongResultData ResultData
        {
            get
            {
                MahjongResultData itemData = MemoryData.Get<MahjongResultData>(MKey.USER_MAHJONG_RESULT_DATA);
                if (itemData == null)
                {
                    itemData = new MahjongResultData();
                    MemoryData.Set(MKey.USER_MAHJONG_RESULT_DATA, itemData);
                }
                return itemData;
            }
        }

    }
    #endregion

}


