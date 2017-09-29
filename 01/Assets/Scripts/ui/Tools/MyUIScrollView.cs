/**
 * @Author YQC
 *
 *
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UI.ScrollViewTool
{
    public class MyUIScrollView : UIScrollView {
        //优化脚本
        private ScrollViewWrapContent _warpContentScript;
        public ScrollViewWrapContent WarpContentScript
        {
            get
            {
                if (_warpContentScript == null)
                    _warpContentScript = GetComponentInChildren<ScrollViewWrapContent>();
                return _warpContentScript;
            }
        }

        [HideInInspector]
        public bool IsCanDrag = true;

        protected override bool shouldMove
        {
            get
            {
                if (!IsCanDrag) return false;
                return base.shouldMove;
            }
        }

        private Bounds MyBounds;
        public override Bounds bounds
        {
            get
            {
                if (!mCalculatedBounds || MyBounds == null)
                {
                    MyBounds = base.bounds;

                    if (WarpContentScript == null|| (horizontalScrollBar == null && verticalScrollBar == null) || WarpContentScript.Children == null || WarpContentScript.Children.Count == 0) return MyBounds;
                    //先排序
                    WarpContentScript.SortChildren();
                    //拿到第一个
                    Transform firstTf = WarpContentScript.Children[0];
                    Transform lastTf = WarpContentScript.Children[WarpContentScript.Children.Count - 1];
                    int firstIndex = WarpContentScript.GetRealIndex(firstTf) / WarpContentScript.MaxPerLineCount;
                    int lastIndex = WarpContentScript.GetRealIndex(lastTf) / WarpContentScript.MaxPerLineCount;

                    int MaxCount = WarpContentScript.ItemTotalCount / WarpContentScript.MaxPerLineCount;

                    Vector3 size;
                    Vector3 center;
                    if (WarpContentScript.Horizontal)
                    {
                        float realTotalExtents = MyBounds.extents.x * 2;
                        float chayi = (WarpContentScript.ItemSize * WarpContentScript.Children.Count) - realTotalExtents;

                        float firstPos = MyBounds.min.x - (WarpContentScript.ItemSize * firstIndex);
                        float lastPos = MyBounds.max.x + (WarpContentScript.ItemSize * (MaxCount - lastIndex - 1));

                        size = new Vector3(Mathf.Abs(lastPos - firstPos - chayi) + WarpContentScript.ItemSize* 0.5f, Mathf.Abs(MyBounds.max.y - MyBounds.min.y), 0f);
                        center = new Vector3((lastPos - firstPos - chayi) * 0.5f - WarpContentScript.ItemSize * 0.25f, MyBounds.max.y - MyBounds.min.y, 0f);
                    }
                    else
                    {
                        float realTotalExtents = MyBounds.extents.y * 2;
                        float chayi = (WarpContentScript.ItemSize * WarpContentScript.Children.Count) - realTotalExtents;

                        float firstPos = MyBounds.max.y + (WarpContentScript.ItemSize * firstIndex);
                        float lastPos = MyBounds.min.y - (WarpContentScript.ItemSize * (MaxCount - lastIndex - 1));

                        size = new Vector3(Mathf.Abs(MyBounds.max.x - MyBounds.min.x), Mathf.Abs(lastPos - firstPos - chayi), 0f);
                        center = new Vector3(MyBounds.max.x - MyBounds.min.x, (lastPos - firstPos - chayi) * 0.5f, 0f);
                    }
                    this.MyBounds = new Bounds(center, size);
                }
                return MyBounds;
            }
        }

    }
}