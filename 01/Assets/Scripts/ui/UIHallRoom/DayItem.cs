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

    public class DayItemData
    {
        public int year;
        public int month;
        public int day;
        public int card;
        public int calcu;
    }
    /// <summary>
    /// 紫色以往有数据的天，橙色以往无数据的，绿色当天，灰色未到的天
    /// </summary>
    public class DayItem : ScrollViewItemBase<DayItemData>//MonoBehaviour
    {

        public UILabel dayLabel;
        public UILabel cardLabel;
        public UILabel calcuLabel;


        private int currentYear;
        private int currentMonth;
        private int currentDay;
        public override void Refresh()
        {
            GetCurrentTime();
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.Append(this.UIData.day);
            dayLabel.text = sb.ToString();
            sb.Length = 0;
            sb.Append(this.UIData.card);
            cardLabel.text = sb.ToString();
            sb.Length = 0;
            sb.Append(this.UIData.calcu);
            calcuLabel.text = sb.ToString();
            sb.Length = 0;

            SetBgColor(this.UIData.year, this.UIData.month, this.UIData.day, this.UIData.card, this.UIData.calcu);
            //TestSetBgColor(this.UIData.day);
        }
        void TestSetBgColor(int day)
        {
            UISprite sprite = gameObject.GetComponent<UISprite>();
            if (day > 0 && day < 8)
            {
                sprite.color = new Color(127.0f / 255.0f, 214.0f / 255.0f, 43.0f / 255.0f);//绿色
            }
            else if (day > 8 && day < 15)
            {
                sprite.color = new Color(161.0f / 255.0f, 161.0f / 255.0f, 161.0f / 255.0f);//灰色
            }
            else if (day > 15 && day < 25)
            {
                sprite.color = new Color(131.0f / 255.0f, 71.0f / 255.0f, 242.0f / 255.0f);//紫色
            }
            else 
            {
                sprite.color = new Color(233.0f / 255.0f, 170.0f / 255.0f, 69.0f / 255.0f);//橙色
            }
        }
        void SetBgColor(int year,int month,int day,int card,int calcu)
        {
            UISprite sprite = gameObject.GetComponent<UISprite>();
            if (this.currentYear == year && this.currentMonth == month && this.currentDay == day)
            {
                sprite.color = new Color(127.0f / 255.0f, 214.0f / 255.0f, 43.0f / 255.0f);//绿色
            }
            else
            {
                if (this.currentYear == year && this.currentMonth == month && this.currentDay < day)
                {
                    sprite.color = new Color(161.0f / 255.0f, 161.0f / 255.0f, 161.0f / 255.0f);//灰色
                }
                else
                {
                    if (card != 0 || calcu != 0)
                    {
                        sprite.color = new Color(131.0f / 255.0f,  71.0f / 255.0f, 242.0f / 255.0f);//紫色
                    }
                    else
                    {
                        sprite.color = new Color(233.0f / 255.0f, 170.0f / 255.0f, 69.0f / 255.0f);//橙色
                    }
                }
            }
        }


        private void GetCurrentTime()
        {
            System.DateTime currentTime = new System.DateTime();
            currentTime = System.DateTime.Now;
            currentYear = currentTime.Year;
            currentMonth = currentTime.Month;
            currentDay = currentTime.Day;
        }
    }

}
