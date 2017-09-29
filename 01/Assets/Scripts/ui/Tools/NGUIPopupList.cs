/**
 * @Author JEFF
 *
 *
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using projectQ;

public abstract class NGUIPopupListItemBase : MonoBehaviour
{
    public NGUIPopupList PopupListScript;
    public object UIData;
    public abstract string Name
    {
         get;
    }

    public virtual void SetData(object data, bool isRefresh)
    {
        this.UIData = data;
        if (isRefresh)
            this.Refresh();
    }

    public virtual void Selected()
    {
        PopupListScript.Selected(gameObject);
    }

    public virtual void OnItemClick()
    {
        if(PopupListScript.onClickCallBack != null)
        {
            PopupListScript.onClickCallBack(gameObject);
        }
    }

    public abstract void Refresh();

    public abstract void RefreshSelectUI(bool isSelected);

    
}

public class NGUIPopupList : MonoBehaviour {
    public UISprite Bg;
    private GameObject BgGameObject;
    private UISprite selfSprite;
    public UIGrid grid;
    public UILabel label;
    public GameObject Item;
    public GameObject taget;
    public int BgBaseHeight = 0;
    public int BgItemHeight = 0;
    public int group = 0;

    public GameObject SelectedGo;

    public List<UITweener> TweenList;

    private List<GameObject> listData = null;

    public bool IsShow
    {
        get { return BgGameObject.activeSelf; }
    }
    public System.Action<GameObject> onClickCallBack = null;

    #region API
    /// <summary>
    /// 初始化数据
    /// </summary>
    /// <param name="dataList"></param>
    public void Init(List<object> dataList,System.Action<GameObject> onClickFunc) 
    {
        onClickCallBack = onClickFunc;
        SelectedGo = null;
        listData = new List<GameObject>();
        UITools.CreateChild<object>(grid.transform, Item, dataList, OnCreateItem, false);
        OnRefreshAllUI();
        grid.Reposition();
        CreateBg();
        Selected(0);
    }

    bool tempIsShow = false;

    /// <summary>
    /// 展示
    /// </summary>
    /// <param name="isShow">是否展示</param>
    public void Show(bool isShow,bool isCloseOther = false)
    {
        if (IsShow != isShow)
        {
            tempIsShow = isShow;
            if (isShow)
            {
                IsUpShow();
                BgGameObject.SetActive(true);
                grid.gameObject.SetActive(true);
            }
            if(TweenList != null && TweenList.Count > 0)
            {
                for (int i=0; i< TweenList.Count;++i)
                {
                    TweenList[i].Play(tempIsShow);
                }
            }

            if(isCloseOther)
            {
                var list = GetPopupList();
                for (int i=0; i<list.Count; ++i)
                {
                    if(list[i] != this)
                    {
                        list[i].Show(false);
                    }
                }
            }
        }
    }

    /// <summary>
    /// 上下显示方向
    /// </summary>
    /// <returns></returns>
    public bool IsUpShow()
    {
        UIPanel panel = UITools.GetNearestPanel(gameObject);
        if(panel != null)
        {
            Vector4 panelBox = panel.finalClipRegion;
            float panelPosY = panel.cachedTransform.localPosition.y + panelBox.y;
            Vector3 bbb = panel.parent.cachedTransform.TransformPoint(new Vector3(0f, panelPosY - panelBox.w * 0.5f + Bg.height, 0f));
            Vector3 ccc = transform.parent.InverseTransformPoint(bbb);
            if (transform.localPosition.y < ccc.y)
            {
                //向上转换
                UpChange(true);
            }
            else
            {
                UpChange(false);
            }
        }
        return false;
    }

    Vector3 downGridPos;
    Vector3 downBgPos;
    private void UpChange(bool isUp)
    {
        //锚点修改
        Bg.pivot = isUp ? UIWidget.Pivot.Bottom : UIWidget.Pivot.Top;

        if (isUp)
        {
            //
            Bg.transform.localPosition = new Vector3(downBgPos.x,0f, downBgPos.z);
            //(背景 - 自己/2 )/2 + childCount / 2 * gridHeight - bgheightBaseHeight/2
            //float h = (Bg.height - selfSprite.height * 0.5f) * 0.5f + listData.Count * 0.5f * grid.cellHeight + BgBaseHeight * 0.5f;
            grid.transform.localPosition = new Vector3(downGridPos.x, Bg.height - BgBaseHeight, downGridPos.z);
        }
        else
        {
            Bg.transform.localPosition = downBgPos;
            grid.transform.localPosition = downGridPos;
        }
        //图片翻转
        Bg.flip = isUp ? UIBasicSprite.Flip.Vertically : UIBasicSprite.Flip.Nothing;
        grid.Reposition();
    }

    private void OnShowTweenFinished()
    {
        BgGameObject.SetActive(tempIsShow);
        grid.gameObject.SetActive(tempIsShow);
    }

    public void Selected(int index)
    {
        if(listData.Count > index)
        {
            var script = listData[index].GetComponent<NGUIPopupListItemBase>();
            if(script != null)
            {
                script.Selected();
            }
        }
    }

    public void Selected(GameObject go)
    {
        SelectedGo = go;
        for (int i = 0; i < listData.Count; i++)
        {
            bool flag = listData[i] == go;
            var temp = listData[i].GetComponent<NGUIPopupListItemBase>();
            temp.RefreshSelectUI(flag);
           
            if (flag)
            {
                label.text = temp.Name;
            }
        }
        Show(false);
    }
    #endregion

    /// <summary>
    /// 创建背景
    /// </summary>
    private void CreateBg()
    {
        Bg.height = BgBaseHeight + (listData.Count * (BgItemHeight <= 0 ? (int)grid.cellHeight: BgItemHeight));
    }

    /// <summary>
    /// 创建Item
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="go"></param>
    /// <param name="data"></param>
    private void OnCreateItem(GameObject go, object data)
    {
        listData.Add(go);
        var script = go.GetComponent<NGUIPopupListItemBase>();
        script.PopupListScript = this;
        script.SetData(data,false);
    }
    private void OnRefreshAllUI()
    {
        for (int i=0;i<listData.Count; ++i)
        {
            var script = listData[i].GetComponent<NGUIPopupListItemBase>();
            script.Refresh();
        }
    }

    #region Event
    private void OnPopupListClick(GameObject go)
    {
        if (onClickCallBack != null)
            onClickCallBack(go);
        Show(!IsShow,true);
    }
    #endregion

    #region 生命周期
    private void Awake()
    {
        selfSprite = GetComponent<UISprite>();
        BgGameObject = Bg.gameObject;
        UIEventListener.Get(gameObject).onClick = OnPopupListClick;

        TweenList = new List<UITweener>();
        UITweener[] tweens = BgGameObject.GetComponents<UITweener>();
        for(int i=0; i< tweens.Length; ++i)
        {
            TweenList.Add(tweens[i]);
        }
        EventDelegate.Add(TweenList[TweenList.Count - 1].onFinished, OnShowTweenFinished);

        if (BgItemHeight == 0f)
            BgItemHeight = (int)grid.cellHeight;

        downGridPos = grid.transform.localPosition;
        downBgPos = Bg.transform.localPosition;
    }

    private void Start()
    {
        Show(false);
    }

    
    private void OnDisable()
    {
        RemovePopup();
    }

    private void OnEnable()
    {
        AddPopup();
        if (SelectedGo != null && listData != null && listData.Count > 0)
        {
            int index = listData.IndexOf(SelectedGo);
            Selected(index == -1 ? 0 : index);
        }
    }
    #endregion

    #region 全局管理
    [HideInInspector]
    public static Dictionary<int, List<NGUIPopupList>> GroupPopupMap = new Dictionary<int, List<NGUIPopupList>>();
    private void AddPopup()
    {
        if (this.group == 0) return;

        if(GroupPopupMap.ContainsKey(group))
        {
            var list = GroupPopupMap[group];
            if(list.IndexOf(this) == -1)
            {
                list.Add(this);
            }
        }
        else
        {
            GroupPopupMap.Add(this.group, new List<NGUIPopupList>() { this });
        }
    }
    private void RemovePopup()
    {
        if (this.group == 0) return;

        if (GroupPopupMap.ContainsKey(group))
        {
            int index = GroupPopupMap[group].IndexOf(this);
            if(index >= 0)
            {
                GroupPopupMap[group].RemoveAt(index);
            }
        }
    }

    private List<NGUIPopupList> GetPopupList()
    {
        if(GroupPopupMap.ContainsKey(this.group))
        {
            return GroupPopupMap[this.group];
        }
        return null;
    }
    #endregion
}
