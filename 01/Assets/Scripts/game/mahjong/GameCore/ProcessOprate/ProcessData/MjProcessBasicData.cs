/**
 * @Author Xin.Wang
 * 流程数据（基本流程
 * 
 */


/**
* 功能描述
* 1.掷筛子（内涵所有人全局压栈，可用开关控制
* 2.全局压栈逻辑（为指定人压栈时间空白
* 
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
        public partial class CoreProcessData
        {
            public MjProcessBasicData processBasic
            {
                get
                {
                    string key = MjProcessOprateType.PROCESS_BASIC.ToString();
                    if (!processDic.ContainsKey(key))
                    {
                        MjProcessBasicData item = new MjProcessBasicData();
                        processDic.Add(key, item);
                    }
                    return processDic[key] as MjProcessBasicData;
                }
            }
        }
    }

    public partial class MjProcessOprateType
    {
        public const string PROCESS_BASIC = "PROCESS_BASIC";
    }


    public partial class MjDataManager
    {
        #region 功能模块 -> 掷筛子
        public void ProcessBasic_RollNums(List<int> rollList, int seatID)
        {
            if (rollList == null || rollList.Count == 0)
            {
                return;
            }

            MjData.CoreData.processBasic.SetRollData(rollList, seatID);
            EventDispatcher.FireEvent(MJEnum.ProcessBasicEnum.PROBASIC_Roll_DatatoLogic.ToString());
        }

        public void ProcesBasic_GetRollNums(out List<int> rollList, out int seatID)
        {
            rollList = new List<int>();
            seatID = -1;
            MjData.CoreData.processBasic.GetCurRollData(out seatID, out rollList);
        }
        #endregion

        #region 功能模块 -> 玩家动画时间统一间隔

        public void ProcessBasic_AnimPlayerTime(bool isAll, float time, List<int> seatID = null)
        {
            if (isAll)
            {
                seatID = GetAllSeatIDCurDesk(false);
            }

            if (seatID != null && seatID.Count > 0 && time >= 0f)
            {
                MjData.CoreData.processBasic.SetAnimPlayerData(seatID, time);
                EventDispatcher.FireEvent(MJEnum.ProcessBasicEnum.PROBASIC_ANIM_PlayerTimeLimit.ToString());
            }
        }

        public void ProcessBasic_GetAnimPlayerTime(out List<int> seatIDList, out float limitTime)
        {
            seatIDList = new List<int>();
            limitTime = 0f;

            MjData.CoreData.processBasic.GetCurAnimPlayerData(out seatIDList, out limitTime);
        }

        #endregion

    }

}

namespace MahjongPlayType
{
    /// <summary>
    /// 基本流程数据（扔色子数据
    /// </summary>
    public class MJProBasicRollData
    {
        public List<int> rollList = new List<int>();
        public int seatID = -1;
    }

    /// <summary>
    /// 玩家动画阻塞数据
    /// </summary>
    public class MjProBasicAnimPlayerData
    {
        public List<int> seatIDList = new List<int>();
        public float limitTime = 0f;
    }


    public class MjProcessBasicData : MahjongPorcessBase
    {
        #region 扔色子模块
        private Queue<MJProBasicRollData> _queueRoll = new Queue<MJProBasicRollData>();

        public void SetRollData(List<int> numbers, int seatID)
        {
            MJProBasicRollData item = new MJProBasicRollData();
            item.rollList = numbers;
            item.seatID = seatID;
            _queueRoll.Enqueue(item);
        }


        public void GetCurRollData(out int seatID, out List<int> roolList)
        {
            seatID = -1;
            roolList = new List<int>();
            if (_queueRoll != null && _queueRoll.Count > 0)
            {
                MJProBasicRollData item = _queueRoll.Dequeue();
                seatID = item.seatID;
                roolList = item.rollList;
            }
        }


        #endregion

        #region 动画时间阻塞模块 
        private Queue<MjProBasicAnimPlayerData> _queueAnimPlayer = new Queue<MjProBasicAnimPlayerData>();

        public void SetAnimPlayerData(List<int> seatIDList, float limitTime)
        {
            MjProBasicAnimPlayerData item = new MjProBasicAnimPlayerData();
            item.seatIDList = seatIDList;
            item.limitTime = limitTime;
            _queueAnimPlayer.Enqueue(item);
        }


        public void GetCurAnimPlayerData(out List<int> seatIDList, out float limitTime)
        {
            seatIDList = new List<int>();
            limitTime = 0f;
            if (_queueAnimPlayer != null && _queueAnimPlayer.Count > 0)
            {
                MjProBasicAnimPlayerData item = _queueAnimPlayer.Dequeue();
                seatIDList = item.seatIDList;
                limitTime = item.limitTime;
            }
        }

        #endregion


    }

}
