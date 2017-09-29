/**
 * @Author lyb
 * 微信分享 - 选择
 *
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace projectQ
{
    public class UIWxShare : UIViewBase
    {
        public UIWxShareModel Model
        {
            get { return _model as UIWxShareModel; }
        }

        /// <summary>
        /// 分享到朋友圈
        /// </summary>
        public GameObject WxShareBtn01;
        /// <summary>
        /// 分享到好友
        /// </summary>
        public GameObject WxShareBtn02;

        /// <summary>
        /// 当前分享的Key - 对应LinkConf数据表中的Key列
        /// </summary>
        private string shareType;

        #region override ------------------------------------------------------------

        /// <summary>
        /// 打开面板，传递参数，先调用该方法
        /// </summary>
        public override void OnPushData(object[] data)
        {
            if (data.Length > 0)
            {
                shareType = (string)data[0];
            }
        }

        public override void Init()
        {
            UIEventListener.Get(WxShareBtn01).onClick = OnClickShareBtn01;
            UIEventListener.Get(WxShareBtn02).onClick = OnClickShareBtn02;
        }

        public override void OnShow(){}

        public override void OnHide() { }

        #endregion-------------------------------------------------------------------

        #region 按钮点击回调方法-----------------------------------------------------

        /// <summary>
        /// 点击分享到朋友圈按钮
        /// 分享链接 Type- 1 分享好友 2 分享朋友圈
        /// 分享图片 Type- 3 分享好友 4 分享朋友圈
        /// </summary>
        void OnClickShareBtn01(GameObject button)
        {
            if (shareType.Equals("WECHAT_SHARE_RESULT_BEST"))
            {
                //截图分享朋友圈的
                //TODO: type = 4
            }
            else
            {
                //链接分享朋友圈的
                //TODO: type = 2
            }
        }

        /// <summary>
        /// 点击分享给好友按钮
        /// 分享链接 Type- 1 分享好友 2 分享朋友圈
        /// 分享图片 Type- 3 分享好友 4 分享朋友圈
        /// </summary>
        void OnClickShareBtn02(GameObject button)
        {
            if (shareType.Equals("WECHAT_SHARE_RESULT_BEST"))
            {
                //截图分享好友
                //TODO: type = 3
            }
            else
            {
                //链接分享好友
                //TODO: type = 1
            }
        }

        #endregion-------------------------------------------------------------------
    }
}
