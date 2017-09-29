/**
 * 作者：周腾
 * 作用：
 * 日期：
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UI.ScrollViewTool;
using System;

namespace projectQ
{

    public class MonthItemData
    {
        public int year;
        public int month;
        public List<DayItemData> daySales;//这个月里每一天的信息
        public int tickets;//这个月桌卡销售量
        public int income;//这个月收入


    }

    public class MonthItem : MonoBehaviour
    {

        public UILabel monthLabel;
        [HideInInspector]
        public int month;
        [HideInInspector]
        public int year;
        [HideInInspector]
        public bool isOnCenter = false;
        private Vector3 scale;
        public void InitLabel(int month, int year)
        {
            if (month < 10)
            {
                monthLabel.text = "0" + month + "月";
            }
            else
            {
                monthLabel.text = month + "月";
            }
         
            this.month = month;
            this.year = year;
            scale = new Vector3(0.7f, 0.7f, 0.7f);
        }

        public void SetCenter()
        {
            gameObject.transform.localScale = Vector3.one;
            monthLabel.alpha = 1f;

        }

        public void SetNoCenter()
        {
            gameObject.transform.localScale = scale;
            monthLabel.alpha = 0.3f;
        }
    }

}
