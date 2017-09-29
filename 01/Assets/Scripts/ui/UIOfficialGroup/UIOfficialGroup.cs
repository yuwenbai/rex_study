/**
 * @Author lyb
 * 官方麻将试玩群
 *
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace projectQ
{
    public class UIOfficialGroup : MonoBehaviour
    {
        public UIGrid GridObj;
        /// <summary>
        /// 更多按钮
        /// </summary>
        public UIOfficialGroupList GroupMoreObj;

        void OnEnable()
        {
            //GameConfig.Instance.OfficialGroupSuccEvent += new GameConfig.OnOfficialGroupSuccEvent(this.OfficialGroupInit);

            //EventDispatcher.FireSysEvent(GEnum.NamedEvent.EGetGameData, GEnum.NamedGameData.EC2SOfficialGroupList);
        }

        void OnDisable()
        {
            //GameConfig.Instance.OfficialGroupSuccEvent -= new GameConfig.OnOfficialGroupSuccEvent(this.OfficialGroupInit);
        }

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
                officialconfig info = (officialconfig)build;

                if (info.ID != "0")
                {
                    if (index < 6)
                    {
                        OfficialGroupList.Add(info);

                        index++;
                    }
                }
                else
                {
                    GroupMoreObj.OfficialGroupListInit(info);
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
                UIOfficialGroupList groupList = go.GetComponent<UIOfficialGroupList>();
                groupList.OfficialGroupListInit(gData);
            });
            GridObj.Reposition();
        }
    }
}
