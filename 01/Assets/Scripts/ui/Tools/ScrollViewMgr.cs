/**
 *	作者:YQC
**/
using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using UI.ScrollViewTool;
namespace UI.ScrollViewTool
{
    public class ScrollViewMgr <T> {
        #region Unity Inspector Fields
        //ScrollView 优化脚本
        public ScrollViewWrapContent script;
        #endregion

        #region 属性
        private List<T> UIDatas;

        private List<ScrollViewItemBase<T>> ChildrenTotal;
        #endregion

        #region 回调函数
        public delegate void VoidDelegate();
        public delegate void VoidDelegateGO(GameObject Item, object data);
        #endregion


        #region API
        public void Init(ScrollViewWrapContent script)
        {
            this.script = script;
        }

        //刷新ScrollView
        public void RefreshScrollView(List<T> data)
        {
            if (this.script == null)
                return;
            if (data == null)
            {
                this.UIDatas = null;
                return;
            }

            this.UIDatas = data;
            this.script.onChangeItem = OnInitializeItem;
            this.script.Init(this.UIDatas.Count);
            UpdateChildRen();
        }
        private void UpdateChildRen()
        {
            ChildrenTotal = null;
            var tempChild = this.script.Children;
            if (tempChild != null)
            {
                ChildrenTotal = new List<ScrollViewItemBase<T>>(tempChild.Count);
                for (int i = 0; i < tempChild.Count; i++)
                {
                    var component = tempChild[i].GetComponent<ScrollViewItemBase<T>>();
                    ChildrenTotal.Add(component);
                }
            }
        }

        public void RefreshData(List<T> data = null)
        {
            if (data != null)
            {
                if(data.Count > this.UIDatas.Count)
                {

                }
                this.UIDatas = data;
            }

            if(ChildrenTotal != null && ChildrenTotal.Count > 0)
            {
                for (int i = 0; i < ChildrenTotal.Count; i++)
                {
                    ChildrenTotal[i].Refresh();
                }
            }
        }

        //删除Item
        public bool DelItem(ScrollViewItemBase<T> item,UITweener removeTween)
        {
            if(this.UIDatas != null && this.UIDatas.Count > 0 && this.script.IsCanRemove)
            {
                this.UIDatas.Remove(item.UIData);
                this.script.Remove(item.transform, removeTween);
                return true;
            }
            return false;
        }
        #endregion

        #region Event
        void OnInitializeItem(int index, GameObject go)
        {
            ScrollViewItemBase<T> scr = go.GetComponent<ScrollViewItemBase<T>>();
            scr.SetData(UIDatas[index],true);
        }
        #endregion
    }
}
