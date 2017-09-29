/**
* @Author YQC
* 牌桌准备
*
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;

namespace projectQ
{
    public class UIPrepareGame : UIViewBase
    {
        public enum UIState
        {
            None        = 0,
            NotOpenDesk = 1,    //未开桌 桌主才会有的
            NotPrepare  = 2,    //未准备界面
            Prepare     = 3,    //已准备界面
            GameIn      = 4,    //游戏开始
        }

        public UIPrepareGameModel Model
        {
            get { return this._model as UIPrepareGameModel; }
        }
        [Tooltip("Root")]
        public GameObject Root;

        public UIPrepareGameNotOpenDeskMgr NotOpenDeskMgr;
        public UIPrepareGameOpenDeskMgr OpenDeskMgr;
        public GameObject DeskTopFunctionRoot;

        [HideInInspector]
        public UIDesktopFunction DeskTopFunctionScript;
        private GameObject DeskTopAllButton;

        //按钮==========================
        [Tooltip("返回按钮")]
        public GameObject ButtonGoBack;
        [Tooltip("规则按钮")]
        public GameObject ButtonRule;

        //头像==========================
        //左头像
        [HideInInspector]
        public UIPrepareGamePlayerItem LeftHead = new UIPrepareGamePlayerItem();

        //上头像
        [HideInInspector]
        public UIPrepareGamePlayerItem UpHead = new UIPrepareGamePlayerItem();

        //右头像
        [HideInInspector]
        public UIPrepareGamePlayerItem RightHead = new UIPrepareGamePlayerItem();

        //自己头像
        [HideInInspector]
        public UIPrepareGamePlayerItem MyHead = new UIPrepareGamePlayerItem();

        public UIState CurrState = UIState.None;

        public GameObject Mask;
 		/// <summary>
        /// 是否弹出过绑定麻将馆
        /// </summary>
        bool isPopBindMjRoom = false;

        #region API
        /// <summary>
        /// 开桌
        /// </summary>
        public void OpenDesk()
        {
            MemoryData.GameStateData.IsMahjongGameIn = true;
            this.RefreshUI();
        }

        [ContextMenu("手动刷新下")]
        public void RefreshUI()
        {
            OpenDeskMgr.gameObject.SetActive(false);
            NotOpenDeskMgr.gameObject.SetActive(false);
            Model.RefreshData();

            CurrState = SelectState();

            Debug.Log("桌子状态变更"+CurrState);

            switch (CurrState)
            {
                case UIState.None:
                    RefreshUI();
                    break;

                case UIState.NotOpenDesk:
                    DeskTopFunctionScript.RefreshData();
                    this.RefreshUI_NotOpenDesk();
                    break;

                case UIState.NotPrepare:
 					if(!isPopBindMjRoom)
                    {
                        isPopBindMjRoom = true;
                        IsPopUpParlorInfoWindow();
                    }
                    this.LoadUIMain("UICheatInfo");
                    DeskTopFunctionScript.RefreshData();
                    this.RefreshUI_NotOpenDesk();
                    break;

                case UIState.Prepare:
                    this.LoadUIMain("UICheatInfo");
                    DeskTopFunctionScript.RefreshData();
                    this.RefreshUI_OpenDesk();
                    break;
                case UIState.GameIn:
                      this.LoadUIMain("UICheatInfo");
                    if(MemoryData.GameStateData.CurrUISign != SysGameStateData.EnumUISign.GameIn)
                        EventDispatcher.FireSysEvent(GEnum.NamedEvent.SysData_CheatInfo_Hide);
                    this.RefreshUI_GameIn();
                    break;
            }
            if (!_R.ui.IsShowUI(GameConst.path_MahjongUIMain))
            {
                _R.ui.OpenUI(GameConst.path_MahjongUIMain);
                if (FakeReplayManager.Instance.ReplayState)
                {
                    _R.ui.OpenUI("UIReplayCtrl");
                }
            }
            else
            {
                if(_R.ui.GetDepth(GameConst.path_MahjongUIMain) <= this.GetDepth())
                {
                    QLoger.LOG("游戏UI层级" + _R.ui.GetDepth(GameConst.path_MahjongUIMain) + " 准备UI层级" + this.GetDepth());

                    _R.ui.AddDepth(GameConst.path_MahjongUIMain);

                    QLoger.LOG("增加层级到" + _R.ui.GetDepth(GameConst.path_MahjongUIMain));
                }
            }

            RefreshHead(Model.data.LeftPlayer, LeftHead);
            RefreshHead(Model.data.UpPlayer, UpHead);
            RefreshHead(Model.data.RightPlayer, RightHead);
            RefreshHead(Model.data.MyPlayer, MyHead);
        }

        //取得状态
        private UIState SelectState()
        {
            if (MemoryData.GameStateData.IsMahjongGameIn)
            {
                return UIState.GameIn;
            }
            if(Model.data.DeskInfo.deskID != 0 && Model.data.MyPlayer != null && Model.data.MyPlayer.hasReady)
            {
                return UIState.Prepare;
            }
            if(Model.data.DeskInfo.deskID != 0)
            {
                return UIState.NotPrepare;
            }
            return UIState.NotOpenDesk;
        }

        /// <summary>
        /// 刷新UI 未开桌
        /// </summary>
        public void RefreshUI_NotOpenDesk()
        {
            MemoryData.GameStateData.SetCurrUISign(SysGameStateData.EnumUISign.PrepareGame_NotOpenDesk);
            //如果我是桌主
            if (Model.data.DeskInfo.ownerUserID == MemoryData.UserID)
            {
                if (Model.data.MyPlayer != null && !Model.data.MyPlayer.hasReady)
                {
                    Model.OnPrepareGame();
                }
            }
            else
            {
                NotOpenDeskMgr.gameObject.SetActive(true);
                NotOpenDeskMgr.RefreshUI();
                HideAllButton(false);
            }
            if (FakeReplayManager.Instance.ReplayState)
            {
                NotOpenDeskMgr.gameObject.SetActive(false);
                HideAllButton(false);
            }
            UpRuleDepth();
        }

        /// <summary>
        /// 开桌
        /// </summary>
        public void RefreshUI_OpenDesk()
        {
            if (!FakeReplayManager.Instance.ReplayState)
            {
                MemoryData.GameStateData.SetCurrUISign(SysGameStateData.EnumUISign.PrepareGame);
                OpenDeskMgr.gameObject.SetActive(true);
                OpenDeskMgr.RefreshUI();
                HideAllButton(true);
            }
        }

        /// <summary>
        /// 游戏进入
        /// </summary>
        public void RefreshUI_GameIn()
        {
            MemoryData.GameStateData.SetCurrUISign(SysGameStateData.EnumUISign.GameIn);
            DeskTopAllButton.SetActive(true);
            if (!FakeReplayManager.Instance.ReplayState)
            {
                this.ReverAllButton();
            }
            Root.SetActive(false);
        }
        #endregion

        #region 准备UI 刷新
        public void RefreshUI_Prepare(bool isClear)
        {
            Model.RefreshData();
            if(isClear)
            {
            }
            
            RefreshHead(Model.data.LeftPlayer, LeftHead);
            RefreshHead(Model.data.UpPlayer, UpHead);
            RefreshHead(Model.data.RightPlayer, RightHead);
            RefreshHead(Model.data.MyPlayer, MyHead);
        }
        #endregion

        #region 规则
        private void UpRuleDepth()
        {
            this.NotOpenDeskMgr.GetComponent<UIPanel>().depth = GetDepth() + 4;
            this.NotOpenDeskMgr.PlayRuleScript.ScrollView.GetComponent<UIPanel>().depth = GetDepth() + 5;
        }
        #endregion 

        #region 头像
        private void RefreshHead(UIPrepareGameData.PlayerData data, UIPrepareGamePlayerItem ui)
        {
            ui.Root.SetActive(data != null);
            if (data == null)
            {
                ui.Prepare(false);
            }
            else
            {
                ui.RefreshUI(data.userID, data.hasReady);
            }
        }
        #endregion

        #region Event

        /// <summary>
        /// 规则按钮
        /// </summary>
        private void OnButtonRuleClick(GameObject go)
        {
            if(this.CurrState == UIState.Prepare && !this.OpenDeskMgr.IsFriendListOpen)
            {
                NotOpenDeskMgr.Show();
                StartCoroutine(UITools.ResetScrollView(NotOpenDeskMgr.PlayRuleScript.ScrollView));
            }
        }


        //返回按钮
        private void OnButtonGoBackClick(GameObject go)
        {
            string content = null;
            if (Model.data != null && Model.data.MyPlayer != null && Model.data.MyPlayer.seatID == 1)
            {
                content = "是否解散牌桌?\n" + "[1A72EAFF]" + "(开局前退出将解散房间,不消耗桌卡)";
            }
            else
            {
                content = "是否退出牌桌?";
            }

            this.LoadPop(WindowUIType.SystemPopupWindow, "提示", content, new string[] { "取消", "确认" },
                (index) => {
                    if(index == 1)
                    {
                        if(this.CurrState == UIState.NotOpenDesk)
                        {
                            EventDispatcher.FireSysEvent(GEnum.NamedEvent.OpenUI_UICreateRoom);
                            //this.LoadUIMain("UICreateRoom");
                            this.Hide();
                        }
                        else
                        {
                            Model.OnQuitDesk();
                        }
                    }
                });
        }


        private void OnMaskClick(GameObject go)
        {
            EventDispatcher.FireSysEvent(GEnum.NamedEvent.SysUI_FriendList_OpenClose_Req, false);
        }
        #endregion

        #region override
        public override void Init()
        {
#if __DEBUG && !__DEBUG_CLOSE_ZUOBI
            transform.FindChild("Root/Test").gameObject.SetActive(true);
#else
            transform.FindChild("Root/Test").gameObject.SetActive(false);
#endif
            //返回按钮
            UIEventListener.Get(ButtonGoBack.gameObject).onClick = OnButtonGoBackClick;
            UIEventListener.Get(ButtonRule.gameObject).onClick = OnButtonRuleClick;
            
            UIEventListener.Get(Mask).onClick = OnMaskClick;

            this.LoadSubUI("UIDesktopFunction");
            NotOpenDeskMgr.Init(this);



            //IsPopUpParlorInfoWindow();
            //IsPopUpParlorInfoWindow();
        }

        public override void OnHide()
        {
        }

        public override void OnShow()
        {
            Model.data = new UIPrepareGameData();
            CurrState = UIState.None;
            this.RefreshUI();
        }


        List<KeyValuePair<GameObject, bool>> GameInUIAllButtonActive = new List<KeyValuePair<GameObject, bool>>();
        GameObject ruleButton = null;
        GameObject remaindRound = null;
        GameObject settingButton = null;
        GameObject voiceButton = null;
        protected override GameObject OnSubUILoaded(string subName, GameObject go)
        {
            GameObject goo = NGUITools.AddChild(DeskTopFunctionRoot, go);
            DeskTopFunctionScript = goo.GetComponent<UIDesktopFunction>();
            DeskTopFunctionScript.Init(this);
            MyHead.Init(DeskTopFunctionScript.mahjongPositionPlayerInfo.playerInfoArray[0]);
            RightHead.Init(DeskTopFunctionScript.mahjongPositionPlayerInfo.playerInfoArray[1]);
            UpHead.Init(DeskTopFunctionScript.mahjongPositionPlayerInfo.playerInfoArray[2]);
            LeftHead.Init(DeskTopFunctionScript.mahjongPositionPlayerInfo.playerInfoArray[3]);


            DeskTopAllButton = DeskTopFunctionScript.transform.FindChild("allButton").gameObject;
            ruleButton = null;
            remaindRound = null;
            settingButton = null;
            voiceButton = null;
            GameInUIAllButtonActive.Clear();
            Transform tf = DeskTopAllButton.transform;
                for (int i = 0; i < tf.childCount; i++)
                {
                    var tempGo = tf.GetChild(i).gameObject;
                    GameInUIAllButtonActive.Add(new KeyValuePair<GameObject, bool>(tempGo, tempGo.activeSelf));

                    switch (tempGo.name)
                    {
                        case "ruleButton":
                            ruleButton = tempGo;
                            break;
                        case "remaindRound":
                            remaindRound = tempGo;
                            break;
                        case "settingButton":
                            settingButton = tempGo;
                            break;
                        case "voiceButton":
                            voiceButton = tempGo;
                            break;
                    }

                    tempGo.SetActive(false);
                }
            return goo;
        }

        public override void GoBack()
        {
            if(this.CurrState != UIState.GameIn && this.CurrState != UIState.None)
            {
                this.OnButtonGoBackClick(null);
            }
        }
        #endregion
        public bool isaaa = false;
        private void ReverAllButton()
        {
            if(!isaaa)
            {
                isaaa = true;
                for (int i=0; i < GameInUIAllButtonActive.Count;++i)
                {
                    GameInUIAllButtonActive[i].Key.SetActive(GameInUIAllButtonActive[i].Value);
                }
            }
        }
        private void HideAllButton(bool openDesk)
        {
            for (int i = 0; i < GameInUIAllButtonActive.Count; ++i)
            {
                GameInUIAllButtonActive[i].Key.SetActive(false);
            }
            if(openDesk)
            {
                ruleButton.SetActive(true);
                remaindRound.SetActive(true);
                settingButton.SetActive(true);
                voiceButton.SetActive(true);
            }
        }

        /// <summary>
        /// 是否要弹关联麻将馆界面
        /// </summary>
        private void IsPopUpParlorInfoWindow()
        {
            //if (MemoryData.GameStateData.IsWxShareInvitePlay)
            if (MemoryData.GameStateData.JoinDeskRoomId > 0)
            {
                MemoryData.GameStateData.IsWxShareInvitePlay = true;
                EventDispatcher.FireSysEvent(GEnum.NamedEvent.SysUI_ParlorWindow_Rsp, MemoryData.GameStateData.JoinDeskRoomId.ToString());
                MemoryData.GameStateData.JoinDeskRoomId = 0;
            }
        }

    }
}