/**
 * 作者：周腾
 * 作用：
 * 日期：
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace projectQ
{

    public class Area
    {
        public int RegionID;
        public string Region;
        public List<PlayType> typeList;
    }
    public class PlayType
    {
        public int CityID;
        public string City;
    }

    public class SortPanelCityData : MonoBehaviour
    {
        //private List<Area> areaList = new List<Area>();
        [HideInInspector]
        public List<SelectRegion> cityTableDataList = new List<SelectRegion>();
        [HideInInspector]
        public Dictionary<int, int> typeAreaDict = new Dictionary<int, int>();//市，省
        [HideInInspector]
        public Dictionary<int, Area> areaDict = new Dictionary<int, Area>();
        private List<PlayType> typeList = new List<PlayType>();
        [HideInInspector]
        public List<SortItemData> cityItemDataList = new List<SortItemData>();//所有玩法列表
        public SortPanel panel;
        [HideInInspector]
        public List<string> areaNameList = new List<string>();
        [HideInInspector]
        public List<int> areaIdList = new List<int>();
        public void CityTableData_LoadXml()
        {
            //XmlAreaTypeData data = new XmlAreaTypeData();
            //XmlData.XmlInit<XmlAreaTypeData>(data);
            //cityTableDataList = new List<AreaTypeData>();
            //cityTableDataList = XmlAreaTypeData.CityTableList;
            if (cityTableDataList.Count <= 0)
            {
                List<BaseXmlBuild> buildList = MemoryData.XmlData.XmlBuildDataDic["SelectRegion"];
                foreach (BaseXmlBuild build in buildList)
                {
                    SelectRegion info = (SelectRegion)build;
                    cityTableDataList.Add(info);
                }
            }
           

        }
        public void InitTypeAreaDict()
        {
            typeAreaDict.Clear();
            for (int i = 0; i < cityTableDataList.Count; i++)
            {
                //if (typeAreaDict.ContainsKey(int.Parse(cityTableDataList[i].CityID)))
                //{
                //    continue;
                //}
                typeAreaDict.Add(int.Parse(cityTableDataList[i].CityID), int.Parse(cityTableDataList[i].RegionID));
            }
        }
        public void InitAreaType()
        {

            areaDict.Clear();
            List<int> list = GetAreaId();
            for (int j = 0; j < list.Count; j++)
            {
                List<PlayType> typeList = new List<PlayType>();
                for (int i = 0; i < cityTableDataList.Count; i++)
                {
                    PlayType type = new PlayType();
                    type.CityID = int.Parse(cityTableDataList[i].CityID);
                    type.City = cityTableDataList[i].CityName;
                    if (int.Parse(cityTableDataList[i].RegionID) == list[j])
                    {
                        typeList.Add(type);
                    }
                }
                Area area = new Area();
                area.RegionID = list[j];
                area.Region = GetAreaName()[j];
                area.typeList = typeList;
                areaDict.Add(list[j], area);
            }
        }
        public void XMLToData(int areaId)
        {
            cityItemDataList.Clear();
            SortItemData data = new SortItemData();
            data.cityId = 0;
            data.cityName = "全国";
            data.OnClick = panel.OnItemClick;
            cityItemDataList.Add(data);
            //之前的初始化选单的方法，需求已更改，方法先保留
            //if (areaDict.ContainsKey(areaId))
            //{
            //    List<PlayType> typeList = areaDict[areaId].typeList;
            //    for (int j = 0; j < typeList.Count; j++)
            //    {
            //        SortItemData data1 = new SortItemData();
            //        data1.cityId = typeList[j].CityID;
            //        data1.cityName = typeList[j].City;
            //        data1.OnClick = panel.OnItemClick;
            //        cityItemDataList.Add(data1);
            //    }
            //}

            if (panel == null || panel.parlorList == null)
                return;

            foreach (var item in areaDict)
            {
                //临时添加的过滤城市选单有没有麻将馆的方法，调用比较混乱，可能有风险
                if (!panel.parlorList.CheckIsHaveRoom(item.Value.RegionID))
                    continue;
                SortItemData data1 = new SortItemData();
                data1.cityId = item.Value.RegionID;
                data1.cityName = item.Value.Region;
                data1.OnClick = panel.OnItemClick;
                cityItemDataList.Add(data1);
            }
        }

        public int CheckRegionIDByCityID(int CityID)
        {
            foreach (var item in areaDict)
            {
                if (item.Key == CityID)
                    return item.Value.RegionID;
            }

            //foreach (var item in areaDict.Values)
            //{
            //    for (int i = 0; i < item.typeList.Count; i++)
            //    {
            //        if(item.typeList[i].CityID == CityID)
            //        {
            //            return item.RegionID;
            //        }
            //    }
            //}

            return 0;
        }


        /// <summary>
        /// 获取地区名字
        /// </summary>
        public List<string> GetAreaName()
        {
            for (int i = 0; i < cityTableDataList.Count; i++)
            {
                if (areaNameList.Contains(cityTableDataList[i].RegionName))
                {
                    continue;
                }
                else
                {
                    areaNameList.Add(cityTableDataList[i].RegionName);
                }

            }
            return areaNameList;
        }
        /// <summary>
        /// 获取地区ID
        /// </summary>
        /// <returns></returns>
        public List<int> GetAreaId()
        {
            for (int i = 0; i < cityTableDataList.Count; i++)
            {
                if (areaIdList.Contains(int.Parse(cityTableDataList[i].RegionID)))
                {
                    continue;
                }
                else
                {
                    areaIdList.Add(int.Parse(cityTableDataList[i].RegionID));
                }

            }
            return areaIdList;
        }
    }

}
