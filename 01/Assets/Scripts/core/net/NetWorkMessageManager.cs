/**
 * @Author YQC
 *
 *
 */

using System.Collections.Generic;
using Msg;
using System;
using UnityEngine;
using System.Text;

namespace projectQ
{
    public class NetWorkMessageManagerData
    {
        public CmdNo CmdKey;
        public float RTime;
        public bool IsRemove;

        public NetWorkMessageManagerData(CmdNo CmdKey)
        {
            this.CmdKey = CmdKey;
            IsRemove = false;
            UpdateTime();
        }
        public void UpdateTime()
        {
            this.RTime = Time.realtimeSinceStartup;
        }
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("CmdKey: ").Append(this.CmdKey)
                .Append(" Time: ").Append(this.RTime)
                .Append(" IsRemove: ").Append(this.IsRemove);
            return sb.ToString();
        }
    }
    public class NetWorkMessageManager
    {
        /// <summary>
        /// 上次检测时间
        /// </summary>
        private static float LastCheckTime = 0;

        /// <summary>
        /// 检测间隔时间
        /// </summary>
        private const float WaitTime = 1f;

        /// <summary>
        /// 超时时间
        /// </summary>
        private static float MaxTimeOut = 10f;
        //必回对应列表
        private static Dictionary<CmdNo, List<CmdNo>> ms_matchedReqMap = new Dictionary<CmdNo, List<CmdNo>>();
        private static Dictionary<CmdNo, List<CmdNo>> ms_matchedRspMap = new Dictionary<CmdNo, List<CmdNo>>();
        //对应记录表
        private static Dictionary<CmdNo, NetWorkMessageManagerData> ms_matchedRecord = new Dictionary<CmdNo, NetWorkMessageManagerData>();
        private static LinkedList<NetWorkMessageManagerData> ms_matchedLinkList = new LinkedList<NetWorkMessageManagerData>();


        #region API
        /// <summary>
        /// 初始化 调用一次
        /// </summary>
        public static void Init()
        {
            RegiestCMDMatched();
            GameDelegateCache.Instance.InvokeMethodEvent += UpdateForeachMatched;
            LastCheckTime = Time.realtimeSinceStartup;
            

        }


        /// <summary>
        /// 发送消息
        /// </summary>
        /// <param name="req"></param>
        public static void MatchedReq(CmdNo req)
        {
            if (!ms_matchedReqMap.ContainsKey(req)) return;
            DebugPro.Log(DebugPro.EnumLog.NetWork, "网络消息超时管理  Req发送", req.ToString());

            lock (ms_matchedRecord)
            {
                if (!ms_matchedRecord.ContainsKey(req))
                {
                    NetWorkMessageManagerData data = new NetWorkMessageManagerData(req);
                    ms_matchedRecord.Add(req, data);
                    ms_matchedLinkList.AddLast(data);
                }
                //else
                //{
                //    ms_matchedRecord[req].UpdateTime();
                //}
            }
        }

        /// <summary>
        /// 接收消息
        /// </summary>
        /// <param name="rsp"></param>
        public static void MatchedRsp(CmdNo rsp)
        {
            if (!ms_matchedRspMap.ContainsKey(rsp)) return;
            DebugPro.Log(DebugPro.EnumLog.NetWork, "网络消息超时管理  Rsp接收",rsp.ToString());

            lock (ms_matchedRecord)
            {
                var list = ms_matchedRspMap[rsp];
                for(int i=0;i<list.Count;++i)
                {
                    if (ms_matchedRecord.ContainsKey(list[i]))
                    {
                        ms_matchedRecord[list[i]].IsRemove = true;
                        ms_matchedRecord.Remove(list[i]);
                    }
                }
            }
        }
        #endregion

        /// <summary>
        /// 注册
        /// </summary>
        private static void RegiestCMDMatched()
        {
            //开桌
            AddMatched(CmdNo.MjCmd_NewDesk_Req, CmdNo.MjCmd_NewDesk_Rsp);

            AddMatched(CmdNo.CmdNo_Gateway_Req, CmdNo.CmdNo_Gateway_Rsp);
            //登录
            AddMatched(CmdNo.CmdNo_Login_Auth_Req, CmdNo.CmdNo_Login_Auth_Rsp);
            //进入
            AddMatched(CmdNo.CmdNo_Login_Enter_Req, CmdNo.CmdNo_Login_Enter_Rsp);
            //注册
            AddMatched(CmdNo.CmdNo_Login_Reg_Req, CmdNo.CmdNo_Login_Reg_Rsp);

            AddMatched(CmdNo.MjCmd_AskDeskData_Req, CmdNo.MjCmd_AskDeskData_Rsp);
        }

        /// <summary>
        /// 添加匹配关系
        /// </summary>
        /// <param name="req"></param>
        /// <param name="rsp"></param>
        private static void AddMatched(CmdNo req, CmdNo rsp)
        {
            AddMatchedByMap(ms_matchedReqMap, req, rsp);
            AddMatchedByMap(ms_matchedRspMap, rsp, req);
        }

        /// <summary>
        /// 陪陪关系加入到Map中
        /// </summary>
        /// <param name="map"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        private static void AddMatchedByMap(Dictionary<CmdNo, List<CmdNo>> map, CmdNo key, CmdNo value)
        {
            if (map.ContainsKey(key))
            {
                if (map[key].IndexOf(value) == -1)
                {
                    map[key].Add(value);
                }
                else
                {
                    DebugPro.LogError("添加必回对应列表 重复消息");
                }
            }
            else
            {
                map.Add(key, new List<CmdNo>() { value });
            }
        }

        /// <summary>
        /// 清除对应记录表
        /// </summary>
        public static void ClearMatchedRecord()
        {
            lock (ms_matchedRecord)
            {
                ms_matchedRecord.Clear();
                ms_matchedLinkList.Clear();
            }
        }

        /// <summary>
        /// 循环执行
        /// </summary>
        private static void UpdateForeachMatched()
        {
            if(Time.realtimeSinceStartup - LastCheckTime > WaitTime && ms_matchedRecord.Count > 0)
            {
                CmdNo ErrorCmd = CmdNo.CmdNo_None;
                LastCheckTime = Time.realtimeSinceStartup;
                bool flag = false;
                var item = ms_matchedLinkList.First;
                while (ms_matchedLinkList.Count > 0 && item != null)
                {
                    var next = item.Next;
                    if(item.Value.IsRemove)
                    {
                        ms_matchedLinkList.Remove(item);
                    }
                    else if (Time.realtimeSinceStartup - item.Value.RTime > MaxTimeOut)
                    {
                        //此时这条消息超时 没有回复 重连
                        flag = true;
                        ErrorCmd = item.Value.CmdKey;
                        ms_matchedLinkList.Remove(item);
                        break;
                    }
                    item = next;
                }
                if(flag)
                {
                    OverTimeFunc(ErrorCmd);
                }
            }
        }

        private static void OverTimeFunc(CmdNo errorCmd)
        {
            UserActionManager.AddLocalTypeLog("Log1", "[00FF00]网络重连 OverTimeFunc 发送消息超时 "+ errorCmd.ToString());
            //主动断开网络

            NetWorkerImpl.Instance.CloseConnection();
            ClearMatchedRecord();

            //自动
            if(_R.flow.CurrExecute != null && _R.flow.CurrFlow == QFlowManager.FlowType.InitLoginNew)
            {
                EventDispatcher.FireSysEvent(GEnum.NamedEvent.SysFlow_Flow_CallBack, "Branch_AutoLogin_CallBack", false);
            }
            EventDispatcher.FireSysEvent(GEnum.NamedEvent.SysUI_SmallLoading_Kill);
            /*
            WindowUIManager.Instance.CreateOrAddWindow(WindowUIType.SystemPopupWindow, "提示", "您与服务器已经断开链接，请重新登录", new string[] { "确认" }, delegate (int index)
            {
                EventDispatcher.FireSysEvent(GEnum.NamedEvent.SysUI_OpenLogin,true);
            }, WindowUIRank.Special);*/
        }
    }
}