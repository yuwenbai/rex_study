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
    public class PingManager : SingletonTamplate<PingManager>
    {
        public enum EnumPingState
        {
            PingClose,
            PingSend,
            PingWrite,
            PingError,
        }
        public EnumPingState CurrState = EnumPingState.PingClose;
        
        /// <summary>
        /// 状态机检测时间间隔
        /// </summary>
        private float PingStateCheckTime = 1f;

        /// <summary>
        /// Ping时间间隔
        /// </summary>
        private float PingIntervalTime = 5.0f;

        /// <summary>
        /// 上次Ping时间
        /// </summary>
        private float lastPingTime;
        public float LastPingTime
        {
            set { lastPingTime = value; }
            get { return lastPingTime; }
        }

        /// <summary>
        /// 网路延迟
        /// </summary>
        private float netDelay;
        public float NetDelay
        {
            set { netDelay = value; }
            get { return netDelay; }
        }

        /// <summary>
        /// Ping最大重试次数
        /// </summary>
        public int PingReverCountMax = 3;
        public int CurrPingCount = 0;




        #region API
        public void Init()
        {
            GameDelegateCache.Instance.InvokeMethodEvent += UpdataPing;
        }

        public void PingRsp()
        {
            //UserActionManager.AddLocalTypeLog("NetR", "Ping [00FFFF]回复");
            this.CurrPingCount = 0;
            UpdateNetDelay();
        }
        #endregion

        #region 更新机制
        private float LastStateCheackTime = 0f;
        private void UpdataPing()
        {
            if(Time.realtimeSinceStartup - LastStateCheackTime > PingStateCheckTime)
            {
                LastStateCheackTime = Time.realtimeSinceStartup;
                switch (CurrState)
                {
                    case EnumPingState.PingSend:
                        this.StatePingSend();
                        break;
                    case EnumPingState.PingWrite:
                        this.StatePingWrite();
                        break;
                    case EnumPingState.PingClose:
                        this.StatePingClose();
                        break;
                    case EnumPingState.PingError:
                        this.StatePingError();
                        break;
                }
            }

        }

        private void StatePingSend()
        {
            //UserActionManager.AddLocalTypeLog("NetR", "Ping [00FFFF]发送");
            this.CurrPingCount++;
            //发送Ping
            ModelNetWorker.Instance.PingCmd();

            //更新上次Ping的时间
            LastPingTime = Time.realtimeSinceStartup;
            this.CurrState = EnumPingState.PingWrite;
        }

        private void StatePingWrite()
        {
            if(MemoryData.GameStateData.IsPause)
            {
                UserActionManager.AddLocalTypeLog("NetR", "Ping [00FFFF]游戏在后台");

                this.CurrState = EnumPingState.PingClose;
            }
            //如果 ping的次数超过 则改为错误状态
            else if(this.CurrPingCount > this.PingReverCountMax)
            {
                UserActionManager.AddLocalTypeLog("NetR", "Ping [00FFFF]超过次数");

                CurrState = EnumPingState.PingError;
            }
            //如果 ping时间间隔到
            else if(Time.realtimeSinceStartup - LastPingTime > PingIntervalTime)
            {
                //UserActionManager.AddLocalTypeLog("NetR", "Ping [00FFFF]发送时间到");

                this.CurrState = EnumPingState.PingSend;
            }
            else if(MemoryData.GameStateData.CurrGameNetWorkStatus != SysGameStateData.EnumGameNetWorkStatus.LinkOk)
            {
                UserActionManager.AddLocalTypeLog("NetR", "Ping [00FFFF]网络断开中");

                this.CurrState = EnumPingState.PingClose;
            }
        }

        private void StatePingClose()
        {
            if (MemoryData.GameStateData.CurrGameNetWorkStatus == SysGameStateData.EnumGameNetWorkStatus.LinkOk && !MemoryData.GameStateData.IsPause)
            {
                UserActionManager.AddLocalTypeLog("NetR", "Ping [00FFFF]网络好了 并且是 前台");

                LastPingTime = 0;
                CurrPingCount = 0;
                this.CurrState = EnumPingState.PingWrite;
            }
        }

        private void StatePingError()
        {
            UserActionManager.AddLocalTypeLog("NetR", "Ping [00FFFF]错误 发起断链");

            EventDispatcher.FireSysEvent(GEnum.NamedEvent.ENetError_Client);
            this.CurrState = EnumPingState.PingClose;
        }

        #endregion

        #region 辅助方法
        /// <summary>
        /// 更新网络延迟
        /// </summary>
        private void UpdateNetDelay()
        {
            NetDelay = Time.realtimeSinceStartup - LastPingTime;
        }
        #endregion
    }
}