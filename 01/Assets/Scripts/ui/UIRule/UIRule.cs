/**
 * @Author lyb
 *  玩法规则模块
 *
 */

using UnityEngine;

namespace projectQ
{
    public class UIRule : UIViewBase
    {
        public UIRuleModel Model
        {
            get { return _model as UIRuleModel; }
        }

        /// <summary>
        /// 标题文本
        /// </summary>
        public UILabel TitleLab;
        /// <summary>
        /// 规则文本
        /// </summary>
        public UILabel RuleDocLab;
        /// <summary>
        /// 错误提示文本
        /// </summary>
        public GameObject RuleErrObj;
        /// <summary>
        /// 关闭面板按钮
        /// </summary>
        public GameObject CloseBtn;
        /// <summary>
        /// 滑动控件
        /// </summary>
        public UIScrollView ScrollViewObj;

        /// <summary>
        /// 外界调用参数
        /// 当前显示哪一个玩法规则
        /// </summary>
        private int _PlayingID;
        public int PlayingID
        {
            get { return _PlayingID; }
            set { _PlayingID = value; }
        }

        #region override-------------------------------------------------

        /// <summary>
        /// 打开面板，传递参数，先调用该方法
        /// </summary>
        public override void OnPushData(object[] data)
        {
            if (data.Length > 0)
            {
                PlayingID = (int)data[0];
            }
        }

        public override void Init()
        {
            UIEventListener.Get(CloseBtn).onClick = CloseBtnClick;
            EventDispatcher.AddEvent(GEnum.NamedEvent.EMjControlCloseMaskUI, CloseUI);
        }

        public override void OnHide() { }

        public override void OnShow()
        {
            Model.PlayRuleDataInit();
        }

        protected override void OnClose()
        {
            EventDispatcher.RemoveEvent(GEnum.NamedEvent.EMjControlCloseMaskUI, CloseUI);
            base.OnClose();
        }

        #endregion-------------------------------------------------------

        /// <summary>
        /// 返回按钮
        /// </summary>
        private void CloseBtnClick(GameObject go)
        {
            this.Close();
        }


        private void CloseUI(object[] vars)
        {
            this.Close();
        }
    }
}