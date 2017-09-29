/**
 * @Author lyb
 * 地区选择小窗按钮
 *
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace projectQ
{
    public class UIMapSelectBtn : MonoBehaviour
    {
        public delegate void MapSelectBtnDelegate(int id);
        public MapSelectBtnDelegate OnClickCallBack;

        /// <summary>
        /// 地区名字
        /// </summary>
        public UILabel SelectName;

        private int AreaId;

        void Start() { }

        /// <summary>
        /// 初始化按钮数据
        /// </summary>
        public void MapSelectBtnInit(string areaId)
        {
            AreaId = int.Parse(areaId);

            string AreaName = MemoryData.XmlData.XmlBuildDataSole_Get("RegionUnLock", "RegionID", areaId, "RegionName");

            SelectName.text = AreaName;
        }

        void OnClick()
        {
            if (OnClickCallBack != null)
            {
                OnClickCallBack(AreaId);
            }
        }
    }
}