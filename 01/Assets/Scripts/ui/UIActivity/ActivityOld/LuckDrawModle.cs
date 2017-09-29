///**
// * 作者：周腾
// * 作用：
// * 日期：
// */

//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using projectQ;
//using Msg;
//public class LuckDrawModle : UIModelBase {
//	//public ActivityLuckDraw UI { get { return _ui as ActivityLuckDraw; } }
//    [HideInInspector]
//    public int awardID;
//    //[HideInInspector]
//    //public bool isBeginTick;
//    [HideInInspector]
//    public float duringTime = 0f;
//    [HideInInspector]
//    public Vector3 v = new Vector3(0, 0, 0);
//    /// <summary>
//    /// 获取抽奖奖品配置列表
//    /// </summary>
//    /// <returns></returns>
//    public List<AwardConfigData> GetAwardConfigList()
//    {
//        List<projectQ.AwardConfigData> data = MemoryData.Get<SysOtherData>(MKey.SYS_OTHER_DATA).GetAwardList();
//        return data;
//    }
  
//    /// <summary>
//    /// 请求抽奖配置信息
//    /// </summary>
//    /// <param name="type"></param>
//    public void GetAwardConfigReq(LotteryTypeDef type)
//    {
//        object[] data = new object[1];
//        data[0] = type;
//        ModleNetWorker.Instance.GetAwardConfigReq(data);
//    }
//    /// <summary>
//    /// 抽奖请求
//    /// </summary>
//    /// <param name="type"></param>
//    public void GetLotteryReq(LotteryTypeDef type)
//    {
//        object[] data = new object[1];
//        data[0] = type;
//        ModleNetWorker.Instance.LotteryReq(data);
//    }

//    #region Override
//    protected override GEnum.NamedEvent[] FocusNetWorkData()
//    {
//        return new GEnum.NamedEvent[] {
//            GEnum.NamedEvent.SysData_Other_AwardConfig_Rsp,
//            GEnum.NamedEvent.SysData_Other_Lottery_Rsp
//        };
//    }
//    protected override void OnNetWorkDataCallBack(GEnum.NamedEvent msgEnum, object[] data)
//    {
//        switch (msgEnum)
//        {
//            case GEnum.NamedEvent.SysData_Other_AwardConfig_Rsp:
//               // UI.InitLuckDraw();
//                break;
//            case GEnum.NamedEvent.SysData_Other_Lottery_Rsp:
//                UI.isBeginTick = false;
//                awardID = (int)data[0];
//                for (int i = 0; i < UI.awardItemList.Count; i++)
//                {
//                    if (awardID == UI.awardItemList[i].GetComponent<LuckAwardItem>().awardId)
//                    {
                      
//                        UI.GetNeedRotate(i);
//                    }
//                }

//                break;
//        }
//    }


//    #endregion

//    private void Update()
//    {
//        if (UI.isBeginTick)
//        {
//            duringTime += Time.deltaTime;
//        }
//    }
//}
