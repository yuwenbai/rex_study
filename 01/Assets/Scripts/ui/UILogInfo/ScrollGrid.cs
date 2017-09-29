/***
 ****************** scrollView 循环 item 减少性能消耗 ************
 *@2015/03/31
 *@author fastHro
 *
 *基于NGUI Version 3.7.7
 *使用说明：在scrollView 下新建空物体，然后把此脚本挂在上面然后设置相关属性.
 *注意：scrollGrid 下不需要有item ，由脚本自动创建关联的item
 *        刷新item数据通过 委托函数scrollGridSetItem 来设置
 *
 */
using UnityEngine;
using System.Collections;

public class ScrollGrid : MonoBehaviour
{
    public delegate void ScrollGridSetItem(Transform[] trans, int start, int end);
    /** @index -1 start， 0 start and end， 1 end， 2 center */
    public delegate void ScrollGridMessage(int index);
    private ScrollGridSetItem scrollGridSetItem;
    private ScrollGridMessage scrollGridMessage;
    #region  外部数据 
    /** 最多显示几个 */
    public int defaultShowMaxLine;
    /** 默认生成几个 */
    public int defaultMaxLine;
    /** 每一行的间距 */
    public int space;
    /** 每一行的size */
    public Vector2 size;

    /** gotoIndex 速率 */
    public float autoVelocity = 8f;
    /** 当数据个数小于defaultMaxLine 列表是否可以移动 */
    public bool moveScrollViewLessMaxLine = true;
    /** 每次滑动之后是否使item显示在中心 */
    public bool centerScrollViewOnChild = false;
    #endregion

    #region 内部数据
    private int indexStar = 0;
    private int indexEnd = 0;
    private int indexMax = 0;
    private Vector3 panelPos = Vector3.zero;
    private int lastIndex = 0;
    private int showStartIndex = 0;
    #endregion

    #region  scrollView
    private UIPanel panel;
    private UIScrollView sView;
    /** 列表的 cliping size */
    public Vector2 vSize;
    /** 列表移动方式 */
    private UIScrollView.Movement movement;
    private Transform panelTransfrom;
    #endregion

    #region item
    public GameObject ItemPrefab;
    private Transform[] itemTrans;
    #endregion

    void Awake()
    {
        sView = transform.GetComponentInParent<UIScrollView>();
        panel = sView.transform.GetComponentInParent<UIPanel>();
        panelTransfrom = panel.gameObject.transform;
    }

    #region init
    /** 初始化设置 */
    public void SetGrid(int imax, ScrollGridSetItem sc)
    {
        if (!moveScrollViewLessMaxLine)
        {
            if (imax < defaultShowMaxLine)
            {
                sView.enabled = false;
            }
        }
        if (defaultMaxLine > imax)
        {
            defaultMaxLine = imax;
        }
        initGrid();
        indexMax = imax;
        scrollGridSetItem = sc;
        if (scrollGridSetItem != null)
        {
            scrollGridSetItem(itemTrans, indexStar, indexEnd);
        }
        SetMessage();
    }

    public void SetGrid(int imax, ScrollGridSetItem sc, ScrollGridMessage sm)
    {
        scrollGridMessage = sm;
        SetGrid(imax, sc);
    }

    /** 向后滑动一格 */
    public void NextIndex()
    {
        int ix = GetShowStartIndex();
        //Debug.Log ("next = " + ix);
        GotoIndex(ix + 1);
    }

    /** 向前滑动一格 */
    public void PreIndex()
    {
        int ix = GetShowStartIndex();
        //Debug.Log ("pre = " + ix);
        GotoIndex(ix - 1);
    }

    /**直接跳到指定位置*/
    public void GotoIndex(int index)
    {
        if (index < 0)
        {
            return;
        }
        if (index > indexMax - defaultShowMaxLine)
        {
            return;
        }
        float ip = GetPanelPositionByIndex(index);
        ip = Mathf.Abs(ip);
        Vector3 v3 = Vector3.zero;
        Vector2 v2 = Vector2.zero;

        if (movement == UIScrollView.Movement.Horizontal)
        {
            v3.x = panel.clipSoftness.x - ip;
            v2.x = ip + panel.clipSoftness.x;
            if (panelTransfrom.localPosition.x == ip)
            {
                return;
            }
        }
        else if (movement == UIScrollView.Movement.Vertical)
        {
            v3.y = ip - panel.clipSoftness.y;
            v2.y = -ip + panel.clipSoftness.y;
            if (panelTransfrom.localPosition.y == ip)
            {
                return;
            }
        }

        SpringPanel.Begin(sView.panel.cachedGameObject, v3, autoVelocity);
    }

    /** 初始化 */
    void initGrid()
    {
        ClearChild();

        itemTrans = new Transform[defaultMaxLine];
        for (int i = 0; i < defaultMaxLine; i++)
        {
            itemTrans[i] = CreatItemTransfrom(i);
        }

        movement = sView.movement;

        sView.onDragStarted = onDragStarted;
        sView.onDragFinished = onDragFinished;
        sView.onMomentumMove = onMomentumMove;
        sView.onStoppedMoving = onStoppedMoving;

        /** 初始化位置 */
        ResetPostion();

        /** 初始化数据 */
        indexStar = 0;
        indexEnd = defaultMaxLine - 1;
        indexMax = 0;
        panelPos = Vector3.zero;
        lastIndex = 0;
        showStartIndex = 0;
        if (scrollGridSetItem != null)
        {
            scrollGridSetItem(itemTrans, indexStar, indexEnd);
        }
        SetMessage();
        ResetScrollPanelPosition();
    }

    void ResetPostion()
    {
        float fristCoord = 0;
        bool hasFristCoord = false;
        Vector3 np = Vector3.zero;
        float vp = 0;
        float ip = 0;
        float fp = 0;
        if (movement == UIScrollView.Movement.Horizontal)
        {
            vp = vSize.x;
            ip = size.x;
        }
        else if (movement == UIScrollView.Movement.Vertical)
        {
            vp = vSize.y;
            ip = size.y;
        }
        for (int i = 0; i < itemTrans.Length; i++)
        {
            if (!hasFristCoord)
            {
                fristCoord = vp / 2.0f - ip / 2.0f;
                if (fristCoord > vp / 2.0f)
                {
                    fristCoord = vp / 2.0f;
                }
                hasFristCoord = true;
                fp = fristCoord;
            }
            else
            {
                fp = fristCoord - (ip + space) * i;
            }
            if (movement == UIScrollView.Movement.Horizontal)
            {
                np.x = -fp;
            }
            else if (movement == UIScrollView.Movement.Vertical)
            {
                np.y = fp;
            }
            itemTrans[i].localPosition = np;
        }
    }
    #endregion

    #region switch Item
    void SwitchItem(int id)
    {
        indexStar = id;
        //QLoger.LOG("indexStar : " + indexStar.ToString());
        if (movement == UIScrollView.Movement.Horizontal)
        {
            //QLoger.LOG("indexStar : " + indexStar.ToString());
            //QLoger.LOG("stop       : " + (indexMax - defaultMaxLine).ToString());
            if (indexStar > 0)
            {
                indexStar = 0;
                return;
            }
            if (indexStar < -(indexMax - defaultMaxLine))
            {
                indexStar = indexMax - defaultMaxLine;
                return;
            }
            indexStar = Mathf.Abs(indexStar);
        }
        else if (movement == UIScrollView.Movement.Vertical)
        {
            /** star */
            if (indexStar < 0)
            {
                indexStar = 0;
                return;
            }
            /**end*/
            if (indexStar > indexMax - defaultMaxLine)
            {
                indexStar = indexMax - defaultMaxLine;
                return;
            }
        }

        if (lastIndex != indexStar)
        {
            if (lastIndex < indexStar)
            {
                Transform t = itemTrans[0];
                for (int i = 0; i < itemTrans.Length - 1; i++)
                {
                    itemTrans[i] = itemTrans[i + 1];
                }

                float fy = 0;
                Vector3 v3 = t.localPosition;
                if (movement == UIScrollView.Movement.Horizontal)
                {
                    fy = itemTrans[itemTrans.Length - 2].localPosition.x + size.x + space;
                    v3.x = fy;
                }
                else if (movement == UIScrollView.Movement.Vertical)
                {
                    fy = itemTrans[itemTrans.Length - 2].localPosition.y - size.y - space;
                    v3.y = fy;
                }

                t.localPosition = v3;
                itemTrans[itemTrans.Length - 1] = t;
            }
            else
            {
                Transform t = itemTrans[itemTrans.Length - 1];
                for (int i = itemTrans.Length - 1; i > 0; i--)
                {
                    itemTrans[i] = itemTrans[i - 1];
                }

                float fy = 0;
                Vector3 v3 = t.localPosition;
                if (movement == UIScrollView.Movement.Horizontal)
                {
                    fy = itemTrans[1].localPosition.x - size.x - space;
                    v3.x = fy;
                }
                else if (movement == UIScrollView.Movement.Vertical)
                {
                    fy = itemTrans[1].localPosition.y + size.y + space;
                    v3.y = fy;
                }

                t.localPosition = v3;
                itemTrans[0] = t;
            }

            lastIndex = indexStar;

            if (scrollGridSetItem != null)
            {
                scrollGridSetItem(itemTrans, indexStar, indexEnd);
            }
            sView.UpdatePosition();
        }
    }

    /** 重置scroll panel */
    void ResetScrollPanelPosition()
    {
        if (movement == UIScrollView.Movement.Horizontal)
        {
            if (indexStar == 0)
            {
                ResetPostion();
                Vector3 v3 = Vector3.zero;
                v3.x = panel.clipSoftness.x;
                Vector2 v2 = Vector2.zero;
                v2.x = -panel.clipSoftness.x;
                panelTransfrom.localPosition = v3;
                panel.clipOffset = v2;
            }
        }
        else if (movement == UIScrollView.Movement.Vertical)
        {
            if (indexStar == 0)
            {
                ResetPostion();
                Vector3 v3 = Vector3.zero;
                v3.y = -panel.clipSoftness.y;
                Vector2 v2 = Vector2.zero;
                v2.y = panel.clipSoftness.y;
                panelTransfrom.localPosition = v3;
                panel.clipOffset = v2;
            }
        }
    }

    void SetItemPostion()
    {
        float fristCoord = GetPositionByIndex(indexStar);
        //Debug.Log ("fristCoord = " + fristCoord + "  indexStar  " + indexStar);
        Vector3 np = Vector3.zero;
        float ip = 0;
        float fp = 0;
        if (movement == UIScrollView.Movement.Horizontal)
        {
            ip = size.x;
        }
        else if (movement == UIScrollView.Movement.Vertical)
        {
            ip = size.y;
        }
        for (int i = 0; i < itemTrans.Length; i++)
        {
            fp = fristCoord - (ip + space) * i;
            if (movement == UIScrollView.Movement.Horizontal)
            {
                np.x = -fp;
            }
            else if (movement == UIScrollView.Movement.Vertical)
            {
                np.y = fp;
            }
            itemTrans[i].localPosition = np;
        }
    }

    /** 通过index 获得item位置 */
    public float GetPositionByIndex(int index)
    {
        float fristCoord = 0;
        bool hasFristCoord = false;
        Vector3 np = Vector3.zero;
        float vp = 0;
        float ip = 0;
        float fp = 0;
        if (movement == UIScrollView.Movement.Horizontal)
        {
            vp = vSize.x;
            ip = size.x;
        }
        else if (movement == UIScrollView.Movement.Vertical)
        {
            vp = vSize.y;
            ip = size.y;
        }
        fristCoord = vp / 2.0f - ip / 2.0f;
        if (fristCoord > vp / 2.0f)
        {
            fristCoord = vp / 2.0f;
        }

        fp = fristCoord - (ip + space) * index;
        return fp;
    }

    /** 通过index 获得panel位置 */
    public float GetPanelPositionByIndex(int index)
    {
        if (movement == UIScrollView.Movement.Horizontal)
        {
            return (size.x + space) * index;
        }
        else if (movement == UIScrollView.Movement.Vertical)
        {
            return (size.y + space) * index;
        }
        return 0;
    }

    /** 获得列表显示的第一个的index */
    int GetShowStartIndex()
    {
        Vector3 v3 = panelTransfrom.localPosition;
        if (movement == UIScrollView.Movement.Horizontal)
        {
            if (indexStar > 0)
            {
                int index = (int)((v3.x - panel.clipSoftness.x) / (size.x + space));
                return Mathf.Abs(index);
            }
        }
        else if (movement == UIScrollView.Movement.Vertical)
        {
            if (indexStar > 0)
            {
                int index = (int)((v3.y + panel.clipSoftness.y) / (size.y + space));
                return index;
            }
        }
        return indexStar;
    }
    #endregion

    #region  创建 和 清空Item 
    Transform CreatItemTransfrom(int index)
    {
        GameObject go = Instantiate(ItemPrefab, Vector3.zero, Quaternion.identity) as GameObject;
        if (go != null)
        {
            go.transform.parent = transform;
            GameObjectHelper.NormalizationTransform(go.transform);
            go.name = "ScrollGrid Item " + index.ToString();

            return go.transform;
        }
        projectQ.QLoger.ERROR("scrollGrid creat item prefab ");
        return null;
    }

    public void ClearChild()
    {
        if (transform.childCount < 1)
        {
            return;
        }
        Transform t = transform.GetChild(0);
        t.parent = null;
        Destroy(t.gameObject);
        ClearChild();
    }
    #endregion

    #region scroll delegate

    void onDragStarted()
    {
        //Debug.Log ("onDragStarted");
    }
    void onDragFinished()
    {
        //Debug.Log ("onDragFinished");
        SetItemPostion();
        if (centerScrollViewOnChild)
        {
            GotoIndex(GetShowStartIndex());
        }
    }

    void onMomentumMove()
    {
        //Debug.Log ("onMomentumMove");

    }
    void onStoppedMoving()
    {
        //Debug.Log ("onStoppedMoving");
        ResetScrollPanelPosition();
    }
    #endregion

    #region update and LateUpdate
    int stepIndex = 0;
    void LateUpdate()
    {
        panelPos = panel.gameObject.transform.localPosition;

        if (movement == UIScrollView.Movement.Horizontal)
        {
            stepIndex = (int)(panelPos.x / (size.x + space));
        }
        else if (movement == UIScrollView.Movement.Vertical)
        {
            stepIndex = (int)(panelPos.y / (size.y + space));
        }
        SwitchItem(stepIndex);
        SetMessage();
    }
    #endregion

    #region message
    /** 通知是否到两头 */
    void SetMessage()
    {
        if (scrollGridMessage != null)
        {
            int index = GetShowStartIndex();
            if (index == 0)
            {
                if (indexMax <= defaultShowMaxLine)
                {
                    scrollGridMessage(0);
                }
                else
                {
                    scrollGridMessage(-1);
                }
            }
            else
            {
                if (indexMax - defaultShowMaxLine > index)
                {
                    if (movement == UIScrollView.Movement.Horizontal)
                    {
                        if (index + 1 == indexMax - defaultShowMaxLine)
                        {
                            if (Mathf.Abs(itemTrans[itemTrans.Length - 1].localPosition.x) == Mathf.Abs(GetPositionByIndex(indexMax - 1)))
                            {
                                scrollGridMessage(1);
                            }
                            else
                            {
                                scrollGridMessage(2);
                            }
                        }
                        else
                        {
                            scrollGridMessage(2);
                        }

                    }
                    else if (movement == UIScrollView.Movement.Vertical)
                    {
                        if (index + 1 == indexMax - defaultShowMaxLine)
                        {
                            if (Mathf.Abs(itemTrans[itemTrans.Length - 1].localPosition.y) == Mathf.Abs(GetPositionByIndex(indexMax - 1)))
                            {
                                scrollGridMessage(1);
                            }
                            else
                            {
                                scrollGridMessage(2);
                            }
                        }
                        else
                        {
                            scrollGridMessage(2);
                        }
                    }

                }
                else
                {
                    scrollGridMessage(1);
                }
            }
        }
    }
    #endregion
}