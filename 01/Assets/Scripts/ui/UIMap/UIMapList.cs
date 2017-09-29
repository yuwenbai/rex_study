/**
 * @Author lyb
 * 地区选择
 *
 */

using UnityEngine;
using System.Collections.Generic;

namespace projectQ
{
    public class UIMapList : MonoBehaviour
    {
        public delegate void MapAreaBtnDelegate(int id);
        public MapAreaBtnDelegate OnClickCallBack;

        /// <summary>
        /// 区域ID
        /// </summary>
        public int AreaId;
        /// <summary>
        /// 区域Sprite
        /// </summary>
        public UISprite AreaSpr;

        private string RegionSelectStr;
        private string SelectPositionStr;
        private GameObject MapSelectObj;
        private GameObject UIMapObj;

        /// <summary>
        /// 地图区域初始化
        /// </summary>
        public void MapAreaInit(GameObject mapObj)
        {
            UIMapObj = mapObj;

            AreaId = int.Parse(AreaSpr.spriteName);

            List<BaseXmlBuild> buildList = MemoryData.XmlData.XmlBuildDataDic["RegionUnLock"];

            foreach (BaseXmlBuild bData in buildList)
            {
                RegionUnLock regionData = (RegionUnLock)bData;

                if (regionData.RegionID.Equals(AreaId.ToString()))
                {
                    SetMapUnLockValue(regionData.IsUnLock == "0" ? false : true);

                    RegionSelectStr = regionData.RegionSelect;
                    SelectPositionStr = regionData.SelectPosition;

                    break;
                }
            }
        }

        /// <summary>
        /// 设置地图解锁数据
        /// </summary>
        void SetMapUnLockValue(bool isBol)
        {
            gameObject.GetComponent<UIDefinedButton>().isEnabled = isBol;
        }

        void OnClick()
        {
            if (!MapSelectPanelCreat())
            {
                C2CMapSelectCallBack(AreaId);
            }
        }

        /// <summary>
        /// 创建地区选择面板
        /// </summary>
        private bool MapSelectPanelCreat()
        {
            if (RegionSelectStr != "-1" && SelectPositionStr != "-1")
            {
                if (MapSelectObj == null)
                {
                    string[] values = SelectPositionStr.Split(new char[] { ';' });

                    GameObject prefab = ResourcesDataLoader.Load<GameObject>(GameAssetCache.Prefab_UIMapSelect_Path);

                    MapSelectObj = NGUITools.AddChild(UIMapObj, prefab);

                    MapSelectObj.transform.localPosition = new Vector3(float.Parse(values[0]), float.Parse(values[1]), 0.0f);

                    UIMapSelect mapSelect = MapSelectObj.GetComponent<UIMapSelect>();

                    mapSelect.MapSelectInit(RegionSelectStr, this);
                }

                return true;
            }

            return false;
        }

        /// <summary>
        /// 选择框选择地区回调
        /// </summary>
        public void C2CMapSelectCallBack(int AreaId)
        {
            if (MapSelectObj != null)
            {
                GameObject.Destroy(MapSelectObj);
            }

            if (OnClickCallBack != null)
            {
                OnClickCallBack(AreaId);
            }
        }
    }
}