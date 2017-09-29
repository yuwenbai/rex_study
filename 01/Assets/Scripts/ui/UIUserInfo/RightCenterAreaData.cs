using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace projectQ
{
    public class RightCenterAreaData : MonoBehaviour
    {
        public UIUserInfoRightCenterControll UI;
        [HideInInspector]
        public List<SelectRegion> areaDataList = new List<SelectRegion>();
        [HideInInspector]
        public List<string> areaNameList = new List<string>();
        [HideInInspector]
        public List<int> areaIdList = new List<int>();

        /// <summary>
        /// 读取xml
        /// </summary>
        public void AreaData_LoadXml()
        {
            //XmlAreaTypeData data = new projectQ.XmlAreaTypeData();
            //XmlData.XmlInit<XmlAreaTypeData>(data);
            //areaDataList = new List<AreaTypeData>();
            //areaDataList = XmlAreaTypeData.CityTableList;
            if (areaDataList.Count <= 0)
            {
                List<BaseXmlBuild> buildList = MemoryData.XmlData.XmlBuildDataDic["SelectRegion"];
                foreach (BaseXmlBuild build in buildList)
                {
                    SelectRegion info = (SelectRegion)build;
                    areaDataList.Add(info);
                }
            }
        }
        /// <summary>
        /// 获取地区
        /// </summary>
        public List<string> GetAreaName()
        {
            for (int i = 0; i < areaDataList.Count; i++)
            {
                if (areaNameList.Contains(areaDataList[i].RegionName))
                {
                    continue;
                }
                else
                {
                    areaNameList.Add(areaDataList[i].RegionName);
                }
              
            }
            return areaNameList;
        }

        public List<int> GetAreaId()
        {
            for (int i = 0; i < areaDataList.Count; i++)
            {
                if (areaIdList.Contains(int.Parse(areaDataList[i].RegionID)))
                {
                    continue;
                }
                else
                {
                    areaIdList.Add(int.Parse(areaDataList[i].RegionID));
                }

            }
            return areaIdList;
        }
    }
}
