/**
 * @Author Xin.Wang
 *麻将特殊玩法： 胡飘 杠飘 飘素自摸  飘金钓鱼
 *
 */

using System.Collections.Generic;
using MahjongPlayType;


namespace projectQ
{
    public partial class MahongDataModel
    {
        public partial class GameProcessData
        {
            public MahjongPlayOprateXuanPiao processXuanPiao
            {
                get
                {
                    string key = MjPlayOprateType.OPRATE_XUANPIAO.ToString();
                    if (!playOprateDic.ContainsKey(key))
                    {
                        MahjongPlayOprateXuanPiao item = new MahjongPlayOprateXuanPiao();
                        playOprateDic.Add(key, item);
                    }

                    return playOprateDic[key] as MahjongPlayOprateXuanPiao;
                }
            }
        }
    }


    public partial class MjPlayOprateType
    {
        public const string OPRATE_XUANPIAO = "OPRATE_XUANPIAO";
    }



    public partial class MjDataManager
    {
        #region  选飘(胡飘 杠飘 飘金钓鱼 飘素自摸 跑头 跑杠)

        public void SetXuanPiaoData(int seatID, int typeID, int curValue, List<int> valueList, bool haveChoose, int showType)
        {
            MjData.ProcessData.processXuanPiao.SetXuanPiaoData(true, seatID, typeID, curValue, valueList, haveChoose, showType);
        }

        public void UpdateXuanPiaoData(int seatID, int typeID, int curValue)
        {
            MjData.ProcessData.processXuanPiao.SetXuanPiaoData(false, seatID, typeID, curValue, null, true, -1);
        }

        public MjXuanPiaoData GetXuanPiaoDataBySeatID(int seatID)
        {
            return MjData.ProcessData.processXuanPiao.GetPlayerData(seatID);
        }


        public EnumMjSpecialCheck GetShowTipTypeByDataType(MjXuanPiaoData.EnumCommonType type)
        {

            EnumMjSpecialCheck showType = EnumMjSpecialCheck.Null;
            switch (type)
            {
                case MjXuanPiaoData.EnumCommonType.HuPiao:
                    {
                        showType = EnumMjSpecialCheck.MjBaseCheckType_PiaoHu;
                    }
                    break;
                case MjXuanPiaoData.EnumCommonType.GangPiao:
                    {
                        showType = EnumMjSpecialCheck.MjBaseCheckType_PiaoGang;
                    }
                    break;
                case MjXuanPiaoData.EnumCommonType.Piaojin:
                    {
                        showType = EnumMjSpecialCheck.MjBaseCheckType_PiaoJin;
                    }
                    break;
                case MjXuanPiaoData.EnumCommonType.PiaoSu:
                    {
                        showType = EnumMjSpecialCheck.MjBaseCheckType_PiaoSu;
                    }
                    break;
                case MjXuanPiaoData.EnumCommonType.PaoKou:
                    {
                        showType = EnumMjSpecialCheck.MjBaseCheckType_PaoKou;
                    }
                    break;
                case MjXuanPiaoData.EnumCommonType.PaoTou:
                    {
                        showType = EnumMjSpecialCheck.MjBaseCheckType_PaoTou;
                    }
                    break;
                case MjXuanPiaoData.EnumCommonType.XiaPiao:
                    {
                        showType = EnumMjSpecialCheck.MjBaseCheckType_XiaPiao;
                    }
                    break;
                case MjXuanPiaoData.EnumCommonType.XiaBang:
                    {
                        showType = EnumMjSpecialCheck.MjBaseCheckType_XiaBang;
                    }
                    break;
                case MjXuanPiaoData.EnumCommonType.XinXiaPiao:
                    {
                        showType = EnumMjSpecialCheck.MjBaseCheckType_XinXiaPiao;
                    }
                    break;
                case MjXuanPiaoData.EnumCommonType.XiaYuNingXia:
                    {
                        showType = EnumMjSpecialCheck.MjBaseCheckType_XiaYuNingXia;
                    }
                    break;
            }

            return showType;
        }

        #endregion
    }

}

namespace MahjongPlayType
{
    public enum EnumThreeStepSelect
    {
        Null = 0,
        Start,
        WaitOther,
        Close
    }

    public enum EnumChooseType
    {
        Single = 1,                     //单选框
        Multy = 2,                      //复选框
    }

    public class MjXuanPiaoData
    {
        public MjXuanPiaoData(int seatID)
        {
            this.seatID = seatID;
        }

        public enum EnumCommonType
        {
            Null,
            XiaPao = 20012,                 //下跑

            HuPiao = 20038,                 //胡飘
            GangPiao = 20039,               //杠飘
            Piaojin = 20040,                //飘金钓鱼
            PiaoSu = 20041,                 //飘素自摸

            PaoKou = 20043,                //跑扣
            PaoTou = 20044,                 //跑头

            XiaPiao = 20053,                   //下漂
            XiaMa = 20055,                      //下码
            MingDa = 20058,                         //明打
            XiaBang = 20059,                        //下绑

            XinXiaPiao = 20063,                     //新下跑
            XiaYuNingXia = 20073,                          //宁夏下鱼
        }


        public class CommonData
        {
            public EnumCommonType enumType = EnumCommonType.Null;   //玩法类型
            public string typeName = null;      //玩法名称
            public bool counTain = true;        //是否包含该玩法
            public List<int> chooseVlaueList = new List<int>();             //选择数值列表
            public int curChooseValue = 0;                                  //当前值    
            public string OprateName = string.Empty;                        //玩法名称(选项) 飘、跑    
            public string TitleName = string.Empty;                         //玩法名称(标题) 飘、跑    
        }


        public void IniCommonData(EnumCommonType type, string name, int curValue, List<int> valueList, bool haveChoose, int showType)
        {
            CommonData item = GetDataByType(type);
            if (item == null)
            {
                CommonData data = new CommonData();
                data.typeName = name;
                data.enumType = type;
                valueList.Sort();
                data.chooseVlaueList = valueList;
                data.curChooseValue = curValue;
                this.chooseType = (EnumChooseType)showType;

                if (type != EnumCommonType.Null)
                {
                    if (type <= EnumCommonType.PiaoSu)
                    {
                        data.OprateName = "飘";
                        data.TitleName = "下飘";
                    }
                    else
                    {
                        data.OprateName = "跑";
                        data.TitleName = "下跑";
                    }
                }

                Data.Add(data);
            }
            else
            {
                item.curChooseValue = curValue;
            }

            if (!hadChoose && haveChoose)
            {
                hadChoose = haveChoose;
            }

        }


        public void UpdateCommonData(EnumCommonType enumType, int curValue)
        {
            CommonData data = GetDataByType(enumType);
            if (data != null)
            {
                data.curChooseValue = curValue;
                this.hadChoose = true;
            }

        }

        private CommonData GetDataByType(EnumCommonType searchType)
        {

            if (Data != null && Data.Count > 0)
            {
                for (int i = 0; i < Data.Count; i++)
                {
                    if (Data[i].enumType == searchType)
                    {
                        return Data[i];
                    }
                }
            }

            return null;
        }

        //property
        public EnumChooseType chooseType = EnumChooseType.Multy;        //UI表现方式 
        public List<CommonData> Data = new List<CommonData>();
        public bool hadChoose = false;


        public int seatID = 0;
    }

    public class MjXuanPiaoServerData
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

    public class MahjongPlayOprateXuanPiao : MahjongPlayOprateBase
    {
        public Dictionary<int, MjXuanPiaoData> playDataDic = new Dictionary<int, MjXuanPiaoData>();


        public void SetXuanPiaoData(bool needAdd, int seatID, int typeID, int curValue, List<int> valueData, bool haveChoose, int showType)
        {
            MjXuanPiaoData.EnumCommonType curType = GetPlayTypeEnum(typeID);
            if (needAdd)
            {
                MjXuanPiaoData data = IniOrGetData(seatID);

                string typeName = GetPlayTypeName(curType);
                data.IniCommonData(curType, typeName, curValue, valueData, haveChoose, showType);
            }
            else
            {
                UpdateData(curType, seatID, curValue);
            }
        }



        public MjXuanPiaoData GetPlayerData(int seatID)
        {
            MjXuanPiaoData data = null;
            if (checkDataState())
            {
                if (playDataDic.ContainsKey(seatID))
                {
                    return playDataDic[seatID];
                }
            }


            return data;
        }

        private MjXuanPiaoData IniOrGetData(int seatID)
        {
            if (!checkDataState())
            {
                playDataDic = new Dictionary<int, MjXuanPiaoData>();
            }

            if (!playDataDic.ContainsKey(seatID))
            {
                MjXuanPiaoData itemData = new MjXuanPiaoData(seatID);
                playDataDic.Add(seatID, itemData);
            }

            return playDataDic[seatID];
        }

        private MjXuanPiaoData UpdateData(MjXuanPiaoData.EnumCommonType type, int seatID, int curValue)
        {
            MjXuanPiaoData data = GetPlayerData(seatID);
            if (data != null)
            {
                data.UpdateCommonData(type, curValue);
            }
            return data;
        }

        private bool checkDataState()
        {
            bool state = (playDataDic != null && playDataDic.Count > 0);
            return state;
        }



        private MjXuanPiaoData.EnumCommonType GetPlayTypeEnum(int playID)
        {
            MjXuanPiaoData.EnumCommonType type = (MjXuanPiaoData.EnumCommonType)playID;
            return type;
        }


        private string GetPlayTypeName(MjXuanPiaoData.EnumCommonType playType)
        {
            string playName = null;
            switch (playType)
            {
                case MjXuanPiaoData.EnumCommonType.HuPiao:
                    {
                        playName = "胡      飘";
                    }
                    break;
                case MjXuanPiaoData.EnumCommonType.GangPiao:
                    {
                        playName = "杠      飘";
                    }
                    break;
                case MjXuanPiaoData.EnumCommonType.Piaojin:
                    {
                        playName = "飘金钓鱼";
                    }
                    break;
                case MjXuanPiaoData.EnumCommonType.PiaoSu:
                    {
                        playName = "飘素自摸";
                    }
                    break;
                case MjXuanPiaoData.EnumCommonType.PaoTou:
                    {
                        playName = "跑      头";
                    }
                    break;
                case MjXuanPiaoData.EnumCommonType.PaoKou:
                    {
                        playName = "跑      扣";
                    }
                    break;
            }


            return playName;
        }

    }

}
