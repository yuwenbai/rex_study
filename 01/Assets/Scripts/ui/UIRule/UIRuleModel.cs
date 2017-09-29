/**
 * @Author lyb
 *
 *
 */

using System.Collections.Generic;
using UnityEngine;

namespace projectQ
{
    public class UIRuleModel : UIModelBase
    {
        public UIRule UI
        {
            get { return _ui as UIRule; }
        }

        #region override------------------------------------------

        protected override GEnum.NamedEvent[] FocusNetWorkData()
        {
            return new GEnum.NamedEvent[]
            {
            };
        }

        protected override void OnNetWorkDataCallBack(GEnum.NamedEvent msgEnum, object[] data)
        {
            switch (msgEnum)
            {
                default:
                    break;
            }
        }

        #endregion------------------------------------------------

        private MahjongPlay MjData;

        /// <summary>
        /// 初始化面板数据
        /// </summary>
        public void PlayRuleDataInit()
        {
            MjData = MemoryData.MahjongPlayData.GetMahjongPlayByConfigId(UI.PlayingID);

            UI.TitleLab.text = MjData.Name;

            PlayRule ruleData = Rule_LoadXml();

            if (ruleData != null)
            {
                UI.RuleErrObj.SetActive(false);

                string ruleStr = "";
                if (!string.IsNullOrEmpty(ruleData.RuleBase))
                {
                    ruleStr += "[e29e21]玩法简介:[-]\n" + ruleData.RuleBase + "\n\n";
                }

                if (!string.IsNullOrEmpty(ruleData.RuleForeign))
                {
                    ruleStr += "[e29e21]基本玩法:[-]\n" + ruleData.RuleForeign + "\n\n";
                }

                if (!string.IsNullOrEmpty(ruleData.RuleSpecial))
                {
                    ruleStr += "[e29e21]可选玩法:[-]\n" + ruleData.RuleSpecial + "\n\n";
                }

                if (!string.IsNullOrEmpty(ruleData.RuleFinish))
                {
                    ruleStr += "[e29e21]牌型番数:[-]\n" + ruleData.RuleFinish + "\n\n";
                }

                if (!string.IsNullOrEmpty(ruleData.RuleDetail))
                {
                    ruleStr += "[e29e21]结算规则:[-]\n" + ruleData.RuleDetail + "\n\n";
                }                

                UI.RuleDocLab.text = ruleStr;
            }
            else
            {
                UI.RuleErrObj.SetActive(true);
            }

            UI.ScrollViewObj.ResetPosition();
        }

        #region lyb 查找数据表中的某一行数据----------------------

        private PlayRule Rule_LoadXml()
        {
            List<BaseXmlBuild> buildList = MemoryData.XmlData.XmlBuildDataDic["PlayRule"];

            foreach (BaseXmlBuild build in buildList)
            {
                PlayRule info = (PlayRule)build;

                if (UI.PlayingID > 0)
                {
                    if (MjData != null)
                    {
                        if (MjData.Name.Equals(info.RuleName))
                        {
                            return info;
                        }
                    }
                }
            }

            return null;
        }

        #endregion ------------------------------------------------
    }
}