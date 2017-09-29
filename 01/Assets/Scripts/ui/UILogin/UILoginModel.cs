using UnityEngine;
using System.Collections;
using System;
using projectQ;

public class UILoginModel : UIModelBase
{
    public UILogin UI
    {
        get { return _ui as UILogin; }
    }
    string m_username = "";
    string m_password = "";
    int m_inviteCode = 0;
    int m_loginType = 0;
    Action<bool> CallBack = null;

    private string _IsWx = "";
    public string IsWx
    {
        set { _IsWx = value; }
        get { return _IsWx; }
    }

    //public void SaveUserNamePwd()
    //{
    //    //MemoryData.LoginDataMgr.Save();
    //    //if (PlayerPrefsTools.GetString(MKey.LS_AUTO_USERNAME) != m_username)
    //    //{
    //    //    PlayerPrefsTools.DeleteAll();
    //    //    PlayerPrefsTools.SetString(MKey.LS_AUTO_USERNAME, m_username);
    //    //    PlayerPrefsTools.SetString(MKey.LS_AUTO_PWD, m_password);
    //    //}
    //}

    #region 注册 -----------------------------------------------------

    public void OnLoginRegistReq(string userName, string userPwd)
    {
        m_username = userName;
        m_password = userPwd;
        m_inviteCode = 0;
        this.CallBack = OnLoginRegistReqCallback;
        this.OnConnect();
    }

    private void OnLoginRegistReqCallback(bool isSuccess)
    {
        if (isSuccess)
        {
            ModelNetWorker.Instance.RegisterReq(this.m_username, this.m_password, this.m_inviteCode);
        }
        else
        {
            //UI.LoadPop(WindowUIType.SystemPopupWindow, "提示", "网络连接失败，请确认已联网", new string[] { "确认" }, (index) => { });
        }
    }

    #endregion -------------------------------------------------------

    #region 登录 -----------------------------------------------------

    /// <summary>
    /// 登录操作
    /// 0：游戏内登录，1：微信登录，2：游客，3：自动登录，使用openid
    /// </summary>
    public void C2SLoginReq(string userName, string userPwd, int loginType = 0)
    {
        m_username = userName;
        m_password = userPwd;
        m_loginType = loginType;
        this.CallBack = OnLoginReqCallback;
        this.OnConnect();
    }

    private void OnLoginReqCallback(bool isSuccess)
    {
        if (isSuccess)
        {
            QLoger.LOG(" ----------------------- #[Unity - 发送登录消息]# m_username = " + m_username);
            MemoryData.LoginDataMgr.LoginData.SetData((SysLoginData.EnumLoginType)this.m_loginType, this.m_username, this.m_password);
            EventDispatcher.FireSysEvent(GEnum.NamedEvent.SysLoginRequest, (SysLoginData.EnumLoginType)this.m_loginType);
            //ModelNetWorker.Instance.LoginReq(this.m_username, this.m_password, this.m_loginType);
        }
        else
        {
            //UI.LoadPop(WindowUIType.SystemPopupWindow, "提示", "网络连接失败,请确认已联网", new string[] { "确认" }, (index) => { });
        }
    }

    #endregion -------------------------------------------------------

    private void OnConnect()
    {
        //if (LoginEnterNetWork.Instance.IsConnect)
        //{
        //    CallBack(true);
        //}
        //else
        {
            //强制断开连接
            NetWorkerImpl.Instance.CloseConnectionWithoutMessage();
            LoginEnterNetWork.Instance.Connect(CallBack);
        }
    }

    #region override -----------------------------------------------------

    protected override GEnum.NamedEvent[] FocusNetWorkData()
    {
        return new GEnum.NamedEvent[] {
            GEnum.NamedEvent.ERegistResult,
            GEnum.NamedEvent.ELoginResult,
            GEnum.NamedEvent.EEnterGameResult,
            GEnum.NamedEvent.ELoginWXResult,
            GEnum.NamedEvent.EWXInstalledResult,
            GEnum.NamedEvent.SysUI_PlayUILoginEndTween,

            //CmdNo.CmdNo_Login_Reg_Rsp,
            //CmdNo.CmdNo_Login_Auth_Rsp,
            //CmdNo.CmdNo_Login_Enter_Rsp,
        };
    }

    protected override void OnNetWorkDataCallBack(GEnum.NamedEvent msgEnum, object[] data)
    {
        switch (msgEnum)
        {
            case GEnum.NamedEvent.ERegistResult:
                {
                    int result = (int)data[0];
                    if (result == 0)
                    {
                        C2SLoginReq(m_username, m_password);
                    }
                    else
                    {
                        MemoryData.SDKData.Wx.Tcode = "";
                        UI.ClearInput();
                        WindowUIManager.Instance.CreateOrAddWindow(WindowUIType.SystemPopupWindow,
                            "注册失败", "该用户名已经被使用，请尝试其他名字", new string[] { "确定" }, delegate (int index) { });

                        DebugPro.LogError("注册错误" + result);
                    }
                }
                break;
            case GEnum.NamedEvent.ELoginResult:
                {
                    int result = (int)data[0];
                    if (result == 0)
                    {
                        ////登录成功后计时器开始
                        //MemoryData.Set(MKey.USER_WALLOW_TIME, Time.realtimeSinceStartup);
                        //SaveUserNamePwd();
                    }
                    else if (result != (int)Msg.ErrorCode.ErrCode_Version_Error)
                    {
                        _ui.LoadError(result, (index) =>
                        {
                            UI.InputPassword.value = "";
                        });
                        DebugPro.LogError("登陆错误" + result);
                    }
                }
                break;
            //case GEnum.NamedEvent.EEnterGameResult:
            //    {
            //        int result = (int)data[0];
            //        if (result == 0)
            //        {
            //            UI.OnLoginSouccess();
            //        }
            //        else
            //        {
            //            UI.LoadError(result,(index) => {
            //            });
            //            //WindowUIManager.Instance.CreateErrorWindow(result, null);
            //            DebugPro.LogError("进入游戏错误" + result);
            //        }
            //    }
            //    break;
            case GEnum.NamedEvent.ELoginWXResult:
                C2SLoginReq(MemoryData.SDKData.Wx.Tcode, "", 1);
                break;
            case GEnum.NamedEvent.EWXInstalledResult:
                string rValue = data[0].ToString();
                IsWx = rValue;
                break;
            case GEnum.NamedEvent.SysUI_PlayUILoginEndTween:
                UI.WxLoginTweenBegin();
                break;
        }
    }

#endregion -------------------------------------------------------
}