 
using System;
/**
* @Author YQC
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
    /// 主玩法 1
    /// </summary>
    public class MahjongPlay : IComparable<MahjongPlay>
    {
        public enum MahjongPlayType
        {
            Fashion = 1,    //流行
            Region = 2,     //地方
        }
        //唯一id
        public int ConfigId;
        //玩法ID
        public int PlayId;
        //玩法类型
        public MahjongPlayType MjType;
        //名字
        public string Name;
        //地区ID
        public int RegionID;
        //城市ID
        public int CityID;
        //排序ID
        public int SortID;
        //规则列表
        public List<MahjongPlayRule> RuleList;
        //规则描述
        public string Desc = string.Empty;

        public bool IsSelected;

        public MahjongPlayOptionLogic OptionLogic = new MahjongPlayOptionLogic(); //选项处理逻辑

        public MahjongPlay(int ConfigId, int PlayId, MahjongPlayType MjType, string Name, int RegionID, int CityID, int SortID)
        {
            this.ConfigId = ConfigId;
            this.PlayId = PlayId;
            this.MjType = MjType;
            this.Name = Name;
            this.RegionID = RegionID;
            this.CityID = CityID;
            this.SortID = SortID;
            RuleList = new List<MahjongPlayRule>();
        }

        public void Sort()
        {
            if(this.RuleList != null && this.RuleList.Count > 0)
            {
                this.RuleList.Sort(
                    (left, right)=>
                    {
                        if (left.RuleSort > right.RuleSort)
                        {
                            return 1;
                        }
                        else if (left.RuleSort == right.RuleSort)
                            return 0;
                        {
                            return -1;
                        }
                    }
                    );

                for (int i = 0; i < this.RuleList.Count; i++)
                {
                    //this.RuleList[i].Sort();
                }

                //for (int i = 0; i < this.RuleList.Count; i++)
                //{
                //    //this.RuleList[i].Sort();
                //}
            }
        }

        public int CompareTo(MahjongPlay other)
        {
            int num = MemoryData.MahjongPlayData.GetPlayTime(this.ConfigId);
            int next = MemoryData.MahjongPlayData.GetPlayTime(other.ConfigId);
            if (num < next) return 1;
            else if (num > next) return -1;

            if (this.SortID < other.SortID) return -1;
            else if (this.SortID > other.SortID) return 1;

            if (this.ConfigId < other.ConfigId) return -1;
            else if (this.ConfigId > other.ConfigId) return 1;

            return 0;
        }

        public static void SortPlayList(List<MahjongPlay> playList)
        {
            if (playList == null || playList.Count == 0) return;

            playList.Sort();
            for (int i = 0; i < playList.Count; i++)
            {
                playList[i].Sort();
            }
        }
    }

    /// <summary>
    /// 游戏玩法的次数
    /// </summary>
    [Serializable, XmlRoot("PlayMahjongTime")]
    public class PlayMahjongTime
    {
        //保存记录玩法次数的列表
        public List<int> PlayTimeList;
        //保存游戏玩法选项的列表
        public List<SelectedPlay> PlaySetDataList;
        //保存最近游戏玩法选项的列表
        public List<SelectedPlay> PlayIDList;
    }
    public class MahjongPlayData
    {
        //流行玩法列表
        private List<MahjongPlay> _fashionPlayList;
        //地区玩法列表
        private List<MahjongPlay> _regionPlayList;
        //所有的规则
        public Dictionary<int, MahjongRulerTypeConfig> AllRulerTypeMap;
        //所有的玩法
        private Dictionary<int, MahjongPlay> AllPlayMap;
        //所有的选项
        private Dictionary<string, MahjongPlayOption> AllPlayOptionMap;

        //玩法的次数相关
        private PlayMahjongTime playTime;
        private string SAVE_PLAY_TIME = "SAVE_PLAY_TIME";

        #region 选中
        /// <summary>
        /// 根据玩法取得选中的内容
        /// </summary>
        /// <param name="select"></param>
        /// <returns></returns>
        public SelectedPlay GetSelected(MahjongPlay select)
        {
            SelectedPlay result = new SelectedPlay();
            //result.SelectedList = new List<int>();
            result.SelectedItemList = new List<RulerItem>();

            if (select == null) return result;

            SelectedPlay data = GetPlaySet(select.ConfigId);
            if (data != null)
            {
                result.ViewScore = data.ViewScore;
            }
            else
            {
                result.ViewScore = true;
            }

            if (select != null)
            {
                result.configID = select.ConfigId;
                //for (int i = 0; i < select.RuleList.Count; i++)
                //{
                //    var optionList = select.OptionLogic.GetSelectOptionList();
                //    for (int j = 0; j < optionList.Count; j++)
                //    {
                //        result.SelectedList.Add(optionList[j].OptionId);
                //    }
                //}

                var optionList = select.OptionLogic.GetSelectOptionList();
                for (int j = 0; j < optionList.Count; j++)
                {
                    int value = optionList[j].showType == (int)e_OptionType.ChangeValue ? optionList[j].ChangeValueOption.value : 0;
                    RulerItem rule = new RulerItem();
                    rule.RulerConfigID = optionList[j].OptionId;
                    rule.RulerValue = value;

                    if(optionList[j].ParamType == 4)
                    {
                        rule.StarConfigId = select.OptionLogic.GetStarConfigIdByOptionId(optionList[j].OptionId);
                    }

                    result.SelectedItemList.Add(rule);
                }
            }
            return result;
        }

        /// <summary>
        /// 取得选中的玩法
        /// </summary>
        /// <returns></returns>
        public MahjongPlay GetSelectMainPlay()
        {
            for (int i = 0; i < _fashionPlayList.Count; ++i)
            {
                if (_fashionPlayList[i].IsSelected)
                    return _fashionPlayList[i];
            }

            for (int i = 0; i < _regionPlayList.Count; ++i)
            {
                if (_regionPlayList[i].IsSelected)
                    return _regionPlayList[i];
            }
            return null;
        }

        /// <summary>
        /// 选中一个玩法
        /// </summary>
        public void Selected(MahjongPlay play)
        {
            for (int i = 0; i < _fashionPlayList.Count; ++i)
            {
                _fashionPlayList[i].IsSelected = false;
            }
            for (int i = 0; i < _regionPlayList.Count; ++i)
            {
                _regionPlayList[i].IsSelected = false;
            }
            play.IsSelected = true;
        }
        #endregion

        #region 玩法

        /// <summary>
        /// 地区玩法列表
        /// </summary>
        public List<MahjongPlay> RegionPlayList
        {
            get
            {
                return _regionPlayList;
            }
        }

        /// <summary>
        /// 流行玩法列表
        /// </summary>
        public List<MahjongPlay> FashionPlayList
        {
            get
            {
                _fashionPlayList.Sort();
                return _fashionPlayList;
                //return GetPlayListByRegionId(_rightList, MemoryData.PlayerData.MyPlayerModel.PlayerDataBase.RegionID);
            }
        }

        /// <summary>
        /// 本地玩法列表 (根据用户选择的地区筛选)
        /// </summary> 
        public List<MahjongPlay> LocalRegionPlayList
        {
            get
            {
                return GetPlayListByRegionId(_regionPlayList, MemoryData.PlayerData.MyPlayerModel.PlayerDataBase.RegionID);
            }
        }

        /// <summary>
        /// 本地玩法与流行玩法列表总和
        /// </summary>
        public List<MahjongPlay> LocalRegionAndFashionPlayList
        {
            get
            {
                List<MahjongPlay> result = new List<MahjongPlay>();
                foreach (var item in FashionPlayList)
                {
                    result.Add(item);
                }
                foreach (var item in LocalRegionPlayList)
                {
                    result.Add(item);
                }

                return result;
            }
        }

        /// <summary>
        /// 根据省份ID 取得麻将列表
        /// </summary>
        /// <param name="playList">列表</param>
        /// <param name="regionID">地区ID</param>
        /// <returns></returns>
        public List<MahjongPlay> GetPlayListByRegionId(List<MahjongPlay> playList, int regionID)
        {
            List<MahjongPlay> result = new List<MahjongPlay>();
            for (int i = 0; i < playList.Count; i++)
            {
                if (playList[i].RegionID == regionID)
                {
                    result.Add(playList[i]);
                }
            }
            result.Sort();
            return result;
        }

        /// <summary>
        /// 取得麻将玩法
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public MahjongPlay GetMahjongPlayByConfigId(int ConfigId)
        {
            if (AllPlayMap != null && AllPlayMap.ContainsKey(ConfigId))
            {
                return AllPlayMap[ConfigId];
            }
            return null;
        }

        /// <summary>
        /// 取得唯一CongfigID 如果没有或者不是唯一的返回0
        /// </summary>
        /// <returns></returns>
        public int GetMahjongPlayOnlyConfigId()
        {
            if(this.AllPlayMap.Count == 1)
            {
                if (this._fashionPlayList.Count > 0)
                {
                    return _fashionPlayList[0].ConfigId;
                }
                if(this._regionPlayList.Count > 0)
                {
                    return _regionPlayList[0].ConfigId;
                }
            }
            return 0;
        }

        /// <summary>
        /// 添加麻将玩法
        /// </summary>
        /// <param name="play"></param>
        public void AddMahjongPlay(MahjongPlay play)
        {
            if (AllPlayMap == null)
                AllPlayMap = new Dictionary<int, MahjongPlay>();

            if (!AllPlayMap.ContainsKey(play.ConfigId))
            {
                AllPlayMap.Add(play.ConfigId, play);
            }
            else
            {
                AllPlayMap[play.ConfigId] = play;
            }
        }

        /// <summary>
        /// 取得玩法说明
        /// </summary>
        public string GetMahjongPlayDesc(int configId)
        {
            if (this.AllPlayMap != null && this.AllPlayMap.ContainsKey(configId))
            {
                return AllPlayMap[configId].Desc;
            }
            return string.Empty;
        }

        #endregion

        #region 选项
        /// <summary>
        /// 取得选项字典 的key
        /// </summary>
        private string GetOptionMapKey(int configId, int optionId)
        {
            return configId + "_" + optionId;
        }

        /// <summary>
        /// 添加选项
        /// </summary>
        /// <param name="option"></param>
        public void AddMahjongPlayOption(MahjongPlayOption option, int configId)
        {
            if (option == null) return;
            string key = GetOptionMapKey(configId, option.OptionId);
            if (AllPlayOptionMap == null)
                AllPlayOptionMap = new Dictionary<string, MahjongPlayOption>();

            if (AllPlayOptionMap.ContainsKey(key))
            {
                AllPlayOptionMap[key] = option;
            }
            else
            {
                AllPlayOptionMap.Add(key, option);
            }
        }

        /// <summary>
        /// 根据唯一ID与选项idList 取得选项名称List
        /// </summary>
        /// <param name="configId">玩法唯一ID</param>
        /// <param name="optionIds">选项ID列表</param>
        /// <returns>选项名称列表</returns>
        public List<string> GetMahjongPlayOptionNames(int configId, List<RulerItem> optionItems)
        {
            List<string> result = new List<string>();

            var play = GetMahjongPlayByConfigId(configId);

            if (play != null)
            {
                //这样一级一级循环而不使用GetMahjongPlayOption 是因为要有顺序的....... 不敢保证后端传来的数据是有序的
                //规则
                #region 旧排序
                /*
                for (int i = 0; i < play.RuleList.Count; ++i)
                {
                    var optionList = play.OptionLogic.GetAllOptionData();

                    for (int k = 0; k < optionList.Count; k++)
                    {
                        var temp = optionItems.Find((findItem) =>
                        {
                            return findItem.RulerConfigID == optionList[k].OptionId;
                        });
                        if (temp != null)
                        {
                            string name = optionList[k].Name;
                            if (name.IndexOf("(") >= 0)
                            {
                                name = name.Substring(0, name.IndexOf("("));
                            }

                            //判断如果有分数的话
                            if (optionList[k].ParamType == 2)
                            {
                                name += "(" + temp.RulerValue + ")";
                            }

                            //带星星的选项
                            if(optionList[k].ParamType == 4)
                            {
                                OptionStarData starData = play.OptionLogic.GetStarDataByOptionId(temp.RulerConfigID);
                                name = starData.name;
                            }

                            if (result.IndexOf(name) == -1)
                            {
                                result.Add(name);
                            }
                        }
                     }
                }
                */
                #endregion
                int count = optionItems.Count;
                for (int i = 0; i < count; i++)
                {
                    RulerItem rulerItem = optionItems[i];
                    MahjongPlayOption option = GetMahjongPlayOption(configId, rulerItem.RulerConfigID);

                    string name = option.Name;
                    if (name.IndexOf("(") >= 0)
                    {
                        name = name.Substring(0, name.IndexOf("("));
                    }

                    //判断如果有分数的话
                    if (option.ParamType == 2)
                    {
                        name += "(" + rulerItem.RulerValue + ")";
                    }

                    //带星星的选项
                    if (option.ParamType == 4)
                    {
                        OptionStarData starData = play.OptionLogic.GetStarDataByOptionId(rulerItem.RulerConfigID);
                        name = starData.name;
                    }

                    if (result.IndexOf(name) == -1)
                    {
                        result.Add(name);
                    }
                }
            }
            return result;
        }
        public string[] GetMahjongPlayOptionStr(int configId, List<RulerItem> optionItems, string appendString)
        {
            return GetMahjongPlayOptionStr(configId, optionItems, 18, 2, true, appendString);
        }
        public string[] GetMahjongPlayOptionStr(int configId, List<RulerItem> optionItems, int fontMaxLength = 18, int maxLine = 2, bool isShowPlayName = true, string appendString = "  ")
        {
            SortOption(ref optionItems, configId);

            string[] result = new string[maxLine];
            for (int i = 0; i < result.Length; i++)
            {
                result[i] = string.Empty;
            }
            var play = GetMahjongPlayByConfigId(configId);
            if (play != null)
            {
                StringBuilder tempSb = new StringBuilder();
                if (isShowPlayName)
                {
                    //玩法
                    tempSb.Append(play.Name).Append(appendString);
                }

                //取得所有选项名字
                List<string> optionNames = this.GetMahjongPlayOptionNames(configId, optionItems);
                for (int i = 0; i < optionNames.Count; i++)
                {
                    //判断这一行是否够放下此选项名称
                    bool flag = GetMahjongPlayOptionStr_AppString(ref tempSb, optionNames[i], fontMaxLength);
                    if (!flag)
                    {
                        for (int r = 0; r < result.Length; ++r)
                        {
                            if (result[r].Length == 0)
                            {
                                result[r] = tempSb.ToString().Trim();
                                tempSb.Length = 0;
                                GetMahjongPlayOptionStr_AppString(ref tempSb, optionNames[i], fontMaxLength);
                                break;
                            }
                        }
                    }
                }
                if (tempSb.Length > 0)
                {
                    for (int r = 0; r < result.Length; ++r)
                    {
                        if (result[r].Length == 0)
                        {
                            result[r] = tempSb.ToString().Trim();
                            break;
                        }
                    }
                }
            }
            return result;
        }

        private bool GetMahjongPlayOptionStr_AppString(ref StringBuilder sb, string name, int fontMaxLength)
        {
            if (sb.Length + name.Length > fontMaxLength)
            {
                return false;
            }
            sb.Append(name).Append(" ");
            return true;
        }

        /// <summary>
        /// 取得选项
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public MahjongPlayOption GetMahjongPlayOption(int configId, int optionId)
        {
            string key = GetOptionMapKey(configId, optionId);
            if (AllPlayOptionMap != null && AllPlayOptionMap.ContainsKey(key))
            {
                return AllPlayOptionMap[key];
            }
            return null;
        }
        #endregion

        #region 数据解析

        public void ProtoToData(List<Msg.RulerTypeConfig> list)
        {
            if (AllRulerTypeMap == null)
                AllRulerTypeMap = new Dictionary<int, MahjongRulerTypeConfig>();
            for (int i = 0; i < list.Count; ++i)
            {
                if (AllRulerTypeMap.ContainsKey(list[i].RulerType))
                {
                    AllRulerTypeMap[list[i].RulerType] = new MahjongRulerTypeConfig(list[i]);
                }
                else
                {
                    AllRulerTypeMap.Add(list[i].RulerType, new MahjongRulerTypeConfig(list[i]));
                }
            }
        }
        public void ProtoToData(List<Msg.MjGameConfig> list)
        {
            if (_fashionPlayList == null)
                _fashionPlayList = new List<MahjongPlay>();
            if (_regionPlayList == null)
                _regionPlayList = new List<MahjongPlay>();


            for (int i = 0; i < list.Count; ++i)
            {
                #region 玩法=======================================================
                //麻将玩法
                MahjongPlay mjPlay = GetMahjongPlayByConfigId(list[i].ConfigID);

                MahjongPlay.MahjongPlayType mjType = (MahjongPlay.MahjongPlayType)list[i].MjType;
                if (mjPlay == null)
                {
                    //麻将类型 流行还是 地方
                    mjPlay = new MahjongPlay(list[i].ConfigID, list[i].GameID, mjType, list[i].GameName, list[i].RegionID, list[i].CityID, list[i].SortID);

                    if (mjType == MahjongPlay.MahjongPlayType.Fashion) //流行
                    {
                        _fashionPlayList.Add(mjPlay);
                    }
                    else
                    {
                        _regionPlayList.Add(mjPlay);
                    }

                    AddMahjongPlay(mjPlay);
                }
                else
                {
                    mjPlay.ConfigId = list[i].ConfigID;
                    mjPlay.PlayId = list[i].GameID;
                    mjPlay.MjType = mjType;
                    mjPlay.Name = list[i].GameName;
                    mjPlay.RegionID = list[i].RegionID;
                    mjPlay.CityID = list[i].CityID;
                    mjPlay.SortID = list[i].SortID;
                }

                #endregion

                #region 规则=======================================================
                
                var protoRulerList = list[i].RulerList;
                protoRulerList.Sort(
                        (left, right) =>
                        {
                            if (left.RulerSort < right.RulerSort)
                            {
                                return -1;
                            }
                            else if (left.RulerSort == right.RulerSort)
                            {
                                return 0;
                            }
                            else
                            {
                                return 1;
                            }
                        }
                    );
                for (int j = 0; j < protoRulerList.Count; j++)
                {
                    if (protoRulerList[j].RulerIDList.Count == 0)
                    {
                        continue;
                    }

                    int rulerId = protoRulerList[j].RulerType;

                    var rule = mjPlay.RuleList.Find((findRule) =>
                    {
                        return findRule.id == rulerId;
                    });


                    //创建规则
                    if (rule == null)
                    {
                        rule = new MahjongPlayRule(rulerId, mjPlay);
                        mjPlay.RuleList.Add(rule);
                    }

                    int ruleIndex = 0;
                    for (int m = 0; m < mjPlay.RuleList.Count; m++)
                    {
                        if (mjPlay.RuleList[m].id == rulerId)
                        {
                            ruleIndex = m;
                        }
                    }

                    #region 选项=======================================================
                    var optionList = protoRulerList[j].RulerIDList;

                    optionList.Sort(
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

                    rule.row = rule.row + 1;
                    int addColumn = 0;
                    int optionCount = optionList.Count;
                    for (int k = 0; k < optionCount; k++)
                    {
                        MahjongPlayOptionLogic optionLogic = mjPlay.OptionLogic;

                        MahjongPlayOption option = new MahjongPlayOption(optionList[k].RulerID, optionList[k].RulerName, optionList[k].NormalSet, rule, optionList[k].AlignType, optionList[k].Row, optionList[k].Column, optionList[k].Sort);

                        //选项类型
                        option.showType = (int)protoRulerList[j].GroupType;
                        if (optionList[k].ParamShowType == 2 || optionList[k].ParamShowType == 3)
                        {
                            option.showType = (int)e_OptionType.ChangeValue;
                        }

                        option.InitData(optionList[k].RulerTypeID, optionList[k].ParamShowType, optionList[k].Param1, optionList[k].Param2, optionList[k].Param3);

                        #region 自动计算行列
                        int showCount = 4; //每行显示的数量

                        //计算行数
                        if ((k + addColumn) % showCount == 0 && k > 0)
                        {
                            rule.row += 1;
                        }

                        int column = (k + addColumn) % showCount + 1;

                        if (option.row == 0)
                        {
                            option.row = rule.row;
                        }

                        if (option.column == 0)
                        {
                            option.column = column;
                            if (option.showType == (int)e_OptionType.Menu)
                            {
                                option.column = 1;
                            }
                        }

                        //解析星星数据
                        if (optionList[k].ParamShowType == 4)
                        {
                            SetStarData(option);
                        }

                        //加减号选项 或 字数过长(非最后列)时 下一个选项 跳过一列
                        if (option.showType == (int)e_OptionType.ChangeValue || (option.Name.Length > 7 && (k + 1 + addColumn) % showCount != 0))
                        {
                            addColumn += 1;
                        }

                        #endregion

                        option.GroupId = (option.showType == (int)e_OptionType.One || option.showType == (int)e_OptionType.OtherMore) ? optionList[0].RulerID : 0;


                        optionLogic.AddOptionData(option);

                        AddMahjongPlayOption(option, mjPlay.ConfigId);

                        //排斥 判断是否是 json 结构
                        if (optionLogic.IsJsonString(optionList[k].MutexID))
                        {
                            optionLogic.AnalysisAppointDataDictAndAdd(option, optionList[k].MutexID, true);
                        }
                        else
                        {
                            optionLogic.AnalysisIdListAndAdd(option, optionList[k].MutexID, true);
                        }

                        //前置 判断是否是 json 结构
                        if (optionLogic.IsJsonString(optionList[k].FrontID))
                        {
                            optionLogic.AnalysisAppointDataDictAndAdd(option, optionList[k].FrontID, false);
                        }
                        else
                        {
                            optionLogic.AnalysisIdListAndAdd(option, optionList[k].FrontID, false);
                        }

                        //默认选中
                        if (optionList[k].NormalSet || optionList[k].ParamShowType == 3)
                        {
                            option.IsSelected = true;
                        }
                        
                    }


                    //-> 当单选 无默认选择处理
                    if (protoRulerList[j].GroupType == (int)e_OptionType.One)
                    {
                        bool isExistSelected = false;
                        for (int k = 0; k < optionCount; k++)
                        {
                            if(optionList[k].NormalSet)
                            {
                                isExistSelected = true;
                            }
                        }
                        if(!isExistSelected)
                        {
                            mjPlay.OptionLogic.GetOptionDataById(optionList[0].RulerID).IsSelected = true;
                        }
                    }

                    #endregion
                }

                SortOption(mjPlay);
                #endregion
                //mjPlay.OptionLogic.CreateOptionImpactStructure(); //创建选项之间条件关系结构
                #endregion
            }

            MahjongPlay.SortPlayList(_fashionPlayList);
            MahjongPlay.SortPlayList(_regionPlayList);
            EventDispatcher.FireSysEvent(GEnum.NamedEvent.SysData_MJPlayData_Update);
        }

        private void SortOption(MahjongPlay mjPlay)
        {
            #region 排序
            mjPlay.RuleList.Sort(
                    (left, right) =>
                    {
                        if (left.RuleSort < right.RuleSort)
                        {
                            return -1;
                        }
                        else if (left.RuleSort == right.RuleSort)
                        {
                            return 0;
                        }
                        else
                        {
                            return 1;
                        }
                    }
                );
            int sortIndex = 0;
            for (int i = 0; i < mjPlay.RuleList.Count; i++)
            {
                sortIndex += 100 * i;
                int ruleId = mjPlay.RuleList[i].id;
                List<OneRowOptionsData> eachRowOptionList = mjPlay.OptionLogic.GetRowColumnOptionData(ruleId);
                for (int j = 0; j < eachRowOptionList.Count; j++)
                {
                    for(int k = 0; k < eachRowOptionList[j].OptionPlayArray.Length; k++)
                    {
                        var optionData = eachRowOptionList[j].OptionPlayArray[k];
                        if(optionData != null)
                        {
                            sortIndex++;
                            optionData.Sort = sortIndex;
                        }

                    }
                }
            }
        }

        //选项 星星数据解析并赋值
        private void SetStarData(MahjongPlayOption optionData)
        {
            string[] eachStarStr = optionData.Name.Split('|');

            int length = eachStarStr.Length;
            for (int i = 0; i < length; i++)
            {
                string[] str = eachStarStr[i].Split('@');
                string name = str[0];
                int configId = int.Parse(str[1]);

                OptionStarData starData = new OptionStarData();
                starData.configId = configId;
                starData.name = name;
                starData.isSelected = false;

                if (i == 0)
                {
                    optionData.DefaultStarData = starData;
                }
                else
                {
                    optionData.AddStarData(starData);
                }
            }
        }

        public void XMLToData()
        {
            var xmldata = MemoryData.MahjongPlayDataXML.XMLPlayData;
            List<Msg.MjGameConfig> MjGameConfigList = new List<Msg.MjGameConfig>();
            List<Msg.RulerTypeConfig> RulerTypeConfigList = new List<Msg.RulerTypeConfig>();

            #region 转换到Proto
            for (int i = 0; i < xmldata.GameList.Count; ++i)
            {
                var gameConfig = xmldata.GameList[i];
                var proGameConfig = new Msg.MjGameConfig();
                MjGameConfigList.Add(proGameConfig);
                proGameConfig.ConfigID = gameConfig.ConfigID;
                proGameConfig.MjType = gameConfig.MjType;
                proGameConfig.GameID = gameConfig.GameID;
                proGameConfig.GameName = gameConfig.GameName;
                proGameConfig.RegionID = gameConfig.RegionID;
                proGameConfig.CityID = gameConfig.CityID;
                proGameConfig.SortID = gameConfig.SortID;

                for (int j = 0; j < gameConfig.RulerList.Count; j++)
                {
                    var rulerConfig = gameConfig.RulerList[j];
                    Msg.RulerConfig proRulerConfig = new Msg.RulerConfig();
                    proGameConfig.RulerList.Add(proRulerConfig);

                    proRulerConfig.RulerType = rulerConfig.RulerType;
                    proRulerConfig.GroupType = rulerConfig.GroupTypeDef;
                    proRulerConfig.RulerSort = rulerConfig.RulerSort;
                    var findResult = RulerTypeConfigList.Find((find) =>
                    {
                        return find.RulerType == rulerConfig.RulerType;
                    });
                    if (findResult == null)
                    {
                        var proRulerTypeConfig = new Msg.RulerTypeConfig();
                        RulerTypeConfigList.Add(proRulerTypeConfig);

                        proRulerTypeConfig.RulerType = rulerConfig.RulerType;
                        proRulerTypeConfig.RulerTypeName = rulerConfig.RulerTypeName;
                        proRulerTypeConfig.RulerTypeDesc = rulerConfig.RulerTypeDesc;
                        proRulerTypeConfig.RulerTypeSort = rulerConfig.RulerTypeSort;
                    }

                    for (int k = 0; k < rulerConfig.RulerData.Count; k++)
                    {
                        var rulerData = rulerConfig.RulerData[k];
                        //TODO XML读取空选项过滤
                        if (rulerData.RulerTypeID == 0)
                        {
                            continue;
                        }


                        Msg.RulerData proRulerData = new Msg.RulerData();
                        proRulerConfig.RulerIDList.Add(proRulerData);

                        //proRulerData.RulerID = rulerData.RulerID;
                        proRulerData.RulerID = rulerData.RulerConfigID;
                        proRulerData.RulerName = rulerData.RulerName;
                        proRulerData.RulerTypeID = rulerData.RulerTypeID;
                        proRulerData.ParamShowType = rulerData.ParamShowType;
                        proRulerData.NormalSet = rulerData.NormalSet == 1;
                        proRulerData.ParamType = rulerData.ParamType;
                        proRulerData.Sort = rulerData.Sort;
                        proRulerData.Param1 = rulerData.Param1;
                        proRulerData.Param2 = rulerData.Param2;
                        //proRulerData.Param3 = rulerData.Param3;
                        proRulerData.MutexID = rulerData.MutexID;
                        proRulerData.FrontID = rulerData.Front;

                        //-->添加字段(金龙)
                        proRulerData.AlignType = rulerData.AlignType;
                        proRulerData.Row = rulerData.Row;
                        proRulerData.Column = rulerData.Column;

                    }

                    for (int n = 0; n < proRulerConfig.RulerIDList.Count - 1; n++)
                    {
                        for (int m = 1; m < proRulerConfig.RulerIDList.Count; m++)
                        {
                            if (proRulerConfig.RulerIDList[n].Sort > proRulerConfig.RulerIDList[m].Sort)
                            {
                                Msg.RulerData data = proRulerConfig.RulerIDList[n];
                                proRulerConfig.RulerIDList[n] = proRulerConfig.RulerIDList[m];
                                proRulerConfig.RulerIDList[m] = data;
                            }
                        }
                    }
                }
            }
            #endregion 

            ProtoToData(RulerTypeConfigList);
            ProtoToData(MjGameConfigList);

            ReadSaveData();
            if (playTime != null)
            {
                for (int i = 0; i < playTime.PlaySetDataList.Count; i++)
                {
                    MahjongPlay data = GetMahjongPlayByConfigId(playTime.PlaySetDataList[i].configID);

                    if (data != null)
                    {
                        //if (CheckError(data.ConfigId, playTime.PlaySetDataList[i].SelectedItemList))
                        // continue;

                        data.OptionLogic.ClearSelected();
                        data.OptionLogic.ImportSelectedList(playTime.PlaySetDataList[i].SelectedItemList);
                        //data.OptionLogic.CreateOptionImpactStructure();
                        //创建结构
                    }
                }
            }
        }
        #endregion

        /// <summary>
        /// 构造
        /// </summary>
        public MahjongPlayData()
        {

        }

        private void InitPlayTime()
        {
            SAVE_PLAY_TIME = string.Format("SAVE_PLAY_TIME{0}", MemoryData.UserID);
            if (playTime == null)
            {
                playTime = PlayerPrefsTools.GetObject<PlayMahjongTime>(SAVE_PLAY_TIME);
            }
            if (playTime == null)
            {
                playTime = new PlayMahjongTime();
                playTime.PlayTimeList = new List<int>();
                playTime.PlaySetDataList = new List<SelectedPlay>();
                playTime.PlayIDList = new List<SelectedPlay>();
            }
        }

        /// <summary>
        /// 增加玩法选择的次数
        /// </summary>
        /// <param name="configID">玩法ID</param>
        public void AddPlayTime(int configID)
        {
            InitPlayTime();

            bool isHave = false;
            int index = 0;
            int num = 0;
            for (int i = 0; i < playTime.PlayTimeList.Count; i += 2)
            {
                if (playTime.PlayTimeList[i] == configID)
                {
                    isHave = true;
                    num = playTime.PlayTimeList[i + 1];
                    index = i;
                }
            }

            if (isHave)
            {
                num++;
                playTime.PlayTimeList[index + 1] = num;
            }
            else
            {
                playTime.PlayTimeList.Add(configID);
                playTime.PlayTimeList.Add(1);
            }
            PlayerPrefsTools.SetObject<PlayMahjongTime>(SAVE_PLAY_TIME, playTime, true);
        }

        /// <summary>
        /// 获取玩法选择的次数
        /// </summary>
        /// <param name="configID">玩法ID</param>
        /// <returns></returns>
        public int GetPlayTime(int configID)
        {
            InitPlayTime();

            if (playTime != null)
            {
                int num = 0;
                for (int i = 0; i < playTime.PlayTimeList.Count; i += 2)
                {
                    if (playTime.PlayTimeList[i] == configID)
                    {
                        num = playTime.PlayTimeList[i + 1];
                    }
                }

                return num;
            }
            return 0;
        }

        /// <summary>
        /// 保存玩法选项设置
        /// </summary>
        /// <param name="configID"></param>
        /// <param name="setData"></param>
        public void SavePlaySet(int configID, SelectedPlay setData)
        {
            InitPlayTime();

            bool isHave = false;
            int index = -1;

            for (int i = 0; i < playTime.PlaySetDataList.Count; i++)
            {
                if (playTime.PlaySetDataList[i].configID == configID)
                {
                    isHave = true;
                    index = i;
                }
            }

            if (isHave)
            {
                playTime.PlaySetDataList[index] = setData;
            }
            else
            {
                playTime.PlaySetDataList.Add(setData);
                index = playTime.PlaySetDataList.Count - 1;
            }

            SavePlayID(configID);

            if (playTime != null)
            {
                MahjongPlay data = GetMahjongPlayByConfigId(playTime.PlaySetDataList[index].configID);
                if (data != null)
                    data.OptionLogic.ImportSelectedList(playTime.PlaySetDataList[index].SelectedItemList);
            }

            PlayerPrefsTools.SetObject<PlayMahjongTime>(SAVE_PLAY_TIME, playTime, true);
        }
        public void SavePlaySet(int configID, MahjongPlay mahjongData)
        {
            SelectedPlay data = GetSelected(mahjongData);
            SavePlaySet(configID, data);
        }
        public void SavePlaySet(int configID, bool canSee, List<int> ruleList, List<RulerItem> ruleItemList = null)
        {
            SelectedPlay data = new SelectedPlay();
            //data.SelectedList = ruleList;
            data.ViewScore = canSee;
            data.configID = configID;
            data.SelectedItemList = ruleItemList;
            SavePlaySet(configID, data);
        }

        /// <summary>
        /// 获取玩法选项设置
        /// </summary>
        /// <param name="configID"></param>
        /// <param name="setData"></param>
        /// <returns></returns>
        public SelectedPlay GetPlaySet(int configID)
        {
            InitPlayTime();

            if (playTime != null)
            {
                bool isHave = false;
                int index = 0;

                for (int i = 0; i < playTime.PlaySetDataList.Count; i++)
                {
                    if (playTime.PlaySetDataList[i].configID == configID)
                    {
                        isHave = true;
                        index = i;
                    }
                }

                if (isHave)
                {
                    SelectedPlay selectData = playTime.PlaySetDataList[index];

                    if (CheckError(selectData.configID, selectData.SelectedItemList))
                    {
                        return null;
                    }

                    return selectData;
                }
            }

            return null;
        }

        private bool CheckError(int configID, List<RulerItem> ruleIDs)
        {
            for (int i = 0; i < ruleIDs.Count; i++)
            {
                int id = ruleIDs[i].RulerConfigID;
                if (MemoryData.MahjongPlayData.GetMahjongPlayOption(configID, id) == null)
                {
                    return true;
                }
            }
            return false;
        }

        public void ReadSaveData()
        {
            InitPlayTime();
        }

        /// <summary>
        /// 保存最近玩的列表
        /// </summary>
        /// <param name="configID"></param>
        public void SavePlayID(int configID)
        {
            InitPlayTime();

            int count = playTime.PlayIDList.Count;

            bool isDelet = false;
            if (count >= 10)
            {
                count -= 9;
                isDelet = true;
            }

            if (isDelet)
            {
                for (int i = 0; i < count; i++)
                {
                    playTime.PlayIDList.RemoveAt(0);
                }
            }

            SelectedPlay data = GetPlaySet(configID);
            playTime.PlayIDList.Add(data);
        }

        public SelectedPlay GetPlayID()
        {
            InitPlayTime();

            if (playTime.PlayIDList.Count > 0)
            {
                return playTime.PlayIDList[playTime.PlayIDList.Count - 1];
            }

            return null;
        }

        /// <summary>
        /// 排序
        /// </summary>
        public void SortOption(ref List<RulerItem> itemList, int configId)
        {
            int count = itemList.Count;
            itemList.Sort(
                (left, right)=>
                {
                    if (GetMahjongPlayOption(configId, left.RulerConfigID) == null || GetMahjongPlayOption(configId, right.RulerConfigID) == null)
                        return 0;

                    if (GetMahjongPlayOption(configId, left.RulerConfigID).Sort > GetMahjongPlayOption(configId, right.RulerConfigID).Sort)
                        return 1;
                    else if (GetMahjongPlayOption(configId, left.RulerConfigID).Sort == GetMahjongPlayOption(configId, right.RulerConfigID).Sort)
                        return 0;
                    else
                        return -1;
                }
                );
        }
        public void SortOption(ref List<int> itemList, int configId)
        {
            int count = itemList.Count;
            itemList.Sort(
                (left, right) =>
                {
                    if (GetMahjongPlayOption(configId, left) == null || GetMahjongPlayOption(configId, right) == null)
                        return 0;

                    if (GetMahjongPlayOption(configId, left).Sort > GetMahjongPlayOption(configId, right).Sort)
                        return 1;
                    else if (GetMahjongPlayOption(configId, left).Sort == GetMahjongPlayOption(configId, right).Sort)
                        return 0;
                    else
                        return -1;
                }
                );
        }

    }

    /// <summary>
    /// 选中的内容
    /// </summary>
    [Serializable, XmlRoot("SelectedPlay")]
    public class SelectedPlay
    {
        public int configID;
        //public List<int> SelectedList;
        public List<RulerItem> SelectedItemList;
        //是否可以让代理查看房间
        public bool ViewScore;
    }

    [Serializable, XmlRoot("RulerItem")]
    public class RulerItem
    {
        public int RulerConfigID;
        public int RulerValue;
        public int StarConfigId;
    }

    public class MahjongPlayDataXML
    {
        private PlayGameConfigData xmlPlayData;
        public PlayGameConfigData XMLPlayData
        {
            set { xmlPlayData = value; }
            get
            {
                if (xmlPlayData == null)
                {
                    xmlPlayData = new PlayGameConfigData();
                }
                return xmlPlayData;
            }
        }
    }
    
    #region 内存数据
    public partial class MKey
    {
        /// <summary>
        /// 游戏玩法数据
        /// </summary>
        public const string MJ_PLAY_DATA = "MJ_PLAY_DATA";
        public const string MJ_PLAY_DATA_XML = "MJ_PLAY_DATA_XML";
    }

    public partial class MemoryData
    {
        static public MahjongPlayData MahjongPlayData
        {
            get
            {
                MahjongPlayData data = MemoryData.Get<MahjongPlayData>(MKey.MJ_PLAY_DATA);
                if (data == null)
                {
                    data = new MahjongPlayData();
                    MemoryData.Set(MKey.MJ_PLAY_DATA, data);
                }
                return data;
            }
        }
        static public MahjongPlayDataXML MahjongPlayDataXML
        {
            get
            {
                MahjongPlayDataXML data = MemoryData.Get<MahjongPlayDataXML>(MKey.MJ_PLAY_DATA_XML);
                if (data == null)
                {
                    data = new MahjongPlayDataXML();
                    MemoryData.Set(MKey.MJ_PLAY_DATA_XML, data);
                }
                return data;
            }
        }
    }
    #endregion
}
