/**
 * @Author JinLong
 *
 * 规则选项数据
 *
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace projectQ
{
    public enum OptionState
    {
        Show = 1, //显示
        Hide, //隐藏
        ChangeValue,  //改变值
        NoClick,  //不可点击
        CanClick  //可操作
    }
    /// 选项 4
    public class MahjongPlayOption
    {
        public MahjongPlayRule ParentRule;
        public OptionConditionInfo ConditionInfo = new OptionConditionInfo(); //选项条件和状态数据

        //选项ID
        public int OptionId;
        //选项名称
        public string Name;
        //是否默认勾选
        public bool DefaultSelect = false;

        public int GroupId; //toggole 组ID

        public bool IsSelected = false; //是否被选中

        //2017-07-10 新增
        //原规则类型ID
        public int RulerTypeID;
        public int ParamType = 0; //0默认参数1，其他按照约定
        public int Param1 = 0;
        public int Param2 = 0;
        public int Param3 = 0;

        public int OptionSort;

        public int Sort;

        //下拉列表时 才用
        public List<MahjongPlayOption> DropOptionList = null;

        //-> 星星相关
        public List<OptionStarData> StarDataList;
        public OptionStarData DefaultStarData;

        public void AddStarData(OptionStarData data)
        {
            if (StarDataList == null)
                StarDataList = new List<OptionStarData>();

            StarDataList.Add(data);
        }

        public void AddDropListOptionData(MahjongPlayOption optionData)
        {
            if (DropOptionList == null)
                DropOptionList = new List<MahjongPlayOption>();

            MahjongPlayOption option = DropOptionList.Find(
                (data) =>
                {
                    if (data.OptionId == optionData.OptionId)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                );

            if (option == null)
            {
                DropOptionList.Add(optionData);
            }
        }

        public string GetNameStr(RulerItem item = null)
        {
            string result = Name;
            int index = result.IndexOf("(");
            if (index > 0)
            {
                result = result.Substring(0, index);
            }

            if ((ParamType == 2 || ParamType == 3) && item != null)
            {
                return result + "(" + item.RulerValue + ")";
            }

            if(ParamType == 4)
            {
                result = GetStarData().name;
            }

            return result;
        }

        public ChangeValue ChangeValueOption;

        //XML数据
        public int row;
        public int column;
        public int alignType = (int)AlignType.Left; //对其方式

        public int showType; //-> 显示类型(1单选 2复选 3列表)

        public MahjongPlayOption(int id, string name, bool defaultSelect, MahjongPlayRule ParentRule, int alignType = 1, int row = 0, int column = 0, int sort = 0)
        {
            this.OptionId = id;
            this.Name = name;
            this.DefaultSelect = defaultSelect;
            this.OptionSort = sort;

            this.ParentRule = ParentRule;

            this.alignType = alignType;
            this.row = row;
            this.column = column;
        }
        public void InitData(int RulerTypeID, int ParamType, int Param1, int Param2, int Param3)
        {
            this.RulerTypeID = RulerTypeID;
            this.ParamType = ParamType;
            this.Param1 = Param1;
            this.Param2 = Param2;
            this.Param3 = Param3;

            if (showType == (int)e_OptionType.ChangeValue)
            {
                ChangeValueOption = new ChangeValue(Param1);
                ChangeValueOption.ChangeAllValue(Param1, Param2, Param1);
            }
        }

        public OptionStarData GetStarData()
        {
            OptionStarData starData = DefaultStarData;

            int count = StarDataList.Count;
            for (int i = 0; i < count; i++)
            {
                if (StarDataList[i].isSelected)
                {
                    starData = StarDataList[i];
                }
            }
            return starData;
        }
    }

    //选项条件和状态数据
    public class OptionConditionInfo
    {
        public int myState;         //我的状态 1显示 2不显示 3改变值 4 置灰 5 可点击(取消置灰)
        public bool isChanged = false; //是否被改变过

        private List<List<AppointData>> frontList; //前置条件  *-》每个索引之间是"或"关系 *-》每个索引里的List是"并且"关系
        private List<List<AppointData>> mutexList; //互斥项  *-》跟上面一样

        public void AddMutexList(List<AppointData> mutex)
        {
            if (mutexList == null)
                mutexList = new List<List<AppointData>>();

            mutexList.Add(mutex);
        }

        public void AddFrontList(List<AppointData> front)
        {
            if (frontList == null)
                frontList = new List<List<AppointData>>();

            frontList.Add(front);
        }

        public List<List<AppointData>> GetFrontList()
        {
            return frontList;
        }

        public List<List<AppointData>> GetMutexList()
        {
            return mutexList;
        }
    }

    //加减号
    public class ChangeValue
    {
        public GameObject ChangeValueObj;
        public int minValue = 0;
        public int maxValue = 0;
        private int m_Value = 0;
        public int value
        {
            get
            {
                if (m_Value <= minValue)
                {
                    m_Value = minValue;
                }
                if (m_Value >= maxValue)
                {
                    m_Value = maxValue;
                }
                return m_Value;
            }
        }

        public ChangeValue()
        {

        }

        public ChangeValue(int num)
        {
            m_Value = num;
        }

        public void AddValue()
        {
            if (m_Value >= maxValue)
            {
                m_Value = maxValue;
            }
            else
            {
                m_Value++;
            }
        }

        public void SubValue()
        {
            if (m_Value <= minValue)
            {
                m_Value = minValue;
            }
            else
            {
                m_Value--;
            }
        }

        public void SetValue(int remValue)
        {
            m_Value = remValue;
        }

        public void ChangeAllValue(int min, int max, int value)
        {
            minValue = min;
            maxValue = max;

            if (m_Value > minValue || m_Value < maxValue)
                m_Value = value;
        }
    }

    public class OptionStarData
    {
        public GameObject star;
        public bool isSelected;
        public int configId;
        public string name;
    }

    [System.Serializable]
    public class AppointData
    {
        public int frontID;         //前置ID
        public int frontType;       //前置类型 1显示 2不显示 3值改变
        public int frontValue;      //前置参数 
        public int myState;         //我的状态 1显示 2不显示 3值改变 4 置灰 5 可点击(取消置灰)
        public int defaultValue;    //默认参数
        public int minValue;        //最小参数
        public int maxValue;        //最大参数

        public void InitDefault(int id)
        {
            frontID = id;
            this.frontType = 1;
            this.myState = 1;
        }

        public bool Equals(AppointData other)
        {
            if (this == other) return true;

            if (this.frontID == other.frontID
                && this.frontType == other.frontType
                && this.frontValue == other.frontValue
                && this.myState == other.myState
                && this.defaultValue == other.defaultValue
                && this.minValue == other.minValue
                && this.maxValue == other.maxValue)
            {
                return true;
            }
            return false;
        }
    }
}
