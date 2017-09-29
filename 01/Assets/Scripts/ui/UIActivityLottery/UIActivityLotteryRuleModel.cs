/**
 * @Author jl
 *  转盘活动规则
 *
 */

namespace projectQ
{
    public class UIActivityLotteryRuleModel : UIModelBase
    {
        public UIActivityLotteryRule UI
        {
            get { return _ui as UIActivityLotteryRule; }
        }

        #region override------------------------------------------

        protected override GEnum.NamedEvent[] FocusNetWorkData()
        {
            return new GEnum.NamedEvent[] { };
        }

        protected override void OnNetWorkDataCallBack(GEnum.NamedEvent msgEnum, object[] data) { }

        #endregion------------------------------------------------

        /// <summary>
        /// 初始化面板数据
        /// </summary>
        public void PlayRuleDataInit()
        {
            string mentStr = MemoryData.XmlData.XmlBuildDataText_Get(GTextsEnum.G_Text_4);

            UI.RuleDocLab.text = mentStr;

            UI.ScrollViewObj.ResetPosition();
        }
    }
}