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
    public class UIFriendAwardModel : UIModelBase
    {
        public UIFriendAward UI
        {
            get { return this._ui as UIFriendAward; }
        }

        #region override------------------------------------------------------------------

        protected override GEnum.NamedEvent[] FocusNetWorkData()
        {
            return new GEnum.NamedEvent[]
            {
                GEnum.NamedEvent.SysUI_FriendInvite_CodeAward_Get,
                GEnum.NamedEvent.SysUI_FriendAward_OnPress,
            };
        }

        protected override void OnNetWorkDataCallBack(GEnum.NamedEvent msgEnum, object[] data)
        {
            switch (msgEnum)
            {
                case GEnum.NamedEvent.SysUI_FriendInvite_CodeAward_Get:

                    int isSucc = (int)data[0];
                    if (isSucc == 0)
                    {
                        MemoryData.PlayerData.MyPlayerModel.PlayerDataBase.BeInviteeCode = (string)data[1];
                        MemoryData.PlayerData.MyPlayerModel.PlayerDataBase.BeInviteeName = (string)data[2];
                        UI.FriendAwardInit();
                        UI.UIPrompt.UIPromptInit();
                        UI.UIPrompt.gameObject.SetActive(false);
                    }
                    else
                    {
                        this.UI.LoadPop(WindowUIType.SystemPopupWindow, "邀好友有礼", "验证邀请码失败，请检查后重试",
                            new string[] { "确定" }, (index) => { });
                    }
                    break;

                case GEnum.NamedEvent.SysUI_FriendAward_OnPress:
                    UI.OnPressCopy();
                    break;
            }
        }

        #endregion------------------------------------------------------------------------

        /// <summary>
        /// 发送邀请码给服务器
        /// </summary>
        public void C2SInviteCodeSend(string codeStr)
        {
            if (codeStr != "")
            {
                ModelNetWorker.Instance.C2SInviteCodeGetAwardReq(codeStr);
            }
            else
            {
                this.UI.LoadPop(WindowUIType.SystemPopupWindow, "邀好友有礼", "邀请码为空或输入有误，请检查后重试",
                new string[] { "确定" }, (index) => { });
            }
        }
    }
}
