/**
 * @Author lyb
 * 地区选择小窗
 *
 */

using UnityEngine;
using System.Collections.Generic;

namespace projectQ
{
    public class UIMapSelect : MonoBehaviour
    {
        public UIGrid GridPanelObj;
        public GameObject BaseMaskObj;

        private UIMapList MapList;

        void Start()
        {
            UIEventListener.Get(BaseMaskObj).onClick = OnBaseMaskClick;
        }

        /// <summary>
        /// 背景遮罩响应
        /// </summary>
        void OnBaseMaskClick(GameObject obj)
        {
            GameObject.Destroy(gameObject);
        }

        /// <summary>
        /// 初始化当前地区要创建的按钮数据
        /// </summary>
        public void MapSelectInit(string areaStr, UIMapList mList)
        {
            MapList = mList;

            string[] values = areaStr.Split(new char[] { ';' });

            List<string> areaList = new List<string>(values);

            SelectListCreat(areaList);
        }

        /// <summary>
        /// 创建小框的按钮
        /// </summary>
        void SelectListCreat(List<string> areaList)
        {
            UITools.CreateChild<string>(GridPanelObj.transform, null, areaList, (go, areaId) =>
            {
                UIMapSelectBtn selectBtnList = go.GetComponent<UIMapSelectBtn>();
                selectBtnList.MapSelectBtnInit(areaId);

                selectBtnList.OnClickCallBack = C2CMapSelectBtnCallBack;
            });
            GridPanelObj.Reposition();
        }

        /// <summary>
        /// 选择框选择地区回调
        /// </summary>
        void C2CMapSelectBtnCallBack(int AreaId)
        {
            MapList.C2CMapSelectCallBack(AreaId);
        }
    }
}