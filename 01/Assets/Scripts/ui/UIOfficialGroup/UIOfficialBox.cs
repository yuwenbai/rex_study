/**
 * @Author lyb
 * 官方麻将试玩群弹框
 *
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace projectQ
{
    public class UIOfficialBox : UIViewBase
    {
        /// <summary>
        /// 官方试玩群的名字
        /// </summary>
        public UILabel Lab_OfficialName;
        /// <summary>
        /// 关闭按钮
        /// </summary>
        public GameObject CloseBtn;
        /// <summary>
        /// 分享到微信按钮
        /// </summary>
        public GameObject WxShareBtn;
        /// <summary>
        /// 客服按钮
        /// </summary>
        public GameObject ServiceBtn;

        /// <summary>
        /// 官方推荐群数据保存
        /// </summary>
        private officialconfig officeInfo;

        void Start() { }

        #region override ------------------------------------------------------------

        public override void OnPushData(object[] data)
        {
            if (data != null && data.Length > 0)
            {
                officeInfo = (officialconfig)data[0];
            }
        }

        public override void Init()
        {
            Lab_OfficialName.text = officeInfo.PlayName;
            if (officeInfo.ID == "0")
            {
                Lab_OfficialName.text = string.Format("{0}官方试玩群", officeInfo.PlayName);
            }

            UIEventListener.Get(CloseBtn).onClick = OnCloseBtnClick;
            UIEventListener.Get(WxShareBtn).onClick = OnWxShareBtnClick;
            UIEventListener.Get(ServiceBtn).onClick = OnServiceBtnClick;
        }

        public override void OnShow() { }

        public override void OnHide() { }

        #endregion-------------------------------------------------------------------

        #region 按钮点击回调方法-----------------------------------------------------

        /// <summary>
        /// 点击关闭按钮
        /// </summary>
        void OnCloseBtnClick(GameObject button)
        {
            this.Close();
        }

        /// <summary>
        /// 点击分享到微信按钮
        /// </summary>
        void OnWxShareBtnClick(GameObject button)
        {
            //QLoger.LOG("试玩群ID = " + officeInfo.ID + officeInfo.PlayName);
            WXShareParams shareParams = new WXShareParams("WECHAT_SHARE_OFFICIAL", officeInfo.UrlRes, Lab_OfficialName.text, null);
            SDKManager.Instance.SDKFunction("WECHAT_SHARE_OFFICIAL", shareParams);
        }

        /// <summary>
        /// 点击客服按钮
        /// </summary>
        void OnServiceBtnClick(GameObject button)
        {
            WebSDKParams param = new WebSDKParams("WEB_OPEN_CS_SERVICE");
            param.InsertUrlParams(MemoryData.UserID, MemoryData.PlayerData.MyPlayerModel.PlayerDataBase.Name);
            SDKManager.Instance.SDKFunction("WEB_OPEN_CS_SERVICE", param);
        }

        #endregion-------------------------------------------------------------------
    }
}
