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
    public class UIPrepareGameOpenDeskMgr : MonoBehaviour
    {
        [Tooltip("邀请好友按钮")]
        public UISprite ButtonInviteFriend;
        [Tooltip("邀请WeiXin按钮")]
        public GameObject ButtonInviteWeiXin;

        public void RefreshUI()
        {
            MemoryData.GameStateData.SetCurrUISign(SysGameStateData.EnumUISign.PrepareGame);
        }

        #region Event
        /// <summary>
        /// 邀请好友按钮
        /// </summary>
        /// <param name="go"></param>
        public void OnButtonInviteFriendClick(GameObject go)
        {
            EventDispatcher.FireSysEvent(GEnum.NamedEvent.SysUI_FriendList_OpenClose_Req,true);
        }
        /// <summary>
        /// 邀请微信好友按钮
        /// </summary>
        /// <param name="go"></param>
        public void OnButtonInviteWeiXinClick(GameObject go)
        {
            //邀请微信好友进桌
            LinkConf conf = SDKManager.Instance.GetDataByKey("WECHAT_SHARE_INVITE_PLAY");
            var deskInfo = MemoryData.DeskData.GetOneDeskInfo(MjDataManager.Instance.MjData.curUserData.selfDeskID);
            var mjPlay = MemoryData.MahjongPlayData.GetMahjongPlayByConfigId(deskInfo.mjGameConfigId);

            System.Text.StringBuilder sb = new System.Text.StringBuilder();

            List<string> strList = MemoryData.MahjongPlayData.GetMahjongPlayOptionNames(deskInfo.mjGameConfigId, deskInfo.mjRulerSelected);
            for(int i = 0; i < strList.Count; i++)
            {
                if(i > 0)
                {
                    sb.Append("、");
                }
                sb.Append(strList[i]);
            }

            WXShareParams shareParams = new WXShareParams("WECHAT_SHARE_INVITE_PLAY");
            shareParams.InsertUrlParams(new object[] {(int)WXOpenParaEnum.SHARE_INVITE_PLAY, MjDataManager.Instance.MjData.curUserData.selfDeskID,
                "", "", "", "", "" });
            shareParams.InserTitleParams(new object[] { mjPlay.Name, MjDataManager.Instance.MjData.curUserData.selfDeskID });
            shareParams.InsertDescParams(new object[] { sb.ToString() });
            SDKManager.Instance.SDKFunction("WECHAT_SHARE_INVITE_PLAY", shareParams);
        }

        public bool IsFriendListOpen = false;
        /// <summary>
        /// 更新按钮状态
        /// </summary>
        /// <param name="isCanClick"></param>
        public void OnUpdateButton(bool isCanClick)
        {
            IsFriendListOpen = !isCanClick;
            ButtonInviteFriend.spriteName = isCanClick ? "prepare_button_02" : "prepare_button_03";
            ButtonInviteFriend.GetComponent<UIButton>().normalSprite = isCanClick ? "prepare_button_02" : "prepare_button_03";
            //ButtonInviteFriend.isEnabled = isCanClick;
        }
        #endregion

        void Awake()
        {
            //邀请好友按钮
            UIEventListener.Get(ButtonInviteFriend.gameObject).onClick = OnButtonInviteFriendClick;

            //邀请微信好友按钮
            UIEventListener.Get(ButtonInviteWeiXin.gameObject).onClick = OnButtonInviteWeiXinClick;
        }
    }
}