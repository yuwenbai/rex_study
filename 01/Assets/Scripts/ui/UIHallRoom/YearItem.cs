/**
 * 作者：周腾
 * 作用：
 * 日期：
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UI.ScrollViewTool;
namespace projectQ
{
    public class YearData
    {
        public int year;
        public int tickets;
        public int income;
        public List<MonthItemData> monthSaleData;
        public Action<YearData, GameObject> OnClick;

    }

    public class YearItem :MonoBehaviour// ScrollViewItemBase<YearData>
    {
        [HideInInspector]
        public int year;
        [HideInInspector]
        public bool isOnCenter;
        public UILabel yearItem;
        private Vector3 scale;
        public void InitYearItem(int year)
        {
            yearItem.text = year.ToString();
            this.year = year;
            scale = new Vector3(0.7f, 0.7f, 0.7f);
        }

        public void SetCenter()
        {
            gameObject.transform.localScale = Vector3.one;
            yearItem.alpha = 1f;

        }

        public void SetNoCenter()
        {
            gameObject.transform.localScale = scale;
            yearItem.alpha = 0.3f;
        }

    }
}
