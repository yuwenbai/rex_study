using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace projectQ
{
    public class UIReplayCtrlModel : UIModelBase
    {
        public UIReplayCtrl UI
        {
            get { return this._ui as UIReplayCtrl; }
        }

        #region override------------------------------------------------------------------

        protected override GEnum.NamedEvent[] FocusNetWorkData()
        {
            return new GEnum.NamedEvent[]
            {
                GEnum.NamedEvent.SysUI_UIReplayCtrl_Play_Over,
                GEnum.NamedEvent.SysUI_UIReplayCtrl_Play_Init,
                GEnum.NamedEvent.SysUI_UIReplayCtrl_Play_Speed,
            };
        }

        protected override void OnNetWorkDataCallBack(GEnum.NamedEvent msgEnum, object[] data)
        {
            switch (msgEnum)
            {
                case GEnum.NamedEvent.SysUI_UIReplayCtrl_Play_Over:
                    {
                        UI.OnReplayOver();
                        UI.IsCanOpen = false;
                        UI.ChangeSpeedBtn.GetComponent<UIDefinedButton>().isEnabled = false;
                        UI.PauseReplayBtn.GetComponent<UIDefinedButton>().isEnabled = false;
                    }
                    break;
                case GEnum.NamedEvent.SysUI_UIReplayCtrl_Play_Init:
                    {
                        UI.IsCanOpen = true;
                        UI.PanelOpen = true;
                        UI.ChangeSpeedBtn.GetComponent<UIDefinedButton>().isEnabled = true;
                        UI.PauseReplayBtn.GetComponent<UIDefinedButton>().isEnabled = true;
                        UI.UpdateSpeedState();
                    }
                    break;
                case GEnum.NamedEvent.SysUI_UIReplayCtrl_Play_Speed:
                    {
                        UI.UpdateSpeedState();
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
            //FriendInviteListData inviteData = MemoryData.FriendInviteData.FInviteListData;
            //UI.FriendNumLab.text = inviteData.InviteCount.ToString();
            //UI.RoomCardNumLab.text = inviteData.RoomCardCount.ToString();


            //List<FriendInviteData> InviteList = inviteData.InviteListData;
            //if (InviteList.Count <= 0)
            //{
            //    UI.NoFriendLab.SetActive(true);
            //}
            //else
            //{
            //    UI.NoFriendLab.SetActive(false);
            //}

            //UITools.CreateChild<FriendInviteData>(UI.GridObj.transform, null, InviteList, (go, invitationData) =>
            //{
            //    UIFriendInvitationList invitationList = go.GetComponent<UIFriendInvitationList>();

            //    invitationList.FriendInvitationListInit(invitationData);
            //});
            //UI.GridObj.Reposition();
        }

        #endregion------------------------------------------------------------------------
    }
}
