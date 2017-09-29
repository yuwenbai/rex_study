/**
 * @Author Xin.Wang
 *麻将特殊玩法： 坎牌 闹庄 末留玩法功能类
 *
 */

using System.Collections.Generic;
using projectQ;
using MahjongPlayType;

namespace projectQ
{
    public partial class MahongDataModel
    {
        public partial class GameProcessData
        {
            public MahjongPlayOprateCanNaoMo processKanNaoMo
            {
                get
                {
                    string key = MjPlayOprateType.OPRATE_KANNAOMO.ToString();
                    if (!playOprateDic.ContainsKey(key))
                    {
                        MahjongPlayOprateCanNaoMo item = new MahjongPlayOprateCanNaoMo();
                        playOprateDic.Add(key, item);
                    }

                    return playOprateDic[key] as MahjongPlayOprateCanNaoMo;
                }
            }
        }
    }

    public partial class MjPlayOprateType
    {
        public const string OPRATE_KANNAOMO = "OPRATE_KANNAOMO";
    }

    public partial class MjDataManager
    {
        #region 坎牌 闹庄 末留

        public void SetKanPaiData(bool needAdd, int seatID, bool curState, List<int> valueList, bool haveChoose, bool chooseDefault = false)
        {
            MjData.ProcessData.processKanNaoMo.SetNaoZhuangData(needAdd, seatID, curState, MjCanNaoMoData.EnumCommonType.CanPai, valueList, haveChoose);
        }

        public void SetNaozhuangData(bool needAdd, int seatID, bool curState, List<int> valueList, bool haveChoose, bool chooseDefault = false)
        {
            MjData.ProcessData.processKanNaoMo.SetNaoZhuangData(needAdd, seatID, curState, MjCanNaoMoData.EnumCommonType.NaoZhuang, valueList, haveChoose, chooseDefault);
        }

        public void SetMoLiuData(bool needAdd, int seatID, bool curState, List<int> paiList, bool haveChoose, bool chooseDefault = false)
        {
            MjData.ProcessData.processKanNaoMo.SetNaoZhuangData(needAdd, seatID, curState, MjCanNaoMoData.EnumCommonType.MoLiu, paiList, haveChoose);
        }

        public MjCanNaoMoData GetKanNoMoDataBySeat(int seatID)
        {
            return MjData.ProcessData.processKanNaoMo.GetPlayerDataBySeatID(seatID);
        }

        public bool GetKanNaoMoDataState(int seatID, MjCanNaoMoData.EnumCommonType checkType)
        {
            return MjData.ProcessData.processKanNaoMo.GetPlayerDataStateBySeat(seatID, checkType);
        }

        #endregion
    }
}



namespace MahjongPlayType
{

    /// <summary>
    /// 坎牌 闹庄 末留功能类
    /// </summary>
    public class MjCanNaoMoData
    {
        public MjCanNaoMoData(int seatID)
        {
            this.seatID = seatID;
        }

        public enum EnumCommonType
        {
            Null,
            CanPai,                     //坎牌
            NaoZhuang,                  //闹庄
            MoLiu,                      //末留
        }

        public class CommonData
        {
            public EnumCommonType enumType = EnumCommonType.Null;
            public bool chooseState = false;                            //当前选中状态
            public string chooseName = null;                            //当前选项的名字
            public EnumChooseType chooseType = EnumChooseType.Multy;     //选择框的样式
            public bool chooseDefault = false;                          //选择失效

            public CommonData(EnumCommonType type, bool curState, string name, EnumChooseType chooseType = EnumChooseType.Multy)
            {
                enumType = type;
                chooseState = curState;
                chooseName = name;
            }
        }

        //playerData
        public int seatID = 0;
        public List<CommonData> dataList = new List<CommonData>();
        public List<int> moList = new List<int>();
        public bool hadChoose = false;

        public void SetPlayerCommonData(EnumCommonType type, bool curSate, string name, bool haveChoose = false,
            bool chooseDefault = false, EnumChooseType chooseType = EnumChooseType.Multy)
        {
            CommonData comData = new CommonData(type, curSate, name, chooseType);
            comData.chooseDefault = chooseDefault;
            CommonData original = GetDataByType(type);
            if (original != null)
            {
                dataList.Remove(original);
            }
            else
            {
                if (dataList == null)
                {
                    dataList = new List<CommonData>();
                }
            }

            dataList.Add(comData);

            if (!hadChoose && hadChoose != haveChoose)
            {
                hadChoose = haveChoose;
            }

        }


        /// <summary>
        /// 获取状态
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public bool CheckChooseStateByType(EnumCommonType type)
        {
            CommonData data = GetDataByType(type);
            if (data != null)
            {
                return data.chooseState;
            }
            return false;
        }



        /// <summary>
        /// 获取该数据
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public CommonData GetDataByType(EnumCommonType type)
        {
            if (dataList != null && dataList.Count > 0)
            {
                for (int i = 0; i < dataList.Count; i++)
                {
                    if (dataList[i].enumType == type)
                    {
                        return dataList[i];
                    }
                }
            }

            return null;
        }

    }

    public class MjCanNaoMoServerData
    {
        public enum EnumMjNaozhuangType
        {
            Null,
            NaoType_All = 1,                //都闹庄
            NaoType_None,                   //都不闹
            NaoType_Xian,                   //闲家闹了
            NaoType_XianNone,               //闲家都不闹
        }

        public MjCanNaoMoData.EnumCommonType curType = MjCanNaoMoData.EnumCommonType.Null;
        public int seatID;
        public bool curState;
        public List<int> valueList = new List<int>();
        public bool chooseDefault = false;
        public bool haveChoose = false;
    }

    public class MahjongPlayOprateCanNaoMo : MahjongPlayOprateBase
    {
        public Dictionary<int, MjCanNaoMoData> playerDataDic = new Dictionary<int, MjCanNaoMoData>();

        /// <summary>
        /// 设置坎牌 闹庄 末留 数据
        /// </summary>
        /// <param name="seatID"></param>
        /// <param name="curState"></param>
        public void SetNaoZhuangData(bool needAdd, int seatID, bool curState, MjCanNaoMoData.EnumCommonType curType,
            List<int> paiList, bool haveChoose, bool chooseDefault = false)
        {
            MjCanNaoMoData dataItem = IniPlayerData(seatID);
            if (paiList != null)
            {
                dataItem.moList = paiList;
            }

            if (needAdd || dataItem.GetDataByType(curType) != null)
            {
                string playName = GetPlayNameByType(curType);
                dataItem.SetPlayerCommonData(curType, curState, playName, haveChoose, chooseDefault);
            }

        }

        public MjCanNaoMoData IniPlayerData(int seatID)
        {
            if (!checkDataState())
            {
                playerDataDic = new Dictionary<int, MjCanNaoMoData>();
            }

            if (!playerDataDic.ContainsKey(seatID))
            {
                MjCanNaoMoData itemData = new MjCanNaoMoData(seatID);
                playerDataDic.Add(seatID, itemData);
            }

            return playerDataDic[seatID];
        }


        public MjCanNaoMoData GetPlayerDataBySeatID(int seatID)
        {
            MjCanNaoMoData data = null;
            if (checkDataState())
            {
                if (playerDataDic.ContainsKey(seatID))
                {
                    return playerDataDic[seatID];
                }
            }
            return data;
        }

        public bool GetPlayerDataStateBySeat(int seatID, MjCanNaoMoData.EnumCommonType checkType)
        {
            MjCanNaoMoData data = GetPlayerDataBySeatID(seatID);
            if (data == null)
            {
                return false;
            }

            return data.CheckChooseStateByType(checkType);
        }



        public void ClearMessage()
        {
            if (checkDataState())
            {
                playerDataDic.Clear();
            }
        }


        private bool checkDataState()
        {
            bool state = (playerDataDic != null && playerDataDic.Count > 0);
            return state;
        }


        private string GetPlayNameByType(MjCanNaoMoData.EnumCommonType type)
        {
            string typeName = null;
            switch (type)
            {
                case MjCanNaoMoData.EnumCommonType.CanPai:
                    {
                        typeName = "坎牌";
                    }
                    break;
                case MjCanNaoMoData.EnumCommonType.NaoZhuang:
                    {
                        typeName = "闹庄";
                    }
                    break;
                case MjCanNaoMoData.EnumCommonType.MoLiu:
                    {
                        typeName = "末留";
                    }
                    break;
            }

            return typeName;
        }


    }

}

