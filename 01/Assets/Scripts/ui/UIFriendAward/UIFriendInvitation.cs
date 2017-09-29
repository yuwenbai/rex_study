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
    public class UIFriendInvitation : UIViewBase
    {
        public UIFriendInvitationModel Model
        {
            get { return _model as UIFriendInvitationModel; }
        }
        
        /// <summary>
        /// 关闭按钮
        /// </summary>
        public GameObject CloseButton;
        /// <summary>
        /// 继续邀请按钮
        /// </summary>
        public GameObject InvitationBtn;
        /// <summary>
        /// 累计邀请好友文字
        /// </summary>
        public UILabel FriendNumLab;
        /// <summary>
        /// 累计领取桌卡文字
        /// </summary>
        public UILabel RoomCardNumLab;
        /// <summary>
        /// 创建已邀请好友列表
        /// </summary>
        public UIGrid GridObj;
        /// <summary>
        /// 当前无邀请好友Lab
        /// </summary>
        public GameObject NoFriendLab;

        #region override ------------------------------------------------------------

        public override void Init()
        {
            //ShowMask();
            UIEventListener.Get(CloseButton).onClick = OnCloseBtnClick;
            UIEventListener.Get(InvitationBtn).onClick = OnInvitationBtnClick;
        }

        public override void OnShow()
        {
            Model.FriendInvitationInit();
        }

        public override void OnHide(){}

        public override void GoBack()
        {
            this.OnCloseBtnClick(null);
        }
        #endregion-------------------------------------------------------------------

        #region 按钮点击回调方法-----------------------------------------------------

        /// <summary>
        /// 点击关闭按钮
        /// </summary>
        void OnCloseBtnClick(GameObject button)
        {
            this.LoadUIMain("UIFriendAward");
            this.Close();
        }

        /// <summary>
        /// 点击立即邀请按钮
        /// </summary>
        void OnInvitationBtnClick(GameObject button)
        {
            WXShareParams shareParams = new WXShareParams("WECHAT_SHARE_INVITE_FRIEND_TO_GET_REWARD");
            shareParams.InsertUrlParams(new object[] {
                MemoryData.PlayerData.MyPlayerModel.PlayerDataBase.InviteCode,
                MemoryData.PlayerData.MyPlayerModel.PlayerDataBase.MyAgentID});
            SDKManager.Instance.SDKFunction("WECHAT_SHARE_INVITE_FRIEND_TO_GET_REWARD", shareParams);
        }
        
        #endregion-------------------------------------------------------------------
    }
}