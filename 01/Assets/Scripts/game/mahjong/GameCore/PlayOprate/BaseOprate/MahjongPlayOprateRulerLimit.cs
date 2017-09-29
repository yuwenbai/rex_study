/**
 * @Author Xin.Wang
 *  基本流程阻塞
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
            public MahjongPlayOprateRulerLimit baseRulerLimit
            {
                get
                {
                    string key = MjPlayOprateType.OPRATE_BASE_RULERLIMIT.ToString();
                    if (!playOprateDic.ContainsKey(key))
                    {
                        MahjongPlayOprateRulerLimit item = new MahjongPlayOprateRulerLimit();
                        playOprateDic.Add(key, item);
                    }

                    return playOprateDic[key] as MahjongPlayOprateRulerLimit;
                }
            }
        }
    }


    public partial class MjPlayOprateType
    {
        public const string OPRATE_BASE_RULERLIMIT = "OPRATE_BASE_RULERLIMIT";
    }



    public partial class MjDataManager
    {
        public void Base_RulerLimit(List<int> rulerList)
        {
            MjData.ProcessData.baseRulerLimit.SetCurRulerList(rulerList);
        }


        public bool Base_GetRulerLimitClick(bool canFlower, bool canFangmao)
        {
            bool canclick = !canFlower && !canFangmao;
            bool dataClick = MjData.ProcessData.baseRulerLimit.limitData.clickLimit;
            return canclick && dataClick;
        }

    }

}


namespace MahjongPlayType
{
    public enum EnumBaseLimit
    {
        MjRulerType_Gameing_NIUPAI = 20051,          //扭牌
    }


    public class BaseLimitData
    {
        public bool clickLimit = true;             //出牌限制
    }


    public class MahjongPlayOprateRulerLimit : MahjongPlayOprateBase
    {
        public List<int> curRulerList = null;
        public BaseLimitData limitData = new BaseLimitData();

        public void SetCurRulerList(List<int> rulerList)
        {
            this.curRulerList = rulerList;
            if (curRulerList != null || curRulerList.Count != 0)
            {
                SetBaseRulerLimit();
            }
            else
            {
                limitData = new BaseLimitData();
            }
        }


        private void SetBaseRulerLimit()
        {
            if (curRulerList.Contains((int)EnumBaseLimit.MjRulerType_Gameing_NIUPAI))
            {
                //扭牌
                limitData.clickLimit = false;
            }

        }


    }
}
