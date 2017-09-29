/**
 * @Author Xin.Wang
 * 枣庄玩法 胡漂跟漂
 *
 */

using System.Collections;
using System.Collections.Generic;
using MahjongPlayType;
using projectQ;


namespace projectQ
{
    public partial class MahongDataModel
    {
        public partial class GameProcessData
        {
            public MahjongPlayOprateHuPiaoGenPiao processHuPiaoGenPiao
            {
                get
                {
                    string key = MjPlayOprateType.OPRATE_HUPIAOGENPIAO.ToString();
                    if (!playOprateDic.ContainsKey(key))
                    {
                        MahjongPlayOprateHuPiaoGenPiao item = new MahjongPlayOprateHuPiaoGenPiao();
                        playOprateDic.Add(key, item);
                    }

                    return playOprateDic[key] as MahjongPlayOprateHuPiaoGenPiao;
                }
            }
        }
    }


    public partial class MjPlayOprateType
    {
        public const string OPRATE_HUPIAOGENPIAO = "OPRATE_HUPIAOGENPIAO";
    }



    public partial class MjDataManager
    {
        /// <summary>
        /// 更新数据
        /// </summary>
        /// <param name="seatID"></param>
        /// <param name="curValue"></param>
        public void SetHuPiaoGenPiaoData(int seatID, int curValue)
        {
            MjData.ProcessData.processHuPiaoGenPiao.SetPlayerDataUpdate(seatID, curValue);
        }

        /// <summary>
        /// 设置当前展示
        /// </summary>
        /// <param name="seatID"></param>
        public void SetHuPiaoGenPiaoShow(int seatID)
        {
            EventDispatcher.FireEvent(MJEnum.MjHuPiaoGenPiaoEvent.HPGP_DataToLogicShowEffect.ToString(), seatID);
        }


        /// <summary>
        /// 检查是否存在胡飘杠飘玩法
        /// </summary>
        /// <returns></returns>
        public bool CheckHuPiaoGenPiaoData()
        {
            return MjData.ProcessData.playOprateDic.ContainsKey(MjPlayOprateType.OPRATE_HUPIAOGENPIAO.ToString());
        }

        /// <summary>
        /// 获取某个位置的值
        /// </summary>
        /// <param name="seatID"></param>
        /// <returns></returns>
        public int GetHuPiaoGenPiaoValue(int seatID)
        {
            MJHuPiaoGenPiaoData data = MjData.ProcessData.processHuPiaoGenPiao.GetDataBySeatID(seatID);
            return data.curNum;
        }

    }

}


namespace MahjongPlayType
{
    public class MJHuPiaoGenPiaoData
    {
        public int seatID;          //座位号
        public int curNum;          //当前漂几

        public MJHuPiaoGenPiaoData(int seat)
        {
            this.seatID = seat;
        }
    }


    public class MahjongPlayOprateHuPiaoGenPiao : MahjongPlayOprateBase
    {
        private Dictionary<int, MJHuPiaoGenPiaoData> _playDataDic = new Dictionary<int, MJHuPiaoGenPiaoData>();

        public int curShowType = -1;                ///缺省值 以后扩展 

        public void SetPlayerDataUpdate(int seatID, int curValue)
        {
            MJHuPiaoGenPiaoData item = GetDataBySeatID(seatID);
            item.curNum = curValue;
        }

        public MJHuPiaoGenPiaoData GetDataBySeatID(int seatID)
        {
            if (!_playDataDic.ContainsKey(seatID))
            {
                MJHuPiaoGenPiaoData item = new MJHuPiaoGenPiaoData(seatID);
                _playDataDic.Add(seatID, item);
            }

            return _playDataDic[seatID];
        }

    }

}
