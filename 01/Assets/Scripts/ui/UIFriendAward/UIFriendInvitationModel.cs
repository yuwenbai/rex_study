/**
 * @Author lyb
 *  加好友有礼
 *
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace projectQ
{
    public class UIFriendInvitationModel : UIModelBase
    {
        public UIFriendInvitation UI
        {
            get{ return this._ui as UIFriendInvitation; }
        }

        #region override------------------------------------------------------------------

        protected override GEnum.NamedEvent[] FocusNetWorkData()
        {
            return new GEnum.NamedEvent[] 
            {
                GEnum.NamedEvent.SysUI_FriendInvite_PickUpAward_Get,
            };
        }

        protected override void OnNetWorkDataCallBack(GEnum.NamedEvent msgEnum, object[] data)
        {
            switch (msgEnum)
            {
                case GEnum.NamedEvent.SysUI_FriendInvite_PickUpAward_Get:

                    int isSucc = (int)data[0];
                    if (isSucc == 0)
                    {
                        FriendInvitationInit();
                    }
                    else
                    {
                        this.UI.LoadPop(WindowUIType.SystemPopupWindow, "邀请好友有礼", "验证邀请码失败，请检查后重试",
                            new string[] { "确定" }, (index) => { });
                    }

                    break;
            }
        }

        #endregion------------------------------------------------------------------------

        #region 初始化--------------------------------------------------------------------

        /// <summary>
        /// 被邀请好友列表初始化
        /// </summary>
        public void FriendInvitationInit()
        {
            FriendInviteListData inviteData = MemoryData.FriendInviteData.FInviteListData;
            UI.FriendNumLab.text = inviteData.InviteCount.ToString();
            UI.RoomCardNumLab.text = inviteData.RoomCardCount.ToString();


            List<FriendInviteData> InviteList = inviteData.InviteListData;
            if (InviteList.Count <= 0)
            {
                UI.NoFriendLab.SetActive(true);
            }
            else
            {
                UI.NoFriendLab.SetActive(false);
            }

            UITools.CreateChild<FriendInviteData>(UI.GridObj.transform, null, InviteList, (go, invitationData) =>
            {
                UIFriendInvitationList invitationList = go.GetComponent<UIFriendInvitationList>();
                
                invitationList.FriendInvitationListInit(invitationData);
            });
            UI.GridObj.Reposition();
        }

        #endregion------------------------------------------------------------------------
    }
}
