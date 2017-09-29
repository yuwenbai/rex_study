/**
 *	作者:YQC
 **/
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
namespace UI.ScrollViewTool
{
    [RequireComponent(typeof(UIGrid))]
    public class ScrollViewWrapContent : MonoBehaviour {
	    #region CustomEvent
	    public delegate void OnChangeItem(int index,GameObject go);
	    public OnChangeItem onChangeItem;
	    #endregion

	    #region Unity Inspector Fields
	    [Tooltip("ItemPrefab")]
	    public GameObject ItemPrefab;

	    [Tooltip("(可选)创建的Child数量 只能大于 (ScrollView.长度 / ItemPrefab.长度) + 1")]
	    public int MaxChildCount = 0;

        [Tooltip("竖向滑动栏")]
        public UIScrollBar VerticalBar;
	    #endregion

	    #region UI
	    //transform
	    private Transform mTransform;
	    //Panel
	    private UIPanel mPanel;
        //ScrollView
        private UIScrollView mScroll;
        //ScrollView
        private MyUIScrollView myScroll { get { return mScroll as MyUIScrollView; } }
        //自身的Grid
        private UIGrid mGrid;
	    #endregion

	    #region Property
	    //Item的总数
	    public int ItemTotalCount { set;get;}
	    //是不是横向排序
	    private bool mHorizontal;
        public bool Horizontal {
            get
            {
                return mHorizontal;
            }
        }
	    //Grid排列方向;
	    private bool mGridHorizontal;
	    //每行最大个数
	    private int maxPerLineCount;
        public int MaxPerLineCount
        {
            get { return maxPerLineCount; }
        }
        //子的宽或者高
        private float itemSize ;
        public float ItemSize
        {
            get { return itemSize; }
        }
	    //子UI列表
	    protected List<Transform> mChildren = new List<Transform>();
        public List<Transform> Children
        {
            get { return mChildren; }
        }


        //获取总的子UI数量	最大多少行 * 每行最大多少个
        private int TotalChildCount{get{return MaxChildCount * maxPerLineCount;}}


	    #endregion 

	    #region 初始化阶段
	    public void Init(int itemTotalCount)
	    {
            isCanRemove = true;
		    ItemTotalCount = itemTotalCount;
		    if (ItemTotalCount <= 0) {
			    //projectQ.QLoger.ERROR("Item总数量必须大于0");
			    //return ;
		    }
		    if (!InitUI ())
			    return;
		    CreateChild (itemTotalCount);

            if(mScroll.isActiveAndEnabled)
                mScroll.StartCoroutine(InitScrollView());
        }

        //private void SetVerticalBarSize()
        //{
        //    if (this.VerticalBar == null) return;
        //    //全部总行数
        //    int AllTotalLine = Mathf.CeilToInt((float)ItemTotalCount / maxPerLineCount);
        //    //AllTotalLine = 

        //    float extents = (float)mPanel.height / (itemSize * AllTotalLine);
        //    if(extents >= 1)
        //    {
        //        this.VerticalBar.gameObject.SetActive(false);
        //    }
        //    else
        //    {
        //        this.VerticalBar.gameObject.SetActive(true);
        //        this.VerticalBar.barSize = extents; 
        //    }
        //}
        //public virtual void UpdateScrollbars(bool recalculateBounds)
        //{
        //    if (mPanel == null) return;

        //    if (/*horizontalScrollBar != null ||*/ VerticalBar != null)
        //    {
        //        if (recalculateBounds)
        //        {
        //            //mCalculatedBounds = false;
        //            //mShouldMove = shouldMove;
        //        }

        //        Bounds b = mScroll.bounds;
        //        Vector2 bmin = b.min;
        //        Vector2 bmax = b.max;

        //        //if (horizontalScrollBar != null && bmax.x > bmin.x)
        //        //{
        //        //    Vector4 clip = mPanel.finalClipRegion;
        //        //    int intViewSize = Mathf.RoundToInt(clip.z);
        //        //    if ((intViewSize & 1) != 0) intViewSize -= 1;
        //        //    float halfViewSize = intViewSize * 0.5f;
        //        //    halfViewSize = Mathf.Round(halfViewSize);

        //        //    if (mPanel.clipping == UIDrawCall.Clipping.SoftClip)
        //        //        halfViewSize -= mPanel.clipSoftness.x;

        //        //    float contentSize = bmax.x - bmin.x;
        //        //    float viewSize = halfViewSize * 2f;
        //        //    float contentMin = bmin.x;
        //        //    float contentMax = bmax.x;
        //        //    float viewMin = clip.x - halfViewSize;
        //        //    float viewMax = clip.x + halfViewSize;

        //        //    contentMin = viewMin - contentMin;
        //        //    contentMax = contentMax - viewMax;

        //        //    UpdateScrollbars(horizontalScrollBar, contentMin, contentMax, contentSize, viewSize, false);
        //        //}

        //        if (VerticalBar != null && bmax.y > bmin.y)
        //        {
        //            Vector4 clip = mPanel.finalClipRegion;
        //            int intViewSize = Mathf.RoundToInt(clip.w);
        //            if ((intViewSize & 1) != 0) intViewSize -= 1;
        //            float halfViewSize = intViewSize * 0.5f;
        //            halfViewSize = Mathf.Round(halfViewSize);

        //            if (mPanel.clipping == UIDrawCall.Clipping.SoftClip)
        //                halfViewSize -= mPanel.clipSoftness.y;

        //            float contentSize = bmax.y - bmin.y;
        //            float viewSize = halfViewSize * 2f;
        //            float contentMin = bmin.y;
        //            float contentMax = bmax.y;
        //            float viewMin = clip.y - halfViewSize;
        //            float viewMax = clip.y + halfViewSize;

        //            contentMin = viewMin - contentMin;
        //            contentMax = contentMax - viewMax;

        //            UpdateScrollbars(VerticalBar, contentMin, contentMax, contentSize, viewSize, true);
        //        }
        //    }
        //    //else if (recalculateBounds)
        //    //{
        //    //    mCalculatedBounds = false;
        //    //}
        //}
        //void UpdateScrollbars(UIProgressBar slider, float contentMin, float contentMax, float contentSize, float viewSize, bool inverted)
        //{
        //    //QLoger.LOG("contentMin"+ contentMin+ " contentMax"+ contentMax+ "contentSize"+ contentSize+ "viewSize"+ viewSize);
        //    if (slider == null) return;
        //    {
        //        float contentPadding;

        //        if (viewSize < contentSize)
        //        {
        //            contentMin = Mathf.Clamp01(contentMin / contentSize);
        //            contentMax = Mathf.Clamp01(contentMax / contentSize);

        //            contentPadding = contentMin + contentMax;
        //            slider.value = inverted ? ((contentPadding > 0.001f) ? 1f - contentMin / contentPadding : 0f) :
        //                ((contentPadding > 0.001f) ? contentMin / contentPadding : 1f);
        //        }
        //        else
        //        {
        //            contentMin = Mathf.Clamp01(-contentMin / contentSize);
        //            contentMax = Mathf.Clamp01(-contentMax / contentSize);

        //            contentPadding = contentMin + contentMax;
        //            slider.value = inverted ? ((contentPadding > 0.001f) ? 1f - contentMin / contentPadding : 0f) :
        //                ((contentPadding > 0.001f) ? contentMin / contentPadding : 1f);

        //            if (contentSize > 0)
        //            {
        //                contentMin = Mathf.Clamp01(contentMin / contentSize);
        //                contentMax = Mathf.Clamp01(contentMax / contentSize);
        //                contentPadding = contentMin + contentMax;
        //            }
        //        }

        //        UIScrollBar sb = slider as UIScrollBar;
        //        if (sb != null) sb.barSize = 1f - contentPadding;
        //    }
        //}

        private IEnumerator InitScrollView()
        {
            //this.gameObject.SetActive(false);
            yield return null;
            mScroll.ResetPosition();
            yield return null;
            //SetVerticalBarSize();
            //this.gameObject.SetActive(true);
        }

	    //初始化UI部件
	    private bool InitUI()
	    {
		    if (ItemPrefab == null) {
			    projectQ.QLoger.ERROR("Prefab is Null!");
			    return false;
		    }

		    mTransform = transform;
		    mPanel = NGUITools.FindInParents<UIPanel>(gameObject);
		    //获取ScrollView
		    mScroll = mPanel.GetComponent<UIScrollView>();
		    if (mScroll == null)
			    return false;
		    mScroll.GetComponent<UIPanel>().onClipMove = OnMove;

		    //ScrollView 方向
		    if (mScroll.movement == UIScrollView.Movement.Horizontal)
			    mHorizontal = true;
		    else if (mScroll.movement == UIScrollView.Movement.Vertical)
			    mHorizontal = false;
		    else
			    return false;

		    //获取Grid
		    mGrid = GetComponent<UIGrid>();
		    if (mGrid == null)
			    return false;
		    //Grid 方向把
		    if (mGrid.arrangement == UIGrid.Arrangement.Horizontal)
			    mGridHorizontal = true;
		    else if (mGrid.arrangement == UIGrid.Arrangement.Vertical)
			    mGridHorizontal = false;
		    else
			    return false;

		    if (mGrid.maxPerLine == 0 && mGridHorizontal == mHorizontal) {
			    maxPerLineCount = 1;
		    } else if (mGrid.maxPerLine > 0 && mGridHorizontal != mHorizontal) {
			    maxPerLineCount = mGrid.maxPerLine;
		    } else {
			    return false;
		    }
		    return true;
	    }
	    #endregion 

	    #region 功能

	    //创建内容
	    private void CreateChild(int itemTotalCount)
	    {
		    //计算出需要多少个Child
		    int ChildCount = 0;

		    if (mHorizontal) {//如果是横向比的是宽
			    itemSize = mGrid.cellWidth;
			    ChildCount = Mathf.RoundToInt(mPanel.width / itemSize) + 1;
		    } else {
			    itemSize = mGrid.cellHeight;
			    ChildCount = Mathf.RoundToInt(mPanel.height / itemSize) + 1;
		    }

            //设置最大ChildCount
            MaxChildCount = Mathf.Max(MaxChildCount, ChildCount);

            int len = Mathf.Min(itemTotalCount, TotalChildCount);
            //复用之前的
            if (mChildren.Count > len)
            {
                for (int i = len; i < mChildren.Count; ++i)
                {
                    Destroy(mChildren[i].gameObject);
                }
                mChildren.RemoveRange(len,mChildren.Count - len);
            }
            else
            {
                for(int i = mChildren.Count; i <len; ++i)
                {
                    GameObject item = NGUITools.AddChild(gameObject, ItemPrefab);
                    mChildren.Add(item.transform);
                }
            }

            //创建出Child 存入List
            for (int i=0; i< mChildren.Count; ++i) {
                onChangeItem(i, mChildren[i].gameObject);
            }
		    mGrid.repositionNow = true;
	    }

	    //更新内容
	    public void WrapContent(bool isaaa = false)
	    {
		    bool allWithinRange = true;
		    if (mChildren.Count >= ItemTotalCount)
			    return;
            //获得总长度
            float totalExtents = GetTotalExtents();
            //获得中心点
            Vector3 center = GetCenter();
            if (mHorizontal) {
			    for (int i = 0, len = mChildren.Count; i < len; i+=maxPerLineCount) {
                    float PosX = 0;
				    Transform t = mChildren [i];

                    if (EditPos(t.localPosition,center,totalExtents,ref PosX) == 0)
                    {
                        continue;
                    }
                    int realIndex = GetRealIndex(PosX);
                    if (0 <= realIndex && realIndex < ItemTotalCount) {
					    UpdateItem (PosX, mChildren, i, realIndex, maxPerLineCount);
				    } else
					    allWithinRange = false;
			    }
		    } else {
			    for (int i = 0, len = mChildren.Count; i < len; i+=maxPerLineCount) {
                    float PosY = 0;
                    Transform t = mChildren [i];

                    if (EditPos(t.localPosition, center, totalExtents, ref PosY) == 0)
                    {
                        continue;
                    }

                    int realIndex = GetRealIndex(PosY);

                    if (0 <= realIndex && realIndex < ItemTotalCount) {
					    UpdateItem (PosY, mChildren, i, realIndex, maxPerLineCount);
				    } else
					    allWithinRange = false;
			    }
            }
		    mScroll.restrictWithinPanel = !allWithinRange;
            mScroll.constrainOnDrag = !allWithinRange;

        }

        /// <summary>
        /// 根据位置对子排序
        /// </summary>
        public void SortChildren()
        {
            //排序
            mChildren.Sort((tempTf1, tempTf2) =>
            {
                return ComparePos(tempTf1.localPosition, tempTf2.localPosition);
            });
        }

        /// <summary>
        /// 总长度
        /// </summary>
        /// <returns></returns>
        private float GetTotalExtents()
        {
            //所有Item 总长度 
            return itemSize * MaxChildCount;
        }

        /// <summary>
        /// 获得中心点
        /// </summary>
        /// <returns></returns>
        private Vector3 GetCenter()
        {
            //得到Panel 4个角的位置
            Vector3[] corners = mPanel.worldCorners;
            //转换到自己的相对位置
            for (int i = 0; i < 4; ++i)
            {
                Vector3 v = corners[i];
                v = mTransform.InverseTransformPoint(v);
                corners[i] = v;
            }

            //计算中心点 左下---右上 取一半
            return Vector3.Lerp(corners[0], corners[2], 0.5f);
        }

        /// <summary>
        /// 修改Pos值
        /// </summary>
        /// <param name="tf">child</param>
        /// <param name="CenterPoint">中心点的位置</param>
        /// <param name="halfExtents">总Item的一半</param>
        /// <returns></returns>
        private int EditPos(Vector3 itemLocalPos,Vector3 CenterPoint,float totalExtents,ref float posXorY)
        {
            float half = totalExtents * 0.5f;
            posXorY = 0f;
            if (mHorizontal)
            {
                //用Item的位置 - 中心点 得到距中心点的距离
                float distance = itemLocalPos.x - CenterPoint.x;
                posXorY = itemLocalPos.x;

                //判断如果小于整体子长度的一半 则表示在屏幕左边 如: 口【屏幕】
                if (distance < -half)
                {
                    //将x + 总长度  = 【屏幕】 口
                    posXorY += totalExtents;
                    return 1;
                }
                else if (distance > half)
                {
                    //将x - 总长度  = 口【屏幕】 
                    posXorY -= totalExtents;
                    return -1;
                }
            }
            else
            {
                //用Item的位置 - 中心点 得到距中心点的距离
                float distance = itemLocalPos.y - CenterPoint.y;
                posXorY = itemLocalPos.y;

                if (distance < - half)
                {
                    posXorY += totalExtents;
                    return 1;
                }
                else if (distance > half)
                {
                    posXorY -= totalExtents;
                    return -1;
                }
            }
            return 0;
        }

        /// <summary>
        /// 根据Transfrom获得真实Index
        /// </summary>
        /// <param name="tf"></param>
        /// <returns></returns>
        public int GetRealIndex(Transform tf)
        {
            if (mHorizontal)
                return GetRealIndex(tf.localPosition.x);
            else
                return GetRealIndex(tf.localPosition.y);
        }

        /// <summary>
        /// 根据X 或者 Y 获得真实Index
        /// </summary>
        /// <param name="tf"></param>
        /// <returns></returns>
        private int GetRealIndex(float PosXorY)
        {
            if(mHorizontal)
                return Mathf.RoundToInt(PosXorY / itemSize) * maxPerLineCount;
            else
                return Mathf.RoundToInt(PosXorY / itemSize) * -maxPerLineCount;
        }


        //是否可以删除
        private bool isCanRemove = true;
        public bool IsCanRemove
        {
            get { return isCanRemove; }
        }
        public void Remove(Transform tf, UITweener RemoveTween)
        {
            if (!isCanRemove) return;
            //禁止再次删除
            isCanRemove = false;
            //禁止拖动
            this.myScroll.IsCanDrag = false;
            //删除的物体 赋值
            RemoveTf = tf;
            //总数减少
            this.ItemTotalCount--;

            spList.Clear();
            //排序
            SortChildren();

            //取得当前Index
            int index = mChildren.IndexOf(tf);

            #region 设置动画
            for (int i= 0; i<mChildren.Count; ++i)
            {
                SpringPosition sp = mChildren[i].GetComponent<SpringPosition>();
                if (i <= index)
                {
                    if (sp != null)
                    {
                        Destroy(sp);
                    }
                }
                else
                {
                    if (sp == null)
                        sp = mChildren[i].gameObject.AddComponent<SpringPosition>();
                    sp.enabled = false;

                    sp.target = GetPosByIndex(mChildren[0].localPosition,i-1);
                    spList.Add(sp);
                }
            }
            #endregion
            //如果
            if (this.ItemTotalCount >= mChildren.Count)
            {
                float total = this.GetTotalExtents();
                Vector3 centerPoint = this.GetCenter();
                //得到最下边的位置
                RemovePos = GetPosByIndex(mChildren[0].localPosition, mChildren.Count - 1);
                float posY = 0; 
                this.EditPos(RemovePos, centerPoint, total, ref posY);
                int realIndex = this.GetRealIndex(posY);

                if (realIndex >= ItemTotalCount - 1)
                {
                    RemovePos = GetPosByIndex(mChildren[0].localPosition, -1);
                }
            }
            
            EventDelegate.Remove(RemoveTween.onFinished, OnRemoveTweenFinished);
            EventDelegate.Add(RemoveTween.onFinished, OnRemoveTweenFinished);
            //启动删除动画
            RemoveTween.ResetToBeginning();
            RemoveTween.PlayForward();
        }
        List<SpringPosition> spList = new List<SpringPosition>();
        Transform RemoveTf;
        Vector3 RemovePos;

        private void OnRemoveTweenFinished()
        {
            if (spList.Count == 0)
            {
                RemoveEnd();
            }
            else
            {
                for (int i = 0; i < spList.Count; i++)
                {
                    if (i == spList.Count - 1)
                        spList[i].onFinished = () =>
                        {
                            RemoveEnd();
                        };

                    else
                        spList[i].onFinished = null;
                    spList[i].enabled = true;
                }
            }
        }

        private void RemoveEnd()
        {

            if (this.ItemTotalCount >= mChildren.Count)
            {
                RemoveTf.localPosition = RemovePos;
                int realIndex = Mathf.RoundToInt(RemovePos.y / itemSize) * -maxPerLineCount;
                onChangeItem(realIndex, RemoveTf.gameObject);
            }
            else
            {
                mChildren.Remove(RemoveTf);
                Destroy(RemoveTf.gameObject);
            }
            isCanRemove = true;
            this.myScroll.IsCanDrag = true;
        }

        private int ComparePos(Vector3 pos1,Vector3 pos2)
        {
            if(mHorizontal)
            {
                if (pos1.x > pos2.x) return 1;
                if (pos1.x < pos2.x) return -1;
                if (pos1.y > pos2.y) return -1;
                if (pos1.y > pos2.y) return 1;

                return 0;
            }
            else
            {
                if (pos1.y > pos2.y) return -1;
                if (pos1.y > pos2.y) return 1;
                if (pos1.x > pos2.x) return 1;
                if (pos1.x < pos2.x) return -1;
                return 0;
            }
        }

        private Vector3 GetPosByIndex(Vector3 posFirst,int index)
        {
            Vector3 result = new Vector3();
            float x = posFirst.x;
            float y = posFirst.y;
            if(mHorizontal)
            {
                result.x = x + itemSize * index;
                result.y = y; 
            }
            else
            {
                result.x = x;
                result.y = y - itemSize * index;
            }
            return result;
        }

        /// <summary>
        /// 更新Item内容
        /// </summary>
        private void UpdateItem(float pos,List<Transform> itemList,int index,int readIndex,int length)
	    {
		    for(int i=0; i<length && readIndex + i < ItemTotalCount; ++i)
		    {
			    Vector3 tempPos = itemList[index+i].localPosition;
			    if(mHorizontal)
			    {
				    tempPos.x = pos;
			    }else{
				    tempPos.y = pos;
			    }
			    itemList[index+i].localPosition = tempPos;
			    if (onChangeItem != null)
			    {
				    onChangeItem(readIndex+i,itemList[index+i].gameObject);
			    }
		    }
	    }



	    #endregion

	    #region 生命周期
	    void OnEnable()
	    {
		    //Init (10);
	    }
	    #endregion

	    #region Event
	    void OnMove(UIPanel panel)
	    {
		    WrapContent();
	    }
	    #endregion
    }
}
