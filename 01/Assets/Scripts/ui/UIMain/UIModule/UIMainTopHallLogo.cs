/**
 * @Author lyb
 * 大厅Logo的逻辑控制脚本
 *
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace projectQ
{
    public class UIMainTopHallLogo : UIMainBase
    {
        /// <summary>
        /// 地区名字
        /// </summary>
        public UILabel RegionLab;
        /// <summary>
        /// 切换区域
        /// </summary>
        public GameObject RegionSwitchBtn;

        private void Awake()
        {
            UIEventListener.Get(RegionSwitchBtn).onClick = OnRegionSwitchBtnClick;
        }

        void Start(){}

        public void HallLogoInit()
        {
            UpdateRegion();
        }

        /// <summary>
        /// 更新地区
        /// </summary>
        public void UpdateRegion()
        {
            string regionName = MemoryData.XmlData.XmlBuildDataRegion_Get();
            if (regionName != "")
            {
                string str = regionName.Substring(0, 2);
                RegionLab.text = string.Format("{0}专区", str);
            }
            else
            {
                RegionLab.text = "请选择地方玩法";
            }
        }

        /// <summary>
        /// 切换区域按钮点击事件响应
        /// </summary>
        private void OnRegionSwitchBtnClick(GameObject go)
        {
            EventDispatcher.FireSysEvent(GEnum.NamedEvent.SysUI_UIMap_Open);
        }
    }
}
