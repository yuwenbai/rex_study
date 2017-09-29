/**
 * @Author Xin.Wang
 * 亮一张玩法类
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
            public MahjongPlayOprateLiangYi processLiangYi
            {
                get
                {
                    string key = MjPlayOprateType.OPRATE_LIANGYI.ToString();
                    if (!playOprateDic.ContainsKey(key))
                    {
                        MahjongPlayOprateLiangYi item = new MahjongPlayOprateLiangYi();
                        playOprateDic.Add(key, item);
                    }

                    return playOprateDic[key] as MahjongPlayOprateLiangYi;
                }
            }
        }
    }

    public partial class MjPlayOprateType
    {
        public const string OPRATE_LIANGYI = "OPRATE_LIANGYI";
    }

    /// <summary>
    /// 管理
    /// </summary>
    public partial class MjDataManager
    {
        #region Message
        /// <summary>
        /// 网络消息状态 
        /// </summary>
        public void LiangyiNetSetData(int selectType, List<MjLiangyiData> dataList, bool isNormalLogic)
        {
            MjData.ProcessData.processLiangYi.IniDataModel(selectType, dataList, isNormalLogic);
            if (isNormalLogic)
            {
                //fire show ui
                EventDispatcher.FireEvent(MJEnum.MjLiangyiEvent.MJLY_LogicShowUI.ToString());
            }
        }


        public void LiangyiNetUpdateData(int seatID, int curValue)
        {
            MjData.ProcessData.processLiangYi.IniOrUpdateLiangyi(seatID, curValue, true);

            //fire update
            EventDispatcher.FireEvent(MJEnum.MjLiangyiEvent.MJLY_LogicUpdateUI.ToString(), seatID);
        }

        #endregion


        #region UI
        public void LiangyiUISetChooseState(bool state, int chooseCardID, int chooseIndex)
        {
            int selfSeat = MjData.curUserData.selfSeatID;
            if (!state)
            {
                chooseIndex = chooseCardID = -1;
            }

            MjData.ProcessData.processLiangYi.IniOrUpdateLiangyi(selfSeat, chooseCardID, false);
            MjData.ProcessData.processLiangYi.SetLiangYiIndex(selfSeat, chooseIndex);
            MjData.ProcessData.processLiangYi.uiChooseState = state;

            //fire ui state change
            EventDispatcher.FireEvent(MJEnum.MjLiangyiEvent.MJLY_UIChangeState.ToString());
        }


        public void LiangyiUISendChooseResult()
        {
            int selfSeat = MjData.curUserData.selfSeatID;
            bool canSend = MjData.ProcessData.processLiangYi.CheckSendData(selfSeat);
            if (canSend)
            {
                //fire send
                EventDispatcher.FireEvent(MJEnum.MjLiangyiEvent.MJLY_LogicSendData.ToString());
            }
        }

        #endregion
    }
}


namespace MahjongPlayType
{
    public class MjLiangyiData
    {
        public int seatID = 0;              //座位号
        public bool haveChoose = false;         //是否已经选择
        public int chooseIndex = 0;         //选择的索引
        public int chooseCard = -1;          //选择的ID

        public MjLiangyiData(int seatID)
        {
            this.seatID = seatID;
        }
    }


    public class MahjongPlayOprateLiangYi : MahjongPlayOprateBase
    {
        private Dictionary<int, MjLiangyiData> _dataDic = new Dictionary<int, MjLiangyiData>();
        public bool curISReconned = false;


        /// <summary>
        /// UI可选状态
        /// </summary>
        public bool uiChooseState = false;
        /// <summary>
        /// 当前选择阶段
        /// </summary>
        public EnumMjSelectSubType selectSubType = EnumMjSelectSubType.MjSelectSubType_NoOperation;

        /// <summary>
        /// 流程初始化
        /// </summary>
        /// <param name="selectType"></param>
        /// <param name="dataList"></param>
        public void IniDataModel(int selectType, List<MjLiangyiData> dataList, bool isLogic)
        {
            curISReconned = !isLogic;
            this.selectSubType = (EnumMjSelectSubType)selectType;
            if (dataList != null && dataList.Count > 0)
            {
                for (int i = 0; i < dataList.Count; i++)
                {
                    IniOrUpdateLiangyi(dataList[i].seatID, dataList[i].chooseCard, dataList[i].haveChoose);
                }
            }
        }


        /// <summary>
        /// 初始化或者更新数据
        /// </summary>
        /// <param name="seatID"></param>
        /// <param name="chooseCard"></param>
        /// <param name="haveChoose"></param>
        public void IniOrUpdateLiangyi(int seatID, int chooseCard, bool haveChoose)
        {
            MjLiangyiData dataItem = GetADataBySeat(seatID);
            dataItem.chooseCard = chooseCard;
            dataItem.haveChoose = haveChoose;
        }


        public void SetLiangYiIndex(int seatID, int chooseIndex)
        {
            MjLiangyiData dataItem = GetADataBySeat(seatID);
            dataItem.chooseIndex = chooseIndex;
        }



        /// <summary>
        /// 查看是否做出选择
        /// </summary>
        /// <param name="seatID"></param>
        /// <returns></returns>
        public bool CheckChooseState(int seatID)
        {
            MjLiangyiData dataItem = GetADataBySeat(seatID);
            return dataItem.haveChoose;
        }

        /// <summary>
        /// 检查发送逻辑
        /// </summary>
        /// <param name="seatID"></param>
        /// <returns></returns>
        public bool CheckSendData(int seatID)
        {
            MjLiangyiData dataItem = GetADataBySeat(seatID);
            bool canSend = false;
            if (dataItem.chooseCard > 0 && dataItem.chooseIndex >= 0)
            {
                canSend = true;
            }
            else
            {
                QLoger.ERROR("检查亮一发送数据有问题 Index : {0} + CardID : {1}",
                    dataItem.chooseIndex, dataItem.chooseCard);
            }
            return canSend;
        }


        public int GetChooseCard(int seatID)
        {
            if (_dataDic.ContainsKey(seatID))
            {
                return _dataDic[seatID].chooseCard;
            }
            return -1;
        }


        public MjLiangyiData GetADataBySeat(int seatID)
        {
            if (!_dataDic.ContainsKey(seatID))
            {
                MjLiangyiData dataItem = new MjLiangyiData(seatID);
                _dataDic.Add(seatID, dataItem);
            }

            return _dataDic[seatID];
        }

    }

}
