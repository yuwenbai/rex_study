/**
 * @Author Xin.Wang
 *麻将特殊玩法： 胡飘 杠飘 飘素自摸  飘金钓鱼
 *
 */

using System.Collections.Generic;
using MahjongPlayType;
using projectQ;

namespace projectQ
{
    public partial class MahongDataModel
    {
        public partial class GameProcessData
        {
            public MahjongPlayOperateBankerFirstPut processBankerFirstCard
            {
                get
                {
                    string key = MjPlayOprateType.OPRATE_BankerFirstCard.ToString();
                    if (!playOprateDic.ContainsKey(key))
                    {
                        MahjongPlayOperateBankerFirstPut item = new MahjongPlayOperateBankerFirstPut();
                        playOprateDic.Add(key, item);
                    }

                    return playOprateDic[key] as MahjongPlayOperateBankerFirstPut;
                }
            }
        }
    }


    public partial class MjPlayOprateType
    {
        public const string OPRATE_BankerFirstCard = "OPRATE_BankerFirstCard";
    }


    public partial class MjDataManager
    {
        public void SetDealerPutState(bool state)
        {
            MjData.ProcessData.processBankerFirstCard.IsFirstPut = state;
        }
    }

}


namespace MahjongPlayType
{
    public class MahjongPlayOperateBankerFirstPut : MahjongPlayOprateBase
    {
        private bool _IsFirstPut = false;
        public bool IsFirstPut
        {
            get { return _IsFirstPut; }
            set
            {
                if (MjDataManager.Instance.MjData.curUserData.selfSeatID != MjDataManager.Instance.MjData.ProcessData.dealerSeatID)
                {
                    return;
                }
                _IsFirstPut = value;
                if (value)
                {
                    EventDispatcher.FireEvent(MJEnum.BankerPutAlert.BPA_BankerFirstTurn.ToString());
                }
                else
                {
                    EventDispatcher.FireEvent(MJEnum.BankerPutAlert.BPA_BankerTurnOver.ToString());
                }

            }
        }
    }

}
