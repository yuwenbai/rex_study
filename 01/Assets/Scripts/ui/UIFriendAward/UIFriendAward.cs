/**
 * @Author lyb
 *  加好友有礼
 *
 */

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace projectQ
{
    public class UIFriendAward : UIViewBase
    {
        public UIFriendAwardModel Model
        {
            get { return _model as UIFriendAwardModel; }
        }

        /// <summary>
        /// 关闭按钮
        /// </summary>
        public GameObject CloseButton;
        /// <summary>
        /// 立即邀请按钮
        /// </summary>
        public GameObject InvitationBtn;
        /// <summary>
        /// 已邀好友按钮
        /// </summary>
        public GameObject InvitationFriendBtn;
        /// <summary>
        /// 填写邀请我的人按钮
        /// </summary>
        public GameObject WriteFriendBtn;
        /// <summary>
        /// 我的邀请码
        /// </summary>
        public UILabel InvitationNum;
        /// <summary>
        /// 点击填写邀请我的人。。弹出的面板
        /// </summary>
        public UIPrompt UIPrompt;

        /// <summary>
        /// 邀请好友双发的好礼
        /// </summary>
        public UILabel StrLab01;
        /// <summary>
        /// 邀请好友有礼描述
        /// </summary>
        public UILabel StrLab02;
        /// <summary>
        /// 受邀请者描述
        /// </summary>
        public UILabel StrLab03;
        /// <summary>
        /// 被邀请的邀请人的名字
        /// </summary>
        public UILabel InviteePlayerName;


        #region override ------------------------------------------------------------

        public override void Init()
        {
            InvitationNum.text = MemoryData.PlayerData.MyPlayerModel.PlayerDataBase.InviteCode;

            string str01 = MemoryData.XmlData.XmlBuildDataText_Get(GTextsEnum.G_Text_103);
            string str02 = MemoryData.XmlData.XmlBuildDataText_Get(GTextsEnum.G_Text_104);
            string str03 = MemoryData.XmlData.XmlBuildDataText_Get(GTextsEnum.G_Text_105);
            StrLab01.text = str01;
            StrLab02.text = str02;
            StrLab03.text = str03;

            UIPrompt.gameObject.SetActive(false);

            UIEventListener.Get(CloseButton).onClick = OnCloseBtnClick;
            UIEventListener.Get(InvitationBtn).onClick = OnInvitationBtnClick;
            UIEventListener.Get(InvitationFriendBtn).onClick = OnInvitationFriendBtnClick;
            UIEventListener.Get(WriteFriendBtn).onClick = OnWriteFriendBtnClick;
        }

        public override void OnShow()
        {
            // 请求邀请好友列表
            ModelNetWorker.Instance.C2SInviteFriendListReq();

            FriendAwardInit();
        }

        public override void OnHide() { }

        #endregion-------------------------------------------------------------------

        public void FriendAwardInit()
        {
            InviteePlayerName.text = "";
            if (!string.IsNullOrEmpty(MemoryData.PlayerData.MyPlayerModel.PlayerDataBase.BeInviteeCode))
            {
                WriteFriendBtn.SetActive(false);

                InviteePlayerName.text = "邀请人\n" + MemoryData.PlayerData.MyPlayerModel.PlayerDataBase.BeInviteeName;
            }
        }

        #region 按钮点击回调方法-----------------------------------------------------

        /// <summary>
        /// 点击关闭按钮
        /// </summary>
        void OnCloseBtnClick(GameObject button)
        {
            this.Close();
        }

        /// <summary>
        /// 点击立即邀请按钮
        /// </summary>
        void OnInvitationBtnClick(GameObject button)
        {
            WXShareParams shareParams = new WXShareParams("WECHAT_SHARE_INVITE_FRIEND_TO_GET_REWARD");
            shareParams.InsertUrlParams(new object[] { InvitationNum.text,
                MemoryData.PlayerData.MyPlayerModel.PlayerDataBase.MyAgentID});
            SDKManager.Instance.SDKFunction("WECHAT_SHARE_INVITE_FRIEND_TO_GET_REWARD", shareParams);
        }

        /// <summary>
        /// 点击已邀好友按钮
        /// </summary>
        void OnInvitationFriendBtnClick(GameObject button)
        {
            this.LoadUIMain("UIFriendInvitation");
            this.Close();
        }

        /// <summary>
        /// 点击填写邀请我的人按钮
        /// </summary>
        void OnWriteFriendBtnClick(GameObject button)
        {
            UIPrompt.gameObject.SetActive(true);
            UIPrompt.UIPromptInit();
        }

        /// <summary>
        /// 长按copy一个字符串
        /// </summary>
        public void OnPressCopy()
        {
            SDKManager.Instance.PutStringToClipboard(new SDKData.ClipboardData(InvitationNum.text), OnPressCopyCallBack);
        }

        /// <summary>
        /// 粘贴回调
        /// </summary>
        void OnPressCopyCallBack(string copyValue)
        {
            if (InvitationNum.text.Equals(copyValue))
            {
                WindowUIManager.Instance.CreateTip(" 邀请码复制成功 ");
            }
            else
            {
                WindowUIManager.Instance.CreateTip(" 邀请码复制失败，请重试 ");
            }            
        }

        #endregion-------------------------------------------------------------------
    }
}