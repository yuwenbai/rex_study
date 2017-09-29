using UnityEngine;
using System.Collections;
using projectQ;

public class UILoginTemp : UIViewBase
{
    public UILoginTempModel Model {
        get { return _model as UILoginTempModel; }
    }

    [Tooltip("登录按钮")]
    public GameObject WXButton;
    [Tooltip("注册按钮")]
    public GameObject RegisterButton;
    [Tooltip("账号")]
    public UIInput InputAccount;
    [Tooltip("密码")]
    public UIInput InputPassword;    
    [Tooltip("退出游戏按钮")]
    public GameObject GameQuitBtn;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            this.LoadUIMain("UILoginBackUp");
        }            
    }

    public void ClearInput()
    {
        InputAccount.value = "";
        InputPassword.value = "";
    }

    #region 登录注册 ------------------------------------------------------

    /// <summary>
    /// 注册按钮点击回调
    /// </summary>
    public void OnRegisterClick(GameObject go)
    {
        if (InputAccount.value != "" && InputPassword.value != "")
        {
			string userName = InputAccount.value;
			string pwd = InputPassword.value;
                        
			Model.OnLoginRegistReq(userName , pwd);
        }
        else
        {
            this.LoadPop(WindowUIType.SystemPopupWindow, "错误", "请检查用户名和密码是否为空", new string[] { "确定" }
            , (index) => { });
        }        
    }

    /// <summary>
    /// 登录按钮点击回调
    /// </summary>
    public void OnWXBtnClick(GameObject go)
    {
        Model.OnLoginReq(InputAccount.value, InputPassword.value);
    }
    
    /// <summary>
    /// 退出游戏按钮点击回调
    /// </summary>
    public void OnGameQuitBtnClick(GameObject go)
    {
        this.LoadPop(WindowUIType.SystemPopupWindow, "退出游戏", "是否退出游戏", new string[] { "确定" , "取消" }
        , (index) => 
        {
            if (index == 0)
            {
                Application.Quit();
            }
            else
            {

            }
        });
    }

    #endregion -------------------------------------------------------------
        
    #region 登陆成功 ------------------------------------------------------------

    /// <summary>
    /// 登陆成功
    /// </summary>
    public void OnLoginSouccess()
    {
        StartCoroutine(OpenUIMain());
    }

    private IEnumerator OpenUIMain()
    {
        yield return new WaitForSeconds(1f);
        this.Hide();
    }

    #endregion ------------------------------------------------------------------

    #region override ------------------------------------------------------------

    public override void Init()
    {
        UIEventListener.Get(WXButton).onClick = OnWXBtnClick;
        UIEventListener.Get(RegisterButton).onClick = OnRegisterClick;
        UIEventListener.Get(GameQuitBtn).onClick = OnGameQuitBtnClick;


        string userName = MemoryData.LoginDataMgr.LoginData.UserName;// PlayerPrefsTools.GetString(MKey.LS_AUTO_USERNAME);
        string userPwd = MemoryData.LoginDataMgr.LoginData.PassWord;// PlayerPrefsTools.GetString(MKey.LS_AUTO_PWD);
        if (!string.IsNullOrEmpty(userName) && !string.IsNullOrEmpty(userPwd))
        {
            this.InputAccount.value = userName;
            this.InputPassword.value = userPwd;
        }

        // ip赋值
        LoginEnterNetWork.Instance.GatewayServerIp = "47.93.3.120";

        // 端口赋值
        LoginEnterNetWork.Instance.GatewayServerPort = 6050;
    }

    public override void OnHide()
    {

    }

    public override void OnShow()
    {
        //主动断开连接
        LoginEnterNetWork.Instance.Disconnect();
        MemoryData.Reset();
    }

    #endregion ------------------------------------------------------------
}
