/**
 * @Author lyb
 *
 *
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace projectQ
{
    public class UIFriendInvitationList : MonoBehaviour
    {
        /// <summary>
        /// 被邀请人的ID
        /// </summary>
        public long FriendInvitationId;
        /// <summary>
        /// 被邀请人的名字
        /// </summary>
        public UILabel FriendName;
        /// <summary>
        /// 被邀请人的时间
        /// </summary>
        public UILabel FriendTime;
        /// <summary>
        /// 领取桌卡按钮
        /// </summary>
        public UIButton ReceiveAwardBtn;
        /// <summary>
        /// 领取桌卡按钮状态
        /// </summary>
        public UILabel ReceiveAwardBtnLab;

        void Start() { }

        /// <summary>
        /// 被邀请好友列表
        /// </summary>
        public void FriendInvitationListInit(FriendInviteData inviteData)
        {
            FriendInvitationId = inviteData.FriendInviteID;

            FriendName.text = inviteData.FriendInviteName;

            FriendTime.text = inviteData.FriendInviteTime;

            switch (inviteData.FriendInviteAwardState)
            {
                case 0:
                    //可领取状态 isEnabled 有问题，暂时使用一个low一点的解决办法
                    ReceiveAwardBtn.GetComponent<UISprite>().spriteName = "public_button_03";
                    ReceiveAwardBtn.GetComponent<BoxCollider>().enabled = true;
                    ReceiveAwardBtn.GetComponent<UIDefinedButton>().enabled = true;
                    ReceiveAwardBtnLab.text = "领取";
                    gameObject.name = "aUIFriendInvitationList";
                    break;
                case 1:
                    //已领取状态
                    ReceiveAwardBtn.GetComponent<UISprite>().spriteName = "public_button_04";
                    ReceiveAwardBtn.GetComponent<BoxCollider>().enabled = false;
                    ReceiveAwardBtn.GetComponent<UIDefinedButton>().enabled = false;
                    ReceiveAwardBtnLab.text = "已领取";
                    gameObject.name = "bUIFriendInvitationList";
                    break;
                case 2:
                    //不可领取状态
                    ReceiveAwardBtn.isEnabled = false;
                    ReceiveAwardBtnLab.text = "不可领取";
                    break;
            }

            UIEventListener.Get(ReceiveAwardBtn.gameObject).onClick = OnReceiveAwardBtnClick;
        }

        void OnReceiveAwardBtnClick(GameObject obj)
        {
            ModelNetWorker.Instance.C2SInvitedAwardReq(FriendInvitationId);
        }
    }
}