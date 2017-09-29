/**
 * @Author lyb
 *  战绩消息模块
 *
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using projectQ;

namespace projectQ
{
    public class UIMessageBattle : UIViewBase
    {
        public UIMessageBattleModel Model
        {
            get { return _model as UIMessageBattleModel; }
        }

        /// <summary>
        /// 删除按钮
        /// </summary>
        public GameObject DeleteBtn;
        /// <summary>
        /// 关闭面板按钮
        /// </summary>
        public GameObject CloseBtn;
        /// <summary>
        /// 创建局数列表的Grid
        /// </summary>
        public UIGrid GrideBattleObj;
        /// <summary>
        /// 全选按钮
        /// </summary>
        public UIToggle SelectAllBtn;
        /// <summary>
        /// 滑动控件
        /// </summary>
        public UIScrollView ScrollViewObj;
        /// <summary>
        /// 没有可查看的战绩显示
        /// </summary>
        public GameObject NoMessageObj;

        #region override-------------------------------------------------

        public override void Init()
        {
            UIEventListener.Get(DeleteBtn).onClick = OnDeleteBtnClick;
            UIEventListener.Get(CloseBtn).onClick = OnCloseBtnClick;
        }

        public override void OnHide() { }

        public override void OnShow() { }

        protected override void OnShowAndAnimationEnd()
        {
            C2SMessageBattleDataSend();
        }

        void C2SMessageBattleDataSend()
        {
            //ScrollViewObj.ResetPosition();

            //Model.Init();

            LoadSendLoading();

            EventDispatcher.FireSysEvent(GEnum.NamedEvent.EGetGameData, GEnum.NamedGameData.EC2SMessageBattleList);
        }

        #endregion-------------------------------------------------------

        /// <summary>
        /// 点击了删除按钮
        /// </summary>
        private void OnDeleteBtnClick(GameObject go)
        {
            Model.BtnDeleteClick();
        }

        /// <summary>
        /// 返回按钮
        /// </summary>
        private void OnCloseBtnClick(GameObject go)
        {
            this.Close();
        }
    }
}