/**
 * @Author JEFF
 *
 *
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MahjongPlayType;
using projectQ;
public class UINumberFlyingItem : MonoBehaviour
{

    #region 组件
    public Transform Grid;
    public UILabel TitleLabel;
    public UILabel ScrollViewItem;
    public UILabel ScrollViewGrayItem;
    public UIScrollView ScrollView;
    public UICenterOnChild CenterTransform;

    public UIGrid BtnGrid;
    public GameObject BtnItem;
    public UISprite Bg;
    private GameObject m_CurBtn;
    #endregion

    #region 数据

    private MjXuanPiaoData.CommonData _Data = null;
    private List<GameObject> listItem = new List<GameObject>();
    private List<int> dataList = new List<int>();

    private List<GameObject> m_BtnObjList = new List<GameObject>();
    private const string m_SelectName = "Select";
    private const string m_LabelName = "Label";
    #endregion

    #region 方法
    void Awake()
    {
        if (NullHelper.IsObjectIsNull(CenterTransform))
        {
            return;
        }
        
    }
    private void OnChoose(GameObject obj)
    {
        if (NullHelper.IsObjectIsNull(_Data) /*|| NullHelper.IsObjectIsNull(_Data.Data)*/)
        {
            return;
        }
        if (listItem.Contains(obj))
        {
            int index = listItem.IndexOf(obj);
            if (!NullHelper.IsInvalidIndex(index, dataList))
            {
                _Data.curChooseValue = dataList[index];
                EventDispatcher.FireEvent(GEnum.NamedEvent.UINumberFlyingChoose, _Data);
            }
        }
        else
        {
            DebugPro.DebugError("choose obj:", obj == null ? "null" : obj.name);
        }
    }
    public void OnPushData(object[] data)
    {
        if (NullHelper.IsObjectIsNull(data))
        {
            return;
        }
        if (!NullHelper.IsInvalidIndex(0, data))
        {
            _Data = (MjXuanPiaoData.CommonData)data[0];
            if (NullHelper.IsObjectIsNull(_Data))
            {
                return;
            }
            RefreshUI();
        }

    }
    public void RefreshUI()
    {
        if (!NullHelper.IsObjectIsNull(TitleLabel))
        {
            TitleLabel.text = _Data.typeName;
        }
        FillScrollView();
    }
    private void FillScrollView()
    {
        if (NullHelper.IsObjectIsNull(Grid) || NullHelper.IsObjectIsNull(ScrollView))
        {
            return;
        }
        Grid.transform.DestroyChildren();

        int count = _Data.chooseVlaueList.Count;
        if (count <= 0)
        {
            DebugPro.DebugError("no data to show");
            return;
        }
        for (int i = 0; i < count; i++)
        {
            GameObject obj = NGUITools.AddChild(BtnGrid.gameObject, BtnItem);

            obj.name = _Data.chooseVlaueList[i].ToString();
            Transform select = obj.transform.Find(m_SelectName);
            if(select != null && select.gameObject.activeSelf)
            {
                select.gameObject.SetActive(false);
            }

            Transform labelObj = obj.transform.Find(m_LabelName);
            if (labelObj != null)
            {
                UILabel lab = labelObj.GetComponent<UILabel>();
                lab.text = _Data.chooseVlaueList[i] == 0 ? "不" + _Data.OprateName : _Data.chooseVlaueList[i].ToString() + _Data.OprateName;
            }

            UIEventListener.Get(obj).onClick = OnClickBtn;
            dataList.Add(_Data.chooseVlaueList[i]);

            m_BtnObjList.Add(obj);
            /*UILabel item = GameTools.InstantiatePrefabAndReturnComp<UILabel>(_Data.chooseVlaueList[i] == 0 ? ScrollViewGrayItem.gameObject : ScrollViewItem.gameObject, Grid.transform, true, true);
            if (NullHelper.IsObjectIsNull(item))
            {
                break;
            }
            GameObjectHelper.NormalizationTransform(item.transform);

            item.text = _Data.chooseVlaueList[i] == 0 ? "不" + _Data.OprateName : _Data.chooseVlaueList[i].ToString() + _Data.OprateName;

            dataList.Add(_Data.chooseVlaueList[i]);
            item.name = item.text;
            item.gameObject.SetActive(true);
            listItem.Add(item.gameObject);
            item.transform.localPosition = -Vector3.up * this.ScrollView.scrollWheelFactor * (i + 1);*/
        }
        //if (!NullHelper.IsObjectIsNull(CenterTransform) && _Data.chooseVlaueList.Contains(_Data.curChooseValue))
        //{
        //    CenterTransform.Recenter();
        //    CenterTransform.CenterOn(listItem[_Data.chooseVlaueList.IndexOf(_Data.curChooseValue)].transform);
        //}

        if (!NullHelper.IsObjectIsNull(ScrollView) && listItem.Count <= 1)
        {
            this.ScrollView.enabled = false;
        }

        BtnGrid.Reposition();

        int num = m_BtnObjList.Count;
        int width = (int)(num * BtnGrid.cellWidth) + TitleLabel.width;
        Bg.width = width + 10;

        if (m_CurBtn == null)
            OnClickBtn(m_BtnObjList[0]);
        else
            OnClickBtn(m_CurBtn);
    }
    public void CenterOnTarget()
    {
        //this.StartCoroutine(ScrollToTarget());
    }
    private IEnumerator ScrollToTarget()
    {
        yield return new WaitForSeconds(0.1f);
        //1.第一种效果,按照距离最近滑动
        int index = _Data.chooseVlaueList.IndexOf(_Data.curChooseValue);
        SpringPanel.Begin(ScrollView.gameObject, Vector3.up * 50 * index, 45f);
        if (NullHelper.IsObjectIsNull(CenterTransform))
        {
            yield break;
        }
        //2.顺时针滑动
        //  CenterTransform.Recenter();
        // CenterTransform.CenterOn(listItem[index].transform);

        //ScrollView.transform.localPosition = new Vector3(0, 220f, 0f);
        //ScrollView.panel.clipOffset = new Vector3(0, -220f, 0f);
        CenterTransform.onCenter = OnChoose;
    }

    public void ChangeBgSprite(int wid)
    {
        Bg.width = wid;
    }

    private void OnClickBtn(GameObject obj)
    {
        if (m_CurBtn == null || m_CurBtn != obj)
            m_CurBtn = obj;

        ChangeBtn();
    }

    private void ChangeBtn()
    {
        if (m_BtnObjList == null || m_CurBtn == null)
            return;

        for (int i = 0; i < m_BtnObjList.Count; i++)
        {
            bool isCur = m_BtnObjList[i] == m_CurBtn;
            GameObject obj = m_BtnObjList[i];

            Transform select = obj.transform.Find(m_SelectName);
            if (select != null && select.gameObject.activeSelf != isCur)
            {
                select.gameObject.SetActive(isCur);
            }

            Transform labelObj = obj.transform.Find(m_LabelName);
            if (labelObj != null)
            {
                UILabel lab = labelObj.GetComponent<UILabel>();
                if (isCur)
                    lab.color = Color.white;
                else
                    lab.color = new Color(191 / 255f, 191 / 255f, 191 / 255f);
            }

            if(isCur)
            {
                int index = m_BtnObjList.IndexOf(obj);
                if (!NullHelper.IsInvalidIndex(index, dataList))
                {
                    _Data.curChooseValue = dataList[index];
                    EventDispatcher.FireEvent(GEnum.NamedEvent.UINumberFlyingChoose, _Data);
                }
            }
        }
    }

    #endregion
}
