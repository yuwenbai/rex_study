/****************
 * 
 * @Author GarFey
 * 
 * ***************/
using System.Collections.Generic;
using UnityEngine;

namespace projectQ
{
    public class CreateRoomBottom : MonoBehaviour
    {
        /// <summary>
		/// 切换按钮
		/// </summary>
        public GameObject SwitchBtn;
        /// <summary>
        ///  地方特色玩法 全部 Ojb
        /// </summary>
        public GameObject FullObj;
        /// <summary>
        /// 地方特色玩法 少数 《=5 Obj
        /// </summary>
        public GameObject NumObj;
        /// <summary>
		/// 地区玩法空数据显示 只有少数里会有，全部里没有
		/// </summary>
        public UILabel RegionNullTatile;
        /// <summary>
		/// 地区麻将名称
		/// </summary>
		public UILabel RegionName;
        /// <summary>
		/// 地方特色玩法创建栏Grid 少数 《=5
		/// </summary>
		public UIGrid GridNumRegionGrid;
        /// <summary>
        /// 地方特色玩法创建栏Grid 全部
        /// </summary>
        public UIGrid GridFullRegionGrid;
        /// <summary>
        /// 特殊5个按钮是排列
        /// </summary>
        public GameObject FiveRegionObj;
        void Awake()
        {
            UIEventListener.Get(SwitchBtn).onClick = SwitchBtnClick;
        }
        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="_list"></param>
        public void InitBottom()
        {
            List<MahjongPlay> regionList = MemoryData.MahjongPlayData.LocalRegionPlayList;
            string regionName = "";
            if (MemoryData.PlayerData.MyPlayerModel.PlayerDataBase.RegionID == 0)
            {
                regionName = "未选择地区";
            }
            else
            {
                regionName = MemoryData.XmlData.XmlBuildDataRegion_Get(MemoryData.PlayerData.MyPlayerModel.PlayerDataBase.RegionID);
            }
            RegionName.text = regionName;

            switch(regionList.Count)
            {
                case 0:
                    
                    FullObj.SetActive(false);
                    NumObj.SetActive(true);
                    FiveRegionObj.SetActive(false);
                    RegionNullTatile.gameObject.SetActive(true);
                    GridNumRegionGrid.gameObject.SetActive(false);
                    break;
                case 1:
                case 2:
                case 3:
                case 4:
                    if (regionList.Count == 4)
                    {
                        GridNumRegionGrid.maxPerLine = 2;
                    }
                    FullObj.SetActive(false);
                    NumObj.SetActive(true);
                    FiveRegionObj.SetActive(false);
                    RegionNullTatile.gameObject.SetActive(false);
                    GridNumRegionGrid.gameObject.SetActive(true);
                    RefreshList(GridNumRegionGrid, regionList);
                    break;
                case 5:
                    FullObj.SetActive(false);
                    NumObj.SetActive(false);
                    FiveRegionObj.SetActive(true);
                    RefiveList(regionList);
                    break;
                default:
                    if (regionList.Count > 5)
                    {
                        FullObj.SetActive(true);
                        NumObj.SetActive(false);
                        FiveRegionObj.SetActive(false);
                        RefreshList(GridFullRegionGrid, regionList);
                    }
                    break;
            }
        }

        private void RefiveList(List<MahjongPlay> list)
        {
            for(int i=0;i<FiveRegionObj.transform.childCount;i++)
            {
                MahjongPlay tmpMjPlay = list[i];
                GameObject tmpGo = FiveRegionObj.transform.GetChild(i).gameObject;
                UIRuleBtnInfo tabBtnList = tmpGo.GetComponent<UIRuleBtnInfo>();
                tabBtnList.OnClickCallBack = RuleTabBtnCallBack;
                if (i == 0)
                {
                    // 拿到当前地区玩法次数 达到 一定次数的显示热度标志
                    int playNum = MemoryData.MahjongPlayData.GetPlayTime(tmpMjPlay.ConfigId);
                    if (playNum >= 5)
                    {
                        tabBtnList.RuleTabBtnInit(tmpMjPlay, true);
                    }
                    else
                    {
                        tabBtnList.RuleTabBtnInit(tmpMjPlay);
                    }
                }
                else
                {
                    tabBtnList.RuleTabBtnInit(tmpMjPlay);
                }
            }
        }

        /// <summary>
        /// 刷新指定目录信息
        /// </summary>
        /// <param name="grid">Grid.</param>
        /// <param name="list">List.</param>
        /// <param name="isClick">If set to <c>true</c> is click.</param>
        private void RefreshList(UIGrid grid, List<MahjongPlay> list)
        {
            int index = 0;
            UITools.CreateChild<MahjongPlay>(grid.transform, null, list, (go, ruleData) =>
            {
                index++;

                UIRuleBtnInfo tabBtnList = go.GetComponent<UIRuleBtnInfo>();
                tabBtnList.OnClickCallBack = RuleTabBtnCallBack;
                if (index == 1)
                {
                    // 拿到当前地区玩法次数 达到 一定次数的显示热度标志
                    int playNum = MemoryData.MahjongPlayData.GetPlayTime(ruleData.ConfigId);
                    if (playNum >= 5)
                    {
                        tabBtnList.RuleTabBtnInit(ruleData, true);
                    }
                    else
                    {
                        tabBtnList.RuleTabBtnInit(ruleData);
                    }
                }
                else
                {
                    tabBtnList.RuleTabBtnInit(ruleData);
                }
            });
            grid.Reposition();
        }

        /// <summary>
        /// 点击页签按钮回调
        /// </summary>
        public void RuleTabBtnCallBack(MahjongPlay data)
        {
            //QLoger.ERROR(" RoomModel ######################### obj.name = " + data.Name);
            _R.ui.OpenUI("UICreateRoomRule", data.ConfigId);
            _R.ui.GetUI("UICreateRoom").Close();
        }
        /// <summary>
        /// 切换按钮点击
        /// </summary>
        private void SwitchBtnClick(GameObject go)
        {
            _R.ui.OpenUI("UIMap");
        }
    }
}

