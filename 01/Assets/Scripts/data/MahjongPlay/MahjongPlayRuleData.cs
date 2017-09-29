using System;
/**
* @Author JL
*
*
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using projectQ.projectQ.XmlData;
using System.Text;
using System.Xml.Serialization;
namespace projectQ
{
    /// <summary>
    /// 规则 2 RulerTypeConfig
    /// </summary>
    public class MahjongPlayRule : IComparable<MahjongPlayRule>
    {
        public int row = 0; //计算行数用
        public int optionSort = 0;
        public MahjongPlay ParentPlay;
        public int id;
        //规则名称
        public string Name
        {
            get
            {
                if (MemoryData.MahjongPlayData.AllRulerTypeMap.ContainsKey(this.id))
                {
                    return MemoryData.MahjongPlayData.AllRulerTypeMap[this.id].RulerTypeName;
                }
                return "临时规则";
            }
        }
        //规则说明
        public string Memo
        {
            get
            {
                if (MemoryData.MahjongPlayData.AllRulerTypeMap.ContainsKey(this.id))
                {
                    return MemoryData.MahjongPlayData.AllRulerTypeMap[this.id].RulerTypeExplain;
                }
                return "临时规则说明";
            }
        }

        public int ruleSort = int.MinValue;
        public int RuleSort
        {
            get
            {
                if (ruleSort == int.MinValue && MemoryData.MahjongPlayData.AllRulerTypeMap.ContainsKey(this.id))
                {
                    ruleSort = MemoryData.MahjongPlayData.AllRulerTypeMap[this.id].RulerTypeSort;
                }
                return ruleSort;
            }
        }

        public MahjongPlayRule(int id, MahjongPlay Parent)
        {
            this.id = id;
            this.ParentPlay = Parent;
        }

        /// <summary>
        /// 通过ID找到Option 保证顺序. 主要是为了顺序才有此方法
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public List<MahjongPlayOption> GetOptionByIds(List<int> ids)
        {
            var result = new List<MahjongPlayOption>();

            var optionDataList = ParentPlay.OptionLogic.GetAllOptionData();
            for (int i = 0; i < optionDataList.Count; i++)
            {
                var option = optionDataList[i];
                if (ids.IndexOf(option.OptionId) >= 0)
                {
                    if (result.IndexOf(option) == -1)
                        result.Add(option);
                }
            }
            return result;
        }
        public List<MahjongPlayOption> GetOptionByRulers(List<int> ruler)
        {
            return GetOptionByIds(ruler);
        }

        public List<string> GetOptionByRulersStr(List<RulerItem> rulerItem, int ruleId = 0)
        {
            var result = new List<string>();

            var optionDataList = ParentPlay.OptionLogic.GetOptionDataListByRuleId(ruleId);
            
            for (int j = 0; j < optionDataList.Count; j++)
                {
                    var option = optionDataList[j];

                    var temp = rulerItem.Find((findItem) =>
                    {
                        return findItem.RulerConfigID == option.OptionId;
                    });
                    if (temp != null)
                    {
                        string str = option.GetNameStr(temp);
                        if (result.IndexOf(str) == -1)
                            result.Add(str);
                    }
                }
            return result;
        }

        public Dictionary<int, MahjongPlayOption> GetAllOption()
        {
            var result = new Dictionary<int, MahjongPlayOption>();

            var optionDataList = ParentPlay.OptionLogic.GetAllOptionData();
            for (int j = 0; j < optionDataList.Count; j++)
            {
                var option = optionDataList[j];
                if (!result.ContainsKey(option.OptionId))
                {
                    result.Add(option.OptionId, option);
                }
            }

            return result;
        }

        public int CompareTo(MahjongPlayRule other)
        {
            return this.RuleSort - other.RuleSort;
        }

    }

    /// <summary>
    /// 规则
    /// </summary>
    public class MahjongRulerTypeConfig
    {
        public int RulerTypeId;
        public string RulerTypeName;
        public string RulerTypeExplain;
        public int RulerTypeSort;

        public MahjongRulerTypeConfig(Msg.RulerTypeConfig pro)
        {
            this.RulerTypeId = pro.RulerType;
            this.RulerTypeName = pro.RulerTypeName;
            this.RulerTypeExplain = string.IsNullOrEmpty(pro.RulerTypeDesc) ? "" : pro.RulerTypeDesc;
            this.RulerTypeSort = pro.RulerTypeSort;
        }
    }

}