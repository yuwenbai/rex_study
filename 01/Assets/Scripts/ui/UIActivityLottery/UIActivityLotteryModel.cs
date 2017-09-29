/**
* @Author jl
* 转盘抽奖
*
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace projectQ
{
    public class UIActivityLotteryModel : UIModelBase
    {
        public UIActivityLottery UI
        {
            get { return _ui as UIActivityLottery; }
        }


        protected override GEnum.NamedEvent[] FocusNetWorkData()
        {
            return new GEnum.NamedEvent[]
            {
                GEnum.NamedEvent.SysData_Other_AwardConfig_Rsp,
                GEnum.NamedEvent.SysData_Other_Lottery_Rsp
            };
        }

        protected override void OnNetWorkDataCallBack(GEnum.NamedEvent msgEnum, object[] data)
        {
            switch (msgEnum)
            {
                case GEnum.NamedEvent.SysData_Other_AwardConfig_Rsp: //奖励配置信息
                    List<AwardConfigData> list = MemoryData.SysActivityData.GetAwardList();
                    list.Sort(
                            (left, right) =>
                            {
                                if (left.SortID > right.SortID)
                                {
                                    return 1;
                                }
                                else if (left.SortID == right.SortID)
                                {
                                    return 0;
                                }
                                else
                                {
                                    return -1;
                                }
                            }
                        );
                    UI.SetData(list);
                    break;
                case GEnum.NamedEvent.SysData_Other_Lottery_Rsp: //接收抽奖结果
                    MemoryData.PlayerData.MyPlayerModel.PlayerDataBase.lotteryTimes++;

                    int awardId = (int)data[0];

                    int index = 0;

                    List<AwardConfigData> awardList = MemoryData.SysActivityData.GetAwardList();
                    int count = awardList.Count;
                    for (int i = 0; i < count; i++)
                    {
                        if(awardList[i].AwardID == awardId)
                        {
                            index = i;
                            break;
                        }
                    }
                    UI.Lottery_RotationBegin(index);
                    break;
                default:
                    break;
            }
        }
    }
}