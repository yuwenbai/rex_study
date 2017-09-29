using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Msg;

namespace projectQ
{

    //登陆模块 数据处理
    public partial class ModelNetWorker
    {
        public void initDefaultHandleOfLogin()
        {
            ModelNetWorker.Regiest<GatewayReq>(GatewayReq);
            ModelNetWorker.Regiest<GatewayRsp>(GatewayRsp);

            ModelNetWorker.Regiest<RegisterRsp>(RegisterRsp);

            ModelNetWorker.Regiest<LoginRsp>(LoginRsp);

            ModelNetWorker.Regiest<EnterGameReq>(EnterGameReq);
            ModelNetWorker.Regiest<EnterGameRsp>(EnterGameRsp);

            ModelNetWorker.Regiest<UserDataNotify>(UserDataNotify);
        }

        /// <summary>
        /// 开启游戏请求
        /// </summary>
        /// <param name="data"></param>
        public void GatewayReq(object data)
        {
            var req = new GatewayReq();
            this.send(req);
        }

        /// <summary>
        /// 开启游戏接收
        /// </summary>
        /// <param name="data"></param>
        public void GatewayRsp(object data)
        {
            var rsp = data as GatewayRsp;

            MemoryData.Set(MKey.AS_IPADDR, rsp.IpAddr);
            MemoryData.Set(MKey.AS_PORT, rsp.Port);
            EventDispatcher.FireSysEvent(GEnum.NamedEvent.EBeforGame, true);
        }

        /// <summary>
        /// 注册请求
        /// </summary>
        /// <param name="userName">用户名</param>
        /// <param name="password">密码</param>
        /// <param name="inviteCode">邀请码</param>
        public void RegisterReq(string userName,string password,int inviteCode)
        {
            var req = new RegisterReq();
            req.account = userName;
            req.password = password;
            req.invitecode = inviteCode;


            MemoryData.LoginDataMgr.SetDataByNormalLogin(userName, password);
            //MemoryData.Set(MKey.LS_AUTO_USERNAME, req.account);
            //MemoryData.Set(MKey.LS_AUTO_PWD, req.password);

            this.send(req);
        }

        /// <summary>
        /// 注册接收
        /// </summary>
        /// <param name="data"></param>
        public void RegisterRsp(object data)
        {
            var rsp = data as RegisterRsp;
            int isOK = rsp.result;
            EventDispatcher.FireSysEvent(GEnum.NamedEvent.ERegistResult, isOK);
        }


        /// <summary>
        /// 登陆请求
        /// </summary>
   //     /// <param name="obj"></param>
   //     public void LoginReq(string userName, string password,int loginType = -1)
   //     {
   //         //if(userName == null)
   //         //{
   //         //    userName = MemoryData.Get<string>(MKey.LS_AUTO_USERNAME);
   //         //}
   //         //if(password == null)
   //         //{
   //         //    password = MemoryData.Get<string>(MKey.LS_AUTO_PWD);
   //         //}
   //         //if(loginType == -1)
   //         //{
   //         //    loginType = MemoryData.Get<int>(MKey.LS_AUTO_LOGIN_TYPE);
   //         //}



   //         var req = new LoginReq();
			//req.account = userName; /*MemoryData.SDKData.Wx.tcode; */
   //         req.password = password;
   //         // 0：游戏内登录，1：微信登录 2:游客登录 3:自动登录
   //         req.logintype = loginType;
            
			//MemoryData.Set(MKey.LS_AUTO_USERNAME, req.account); 
   //         MemoryData.Set(MKey.LS_AUTO_PWD, req.password);
   //         MemoryData.Set(MKey.LS_AUTO_LOGIN_TYPE, req.logintype);
   //         this.send(req);
   //     }


        public void LoginReq(LoginReq req)
        {
            this.send(req);
        }

        /// <summary>
        /// 登陆接收
        /// </summary>
        public void LoginRsp(object data)
        {
            var rsp = data as LoginRsp;
            QLoger.LOG("[LoginRsp] 接收消息 {0}", CommonTools.ReflactionObject(rsp));
            int isOK = rsp.result;
            if (isOK == 0)
            {
                MemoryData.PlayerData.SetMyPlayer(rsp.UserID);
                //MemoryData.Set(MKey.AS_IPADDR, rsp.ip_addr);
                //MemoryData.Set(MKey.AS_TOKEN, rsp.token);
                //MemoryData.Set(MKey.AS_PORT, rsp.port);

                //this.openAccesstServer();
                
                //string openId = DESPassword.Encrypt(rsp.OpenID , "pFV78VlX");

                if(!string.IsNullOrEmpty(rsp.OpenID))
                {
                    MemoryData.LoginDataMgr.SetDataByAutoLogin(rsp.OpenID);
                }
                //PlayerPrefsTools.SetString(MKey.LOGIN_OPEND_ID, openId);
                //PlayerPrefsTools.SetString(MKey.LS_AUTO_USERNAME, MemoryData.Get<string>(MKey.LS_AUTO_USERNAME));
                //PlayerPrefsTools.SetString(MKey.LS_AUTO_PWD, MemoryData.Get<string>(MKey.LS_AUTO_PWD));
                //PlayerPrefsTools.SetInt(MKey.LS_AUTO_LOGIN_TYPE, MemoryData.Get<int>(MKey.LS_AUTO_LOGIN_TYPE));
                MemoryData.LoginDataMgr.Save();

                NetWorkerImpl.UserLogicNetCanSend = true;
            }
            else
            {
                EventDispatcher.FireSysEvent(GEnum.NamedEvent.SysFlow_Flow_CallBack, "Branch_AutoLogin_CallBack", false);
                MemoryData.LoginDataMgr.Reset();
            }

            EventDispatcher.FireSysEvent(GEnum.NamedEvent.ELoginResult, isOK);

        }

        /// <summary>
        /// 进入游戏请求
        /// </summary>
        public void EnterGameReq(object data)
        {
            var req = new EnterGameReq();
            req.UserID = MemoryData.UserID;
            //req.token = MemoryData.Get<int>(MKey.AS_TOKEN);

            this.send(req);
        }


        /// <summary>
        /// 进入游戏接收
        /// </summary>
        public void EnterGameRsp(object data)
        {
            var rsp = data as EnterGameRsp;
            int result = rsp.result;
            if (result == 0)
            {
                MemoryData.PlayerData.MyPlayerModel.PlayerDataBase.UserID = rsp.user_id;
                //EventDispatcher.FireSysEvent(GEnum.NamedEvent.EEnterGameResult, result);
            }
            else
            { 
                //WindowUIManager.Instance.CreateErrorWindow(result,null);
            }
            EventDispatcher.FireSysEvent(GEnum.NamedEvent.EEnterGameResult, result);
            EventDispatcher.FireSysEvent(GEnum.NamedEvent.SysFlow_Flow_CallBack, "Branch_AutoLogin_CallBack", result == 0);
        }

        /// <summary>
        /// 用户基本信息接收
        /// </summary>
        public void UserDataNotify(object data)
        {
            var rsp = data as UserDataNotify;

            MemoryData.PlayerData.MyPlayerModel.SetUserInfo(rsp.UserData);
            MemoryData.SysPlayerDataHandle.PlayerRsp(rsp.UserData.UserID, SysPlayerDataHandle.PlayerDataType.UserInfo);
        }
    }

}

