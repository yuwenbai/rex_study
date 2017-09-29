/**
 * @Author YQC
 *
 *
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace projectQ
{
    public class SysPlayerDataHandleData
    {
        public Dictionary<SysPlayerDataHandle.PlayerDataType, float> LastReqTime;

        public float UserInfoTime
        {
            set { LastReqTime[SysPlayerDataHandle.PlayerDataType.UserInfo] = value; }
            get { return LastReqTime[SysPlayerDataHandle.PlayerDataType.UserInfo]; }
        }

        public float MjRoomTime
        {
            set { LastReqTime[SysPlayerDataHandle.PlayerDataType.MjRoomData] = value; }
            get { return LastReqTime[SysPlayerDataHandle.PlayerDataType.MjRoomData]; }
        }

        public SysPlayerDataHandleData()
        {
            LastReqTime = new Dictionary<SysPlayerDataHandle.PlayerDataType, float>();
            LastReqTime.Add(SysPlayerDataHandle.PlayerDataType.UserInfo, 0);
            LastReqTime.Add(SysPlayerDataHandle.PlayerDataType.MjRoomData, 0);
        }

        public void SetTime(SysPlayerDataHandle.PlayerDataType type,float time)
        {
            switch(type)
            {
                case SysPlayerDataHandle.PlayerDataType.All:
                    {
                        var buffer = new List<SysPlayerDataHandle.PlayerDataType>(LastReqTime.Keys);
                        foreach (var item in buffer)
                        {
                            this.LastReqTime[item] = time;
                        }
                    }
                    break;
                default:
                    this.LastReqTime[type] = time;
                    break;
            }

        }

        public float GetTime(SysPlayerDataHandle.PlayerDataType type)
        {
            float result = -99;
            switch(type)
            {
                case SysPlayerDataHandle.PlayerDataType.All:
                    {
                        foreach(var item in this.LastReqTime)
                        {
                            if(result < 0 || item.Value < result)
                            {
                                result = item.Value;
                            }
                        }
                    }
                    break;
                default:
                    result = this.LastReqTime[type];
                    break;
            }

            return result;
        }
    }
    public class SysPlayerDataHandle {
        //用户数据类型
        public enum PlayerDataType
        {
            All         = 0,    //全部
            UserInfo    = 1,    //用户基础数据
            MjRoomData  = 2,    //用户麻将数据
        }
        private Dictionary<long, SysPlayerDataHandleData> PlayerMap = new Dictionary<long, SysPlayerDataHandleData>();
        public float IntervalTime = 0f;

        /// <summary>
        /// 请求用户信息
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="type"></param>
        /// <param name="force"></param>
        public void PlayerReq(long userId, PlayerDataType type, bool force = false)
        {
            if(!PlayerMap.ContainsKey(userId))
            {
                PlayerMap.Add(userId, new SysPlayerDataHandleData());
            }

            var data = PlayerMap[userId];
            if (force || IsReq(data, type))
            {
                PlayerDataReq(userId, type);
            }
            else
            {
                EventDispatcher.FireSysEvent(GEnum.NamedEvent.SysData_PlayerData_Update, userId, type);
            }
        }


        /// <summary>
        /// 接收用户信息
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="type"></param>
        public void PlayerRsp(long userId, PlayerDataType type)
        {
            if (!PlayerMap.ContainsKey(userId))
            {
                PlayerMap.Add(userId, new SysPlayerDataHandleData());
            }

            PlayerMap[userId].SetTime(type, Time.realtimeSinceStartup);

            EventDispatcher.FireSysEvent(GEnum.NamedEvent.SysData_PlayerData_Update, userId, type);
        }

        /// <summary>
        /// 是否需要去请求
        /// </summary>
        /// <param name="data"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        private bool IsReq(SysPlayerDataHandleData data, PlayerDataType type)
        {
            float tempTime = data.GetTime(type);
            return tempTime == 0 || Time.realtimeSinceStartup - tempTime > IntervalTime;
        }

        /// <summary>
        /// 请求用户数据
        /// </summary>
        private void PlayerDataReq(long userId, PlayerDataType type)
        {
            if (userId == MemoryData.UserID)
            {
                switch (type)
                {
                    case PlayerDataType.UserInfo:
                        //请求用户基本信息 和 麻将馆信息
                        EventDispatcher.FireSysEvent(GEnum.NamedEvent.SysData_PlayerData_Update, userId, type);
                        break;
                    case PlayerDataType.All:
                    case PlayerDataType.MjRoomData:
                        //请求用户的麻将馆信息
                        EventDispatcher.FireSysEvent(GEnum.NamedEvent.EGetGameData, GEnum.NamedGameData.EC2SMessageRoomPlayerInfo, userId);
                        //ModleNetWorker.Instance.FMjRoomPlayerInfoReq(userId);
                        break;
                }
            }
            else
            {
                switch (type)
                {
                    case PlayerDataType.All:
                    case PlayerDataType.UserInfo:
                        //请求用户麻将信息
                        ModelNetWorker.Instance.OtherPlayerInfoReq(userId);
                        break;
                    case PlayerDataType.MjRoomData:
                        //请求用户的麻将馆信息
                        EventDispatcher.FireSysEvent(GEnum.NamedEvent.EGetGameData, GEnum.NamedGameData.EC2SMessageRoomPlayerInfo, userId);
                        //ModleNetWorker.Instance.FMjRoomPlayerInfoReq(userId);
                        break;
                }
            }


        }

    }
    #region 内存数据------------------------------------------------------------------------------

    public partial class MKey
    {
        public const string USER_DATA_HANDLE = "USER_DATA_HANDLE";
    }

    public partial class MemoryData
    {
        static public SysPlayerDataHandle SysPlayerDataHandle
        {
            get
            {
                SysPlayerDataHandle playerDataHandle = MemoryData.Get<SysPlayerDataHandle>(MKey.USER_DATA_HANDLE);
                if (playerDataHandle == null)
                {
                    playerDataHandle = new SysPlayerDataHandle();
                    MemoryData.Set(MKey.USER_DATA_HANDLE, playerDataHandle);
                }
                return playerDataHandle;
            }
        }
    }

    #endregion -----------------------------------------------------------------------------------

}