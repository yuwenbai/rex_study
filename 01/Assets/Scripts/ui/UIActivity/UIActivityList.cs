/**
 * @Author lyb
 * 单个活动控制脚本
 *
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace projectQ
{
    public class UIActivityList : MonoBehaviour
    {
        public delegate void OnActivityClickDelegate(ActivityData aData);
        public OnActivityClickDelegate ActivityClickCallBack;

        /// <summary>
        /// 活动名字
        /// </summary>
        public UILabel ActivityName;

        private ActivityData activityData;

        /// <summary>
        /// 初始化
        /// </summary>
        public void ActivityListInit(ActivityData aData, OnActivityClickDelegate aCallBack)
        {
            ActivityClickCallBack = aCallBack;

            activityData = aData;

            ActivityName.text = aData.ActivityName;
        }

        public void OnClick()
        {
            if (ActivityClickCallBack != null)
            {
                ActivityClickCallBack(activityData);
            }
        }
    }
}