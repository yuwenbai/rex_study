/****************
 * 
 * @Author GarFey
 * 
 * ***************/
using System;
using System.Collections.Generic;
using UnityEngine;

namespace projectQ
{
    public class CreateRoomUp : MonoBehaviour
    {
        /// <summary>
		/// 流行玩法空数据显示
		/// </summary>
		public UILabel FadNullTatile;
        /// <summary>
		/// 流行玩法创建栏Grid
		/// </summary>
		public UIGrid GridFadGrid;

        public void InitUp()
        {
            List<MahjongPlay> fadList = MemoryData.MahjongPlayData.FashionPlayList;
            if (fadList.Count>0)
            {
                FadNullTatile.gameObject.SetActive(false);
                GridFadGrid.gameObject.SetActive(true);
                RefreshList(GridFadGrid, fadList);
            }else
            {
                FadNullTatile.gameObject.SetActive(true);
                GridFadGrid.gameObject.SetActive(false);
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
    }
}
