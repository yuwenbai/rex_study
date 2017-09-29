/**
 * @Author YQC
 *
 *
 */

using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace projectQ
{
    public class UICheatInfoModel : UIModelBase {
        public UICheatInfo UI
        {
            get { return _ui as UICheatInfo; }
        }
        public StringBuilder IpText = new StringBuilder();
        public StringBuilder LocationText = new StringBuilder();
        public StringBuilder LocationEmptyText = new StringBuilder();
        public bool IpCheate = false;
        public bool LocationCheate = false;
        public int GameInShowCount = 0;
        public float StandingTime = 15f;

        public void RefreshData()
        {
            IpText.Length = 0;
            LocationText.Length = 0;
            LocationEmptyText.Length = 0;
            var deskInfo = MemoryData.DeskData.GetOneDeskInfo(MjDataManager.Instance.MjData.curUserData.selfDeskID);
            var playerIds = deskInfo.GetAllPlayerID();
            var playerIdList = new List<long>(playerIds);


            var ipUserIds= MemoryData.OtherData.GetMyCheateInfoUserIdsNew(CheatInfo.EnumWarningType.Ip);
            if(ipUserIds != null && ipUserIds.Count > 0)
            {
                IpCheate = true;

                IpText.Append(GetConentByUseridss(ipUserIds, "IP相同"));
            }
            else
            {
                IpCheate = false;
                IpText.Append(GetConentByUserids(playerIdList));
                IpText.Append("IP正常");
            }

            //位置信息
            var LocationUserIds = MemoryData.OtherData.GetMyCheateInfoUserIdsNew(CheatInfo.EnumWarningType.Location);
            var LocationEmptyUserIds = MemoryData.OtherData.GetMyCheateInfoUserIdsNew(CheatInfo.EnumWarningType.LocationEmpty);

            //
            if (LocationUserIds != null && LocationUserIds.Count > 0)
            {
                LocationCheate = true;
                LocationText.Append(GetConentByUseridss(LocationUserIds, "地理位置相近"));
            }
            if (LocationEmptyUserIds != null && LocationEmptyUserIds.Count > 0)
            {
                LocationCheate = true;
                LocationEmptyText.Append(GetConentByUseridss(LocationEmptyUserIds, "无法定位"));
            }

            if((LocationUserIds == null || LocationUserIds.Count == 0 )&& (LocationEmptyUserIds == null || LocationEmptyUserIds.Count == 0))
            {
                LocationCheate = false;
                LocationText.Append(GetConentByUserids(playerIdList));
                LocationText.Append("地理位置正常");
            }
        }

        private string GetConentByUseridss(List<List<long>> ids,string endStr)
        {
            if (ids == null || ids.Count == 0) return null;

            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < ids.Count; i++)
            {
                string temp = GetConentByUserids(ids[i]);
                temp = temp.Trim();
                if(!string.IsNullOrEmpty(temp))
                {
                    if(sb.Length != 0)
                    {
                        sb.Append(" ");
                    }
                    sb.Append(temp).Append(" ").Append(endStr);
                }
            }
            return sb.ToString();
        }

        public string GetConentByUserids(List<long> ids)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < ids.Count; i++)
            {
                sb.Append(MemoryData.PlayerData.get(ids[i]).PlayerDataBase.Name);
                sb.Append((i == ids.Count - 1) ? string.Empty : ",");
            }
            return sb.ToString();
        }

        private void ShowUI()
        {
            SendGPSServer();
            UI.PlayAnimation(true, StandingTime);
        }

        private void SendGPSServer()
        {
            GPSManager.Instance.GPSServeRequest();
        }
        #region override
        protected override GEnum.NamedEvent[] FocusNetWorkData()
        {
            return new GEnum.NamedEvent[] {
                 GEnum.NamedEvent.SysData_CheatInfo_Open,
                 GEnum.NamedEvent.SysData_CheatInfo_Hide,
                 GEnum.NamedEvent.SysData_CheatInfo_Update,
                 GEnum.NamedEvent.SysScene_Close,
            };
        }
        bool IsCanShow = false;
        bool IsUpdataData = false;
        protected override void OnNetWorkDataCallBack(GEnum.NamedEvent msgEnum, object[] data)
        {
            switch (msgEnum)
            {
                case GEnum.NamedEvent.SysData_CheatInfo_Open:
                    {
                        bool isShow = (bool)data[0];
                        QLoger.LOG("SysData_CheatInfo_Open__" + isShow);

                        if (isShow)
                        {
                            IsCanShow = true;
                            //UI.RefreshUI();
                            //ShowUI();
                        }
                        else
                        {
                            UI.SetShow(false);
                        }
                    }
                    break;
                case GEnum.NamedEvent.SysData_CheatInfo_Hide:
                    UI.SetShow(false);
                    break;

                case GEnum.NamedEvent.SysData_CheatInfo_Update:
                    {
                        if(MemoryData.GameStateData.CurrUISign >= SysGameStateData.EnumUISign.PrepareGame)
                        {
                            IsUpdataData = true;
                        }
                    }
                    break;
                case GEnum.NamedEvent.SysScene_Close:
                    {
                        string sceneName = data[0] as string;
                        if(sceneName == "MahJong")
                        {
                            IsCanShow = false;
                            IsUpdataData = false;
                        }
                    }
                    break;

            }

            if(IsCanShow && IsUpdataData)
            {
                if(MemoryData.GameStateData.CurrUISign >= SysGameStateData.EnumUISign.PrepareGame)
                {
                    IsCanShow = false;
                    IsUpdataData = false;
                    UI.RefreshUI();
                    ShowUI();
                }
            }
        }
        #endregion
    }
}