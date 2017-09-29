using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using projectQ;

public class UILogin : UIViewBase
{
    public UILoginModel Model
    {
        get { return _model as UILoginModel; }
    }

    [Tooltip("微信登录按钮")]
    public GameObject WXButton;
    [Tooltip("游客登录按钮")]
    public GameObject YKButton;
    [Tooltip("注册按钮")]
    public GameObject RegisterButton;
    [Tooltip("登录按钮")]
    public GameObject LoginButton;
    [Tooltip("账号")]
    public UIInput InputAccount;
    [Tooltip("密码")]
    public UIInput InputPassword;
    [Tooltip("同意使用协议")]
    public GameObject ButtonAgreement;
    [Tooltip("协议复选框")]
    public UIToggle ToggleAgreement;
    [Tooltip("退出游戏按钮")]
    public GameObject GameQuitBtn;

    /// <summary>
    /// 输入用户名密码界面
    /// </summary>
    public GameObject LoginOldObj;
    /// <summary>
    /// 游戏提示
    /// </summary>
    public UILabel NoticeLab;
    /// <summary>
    /// 游戏提示
    /// </summary>
    public UILabel RecordNumberLab;
    /// <summary>
    /// 微信登录按鈕obj
    /// </summary>
    public GameObject WxLoginObj;
    /// <summary>
    /// 特效控制显示隐藏
    /// </summary>
    public GameObject LoginEffectObj;
    /// <summary>
    /// 背景的Anchor、
    /// </summary>
    public UIAnchor LoginBgAnchorObj;

    #region override ------------------------------------------------------------

    public override void Init()
    {
        Init_BtnClick();

        Init_Text();

        Init_LoginInfo();

        Init_LoginBtn();

        Init_Login();

        Init_LoginShow();
    }

    public override void OnHide() { }

    public override void OnShow()
    {
        LoginBgAnchorObj.enabled = true;

        MemoryData.GameStateData.SetCurrUISign(SysGameStateData.EnumUISign.Login);
        LoginEffectObj.SetActive(true);

        _R.ui.ClearAll(new List<string>() { "UILogin" });
        MessageStagingManager.Instance.ClearAll();
        //主动断开连接
        LoginEnterNetWork.Instance.Init();
        LoginEnterNetWork.Instance.Disconnect();

        MemoryData.Reset();
        MemoryData.LoginDataMgr.ClearAutoLoginData();
        _R.flow.SetQueue(QFlowManager.FlowType.InitLogin);

        EventDispatcher.FireEvent(GEnum.NamedEvent.SysTools_FunctionShow);
    }

    public override void GoBack()
    {
        this.OnGameQuitBtnClick(null);
    }

    #endregion ------------------------------------------------------------

    #region Common --------------------------------------------------------

    /// <summary>
    /// 根据不同的宏定义选择不同的登录显示信息
    /// </summary>
    public void Init_LoginShow()
    {
#if __DEBUG_NOT_SELECT_SERVER
        LoginOldObj.SetActive(true);
        ServerSelectBtn.SetActive(false);
        ServerSelectPanel.gameObject.SetActive(false);
#endif
#if __DEBUG
        LoginOldObj.SetActive(true);
#elif __DEBUG_FIXWX
        LoginOldObj.SetActive(false);
#endif
    }

    /// <summary>
    /// 清理输入文本框数据
    /// </summary>
    public void ClearInput()
    {
        InputAccount.value = "";
        InputPassword.value = "";
    }

    /// <summary>
    /// 按钮点击初始化
    /// </summary>
    void Init_BtnClick()
    {
        UIEventListener.Get(WXButton).onClick = OnWXBtnClick;
        UIEventListener.Get(YKButton).onClick = OnGustBtnClick;
        UIEventListener.Get(RegisterButton).onClick = OnRegisterClick;
        UIEventListener.Get(LoginButton).onClick = OnLoginBtnClick;
        UIEventListener.Get(ButtonAgreement).onClick = OnButtonAgreementClick;
        UIEventListener.Get(GameQuitBtn).onClick = OnGameQuitBtnClick;

        //选择服务器按钮事件
        UIEventListener.Get(ServerSelectBtn).onClick = OnServerSelectClick;

        //同意使用协议事件
        EventDelegate.Add(ToggleAgreement.onChange, OnToggleAgreementChange);
    }

    /// <summary>
    /// 登录界面用户名，密码初始化
    /// </summary>
    void Init_LoginInfo()
    {
        string userName = MemoryData.LoginDataMgr.LoginData.UserName;//PlayerPrefsTools.GetString(MKey.LS_AUTO_USERNAME);
        string userPwd = MemoryData.LoginDataMgr.LoginData.PassWord;
        if (!string.IsNullOrEmpty(userName) && !string.IsNullOrEmpty(userPwd))
        {
            this.InputAccount.value = userName;
            this.InputPassword.value = userPwd;
        }
    }

    /// <summary>
    /// 根据数据表决定当前是否显示游客登录按钮
    /// </summary>
    void Init_LoginBtn()
    {
        string shStr = "";

#if UNITY_ANDROID
        shStr = "IsShow";
#elif UNITY_IOS
        shStr = "IsIOSShow";
#endif

#if __BUNDLE_IOS_SERVER
        shStr = "IsIosJudgeShow";
#endif

        string showStr = MemoryData.XmlData.XmlBuildDataSole_Get("FunctionIcon", "FunctionID",
            ((int)GEnum.FunctionIconEnum.Function_2000).ToString(), shStr);

        if (showStr.Equals("0"))
        {
            //显示按钮            
            YKButton.SetActive(true);
            WXButton.SetActive(false);
        }
        else
        {
            //隐藏按钮
            YKButton.SetActive(false);
            WXButton.SetActive(true);
        }
    }

    /// <summary>
    /// 登录界面文本初始化
    /// </summary>
    void Init_Text()
    {
        string noticeStr = MemoryData.XmlData.XmlBuildDataText_Get(GTextsEnum.G_Text_102);
        if (noticeStr != "")
        {
            NoticeLab.text = noticeStr;
        }

        string recordStr = MemoryData.XmlData.XmlBuildDataText_Get(GTextsEnum.G_Text_101);
        if (recordStr != "")
        {
            RecordNumberLab.text = recordStr;
        }
    }

    /// <summary>
    /// 登录按钮点击回调
    /// </summary>
    void Init_Login()
    {
        SDKManager.Instance.SDKFunction(15);
    }

    #endregion ------------------------------------------------------------

    #region 登录注册 ------------------------------------------------------

    /// <summary>
    /// 注册按钮点击回调
    /// </summary>
    public void OnRegisterClick(GameObject go)
    {
        //随机生成四位数在用户名和密码后面

        if (InputAccount.value != "" && InputPassword.value != "")
        {
            int num = Random.Range(1000, 9999);

            string userName = InputAccount.value;
            string pwd = InputPassword.value;
            string n = userName.Substring(userName.Length - 1, 1);
            int a = 0;
            if (!int.TryParse(n, out a))
            {
                userName = InputAccount.value + num.ToString();
                pwd = userName;
            }

            QLoger.LOG("注册的时候随机生成一个四位数加载到用户名后面 注册的用户名 = " + userName);

            Model.OnLoginRegistReq(userName, pwd);
        }
        else
        {
            this.LoadPop(WindowUIType.SystemPopupWindow, "错误", "请检查用户名和密码是否为空", new string[] { "确定" }
            , (index) => { });
        }
    }

    /// <summary>
    /// 普通登录按钮点击回调
    /// </summary>
    public void OnLoginBtnClick(GameObject go)
    {
        Model.C2SLoginReq(InputAccount.value, InputPassword.value);
    }

    /// <summary>
    /// 登录按钮点击回调
    /// </summary>
    public void OnWXBtnClick(GameObject go)
    {
        if (Model.IsWx.Equals("false"))
        {
            this.LoadPop(WindowUIType.SystemPopupWindow, "登录出错",
                "您未安装微信，请安装微信后再试！", new string[] { "确定" }, delegate (int index)
                {
                    Application.Quit();
                });
        }
        else
        {
            //yqc 每次启动微信登录授权 清除Tcode 防止二次登录Tcode有值登录失败BUG
            MemoryData.SDKData.Wx.Tcode = "";
            SDKManager.Instance.SDKFunction(5);
        }
    }

    /// <summary>
    /// 游客按钮点击回调
    /// </summary>
    public void OnGustBtnClick(GameObject go)
    {
        Model.C2SLoginReq("", "", 2);
    }

    /// <summary>
    /// 退出游戏按钮点击回调
    /// </summary>
    public void OnGameQuitBtnClick(GameObject go)
    {
        this.LoadPop(WindowUIType.SystemPopupWindow, "退出游戏", "是否退出游戏", new string[] { "取消", "确定" }
        , (index) =>
        {
            if (index == 1)
            {
                Application.Quit();
            }
        });
    }

    #endregion -------------------------------------------------------------

    #region 点击选择服务器列表 ---------------------------------------------

    /// <summary>
    /// 选择服务器列表按钮
    /// </summary>
    public GameObject ServerSelectBtn;
    /// <summary>
    /// 选择服务器列表按钮文字
    /// </summary>
    public UILabel ServerSelectBtnLab;
    /// <summary>
    /// 服务器选择面板
    /// </summary>
    public UIServerSelect ServerSelectPanel;

    /// <summary>
    /// 点击选择服务器
    /// </summary>
    void OnServerSelectClick(GameObject obj)
    {
        ServerSelectPanel.gameObject.SetActive(true);
        ServerSelectPanel.ServerSelectInit();
    }

    /// <summary>
    /// 服务器选择成功后返回
    /// </summary>
    public void OnServerSelectSucc(ServerList sData)
    {
        // ip赋值
        LoginEnterNetWork.Instance.GatewayServerIp = sData.Ip;

        // 端口赋值
        LoginEnterNetWork.Instance.GatewayServerPort = int.Parse(sData.Port);

        //记录服务器Id，下次默认选中
        PlayerPrefsTools.SetInt("TEST_SERVER_ID", int.Parse(sData.ID));

        ServerSelectBtnLab.text = sData.Ip + " : " + sData.Port + " : " + sData.Name;
    }

    #endregion -------------------------------------------------------------

    #region 同意规则 -------------------------------------------------------

    /// <summary>
    /// “同意使用协议” - 复选框 数据变化
    /// </summary>
    private void OnToggleAgreementChange()
    {
        WXButton.GetComponent<BoxCollider>().enabled = ToggleAgreement.value;
        WXButton.GetComponent<UISprite>().color = ToggleAgreement.value ? Color.white : Color.gray;
    }

    private void OnButtonAgreementClick(GameObject go)
    {
        string mentStr = MemoryData.XmlData.XmlBuildDataText_Get(GTextsEnum.G_Text_2);

        this.LoadPop(WindowUIType.SystemPopupPlusWindow, "用户协议", mentStr,
            new string[] { "关闭", "同意协议" }, (index) =>
            {
                if (index == 1)
                {
                    ToggleAgreement.value = true;
                }
            });
    }

    #endregion ------------------------------------------------------------------

    #region 登陆成功 ------------------------------------------------------------

    /// <summary>
    /// 登陆成功
    /// </summary>
    public void OnLoginSouccess()
    {
        //StartCoroutine(OpenUIMain());
    }

    private IEnumerator OpenUIMain()
    {
        yield return new WaitForSeconds(1f);
        this.Hide();
    }

    #endregion ------------------------------------------------------------------

    #region 控制动画运动 --------------------------------------------------------

    /// <summary>
    /// 登录界面的按钮做tween动画
    /// </summary>
    public void WxLoginTweenBegin()
    {
        RecordNumberLab.transform.localPosition = Vector3.zero;
        Vector3 endV3 = new Vector3(0.0f, 120.0f, 0.0f);
        TweenPosition.Begin(RecordNumberLab.gameObject, 0.3f, endV3);

        WxLoginObj.transform.localPosition = new Vector3(0.0f, 155.0f, 0.0f);
        TweenPosition tp = TweenPosition.Begin(WxLoginObj, 0.5f, Vector3.zero);

        EventDelegate.Add(tp.onFinished, () =>
        {
            WxLoginTweenFinished();
        });

        tp.PlayForward();
    }

    void WxLoginTweenFinished()
    {
        QLoger.LOG(" 登录界面 -- 动画播放完毕 ");
        EventDispatcher.FireSysEvent(GEnum.NamedEvent.SysFlow_Flow_CallBack, "PlayUILoginEndTween");
    }

    #endregion ------------------------------------------------------------------
}