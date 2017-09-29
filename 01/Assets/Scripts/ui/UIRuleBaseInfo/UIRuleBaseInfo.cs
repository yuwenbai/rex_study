/**
 * @Author GarFey
 *  麻将基础规则模块
 */
using UnityEngine;

namespace projectQ
{
    public class UIRuleBaseInfo : UIViewBase
    {
        public UIRuleBaseInfoModel Model
        {
            get { return _model as UIRuleBaseInfoModel; }
        }
        /// <summary>
        /// 地区ScrollView
        /// </summary>
        public UIScrollView RegionScrollView;
        /// <summary>
        /// 流行玩法空数据显示
        /// </summary>
        public UILabel FadNullTatile;
        /// <summary>
        /// 地区玩法空数据显示
        /// </summary>
        public UILabel RegionNullTatile;
        /// <summary>
        /// 地区麻将名称
        /// </summary>
        public UILabel RegionName;
        /// <summary>
        /// 地方特色玩法创建栏Grid
        /// </summary>
        public UIGrid GridRegionObj;
        /// <summary>
        /// 流行玩法创建栏Grid
        /// </summary>
        public UIGrid GridFadObj;
        /// <summary>
        /// 关闭面板按钮
        /// </summary>
        public GameObject CloseBtn;
        /// <summary>
        /// 切换按钮
        /// </summary>
        public GameObject SwitchBtn;
        /// <summary>
        /// 三角
        /// </summary>
        public GameObject TriangleObj;
        public UISprite TriangleUpSp;
        public UISprite TriangleDownSp;

        /// <summary>
        /// 打开面板，传递参数，先调用该方法
        /// </summary>
        public override void OnPushData(object[] data)
        {

        }

        public override void Init()
        {
            UIEventListener.Get(CloseBtn).onClick = CloseBtnClick;
            UIEventListener.Get(SwitchBtn).onClick = SwitchBtnClick;
        }

        public override void OnHide() { }

        public override void OnShow()
        {
            Model.RulePlayTabBtnCreat();
        }

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