
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace projectQ
{
    public enum MjAlignType
    {
        AlignLeft,
        AlignCenter,
    }
    [System.Serializable]
    public class OpenMaTitle
    {
        public EnumMjOpenMaType Type;
        public GameObject TitleObj;
    }
    public class UIOpenMa : UIViewBase
    {
        # region 组件引用
        public GameObject TemplateItem;
        public List<OpenMaTitle> ListTitleObjs = new List<OpenMaTitle>();
        public UIGrid Grid_Up;
        public UIGrid Grid_Down;
        public Animation Animation;

        public UISprite BgSp;
        public UITable New_Table;
        public UIGrid New_Grid;
        public GameObject New_Item;
        #endregion


        #region 数据
        private List<MjHorse> _DataList = new List<MjHorse>();
        private EnumMjOpenMaType _MjOpenMaType = EnumMjOpenMaType.Null;
        #endregion

        //界面上所有动态加载进来的麻将
        private List<UIOpenMaItem> _OpenItems = new List<UIOpenMaItem>();

        //麻将分为上下两排，下面依次是麻将不同个数（一共10个）对应的第一排和第二排动态加载的麻将个数
        private List<int> AlignCenter_Up = new List<int>() { 1, 2, 3, 4, 5, 3, 3, 4, 4, 5, 5 };
        private List<int> AlignCenter_Down = new List<int>() { 0, 0, 0, 0, 0, 3, 4, 4, 5, 5, 6 };


        #region Override
        public override void Init()
        {

        }

        public override void OnHide()
        {
        }

        public override void OnPushData(object[] data)
        {
            if (NullHelper.IsListNullOrEmpty(data))
            {
                return;
            }
            if (NullHelper.IsInvalidIndex(1, data))
            {
                return;
            }
            _MjOpenMaType = (EnumMjOpenMaType)data[0];
            _DataList = (List<MjHorse>)data[1];



        }


        public override void OnShow()
        {
            if (ClearUI())
            {
                if (_MjOpenMaType <= EnumMjOpenMaType.ZhuaNiao)
                {
                    BgSp.width = 800;
                    BgSp.height = 310;
                    this.StartCoroutine(ShowTitleAnim());
                    // ShowTitle();
                    FillGrids();
                }
                else
                {
                    BgSp.width = 1000;
                    TimeTick.Instance.SetAction(ShowTitle, 0.17f);
                    OnStart();
                }
            }
        }

        private IEnumerator ShowTitleAnim()
        {
            yield return new WaitForSeconds(0.17f);
            ShowTitle();
        }
        public bool ClearUI()
        {
            if (NullHelper.IsObjectIsNull(Grid_Up))
            {
                return false;
            }
            if (NullHelper.IsObjectIsNull(Grid_Down))
            {
                return false;
            }
            Grid_Up.transform.DestroyChildren();
            Grid_Down.transform.DestroyChildren();
            return true;
        }
        private void ShowTitle()
        {
            for (int i = 0; i < ListTitleObjs.Count; i++)
            {
                if (NullHelper.IsObjectIsNull(ListTitleObjs[i])  || NullHelper.IsObjectIsNull(ListTitleObjs[i].TitleObj))
                {
                    continue;
                }
                if (ListTitleObjs[i].Type == _MjOpenMaType)
                {
                    GameObjectHelper.SetEnable( ListTitleObjs[i].TitleObj,true);
                    break;
                }
                else
                {
                    GameObjectHelper.SetEnable(ListTitleObjs[i].TitleObj, false);
                }
            }
        }
        private void FillGrids()
        {
            if (_DataList == null || _DataList.Count <= 0)
            {
                DebugPro.DebugError("_DataList is null");
                return;
            }
            MjHorse horse = this._DataList[0];
            if (horse == null)
            {
                DebugPro.DebugError("horse is null");
                return;
            }
            //一次加载第一排和第二排对应的麻将
            int index = horse.maCode.Count - 1;
            if (index < 0 || index >= AlignCenter_Up.Count)
            {
                DebugPro.DebugError("index error:", index);
                return;
            }
            CreateMjItems(AlignCenter_Up[index], Grid_Up.transform);
            if (index < 0 || index >= AlignCenter_Down.Count)
            {
                DebugPro.DebugError("index error:", index);
                return;
            }
            CreateMjItems(AlignCenter_Down[index], Grid_Down.transform);
            Grid_Up.Reposition();
            Grid_Down.Reposition();
            //如果只有一排数据，居中显示
            if (AlignCenter_Down[index] == 0)
            {
                Grid_Up.transform.localPosition = Vector3.zero;
            }
            FillCards();
            //显示动画
            PlayMjAim();
        }
        private void CreateMjItems(int count, Transform grid)
        {
            for (int i = 0; i < count; i++)
            {
                CreateMjItem(grid, i, TemplateItem);
            }
        }

        private void CreateMjItem(Transform grid, int index, GameObject mjItem)
        {
            UIOpenMaItem itemControl = GameTools.InstantiatePrefabAndReturnComp<UIOpenMaItem>(mjItem, grid, true, true);
            if (itemControl != null)
            {
                itemControl.name = "UIOpenMaItem_" + grid.name + index;
                _OpenItems.Add(itemControl);
                if (itemControl.gameObject != null)
                {
                    itemControl.gameObject.SetActive(false);
                }
                else
                {
                    DebugPro.DebugError("SupplementCard selfObj  is null");
                }
            }
            else
            {
                DebugPro.DebugError("SupplementCard instance  is null");
            }
        }

        private void PlayMjAim()
        {
            this.StartCoroutine(PlayMjItemsAnim());
            this.StartCoroutine(WaitingToCloseUI());
        }
        private void FillCards()
        {
            MjHorse horse = this._DataList[0];
            if (horse.maCode == null || horse.maCode.Count <= 0)
            {
                DebugPro.DebugError("horse.maCode   is null");
            }

            if (horse.maCode.Count != horse.horseType.Count)
            {
                DebugPro.DebugError("horse.maCode   is Erro");
                return;
            }

            for (int i = 0; i < _OpenItems.Count; i++)
            {
                if (i < horse.maCode.Count)
                {
                    _OpenItems[i].InitCard(horse.maCode[i], horse.horseType[i]);
                    _OpenItems[i].gameObject.SetActive(true);
                }
                else
                {
                    DebugPro.DebugError("horse.maCode count is:", horse.maCode.Count, "_OpenItems count is:", _OpenItems.Count);
                }
            }
        }
        private IEnumerator PlayMjItemsAnim()
        {
            yield return new WaitForSeconds(ConstDefine.OpenMaUIOpenTime);
            FillCards();
            for (int i = 0; i < _OpenItems.Count; i++)
            {
                _OpenItems[i].FlipCard();
                yield return new WaitForSeconds(ConstDefine.OpenMaFlippingIntervalTime);
            }
            yield return new WaitForSeconds(0.2f);
            for (int i = 0; i < _OpenItems.Count; i++)
            {
                _OpenItems[i].SetCardAlpha();
            }
        }
        private IEnumerator WaitingToCloseUI()
        {
            float time = CardHelper.GetUITotalAimTime(_OpenItems.Count, ConstDefine.OpenMaFlippingTime, ConstDefine.OpenMaFlippingIntervalTime, ConstDefine.UICloseDlay);
            yield return new WaitForSeconds(time);
            if (Animation != null)
            {
                Animation.enabled = true;
                yield return new WaitForSeconds(ConstDefine.OpenMaUIOpenTime);
            }
            this.Close();
        }
        #endregion


        #region new
        private void OnStart()
        {
            ShowMahjong();
        }

        private void ShowMahjong()
        {
            if (_DataList == null || _DataList.Count == 0)
            {
                return;
            }

            int lineNum = GetLineNum();
            //int everyLineNum = 10;

            InitBgSp();

            for (int i = 0; i < lineNum; i++)
            {
                GameObject gridObj = NGUITools.AddChild(New_Table.gameObject, New_Grid.gameObject);
                int cardNum = 0;
                int count = _DataList[0].maCode.Count;

                if(lineNum == 1)
                {
                    cardNum = count;
                }
                else
                {
                    cardNum = (int)(count / lineNum);
                    if ((i + 1 <= count % lineNum) && (count % lineNum != 0))
                        cardNum++;
                }
                //if (i < lineNum - 1)
                //    cardNum = everyLineNum;
                //else
                //    cardNum = count % everyLineNum;

                for (int n = 0; n < cardNum; n++)
                {
                    CreateMjItem(gridObj.transform, cardNum, New_Item);
                }

                UIGrid grid = gridObj.GetComponent<UIGrid>();
                if (grid != null)
                    grid.Reposition();
            }

            New_Table.Reposition();

            FillCards();
            PlayMjAim();
        }

        private void InitBgSp()
        {
            int lineNum = GetLineNum();
            int chaNum = 80;
            int he = 110;

            BgSp.height = lineNum * he + chaNum;
        }

        private int GetLineNum()
        {
            int lineNum = 0;
            int everyLineNum = 10;
            int count = _DataList[0].maCode.Count;

            if (count <= everyLineNum)
                lineNum = 1;
            else
            {
                lineNum = count / everyLineNum;
                if (count % everyLineNum > 0)
                    lineNum++;
            }

            return lineNum;
        }
        #endregion
    }
}
