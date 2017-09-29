/**
 * @Author lyb
 *  麻将规则模块
 *
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using projectQ;

namespace projectQ
{
    public class UIRuleInfo : UIViewBase
    {
        public UIRuleInfoModel Model
        {
            get { return _model as UIRuleInfoModel; }
        }

        /// <summary>
        /// 规则文本
        /// </summary>
        public UILabel RuleDocLab;
        /// <summary>
        /// 左边地方特色玩法创建栏Grid
        /// </summary>
        public UIGrid GridLeftObj;
        /// <summary>
        /// 右边流行玩法创建栏Grid
        /// </summary>
        public UIGrid GridRightObj;
        /// <summary>
        /// 规则类按钮Grid
        /// </summary>
        public UIGrid GridTopObj;
        /// <summary>
        /// 关闭面板按钮
        /// </summary>
        public GameObject CloseBtn;
        /// <summary>
        /// 滑动控件
        /// </summary>
        public UIScrollView ScrollViewObj;
        /// <summary>
        /// 左边的滑动控件
        /// </summary>
        public UIScrollView ScrollViewLeftObj;
        /// <summary>
        /// 右边的滑动控件
        /// </summary>
        public UIScrollView ScrollViewRightObj;

        /// <summary>
        /// 切换按钮
        /// </summary>
        public GameObject SwitchBtn;

        /// <summary>
        /// 外界调用参数
        /// 1 点击左边第一个  2 点击右边第一个
        /// </summary>
        private int _ClickType = -1;
        public int ClickType
        {
            get { return _ClickType; }
            set { _ClickType = value; }
        }

        #region override-------------------------------------------------

        /// <summary>
        /// 打开面板，传递参数，先调用该方法
        /// </summary>
        public override void OnPushData(object[] data)
        {
            if (data.Length > 0)
            {
                ClickType = (int)data[0];
            }
        }

        public override void Init()
        {
            UIEventListener.Get(CloseBtn).onClick = CloseBtnClick;
            UIEventListener.Get(SwitchBtn).onClick = SwitchBtnClick;
        }

        public override void OnHide() { }

        public override void OnShow()
        {
            Model.Rule_LoadXml();
            Model.RulePlayTabBtnCreat();
        }

        #endregion-------------------------------------------------------

        /// <summary>
        /// 返回按钮
        /// </summary>
        private void CloseBtnClick(GameObject go)
        {
            this.Close();
        }

        /// <summary>
        /// 切换按钮点击
        /// </summary>
        private void SwitchBtnClick(GameObject go)
        {
            this.LoadUIMain("UIMap");
        }
    }
}