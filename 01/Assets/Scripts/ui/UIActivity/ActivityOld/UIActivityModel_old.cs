/**
 * 作者：周腾
 * 作用：
 * 日期：
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using projectQ;
using Msg;
public class UIActivityModel_old : UIModelBase
{
    
    public UIActivity_old UI { get { return _ui as UIActivity_old; } }
    [HideInInspector]
    public int awardID;
    //[HideInInspector]
    //public bool isBeginTick;
    [HideInInspector]
    public float duringTime = 0f;
    [HideInInspector]
    public Vector3 v = new Vector3(0, 0, 0);
    public List<ActivityData> GetActivityList()
    {
        List<ActivityData> data = MemoryData.Get<SysActivityData>(MKey.SYS_OTHER_DATA).GetActivityList();
        return data;
    }

    /// <summary>
    /// 获取抽奖奖品配置列表
    /// </summary>
    /// <returns></returns>
    public List<AwardConfigData> GetAwardConfigList()
    {
        List<AwardConfigData> data = MemoryData.Get<SysActivityData>(MKey.SYS_OTHER_DATA).GetAwardList();
        return data;
    }

    /// <summary>
    /// 请求抽奖配置信息
    /// </summary>
    /// <param name="type"></param>
    public void GetAwardConfigReq(LotteryTypeDef type)
    {
        EventDispatcher.FireSysEvent(GEnum.NamedEvent.EGetGameData, GEnum.NamedGameData.EC2SMessageAwardConfig, type);
    }
    public void PickUpAwardReq(int awardId)
    {
        ModelNetWorker.Instance.C2SPickupAwardReq(awardId);
    }
    /// <summary>
    /// 抽奖请求
    /// </summary>
    /// <param name="type"></param>
    public void GetLotteryReq(LotteryTypeDef type)
    {

        ModelNetWorker.Instance.C2SLotteryReq(type);
    }


    protected override GEnum.NamedEvent[] FocusNetWorkData()
    {
        return new GEnum.NamedEvent[] {
            GEnum.NamedEvent.SysData_Other_AwardConfig_Rsp,
            GEnum.NamedEvent.SysData_Other_Lottery_Rsp,
            GEnum.NamedEvent.SysData_Other_PickupAward_Rsp,
        };
    }
    protected override void OnNetWorkDataCallBack(GEnum.NamedEvent msgEnum, object[] data)
    {
        switch (msgEnum)
        {
            case GEnum.NamedEvent.SysData_Other_AwardConfig_Rsp:
                //UIluckDraw.InitLuckDraw();
                //UI.luckDrawActivity.InitLuckDraw();
                UI.luckDrawActivity.RefreshUI();
                break;
            case GEnum.NamedEvent.SysData_Other_Lottery_Rsp:
                //UI.activityRight.luckDraw.isBeginTick = false;
                awardID = (int)data[0];
                for (int i = 0; i < UI.luckDrawActivity.awardItemList.Count; i++)
                {
                    if (UI.luckDrawActivity.awardItemList[i].GetComponent<LuckAwardItem>().awardId != 0)
                    {
                        if (awardID == UI.luckDrawActivity.awardItemList[i].GetComponent<LuckAwardItem>().awardId)
                        {
                            UI.luckDrawActivity.Offset(i);
                            UI.luckDrawActivity.CanBeginRun();
                        }
                          
                    }
                }
                //for (int i = 0; i < UI.activityRight.luckDraw.awardItemList.Count; i++)
                //{
                   
                //    if (UI.activityRight.luckDraw.awardItemList[i].GetComponent<LuckAwardItem>().awardId != 0)
                //    {
                //        if (awardID == UI.activityRight.luckDraw.awardItemList[i].GetComponent<LuckAwardItem>().awardId)
                //        {
                //            QLoger.LOG("---------------------------" + UI.activityRight.luckDraw.awardItemList[i].GetComponent<LuckAwardItem>().awardId);
                //            //用i找角度坐标
                //            //然后再转到相应角度
                //            //float offset = UI.activityRight.luckDraw.positions[i];
                //            //UI.activityRight.luckDraw.Offset(offset);
                //            UI.activityRight.luckDraw.Offset(i);
                //            UI.activityRight.luckDraw.CanBeginRun();
                //        }
                //    }

               
                //}


                break;

            case GEnum.NamedEvent.SysData_Other_PickupAward_Rsp:
                int resultCode = (int)data[0];
                if (resultCode == 0)
                {
                    UI.LoadTip("领取成功");
                }
                else
                {
                    UI.LoadTip("领取失败");
                }
                break;
        }
    }
    private void Update()
    {
        //if (UI.activityRight.luckDraw.isBeginTick)
        //{
        //    duringTime += Time.deltaTime;
        //}
    }
}
