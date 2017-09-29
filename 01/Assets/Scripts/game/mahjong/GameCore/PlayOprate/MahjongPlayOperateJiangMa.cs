using System.Collections.Generic;
using MahjongPlayType;


namespace projectQ
{
    public partial class MahongDataModel
    {
        public partial class GameProcessData
        {
            public MahjongPlayOperateJiangMa processJiangMa
            {
                get
                {
                    string key = MjPlayOprateType.OPRATE_JIANGMA.ToString();
                    if (!playOprateDic.ContainsKey(key))
                    {
                        MahjongPlayOperateJiangMa item = new MahjongPlayOperateJiangMa();
                        playOprateDic.Add(key, item);
                    }

                    return playOprateDic[key] as MahjongPlayOperateJiangMa;
                }
            }
        }
    }

    public partial class MjPlayOprateType
    {
        public const string OPRATE_JIANGMA = "OPRATE_JIANGMA";
    }

    public partial class MjDataManager
    {
        public void InitSetJiangMaData(int type, List<MjHorse> horse, List<MjScore> score)
        {
            MjData.ProcessData.processJiangMa.InitSetData(type, horse, score);
        }

        public string GetJiangMaNameByType(EnumMjOpenMaType type)
        {
            string titName = "";
            switch (type)
            {
                case EnumMjOpenMaType.ZhuaMa:
                    titName = "抓马";
                    break;
                case EnumMjOpenMaType.JiangMa:
                    titName = "奖马";
                    break;
                case EnumMjOpenMaType.BaoZhaMa:
                    titName = "爆炸马";
                    break;
            }
            return titName;
        }
    }
}

namespace MahjongPlayType
{
    public class MjJiangMaData
    {
        private List<MjHorse> m_CardList;
        public List<MjHorse> GetCards
        {
            get
            {
                return m_CardList;
            }
        }

        private List<MjScore> m_ScoreList;
        public List<MjScore> GetScore
        {
            get
            {
                return m_ScoreList;
            }
        }

        private int m_Type;
        public int GetHorseType
        {
            get
            {
                return m_Type;
            }
        }

        public MjJiangMaData(int type, List<MjHorse> horse, List<MjScore> score)
        {
            m_Type = type;
            
            if (horse != null)
                m_CardList = horse;
            else
                m_CardList = new List<MjHorse>();

            if (score != null)
                m_ScoreList = score;
            else
                m_ScoreList = new List<MjScore>();
        }

        public void SetCards(List<MjHorse> horse)
        {
            if (horse != null)
                m_CardList = horse;
            else
                m_CardList = new List<MjHorse>();
        }

        public void SetScore(List<MjScore> score)
        {
            if (score != null)
                m_ScoreList = score;
            else
                m_ScoreList = new List<MjScore>();
        }
    }

    public class MahjongPlayOperateJiangMa : MahjongPlayOprateBase
    {
        public Dictionary<int, MjJiangMaData> dataDict = new Dictionary<int, MjJiangMaData>();

        public void InitSetData(int type, List<MjHorse> horse, List<MjScore> score)
        {
            if (dataDict.ContainsKey(type))
            {
                MjJiangMaData data = dataDict[type];
                data.SetCards(horse);
                data.SetScore(score);
                dataDict[type] = data;
            }
            else
            {
                MjJiangMaData data = new MjJiangMaData(type, horse, score);
                dataDict[type] = data;
            }
        }

        public MjJiangMaData GetJiangMaDataByType(int type)
        {
            if (dataDict.ContainsKey(type))
            {
                return dataDict[type];
            }
            else
            {
                return null;
            }
        }
    }
}

public class MjHorse
{
    public int seatID;
    public List<int> maCode;
    public List<EnumMjBuyhorseStateType> horseType = new List<EnumMjBuyhorseStateType>();

    public MjHorse(int seatID, List<int> maCode, List<int> horseType)
    {
        this.seatID = seatID;
        this.maCode = maCode;
        if (horseType != null && horseType.Count > 0)
        {
            for (int i = 0; i < horseType.Count; i++)
            {
                this.horseType.Add((EnumMjBuyhorseStateType)horseType[i]);
            }
        }
    }

    //for self
    public string headUrl = null;
}

