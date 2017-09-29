/**
 * @Author YQC
 *
 *
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Msg ;

namespace projectQ
{
    //注册数据处理 好友网络数据处理
    public partial class ModelNetWorker {

        public void initDefaultHandleOfOther()
        {
            ModelNetWorker.Regiest<WeiXinSignRsp>(WeiXinSignRsp);
            ModelNetWorker.Regiest<CertificationRsp>(CertificationRsp);
            ModelNetWorker.Regiest<BindPhoneRsp>(BindPhoneRsp);            
            
            ModelNetWorker.Regiest<UserRegionSetupRsp>(UserRegionSetupRsp);
            
            ModelNetWorker.Regiest<MsgBoardNotify>(MsgBoardNotify);

            //踢下线
            ModelNetWorker.Regiest<PlayerKickNotify>(PlayerKickNotify);
            
            ModelNetWorker.Regiest<ReportGPSInfoRsp>(ReportGPSInfoRsp);
            ModelNetWorker.Regiest<PlayerWarningNotify>(PlayerWarningNotify);
            
            ModelNetWorker.Regiest<UserGPSInfoRsp>(UserGPSInfoRsp);

            ModelNetWorker.Regiest<NetErrBreakNotify>(NetErrBreakNotify);

            ModelNetWorker.Regiest<AskDeskDataRsp>(AskDeskDataRsp);            
        }

        #region Get Message

        /// <summary>
        /// 获得我的道具列表
        /// </summary>
        /// <param name="rsp">Rsp.</param>
        public void WeiXinSignRsp(object rsp)
        {
            var prsp = rsp as WeiXinSignRsp;
            //TODO 微信登陆处理

        }


        /// <summary>
        /// 返回设置地区结果
        /// </summary>
        /// <param name="rsp">Rsp.</param>
        public void UserRegionSetupRsp(object rsp)
        {
            var prsp = rsp as UserRegionSetupRsp;
            EventDispatcher.FireSysEvent(GEnum.NamedEvent.SysData_Region_Update);
            //prsp.ResultCode;
        }

        /// <summary>
        /// 实名认证返回信息
        /// </summary>
        public void CertificationRsp(object obj)
        {
            var rsp = obj as CertificationRsp;
            EventDispatcher.FireSysEvent(GEnum.NamedEvent.SysData_Certification_Rsp, rsp.ResultCode);
        }

        public void BindPhoneRsp(object obj)
        {
            var rsp = obj as BindPhoneRsp;
            EventDispatcher.FireSysEvent(GEnum.NamedEvent.SysData_PhoneNoBind_Rsp, rsp.ResultCode);
        }

        /// <summary>
        /// 消息Board
        /// </summary>
        /// <param name="obj"></param>
        public void MsgBoardNotify(object obj)
        {
            var rsp = obj as MsgBoardNotify;
            EventDispatcher.FireSysEvent(GEnum.NamedEvent.SysData_Other_Msg_Notify,rsp.MsgContent);
        }

        /// <summary>
        /// 踢下线提示
        /// </summary>
        /// <param name="obj"></param>
        public void PlayerKickNotify(object obj)
        {
            var rsp = obj as PlayerKickNotify;
            EventDispatcher.FireSysEvent(GEnum.NamedEvent.ENetError_Kick);
        }


        

        #endregion

        #region Send Message

        /// <summary>
        /// 微信注册请求
        /// </summary>
        public void WeiXinSignReq(string addr)
        {
            var msg = new WeiXinSignReq();
            msg.addr = addr ;
            this.send (msg);
        }

        /// <summary>
        /// 请求设置地区
        /// </summary>
        /// <param name="localtion">Localtion.</param>
        public void UserRegionSetupReq(int localtion)
        {
            var msg = new UserRegionSetupReq();
            msg.UserID = MemoryData.UserID ;
            msg.RegionID = localtion;
            this.send (msg);
        }

        /// <summary>
        /// 实名认证
        /// </summary>
        /// <param name="name">姓名</param>
        /// <param name="idCard">身份证号</param>
        public void CertificationReq(string name, string idCard)
        {
            var req = new CertificationReq();
            req.UserID = MemoryData.UserID;
            req.RealName = name;
            req.IDCard = idCard;
            req.Sex = ((int.Parse(idCard.Substring(16, 1)) % 2) == 0) ? 2 : 1;
            this.send(req);
        }

        /// <summary>
        /// 绑定手机
        /// </summary>
        /// <param name="phone">Phone.</param>
        /// <param name="vcode">Vcode.</param>
        public void BindPhoneReq(string phoneNo, string code)
        {
            var req = new BindPhoneReq();
            req.PhoneNo = phoneNo;
            req.UserID = MemoryData.UserID;
            req.VerifyCode = code;
            this.send(req);
        }


        /// <summary>
        /// 获取验证码
        /// </summary>
        /// <param name="phoneNo">11位手机号</param>
        /// <param name="isAgent">是否代理申请</param>
        public void GetVerifyCodeReq(string phoneNo, bool isAgent = false)
        {
            var req = new GetVerifyCodeReq();
            req.PhoneNo = phoneNo;
            req.UserID = MemoryData.UserID;
            req.VerifyType = isAgent ? VerifyTypeDef.Verify_AgentApply : VerifyTypeDef.Verify_BindPhone;
            this.send(req);
        }
        
        /// <summary>
        /// Todo 作弊码 MjTestReq
        /// </summary>
        public void MjTestReq(int deskId, List<int> nHandCodeE, List<int> nHandCodeS, List<int> nHandCodeW, List<int> nHandCodeN,
                                            List<int> nGetCodeE, List<int> nGetCodeS, List<int> nGetCodeW, List<int> nGetCodeN)
        {
            var req = new MjTestReq();
            req.nDeskID = deskId;
            if (nHandCodeE != null)
            {
                for (int i = 0; i < nHandCodeE.Count; i++)
                {
                    req.nHandCodeE.Add(nHandCodeE[i]);
                }
            }
            if (nHandCodeS != null)
            {
                for (int i = 0; i < nHandCodeS.Count; i++)
                {
                    req.nHandCodeS.Add(nHandCodeS[i]);
                }
            }

            if (nHandCodeW != null)
            {
                for (int i = 0; i < nHandCodeW.Count; i++)
                {
                    req.nHandCodeW.Add(nHandCodeW[i]);
                }
            }

            if (nHandCodeN != null)
            {
                for (int i = 0; i < nHandCodeN.Count; i++)
                {
                    req.nHandCodeN.Add(nHandCodeN[i]);
                }
            }
            if (nGetCodeE != null)
            {
                for (int i = 0; i < nGetCodeE.Count; i++)
                {
                    req.nGetCodeE.Add(nGetCodeE[i]);
                }
            }
            if (nGetCodeS != null)
            {
                for (int i = 0; i < nGetCodeS.Count; i++)
                {
                    req.nGetCodeS.Add(nGetCodeS[i]);
                }
            }
            if (nGetCodeW != null)
            {
                for (int i = 0; i < nGetCodeW.Count; i++)
                {
                    req.nGetCodeW.Add(nGetCodeW[i]);
                }
            }
            if (nGetCodeN != null)
            {
                for (int i = 0; i < nGetCodeN.Count; i++)
                {
                    req.nGetCodeN.Add(nGetCodeN[i]);
                }
            }
            this.send(req);
        }
        #endregion

        #region GPS
        public void ReportGPSInfoReq(int deskId,GPSData gpsData)
        {
            DebugPro.Log(DebugPro.EnumLog.System, "GPS发送");

            if (gpsData == null) return;
            //modify by rexzhao 重连状态不发送gps数据到服务器
            if (!FakeReplayManager.Instance.ReplayState)
            {
                var req = new ReportGPSInfoReq();
                req.UserID = MemoryData.UserID;
                req.DeskID = deskId;
                req.PosX = gpsData == null ? 0f : gpsData.Longitude;
                req.PosZ = gpsData == null ? 0f : gpsData.Latitude;
                this.send(req);
            }
        }
        void ReportGPSInfoRsp(object obj)
        {
            var rsp = obj as ReportGPSInfoRsp;
            DebugPro.Log(DebugPro.EnumLog.NetWork, "ReportGPSInfoRsp", rsp.ResultCode);
        }

        public void UserGPSInfoReq(GPSData gpsData)
        {
            DebugPro.Log(DebugPro.EnumLog.System, "GPS发送");


            var req = new UserGPSInfoReq();
            req.UserID = MemoryData.UserID;
            req.PosX = gpsData == null ? 0f : gpsData.Longitude;
            req.PosZ = gpsData == null ? 0f : gpsData.Latitude;
            this.send(req);
        }
        void UserGPSInfoRsp(object obj)
        {
            var rsp = obj as UserGPSInfoRsp;
            DebugPro.Log(DebugPro.EnumLog.NetWork, "UserGPSInfoRsp", rsp.ResultCode);
        }

        void PlayerWarningNotify(object obj)
        {
            var rsp = obj as PlayerWarningNotify;

            QLoger.LOG("[PlayerWarningNotify] 接收消息 {0}", CommonTools.ReflactionObject(rsp));

            MemoryData.OtherData.ClearCheatInfo();
            MemoryData.OtherData.AddCheatInfoByNetWork(rsp.PlayerList);
        }
        #endregion

        #region NetErr
        private void NetErrBreakNotify(object obj)
        {
            var rsp = obj as NetErrBreakNotify;
            UserActionManager.AddLocalTypeLog("Log1", "NetErrBreakNotify 服务器发送消息 主动调用关闭网络连接");

            EventDispatcher.FireSysEvent(GEnum.NamedEvent.NetWork_CloseConnection);
        }
        #endregion

        #region 断线重连2
        public void AskDeskDataReq()
        {
            var req = new AskDeskDataReq();
            req.UserID = MemoryData.UserID;
            this.send(req);
        }
        private void AskDeskDataRsp(object obj)
        {
            var rsp = obj as AskDeskDataRsp;
            if(rsp != null)
            {
                if(rsp.ResultCode == (int) Msg.ErrorCode.ErrCode_FRoom_DeskInvalid)
                {
                    EventDispatcher.FireSysEvent(GEnum.NamedEvent.LoadMahjongScene, false, false);
                }
                else if(rsp.ResultCode != 0)
                {
                    UserActionManager.AddLocalTypeLog("NetS", "[FF0000FF]小小重连消息错误AskDeskDataRsp ResultCode=" + rsp.ResultCode);
                    EventDispatcher.FireSysEvent(GEnum.NamedEvent.ENetError_Client);
                }
            }
            else
            {
                EventDispatcher.FireSysEvent(GEnum.NamedEvent.ENetError_Client);
            }
            //QLoger.LOG("AskDeskDataRsp 接收");
        }
        #endregion
    }
}