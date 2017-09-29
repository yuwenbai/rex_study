using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace projectQ
{

    public enum e_OptionType
    {
        One = 1,//单选
        More,//多选
        Menu,//下拉菜单
        OtherMore,//只能选一个或可不选（UI用多选框）   
        ChangeValue, //加减号
        Text, //文本
    }

    public class UICreateRoomRuleItem : MonoBehaviour
    {
        //选项相关
        public GameObject OptionRadio;
        public GameObject OptionCheckBox;
        public GameObject OptionDropList;
        public GameObject OptionChangeValue;
        public GameObject OptionText;
        public GameObject OptionStar;

        public GameObject RowPanel; 

        public UILabel TxtRuleName;
        public GameObject OptionsTable; //选项面板


        private MahjongPlayOptionLogic OptionLogic; //选项相关 逻辑和数据 （包括前置互斥关系处理）
        private Dictionary<GameObject, MahjongPlayOption> OptionDict = new Dictionary<GameObject, MahjongPlayOption>();
        private Dictionary<int, List<GameObject>> RadioDict = new Dictionary<int, List<GameObject>>();

        private UICreateRoomRule Parent; //主界面

        private MahjongPlayRule RuleData; //规则数据

        private List<OneRowOptionsData> EachRowOptionsDataList; //每行选项数据列表

        private bool isLock = false; //是否锁定

        private float posY; //每个选项的Y坐标

        private readonly Color NotClickColor = new Color(168 / 255f, 170 / 255f, 169 / 255f); //不可点击时的颜色

        private bool isUnifiedColumn = true; //是否统一列数

        public void Init(MahjongPlayRule data, UICreateRoomRule parent = null, bool isLock = false)
        {
            RuleData = data;
            Parent = parent;
            this.isLock = isLock;
            OptionLogic = RuleData.ParentPlay.OptionLogic;

            TxtRuleName.text = RuleData.Name;

            posY = RowPanel.GetComponent<UIWidget>().localSize.y / 2 * -1; //每行中间的坐标

            EachRowOptionsDataList = OptionLogic.GetRowColumnOptionData(RuleData.id); //返回行列式方式的数据
            //==> 创建每行选项
            if (EachRowOptionsDataList != null && EachRowOptionsDataList.Count > 0)
            {
                UITools.CreateChild<OneRowOptionsData>(OptionsTable.transform, null, EachRowOptionsDataList, CreateEachRowOptions);
                OptionsTable.GetComponent<UITable>().Reposition();
            }
            RefreshOptions(); //刷新 选项显示状态
        }

        //创建每行选项
        private void CreateEachRowOptions(GameObject obj, OneRowOptionsData oneRowOptionsData)
        {
            CreateOneRowOptions(obj, oneRowOptionsData); //创建每行选项
        }

        //修改当前选项所影响的选项 并 刷新
        private void ChangeImpactOptionAndRefresh(MahjongPlayOption optionData)
        {
            OptionLogic.ChangeImpactOption(optionData);
            RefreshAllRuleOptions();
            StartCoroutine(UITools.WaitExcution(CheckRadio, 0.01f));
        }

        //刷新所有规则下的选项
        private void RefreshAllRuleOptions()
        {
            if (Parent != null)
            {
                Parent.RefreshAllRuleOptions();
            }
        }

        //创建一行选项
        private void CreateOneRowOptions(GameObject optionParent, OneRowOptionsData oneRowOptionsData)
        {
            MahjongPlayOption[] optionDataArray = oneRowOptionsData.OptionPlayArray;

            int length = optionDataArray.Length;

            int column = isUnifiedColumn ? OptionLogic.GetMaxColumn() : length;

            float[] posXArray = CreateEachPosXArray(column, oneRowOptionsData.alignType, optionParent.GetComponent<UIWidget>().localSize.x);

            optionParent.transform.DestroyChildren();

            for (int i = 0; i < length; ++i)
            {
                var optionData = optionDataArray[i];

                if (optionData == null)
                    continue;

                GameObject go = null;

                switch (optionData.showType)
                {
                    case (int)e_OptionType.One: //单选

                        go = UITools.CloneObject(OptionRadio, optionParent);
                        go.SetActive(true);
                        InitOtherOption(go, optionData);

                        if(!RadioDict.ContainsKey(optionData.GroupId))
                        {
                            RadioDict[optionData.GroupId] = new List<GameObject>();
                        }
                        RadioDict[optionData.GroupId].Add(go);

                        break;

                    case (int)e_OptionType.More: //复选

                    case (int)e_OptionType.OtherMore: //复选 只能选一个

                        go = UITools.CloneObject(OptionCheckBox, optionParent);
                        go.SetActive(true);
                        InitOtherOption(go, optionData);
                        break;

                    case (int)e_OptionType.ChangeValue: //加减号

                        go = UITools.CloneObject(OptionChangeValue, optionParent);
                        go.SetActive(true);
                        InitChangeValue(go, optionData);
                        break;

                    case (int)e_OptionType.Menu: //下拉列表

                        go = UITools.CloneObject(OptionDropList, optionParent);
                        go.SetActive(true);
                        InitDropList(go, optionData);
                        break;

                    case (int)e_OptionType.Text: //文本
                        go = UITools.CloneObject(OptionText, optionParent);
                        go.SetActive(true);
                        InitText(go, optionData);
                        break;
                }

                //是否带星星
                if (optionData.ParamType == 4)
                {
                    CreateStar(go, optionData);
                }

                OptionDict[go] = optionData;

                //-> 根据计算的位置 排列
                /*
                int childCount = go.transform.childCount;
                float width = 0;
                for (int j = 0; j < childCount; j++)
                {
                    width += go.transform.GetChild(j).GetComponent<UIWidget>().localSize.x;
                }
                UIWidget widget = go.GetComponent<UIWidget>();
                widget.width = (int)width;
                */

                float posX = posXArray[optionData.column - 1];

                if (oneRowOptionsData.alignType == (int)AlignType.Center)
                    posX -= 220 / 2 - 20; //189 = 大概6个toggle字的宽度， 20 = toggle的一半宽度
                else
                    posX += 10;

                go.transform.localPosition = new Vector2(posX + 10, posY);
            }
        }

        #region 选项初始化
        //其他选项初始化
        private void InitOtherOption(GameObject go, MahjongPlayOption optionData)
        {
            UIToggle toggle = go.GetComponentInChildren<UIToggle>();
            UILabel label = go.transform.GetComponentInChildren<UILabel>();

            label.text = optionData.Name;
            label.color = GetColorByMode(optionData);

            NGUITools.UpdateWidgetCollider(label.gameObject);

            var box = label.GetComponent<BoxCollider>();
            if (box != null)
                box.size = new Vector2(box.size.x * 1.2f, box.size.y * 2f);

            toggle.group = optionData.GroupId;

            if (optionData.showType == (int)e_OptionType.OtherMore)
            {
                toggle.optionCanBeNone = true;
            }

            UIEventListener.Get(label.gameObject).onClick = (tempGo) =>
            {
                PlayMusic();
                toggle.value = !toggle.value;
            };

            UIEventListener.Get(go).onClick = (tempGo) =>
            {
                PlayMusic();
            };

            bool isCall = false;
            EventDelegate.Add(toggle.onChange, () => {
                if (isCall)
                {
                    OnToggleChange(optionData, toggle.value);
                }
                isCall = true;
            });
        }

        //加减号选项初始化
        private void InitChangeValue(GameObject go, MahjongPlayOption optionData)
        {
            UILabel label = go.transform.FindChild("Label").GetComponent<UILabel>();
            label.text = optionData.Name;

            //3 = 不显示复选框
            if (optionData.ParamType == 3)
            {
                go.transform.FindChild("ToggleRange").gameObject.SetActive(false);
                var pos = go.transform.FindChild("Label").localPosition;
                go.transform.FindChild("Label").localPosition = new Vector2(0, pos.y);
            }

            if (!isLock)
            {
                UIDefinedButton adbtn = go.transform.FindChild("Range/Bg/AddNumBtn").GetComponent<UIDefinedButton>();
                UIDefinedButton subtn = go.transform.FindChild("Range/Bg/SubNumBtn").GetComponent<UIDefinedButton>();

                UIEventListener.Get(adbtn.gameObject).onClick = (obj) =>
                {
                    if(optionData.IsSelected)
                    {
                        optionData.ChangeValueOption.AddValue();

                        int currentValue = -1;
                        if (optionData.ChangeValueOption != null)
                            currentValue = optionData.ChangeValueOption.value;

                        ChangeImpactOptionAndRefresh(optionData);
                    }
                };

                UIEventListener.Get(subtn.gameObject).onClick = (obj) =>
                {
                    if (optionData.IsSelected)
                    {
                        optionData.ChangeValueOption.SubValue();

                        int currentValue = -1;
                        if (optionData.ChangeValueOption != null)
                            currentValue = optionData.ChangeValueOption.value;

                        ChangeImpactOptionAndRefresh(optionData);
                    }
                };

                UIToggle toggle = go.GetComponentInChildren<UIToggle>();
                if (toggle != null)
                {
                    toggle.group = optionData.GroupId;

                    UIEventListener.Get(label.gameObject).onClick = (tempGo) =>
                    {
                        PlayMusic();
                        toggle.value = !toggle.value;
                    };

                    bool isCall = false;
                    EventDelegate.Add(toggle.onChange, () => {
                        if (isCall)
                        {
                            OnToggleChange(optionData, toggle.value);
                        }
                        isCall = true;
                    }
                    );
                }

                if (optionData.ParamType == 2)
                {
                    NGUITools.UpdateWidgetCollider(label.gameObject);
                    UIEventListener.Get(label.gameObject).onClick = (tempGo) =>
                    {
                        PlayMusic();
                        toggle.value = !toggle.value;
                    };
                }
            }
        }

        //文本
        private void InitText(GameObject go, MahjongPlayOption optionData)
        {
            UILabel label = go.GetComponentInChildren<UILabel>();
            label.text = optionData.Name;
        }

        //下拉列表初始化
        private void InitDropList(GameObject go, MahjongPlayOption optionData)
        {

            if (optionData.DropOptionList == null) return;

            List<object> listData = new List<object>();
            int defaultIndex = 0;
            int count = optionData.DropOptionList.Count;
            for (int i = 0; i < count; i++)
            {
                var temp = new OptionPopupItemData();
                listData.Add(temp);

                temp.optionPlay = optionData.DropOptionList[i];
                temp.IsSelected = optionData.DropOptionList[i].IsSelected;
                if (temp.IsSelected)
                {
                    defaultIndex = i;
                }
                temp.clickCallBack = (uiData) =>
                {
                    MahjongPlayOption select = uiData.optionPlay as MahjongPlayOption;
                    for (int j = 0; j < count; j++)
                    {
                        optionData.DropOptionList[j].IsSelected = optionData.DropOptionList[j].OptionId == select.OptionId;
                    }

                    OptionLogic.ChangeImpactOption(select);

                    StartCoroutine(UITools.WaitExcution(() => {
                        RefreshAllRuleOptions();
                    }, 0.2f));
                };
            }
            NGUIPopupList script = go.GetComponent<NGUIPopupList>();
            script.Init(listData, (popupListGO) => {
                PlayMusic();
            });
            script.Selected(defaultIndex);
        }

        //创建星星
        private void CreateStar(GameObject go, MahjongPlayOption optionData)
        {
            Transform starRange = go.transform.FindChild("Star");

            if (OptionStar == null || starRange == null) return;

            starRange.gameObject.SetActive(true);

            float starPosX = 0;

            int count = optionData.StarDataList.Count;
            for (int i = 0; i < count; i++)
            {
                OptionStarData starData = optionData.StarDataList[i];

                GameObject obj = UITools.CloneObject(OptionStar, starRange.gameObject);
                starData.star = obj;

                obj.transform.localPosition = new Vector2(starPosX, 0);
                starPosX += OptionStar.GetComponent<UIWidget>().localSize.x + 2;

                UIEventListener.Get(obj).onClick = (tempGo) =>
                {
                    PlayMusic();
                    OnClickStar(go, optionData, tempGo);
                };

                optionData.StarDataList[i].star = obj;

                obj.transform.FindChild("Select").gameObject.SetActive(starData.isSelected);
            }

            UILabel label = null;
            if (go.transform.FindChild("Label") != null)
            {
                label = go.transform.FindChild("Label").GetComponent<UILabel>();
            }
            else
            {
                label = go.GetComponentInChildren<UILabel>();
            }
            NGUITools.UpdateWidgetCollider(label.gameObject);
        }
        #endregion



        #region 选项显示刷新

        //刷新 当前规则下 所有选项
        public void RefreshOptions()
        { 
            foreach (var item in OptionDict)
            {
                switch (item.Value.showType)
                {
                    case (int)e_OptionType.Menu:
                        RefreshDropList(item.Key, item.Value);
                        break;
                    case (int)e_OptionType.ChangeValue:
                        RefreshChangeValue(item.Key, item.Value);
                        break;
                    default:
                        RefreshOther(item.Key, item.Value);
                        break;
                }

                if(item.Value.ParamType == 4)
                {
                    RefreshStar(item.Key, item.Value);
                }

                if (isLock)
                {
                    SetColliderEnableInChild(item.Key.transform, false);
                }
            }
        }

        //刷新 加减号
        private void RefreshChangeValue(GameObject obj, MahjongPlayOption optionData)
        {
            UILabel txtValue = obj.transform.FindChild("Range/Bg/Value").GetComponent<UILabel>();
            if (txtValue != null)
                txtValue.text = optionData.ChangeValueOption.value.ToString();

            UILabel label = obj.transform.FindChild("Label").GetComponent<UILabel>();
            label.color = GetColorByMode(optionData);

            UIDefinedButton addBtn = obj.transform.FindChild("Range/Bg/AddNumBtn").GetComponent<UIDefinedButton>();
            UIDefinedButton subBtn = obj.transform.FindChild("Range/Bg/SubNumBtn").GetComponent<UIDefinedButton>();

            if (optionData.ConditionInfo.myState == (int)OptionState.NoClick || isLock)
            {
                SetUIBtnEnable(addBtn, false);
                SetUIBtnEnable(subBtn, false);
            }
            else
            {
                bool isShowAdd = optionData.ChangeValueOption.value < optionData.ChangeValueOption.maxValue;
                bool isShowSub = optionData.ChangeValueOption.value > optionData.ChangeValueOption.minValue;
                SetUIBtnEnable(addBtn, isShowAdd);
                SetUIBtnEnable(subBtn, isShowSub);
                if (!isShowAdd && !isShowSub)
                {
                    label.color = NotClickColor;
                }
            }

            SetOptionState(obj, optionData);
        }
        //按钮置灰设置
        private void SetUIBtnEnable(UIDefinedButton btn, bool isEnabel)
        {
            int clickNum = -1;
            if (isEnabel)
                clickNum = 0;

            if (btn.gameObject.activeInHierarchy && btn.MaxClickCount != clickNum)
            {
                btn.MaxClickCount = clickNum;
                btn.isEnabled = isEnabel;
                btn.fireOnClickAction();
            }
        }

        //刷新 下拉列表
        private void RefreshDropList(GameObject obj, MahjongPlayOption optionData)
        {
            List<MahjongPlayOption> dropList = optionData.DropOptionList;

            if (dropList == null) return;

            bool isCanClick = true;

            int count = dropList.Count;
            for (int i = 0; i < count; i++)
            {
                if (dropList[i].ConditionInfo.myState == (int)OptionState.NoClick)
                {
                    isCanClick = false;
                    break;
                }
            }

            SetColliderEnableInChild(obj.transform, isCanClick);
        }

        //刷新带星星的选项
        private void RefreshStar(GameObject obj, MahjongPlayOption optionData)
        {
            UILabel label = obj.GetComponentInChildren<UILabel>();
            label.text = optionData.DefaultStarData.name;

            if (!optionData.IsSelected)
            {
                int count = optionData.StarDataList.Count;
                for(int i = 0; i < count; i++)
                {
                    optionData.StarDataList[i].isSelected = false;
                    optionData.StarDataList[i].star.transform.FindChild("Select").gameObject.SetActive(false);
                }
            }
            else
            {
                int count = optionData.StarDataList.Count;
                for (int i = 0; i < count; i++)
                {
                    if(optionData.StarDataList[i].isSelected)
                    {
                        label.text = optionData.StarDataList[i].name;
                        optionData.StarDataList[i].star.transform.FindChild("Select").gameObject.SetActive(true);
                    }
                    else
                    {
                        optionData.StarDataList[i].star.transform.FindChild("Select").gameObject.SetActive(false);
                    }
                }
               
            }
            NGUITools.UpdateWidgetCollider(label.gameObject);
        }
        //刷新 其他选项
        private void RefreshOther(GameObject obj, MahjongPlayOption optionData)
        {
            UILabel label = obj.GetComponentInChildren<UILabel>();
            label.color = GetColorByMode(optionData);
            SetOptionState(obj, optionData);
        }

        private void SetOptionState(GameObject obj, MahjongPlayOption optionData)
        {
            obj.SetActive(true);
            UIToggle toggle = obj.GetComponentInChildren<UIToggle>();

            if (optionData.ConditionInfo.myState == (int)OptionState.NoClick)
            {
                SetColliderEnableInChild(obj.transform, false);
            }
            else if (optionData.ConditionInfo.myState == (int)OptionState.Hide)
            {
                obj.SetActive(false);
            }
            else
            {
                SetColliderEnableInChild(obj.transform, true);
            }

            if (toggle != null)
            {
                if (optionData.IsSelected)
                {
                    toggle.value = true;
                    if (optionData.showType == (int)e_OptionType.One)
                    {
                        toggle.transform.GetChild(0).gameObject.SetActive(true);
                    }
                }
                else
                {
                    toggle.value = false;
                    if(optionData.showType == (int)e_OptionType.One)
                    {
                        toggle.transform.GetChild(0).gameObject.SetActive(false);
                    }
                }
            }
        }

        //检查单选框(因为至少有一个要选中)
        private void CheckRadio()
        {
            foreach (KeyValuePair<int, List<GameObject>> item in RadioDict)
            {
                bool isExistSelected = false; //是否存在选中状态

                MahjongPlayOption lastSelectOptionData = null;

                List<GameObject> radios = item.Value;
                int count = radios.Count;
                for(int i = 0; i < count; i++)
                {
                    GameObject obj = radios[i];
                    MahjongPlayOption optionData = OptionDict[obj];
                    if (optionData.IsSelected)
                    {
                        lastSelectOptionData = optionData;
                        isExistSelected = true;
                    }
                }

                if(!isExistSelected)
                {
                    for (int i = 0; i < count; i++)
                    {
                        GameObject obj = radios[i];
                        MahjongPlayOption optionData = OptionDict[obj];
                        if (optionData.ConditionInfo.myState != (int)OptionState.NoClick && optionData.ConditionInfo.myState != (int)OptionState.Hide)
                        {
                            optionData.IsSelected = true;
                            OptionLogic.ChangeImpactOption(optionData);
                            break;
                        }
                    }
                }
            }
            RefreshAllRuleOptions();
        }
        #endregion

        //点击toggle回调
        private void OnToggleChange(MahjongPlayOption optionData, bool value)
        {
            if (isLock || optionData.IsSelected == value) return;

            optionData.IsSelected = value;

            ChangeImpactOptionAndRefresh(optionData);
        }

        private void OnClickStar(GameObject go, MahjongPlayOption optionData, GameObject star)
        {
            if (!optionData.IsSelected) return;

            go.transform.GetComponentInChildren<UILabel>().text = optionData.DefaultStarData.name;

            OptionStarData clickStarData = null;
            int index = 0;

            int starCount = optionData.StarDataList.Count;

            for (int i = 0; i < starCount; i++)
            {
                OptionStarData starData = optionData.StarDataList[i];
                if (starData.star == star)
                {
                    clickStarData = starData;
                    index = i;
                }
            }

            if(clickStarData.isSelected)
            {
                bool isCanHide = true;
                if(index != starCount-1)
                {
                    for (int i = index+1; i < starCount; i++)
                    {
                        OptionStarData starData = optionData.StarDataList[i];
                        if(starData.isSelected)
                        {
                            isCanHide = false;
                            break;
                        }
                    }
                }
                if(isCanHide)
                {
                    clickStarData.isSelected = false;
                }
            }
            else
            {
                bool isCanShow = true;
                for (int i = 0; i < index; i++)
                {
                    OptionStarData starData = optionData.StarDataList[i];
                    if (!starData.isSelected)
                    {
                        isCanShow = false;
                        break;
                    }
                }
                if (isCanShow)
                {
                    clickStarData.isSelected = true;
                }
            }

            RefreshStar(go, optionData);
        }

        //计算每个选项 X坐标
        private float[] CreateEachPosXArray(int columnCount, int alignType, float maxWidth)
        {

            float[] posXArray = new float[columnCount];

            if (alignType == 0)
                alignType = (int)AlignType.Left;

            if (alignType == (int)AlignType.Left)
            {
                float oneWidth = maxWidth / columnCount;
                for (int i = 0; i < columnCount; i++)
                {
                    posXArray[i] = oneWidth * i;
                }
            }
            else if (alignType == (int)AlignType.Center)
            {
                float oneWidth = maxWidth / (columnCount + 1);
                for (int i = 0; i < columnCount; i++)
                {
                    posXArray[i] = oneWidth * (i + 1);
                }
            }
            return posXArray;
        }

        //根据模式 获取颜色
        private Color GetColorByMode(MahjongPlayOption optionData)
        {
            if (optionData.ConditionInfo.myState == (int)OptionState.NoClick)
            {
                return NotClickColor;
            }

            if (isLock)
            {
                if (!optionData.IsSelected)
                    return NotClickColor;
            }

            return Color.white;
        }

        private void PlayMusic()
        {
            MusicCtrl.Instance.Music_SoundPlay(GEnum.SoundEnum.btn_select1);
        }

        //找到所有 Collider 并赋值 enabled
        private void SetColliderEnableInChild(Transform trans, bool enabled)
        {
            Collider col = trans.GetComponent<Collider>();
            if (col != null)
                col.enabled = enabled;

            int count = trans.childCount;
            for (int i = 0; i < count; i++)
            {
                Transform childTrans = trans.GetChild(i);
                Collider collider = childTrans.GetComponent<Collider>();
                if (collider != null)
                {
                    collider.enabled = enabled;
                }
                SetColliderEnableInChild(childTrans, enabled);
            }
        }

    }
}
