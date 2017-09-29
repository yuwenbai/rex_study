/**
 * @Author lyb
 * 活动Data
 *
 */

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace projectQ
{
    //活动消息
    public class ActivityData
    {
        public int ActivityID;          // 活动ID
        public string ResUrl;           // 资源URL
        public string ActivityDesc;     // 活动描述
        public string ActivityName;     // 活动名称
        public string JumpTarget;       // 活动跳转目标

        public static ActivityData ProtoToData(Msg.ActivityNotify msg)
        {
            ActivityData result = new ActivityData();
            result.ActivityID = msg.ActivityID;
            result.ResUrl = msg.ResUrl;
            result.ActivityDesc = msg.ActivityDesc;
            result.ActivityName = msg.ActivityName;
            result.JumpTarget = msg.JumpTarget;
            return result;
        }
    }

    public class AwardConfigData
    {
        public int AwardID;
        public string AwardName;
        public int AwardCount;
        public string ResUrl;
        public int AwardType;
        public int SortID;

        public static AwardConfigData ProtoToData(Msg.AwardConfig msg)
        {
            AwardConfigData result = new AwardConfigData();
            result.AwardID = msg.AwardID;
            result.AwardName = msg.AwardName;
            result.AwardCount = msg.AwardCount;
            result.ResUrl = msg.ResUrl;
            result.AwardType = msg.AwardType;
            result.SortID = msg.SortID;
            return result;
        }
    }

    public class SysActivityData
    {
        //活动消息列表
        private List<ActivityData> ActivityList;
        public List<ActivityData> GetActivityList()
        {
            return ActivityList;
        }

        private List<AwardConfigData> AwardConfigList;
        public List<AwardConfigData> GetAwardList()
        {
            return AwardConfigList;
        }


        public void AddActivity(ActivityData data)
        {
            if (ActivityList == null)
            {
                ActivityList = new List<ActivityData>();
            }

            var index = ActivityList.FindIndex((findData) =>
            {
                return findData.ActivityID == data.ActivityID;
            });

            if (index == -1)
            {
                ActivityList.Add(data);
            }
            else
            {
                ActivityList[index] = data;
            }
        }


        public void AddAwardConfig(AwardConfigData data)
        {
            if (AwardConfigList == null)
            {
                AwardConfigList = new List<AwardConfigData>();
            }

            var index = AwardConfigList.FindIndex((findData) =>
            {
                return findData.AwardID == data.AwardID;
            });

            if (index == -1)
            {
                AwardConfigList.Add(data);
            }
            else
            {
                AwardConfigList[index] = data;
            }
        }
    }

    #region 内存数据

    public partial class MKey
    {
        public const string SYS_ACTIVITY_DATA = "SYS_ACTIVITY_DATA";
    }

    public partial class MemoryData
    {
        static public SysActivityData SysActivityData
        {
            get
            {
                SysActivityData activityData = MemoryData.Get<SysActivityData>(MKey.SYS_ACTIVITY_DATA);
                if (activityData == null)
                {
                    activityData = new SysActivityData();
                    MemoryData.Set(MKey.SYS_ACTIVITY_DATA, activityData);
                }
                return activityData;
            }
        }
    }

    #endregion
}