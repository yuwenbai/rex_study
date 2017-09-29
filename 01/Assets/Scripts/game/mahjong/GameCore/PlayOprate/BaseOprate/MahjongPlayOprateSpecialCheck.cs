/**
 * @Author Xin.Wang
 * 检测特殊玩法
 *
 */

using System.Collections;
using System.Collections.Generic;
using projectQ;
using MahjongPlayType;


public enum EnumMjSpecialType
{
    Null,
    Hun = 1,                        //混牌
    Jin = 2,                        //金牌
    Ci = 3,                         //次张
    Gui = 4,                        //鬼牌


    Que,                        //缺门
    Mao,                        //放毛
    LiangLiu,                   //亮六
    Mo,                         //抹
    Hun_Laizi,                  //混牌-癞子
    Hun_Caishen,                //混牌-财神
    Hun_JinPai,                 //混牌-金牌
    KanPai,                     //坎牌
    MoLiu,                      //末留
    NaoZhuang,                  //闹庄
    Caishen,                    //财神牌
    HuaPai,                     //花牌
    NiuPai,                     //扭牌
}



namespace projectQ
{

    public class MjStandingPlateData
    {
        //standing
        public int standingMjCode = -1;
        public List<int> standingChangeCode = new List<int>();
        public int standingSpecialType = -1;

        //code
        public int standingOffset = -1;
        public bool standingIsCanget = true;
        public bool standingIsFromBegin = false;

        //色子
        public List<int> standingRolls = new List<int>();
        public int standingSeatID = -1;

        /// <summary>
        /// 设置基本信息
        /// </summary>
        /// <param name="mjCode"></param>
        /// <param name="changeCode"></param>
        /// <param name="type"></param>
        public MjStandingPlateData(int mjCode, List<int> changeCode, int type)
        {
            this.standingMjCode = mjCode;
            this.standingSpecialType = type;
            if (changeCode != null)
            {
                this.standingChangeCode = changeCode;
            }
        }

        /// <summary>
        /// 设置牌墙状态 （可以没有牌墙逻辑）
        /// </summary>
        /// <param name="offset"></param>
        /// <param name="canGet"></param>
        /// <param name="fromBegine"></param>
        public void SetDataFromCode(int offset, bool canGet, bool fromBegine)
        {
            this.standingOffset = offset;
            this.standingIsCanget = canGet;
            this.standingIsFromBegin = fromBegine;
        }

        /// <summary>
        /// 设置色子信息
        /// </summary>
        /// <param name="rolls"></param>
        /// <param name="fromSeatID"></param>
        public void SetDataRoll(List<int> rolls, int fromSeatID)
        {
            this.standingSeatID = fromSeatID;
            if (rolls != null)
            {
                standingRolls = rolls;
            }
        }

    }


    public partial class MahongDataModel
    {
        public partial class GameProcessData
        {
            public MahjongPlayOprateSpecialCheck processSpecialCheck
            {
                get
                {
                    string key = MjPlayOprateType.OPRATE_SPECIALCHECK.ToString();
                    if (!playOprateDic.ContainsKey(key))
                    {
                        MahjongPlayOprateSpecialCheck item = new MahjongPlayOprateSpecialCheck();
                        playOprateDic.Add(key, item);
                    }

                    return playOprateDic[key] as MahjongPlayOprateSpecialCheck;
                }
            }
        }
    }

    public partial class MjPlayOprateType
    {
        public const string OPRATE_SPECIALCHECK = "OPRATE_SPECIALCHECK";
    }


    public partial class MjDataManager
    {
        #region OpenSpecial

        //检查是否为特殊牌
        public EnumMjSpecialType CheckSpecialType(int mjCode, int uiSeatID)
        {
            if (mjCode < 0)
            {
                return EnumMjSpecialType.Null;
            }

            if (uiSeatID < 0 || uiSeatID > 3)
            {
                uiSeatID = MjData.curUserData.selfSeatID;
            }

            //定缺逻辑 
            if (MjData.ProcessData.processQue != null)
            {
                //当前有缺
                return MjData.ProcessData.processQue.CheckProcessQue(mjCode, uiSeatID);
            }

            //定混等逻辑 
            EnumMjSpecialType curType = MjData.ProcessData.processSpecialCheck.CheckProcessSpecial(mjCode);

            //开局转换逻辑
            if (curType == EnumMjSpecialType.Null)
            {
                curType = MjData.ProcessData.processSpecialCheck.GetStartChangeSpecial(mjCode);
            }

            return curType;
        }


        //设置游戏定混今次等状态
        public void SetProcessSpecial(MjStandingPlateData standingData, bool needFireShow)
        {

            MjData.ProcessData.processSpecialCheck.SpecialStandingData(standingData);
            if (standingData != null)
            {
                int deskID = MjData.curUserData.selfDeskID;
                MjData.ProcessData.processSpecialCheck.SetCurSpecialType(standingData.standingSpecialType, deskID);
                if (needFireShow)
                {
                    //fire show
                    EventDispatcher.FireEvent(MJEnum.ProcessBasicEnum.PROBASIC_SPECIAL_DataToLogic.ToString());
                }
            }
        }

        /// <summary>
        /// 获取当前翻开的特殊牌类型
        /// </summary>
        /// <returns></returns>
        public EnumMjSpecialType GetCurOpenSpecialType()
        {
            return MjData.ProcessData.processSpecialCheck.GetCurOpenSpecialType();
        }

        /// <summary>
        /// 获取翻开的特殊牌的转换牌
        /// </summary>
        /// <returns></returns>
        public List<int> GetCurOpenSpecialChangeList()
        {
            return MjData.ProcessData.processSpecialCheck.GetCurOpenSpecialChangeList();
        }


        public MjStandingPlateData GetCurOpenSpecialStanding()
        {
            return MjData.ProcessData.processSpecialCheck.GetOpenSpecialStandingData();
        }

        #endregion

        #region StartChange
        public void SetCurStartChange(int cardID, int showType)
        {
            MjData.ProcessData.processSpecialCheck.SetChangeStartList(cardID, showType);
        }


        public MjChangeStart.EnumChangeStartSend GetStartChangeSendType(int cardID)
        {
            MjChangeStart.EnumChangeStartSend sendType = MjData.ProcessData.processSpecialCheck.GetChangeSend(cardID);
            if (cardID > 34)
            {
                //花牌单做处理
                List<int> changeList = GetCurOpenSpecialChangeList();
                if (changeList.Contains(cardID) && sendType == MjChangeStart.EnumChangeStartSend.MjChangeType_Change)
                {
                    sendType = MjChangeStart.EnumChangeStartSend.MjChangeType_Put;
                }
            }
            return sendType;
        }

        #endregion
    }

}


namespace MahjongPlayType
{
    /// <summary>
    /// 翻开产生的特殊牌
    /// </summary>
    public class OpenSpecialData
    {
        public MjStandingPlateData standingData = null;
        public EnumMjSpecialType curSpecialType = EnumMjSpecialType.Null;       //定特殊的类型

        public void SetStandingData(MjStandingPlateData dataItem)
        {
            standingData = dataItem;
            if (dataItem != null)
            {
                curSpecialType = (EnumMjSpecialType)standingData.standingSpecialType;
            }
        }


        public EnumMjSpecialType CheckProcessSpecial(int mjCode)
        {
            EnumMjSpecialType returnType = EnumMjSpecialType.Null;
            if (curSpecialType == EnumMjSpecialType.Null)
            {
                returnType = curSpecialType;
            }
            else
            {
                if (standingData != null)
                {
                    if (standingData.standingChangeCode != null && standingData.standingChangeCode.Count > 0)
                    {
                        returnType = standingData.standingChangeCode.Contains(mjCode) ? curSpecialType : EnumMjSpecialType.Null;
                    }
                }
            }

            return returnType;
        }

        public void SetCurSpecialType(int type, int deskID)
        {
            EnumMjSpecialType curType = (EnumMjSpecialType)type;
            if (curType == EnumMjSpecialType.Hun)
            {
                int gameType = MemoryData.DeskData.GetOneDeskInfo(deskID).mjGameType;

                int hunType = CardHelper.GetHunType(gameType);
                if (hunType == 1)
                {
                    curType = EnumMjSpecialType.Hun_Caishen;
                }
                if (hunType == 2)
                {
                    curType = EnumMjSpecialType.Hun_Laizi;
                }
                if (hunType == 3)
                {
                    curType = EnumMjSpecialType.Hun_JinPai;
                }
            }
            curSpecialType = curType;
        }


        public EnumMjSpecialType GetCurSpecailType()
        {
            return curSpecialType;
        }

    }

    /// <summary>
    /// 开局转换类型
    /// </summary>
    public class MjChangeStart
    {
        public enum EnumChangeStart
        {
            MjBeginChangeType_FLOWER = 1,      //花
        }

        public enum EnumChangeStartSend
        {
            MjChangeType_Put,              //出牌
            MjChangeType_Change,            ///补牌
        }


        public int rulerType;           //展示类型
        public int cardID;              //牌的ID
        public EnumChangeStart changeType;          //牌的花色类型

        public MjChangeStart(int cardID, int showType)
        {
            this.cardID = cardID;
            this.changeType = (EnumChangeStart)showType;
        }
    }


    public class MahjongPlayOprateSpecialCheck : MahjongPlayOprateBase
    {

        #region OpenSpecial
        private OpenSpecialData _openSpecialData = new OpenSpecialData();

        /// <summary>
        /// 检查某张牌是否在翻开数据中为特殊
        /// </summary>
        /// <param name="cardID"></param>
        /// <returns></returns>
        public EnumMjSpecialType CheckProcessSpecial(int cardID)
        {
            return _openSpecialData.CheckProcessSpecial(cardID);
        }

        /// <summary>
        /// 设置基本数据 
        /// </summary>
        /// <param name="standingData"></param>
        public void SpecialStandingData(MjStandingPlateData standingData)
        {
            _openSpecialData.SetStandingData(standingData);
        }

        /// <summary>
        /// 设置当前翻开的特殊类型 
        /// </summary>
        /// <param name="type"></param>
        /// <param name="deskID"></param>
        public void SetCurSpecialType(int type, int deskID)
        {
            _openSpecialData.SetCurSpecialType(type, deskID);
        }

        /// <summary>
        /// 获取当前翻开的特殊类型 
        /// </summary>
        /// <returns></returns>
        public EnumMjSpecialType GetCurOpenSpecialType()
        {
            return _openSpecialData.GetCurSpecailType();
        }

        /// <summary>
        /// 获取翻开的特殊牌的转换牌
        /// </summary>
        /// <returns></returns>
        public List<int> GetCurOpenSpecialChangeList()
        {
            List<int> changeList = new List<int>();
            if (_openSpecialData.standingData != null)
            {
                changeList = _openSpecialData.standingData.standingChangeCode;
            }
            return changeList;
        }

        public MjStandingPlateData GetOpenSpecialStandingData()
        {
            return _openSpecialData.standingData;
        }
        #endregion

        #region StartChange
        private Dictionary<int, MjChangeStart> _changeStartDic = new Dictionary<int, MjChangeStart>();


        public void SetChangeStartList(int changeCard, int changeType)
        {
            MjChangeStart startItem = new MjChangeStart(changeCard, changeType);
            if (!_changeStartDic.ContainsKey(changeCard))
            {
                _changeStartDic.Add(changeCard, startItem);
            }
            else
            {
                _changeStartDic[changeCard] = startItem;
            }
        }

        public EnumMjSpecialType GetStartChangeSpecial(int cardID)
        {
            EnumMjSpecialType showType = EnumMjSpecialType.Null;
            if (_changeStartDic.ContainsKey(cardID))
            {
                MjChangeStart.EnumChangeStart enumType = _changeStartDic[cardID].changeType;
                switch (enumType)
                {
                    case MjChangeStart.EnumChangeStart.MjBeginChangeType_FLOWER:
                        {
                            showType = EnumMjSpecialType.HuaPai;
                        }
                        break;
                }
            }

            return showType;
        }


        public MjChangeStart.EnumChangeStartSend GetChangeSend(int cardID)
        {
            MjChangeStart.EnumChangeStartSend SendType = MjChangeStart.EnumChangeStartSend.MjChangeType_Put;
            if (_changeStartDic.ContainsKey(cardID))
            {
                MjChangeStart.EnumChangeStart enumType = _changeStartDic[cardID].changeType;
                switch (enumType)
                {
                    case MjChangeStart.EnumChangeStart.MjBeginChangeType_FLOWER:
                        {
                            SendType = MjChangeStart.EnumChangeStartSend.MjChangeType_Change;
                        }
                        break;
                }
            }
            else
            {
                if (cardID > 34)
                {
                    //花牌某些情况单算
                    SendType = MjChangeStart.EnumChangeStartSend.MjChangeType_Change;
                }
            }

            return SendType;
        }

        #endregion


    }

}