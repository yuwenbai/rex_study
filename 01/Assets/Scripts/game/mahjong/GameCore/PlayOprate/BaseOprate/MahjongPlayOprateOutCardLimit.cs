using System.Collections;
using System.Collections.Generic;
using MahjongPlayType;


namespace projectQ
{
    public partial class MahongDataModel
    {
        public partial class GameProcessData
        {
            public MahjongPlayOprateOutCardLimit processOutCardLimitData
            {
                get
                {
                    string key = MjPlayOprateType.OPRATE_OUTCARDLIMIT.ToString();
                    if (!playOprateDic.ContainsKey(key))
                    {
                        MahjongPlayOprateOutCardLimit item = new MahjongPlayOprateOutCardLimit();
                        playOprateDic.Add(key, item);
                    }

                    return playOprateDic[key] as MahjongPlayOprateOutCardLimit;
                }

            }
        }
    }

    public partial class MjPlayOprateType
    {
        public const string OPRATE_OUTCARDLIMIT = "OPRATE_OUTCARDLIMIT";
    }

    public partial class MjDataManager
    {
        public void SetInitLimitData(int seatid, int LackType, List<int> typeList, List<int> cardList)
        {
            MjData.ProcessData.processOutCardLimitData.SetInitLimitList(seatid, LackType, typeList, cardList);
        }

        public List<int> GetLimitCardList()
        {
            var data = MjData.ProcessData.processOutCardLimitData.GetDataBySeatID(MjDataManager.Instance.MjData.curUserData.selfSeatID);
            if (data == null)
                return new List<int>();
            return data.mjCard;
        }

        public bool CheckIsLimitCardByID(int id)
        {
            var data = MjData.ProcessData.processOutCardLimitData.GetDataBySeatID(MjDataManager.Instance.MjData.curUserData.selfSeatID);
            if (data == null)
                return false;
            List<int> list = data.mjCard;

            for (int i = 0; i < list.Count; i++)
            {
                if (list[i] == id)
                    return true;
            }
            return false;
        }

        public string GetLockTypeName(int type)
        {
            return MjData.ProcessData.processOutCardLimitData.GetLockTypeName(type);
        }

        public MjPlayerOutCardLimit GetOutCardLimitBySeatID(int seatID)
        {
            return MjData.ProcessData.processOutCardLimitData.GetDataBySeatID(seatID);
        }
    }
}

namespace MahjongPlayType
{
    public class MjPlayerOutCardLimit
    {
        public int seatID;
        public int lackType;
        public List<int> mjCard;
    }

    public class MahjongPlayOprateOutCardLimit: MahjongPlayOprateBase
    {
        private Dictionary<int, MjPlayerOutCardLimit> m_DataDict = new Dictionary<int, MjPlayerOutCardLimit>();

        public void SetInitLimitList(int seatID, int lackType, List<int> typeList, List<int> cardList)
        {
            if (m_DataDict.ContainsKey(seatID))
            {
                MjPlayerOutCardLimit data = m_DataDict[seatID];
                data.lackType = lackType;
                data.seatID = seatID;

                List<int> calist = new List<int>();
                for (int i = 0; i < typeList.Count; i++)
                {
                    List<int> list = GetMahjongListByTypeID((m_MjPutLimitNotifyType)typeList[i]);
                    calist.AddRange(list);
                }
                data.mjCard = calist;
            }
            else
            {
                MjPlayerOutCardLimit data = new MjPlayerOutCardLimit();
                data.lackType = lackType;
                data.seatID = seatID;

                List<int> calist = new List<int>();
                for (int i = 0; i < typeList.Count; i++)
                {
                    List<int> list = GetMahjongListByTypeID((m_MjPutLimitNotifyType)typeList[i]);
                    calist.AddRange(list);
                }
                data.mjCard = calist;
                m_DataDict[seatID] = data;
            }
        }

        public string GetLockTypeName(int type)
        {
            string spName = "";

            switch ((m_MjPutLimitNotifyType)type)
            {
                case m_MjPutLimitNotifyType.MjPutLimitNotifyType_FENGJIAN:
                    break;
                case m_MjPutLimitNotifyType.MjPutLimitNotifyType_WAN:
                    spName = "desk_txt_wan";
                    break;
                case m_MjPutLimitNotifyType.MjPutLimitNotifyType_TIAO:
                    spName = "desk_txt_tiao";
                    break;
                case m_MjPutLimitNotifyType.MjPutLimitNotifyType_TONG:
                    spName = "desk_txt_tong";
                    break;
                case m_MjPutLimitNotifyType.MjPutLimitNotifyType_FLOWER:
                    break;
                default:
                    break;
            }

            return spName;
        }

        public MjPlayerOutCardLimit GetDataBySeatID(int seatID)
        {
            if (m_DataDict.ContainsKey(seatID))
            {
                return m_DataDict[seatID];
            }
            return null;
        }

        private enum m_MjPutLimitNotifyType
        {
            MjPutLimitNotifyType_FENGJIAN = 1000,      //风剑牌
            MjPutLimitNotifyType_WAN = 1001,      //万牌
            MjPutLimitNotifyType_TIAO = 1002,      //条牌
            MjPutLimitNotifyType_TONG = 1003,      //筒牌
            MjPutLimitNotifyType_FLOWER = 1004,      //花牌
        }

        private List<int> GetMahjongListByTypeID(m_MjPutLimitNotifyType type)
        {
            List<int> result = null;

            switch (type)
            {
                case m_MjPutLimitNotifyType.MjPutLimitNotifyType_FENGJIAN:
                    result = new List<int>()
                    {
                        1,2,3,4,5,6,7
                    };
                    break;
                case m_MjPutLimitNotifyType.MjPutLimitNotifyType_WAN:
                    result = new List<int>()
                    {
                        8,9,10,11,12,13,14,15,16
                    };
                    break;
                case m_MjPutLimitNotifyType.MjPutLimitNotifyType_TIAO:
                    result = new List<int>()
                    {
                        17,18,19,20,21,22,23,24,25
                    };
                    break;
                case m_MjPutLimitNotifyType.MjPutLimitNotifyType_TONG:
                    result = new List<int>()
                    {
                        26,27,28,29,30,31,32,33,34
                    };
                    break;
                case m_MjPutLimitNotifyType.MjPutLimitNotifyType_FLOWER:
                    result = new List<int>()
                    {
                        35,36,37,38,39,40,41,42
                    };
                    break;
                default:
                    result = new List<int>();
                    break;
            }

            return result;
        }
    }
}
