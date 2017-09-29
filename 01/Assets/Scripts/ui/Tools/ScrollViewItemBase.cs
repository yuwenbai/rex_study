/****************************************************
 *  Copyright(C) 2015 智幻点击
 *	版权所有。
 *	作者:于秋辰
 *	创建时间：8/19/2016
 *	文件名：  ScrollViewItemBase.cs
 *	文件功能描述：
 *  创建标识：yqc.8/19/2016
 *	创建描述：
 *
 *  修改标识：
 *  修改描述：
 *
 *
 *
 *****************************************************/
using UnityEngine;
using System.Collections;

namespace UI.ScrollViewTool
{
    public abstract class ScrollViewItemBase<T> : MonoBehaviour {
        public T UIData; 

        public virtual void SetData(T data,bool isRefresh)
        {
            this.UIData = data;
            if (isRefresh)
                this.Refresh();
        }
        public abstract void Refresh();
    }
}
