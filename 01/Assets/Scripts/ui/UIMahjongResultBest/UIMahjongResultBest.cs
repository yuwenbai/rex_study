/**
* @Author lyb
* 最大牌型分享
*
*
*/
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace projectQ
{
    public class UIMahjongResultBest : UIViewBase
    {
        public UIMahjongResultBestModel Model
        {
            get { return _model as UIMahjongResultBestModel; }
        }

        /// <summary>
        /// 关闭按钮
        /// </summary>
        public GameObject CloseButton;
        /// <summary>
        /// 分享按钮
        /// </summary>
        public GameObject ShareButton;
        /// <summary>
        /// 玩家昵称
        /// </summary>
        public UILabel PlayerName;
        /// <summary>
        /// 最大番数
        /// </summary>
        public UILabel BoutsMax;
        /// <summary>
        /// 当前积分
        /// </summary>
        public UILabel ScoreNum;
        /// <summary>
        /// 我
        /// </summary>
        public UILabel ScoreOwnLab;
        /// <summary>
        /// 我得积分
        /// </summary>
        public UILabel ScoreOwnNum;
        /// <summary>
        /// 最大牌型
        /// </summary>
        public BestRecordController BesrRecordObj;

        public BestMjRecord MjRecordBest = null;

        /// <summary>
        /// 本局桌号
        /// </summary>
        private int DeskId;

        #region override ------------------------------------------------------------

        /// <summary>
        /// 打开面板，传递参数，先调用该方法
        /// </summary>
        public override void OnPushData(object[] data)
        {
            if (data.Length > 0)
            {
                MjRecordBest = data[0] as BestMjRecord;

                DeskId = (int)data[1];
            }
        }

        public override void Init()
        {
            UIEventListener.Get(CloseButton).onClick = OnCloseBtnClick;
            UIEventListener.Get(ShareButton).onClick = OnShadeBtnClick;
        }

        public override void OnShow()
        {
            if (MjRecordBest != null)
            {
                Model.ResultBestInit();
                BesrRecordObj.IniBestRecord(MjRecordBest);
            }
        }

        public override void OnHide() { }

        #endregion-------------------------------------------------------------------

        #region 按钮点击回调方法-----------------------------------------------------

        /// <summary>
        /// 关闭按钮点击调用
        /// </summary>
        void OnCloseBtnClick(GameObject button)
        {
            this.Hide();
        }

        /// <summary>
        /// 分享按钮点击调用
        /// </summary>
        void OnShadeBtnClick(GameObject button)
        {
            Vector2 startPoint = new Vector2(0.0f, 0.0f);
            Vector2 shotSize = new Vector2(Screen.width, Screen.height);
            Tools_TexScreenshot.Instance.Texture_Screenshot(startPoint, shotSize, TexShotCallBack);
        }

        /// <summary>
        /// 截屏压缩回调
        /// </summary>
        void TexShotCallBack(bool isBol)
        {
            if (isBol)
            {
                string base64Str = Tools_TexScreenshot.Instance.Texture_ToBase64();

                if (!string.IsNullOrEmpty(base64Str))
                {
                    WXShareParams shareParams = new WXShareParams("WECHAT_SHARE_RESULT_BEST", base64Str);
                    SDKManager.Instance.SDKFunction("WECHAT_SHARE_RESULT_BEST", shareParams);
                }
            }
        }

        #endregion-------------------------------------------------------------------
    }
}