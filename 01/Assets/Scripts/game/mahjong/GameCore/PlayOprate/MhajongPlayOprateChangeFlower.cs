/**
 * @Author Xin.Wang
 * 补花玩法新类
 *
 */

using System.Collections;
using System.Collections.Generic;
using MahjongPlayType;
using projectQ;


public class MjRoundGetBuPai
{
    public int buType = 0;                      //补牌的type  
    public List<int> putCode;
    public List<int> changeCode;

    public List<int> putCodeDependent = new List<int>();
    public List<int> getCodeDependent = new List<int>();


    public MjRoundGetBuPai(List<int> putCode, List<int> nvalue, int type)
    {
        this.buType = type;
        this.putCode = putCode;
        this.changeCode = nvalue;
    }

    public void SetDependentData(List<int> putList, List<int> getList)
    {
        if (putList != null && getList != null)
        {
            putCodeDependent = putList;
            getCodeDependent = getList;
        }
    }
}


namespace projectQ
{
    public partial class MahongDataModel
    {
        public partial class GameProcessData
        {
            public MhajongPlayOprateChangeFlower processChangeFlower
            {
                get
                {
                    string key = MjPlayOprateType.OPRATE_CHANGEFLOWER.ToString();
                    if (!playOprateDic.ContainsKey(key))
                    {
                        MhajongPlayOprateChangeFlower item = new MhajongPlayOprateChangeFlower();
                        playOprateDic.Add(key, item);
                    }

                    return playOprateDic[key] as MhajongPlayOprateChangeFlower;
                }
            }
        }
    }

    public partial class MjPlayOprateType
    {
        public const string OPRATE_CHANGEFLOWER = "OPRATE_CHANGEFLOWER";
    }


    public partial class MjDataManager
    {
        /// <summary>
        /// 初始化信息接收
        /// </summary>
        public void SetChangeDataIni()
        {
            MjData.ProcessData.processChangeFlower.ClearChangeData();
        }

        /// <summary>
        /// 设置信息(某个人的某一轮）
        /// </summary>
        /// <param name="seatID"></param>
        /// <param name="loseCard"></param>
        /// <param name="loseType"></param>
        /// <param name="getCard"></param>
        /// <param name="getType"></param>
        public void SetChangeDataWhenDeal(int seatID, int loseCard, int loseType, int getCard, int getType)
        {
            MjData.ProcessData.processChangeFlower.SetChangeData(seatID, loseCard, loseType, getCard, getType);
        }

        /// <summary>
        /// 补花轮数结束(发牌补花
        /// </summary>
        public void SetChangeDataOverWhenDeal()
        {
            EventDispatcher.FireEvent(GEnum.NamedEvent.EMjChangeFlowerWhenDeal);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="seatID"></param>
        /// <param name="loseCard"></param>
        /// <param name="loseType"></param>
        /// <param name="getCard"></param>
        /// <param name="getType"></param>
        public void SetChangeDataWhenPut(int seatID, int loseCard, int loseType, int getCard, int getType, int mjRuler)
        {
            MjData.ProcessData.processChangeFlower.SetChangeDataOne(seatID, loseCard, loseType, getCard, getType);
            bool haveRuler = mjRuler != ConstDefine.MJ_PK_NULL;
            EventDispatcher.FireEvent(GEnum.NamedEvent.EMjChangeFlowerWhenPut, haveRuler);
        }
    }

}

namespace MahjongPlayType
{
    public class MjPlayOprateChangeFlowerRound
    {
        public class RoundBase
        {
            public int loseCard;
            public int loseCardType;
            public int getCard;
            public int getCardType;
        }

        public List<RoundBase> baseList = new List<RoundBase>();

        public void SetRoundBase(int loseCard, int loseType, int getCard, int getType)
        {
            RoundBase item = new RoundBase();
            item.loseCard = loseCard;
            item.getCard = getCard;
            item.loseCardType = loseType;
            item.getCardType = getType;
            baseList.Add(item);

            ChangeToUI(item);
        }


        //数据转化 
        private void ChangeToUI(RoundBase item)
        {
            if (item != null)
            {
                if (item.loseCardType == 1)
                {
                    //独立派 
                    dependentLoseList.Add(item.loseCard);
                }
                else
                {
                    handLoseList.Add(item.loseCard);
                }

                if (item.getCardType == 1)
                {
                    //独立牌1
                    dependentGetList.Add(item.getCard);
                }
                else
                {
                    handGetList.Add(item.getCard);
                }
            }
        }


        //数据用
        public int buType = 1;                                              //默认补花 回头扩展
        public bool canClick = false;                                       //是否锁
        public List<int> handLoseList = new List<int>();                    //失去的手牌
        public List<int> dependentLoseList = new List<int>();               //失去的独立牌

        public List<int> handGetList = new List<int>();                     //获取的手牌
        public List<int> dependentGetList = new List<int>();                //获取的独立牌

    }


    public class MjPlayOprateChangeFlowerBase
    {
        //服务器级别数据
        public int seatID;                         //座位号 
        public int rulerType;                      //玩法ID

        public List<MjPlayOprateChangeFlowerRound> roundList = new List<MjPlayOprateChangeFlowerRound>();

        public MjPlayOprateChangeFlowerBase(int seatID)
        {
            this.seatID = seatID;
        }

        public void SetData(int loseCard, int loseType, int getCard, int getType)
        {
            MjPlayOprateChangeFlowerRound item = new MjPlayOprateChangeFlowerRound();
            item.SetRoundBase(loseCard, loseType, getCard, getType);
            roundList.Add(item);
        }
    }


    public class MhajongPlayOprateChangeFlower : MahjongPlayOprateBase
    {
        private Dictionary<int, MjPlayOprateChangeFlowerBase> _dataBaseDic = new Dictionary<int, MjPlayOprateChangeFlowerBase>();

        public void ClearChangeData()
        {
            _dataBaseDic.Clear();
        }


        public void SetChangeData(int seatID, int loseCard, int loseType, int getCard, int getType)
        {
            MjPlayOprateChangeFlowerBase item = GetChangeData(seatID);
            item.SetData(loseCard, loseType, getCard, getType);
        }


        public MjPlayOprateChangeFlowerBase GetChangeData(int seatID)
        {
            if (!_dataBaseDic.ContainsKey(seatID))
            {
                MjPlayOprateChangeFlowerBase item = new MjPlayOprateChangeFlowerBase(seatID);
                _dataBaseDic.Add(seatID, item);
            }

            return _dataBaseDic[seatID];
        }

        public MjPlayOprateChangeFlowerBase CheckChangeContain(int seatID)
        {
            if (_dataBaseDic.ContainsKey(seatID))
            {
                return _dataBaseDic[seatID];
            }

            return null;
        }


        #region 个人补花

        public MjPlayOprateChangeFlowerBase changeOneQueue = null;

        public void SetChangeDataOne(int seatID, int loseCard, int loseType, int getCard, int getType)
        {
            MjPlayOprateChangeFlowerBase item = new MjPlayOprateChangeFlowerBase(seatID);
            item.SetData(loseCard, loseType, getCard, getType);
            changeOneQueue = item;
        }


        public MjPlayOprateChangeFlowerBase CheckChangeOne()
        {
            return changeOneQueue;
        }

        #endregion


    }


}
