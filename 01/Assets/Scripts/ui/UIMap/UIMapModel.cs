/**
 * @Author YQC
 *
 *
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace projectQ
{
    public class UIMapModel : UIModelBase
    {
        public UIMap UI
        {
            get
            {
                return this._ui as UIMap;
            }
        }
        
        #region override------------------------------------------------------------------

        protected override GEnum.NamedEvent[] FocusNetWorkData()
        {
            return new GEnum.NamedEvent[] {
                GEnum.NamedEvent.SysData_AreaList_Update
            };
        }

        protected override void OnNetWorkDataCallBack(GEnum.NamedEvent msgEnum, object[] data)
        {
            switch(msgEnum)
            {
                case GEnum.NamedEvent.SysData_AreaList_Update:
                    break;
            }
        }

        #endregion------------------------------------------------------------------------

        #region 地图按钮初始化------------------------------------------------------------

        /// <summary>
        /// 初始化地图上的各个区域
        /// </summary>
        public void MapAreaListInit()
        {
            foreach (UIMapList mapList in UI.MapAreaList)
            {
                mapList.MapAreaInit(gameObject);

                mapList.OnClickCallBack = MapAreaClickCallBack;
            }
        }

        /// <summary>
        /// 各个区域按钮点击回调
        /// </summary>
        void MapAreaClickCallBack(int areaId)
        {
            MemoryData.PlayerData.MyPlayerModel.PlayerDataBase.RegionID = areaId;
            ModelNetWorker.Instance.UserRegionSetupReq(areaId);
            
            UI.Close();
        }

        #endregion------------------------------------------------------------------------
    }
}
