/**
 * @Author Xin.Wang
 * 进入包次阶段的提示
 * 日后保留扩展 进入某个阶段的提示
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
            public MahjongPlayOprateBaociTip processBaociTip
            {
                get
                {
                    string key = MjPlayOprateType.OPRATE_BAOCI.ToString();
                    if (!playOprateDic.ContainsKey(key))
                    {
                        MahjongPlayOprateBaociTip item = new MahjongPlayOprateBaociTip();
                        playOprateDic.Add(key, item);
                    }

                    return playOprateDic[key] as MahjongPlayOprateBaociTip;
                }
            }
        }
    }


    public partial class MjPlayOprateType
    {
        public const string OPRATE_BAOCI = "OPRATE_BAOCI";
    }



    public partial class MjDataManager
    {
        /// <summary>
        /// 初始化包次提示
        /// </summary>
        /// <param name="subType"></param>
        /// <param name="showType"></param>
        /// <param name="numLimit"></param>
        /// <param name="needShowInUI"></param>
        public void IniBaociTip(int subType, int showType, int numLimit, bool needShowInUI)
        {
            if (subType < 0 || subType > 3)
            {
                return;
            }

            MjData.ProcessData.processBaociTip.IniDataTip((EnumMjSelectSubType)subType, showType, numLimit, needShowInUI);
        }

        /// <summary>
        /// 检查包次提示是否需要显示
        /// </summary>
        public void CheckAndShowBaociTip()
        {
            MjData.ProcessData.processBaociTip.CheckAndShowBaociTip();
        }
    }

}


namespace MahjongPlayType
{
    public class MahjongPlayOprateBaociTip : MahjongPlayOprateBase
    {
        public EnumMjSelectSubType curSubType = EnumMjSelectSubType.MjSelectSubType_NoOperation;           //当前状态
        public EnumShowType curShowType = 0;                                                                         //预留扩展 没有值  日后扩展
        public int showNumLimit = 0;                                                                        //当前显示的阀值

        public enum EnumShowType
        {
            Baoci = 22054,
            Haidilaoyue = 22085,
        }

        public void IniDataTip(EnumMjSelectSubType subType, int showType, int numLimit, bool needShowInUI)
        {
            curSubType = subType;
            curShowType = (EnumShowType)showType;
            showNumLimit = numLimit;

            if (needShowInUI)
            {
                CheckAndShowBaociTip();
            }
        }


        private string CheckShowTypeStr()
        {
            string showStr = null;
            switch (curShowType)
            {
                case EnumShowType.Baoci:
                    {
                        showStr = "包次阶段";
                    }
                    break;
                case EnumShowType.Haidilaoyue:
                    {
                        showStr = "海底捞月阶段";
                    }
                    break;
            }
            return showStr;
        }


        /// <summary>
        /// 展示阶段
        /// </summary>
        public void CheckAndShowBaociTip()
        {
            bool needShow = false;
            switch (curSubType)
            {
                case EnumMjSelectSubType.MjSelectSubType_NoOperation:
                    {

                    }
                    break;
                case EnumMjSelectSubType.MjSelectSubType_WAIT_NoSelect:
                    {
                        QLoger.ERROR("麻将保持阶段1");
                    }
                    break;
                case EnumMjSelectSubType.MjSelectSubType_WAIT_Select:
                    {
                        QLoger.ERROR("麻将保持阶段0");
                    }
                    break;
                case EnumMjSelectSubType.MjSelectSubType_RESULT_Select:
                    {
                        needShow = true;
                    }
                    break;
            }

            string showStr = CheckShowTypeStr();
            EventDispatcher.FireEvent(GEnum.EnumMjOprateBaoci.BC_LogicShow.ToString(), needShow, showStr);
        }

    }

}

