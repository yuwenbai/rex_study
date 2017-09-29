/**
* @Author lyb
* 活动
*
*
*/

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace projectQ
{
    public class UIActivity : UIViewBase
    {
        public UIActivityModel Model
        {
            get { return _model as UIActivityModel; }
        }

        /// <summary>
        /// 关闭按钮
        /// </summary>
        public GameObject CloseBtn;
        /// <summary>
        /// 创建活动按钮的父类
        /// </summary>
        public UIGrid GridLeft;
        /// <summary>
        /// 活动图片显示
        /// </summary>
        public UITexture ActivityTex;

        #region override --------------------------------------------

        public override void Init()
        {
            UIEventListener.Get(CloseBtn).onClick = OnCloseBtnClick;
        }

        public override void OnHide()
        {
        }

        public override void OnShow()
        {
            ActivityInit();
        }

        protected override void OnShowAndAnimationEnd()
        {
            GridLeft.Reposition();
        }

        #endregion --------------------------------------------------

        #region Common ----------------------------------------------

        public void ActivityInit()
        {
            List<ActivityData> dataList = MemoryData.SysActivityData.GetActivityList();

            if (dataList == null)
            {
                Model.C2SActivityDataSend(Msg.LotteryTypeDef.Lottery_Game);
            }
            else
            {
                Model.S2CActivityInit();
            }
        }

        #endregion --------------------------------------------------

        #region 按钮回调 --------------------------------------------

        void OnCloseBtnClick(GameObject obj)
        {
            this.Close();
        }

        #endregion --------------------------------------------------
    }
}