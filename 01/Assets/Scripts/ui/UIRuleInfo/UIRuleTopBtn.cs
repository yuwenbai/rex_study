/**
 * @Author lyb
 *  规则分类按钮
 *
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace projectQ
{
    /// <summary>
    /// 规则按钮类型
    /// </summary>
    public enum RuleBtnTypeEnum
    {
        /// <summary>
        /// 基本规则
        /// </summary>
        RULE_BASE = 0,
        /// <summary>
        /// 番型规则
        /// </summary>
        RULE_FOREIGN = 1,
        /// <summary>
        /// 特殊规则
        /// </summary>
        RULE_SPECIAL = 2,
        /// <summary>
        /// 结算规则
        /// </summary>
        RULE_FINISH = 3,
        /// <summary>
        /// 详细规则
        /// </summary>
        RULE_DETAIL = 4,
    }

    public class UIRuleTopBtn : MonoBehaviour
    {
        public delegate void RuleTopBtnDelegate(RuleBtnTypeEnum ruleType, string ruleStr);
        public RuleTopBtnDelegate OnClickCallBack;

        /// <summary>
        /// 该按钮的规则类型
        /// </summary>
        public UILabel RuleBtnName;
        /// <summary>
        /// 该按钮对应的规则枚举
        /// </summary>
        private RuleBtnTypeEnum TopBtnRuleType;
        /// <summary>
        /// 该按钮对应的规则数据
        /// </summary>
        private string TopBtnRuleStr;

        void Start() { }

        void OnDestroy()
        {
            RuleBtnName = null;
        }

        /// <summary>
        /// 初始化页签按钮
        /// </summary>
        public void RuleTopBtnInit(KeyValuePair<RuleBtnTypeEnum, string> keyValue)
        {
            TopBtnRuleType = keyValue.Key;

            TopBtnRuleStr = keyValue.Value;

            SetTopBtnName(keyValue.Key);
        }

        /// <summary>
        /// 给当前按钮赋值
        /// </summary>
        void SetTopBtnName(RuleBtnTypeEnum btnType)
        {
            switch (btnType)
            {
                case RuleBtnTypeEnum.RULE_BASE:
                    RuleBtnName.text = "基本规则";
                    break;
                case RuleBtnTypeEnum.RULE_FOREIGN:
                    RuleBtnName.text = "番型规则";
                    break;
                case RuleBtnTypeEnum.RULE_SPECIAL:
                    RuleBtnName.text = "特殊规则";
                    break;
                case RuleBtnTypeEnum.RULE_FINISH:
                    RuleBtnName.text = "结算规则";
                    break;
                case RuleBtnTypeEnum.RULE_DETAIL:
                    RuleBtnName.text = "详细规则";
                    break;
            }
        }

        void OnClick()
        {
            if (OnClickCallBack != null)
            {
                OnClickCallBack(TopBtnRuleType, TopBtnRuleStr);
            }
        }
    }
}