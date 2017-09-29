/**
 * @Author Xin.Wang
 *
 *
 */

using System.Collections;
using System.Collections.Generic;
using MahjongPlayType;


namespace projectQ
{
    public partial class MahongDataModel
    {
        public partial class GameProcessData
        {
            public MahjongPlayOprateCostTip processCostTip
            {
                get
                {
                    string key = MjPlayOprateType.OPRATE_COSTTIP.ToString();
                    if (!playOprateDic.ContainsKey(key))
                    {
                        MahjongPlayOprateCostTip item = new MahjongPlayOprateCostTip();
                        playOprateDic.Add(key, item);
                    }

                    return playOprateDic[key] as MahjongPlayOprateCostTip;
                }
            }
        }
    }

    public partial class MjPlayOprateType
    {
        public const string OPRATE_COSTTIP = "OPRATE_COSTTIP";
    }


    public partial class MjDataManager
    {
        public bool CheckCostDataContain()
        {
            return CheckPlayDataOprateContain(MjPlayOprateType.OPRATE_COSTTIP);
        }

        public void SetCostData(int costNum, int restNum, bool isLast)
        {
            MjData.ProcessData.processCostTip.SetCostData(costNum, restNum, isLast);
        }

        /// <summary>
        /// 注意判断空
        /// </summary>
        /// <returns></returns>
        public CostTipData GetCostData()
        {
            if (CheckCostDataContain())
            {
                return MjData.ProcessData.processCostTip.tipData;
            }
            return null;
        }

    }

}

namespace MahjongPlayType
{
    public class CostTipData
    {
        public int CostNum = 0;             //消耗数量
        public int RestNum = 0;             //剩余数量
        public bool isLastUser = false;     //是否是最后进入的  true  最后进入 false 大赢家付卡s
    }


    public class MahjongPlayOprateCostTip : MahjongPlayOprateBase
    {
        public CostTipData tipData = null;

        public void SetCostData(int costNum, int restNum, bool isLast)
        {
            CostTipData tip = new CostTipData();
            tip.RestNum = restNum;
            tip.CostNum = costNum;
            tip.isLastUser = isLast;
            this.tipData = tip;
        }

    }
}

