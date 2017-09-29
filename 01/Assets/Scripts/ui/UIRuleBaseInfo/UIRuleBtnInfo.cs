/**
 * @Author GarFey
 *  规则页签按钮
 *
 */

using UnityEngine;

namespace projectQ
{
	public class UIRuleBtnInfo : MonoBehaviour 
	{
		public delegate void RuleTabBtnDelegate(MahjongPlay ruleData);
		public RuleTabBtnDelegate OnClickCallBack;

		/// <summary>
		/// 页签按钮ID
		/// </summary>
		public string TabBtnId;
		/// <summary>
		/// 页签按钮名字
		/// </summary>
		public UILabel TabBtnName;
        /// <summary>
        /// 按钮图片
        /// </summary>
        public UISprite TabSprite;
        /// <summary>
        /// 常用Icon
        /// </summary>
        public UISprite RuleIcon;
		/// <summary>
		/// 页签对应的规则数据
		/// </summary>
		private MahjongPlay TabBtnRuleData;

		void Start(){}

		void OnDestroy()
		{
			TabBtnName = null;
            TabSprite = null;
        }

        /// <summary>
        /// 初始化页签按钮
        /// </summary>
        /// <param name="data"></param>
        /// <param name="isHot"> 是否是热度按钮</param>
        public void RuleTabBtnInit(MahjongPlay data,bool isHot = false)
		{
			TabBtnRuleData = data;
			TabBtnId = TabBtnRuleData.RegionID.ToString();
			TabBtnName.text = TabBtnRuleData.Name;
			//gameObject.name = "ruleTabBtnList" + string.Format("{0:d2}" , int.Parse(TabBtnRuleData.Name));
			gameObject.name = "ruleTabBtnList" + TabBtnRuleData.Name;
            RuleIcon.gameObject.SetActive(isHot);
            /*if (isHot)
            {
                if (data.MjType == MahjongPlay.MahjongPlayType.Fashion)
                {
                    TabSprite.spriteName = "prepare_button_02";
                }
                else
                {
                    TabSprite.spriteName = "createdesk_button_diquwanfa02"; 
                }
            }else
            {
                if (data.MjType == MahjongPlay.MahjongPlayType.Fashion)
                {
                    TabSprite.spriteName = "public_button_05";
                }
                else
                {
                    TabSprite.spriteName = "createdesk_button_diquwanfa";
                }
            }*/
        }

		void OnClick()
		{
			if (OnClickCallBack != null)
			{
				OnClickCallBack(TabBtnRuleData);
			}
		}
	
	}
}
