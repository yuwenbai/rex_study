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
    public class UIMainPlayInfo : UIMainBase
    {
        public UILabel PlayName;
        public UILabel Ticket;
        public UITexture HeadTex;
        public GameObject ButtonGoBack;
        public GameObject ButtonHead;
        public GameObject ButtonRoomCard;
        /// <summary>
        /// 在线人数
        /// </summary>
        public UILabel LabelOnlineUsers;

        public System.Action OnGoBackClick;
        public System.Action OnHeadClick;

        private MjRoom mjHall;

        private void Awake()
        {
            UIEventListener.Get(ButtonHead).onClick = OnButtonHeadClick;
            UIEventListener.Get(ButtonGoBack).onClick = OnButtonGoBackClick;
            UIEventListener.Get(ButtonRoomCard).onClick = OnButtonRoomCardClick;
        }
        
        /// <summary>
        /// 返回按钮点击事件 默认返回登录
        /// </summary>
        public void SetData(System.Action goBackClick, System.Action headClick , MjRoom mjHall)
        {
            this.OnGoBackClick = goBackClick;
            this.OnHeadClick = headClick;
            this.mjHall = mjHall;
        }

        public override void RefreshUI(UIMain.EnumUIMainState state)
        {
            //string pName = CommonTools.Tools_NameRegex(MemoryData.PlayerData.MyPlayerModel.PlayerDataBase.Name);
            PlayName.text = MemoryData.PlayerData.MyPlayerModel.PlayerDataBase.Name;

            Ticket.text = MemoryData.PlayerData.MyPlayerModel.PlayerDataBase.TotalTicket.ToString();

            DebugPro.Log(DebugPro.EnumLog.HeadUrl, "UIMainPlayInfo__RefreshUI", MemoryData.PlayerData.MyPlayerModel.PlayerDataBase.HeadURL);
            DownHeadTexture.Instance.WeChat_HeadTextureGet(MemoryData.PlayerData.MyPlayerModel.PlayerDataBase.HeadURL , HeadTexCallBack);

            this.LabelOnlineUsers.gameObject.SetActive(false);
            if (state == UIMain.EnumUIMainState.LinkedMjHall || state == UIMain.EnumUIMainState.NotLinkMjHall)
            {
                if (mjHall != null)
                {
                    if (mjHall.OnlineNum >= 5)
                    {
                        this.LabelOnlineUsers.text = "在线人数: " + mjHall.OnlineNum;
                        this.LabelOnlineUsers.gameObject.SetActive(true);
                    }
                }
            }
        }

        /// <summary>
        /// 头像回调
        /// </summary>
        void HeadTexCallBack(Texture2D HeadTexture, string headName)
        {
            HeadTex.mainTexture = HeadTexture;
        }

        #region ButtonEvent ------------------------------------------------

        /// <summary>
        /// 点击桌卡后面的加号按钮
        /// </summary>
        private void OnButtonRoomCardClick(GameObject go)
        {
            if (MemoryData.PlayerData.MyPlayerModel.PlayerDataBase.OwnerRoomID != 0)
            {
                // 馆主
                this.ui.LoadUIMain("UIRoomCardMaster");
            }
            else
            {
                //非馆主
                this.ui.LoadUIMain("UIRoomCard");
            }
        }

        /// <summary>
        /// 点击头像按钮
        /// </summary>
        private void OnButtonHeadClick(GameObject go)
        {
            if (OnHeadClick == null)
            {
                this.ui.LoadUIMain("UIUserInfo");
            }
            else
            {
                this.OnHeadClick();
            }                
        }
        
        /// <summary>
        /// 返回按钮点击
        /// </summary>
        public void OnButtonGoBackClick(GameObject go)
        {
            if (OnGoBackClick == null)
            {
                this.ui.LoadPop(WindowUIType.SystemPopupWindow, "提示", "是否要更换当前账号", 
                    new string[] { "取消", "确定" }, OnExitLoginTipButtonClick);
            }
                
            else
                this.OnGoBackClick();
        }
        
        /// <summary>
        /// 退出登录的按钮点击事件
        /// </summary>
        private void OnExitLoginTipButtonClick(int btnIndex)
        {
            if(btnIndex == 1)
            {
                _R.ui.ClearAll();
                EventDispatcher.FireSysEvent(GEnum.NamedEvent.SysUI_OpenLogin);
                //MemoryData.GameStateData.SetCurrUISign(SysGameStateData.EnumUISign.Login);
            }
        }

        #endregion ------------------------------------------------

        #region 控制动画运动 --------------------------------------------------------

        /// <summary>
        /// 登录界面的按钮做tween动画
        /// </summary>
        public void HeadBtnTweenBegin(bool isTween)
        {
            if (isTween)
            {
                ButtonHead.transform.localPosition = new Vector3(70.0f, 70.0f, 0.0f);
                Vector3 endV3 = new Vector3(70.0f, -50.0f, 0.0f);
                TweenPosition tp = TweenPosition.Begin(ButtonHead, 0.3f, endV3);

                EventDelegate.Add(tp.onFinished, () =>
                {
                    HeadBtnFinished();
                });

                tp.PlayForward();
            }
            else
            {
                HeadBtnFinished();
            }            
        }

        void HeadBtnFinished()
        {
            QLoger.LOG(" 大厅界面头像组件 -- 动画播放完毕 ");
        }

        #endregion ------------------------------------------------------------------
    }
}