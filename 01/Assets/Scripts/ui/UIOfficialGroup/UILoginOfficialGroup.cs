/**
 * @Author lyb
 * 登录界面官方麻将试玩群通过数据表创建
 *
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace projectQ
{
    public class UILoginOfficialGroup : MonoBehaviour
    {
        public UIGrid GridObj;

        void Start()
        {
            OfficialGroup_LoadXml();
        }

        /// <summary>
        /// 初始化麻将试玩群xml数据。仅适用于登录界面
        /// </summary>
        public void OfficialGroup_LoadXml()
        {
            List<officialconfig> OfficialGroupList = new List<officialconfig>();

            List<BaseXmlBuild> buildList = MemoryData.XmlData.XmlBuildDataDic["officialconfig"];

            int index = 0;

            foreach (BaseXmlBuild build in buildList)
            {
                if (index < 6)
                {
                    officialconfig info = (officialconfig)build;

                    if (info.ID != "0")
                    {
                        OfficialGroupList.Add(info);

                        index++;
                    }
                }
            }

            OfficialGroupInit(OfficialGroupList);
        }

        /// <summary>
        /// 初始化
        /// </summary>
        public void OfficialGroupInit(List<officialconfig> OfficialGroupList)
        {
            UITools.CreateChild<officialconfig>(GridObj.transform, null, OfficialGroupList, (go, gData) =>
            {
                UILabel groupLabel = go.GetComponentInChildren<UILabel>();
                groupLabel.text = gData.PlayName;
            });
            GridObj.Reposition();
        }
    }
}