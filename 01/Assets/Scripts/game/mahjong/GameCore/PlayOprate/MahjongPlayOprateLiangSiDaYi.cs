/**
 * @Author Xin.Wang
 * 亮四打一数据层
 *
 */

using System.Collections;
using System.Collections.Generic;
using projectQ;
using MahjongPlayType;


namespace projectQ
{
    public partial class MahongDataModel
    {
        public partial class GameProcessData
        {
            public MahjongPlayOprateLiangSiDaYi processLiangSiDaYi
            {
                get
                {
                    string key = MjPlayOprateType.OPRATE_LIANGSIDAYI.ToString();
                    if (!playOprateDic.ContainsKey(key))
                    {
                        MahjongPlayOprateLiangSiDaYi item = new MahjongPlayOprateLiangSiDaYi();
                        playOprateDic.Add(key, item);
                    }

                    return playOprateDic[key] as MahjongPlayOprateLiangSiDaYi;
                }
            }
        }
    }


    public partial class MjPlayOprateType
    {
        public const string OPRATE_LIANGSIDAYI = "OPRATE_LIANGSIDAYI";
    }

    public partial class MjDataManager
    {
        public void LSDY_SetData(int seatID, List<int> cardList, bool havePut)
        {
            MjData.ProcessData.processLiangSiDaYi.IniOrUpdateData(seatID, cardList, havePut);
        }


        public void LSDY_SetPutState(int seatID)
        {
            if (LSDY_CheckPlayOprate())
            {
                MjData.ProcessData.processLiangSiDaYi.UpdatePutState(seatID);
            }
        }


        /// <summary>
        /// 检查亮四打一出牌状态
        /// </summary>
        /// <returns></returns>
        public bool LSDY_CheckPutState()
        {
            bool havePut = false;
            if (LSDY_CheckPlayOprate())
            {
                int selfSeat = MjData.curUserData.selfSeatID;
                havePut = MjData.ProcessData.processLiangSiDaYi.GetPutState(selfSeat);
            }
            else
            {
                havePut = true;
            }
            return havePut;
        }


        public List<int> LSDY_GetCardList(int seatID)
        {
            return MjData.ProcessData.processLiangSiDaYi.GetDataBySeatID(seatID).curShowCardList;
        }


        /// <summary>
        /// 检测是否有亮四打一玩法
        /// </summary>
        /// <returns></returns>
        public bool LSDY_CheckPlayOprate()
        {
            bool countain = MjData.ProcessData.playOprateDic.ContainsKey(MjPlayOprateType.OPRATE_LIANGSIDAYI);
            return countain;
        }
    }

}


namespace MahjongPlayType
{
    public class MjLiangSiDayiData
    {
        public int seatID;
        public List<int> curShowCardList = new List<int>();
        public bool havePutOne = false;


        public MjLiangSiDayiData(int seat)
        {
            this.seatID = seat;
        }
    }


    public class MahjongPlayOprateLiangSiDaYi : MahjongPlayOprateBase
    {
        private Dictionary<int, MjLiangSiDayiData> _dataDic = new Dictionary<int, MjLiangSiDayiData>();


        public void IniOrUpdateData(int seatID, List<int> cardList, bool havePut)
        {
            MjLiangSiDayiData item = GetDataBySeatID(seatID);
            if (cardList != null)
            {
                item.curShowCardList = cardList;
            }
            item.havePutOne = havePut;
        }


        public void UpdatePutState(int seatID)
        {
            MjLiangSiDayiData item = GetDataBySeatID(seatID);
            item.havePutOne = true;
        }

        public bool GetPutState(int seatID)
        {
            MjLiangSiDayiData item = GetDataBySeatID(seatID);
            return item.havePutOne;
        }

        public MjLiangSiDayiData GetDataBySeatID(int seatID)
        {
            if (!_dataDic.ContainsKey(seatID))
            {
                MjLiangSiDayiData item = new MjLiangSiDayiData(seatID);
                _dataDic.Add(seatID, item);
            }
            return _dataDic[seatID];
        }

    }
}

