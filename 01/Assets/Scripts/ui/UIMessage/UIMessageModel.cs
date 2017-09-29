/**
* @Author lyb
*
*
*/

using System.Collections.Generic;

namespace projectQ
{
    public class UIMessageModel : UIModelBase
    {
        public UIMessage UI
        {
            get { return _ui as UIMessage; }
        }

        /// <summary>
        /// 系统邮件相关
        /// </summary>
        private List<MessageMail> mailSystemList = new List<MessageMail>();
        /// <summary>
        /// 好友邮件相关
        /// </summary>
        private List<MessageMail> mailFriendList = new List<MessageMail>();

        #region override----------------------------------------------------------------------

        protected override GEnum.NamedEvent[] FocusNetWorkData()
        {
            return new GEnum.NamedEvent[]
            {
                GEnum.NamedEvent.SysUI_Message_Succ,
                GEnum.NamedEvent.SysUI_MessageBattle_Succ,
                GEnum.NamedEvent.ERCCloseingAndRelushUI,
            };
        }

        protected override void OnNetWorkDataCallBack(GEnum.NamedEvent msgEnum, object[] data)
        {
            switch (msgEnum)
            {
                case GEnum.NamedEvent.SysUI_Message_Succ:
                    UI.StopSendLoading();
                    MailMessageListPart();
                    MailSystemBtnClick();
                    break;
                case GEnum.NamedEvent.SysUI_MessageBattle_Succ:
                    UI.StopSendLoading();
                    BtnBattleInfoClick();
                    break;
                case GEnum.NamedEvent.ERCCloseingAndRelushUI:
                    UI.StopSendLoading();
                    break;
            }
        }

        #endregion-----------------------------------------------------------------------------

        #region lyb 按钮相关的处理方法---------------------------------------------------------

        /// <summary>
        /// 点击系统消息按钮的处理方法
        /// </summary>
        public void MailSystemBtnClick()
        {
            MailMessageInit(mailSystemList);
        }

        /// <summary>
        /// 点击好友消息按钮的处理方法
        /// </summary>
        public void MailFriendBtnClick()
        {
            MailMessageInit(mailFriendList);
        }

        /// <summary>
        /// 创建消息列表
        /// </summary>
        void MailMessageInit(List<MessageMail> mailList)
        {
            UI.NoMessagePanelObj.SetActive(false);
            if (mailList.Count <= 0)
            {
                UI.NoMessagePanelObj.SetActive(true);
            }

            UITools.CreateChild<MessageMail>(UI.TableMessageObj.transform, null, mailList, (go, messageData) =>
            {
                go.SetActive(true);

                UIMessageList mList = go.GetComponent<UIMessageList>();
                mList.MessageInit(messageData);

                if (mList.TweerObj.tweenFactor == 1)
                {
                    mList.TweerObj.tweenFactor = 0;
                    mList.TweerObj.gameObject.SetActive(false);

                    //当前面板是打开的 -- 关掉
                    //mList.TweerPlayObj.gameObject.SendMessage("OnClick", SendMessageOptions.DontRequireReceiver);
                }
            });
            UI.TableMessageObj.Reposition();
        }

        #endregion-----------------------------------------------------------------------------

        #region lyb 根据本地列表区分系统邮件和好友邮件-----------------------------------------

        public void MailMessageListPart()
        {
            mailSystemList = new List<MessageMail>();
            mailFriendList = new List<MessageMail>();

            foreach (MessageMail mailData in MemoryData.MessageData.MailList)
            {
                if (mailData.type == MessageMail.EnumMessageType.System)
                {
                    //系统邮件
                    mailSystemList.Add(mailData);
                }
                else
                {
                    //好友邮件
                    mailFriendList.Add(mailData);
                }
            }
        }

        #endregion-----------------------------------------------------------------------------

        #region lyb 点击战绩按钮相关处理方法---------------------------------------------------

        /// <summary>
        /// 点击战绩按钮的处理方法
        /// </summary>
        public void BtnBattleInfoClick()
        {
            UI.NoMessagePanelObj.SetActive(false);
            if (MemoryData.ResultData.ResultData.Count <= 0)
            {
                UI.NoMessagePanelObj.SetActive(true);
            }

            UITools.CreateChild<GameResult>(UI.TableBattleInfoObj.transform, null, MemoryData.ResultData.ResultData, (go, messageData) =>
            {
                UIBattleInfoList bList = go.GetComponent<UIBattleInfoList>();
                bList.OnMessageBattleInfoClickCallBack = MessageBattleInfoBtnCallBack;
                bList.ButtionInfoInit(messageData);

                if (bList.TweerObj.tweenFactor == 1)
                {
                    bList.TweerObj.tweenFactor = 0;
                    bList.TweerObj.gameObject.SetActive(false);
                }
            });
            UI.TableBattleInfoObj.Reposition();
        }

        /// <summary>
        /// 点击详情按钮回调方法
        /// </summary>
        void MessageBattleInfoBtnCallBack(int deskId)
        {
            UI.LoadUIMain("UIMahjongResult", deskId, 1);
        }

        #endregion-----------------------------------------------------------------------------
    }
}