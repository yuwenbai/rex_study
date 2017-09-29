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
    public class UIMainRight : UIMainBase
    {
        [System.Serializable]
        public class ButtonData
        {
            public string UIText;
            public string UIName;
            public UIMain.EnumUIMainState[] UIState;
            public GEnum.FunctionIconEnum FunctionId = GEnum.FunctionIconEnum.None;

            public bool Check(UIMain.EnumUIMainState state)
            {
                if (this.UIState != null)
                {
                    for (int i = 0; i < this.UIState.Length; ++i)
                    {
                        if (this.UIState[i] == state) return true;
                    }
                }
                return false;
            }
        }
                
        /// <summary>
        /// 设置按钮
        /// </summary>
        public GameObject ButtonOption;
        /// <summary>
        /// 客服按钮
        /// </summary>
        public GameObject ButtonService;
        /// <summary>
        /// 幸运转盘按钮
        /// </summary>
        public GameObject ButtonLottery;

        /// <summary>
        /// 竖排创建父obj
        /// </summary>
        public UIGrid BtnDataGrid;
        /// <summary>
        /// 竖排创建按钮索引
        /// </summary>
        public List<ButtonData> ButtonDataList;

        /// <summary>
        /// 横排创建父obj
        /// </summary>
        public UIGrid BtnDataCheckGrid;
        /// <summary>
        /// 横排创建按钮索引
        /// </summary>
        public List<ButtonData> BtnDataCheckList;

        private Dictionary<GameObject, ButtonData> ButtonMap = new Dictionary<GameObject, ButtonData>();

        private System.Action<ButtonData> OnCallback;
        
        private void Awake()
        {
            FunctionBtnCreat(BtnDataGrid.transform, ButtonDataList);
            FunctionBtnCreat(BtnDataCheckGrid.transform, BtnDataCheckList);
            
            //设置按钮
            UIEventListener.Get(ButtonOption).onClick = OnButtonOptionClick;
            //客服按钮
            UIEventListener.Get(ButtonService).onClick = OnButtonServiceClick;
            //转盘按钮
            UIEventListener.Get(ButtonLottery).onClick = OnButtonLotteryClick;
        }

        #region 创建功能按钮 -----------------------------------------------

        /// <summary>
        /// 创建功能按钮
        /// </summary>
        void FunctionBtnCreat(Transform btnGrid, List<ButtonData> btnDataList)
        {
            UITools.CreateChild<ButtonData>(btnGrid, null, btnDataList, (go, data) =>
            {
                UIMainRightList rBtnList = go.GetComponent<UIMainRightList>();
                rBtnList.HallRightListInit(data);
                rBtnList.OnClickCallBack = OnRightBtnClickCallBack;
                ButtonMap.Add(go, data);
            });
        }

        #endregion ---------------------------------------------------------
        
        public void SetData(System.Action<ButtonData> func)
        {
            OnCallback = func;
        }

        public override void RefreshUI(UIMain.EnumUIMainState state)
        {
            if (state == UIMain.EnumUIMainState.MasterCheck)
            {
                ButtonOption.SetActive(false);
                ButtonService.SetActive(false);
                ButtonLottery.SetActive(false);
            }
            else
            {
                ButtonOption.SetActive(true);
                ButtonService.SetActive(true);
                ButtonLottery.SetActive(true);
            }

            foreach (var item in ButtonMap)
            {
                if(item.Value.Check(state))
                {
                    item.Key.SetActive(true);
                }
                else
                {
                    item.Key.SetActive(false);
                }
            }
            BtnDataGrid.Reposition();
            BtnDataCheckGrid.Reposition();
        }        

        #region ButtonEvent ------------------------------------------------

        /// <summary>
        /// 设置按钮点击调用
        /// </summary>
        private void OnButtonOptionClick(GameObject go)
        {
            this.ui.LoadUIMain("UIOption");
        }

        /// <summary>
        /// 客服按钮点击调用
        /// </summary>
        private void OnButtonServiceClick(GameObject go)
        {
            WebSDKParams param = new WebSDKParams("WEB_OPEN_CS_SERVICE");
            param.InsertUrlParams(MemoryData.UserID, MemoryData.PlayerData.MyPlayerModel.PlayerDataBase.Name);
            SDKManager.Instance.SDKFunction("WEB_OPEN_CS_SERVICE",param);
        }

        /// <summary>
        /// 转盘按钮点击调用
        /// </summary>
        private void OnButtonLotteryClick(GameObject go)
        {
            //WindowUIManager.Instance.CreateTip("这是一个转盘。。。还没有开放");
            this.ui.LoadUIMain("UIActivityLottery");
        }

        #endregion ---------------------------------------------------------

        #region Event ------------------------------------------------------

        /// <summary>
        /// 按钮点击事件回调
        /// </summary>
        void OnRightBtnClickCallBack(string btnName)
        {
            if (OnCallback == null)
            {
                if (btnName == "MyMjHall")
                {
                    if (MemoryData.PlayerData.MyPlayerModel.PlayerDataBase.OwnerRoomID > 0)
                    {
                        // 馆主 进行馆主的操作
                        this.ui.ChangeState(UIMain.EnumUIMainState.Master);
                    }
                    else
                    {
                        this.ui.ChangeState(UIMain.EnumUIMainState.LinkedMjHall);
                    }
                }
                else if (btnName == "BindHall")
                {
                    this.ui.LoadUIMain("UIParlorInfo", ui.CurrMjHalll, true);
                }
                else if (btnName == "FriendConnect")
                {
                    //邀请好友关联
                    WXShareParams shareParams = new WXShareParams("WECHAT_SHARE_INVITE_FRIEND_GO_MYMUSEUM");
                    shareParams.InsertUrlParams(new object[] {(int)WXOpenParaEnum.SHARE_INVITE_FRIEND_GO_MYMUSEUM, ui.CurrMjHalll.RoomID.ToString(),
                        "", "", "", "", ""});
                    SDKManager.Instance.SDKFunction("WECHAT_SHARE_INVITE_FRIEND_GO_MYMUSEUM", shareParams);

                }
                else if (btnName == "Recommend")
                {
                    //推荐好友开馆
                    WXShareParams shareParams = new WXShareParams("WEB_OPEN_FRIEND_CURATOR_PROGRESS");
                    shareParams.InsertUrlParams(new object[] { 0,
                        MemoryData.PlayerData.MyPlayerModel.PlayerDataBase.InviteCode,
                        MemoryData.PlayerData.MyPlayerModel.PlayerDataBase.MyAgentID});
                    SDKManager.Instance.SDKFunction("WEB_OPEN_FRIEND_CURATOR_PROGRESS", shareParams);
                }
                else if (btnName == "MasterApply")
                {
                    if (MemoryData.PlayerData.MyPlayerModel.PlayerDataBase.OwnerRoomID > 0)
                    {
                        // 馆主 提示已经是馆长
                        WindowUIManager.Instance.CreateOrAddWindow(WindowUIType.SystemPopupWindow,
                            "提示", "您已经是馆长了", new string[] { "确定" }, delegate (int index) { });
                    }
                    else
                    {
                        //非馆长
                        WebSDKParams shareParams = new WebSDKParams("WEB_OPEN_MY_CURATOR_PROGRESS");
                        shareParams.InsertUrlParams(new object[] { MemoryData.UserID,
                            MemoryData.PlayerData.MyPlayerModel.PlayerDataBase.InviteCode,
                            MemoryData.PlayerData.MyPlayerModel.PlayerDataBase.AgentID});
                        SDKManager.Instance.SDKFunction("WEB_OPEN_MY_CURATOR_PROGRESS", shareParams);
                    }
                }
                else if (btnName == "AgentRequest")
                {
                    WebSDKParams param = new WebSDKParams("WEB_OPEN_AGENT_1");
                    SDKManager.Instance.SDKFunction("WEB_OPEN_AGENT_1", param);
                }
                else
                {
                    this.ui.LoadUIMain(btnName);
                }
            }
            else
            {
                //OnCallback(btnName);
            }
        }
        
        #endregion ---------------------------------------------------------
    }
}