using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace projectQ
{
    public class UIShowAllMa : UIViewBase
    {
        public UISprite BgSp;
        public UIGrid New_Table;
        public UIGrid New_Grid;
        public GameObject New_Item;

        private List<MjBalanceNew.MjHorseInfo.BuyHorseItem> _DataList = new List<MjBalanceNew.MjHorseInfo.BuyHorseItem>();
        private List<int> _ShowList;

        #region 重写
        public override void Init()
        {
        }

        public override void OnShow()
        {
        }

        public override void OnHide()
        {
        }
        #endregion

        public override void OnPushData(object[] data)
        {
            _DataList = (List<MjBalanceNew.MjHorseInfo.BuyHorseItem>)data[0];
            _ShowList = (List<int>)data[1];
            ShowMahjong();
        }

        private void ShowMahjong()
        {
            if (_DataList == null || _DataList.Count == 0|| _ShowList == null || _ShowList.Count == 0)
            {
                return;
            }

            int lineNum = GetLineNum();

            InitBgSp();

            for (int i = 0; i < lineNum; i++)
            {
                GameObject gridObj = NGUITools.AddChild(New_Table.gameObject, New_Grid.gameObject);
                int cardNum = 0;
                int count = _ShowList.Count;//_DataList.Count;//[0].cardIDList.Count;

                if (lineNum == 1)
                {
                    cardNum = count;
                }
                else
                {
                    cardNum = (int)(count / lineNum);
                    if ((i + 1 <= count % lineNum) && (count % lineNum != 0))
                        cardNum++;
                }

                for (int n = 0; n < cardNum; n++)
                {
                    CreateMjItem(gridObj.transform, n, New_Item);
                }

                UIGrid grid = gridObj.GetComponent<UIGrid>();
                if (grid != null)
                    grid.Reposition();
                gridObj.gameObject.SetActive(true);
            }

            New_Table.Reposition();
        }

        private void CreateMjItem(Transform grid, int index, GameObject mjItem)
        {
            UIOpenMaItem itemControl = GameTools.InstantiatePrefabAndReturnComp<UIOpenMaItem>(mjItem, grid, true, true);
            if (itemControl != null)
            {
                itemControl.name = "UIOpenMaItem_" + grid.name + index;
                if (itemControl.gameObject != null)
                {
                    itemControl.gameObject.SetActive(true);
                    float al = 1;

                    int type = _DataList[0].horseStateList[_ShowList[index]];
                    switch (type)
                    {
                        case (int)EnumMjBuyhorseStateType.BuyHorseNull:
                            al = 0.75f;
                            break;
                        case (int)EnumMjBuyhorseStateType.BuyHorseWin:
                            al = 1;
                            break;
                        case (int)EnumMjBuyhorseStateType.BuyHorseLose:
                            al = 0.5f;
                            break;
                    }

                    UISprite mjBg = itemControl.transform.Find("HgihtLight").GetComponent<UISprite>();
                    if (type == (int)EnumMjBuyhorseStateType.BuyHorseWin)
                    {
                        mjBg.spriteName = "desk_bj_kaimahuangguang";
                        mjBg.gameObject.SetActive(true);
                    }
                    else if (type == (int)EnumMjBuyhorseStateType.BuyHorseLose)
                    {
                        mjBg.spriteName = "desk_bj_kaimalanguang";
                        mjBg.gameObject.SetActive(true);
                    }
                    CardHelper.SetRecordUI(itemControl.CardSprite, _DataList[0].cardIDList[_ShowList[index]], true, al);//_DataList[0].cardIDList[index]
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
            int count = _ShowList.Count;//[0].cardIDList.Count;

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
    }
}
