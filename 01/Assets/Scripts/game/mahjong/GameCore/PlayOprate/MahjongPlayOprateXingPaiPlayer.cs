/**
 * @Author Xin.Wang
 * 行牌玩家信息
 *
 */

using System.Collections;
using System.Collections.Generic;
using MahjongPlayType;

namespace projectQ
{
    public partial class MahongDataModel
    {
        public partial class GameProcessData
        {
            public MahjongPlayOprateXingPaiPlayer processPlayer
            {
                get
                {
                    string key = MjPlayOprateType.OPRATE_PLAYERXINGPAI.ToString();
                    if (!playOprateDic.ContainsKey(key))
                    {
                        MahjongPlayOprateXingPaiPlayer item = new MahjongPlayOprateXingPaiPlayer();
                        playOprateDic.Add(key, item);
                    }

                    return playOprateDic[key] as MahjongPlayOprateXingPaiPlayer;
                }
            }
        }
    }

    public partial class MjPlayOprateType
    {
        public const string OPRATE_PLAYERXINGPAI = "OPRATE_PLAYERXINGPAI";
    }


    public partial class MjDataManager
    {
        /// <summary>
        /// 初始化玩家手牌
        /// </summary>
        /// <param name="seatID"></param>
        /// <param name="handList"></param>
        /// <param name="dependentCount"></param>
        /// <returns></returns>
        public int[] SetPlayerHandIni(int seatID, List<int> handList, int dependentCount)
        {
            int[] originalArray = null;

            if (CheckSeatIsSelf(seatID))
            {
                //self
                if (handList == null || handList.Count < 1)
                {
                    return originalArray;
                }

                originalArray = handList.ToArray();
                int firstLast = handList[handList.Count - 1];
                List<int> dependentList = new List<int>();
                if (dependentCount > 0)
                {
                    dependentList = handList.GetRange(0, dependentCount);
                    handList.RemoveRange(0, dependentCount);
                }

                MjData.ProcessData.processPlayer.IniPlayerHandList(seatID, handList, dependentList, firstLast);
            }
            else
            {
                int iniCount = 13;
                if (seatID == MjData.ProcessData.dealerSeatID)
                {
                    iniCount = 14;
                }

                if (dependentCount > 0)
                {
                    iniCount -= dependentCount;
                }

                MjData.ProcessData.processPlayer.IniPlayerHandList(seatID, IniHandList(iniCount), IniHandList(dependentCount), -1);
            }

            return originalArray;
        }

        private List<int> IniHandList(int count)
        {
            List<int> handList = new List<int>();
            for (int i = 0; i < count; i++)
            {
                handList.Add(-1);
            }
            return handList;
        }
        /// <summary>
        /// 获取最后一张牌
        /// </summary>
        /// <returns></returns>
        public int GetPlayerFirstLastHand()
        {
            return MjData.ProcessData.processPlayer.GetOrAddPlayerData(MjData.curUserData.selfSeatID).firstHandListLast;
        }

        //重连
        public void SetPlayerIndependentRefresh(int seatID, List<int> independentList)
        {
            MjData.ProcessData.processPlayer.GetOrAddPlayerData(seatID).baseData.list_Independent = independentList;
        }
        public void SetPlayerHandRefresh(int seatID, List<int> handList)
        {
            MjData.ProcessData.processPlayer.GetOrAddPlayerData(seatID).baseData.list_Hand = handList;
        }

        private void SortPlayerHandSelf()
        {
            int selfSeatID = MjData.curUserData.selfSeatID;
            MjData.ProcessData.processPlayer.GetOrAddPlayerData(selfSeatID).baseData.list_Hand.Sort();
        }

        #region 管理数量

        /// <summary>
        /// 失去一组牌
        /// </summary>
        /// <param name="cards"></param>
        public void LoseCardByNum(List<int> cards)
        {
            if (cards != null && cards.Count > 0)
            {
                int[] array = new int[cards.Count];
                cards.CopyTo(array);
                List<int> newList = new List<int>();
                newList.AddRange(array);

                if (MjData.ProcessData.lastPutCode > 0)
                {
                    newList.Remove(MjData.ProcessData.lastPutCode);
                }

                for (int i = 0; i < newList.Count; i++)
                {
                    LoseCardByNum(newList[i], 1, false);
                }
            }
        }

        /// <summary>
        /// 失去几张牌
        /// </summary>
        /// <param name="cardID"></param>
        /// <param name="loseNum"></param>
        public void LoseCardByNum(int cardID, int loseNum, bool isPut, int seatID = -1)
        {
            if (cardID > 0 && loseNum > 0)
            {
                SetLoseTingkou(cardID, loseNum);
                EventDispatcher.FireEvent(GEnum.NamedEvent.EMjControlLoseCard, cardID, loseNum);
            }
        }

        private void SetLoseTingkou(int cardID, int num)
        {
            if (MjData.ProcessData.curTingInfo != null)
            {
                MjData.ProcessData.curTingInfo.RefreshCount(cardID, num);
            }
        }

        #endregion


        #region 手牌数量管理
        public void SetPlayerHandLose(int seatID, List<int> mjCode)
        {
            for (int i = 0; i < mjCode.Count; i++)
            {
                SetPlayerHandLose(seatID, mjCode[i]);
            }
        }

        public void SetPlayerIndependentLose(int seatID, int mjCode)
        {
            List<int> independentList = MjData.ProcessData.processPlayer.GetOrAddPlayerData(seatID).baseData.list_Independent;
            int loseIndex = -1;

            for (int i = 0; i < independentList.Count; i++)
            {
                if (independentList[i] == mjCode)
                {
                    loseIndex = i;
                    break;
                }
            }

            if (loseIndex < 0)
            {
                int checkCode = -1;
                for (int i = 0; i < independentList.Count; i++)
                {
                    if (independentList[i] == checkCode)
                    {
                        loseIndex = i;
                        break;
                    }
                }

                if (loseIndex < 0)
                {
                    return;
                }
            }
            independentList.RemoveAt(loseIndex);
        }


        public void SetPlayerHandLose(int seatID, int mjCode)
        {
            List<int> handList = MjData.ProcessData.processPlayer.GetOrAddPlayerData(seatID).baseData.list_Hand;
            int loseIndex = -1;
            for (int i = 0; i < handList.Count; i++)
            {
                if (handList[i] == mjCode)
                {
                    loseIndex = i;
                    break;
                }
            }

            if (loseIndex < 0)
            {
                int checkCode = -1;
                for (int i = 0; i < handList.Count; i++)
                {
                    if (handList[i] == checkCode)
                    {
                        loseIndex = i;
                        break;
                    }
                }

                if (loseIndex < 0)
                {
                    return;
                }
            }
            handList.RemoveAt(loseIndex);
        }


        public void SetPlayerHandAdd(int seatID, List<int> mjCode)
        {
            MjData.ProcessData.processPlayer.GetOrAddPlayerData(seatID).baseData.list_Hand.AddRange(mjCode);
            if (CheckSeatIsSelf(seatID))
            {
                SortPlayerHandSelf();
            }
        }

        public void SetPlayerHandDependentAdd(int seatID, List<int> handeList)
        {
            MjData.ProcessData.processPlayer.GetOrAddPlayerData(seatID).baseData.list_Independent.AddRange(handeList);
        }


        public void SetPlayerHandAdd(int seatID, int mjCode)
        {
            if (seatID != MjData.curUserData.selfSeatID)
            {
                mjCode = -1;
            }

            MjData.ProcessData.processPlayer.GetOrAddPlayerData(seatID).baseData.list_Hand.Add(mjCode);
            if (CheckSeatIsSelf(seatID))
            {
                SortPlayerHandSelf();
            }
        }
        #endregion

        #region 出牌管理

        /// <summary>
        /// 设置出牌
        /// </summary>
        /// <param name="seatID"></param>
        /// <param name="mjCode"></param>
        /// <param name="isIndependent"></param>
        public void SetPlayerHandPut(int seatID, int mjCode, bool isIndependent)
        {
            bool isSelf = CheckSeatIsSelf(seatID);

            MjData.ProcessData.processPlayer.GetOrAddPlayerData(seatID).baseData.list_Puts.Add(mjCode);
            MjData.ProcessData.lastPutCode = mjCode;
            MjData.ProcessData.lastPutSeatID = seatID;

            if (isIndependent)
            {
                SetPlayerIndependentLose(seatID, mjCode);
            }
            else
            {
                SetPlayerHandLose(seatID, mjCode);
            }
        }

        /// <summary>
        /// 刷新出牌
        /// </summary>
        public void RefreshPlayerHandPut(int seatID, List<int> putList)
        {
            MjData.ProcessData.processPlayer.GetOrAddPlayerData(seatID).baseData.list_Puts = putList;
        }

        /// <summary>
        /// 移除出牌
        /// </summary>
        /// <param name="seatID"></param>
        /// <param name="mjCode"></param>
        public void SetPlayerHandRemovePut(int seatID, int mjCode)
        {
            List<int> puts = MjData.ProcessData.processPlayer.GetOrAddPlayerData(seatID).baseData.list_Puts;
            if (puts != null && puts.Count > 0)
            {
                int index = puts.Count - 1;
                if (puts[index] == mjCode)
                {
                    puts.RemoveAt(index);
                }
            }
        }
        /// <summary>
        /// 获取牌区数量
        /// </summary>
        /// <param name="seatID"></param>
        /// <returns></returns>
        public int GetPlayerPutCount(int seatID)
        {
            List<int> puts = MjData.ProcessData.processPlayer.GetOrAddPlayerData(seatID).baseData.list_Puts;
            if (puts != null)
            {
                return puts.Count;
            }
            return 0;
        }


        #endregion

        #region 行牌流程管理

        public void SetPlayerHandPeng(int seatID, int mjCode, int independent = 0, bool reConned = false)
        {
            //change hand
            if (!reConned)
            {
                bool isSelf = CheckSeatIsSelf(seatID);

                int needCount = 2;
                if (independent > 0)
                {
                    needCount -= independent;
                    for (int i = 0; i < independent; i++)
                    {
                        SetPlayerIndependentLose(seatID, mjCode);
                    }
                }

                for (int i = 0; i < needCount; i++)
                {
                    SetPlayerHandLose(seatID, mjCode);
                }
            }

            //add peng
            MjData.ProcessData.processPlayer.GetOrAddPlayerData(seatID).baseData.list_Peng.Add(mjCode);
        }


        public void SetPlayerHandHua(int seatID, List<int> mjCode)
        {
            MjData.ProcessData.processPlayer.GetOrAddPlayerData(seatID).list_Flower.AddRange(mjCode);
        }

        public int GetPlayerHandHuaAllCount(int seatID = -1)
        {
            int count = 0;
            if (seatID > 0)
            {
                return MjData.ProcessData.processPlayer.GetOrAddPlayerData(seatID).list_Flower.Count;
            }
            else
            {
                for (int i = 1; i < 5; i++)
                {
                    count += MjData.ProcessData.processPlayer.GetOrAddPlayerData(i).list_Flower.Count;
                }
            }
            return count;
        }

        public List<int>[] GetPlayerHandHuaList()
        {
            List<int>[] array = new List<int>[4];

            for (int i = 1; i < 5; i++)
            {
                List<int> curList = MjData.ProcessData.processPlayer.GetOrAddPlayerData(i).list_Flower;
                if (curList == null)
                {
                    curList = new List<int>();
                }
                array[i - 1] = curList;
            }

            return array;
        }


        public int GetPlayerHandHuaCount(int seatID)
        {
            int curCount = 0;
            List<int> curList = MjData.ProcessData.processPlayer.GetOrAddPlayerData(seatID).list_Flower;
            if (curList != null)
            {
                curCount = curList.Count;
            }
            return curCount;
        }


        public void SetPlayerHandMao(int seatID, List<int> mjCode)
        {
            bool isSelf = CheckSeatIsSelf(seatID);
            for (int i = 0; i < 2; i++)
            {
                int loseID = isSelf ? mjCode[i] : -1;
                SetPlayerHandLose(seatID, loseID);
            }

            //add mao
            int[] maoArray = mjCode.ToArray();
            MjData.ProcessData.processPlayer.GetOrAddPlayerData(seatID).list_Mao.Add(maoArray);
        }


        public void SetPlayerHandChi(int seatID, List<int> chiList)
        {
            int[] chiArray = chiList.ToArray();
            MjData.ProcessData.processPlayer.GetOrAddPlayerData(seatID).baseData.list_Chi.Add(chiArray);
        }


        public void SetPlayerHandZhiGang(int seatID, int mjCode, int independent = 0, bool reConned = false)
        {
            if (!reConned)
            {
                bool isSelf = CheckSeatIsSelf(seatID);
                int needCount = 3;
                if (independent > 0)
                {
                    needCount -= independent;
                    for (int i = 0; i < independent; i++)
                    {
                        SetPlayerIndependentLose(seatID, mjCode);
                    }
                }

                for (int i = 0; i < needCount; i++)
                {
                    SetPlayerHandLose(seatID, mjCode);
                }
            }

            MjData.ProcessData.processPlayer.GetOrAddPlayerData(seatID).baseData.list_ZhiGang.Add(mjCode);
        }

        public void SetPlayerHandBuGang(int seatID, int mjCode, int independent = 0, bool reConned = false)
        {
            if (!reConned)
            {
                bool isSelf = CheckSeatIsSelf(seatID);

                if (independent > 0)
                {
                    SetPlayerIndependentLose(seatID, mjCode);
                }
                else
                {
                    SetPlayerHandLose(seatID, mjCode);
                }
            }

            MjData.ProcessData.processPlayer.GetOrAddPlayerData(seatID).baseData.list_BuGang.Add(mjCode);
        }


        public void SetPlayerHandAnGang(int seatID, int mjCode, int independent = 0, bool reConned = false)
        {
            if (!reConned)
            {
                bool isSelf = CheckSeatIsSelf(seatID);
                int needCount = 4;
                if (independent > 0)
                {
                    needCount -= independent;
                    for (int i = 0; i < independent; i++)
                    {
                        SetPlayerIndependentLose(seatID, mjCode);
                    }
                }


                for (int i = 0; i < needCount; i++)
                {
                    SetPlayerHandLose(seatID, mjCode);
                }
            }


            MjData.ProcessData.processPlayer.GetOrAddPlayerData(seatID).baseData.list_AnGang.Add(mjCode);
        }


        #endregion

        #region 换三张

        public void SetPlayerHandChangeThreeLose(List<int> mjCode)
        {
            for (int i = 1; i < 5; i++)
            {
                if (i == MjData.curUserData.selfSeatID)
                {
                    this.SetPlayerHandLose(i, mjCode);
                }
                else
                {
                    this.SetPlayerHandLose(i, IniHandList(3));
                }
            }
        }

        public void SetPlayerHandChangeThreeAdd(List<int> mjCode)
        {
            for (int i = 1; i < 5; i++)
            {
                if (i == MjData.curUserData.selfSeatID)
                {
                    this.SetPlayerHandAdd(i, mjCode);
                }
                else
                {
                    this.SetPlayerHandAdd(i, IniHandList(3));
                }
            }

        }

        #endregion


        #region 状态管理 

        //public enum EnumHandState
        //{
        //    Null,
        //    Standing = 1,               //站立
        //    Open = 2,                   //摊开
        //    Close = 3,                  //关闭
        //    Back = 4,                   //背对（一般用于自己）
        //}
        public void SetPlayerHandState(int seatID, MjXingpaiPlayerStateData.EnumHandState handState)
        {
            MjData.ProcessData.processPlayer.GetOrAddPlayerData(seatID).playerStateData.handState = handState;
        }


        public MjXingpaiPlayerStateData.EnumHandState GetPlayerHandState(int seatID)
        {
            MjXingpaiPlayerStateData.EnumHandState state = MjData.ProcessData.processPlayer.GetOrAddPlayerData(seatID).playerStateData.handState;
            int selfSeatID = MjData.curUserData.selfSeatID;
            if (selfSeatID != seatID)
            {
                //他人
                //bool isHuiFang = false;
                //if (isHuiFang)
                //{
                //    state = MjXingpaiPlayerStateData.EnumHandState.Open;
                //}
                if (FakeReplayManager.Instance.ReplayState)
                {
                    state = MjXingpaiPlayerStateData.EnumHandState.Open;
                }
            }

            return state;
        }

        public MjXingpaiPlayerStateData.EnumHandState GetPlayerHandStateByUISeat(int uiSeatID)
        {
            int selfSeatID = MjData.curUserData.selfSeatID;
            int serverSeat = CardHelper.GetMjServerPosByUIPos(uiSeatID, selfSeatID);
            return GetPlayerHandState(serverSeat);
        }





        public void SetPlayerHasTing(int seatID)
        {
            MjData.ProcessData.processPlayer.GetOrAddPlayerData(seatID).playerStateData.hasTing = true;
        }

        public void SetPlayerHasHu(int seatID)
        {
            MjData.ProcessData.processPlayer.GetOrAddPlayerData(seatID).playerStateData.hasHu = true;
        }

        public void SetPlayerHasMinglou(int seatID)
        {
            MjData.ProcessData.processPlayer.GetOrAddPlayerData(seatID).playerStateData.hasMinglou = true;
        }


        public bool CheckSeatHuOrTing()
        {
            int seatID = MjData.curUserData.selfSeatID;

            return MjData.ProcessData.processPlayer.GetOrAddPlayerData(seatID).playerStateData.hasHu
                || MjData.ProcessData.processPlayer.GetOrAddPlayerData(seatID).playerStateData.hasTing
                || MjData.ProcessData.processPlayer.GetOrAddPlayerData(seatID).playerStateData.hasMinglou;
        }

        public bool CheckSeatIsSelf(int seatID)
        {
            return seatID == MjData.curUserData.selfSeatID;
        }

        #endregion
    }

}

namespace MahjongPlayType
{
    public class MjXingpaiPlayerBase
    {
        public List<int> list_Hand = new List<int>();
        public List<int> list_Independent = new List<int>();
        public List<int[]> list_Chi = new List<int[]>();
        public List<int> list_Peng = new List<int>();
        public List<int> list_AnGang = new List<int>();
        public List<int> list_ZhiGang = new List<int>();
        public List<int> list_BuGang = new List<int>();
        public List<int> list_Puts = new List<int>();                   //已经打出去的

    }

    public class MjXingpaiPlayerStateData
    {
        public enum EnumHandState
        {
            Null,
            Standing = 1,               //站立
            Open = 2,                   //摊开
            Close = 3,                  //关闭
            Back = 4,                   //背对（一般用于自己）
        }
        public EnumHandState handState = EnumHandState.Standing;            //默认站立


        public bool hasTing = false;                //是否听牌
        public bool hasMinglou = false;             //是否明楼 
        public bool hasHu = false;                  //是否胡牌
    }

    public class MjXingpaiPlayerData
    {
        public MjXingpaiPlayerBase baseData = new MjXingpaiPlayerBase();

        public List<int[]> list_Mao = new List<int[]>();                //放毛
        public List<int> list_Flower = new List<int>();                 //补掉的牌
        public List<int> list_Hu = new List<int>();                     //已经胡的牌

        public int firstHandListLast = -1;                              //当前手牌最后一张

        public MjXingpaiPlayerStateData playerStateData = new MjXingpaiPlayerStateData();       //当前玩家一些状态
    }


    public class MahjongPlayOprateXingPaiPlayer : MahjongPlayOprateBase
    {
        public Dictionary<int, MjXingpaiPlayerData> playerDataDic = new Dictionary<int, MjXingpaiPlayerData>();

        /// <summary>
        /// 初始化手牌
        /// </summary>
        /// <param name="seatID"></param>
        /// <param name="handList"></param>
        /// <param name="independentList"></param>
        /// <param name="lastCard"></param>
        public void IniPlayerHandList(int seatID, List<int> handList, List<int> independentList, int lastCard)
        {
            MjXingpaiPlayerData playerData = GetOrAddPlayerData(seatID);
            playerData.baseData.list_Hand = handList;
            playerData.baseData.list_Independent = independentList;
            playerData.firstHandListLast = lastCard;
        }

        public MjXingpaiPlayerData GetOrAddPlayerData(int seatID)
        {
            if (!playerDataDic.ContainsKey(seatID))
            {
                MjXingpaiPlayerData data = new MjXingpaiPlayerData();
                playerDataDic.Add(seatID, data);
            }

            return playerDataDic[seatID];
        }


    }

}

