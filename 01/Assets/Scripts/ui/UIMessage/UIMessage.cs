/**
 * @Author lyb
 *  消息模块
 *
 */

using UnityEngine;

namespace projectQ
{
    public class UIMessage : UIViewBase
    {
        public UIMessageModel Model
        {
            get { return _model as UIMessageModel; }
        }

        /// <summary>
        /// 消息按钮
        /// </summary>
        public GameObject MessageBtn;
        /// <summary>
        /// 好友消息按钮
        /// </summary>
        public GameObject MessageFriendBtn;
        /// <summary>
        /// 战绩按钮
        /// </summary>
        public GameObject BattleInfoBtn;
        /// <summary>
        /// 关闭面板按钮
        /// </summary>
        public GameObject CloseBtn;
        /// <summary>
        /// 消息面板
        /// </summary>
        public GameObject MessagePanelObj;
        /// <summary>
        /// 战绩面板
        /// </summary>
        public GameObject BattleInfoPanelObj;
        /// <summary>
        /// 没有邮件或者是战绩消息的时候显示无
        /// </summary>
        public GameObject NoMessagePanelObj;
        /// <summary>
        /// 创建消息列表的Table
        /// </summary>
        public UITable TableMessageObj;
        /// <summary>
        /// 创建战绩列表的Table
        /// </summary>
        public UITable TableBattleInfoObj;
        /// <summary>
        /// 滑动控件 - 消息的
        /// </summary>
        public UIScrollView ScrollViewMessageObj;
        /// <summary>
        /// 滑动控件 - 战役的
        /// </summary>
        public UIScrollView ScrollViewBattleObj;

        #region override-------------------------------------------------

        public override void Init()
        {
            UIEventListener.Get(MessageBtn).onClick = OnMessageSystemBtnClick;
            UIEventListener.Get(MessageFriendBtn).onClick = OnMessageFriendBtnClick;
            UIEventListener.Get(BattleInfoBtn).onClick = OnBattleInfoBtnClick;
            UIEventListener.Get(CloseBtn).onClick = OnCloseBtnClick;
        }

        public override void OnHide()
        {
        }

        public override void OnShow()
        {
            MessageBtn.GetComponent<UIButton>().gameObject.SendMessage("OnClick", SendMessageOptions.DontRequireReceiver);

            MessageBtn.GetComponent<UIDefinedButton>().SoundSelect = GEnum.SoundEnum.btn_select3;
        }

        protected override void OnShowAndAnimationEnd()
        {
            C2SMessageDataSend();
        }

        void C2SMessageDataSend()
        {
            LoadSendLoading();

            EventDispatcher.FireSysEvent(GEnum.NamedEvent.EGetGameData, GEnum.NamedGameData.EC2SMessageList);
        }

        #endregion-------------------------------------------------------

        /// <summary>
        /// 系统消息按钮
        /// </summary>
        private void OnMessageSystemBtnClick(GameObject go)
        {
            NoMessagePanelObj.SetActive(false);
            MessagePanelObj.SetActive(true);
            BattleInfoPanelObj.SetActive(false);

            ScrollViewMessageObj.ResetPosition();
            Model.MailSystemBtnClick();
        }

        /// <summary>
        /// 好友消息按钮
        /// </summary>
        private void OnMessageFriendBtnClick(GameObject go)
        {
            NoMessagePanelObj.SetActive(false);
            MessagePanelObj.SetActive(true);
            BattleInfoPanelObj.SetActive(false);

            ScrollViewMessageObj.ResetPosition();
            Model.MailFriendBtnClick();
        }

        /// <summary>
        /// 战绩按钮
        /// </summary>
        private void OnBattleInfoBtnClick(GameObject go)
        {
            NoMessagePanelObj.SetActive(false);
            BattleInfoPanelObj.SetActive(true);
            MessagePanelObj.SetActive(false);

            ScrollViewBattleObj.ResetPosition();
            Model.BtnBattleInfoClick();

            LoadSendLoading();
            EventDispatcher.FireSysEvent(GEnum.NamedEvent.EGetGameData, GEnum.NamedGameData.EC2SMessageBattleList);
        }

        /// <summary>
        /// 返回按钮
        /// </summary>
        private void OnCloseBtnClick(GameObject go)
        {
            this.Close();
        }
    }
}