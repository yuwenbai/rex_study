/**
 * @Author lyb
 *  规则页签按钮
 *
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace projectQ
{
    public class UIRuleTabBtnList : MonoBehaviour
    {
        public delegate void RuleTabBtnDelegate(PlayRule ruleData);
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
        /// 页签对应的规则数据
        /// </summary>
        private PlayRule TabBtnRuleData;

        void Start(){}

        void OnDestroy()
        {
            TabBtnName = null;
        }

        /// <summary>
        /// 初始化页签按钮
        /// </summary>
        public void RuleTabBtnInit(PlayRule data)
        {
            TabBtnRuleData = new PlayRule();
            TabBtnRuleData = data;
            //TabBtnId = TabBtnRuleData.RuleId;
            TabBtnName.text = TabBtnRuleData.RuleName;
            //gameObject.name = "ruleTabBtnList" + string.Format("{0:d2}" , int.Parse(TabBtnRuleData.RuleOrder));

            UIToggle toggle = gameObject.GetComponent<UIToggle>();
            toggle.Set(false);
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


