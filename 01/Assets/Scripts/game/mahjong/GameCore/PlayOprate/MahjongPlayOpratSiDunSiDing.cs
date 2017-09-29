/**
 * @Author Xin.Wang
 * 死蹲死顶的玩法类型
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
            public MahjongPlayOpratSiDunSiDing processDunDing
            {
                get
                {
                    string key = MjPlayOprateType.OPRATE_DUNDING.ToString();
                    if (!playOprateDic.ContainsKey(key))
                    {
                        MahjongPlayOpratSiDunSiDing item = new MahjongPlayOpratSiDunSiDing();
                        playOprateDic.Add(key, item);
                    }

                    return playOprateDic[key] as MahjongPlayOpratSiDunSiDing;
                }

            }
        }
    }


    public partial class MjPlayOprateType
    {
        public const string OPRATE_DUNDING = "OPRATE_DUNDING";
    }

    public partial class MjDataManager
    {
        public void UpdateDunDingData(int seatID, int dealerSeat, int state)
        {
            MjData.ProcessData.processDunDing.SetSiDunSiDingData(seatID, dealerSeat, state);
        }

        public MjSiDunSiDingData GetDunDingData(int seatID)
        {
            return MjData.ProcessData.processDunDing.GetPlayerData(seatID);
        }

        public bool GetDunDingLoseState(int sealfSeatID)
        {
            MjSiDunSiDingData data = GetDunDingData(sealfSeatID);

            if (data != null)
            {
                return data.chooseLose;
            }
            return false;
        }

    }

}




namespace MahjongPlayType
{
    public class MjSiDunSiDingData
    {
        public MjSiDunSiDingData(int seatID, int dealerID)
        {
            playerSeatID = seatID;
            chooseType = seatID == dealerID ? EnumChooseType.Dun : EnumChooseType.Ding;
        }

        public enum EnumChooseType
        {
            Null = 0,
            Dun = 1,                            //选择蹲
            Ding = 2,                           //选择顶
        }

        public enum EnumResultType
        {
            Null = 0,                           //还未选择
            ChooseOk = 1,                       //积极选项
            ChooseCancel = 2,                   //消极选项                   
        }


        public int playerSeatID = 0;
        public EnumChooseType chooseType = EnumChooseType.Null;
        public EnumResultType resultType = EnumResultType.Null;
        public bool haveChoose = false;                         //是否选择过
        public bool chooseLose = false;                         //选择失效


        public void SetResultState(int state)
        {
            EnumResultType resultState = (EnumResultType)state;
            if (resultState != EnumResultType.Null)
            {
                haveChoose = true;
            }
            resultType = resultState;
        }

        /// <summary>
        /// 获取玩家的状态
        /// </summary>
        /// <returns></returns>
        public EnumResultType GetPlayerResult()
        {
            return resultType;
        }

        public bool GetResultState()
        {
            bool state = resultType == EnumResultType.ChooseOk;

            return state;
        }
    }

    public class MjSiDunSiDingServerData
    {
        public int seatID;
        public bool chooseStaet = false;
    }

    public class MahjongPlayOpratSiDunSiDing : MahjongPlayOprateBase
    {
        private bool CheckDicState
        {
            get
            {
                return (playerDataDic != null && playerDataDic.Count > 0);
            }
        }

        public Dictionary<int, MjSiDunSiDingData> playerDataDic = new Dictionary<int, MjSiDunSiDingData>();


        public void SetSiDunSiDingData(int seatID, int dealerSeatID, int curState)
        {
            MjSiDunSiDingData itemData = IniData(seatID, dealerSeatID);
            itemData.SetResultState(curState);
        }


        public MjSiDunSiDingData IniData(int seatID, int dealerSeatID)
        {
            if (!CheckDicState)
            {
                playerDataDic = new Dictionary<int, MjSiDunSiDingData>();
            }

            if (!playerDataDic.ContainsKey(seatID))
            {
                MjSiDunSiDingData itemData = new MjSiDunSiDingData(seatID, dealerSeatID);
                playerDataDic.Add(seatID, itemData);
            }

            return playerDataDic[seatID];
        }


        public MjSiDunSiDingData GetPlayerData(int seatID)
        {
            if (CheckDicState && playerDataDic.ContainsKey(seatID))
            {
                return playerDataDic[seatID];
            }

            return null;
        }


        public MjSiDunSiDingData.EnumResultType GetPlayerState(int seatID)
        {
            MjSiDunSiDingData.EnumResultType state = MjSiDunSiDingData.EnumResultType.Null;
            if (CheckDicState && playerDataDic.ContainsKey(seatID))
            {
                state = playerDataDic[seatID].GetPlayerResult();
            }
            return state;
        }

    }

}
