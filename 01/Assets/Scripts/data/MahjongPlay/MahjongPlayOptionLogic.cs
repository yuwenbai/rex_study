/**
 * @Author JinLong
 *
 * 主处理 规则选项逻辑 相关 
 * 
 * 1.行列式结构 生成
 * 2.前置,互斥结构 解析和生成
 * 3.导入选中列表
 * 
 */

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace projectQ
{
    public enum AlignType
    {
        Left = 1,
        Center
    }
    public class MahjongPlayOptionLogic
    {
        public int OptionSort = 0;

        private List<MahjongPlayOption> OptionDataList = new List<MahjongPlayOption>();

        private Dictionary<int, MahjongPlayOption> OptionDataDict = new Dictionary<int, MahjongPlayOption>(); //选项字典

        public Dictionary<int, List<int>> ImpactOptionIdList; //一个选项所影响的 ID列表(互斥和前置)

        //*=> Add 接口

        //添加影响id
        public void AddImpactOptionId(int optionId, int impactId)
        {
            if(ImpactOptionIdList == null)
            {
                ImpactOptionIdList = new Dictionary<int, List<int>>();
            }

            if (!ImpactOptionIdList.ContainsKey(optionId))
            {
                ImpactOptionIdList[optionId] = new List<int>();
            }

            int id = ImpactOptionIdList[optionId].Find(
                (opId) =>
                {
                    if (opId == impactId)
                    {
                        return true;
                    }
                    return false;
                }
                );

            if(id == 0)
            {
                ImpactOptionIdList[optionId].Add(impactId); //影响选项ID列表
            }
        }

        //存储选项数据
        public void AddOptionData(MahjongPlayOption optionData)
        {
            MahjongPlayOption option = OptionDataList.Find(
                (data)=>
                {
                    if(data.OptionId == optionData.OptionId)
                    {
                        return true;
                    }
                    return false;
                }
                );
            if(option == null)
            {
                OptionDataList.Add(optionData);
            }

            if(!OptionDataDict.ContainsKey(optionData.OptionId))
            {
                OptionDataDict[optionData.OptionId] = optionData;
            }
        }

        //根据规则Id 获取 当前规则下的所有选项数据
        public List<MahjongPlayOption> GetOptionDataListByRuleId(int ruleId)
        {
            List<MahjongPlayOption> curRuleOptionList = new List<MahjongPlayOption>();

            int count = OptionDataList.Count;
            for(int i = 0; i < count; i++)
            {
                if(OptionDataList[i].ParentRule.id == ruleId)
                {
                    curRuleOptionList.Add(OptionDataList[i]);
                }
            }

            return curRuleOptionList;
        }

        //获取所有选中的数据
        public List<MahjongPlayOption> GetSelectOptionList()
        {
            List<MahjongPlayOption> selectOptionList = new List<MahjongPlayOption>();
            int count = OptionDataList.Count;
            for(int i = 0; i < count; i++)
            {
                MahjongPlayOption optionData = OptionDataList[i];
                if(optionData.IsSelected && optionData.ConditionInfo.myState != (int)OptionState.Hide && optionData.ConditionInfo.myState != (int)OptionState.NoClick)
                {
                    selectOptionList.Add(optionData);
                }
            }

            selectOptionList.Sort(
                (left, right) =>
                {
                    if (left.Sort > right.Sort)
                    {
                        return 1;
                    }
                    else if (left.Sort == right.Sort)
                    {
                        return 0;
                    }
                    else
                    {
                        return -1;
                    }
                }
                );

            return selectOptionList;
        }

        //获取星星ID
        public int GetStarConfigIdByOptionId(int optionId)
        {
            int starConfigId = GetStarDataByOptionId(optionId).configId;
            return starConfigId;
        }
        //获取星星数据
        public OptionStarData GetStarDataByOptionId(int optionId)
        {
            MahjongPlayOption optionData = GetOptionDataById(optionId);

            return optionData.GetStarData();
        }

        //获取所有选项数据
        public List<MahjongPlayOption> GetAllOptionData()
        {
            return OptionDataList;
        }

        public MahjongPlayOption GetOptionDataById(int optionId)
        {
            if(OptionDataDict.ContainsKey(optionId))
                return OptionDataDict[optionId];

            return null;
        }

        //获取被当前选项 影响的所有选项ID
        public List<int> GetImpactIdListById(int optionId)
        {
            if(ImpactOptionIdList != null && ImpactOptionIdList.ContainsKey(optionId))
            {
                return ImpactOptionIdList[optionId];
            }
            return null;
        }

        //获取最大列数
        private int MaxColumn = -1;
        public int GetMaxColumn()
        {
            if(MaxColumn == -1)
            {
                int count = OptionDataList.Count;
                for(int i = 0; i < count; i++)
                {
                    if(OptionDataList[i].column > MaxColumn)
                    {
                        MaxColumn = OptionDataList[i].column;
                    }
                }
            }
            MaxColumn = 4;
            return MaxColumn;
        }

        //清理选中状态
        public void ClearSelected()
        {
            int count = OptionDataList.Count;
            for (int i = 0; i < count; i++)
            {
                MahjongPlayOption optionData = OptionDataList[i];
                optionData.IsSelected = false;
            }
        }

        //建立所有选项之间互斥和前置关系
        public void CreateOptionImpactStructure()
        {
            int count = OptionDataList.Count;
            for(int i = 0; i < count; i++)
            {
                ChangeAllImpactOption(OptionDataList[i]);
            }
            ResetAllOptionChangeState();
        }

        //修改 被当前选项 所影响的所有选项(互斥和前置)
        public void ChangeImpactOption(MahjongPlayOption optionData)
        {
            ChangeAllImpactOption(optionData);
            ResetAllOptionChangeState();
        }

        #region 互斥和前置处理

        //重置所有选项 改变状态
        private void ResetAllOptionChangeState()
        {
            int count = OptionDataList.Count;
            for(int i =0; i < count; i++)
            {
                OptionDataList[i].ConditionInfo.isChanged = false;
            }
        }

        //=> 修改所有被影响的选项
        private void ChangeAllImpactOption(MahjongPlayOption optionData)
        {
            List<int> impactIdList = GetImpactIdListById(optionData.OptionId);

            if (impactIdList == null) return;

            int count = impactIdList.Count;
            //遍历自己所影响的 所有选项
            for (int i = 0; i < count; i++)
            {
                int optionId = impactIdList[i];
                RecursionImpactOption(GetOptionDataById(optionId));
            }
        }

        //递归遍历 所影响的选项
        private void RecursionImpactOption(MahjongPlayOption optionData)
        {
            if (optionData == null) return;

            //被前置状态
            int targetState = GetFrontState(optionData);

            //被互斥状态
            int mutexMyState = GetMutexState(optionData);
            if(mutexMyState != -1 && targetState != (int)OptionState.Hide) //如果被前置状态是 隐藏 则互斥无效
            {
                targetState = mutexMyState;
            }

            OptionConditionInfo condition = optionData.ConditionInfo;

            //改变值时 支持多种改变
            if (targetState == (int)OptionState.ChangeValue)
            {
                condition.isChanged = false;
            }

            //已经改变过 或者 状态一样的时候 结束
            if (condition.isChanged || targetState == optionData.ConditionInfo.myState)
            {
                return;
            }

            condition.isChanged = true;
            condition.myState = targetState;

            if(targetState == (int)OptionState.Hide || targetState == (int)OptionState.NoClick)
            {
                optionData.IsSelected = false;
            }

            //遍历自己所影响的 所有选项
            List<int> impactIdList = GetImpactIdListById(optionData.OptionId);

            if (impactIdList == null) return;

            int count = impactIdList.Count;
            for (int i = 0; i < count; i++)
            {
                int optionId = impactIdList[i];
                RecursionImpactOption(GetOptionDataById(optionId));
            }
        }

        //获取被前置的状态 (并且改变加减号的值)
        private int GetFrontState(MahjongPlayOption optionData)
        {
            int targetState = -1; ; //改变成什么状态

            OptionConditionInfo optionCondition = optionData.ConditionInfo;

            List<List<AppointData>> frontList = optionCondition.GetFrontList();

            if (frontList == null) return (int)OptionState.Show; //无前置条件

            int count = frontList.Count;
            for (int i = 0; i < count; i++)
            {
                List<AppointData> andFontList = frontList[i]; //开启条件列表 *-》每个索引i之间是"或"关系 *-》每个索引里的List是"并且"关系
                int andCount = andFontList.Count;
                
                int myState = -1; // 记录 不显示状态(隐藏或置灰)

                for (int j = 0; j < andCount; j++)
                {
                    AppointData appointData = andFontList[j]; 

                    MahjongPlayOption frontOptionData = GetOptionDataById(appointData.frontID); 

                    bool isPassFront = IsPassConditionTest(frontOptionData, appointData); //是否通过 前置条件

                    //前置条件未通过
                    if (!isPassFront)
                    {
                        myState = -1;
                        break;
                    }

                    switch (appointData.myState)
                    {
                        case (int)OptionState.Hide:
                            myState = (int)OptionState.Hide;
                            break;
                        case (int)OptionState.ChangeValue:
                            optionData.ChangeValueOption.ChangeAllValue(appointData.minValue, appointData.maxValue, appointData.defaultValue);
                            myState = (int)OptionState.ChangeValue;
                            break;
                        case (int)OptionState.NoClick:
                            //为了以(Hide 优先) 加此判断
                            if(myState == (int)OptionState.Hide)
                            {
                                myState = (int)OptionState.NoClick;
                            }
                            break;
                        default:
                            //为了以(关闭相关 优先) 加此判断
                            if (myState == -1)
                            {
                                myState = (int)OptionState.Show;
                            }
                            break;
                    }
                }

                if(myState != -1)
                {
                    targetState = myState;
                }

                //隐藏优先
                if(myState == (int)OptionState.Hide)
                {
                    targetState = myState;
                    break;
                }

            }

            //前置条件未通过
            if(targetState == -1)
            {
                targetState = (int)OptionState.NoClick;
            }

            return targetState;
        }

        //获取被互斥的状态
        private int GetMutexState(MahjongPlayOption optionData)
        {
            int targetState = -1; ; //改变成什么状态

            OptionConditionInfo optionCondition = optionData.ConditionInfo;

            List<List<AppointData>> mutexList = optionCondition.GetMutexList();

            if (mutexList == null || mutexList.Count == 0) return targetState;

            int count = mutexList.Count;
            for (int i = 0; i < count; i++)
            {
                List<AppointData> andMutexList = mutexList[i]; //开启条件列表 *-》每个索引i之间是"或"关系 *-》每个索引里的List是"并且"关系
                int andCount = andMutexList.Count;

                int myState = -1;

                for (int j = 0; j < andCount; j++)
                {
                    AppointData appointData = andMutexList[j];

                    MahjongPlayOption frontOptionData = GetOptionDataById(appointData.frontID);

                    bool isPassFront = IsPassConditionTest(frontOptionData, appointData);

                    if (!isPassFront)
                    {
                        myState = -1;
                        break;
                    }

                    switch (appointData.myState)
                    {
                        case (int)OptionState.Hide:
                            myState = (int)OptionState.Hide;
                            break;
                        case (int)OptionState.NoClick:
                            //为了(Hide 优先) 加此判断
                            if (myState != (int)OptionState.Hide)
                            {
                                myState = (int)OptionState.NoClick;
                            }
                            break;
                        default:
                            //为了(关闭相关优先) 加此判断
                            if (myState == -1)
                            {
                                myState = (int)OptionState.Show;
                            }
                            break;
                    }
                }

                if (myState != -1)
                {
                    targetState = myState;
                }

                if (myState == (int)OptionState.Hide)
                {
                    targetState = myState;
                    break;
                }
            }
            return targetState;
        }

        //是否满足条件
        private bool IsPassConditionTest(MahjongPlayOption frontOptionData, AppointData appointData)
        {
            if (frontOptionData == null) return false;

            switch (appointData.frontType)
            {
                case (int)OptionState.Show:
                    if(frontOptionData.IsSelected && frontOptionData.ConditionInfo.myState != (int)OptionState.NoClick && frontOptionData.ConditionInfo.myState != (int)OptionState.Hide)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                case (int)OptionState.Hide:
                    return !frontOptionData.IsSelected;
                case (int)OptionState.ChangeValue:
                    return frontOptionData.ChangeValueOption.value == appointData.frontValue;
                case (int)OptionState.NoClick:
                    break;
                case (int)OptionState.CanClick:
                    break;
            }
            return false;
        }

        #endregion

        //根据规则 返回 行列式数据
        public List<OneRowOptionsData> GetRowColumnOptionData(int ruleId)
        {
            List<MahjongPlayOption> currentRuleOptionList = GetOptionDataListByRuleId(ruleId);

            //重设 行数 改成从1开始
            ResetOptionRow(currentRuleOptionList);

            List<RowColumn> eachRowColumnCount = GetEachRowTotalColumnCount(currentRuleOptionList); //每行下的 列总数

            OneRowOptionsData[] eachRowData = GetEachRowOptionsArray(eachRowColumnCount);

            //生成 行列数据结构
            int optionCount = currentRuleOptionList.Count;
            for (int i = 0; i < optionCount; i++)
            {
                MahjongPlayOption optionData = currentRuleOptionList[i];
                if (optionData == null)
                    continue;

                var newOptionData = eachRowData[optionData.row - 1].OptionPlayArray[optionData.column - 1];
                if (newOptionData == null)
                {
                    eachRowData[optionData.row - 1].OptionPlayArray[optionData.column - 1] = optionData; //根据行和列 放到对应的位置
                }
                else if(optionData.showType != (int)e_OptionType.Menu)
                {
                    Debug.LogError(newOptionData.Name + "和" + optionData.Name + " 行列重复");
                }

                if (optionData.showType == (int)e_OptionType.Menu)
                {
                    eachRowData[optionData.row - 1].OptionPlayArray[optionData.column - 1].AddDropListOptionData(optionData);
                }
            }

            //每行对其方式设置 取第一个Option数据的对其方式
            int rowCount = eachRowData.Length;
            for (int i = 0; i < rowCount; i++)
            {
                if(eachRowData[i] != null && eachRowData[i].OptionPlayArray != null)
                {
                    int alignType = eachRowData[i].OptionPlayArray[0].alignType;
                    if(alignType == 0)
                    {
                        alignType = (int)AlignType.Left;
                    }
                    eachRowData[i].alignType = alignType;
                }
            }

            return new List<OneRowOptionsData>(eachRowData);
        }

        #region 生成行列式 辅助函数

        //根据行 生成数组 (每个索引代表行数)
        private OneRowOptionsData[] GetEachRowOptionsArray(List<RowColumn> eachRowColumnCount)
        {
            int count = eachRowColumnCount.Count;
            OneRowOptionsData[] array = new OneRowOptionsData[count];

            for (int i = 0; i < count; i++)
            {
                OneRowOptionsData oneRowOptionsData = new OneRowOptionsData();
                int column = eachRowColumnCount[i].maxColumn;
                oneRowOptionsData.totalColumnCount = column;

                oneRowOptionsData.OptionPlayArray = new MahjongPlayOption[column];
                array[i] = oneRowOptionsData;

            }
            return array;
        }
        public struct RowColumn
        {
            public int row;
            public int maxColumn;
        }

        //行数 设置成 从1开始
        public void ResetOptionRow(List<MahjongPlayOption> optionList)
        {
            List<RowColumn> list = GetEachRowTotalColumnCount(optionList);

            int optionCount = optionList.Count;
            int count = list.Count;

            for (int j = 0; j < optionCount; j++)
            {
                MahjongPlayOption optionData = optionList[j];

                for (int i = 0; i < count; i++)
                {
                    if (list[i].row == optionData.row)
                    {
                        optionData.row = i + 1;
                    }
                }
            }
        }

        //获取每行的总列数 并赋值显示类型
        private List<RowColumn> GetEachRowTotalColumnCount(List<MahjongPlayOption> optionList)
        {
            List<RowColumn> rowColumnList = new List<RowColumn>();

            //Dictionary<int, int> everyRowTotalColumnCount = new Dictionary<int, int>();

            int optionCount = optionList.Count;
            for (int j = 0; j < optionCount; j++)
            {
                MahjongPlayOption optionData = optionList[j];

                RowColumn rowColumnInfo = rowColumnList.Find(
                        (info) =>
                        {
                            if (info.row == optionData.row)
                            {
                                return true;
                            }
                            else
                            {
                                return false;
                            }
                        }
                    );

                if (rowColumnInfo.row == 0 && rowColumnInfo.maxColumn == 0)
                {
                    rowColumnInfo = new RowColumn();
                    rowColumnInfo.row = optionData.row;
                    rowColumnInfo.maxColumn = optionData.column;
                    rowColumnList.Add(rowColumnInfo);
                }
                else
                {
                    if (optionData.column > rowColumnInfo.maxColumn)
                    {
                        int index = rowColumnList.IndexOf(rowColumnInfo);
                        rowColumnInfo.maxColumn = optionData.column;
                        rowColumnList[index] = rowColumnInfo;
                    }
                }
            }
            rowColumnList.Sort(
                (left, right) =>
                {
                    if (left.row > right.row)
                    {
                        return 1;
                    }
                    else if (left.row == right.row)
                    {
                        return 0;
                    }
                    else
                    {
                        return -1;
                    }
                }
                );
            return rowColumnList;
        }
        #endregion

        #region 解析 前置 后置 相关
        public bool IsJsonString(string content)
        {
            if (!string.IsNullOrEmpty(content) && content.Length > 0)
            {
                if (content.IndexOf('{') >= 0)
                {
                    return true;
                }
            }
            return false;
        }

        private bool IsArrayString(string content)
        {
            if (!string.IsNullOrEmpty(content) && content.Length > 0)
            {
                if (content.IndexOf('[') >= 0)
                {
                    return true;
                }
            }
            return false;
        }

        //解析并记录 所影响的选项ID
        public void AnalysisIdListAndAdd(MahjongPlayOption optionData, string content, bool isMutex)
        {
            List<List<int>> idList = StringToIntListList(content);
            for (int i = 0; i < idList.Count; i++)
            {
                List<int> AndIdList = idList[i]; //AndIdList = "并且"关系ID列表   每个i之间是 "或"关系
                List<AppointData> appointList = new List<AppointData>();
                for (int j = 0; j < AndIdList.Count; j++)
                {

                    int frontId = AndIdList[j];
                    AppointData appointData = new AppointData();
                    appointData.frontType = 1;
                    appointData.frontID = frontId;
                    appointData.myState = isMutex ? 4 : 5; //4 = 置灰, 5 = 可点击

                    appointList.Add(appointData);

                    AddImpactOptionId(frontId, optionData.OptionId); //添加到 一个选项所影响的ID列表中
                }
                if (isMutex)
                {
                    optionData.ConditionInfo.AddMutexList(appointList);
                }
                else
                {
                    optionData.ConditionInfo.AddFrontList(appointList);
                }
            }
        }

        //解析并记录 所影响的选项ID
        public void AnalysisAppointDataDictAndAdd(MahjongPlayOption optionData, string content, bool isMutex)
        {
            //int = 前置frontId  List<AppointData> = 被同一个前置影响的 所有状态（比如 前置是加减号 前置的值1或2或3时 改变自己的值不同）
            List<List<AppointData>> appListList = StringToIntAppointData(content);

            if (appListList == null || appListList.Count == 0) return;

            int count = appListList.Count;
            for(int i = 0; i < count; i++)
            {
                List<AppointData> appointList = appListList[i];
                for (int j = 0; j < appointList.Count; j++)
                {
                    int frontId = appointList[j].frontID;

                    List<AppointData> andAppointList = new List<AppointData>(); //"并且"关系的 列表
                    andAppointList.Add(appointList[j]);

                    if (isMutex)
                    {
                        optionData.ConditionInfo.AddMutexList(andAppointList);
                    }
                    else
                    {
                        optionData.ConditionInfo.AddFrontList(andAppointList);
                    }

                    AddImpactOptionId(frontId, optionData.OptionId); //添加到 一个选项所影响的ID列表中
                }
            }
        }

        public class AnalysisJsonClass
        {
            public int[] IdArray;
            public AppointData[] appointDataArray;
        }
        // | 并且, ; 或
        private List<List<int>> StringToIntListList(string content)
        {
            List<List<int>> result = new List<List<int>>();
            if (!string.IsNullOrEmpty(content) && content.Length > 0)
            {
                //是否是数组形式
                if (IsArrayString(content))
                {
                    string[] idlistStringList = content.Split(';');
                    for (int i = 0; i < idlistStringList.Length; i++)
                    {
                        string idListString = idlistStringList[i];
                        if (!string.IsNullOrEmpty(idListString))
                        {
                            string idListJsonString = "{" + string.Format("\"IdArray\":{0}", idListString) + "}"; //组织成Json字符串
                            AnalysisJsonClass jsonClass = JsonUtility.FromJson<AnalysisJsonClass>(idListJsonString);

                            List<int> res = null;
                            if(jsonClass.IdArray != null)
                            {
                                res = new List<int>(jsonClass.IdArray);
                            }

                            if (res != null)
                                result.Add(res);
                        }
                    }
                }
                else
                {
                    string[] idlist = content.Split(';');
                    for (int i = 0; i < idlist.Length; i++)
                    {
                        if (!string.IsNullOrEmpty(idlist[i]))
                        {

                            string[] strlist = idlist[i].Split(',');
                            //=> 判断是否是json格式
                            if (strlist.Length > 1)
                            {
                                return result;
                            }

                            string[] list = idlist[i].Split('|');
                            List<int> res = new List<int>();
                            for (int n = 0; n < list.Length; n++)
                            {
                                if (!string.IsNullOrEmpty(list[n]))
                                {
                                    int tempId = 0;
                                    if (int.TryParse(list[n], out tempId) && tempId > 0)
                                    {
                                        if (res.IndexOf(tempId) == -1)
                                        {
                                            res.Add(tempId);
                                        }
                                    }
                                }
                            }
                            if (res.Count > 0)
                                result.Add(res);
                        }
                    }
                }
            }
            return result;
        }

        private List<List<AppointData>> StringToIntAppointData(string content)
        {
            List<List<AppointData>> result = new List<List<AppointData>>();
            if (!string.IsNullOrEmpty(content) && content.Length > 0)
            {
                //是否是数组形式
                if(IsArrayString(content))
                {
                    string[] applistStringList = content.Split(';');
                    for (int i = 0; i < applistStringList.Length; i++)
                    {
                        string appListString = applistStringList[i];
                        if (!string.IsNullOrEmpty(appListString))
                        {
                            string appListJsonString = "{" + string.Format("\"appointDataArray\":{0}", appListString) + "}"; //组织成Json字符串
                            AnalysisJsonClass jsonClass = JsonUtility.FromJson<AnalysisJsonClass>(appListJsonString);

                            List<AppointData> res = null;
                            if (jsonClass.appointDataArray != null)
                            {
                                res = new List<AppointData>(jsonClass.appointDataArray);
                            }

                            if (res != null)
                                result.Add(res);
                        }
                    }
                }
                else
                {
                    string[] idlist = content.Split(';');
                    for (int i = 0; i < idlist.Length; i++)
                    {
                        if (!string.IsNullOrEmpty(idlist[i]))
                        {
                            string[] list = idlist[i].Split('|');
                            List<AppointData> res = new List<AppointData>();
                            for (int n = 0; n < list.Length; n++)
                            {
                                if (!string.IsNullOrEmpty(list[n]))
                                {
                                    if (list[n].IndexOf('{') >= 0)
                                    {
                                        AnalysisJson(list[n], ref res);
                                    }
                                    else
                                    {
                                        AnalysisId(list[n], ref res);
                                    }
                                }
                            }
                            if (res.Count > 0)
                            {
                                result.Add(res);
                            }
                        }
                    }
                }
            }
            return result;
        }

        private void AnalysisId(string idStr, ref List<AppointData> list)
        {
            int tempId = 0;
            if (int.TryParse(idStr, out tempId) && tempId > 0)
            {
                var newData = new AppointData();
                newData.InitDefault(tempId);

                var item = list.Find((find) =>
                {
                    return newData.Equals(find);
                });
                if (item == null)
                {
                    list.Add(newData);
                }
            }
        }
        private void AnalysisJson(string jsonStr, ref List<AppointData> list)
        {
            try
            {
                AppointData newData = JsonUtility.FromJson<AppointData>(jsonStr);
                var item = list.Find((find) =>
                {
                    return newData.Equals(find);
                });
                if (item == null)
                {
                    list.Add(newData);
                }
            }
            catch
            {
                Debug.LogError("玩法规则解析出错" + jsonStr);
            }
        }
        #endregion

        /// <summary>
        /// 导入选中列表
        /// </summary>
        /*
        public void ImportSelectedList(List<int> selectOptionIds)
        {
            ClearSelected();

            for (int i = 0; i < selectOptionIds.Count; i++)
            {
                MahjongPlayOption optionData = GetOptionDataById(selectOptionIds[i]);

                if (optionData != null)
                {
                    optionData.IsSelected = true;
                }
            }
        }
        */

        public void ImportSelectedList(List<RulerItem> selectOptionIds)
        {
            ClearSelected();
            for (int i = 0; i < selectOptionIds.Count; i++)
            {
                MahjongPlayOption optionData = GetOptionDataById(selectOptionIds[i].RulerConfigID);

                if (optionData != null)
                {
                    if (optionData.showType == (int)e_OptionType.ChangeValue)
                    {
                        optionData.ChangeValueOption.SetValue(selectOptionIds[i].RulerValue);
                    }

                    //星星点亮数据
                    if(optionData.ParamType == 4)
                    {
                        if(optionData.StarDataList != null && optionData.DefaultStarData.configId != selectOptionIds[i].StarConfigId)
                        {
                            int count = optionData.StarDataList.Count;
                            for(int j = 0; j < count; j++)
                            {
                                optionData.StarDataList[j].isSelected = true;
                                if(optionData.StarDataList[j].configId == selectOptionIds[i].StarConfigId)
                                {
                                    break;
                                }
                            }
                        }
                    }
                    optionData.IsSelected = true;
                }
            }
        }

    }

    public class OneRowOptionsData
    {
        public int alignType; //对其方式
        public int totalColumnCount; //总列数
        public MahjongPlayOption[] OptionPlayArray; //当前行的 选项列表
    }
}