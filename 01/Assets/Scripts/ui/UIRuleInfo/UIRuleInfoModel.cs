/**
 * @Author lyb
 *
 *
 */

using System.Collections.Generic;
using UnityEngine;

namespace projectQ
{
    public class UIRuleInfoModel : UIModelBase
    {
        public UIRuleInfo UI
        {
            get { return _ui as UIRuleInfo; }
        }

        /// <summary>
        /// 规则数据存储列表
        /// </summary>
        public List<PlayRule> RuleDataList = new List<PlayRule>();

        /// <summary>
        /// 当前点击的是哪种规则
        /// </summary>
        private RuleBtnTypeEnum RuleType;

        #region override----------------------------------------------------------------------

        protected override GEnum.NamedEvent[] FocusNetWorkData()
        {
            return new GEnum.NamedEvent[]
            {
                GEnum.NamedEvent.SysData_Region_Update,
            };
        }

        protected override void OnNetWorkDataCallBack(GEnum.NamedEvent msgEnum, object[] data)
        {
            switch (msgEnum)
            {
                case GEnum.NamedEvent.SysData_Region_Update:
                    Rule_LoadXml();
                    RulePlayTabBtnCreat();
                    break;
            }
        }

        #endregion-----------------------------------------------------------------------------

        #region lyb----------------点击页签，规则按钮回调--------------------------------------

        /// <summary>
        /// 点击页签按钮回调
        /// </summary>
        public void RuleTabBtnCallBack(PlayRule data)
        {
            //QLoger.LOG(" ######################### obj.name = " + data.RuleName);
            RuleTopBtnInit(data);
        }

        /// <summary>
        /// 点击规则按钮回调
        /// </summary>
        public void RuleTopBtnCallBack(RuleBtnTypeEnum ruleType, string str)
        {
            UI.ScrollViewObj.ResetPosition();

            RuleType = ruleType;
            UI.RuleDocLab.text = str;
        }

        #endregion-------------------------------------------------------------------------------

        #region lyb----------------初始化规则类相关按钮------------------------------------------

        public void RuleTopBtnInit(PlayRule data)
        {
            List<KeyValuePair<RuleBtnTypeEnum, string>> ruleStrList = GetRuleStrList(data);

            GameObject obj = null;

            UITools.CreateChild<KeyValuePair<RuleBtnTypeEnum, string>>(UI.GridTopObj.transform, null, ruleStrList, (go, ruleStr) =>
            {
                if (ruleStr.Key == RuleType)
                {
                    obj = go;
                }

                UIRuleTopBtn ruleBtn = go.GetComponent<UIRuleTopBtn>();

                ruleBtn.OnClickCallBack = RuleTopBtnCallBack;
                ruleBtn.RuleTopBtnInit(ruleStr);
            });
            UI.GridTopObj.Reposition();

            if (obj != null)
            {
                obj.GetComponent<UIButton>().gameObject.SendMessage("OnClick", SendMessageOptions.DontRequireReceiver);
            }
        }

        #endregion-------------------------------------------------------------------------------

        #region lyb----------------创建左右父类下属子类------------------------------------------

        /// <summary>
        /// 创建左右页签按钮
        /// </summary>
        public void RulePlayTabBtnCreat()
        {
            List<PlayRule> btnLeft = new List<PlayRule>();
            List<PlayRule> btnRight = new List<PlayRule>();

            foreach (PlayRule data in RuleDataList)
            {
                //if (data.RuleType == "1")
                //{
                //    //地方特色，左边
                //    btnLeft.Add(data);
                //}
                //else
                //{
                //    //流行玩法，右边
                //    btnRight.Add(data);
                //}
            }

            bool isclick = false;
            if (btnLeft.Count > 0)
            {
                isclick = true;

                if (UI.ClickType == 2)
                {
                    isclick = false;
                }
            }

            this.RefreshList(UI.GridLeftObj, btnLeft, isclick);
            this.RefreshList(UI.GridRightObj, btnRight, !isclick);

            UI.ScrollViewLeftObj.ResetPosition();
            //UI.ScrollViewObj.ResetPosition();
        }

        /// <summary>
        /// isClick 是否点击第一个按钮
        /// </summary>
        private void RefreshList(UIGrid grid, List<PlayRule> list, bool isClick)
        {
            GameObject obj = null;
            int index = 0;

            UITools.CreateChild<PlayRule>(grid.transform, null, list, (go, ruleData) =>
            {
                index++;
                if (index == 1)
                {
                    obj = go;
                }

                UIRuleTabBtnList tabBtnList = go.GetComponent<UIRuleTabBtnList>();

                tabBtnList.OnClickCallBack = RuleTabBtnCallBack;
                tabBtnList.RuleTabBtnInit(ruleData);
            });
            grid.Reposition();

            if (isClick)
            {
                if (obj != null)
                {
                    obj.GetComponent<UIButton>().gameObject.SendMessage("OnClick", SendMessageOptions.DontRequireReceiver);
                }
            }
        }

        #endregion-------------------------------------------------------------------------------

        #region lyb----------------读取本地Xml文件解析数据存储在本地-----------------------------

        public void Rule_LoadXml()
        {
            RuleDataList = new List<PlayRule>();

            List<BaseXmlBuild> buildList = MemoryData.XmlData.XmlBuildDataDic["PlayRule"];

            foreach (BaseXmlBuild build in buildList)
            {
                PlayRule info = (PlayRule)build;

                //if (info.RuleType == "1")
                //{
                //    if (info.RuleRegion == MemoryData.PlayerData.MyPlayerModel.PlayerDataBase.RegionID.ToString())
                //    {
                //        RuleDataList.Add(info);
                //    }
                //}

                //if (info.RuleType == "2")
                //{
                //    RuleDataList.Add(info);
                //}
            }
        }

        #endregion -----------------------------------------------------------------------------

        #region lyb----------------Tools Data---------------------------------------------------

        /// <summary>
        /// 获取当前玩法下有多少种规则
        /// </summary>
        /// <returns></returns>
        public List<KeyValuePair<RuleBtnTypeEnum, string>> GetRuleStrList(PlayRule rData)
        {
            List<KeyValuePair<RuleBtnTypeEnum, string>> ruleList = new List<KeyValuePair<RuleBtnTypeEnum, string>>();

            if (rData.RuleBase != string.Empty)
            {
                KeyValuePair<RuleBtnTypeEnum, string> kValue
                    = new KeyValuePair<RuleBtnTypeEnum, string>(RuleBtnTypeEnum.RULE_BASE, rData.RuleBase);

                ruleList.Add(kValue);
            }
            if (rData.RuleForeign != string.Empty)
            {
                KeyValuePair<RuleBtnTypeEnum, string> kValue
                    = new KeyValuePair<RuleBtnTypeEnum, string>(RuleBtnTypeEnum.RULE_FOREIGN, rData.RuleForeign);

                ruleList.Add(kValue);
            }
            if (rData.RuleSpecial != string.Empty)
            {
                KeyValuePair<RuleBtnTypeEnum, string> kValue
                    = new KeyValuePair<RuleBtnTypeEnum, string>(RuleBtnTypeEnum.RULE_SPECIAL, rData.RuleSpecial);

                ruleList.Add(kValue);
            }
            if (rData.RuleFinish != string.Empty)
            {
                KeyValuePair<RuleBtnTypeEnum, string> kValue
                    = new KeyValuePair<RuleBtnTypeEnum, string>(RuleBtnTypeEnum.RULE_FINISH, rData.RuleFinish);

                ruleList.Add(kValue);
            }
            if (rData.RuleDetail != string.Empty)
            {
                KeyValuePair<RuleBtnTypeEnum, string> kValue
                    = new KeyValuePair<RuleBtnTypeEnum, string>(RuleBtnTypeEnum.RULE_DETAIL, rData.RuleDetail);

                ruleList.Add(kValue);
            }

            return ruleList;
        }

        #endregion------------------------------------------------------------------------------
    }
}