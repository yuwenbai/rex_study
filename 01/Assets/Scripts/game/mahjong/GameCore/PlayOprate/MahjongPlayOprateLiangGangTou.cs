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
            public MahjongPlayOprateLiangGangTou processLiangGangTou
            {
                get
                {
                    string key = MjPlayOprateType.OPRATE_LIANGGANGTOU.ToString();
                    if (!playOprateDic.ContainsKey(key))
                    {
                        MahjongPlayOprateLiangGangTou item = new MahjongPlayOprateLiangGangTou();
                        playOprateDic.Add(key, item);
                    }

                    return playOprateDic[key] as MahjongPlayOprateLiangGangTou;
                }
            }
        }
    }

    public partial class MjPlayOprateType
    {
        public const string OPRATE_LIANGGANGTOU = "OPRATE_LIANGGANGTOU";
    }


    public partial class MjDataManager
    {
        public void SetInitLiangGangTouData(int type, int seatid, int ruleid, List<int> cards)
        {
            MjData.ProcessData.processLiangGangTou.SetInitData(type, seatid, ruleid, cards);
        }

        public MJLiangGangTouData GetMjLiangGangTouData()
        {
            return MjData.ProcessData.processLiangGangTou.GetGangTouData();
        }

    }
}

namespace MahjongPlayType
{
    public class MJLiangGangTouData
    {
        public int selectType;
        public int seatID;
        public int ruleID;
        public List<int> cardList;

        public MJLiangGangTouData()
        {
            cardList = new List<int>();
        }

        public void SetMJData(int selecttype, int seatid, int ruleid, List<int> list)
        {
            selectType = selecttype;
            seatID = seatid;
            ruleID = ruleid;
            cardList.Clear();
            cardList = list;

            if (cardList == null)
                cardList = new List<int>();
        }
    }

    public class MahjongPlayOprateLiangGangTou : MahjongPlayOprateBase
    {
        public Dictionary<int, List<MJLiangGangTouData>> playerGangDict = new Dictionary<int, List<MJLiangGangTouData>>();

        public void SetInitData(int type, int seatid, int ruleid, List<int> cards)
        {
            playerGangDict.Clear();

            if (playerGangDict.ContainsKey(seatid))
            {
                MJLiangGangTouData data = GetGangTouData(seatid, type);

                if (data == null)
                {
                    List<MJLiangGangTouData> list = playerGangDict[seatid];

                    if (list == null)
                        list = new List<MJLiangGangTouData>();

                    data = new MJLiangGangTouData();
                    data.SetMJData(type, seatid, ruleid, cards);
                    list.Add(data);
                }
                else
                {
                    data.SetMJData(type, seatid, ruleid, cards);
                }
            }
            else
            {
                List<MJLiangGangTouData> list = new List<MJLiangGangTouData>();

                MJLiangGangTouData data = new MJLiangGangTouData();
                data.SetMJData(type, seatid, ruleid, cards);
                list.Add(data);

                playerGangDict[seatid] = list;
            }
        }

        public MJLiangGangTouData GetGangTouData()
        {
            if (playerGangDict == null)
                return null;

            foreach (var item in playerGangDict.Values)
            {
                return item[0];
            }

            return null;
        }

        private MJLiangGangTouData GetGangTouData(int seatID, int type)
        {
            if (playerGangDict.ContainsKey(seatID))
            {
                List<MJLiangGangTouData> list = playerGangDict[seatID];

                if (list == null)
                    return null;

                for (int i = 0; i < list.Count; i++)
                {
                    if (list[i].selectType == type)
                        return list[i];
                }
            }

            return null;
        }
    }
}
