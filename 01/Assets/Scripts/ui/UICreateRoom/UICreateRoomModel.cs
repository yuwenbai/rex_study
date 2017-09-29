/**
 * @Author GarFey
 *  
 */
using System.Collections.Generic;
using UnityEngine;

namespace projectQ
{
	public class UICreateRoomModel : UIModelBase 
	{
		public UICreateRoom UI
		{
			get { return _ui as UICreateRoom; }
		}
        /// <summary>
        ///  排布是否是全部
        /// </summary>
        /// <returns></returns>
        public bool IsFull()
        {
            List<MahjongPlay> regionList = MemoryData.MahjongPlayData.LocalRegionPlayList;
            if(regionList.Count>5)
            {
                return true;
            }
            return false;
        }
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
				RulePlayTabBtnCreat ();
				RefUpPlayData ();
				break;
			}
		}

		#endregion-----------------------------------------------------------------------------

		/// <summary>
		/// 创建左右页签按钮
		/// </summary>
		public void RulePlayTabBtnCreat()
		{
            if (IsFull())
            {
                UI.BgFull.SetActive(true);
                UI.BgNum.SetActive(false);
            }
            else
            {
                UI.BgFull.SetActive(false);
                UI.BgNum.SetActive(true);
            }
            UI.CreatUp.InitUp();
            UI.CreatBottom.InitBottom();
        }

		/// <summary>
		/// 刷新上次玩家信息 
		/// </summary>
		public void RefUpPlayData()
		{
            UI.CreatDown.InitDown(this);
		}
	}
}
