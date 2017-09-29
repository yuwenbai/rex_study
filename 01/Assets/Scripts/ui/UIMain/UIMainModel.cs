/**
 * @Author YQC
 *
 *
 */

using System.Collections;
using System.Collections.Generic;
using Msg;
using UnityEngine;

namespace projectQ
{
    public class UIMainModel : UIModelBase
    {
        private UIMain UI
        {
            get { return this._ui as UIMain; }
        }
        /// <summary>
        /// 刷新跑马灯
        /// </summary>
        public void RefreshBroadcasts()
        {
            UI.BroadcastsScript.Clear();

            List<NoticeMsg> data = null;
            if (UI.RoomId == 0 || UI.CurrMjHalll == null)
            {
                data = MemoryData.OtherData.GetNoticeListByRegionId(MemoryData.PlayerData.MyPlayerModel.PlayerDataBase.RegionID);
            }
            else
            {
                data = MemoryData.OtherData.GetNoticeListByRegionId(UI.CurrMjHalll.RegionID, Msg.NoticeTypeDef.Notice_FMjRoom);
            }
            if(data != null&&data.Count > 0)
            {
                for (int i = 0; i < data.Count; i++)
                {
                    var temp = new NGUIBroadcastsData();
                    temp.value = data[i].Content;
                    temp.Speed = 1;
                    temp.LoopCount = int.MaxValue;
                    UI.BroadcastsScript.Push(temp);
                }
            }
            else  if(MemoryData.OtherData.NoticeMsgList.Count > 0)
            {
                for (int i = 0; i < MemoryData.OtherData.NoticeMsgList.Count; i++)
                {
                    var temp = new NGUIBroadcastsData();
                    temp.value = MemoryData.OtherData.NoticeMsgList[i].Content;
                    temp.Speed = 1;
                    temp.LoopCount = int.MaxValue;
                    UI.BroadcastsScript.Push(temp);
                }
            }
        }
        #region override 
        protected override GEnum.NamedEvent[] FocusNetWorkData()
        {
            return new GEnum.NamedEvent[]
            {
                GEnum.NamedEvent.SysData_User_MjHallInfoUpdate,
                GEnum.NamedEvent.SysData_MjHall_RecommendResultRsp,
                GEnum.NamedEvent.SysData_MjHall_SearchResultRsp,
                GEnum.NamedEvent.SysData_Broadcast_Update,
                GEnum.NamedEvent.SysData_MjHall_BoardUpdata,
                GEnum.NamedEvent.SysData_User_RoomCard_Update,
                GEnum.NamedEvent.SysData_Region_Update,
                GEnum.NamedEvent.SysUI_PlayUIMainStartTween,
                GEnum.NamedEvent.SysUI_UIMap_Open,
                GEnum.NamedEvent.SysUI_OpenBanner,//公告
                GEnum.NamedEvent.SysUI_OpenBannerReq,//公告请求
                GEnum.NamedEvent.SysUI_UIReplayCtrl_Play_ViewRecord, //战绩
            };
        }
        protected override void OnNetWorkDataCallBack(GEnum.NamedEvent msgEnum, object[] data)
        {
            if(!UI.StateScript.SetData(msgEnum, data))
            {
                switch (msgEnum)
                {
                    case GEnum.NamedEvent.SysData_Broadcast_Update:
                        RefreshBroadcasts();
                        break;
                    case GEnum.NamedEvent.SysData_MjHall_BoardUpdata:
                        UI.TopScript.UpdateBoard();
                        break;
                    case GEnum.NamedEvent.SysData_User_RoomCard_Update:
                        UI.PlayInfoScript.RefreshUI(UI.CurrState);
                        break;
                    case GEnum.NamedEvent.SysData_Region_Update:
                        UI.TopScript.UpdateRegion();
                        break;
                    case GEnum.NamedEvent.SysUI_PlayUIMainStartTween:
                        bool isTween = (bool)data[0];
                        UI.GameHallObjTweenBegin(isTween);
                        break;
                    case GEnum.NamedEvent.SysUI_UIMap_Open:
                        UI.LoadUIMain("UIMap");
                        break;
                    case GEnum.NamedEvent.SysUI_OpenBannerReq:
                        //请求公告信息
                        ModelNetWorker.Instance.BannerReq(null);
                        break;
                    case GEnum.NamedEvent.SysUI_OpenBanner:
                        {
                            int resultCode = (int)data[0];
                            if (resultCode == (int) Msg.ErrorCode.ErrCode_Success && checkBannerData(data[1]) == true)
                            {
                                _R.ui.OpenUI("UIBanner", data[1]);
                            }   
                            else
                            {
                                EventDispatcher.FireSysEvent(GEnum.NamedEvent.SysFlow_Flow_CallBack, "FirstActivity");
                                QLoger.LOG("Banner不能打开 code:"+ resultCode);
                            }
                        }
                        break;
                    case GEnum.NamedEvent.SysUI_UIReplayCtrl_Play_ViewRecord:
                        {
                            int deskId = (int)data[0];
                            if (deskId > 0)
                            {
                                UI.LoadUIMain("UIMahjongResult", deskId, 1);
                            }
                        }
                        break;
                }
            }
        }
        /// <summary>
        /// 检查公告数据是否合法
        /// </summary>
        /// <param name="_data"></param>
        /// <returns></returns>
        private bool checkBannerData(object _data)
        {
            if (_data == null )
            {
                return false;
            }
            List<ActivityNotify> activ = (List<ActivityNotify>)_data;
            if (activ.Count == 0)
            {
                DebugPro.Log("======================UIBanner  activ.Count == 0");
                return false;
            }
            string resUrl = activ[0].ResUrl;//"Tex_Notice"
            if (string.IsNullOrEmpty(resUrl))
            {
                
                DebugPro.Log("======================UIBanner  resUrl 为空");
                return false;
            }
            return true;

        }
        #endregion

    }
}