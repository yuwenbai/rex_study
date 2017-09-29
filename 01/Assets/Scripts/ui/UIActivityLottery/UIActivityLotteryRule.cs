/**
 * @Author jl
 *  转盘活动规则
 *
 */

using UnityEngine;

namespace projectQ
{
    public class UIActivityLotteryRule : UIViewBase
    {
        public UIActivityLotteryRuleModel Model
        {
            get { return _model as UIActivityLotteryRuleModel; }
        }

        /// <summary>
        /// 规则文本
        /// </summary>
        public UILabel RuleDocLab;
        /// <summary>
        /// 关闭面板按钮
        /// </summary>
        public GameObject CloseBtn;
        /// <summary>
        /// 滑动控件
        /// </summary>
        public UIScrollView ScrollViewObj;

        #region override-------------------------------------------------

        /// <summary>
        /// 打开面板，传递参数，先调用该方法
        /// </summary>
        public override void OnPushData(object[] data) { }

        public override void Init()
        {
            UIEventListener.Get(CloseBtn).onClick = CloseBtnClick;
        }

        public override void OnHide() { }

        public override void OnShow()
        {
            Model.PlayRuleDataInit();
        }

        #endregion-------------------------------------------------------

        /// <summary>
        /// 返回按钮
        /// </summary>
        private void CloseBtnClick(GameObject go)
        {
            this.Close();
        }
    }
}