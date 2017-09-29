
using System.Collections.Generic;
using MahjongPlayType;
using projectQ;
using Msg;

namespace projectQ
{
    public partial class MahongDataModel
    {
        public partial class GameProcessData
        {
            public MahjongPlayOperateMingDa processMingDaCard
            {
                get
                {
                    string key = MjPlayOprateType.OPRATE_MingDaCard.ToString();
                    if (!playOprateDic.ContainsKey(key))
                    {
                        MahjongPlayOperateMingDa item = new MahjongPlayOperateMingDa();
                        playOprateDic.Add(key, item);
                    }

                    return playOprateDic[key] as MahjongPlayOperateMingDa;
                }
            }
        }
    }


    public partial class MjPlayOprateType
    {
        public const string OPRATE_MingDaCard = "OPRATE_MingDaCard";
    }


    public partial class MjDataManager
    {
        public bool isOutMingDa = false;

        public bool CheckHaveMingDa(int id)
        {
            bool result = false;
            switch (id)
            {
                case (int)MjXuanPiaoData.EnumCommonType.MingDa:
                    result = true;
                    break;
                default:
                    break;
            }
            return result;
        }

        public void SetAllMingDaData(int CurType, List<MjMingDaServerData> dataList)
        {
            if (dataList != null && dataList.Count > 0)
            {
                for (int i = 0; i < dataList.Count; i++)
                {
                    MjMingDaServerData dataItem = dataList[i];
                    int rulerID = dataItem.commonData.rulerID;
                    List<int> valueList = dataItem.commonData.valueList;

                    for (int j = 0; j < dataItem.playerData.Count; j++)
                    {
                        InitMingDaData(dataItem.playerData[j].seatID, CurType, dataItem.playerData[j].curValue,
                            dataItem.playerData[j].haveChoose, dataItem.commonData.rulerID, dataItem.commonData.valueList);
                    }
                }
            }
        }

        private void InitMingDaData(int seatID, int typeID, int curValue, bool isSelect, int ruleID, List<int> values)
        {
            MjData.ProcessData.processMingDaCard.SetMingDaData(true, seatID, curValue, isSelect, typeID, ruleID, values);
        }

        public void UpdateMingDaData(int seatID, int ruleID, int curValue)
        {
            MjData.ProcessData.processMingDaCard.SetMingDaData(false, seatID, curValue, true, -1, ruleID, null);
        }

        public MjMingDaData GetMainPlayer()
        {
            if (MjData.ProcessData.processMingDaCard.playDataDic != null)
            {
                if (MjData.ProcessData.processMingDaCard.playDataDic.ContainsKey(MjData.curUserData.selfSeatID))
                    return MjData.ProcessData.processMingDaCard.playDataDic[MjData.curUserData.selfSeatID];
            }
            return null;
        }

        public MjMingDaData GetPlayerBySeatID(int seatID)
        {
            if (MjData.ProcessData.processMingDaCard.playDataDic != null)
            {
                if (MjData.ProcessData.processMingDaCard.playDataDic.ContainsKey(seatID))
                    return MjData.ProcessData.processMingDaCard.playDataDic[seatID];
            }
            return null;
        }


        public string GetSpriteNameByType(EnumMjSpecialCheck type)
        {
            string spriteName = "";

            switch (type)
            {
                case EnumMjSpecialCheck.MjBaseCheckType_MingDa:
                    {
                        spriteName = "desk_icon_mingda";
                    }
                    break;
                case EnumMjSpecialCheck.MjBaseCheckType_XiaPao:
                case EnumMjSpecialCheck.MjBaseCheckType_XinXiaPiao:
                    {
                        spriteName = "mj_icon_xiapao";
                    }
                    break;
                case EnumMjSpecialCheck.MJBaseCheckType_XiaPaoZi:
                    {
                        spriteName = "mj_icon_xiapaozi";
                    }
                    break;
                case EnumMjSpecialCheck.MjBaseCheckType_XiaYuNingXia:
                case EnumMjSpecialCheck.MjBaseCheckType_XiaYu:
                    {
                        spriteName = "mj_icon_yu";
                    }
                    break;
                case EnumMjSpecialCheck.MjBaseCheckType_XiaPiao:
                    {
                        spriteName = "mj_icon_piao";
                    }
                    break;
                case EnumMjSpecialCheck.MjBaseCheckType_XiaBang:
                    {
                        spriteName = "mj_icon_bang";
                    }
                    break;
                case EnumMjSpecialCheck.MjBaseCheckType_XiaMa:
                    {
                        spriteName = "mj_icon_xiama";
                    }
                    break;
            }

            return spriteName;
        }

        public string GetSpriteNameByType(EnumMjHuaType type)
        {
            string spriteName = "";

            switch (type)
            {
                case EnumMjHuaType.Wan:
                    {
                        spriteName = "desk_icon_wan";
                    }
                    break;
                case EnumMjHuaType.Tong:
                    {
                        spriteName = "desk_icon_tong";
                    }
                    break;
                case EnumMjHuaType.Tiao:
                    {
                        spriteName = "desk_icon_tiao";
                    }
                    break;
            }

            return spriteName;
        }

        private Dictionary<int, int> m_OprateIDDict = new Dictionary<int, int>();
        public Dictionary<int,int > oprateIDDict
        {
            get
            {
                if(m_OprateIDDict == null || m_OprateIDDict.Count == 0)
                {
                    m_OprateIDDict = new Dictionary<int, int>();

                    m_OprateIDDict[20063] = 19;

                }
                return m_OprateIDDict;
            }
        }

        private Dictionary<int, MjOprateXMLData> m_OprateXMLData = new Dictionary<int, MjOprateXMLData>();
        public Dictionary<int, MjOprateXMLData> oprateXMLData
        {
            get
            {
                if(m_OprateXMLData == null || m_OprateXMLData.Count == 0)
                {
                    m_OprateXMLData = new Dictionary<int, MjOprateXMLData>();

                    MjOprateXMLData data = new MjOprateXMLData(20063, 19, "", new string[3] { "xia", "pao", "zhong" }, new string[3] { "yi", "xia", "pao" });
                    m_OprateXMLData[19] = data;
                }
                return m_OprateXMLData;
            }
        }

        public MjOprateXMLData GetOprateDataByOprateID(int type)
        {
            if (MjDataManager.Instance.oprateIDDict.ContainsKey(type))
            {
                int typeID = MjDataManager.Instance.oprateIDDict[type];

                if (MjDataManager.Instance.oprateXMLData.ContainsKey(typeID))
                {
                    var mjData = MjDataManager.Instance.oprateXMLData[typeID];
                    return mjData;
                }
            }
            return null;
        }

        public MjOprateXMLData GetOprateDataByTypeID(int type)
        {
            if (MjDataManager.Instance.oprateXMLData.ContainsKey(type))
            {
                var mjData = MjDataManager.Instance.oprateXMLData[type];
                return mjData;
            }
            return null;
        }

        public class MjOprateXMLData
        {
            public int oprateTypeID;
            public string oprateName;
            public string[] selectWait;
            public string[] selected;

            public MjOprateXMLData(int id, int type, string name, string[] select, string[] selectover)
            {
                oprateTypeID = type;
                oprateName = name;
                selectWait = select;
                selected = selectover;
            }
        }

    }

}

namespace MahjongPlayType
{
    public class MjMingDaData
    {
        public class CommonData
        {
            public MjXuanPiaoData.EnumCommonType enumType = MjXuanPiaoData.EnumCommonType.Null;   //玩法类型
            public bool counTain = true;        //是否包含该玩法
            public List<int> chooseVlaueList = new List<int>();             //选择数值列表
            public int curChooseValue = 0;                                  //当前值    
            public string OprateName = string.Empty;                        //玩法名称 飘、跑    
            public bool isSelect = false;                                       //是否完成选择
        }

        public int SeatID = -1;
        public EnumMjSelectSubType SelectState = EnumMjSelectSubType.MjSelectSubType_NoOperation;

        public EnumChooseType chooseType = EnumChooseType.Single;        //UI表现方式 
        public List<CommonData> Data = new List<CommonData>();

        public MjMingDaData(int seatID)
        {
            SeatID = seatID;
        }

        public void InitMDData(int value, bool isSelect, int curState, MjXuanPiaoData.EnumCommonType ruleType, List<int> values)
        {
            SelectState = (EnumMjSelectSubType)curState;

            CommonData data = GetCommonData(ruleType);
            if(data == null)
            {
                data = new CommonData();
                data.enumType = ruleType;
                data.chooseVlaueList = values;
                data.curChooseValue = value;
                data.isSelect = isSelect;
                Data.Add(data);
            }
            else
            {
                data.curChooseValue = value;
                data.isSelect = isSelect;
            }
        }

        public CommonData GetCommonData(MjXuanPiaoData.EnumCommonType ruleType)
        {
            if (Data != null && Data.Count > 0)
            {
                for (int i = 0; i < Data.Count; i++)
                {
                    if (Data[i].enumType == ruleType)
                    {
                        return Data[i];
                    }
                }
            }

            return null;
        }

        public void UpdateData(int value, bool isSelect, MjXuanPiaoData.EnumCommonType ruleType)
        {
            CommonData data = GetCommonData(ruleType);
            if (data != null)
            {
                data.curChooseValue = value;
                data.isSelect = isSelect;
            }
        }
    }

    public class MjMingDaServerData
    {
        public class RulerCommonData
        {
            public int rulerID;
            public int showType;
            public List<int> valueList = new List<int>();
        }

        public class RulerPlayerData
        {
            public int seatID = 0;
            public int curValue = 0;
            public bool haveChoose = false;
        }


        public RulerCommonData commonData = null;
        public void SetCommonData(int rulerID, int showType, List<int> valueList)
        {
            commonData = new RulerCommonData();
            commonData.rulerID = rulerID;
            commonData.showType = showType;
            commonData.valueList = valueList;
        }

        public List<RulerPlayerData> playerData = new List<RulerPlayerData>();

        public void AddPlayerData(int seatID, int curValue, bool haveChoose)
        {
            RulerPlayerData item = new RulerPlayerData();
            item.seatID = seatID;
            item.curValue = curValue;
            item.haveChoose = haveChoose;
            playerData.Add(item);

        }

    }

    public class MahjongPlayOperateMingDa : MahjongPlayOprateBase
    {
        public Dictionary<int, MjMingDaData> playDataDic = new Dictionary<int, MjMingDaData>();

        public void SetMingDaData(bool isAdd, int uiSeatID, int curValue, bool isSelect, int curType, int ruleID, List<int> values)
        {
            MjXuanPiaoData.EnumCommonType ruleType = GetRuleTypeEnum(ruleID);
            if (isAdd)
            {
                MjMingDaData mdData = InitOrGetData(uiSeatID);

                mdData.InitMDData(curValue, isSelect, curType, ruleType, values);
            }
            else
            {
                MjMingDaData mdData = InitOrGetData(uiSeatID);

                mdData.UpdateData(curValue, isSelect, ruleType);
            }
        }

        public MjMingDaData GetMDDataByUISeatID(int uiSeatID)
        {
            MjMingDaData data = null;
            if(playDataDic!= null)
            {
                if (playDataDic.ContainsKey(uiSeatID))
                {
                    data = playDataDic[uiSeatID];
                }
            }

            return data;
        }

        private MjMingDaData InitOrGetData(int uiSeatID)
        {
            if (playDataDic == null)
            {
                playDataDic = new Dictionary<int, MjMingDaData>();
            }

            if(!playDataDic.ContainsKey(uiSeatID))
            {
                playDataDic[uiSeatID] = new MjMingDaData(uiSeatID);
            }

            return playDataDic[uiSeatID];
        }

        private MjXuanPiaoData.EnumCommonType GetRuleTypeEnum(int ruleID)
        {
            return (MjXuanPiaoData.EnumCommonType)ruleID;
        }

        public void SendMingDaSelect(int deskID, int seatID, List<int> rulerIDList, List<int> valueList)
        {
            var req = new MjReqGameingRulerSet();
            req.SeatID = seatID;
            req.DeskID = deskID;

            if (rulerIDList != null && valueList != null && rulerIDList.Count == valueList.Count)
            {
                for (int i = 0; i < rulerIDList.Count; i++)
                {
                    MjRulerSetSelectData dataItem = new MjRulerSetSelectData();
                    dataItem.nRulerID = rulerIDList[i];
                    dataItem.nValue = valueList[i];
                    req.SelectData.Add(dataItem);
                }

            }

            ModelNetWorker.Send(req);
        }

        public string GetBtnName(EnumMjSpecialCheck type, int value)
        {
            string result = "";

            switch (type)
            {
                case EnumMjSpecialCheck.MjBaseCheckType_MingDa:
                    switch (value)
                    {
                        case 1:
                            {
                                result = "明打";
                            }
                            break;
                        case 2:
                            {
                                result = "不明";
                            }
                            break;
                    }

                    break;
            }

            return result;
        }
    }

}

