/**
 * @Author Xin.Wang
 *
 *
 */

using System.Collections;
using System.Collections.Generic;

namespace projectQ
{

    //小结算的新数据模块
    public class MjBalanceNew
    {
        public class BalancePlayerInfo
        {
            public long userID;                                         //用户ID
            public int userSeat;                                     //用户座位号
            public string userNick;                                     //用户昵称
            public string userHead;                                     //用户头像
            public int scoreAmount;                                     //累计的积分
            public int scoreCur;                                        //当前小局的积分

            public BestMjRecord handRecord = null;                      //结算的手牌
            public List<int> huList = new List<int>();                  //胡牌的列表

            public int huNum = 0;                                       //胡过几次
            public int paoNum = 0;                                      //点过几次炮
            public bool isChengbao = false;                             //是否是承包

            public List<MjDetaildedScore> detailList = new List<MjDetaildedScore>();    //正积分详情信息
            public List<MjDetaildedScore> detailListCut = new List<MjDetaildedScore>(); //负积分详情信息

            public List<MjCheckShow> checkShowList = new List<MjCheckShow>();           //特殊玩法列表
        }

        public class MjCheckShow
        {
            public EnumMjSpecialCheck checkType = EnumMjSpecialCheck.Null;
            public int checkValue = 0;
        }

        /// <summary>
        /// 马的玩法的数据类型 
        /// </summary>
        public class MjHorseInfo
        {
            /// <summary>
            /// 公用数据模块
            /// </summary>
            public class BuyHorseItem
            {
                //公用
                public List<int> cardIDList = new List<int>();          //麻将牌列表
                public List<int> horseStateList = new List<int>();      //每张牌的状态(EnumMjBuyhorseStateType） 不存储具体类型 只存储值  取值时根据需要自行转换


                //public enum EnumMjBuyhorseStateType
                //{
                //    BuyHorseNull = 1,               //没中
                //    BuyHorseWin = 2,               //买中赢家(买中： )
                //    BuyHorseLose = 3,               //买中输家
                //}

                //四家 
                public int seatID;          //座位号
                public List<int> horseHitSeatList = new List<int>();    //买中的座位ID（
                public List<int> scoreList = new List<int>();           //分数列表 

                public bool CheckForShow()
                {
                    bool checkForshow = true;

                    checkForshow = cardIDList.Count == horseStateList.Count
                        && horseHitSeatList.Count == scoreList.Count
                        && horseStateList.Count == horseHitSeatList.Count;

                    return checkForshow;
                }
            }

            #region 单人买马 

            /// <summary>
            /// 单人开马的数据类型，包含：
            /// 奖马、爆炸马、抓码、扎码、一码全中、抓鸟
            /// 日后可以追加
            /// </summary>
            public class BuyHorseNotifyData
            {
                public int gameType = -1;               //玩法类型
                public List<BuyHorseItem> horseItemList = new List<BuyHorseItem>();
                //单家

                public List<int> scoreSeatList = new List<int>();         //分数座位列表
                public List<int> scoreList = new List<int>();           //分数列表 

                public int winNum = 0;                          //中马个数

                public BuyHorseNotifyData(int gameType)
                {
                    this.gameType = gameType;
                }

                public void SetDataItme(List<int> mjCode, List<int> getType)
                {
                    BuyHorseItem item = new BuyHorseItem();
                    item.cardIDList = mjCode;
                    item.horseStateList = getType;
                    winNum = 0;
                    for (int i = 0; i < getType.Count; i++)
                    {
                        if (getType[i] == 2)
                            winNum++;
                    }

                    this.horseItemList.Add(item);
                }


                public void SetDataScore(List<int> seatID, List<int> score)
                {
                    this.scoreSeatList = seatID;
                    this.scoreList = score;
                }


            }

            /// <summary>
            /// 数据列表，可以共存多个
            /// </summary>
            public Dictionary<int, BuyHorseNotifyData> buyHorseDataDic = new Dictionary<int, BuyHorseNotifyData>();

            public void SetHorseDataList(int gameType, List<int> mjCode, List<int> getType)
            {
                BuyHorseNotifyData dataItem = AddOrUpdateADataToList(gameType);

                if (dataItem != null)
                {
                    dataItem.SetDataItme(mjCode, getType);
                }
            }


            public void SetHorseDataScoreList(int gameType, List<int> seatID, List<int> score)
            {
                BuyHorseNotifyData dataItem = AddOrUpdateADataToList(gameType);

                if (dataItem != null)
                {
                    dataItem.SetDataScore(seatID, score);
                }
            }


            private BuyHorseNotifyData AddOrUpdateADataToList(int gameType)
            {
                BuyHorseNotifyData dataItem = null;
                if (buyHorseDataDic.ContainsKey(gameType))
                {
                    dataItem = buyHorseDataDic[gameType];
                }
                else
                {
                    dataItem = new BuyHorseNotifyData(gameType);
                    buyHorseDataDic.Add(gameType, dataItem);
                }
                return dataItem;
            }


            #endregion


            #region 四家买马

            /// <summary>
            /// 四家买马的数据类型
            /// </summary>
            public class BuyHorseSiJiangNotifyData
            {
                public class BuyHorseSiJiaShowData
                {
                    public class ShowItem
                    {
                        public List<int> cardList = new List<int>();            //牌的列表
                        public List<int> hitSeatID = new List<int>();          //命中座位列表
                        public List<int> scoreList = new List<int>();           //分数列表

                        public int scoreAmount = 0;                     //总分
                        public int cardAmount = 0;                      //总数

                        public void AddCardAnScore(int cardID, int score, int seatID)
                        {
                            cardList.Add(cardID);
                            scoreList.Add(score);
                            hitSeatID.Add(seatID);

                            scoreAmount += score;
                            cardAmount += 1;
                        }
                    }

                    public Dictionary<int, ShowItem> showItemDic = new Dictionary<int, ShowItem>();         //用的是买马类型做Key


                    public ShowItem GetShowItemByType(int horseType)
                    {
                        if (!showItemDic.ContainsKey(horseType))
                        {
                            ShowItem showItem = new ShowItem();
                            showItemDic.Add(horseType, showItem);
                        }
                        return showItemDic[horseType];
                    }

                    public void AddCardAnScoreByHoreseType(int horseType, int cardID, int score, int seatID)
                    {
                        ShowItem item = GetShowItemByType(horseType);
                        item.AddCardAnScore(cardID, score, seatID);
                    }
                }



                public int gameType = -1;                                   //游戏类型
                public List<BuyHorseItem> horseItemList = new List<BuyHorseItem>();
                public Dictionary<int, BuyHorseSiJiaShowData> horseShowDataDic = new Dictionary<int, BuyHorseSiJiaShowData>();  //展示类型，用seatID做索引

                public BuyHorseSiJiaShowData GetBuyHorseSiJiaShowData(int seatID)
                {
                    if (!horseShowDataDic.ContainsKey(seatID))
                    {
                        BuyHorseSiJiaShowData item = new BuyHorseSiJiaShowData();
                        horseShowDataDic.Add(seatID, item);
                    }
                    return horseShowDataDic[seatID];
                }



                public BuyHorseSiJiangNotifyData(int gameType)
                {
                    this.gameType = gameType;
                }

                public bool SetDataItme(int seatID, List<int> mjCode, List<int> getType, List<int> hitSeat, List<int> scoreList)
                {
                    BuyHorseItem item = new BuyHorseItem();
                    item.seatID = seatID;
                    item.cardIDList = mjCode;
                    item.horseStateList = getType;
                    item.horseHitSeatList = hitSeat;
                    item.scoreList = scoreList;

                    this.horseItemList.Add(item);

                    bool needSetForShow = item.CheckForShow();
                    return needSetForShow;
                }

                /// <summary>
                /// 封装展示类型 
                /// </summary>
                public void SetDataForShow()
                {
                    if (horseItemList != null && horseItemList.Count > 0)
                    {
                        for (int i = 0; i < horseItemList.Count; i++)
                        {
                            BuyHorseItem itme = horseItemList[i];
                            int curSeatID = itme.seatID;
                            BuyHorseSiJiaShowData showData = GetBuyHorseSiJiaShowData(curSeatID);
                            for (int k = 0; k < itme.cardIDList.Count; k++)
                            {
                                showData.AddCardAnScoreByHoreseType(itme.horseStateList[k], itme.cardIDList[k],
                                    itme.scoreList[k], itme.horseHitSeatList[k]);
                            }
                        }
                    }
                }
            }

            public BuyHorseSiJiangNotifyData buyHorseSiJiaData = null;                  //四家买马数据 (请判空)
            #endregion

        }



        public int showType = 1;                                        //1.普通 2.血流血战
        public int gameType = 0;                                        //游戏类型
        public int gameTypeSub = 0;                                     //游戏类型的索引
        public List<int> gameRulerType = new List<int>();               //规则列表
        public int deskID = 0;                                          //桌子ID
        public int dealerSeatID = 1;                                    //庄家座位ID
        public int ownerSeatID = 1;                                     //桌主座位ID
        public List<BalancePlayerInfo> playerInfoList = new List<BalancePlayerInfo>();      //玩家信息列表
        public bool isDraw = false;                                     //是否流局
        public int curBureau = 0;                                       //当前局数
        public int maxBureau = 0;                                       //总局数
        public MjHorseInfo horseInfo = null;                            //马玩法相关数据(请判空）
        public int gameEndTime = 0;                                     //游戏结束时间

        public void SetHorseDataIni()
        {
            if (horseInfo == null)
            {
                horseInfo = new MjHorseInfo();
            }
        }


        public int showTime;
        public bool showByResult = false;                               //打开方式(true时，跳过下一局逻辑)

    }


    public class MahjongBalanceData
    {


        /// <summary>
        /// 小结算数据查询
        /// </summary>
        /// <param deskID="桌子ID"></param>
        /// <param serverTime="创建时间"></param>
        /// <param curBureau="当前小局"></param>
        /// <returns></returns>
        public MjBalanceNew get(int deskID, int curBureau)
        {
            MjBalanceNew balanceInfo = null;
            string checkStr = createCheckStr(deskID, curBureau);
            object obj = FightMahjongData.MjDeSerialize(checkStr);
            if (obj != null)
            {
                balanceInfo = ModelNetWorker.Instance.GetBalanceNewInfoByMemory(obj);
            }
            return balanceInfo;
        }



        public void AddOrUpdateData(int deskID, int curBureau, Msg.MjBalanceNewNotify proto)
        {
            string checkStr = createCheckStr(deskID, curBureau);

            FightMahjongData.MjSerislize(proto, checkStr);
        }


        public void ClearDataInMemory()
        {
            FightMahjongData.Clear();
        }


        private string createCheckStr(int deskID, int curBureau)
        {
            string checkStr = string.Empty;
            checkStr = string.Format("BALANCE:{0}:{1}", deskID, curBureau);
            return checkStr;
        }

    }



    #region 内存读取

    public partial class MKey
    {
        public const string USER_MAHJONG_BALANCE_DATA = "USER_MAHJONG_BALANCE_DATA";
    }

    public partial class MemoryData
    {
        public static MahjongBalanceData BalanceData
        {
            get
            {
                MahjongBalanceData itemData = MemoryData.Get<MahjongBalanceData>(MKey.USER_MAHJONG_BALANCE_DATA);
                if (itemData == null)
                {
                    itemData = new MahjongBalanceData();
                    MemoryData.Set(MKey.USER_MAHJONG_BALANCE_DATA, itemData);
                }

                return itemData;
            }
        }


    }
    #endregion


}

