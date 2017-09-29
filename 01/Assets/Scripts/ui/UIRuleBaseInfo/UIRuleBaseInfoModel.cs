/**
 * @Author GarFey
 *  
 */

using System.Collections.Generic;
using UnityEngine;

namespace projectQ
{
    public class UIRuleBaseInfoModel : UIModelBase 
	{
		public UIRuleBaseInfo UI
		{
			get { return _ui as UIRuleBaseInfo; }
		}
        // ceil位置最小值
        private float CeilUpMaxPosion;
        // ceil位置最大值
        private float CeilDownMaxPosion;
        // ceil超出的行数 目前ceil支持显示7行 当ceil为10行的时候该值为10-7=3行
        private int CeilOutNum;

		#region override----------------------------------------------------------------------

		protected override GEnum.NamedEvent[] FocusNetWorkData()
		{
			return new GEnum.NamedEvent[]
			{
				GEnum.NamedEvent.SysData_Region_Update,
			};
		}

		protected override void OnNetWorkDataCallBack(GEnum.NamedEvent msgEnum, object[] data)
		{
			switch (msgEnum)
			{
			case GEnum.NamedEvent.SysData_Region_Update:
				RulePlayTabBtnCreat();
				break;
			}
		}

        #endregion-----------------------------------------------------------------------------

        /// <summary>
        /// 创建左右页签按钮
        /// </summary>
        public void RulePlayTabBtnCreat()
        {
            List<MahjongPlay> fadList = MemoryData.MahjongPlayData.FashionPlayList;
            List<MahjongPlay> regionList = MemoryData.MahjongPlayData.LocalRegionPlayList;

            if (MemoryData.PlayerData.MyPlayerModel.PlayerDataBase.RegionID <= 0)
            {
                UI.RegionName.text = "未选择地区";
            }
            else
            {
                string regionName = MemoryData.XmlData.XmlBuildDataRegion_Get(MemoryData.PlayerData.MyPlayerModel.PlayerDataBase.RegionID);
                UI.RegionName.text = regionName;
            }

            if (fadList.Count > 0) {
                UI.GridFadObj.gameObject.SetActive(true);
                UI.FadNullTatile.gameObject.SetActive(false);
                this.RefreshList(UI.GridFadObj, fadList);
            } else {
                UI.GridFadObj.gameObject.SetActive(false);
                UI.FadNullTatile.gameObject.SetActive(true);
            }
            if (regionList.Count > 0) {
                UI.GridRegionObj.gameObject.SetActive(true);
                UI.RegionNullTatile.gameObject.SetActive(false);
                this.RefreshList(UI.GridRegionObj, regionList);
            } else {
                UI.GridRegionObj.gameObject.SetActive(false);
                UI.RegionNullTatile.gameObject.SetActive(true);
            }

            UI.RegionScrollView.onStoppedMoving = RuleStoppedMoving;
            if (UI.RegionScrollView.shouldMoveVertically)
            {
                UI.TriangleObj.SetActive(true);
                CeilUpMaxPosion = -50f;
                //QLoger.ERROR(" :: " + Mathf.CeilToInt(UI.GridRegionObj.transform.childCount / (float)UI.GridRegionObj.maxPerLine));
                CeilOutNum = Mathf.CeilToInt(UI.GridRegionObj.transform.childCount / (float)UI.GridRegionObj.maxPerLine) - 7;
                CeilDownMaxPosion = UI.GridRegionObj.cellHeight * CeilOutNum - 12;
            }
            else
            {
                UI.TriangleObj.SetActive(false);
            }
        }

        /// <summary>
        /// ScrollView 移动完成
        /// </summary>
        void RuleStoppedMoving()
        {
           // QLoger.ERROR("OnDragFinishedOnDragFinished :: " +UI.RegionScrollView.shouldMoveVertically.ToString()+" ||  y == "+ UI.RegionScrollView.transform.localPosition.y + " ||  cellHeight == " + CeilDownMaxPosion);
            if (UI.RegionScrollView.transform.localPosition.y<=CeilUpMaxPosion)
            {
                UI.TriangleDownSp.alpha = 1f;
                UI.TriangleUpSp.alpha = 0.5f;
            }
            else if(UI.RegionScrollView.transform.localPosition.y> CeilUpMaxPosion && UI.RegionScrollView.transform.localPosition.y< CeilDownMaxPosion)
            {
                UI.TriangleDownSp.alpha = 1f;
                UI.TriangleUpSp.alpha = 1f;
            }
            else if(UI.RegionScrollView.transform.localPosition.y>= CeilDownMaxPosion)
            {
                UI.TriangleDownSp.alpha = 0.5f;
                UI.TriangleUpSp.alpha = 1f;
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
                    //tabBtnList.RuleTabBtnInit(ruleData);
                    if (index == 1)
                    {
                        // 拿到当前地区玩法次数 达到 一定次数的显示热度标志
                        int playNum = MemoryData.MahjongPlayData.GetPlayTime(ruleData.ConfigId);
                        if (playNum >= 5)
                        {
                            tabBtnList.RuleTabBtnInit(ruleData, true);
                        }else
                        {
                            tabBtnList.RuleTabBtnInit(ruleData);
                        }
                    }
                    else
                    {
                        tabBtnList.RuleTabBtnInit(ruleData);
                    }
                    /* if (ruleData.MjType == MahjongPlay.MahjongPlayType.Fashion)
                     {
                         if (index == 1)
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
                         if (index <= grid.maxPerLine)
                         {
                             tabBtnList.RuleTabBtnInit(ruleData, true);
                         }
                         else
                         {
                             tabBtnList.RuleTabBtnInit(ruleData);
                         }
                     }*/
                });
			grid.Reposition();

		}

		/// <summary>
		/// 点击页签按钮回调
		/// </summary>
		public void RuleTabBtnCallBack(MahjongPlay data)
		{
			//QLoger.ERROR(" ######################### obj.name = " + data.ConfigId+data.Name);
			UI.LoadUIMain("UIRule",data.ConfigId);
		}
	}
}
