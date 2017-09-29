/**
 * @Author lyb
 *  点击填写邀请我的人。。弹出的面板
 *
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace projectQ
{
    public class UIPrompt : MonoBehaviour
    {
        /// <summary>
        /// 输入框
        /// </summary>
        public UIInput InputPrompt;
        /// <summary>
        /// 关闭按钮
        /// </summary>
        public GameObject CloseButton;
        /// <summary>
        /// 确认按钮
        /// </summary>
        public GameObject PromptOkBtn;
        /// <summary>
        /// 主UI
        /// </summary>
        public UIFriendAwardModel FriendAwardModel;

        void Start()
        {

        }

        /// <summary>
        /// 初始化提示面板
        /// </summary>
        public void UIPromptInit()
        {
            InputPrompt.value = "";
            UIEventListener.Get(CloseButton).onClick = OnCloseBtnClick;
            UIEventListener.Get(PromptOkBtn).onClick = OnPromptOkBtnClick;
        }

        #region 按钮点击回调方法-----------------------------------------------------

        /// <summary>
        /// 点击关闭按钮
        /// </summary>
        void OnCloseBtnClick(GameObject button)
        {
            gameObject.SetActive(false);
        }

        /// <summary>
        /// 点击ok按钮
        /// </summary>
        void OnPromptOkBtnClick(GameObject button)
        {
            FriendAwardModel.C2SInviteCodeSend(InputPrompt.value);
        }

        #endregion-------------------------------------------------------------------
    }
}