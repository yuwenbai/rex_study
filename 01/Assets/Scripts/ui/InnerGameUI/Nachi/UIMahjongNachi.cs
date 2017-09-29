/**
 * @Author JEFF
 *
 *
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using projectQ;

public class UIMahjongNachi : UIViewBase
{

    #region 组件挂载
    public UISprite Big_MaskSprite;
    public UISprite Small_MaskSprite;

    public UILabel Label;
    public UILabel TouLabel;
    public UIGrid CardGrid;
    public UISprite BgSp;
    public GameObject CardItem;
    #endregion

    #region 数据
    private int _LeftCode = -1;
    private int _RightCode = -1;
    private string _LabelText = null;
    #endregion
    public override void Init()
    {

    }
    public override void OnShow()
    {

    }
    public override void OnHide()
    {

    }
    public void RefreshUI()
    {
        MahjongPlayType.GangHouNaChiNotifyData serverData = MjDataManager.Instance.MjData.ProcessData.processGangHouNaChi.GangHouNaChiNotifyData;
        if (NullHelper.IsObjectIsNull(serverData))
        {
            return;
        }

        List<int> mjCodes = serverData.MjCodeList;
        if (!NullHelper.IsInvalidIndex(0, mjCodes))
        {
            _LeftCode = mjCodes[0];
        }
        if (!NullHelper.IsInvalidIndex(1, mjCodes))
        {
            _RightCode = mjCodes[1];
        }
        //RefreshCard(LeftCard, _LeftCode);
        //RefreshCard(RightCard, _RightCode);

        List<int> list = new List<int>();
        list.Add(_LeftCode);
        list.Add(_RightCode);
        CreatCards(list);
    }

    private void CreatCards(List<int> list)
    {
        ClearChild();

        bool isNull = list == null || list.Count < 1;
        if (isNull)
        {
            return;
        }
        if (BgSp.gameObject.activeSelf != !isNull)
            BgSp.gameObject.SetActive(!isNull);

        if (!NullHelper.IsObjectIsNull(Label))
        {
            Label.text = "";
        }
        if (!NullHelper.IsObjectIsNull(TouLabel))
        {
            TouLabel.text = "";
        }



        //int height = 105;

        if (list.Count < 2)
        {
            if (!NullHelper.IsObjectIsNull(Label))
            //height = 90;
            {
                TouLabel.text = "亮杠头";
                TouLabel.gameObject.SetActive(true);
            }
            if (!NullHelper.IsObjectIsNull(Small_MaskSprite))
            {
                Small_MaskSprite.gameObject.SetActive(true);
            }
        }
        else
        {
            if (!NullHelper.IsObjectIsNull(Label))
            {
                Label.text = "杠后拿吃";
                Label.gameObject.SetActive(true);
            }
            if (!NullHelper.IsObjectIsNull(Big_MaskSprite))
            {
                Big_MaskSprite.gameObject.SetActive(true);
            }
        }

        //int width = 15;

        for (int i = 0; i < list.Count; i++)
        {
            GameObject addObj = NGUITools.AddChild(CardGrid.gameObject, CardItem);
            addObj.gameObject.SetActive(true);
            SupplementCard sup = addObj.GetComponent<SupplementCard>();
            RefreshCard(sup, list[i]);

            //width += (int)CardGrid.cellWidth;
        }

        //BgSp.height = height;
        //BgSp.width = width;
    }

    private void ClearChild()
    {
        int count = CardGrid.transform.childCount;

        for (int i = 0; i < count; i++)
        {
            Destroy(CardGrid.GetChild(0));
        }
    }

    public void RefreshUI(List<int> cards)
    {
        if (cards != null)
        {
            CreatCards(cards);
        }
    }

    private void RefreshCard(SupplementCard card, int code)
    {
        if (!NullHelper.IsObjectIsNull(card))
        {
            if (code != -1)
            {
                card.InitCard(code);
                GameObjectHelper.SetEnable(card.gameObject, true);
            }
            else
            {
                DebugPro.DebugError("MJCode error:", code);
                GameObjectHelper.SetEnable(card.gameObject, false);
            }
        }
    }

}


//功能测试脚本
//List<int> data=new List<int>();
//   data.Add(1);
//   data.Add(2);
//   EventDispatcher.FireEvent(GEnum.NamedEvent.UIMahjongNaChiDataNotify, data,"1024");
//   PrefabConfig config = MemoryData.XmlData.FindPrefabConfigById(ConstDefine.UIMahjongNaChi_PrefabConfigID.ToString());
//   if (NullHelper.IsObjectIsNull(config))
//   {
//       return;
//   }
//   GameObject subUI = PrefabCachePool.Intance.CreateGameObject(config);
//   UIViewBase ui = _R.ui.GetUI(GameConst.UIPrepareName);
//   if (!NullHelper.IsObjectIsNull(subUI) && !NullHelper.IsObjectIsNull(ui))
//   {
//       subUI.transform.parent = ui.transform;
//       GameObjectHelper.NormalizationTransform(subUI.transform);
//       GameObjectHelper.SetEnable(subUI,true);
//   }
//  return;