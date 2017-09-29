using UnityEngine;
using System.Collections;
using System;
using projectQ;

public class UILoginTempModel : UIModelBase
{
    public UILoginTemp UI {
        get { return _ui as UILoginTemp; }
    }
    string m_username = "";
    string m_password = "";
    int m_inviteCode = 0;
    int m_loginType = 0;
    Action<bool> CallBack = null;

    public void SaveUserNamePwd()
    {
        MemoryData.LoginDataMgr.Save();
        //if(PlayerPrefsTools.GetString(MKey.LS_AUTO_USERNAME) != m_username)
        //{
        //    PlayerPrefsTools.DeleteAll();
        //    PlayerPrefsTools.SetString(MKey.LS_AUTO_USERNAME, m_username);
        //    PlayerPrefsTools.SetString(MKey.LS_AUTO_PWD, m_password);
        //}
    }

    #region 注册
    public void OnLoginRegistReq(string userName,string userPwd)
    {
        m_username = userName;
        m_password = userPwd;
        m_inviteCode = 0;
        this.CallBack = OnLoginRegistReqCallback;
        this.OnConnect();
    }
    private void OnLoginRegistReqCallback(bool isSuccess)
    {
        if(isSuccess)
            ModelNetWorker.Instance.RegisterReq(this.m_username,this.m_password,this.m_inviteCode);
    }
    #endregion

    #region 登录
    public void OnLoginReq(string userName, string userPwd,int loginType = 0)
    {
        m_username = userName;
        m_password = userPwd;
        m_loginType = loginType;
        this.CallBack = OnLoginReqCallback;
        this.OnConnect();
    }
    private void OnLoginReqCallback(bool isSucess)
    {
        if(isSucess)
        {
            MemoryData.LoginDataMgr.LoginData.SetData((SysLoginData.EnumLoginType)this.m_loginType, this.m_username, this.m_password);
            EventDispatcher.FireSysEvent(GEnum.NamedEvent.SysLoginRequest, (SysLoginData.EnumLoginType)this.m_loginType);
        }
            //ModelNetWorker.Instance.LoginReq(this.m_username, this.m_password, this.m_loginType);
    }
    #endregion

    private void OnConnect()
    {
        //if(LoginEnterNetWork.Instance.IsConnect)
        //{
        //    CallBack(true);
        //}
        //else
        {
            LoginEnterNetWork.Instance.Connect(CallBack);
        }
    }
    #region override
    protected override GEnum.NamedEvent[] FocusNetWorkData()
    {
        return new GEnum.NamedEvent[] {
            GEnum.NamedEvent.ERegistResult,
            GEnum.NamedEvent.ELoginResult,
            GEnum.NamedEvent.EEnterGameResult,
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
                        OnLoginReq(m_username,m_password);
                    }
                    else
                    {
                        UI.ClearInput();
                        WindowUIManager.Instance.CreateOrAddWindow(WindowUIType.SystemPopupWindow, "注册失败", "该用户名已经被使用，请尝试其他名字", new string[] { "确定" }
                            , delegate (int index)
                            {
                            });
                        DebugPro.LogError("注册错误" + result);
                    }
                }
                break;
            case GEnum.NamedEvent.ELoginResult:
                {
                    int result = (int)data[0];
                    if (result == 0)
                    {
                        //登录成功后计时器开始
                        MemoryData.Set(MKey.USER_WALLOW_TIME, Time.realtimeSinceStartup);
                        SaveUserNamePwd();
                    }
                    else
                    {
                        UI.LoadPop(WindowUIType.SystemPopupWindow, "登录出错", "账号密码错误! 请重新输入", new string[] { "确定" },
                           delegate (int index)
                           {
                               UI.InputPassword.value = "";
                           });
                    }
                }
                break;
            case GEnum.NamedEvent.EEnterGameResult:
                {
                    int result = (int)data[0];
                    if (result == 0)
                    {
                        UI.OnLoginSouccess();
                    }
                    else
                    {
                        UI.LoadError(result, (index) =>
                        {
                        });
                        DebugPro.LogError("进入游戏错误" + result);
                    }
                }
                break;
        }
    }

    #endregion

}
