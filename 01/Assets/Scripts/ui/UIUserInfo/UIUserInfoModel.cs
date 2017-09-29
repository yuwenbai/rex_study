/**
* @Author YQC、GarFey
*/

namespace projectQ
{
    public class UIUserInfoModel : UIModelBase
    {
        private UIUserInfo UI
        {
            get { return this._ui as UIUserInfo; }
        }

        private long _userId;
        /// <summary>
        /// 用户id
        /// </summary>
        public long UserId
        {
            set
            {
                _userId = value;
                MemoryData.SysPlayerDataHandle.PlayerReq(_userId, SysPlayerDataHandle.PlayerDataType.All);
            }
            get { return _userId; }
        }
        /// <summary>
        /// 用户数据
        /// </summary>
        public PlayerDataModel UserInfoData
        {
            get{return MemoryData.PlayerData.get(this._userId);}
        }
        #region override
        protected override GEnum.NamedEvent[] FocusNetWorkData()
        {
            return new GEnum.NamedEvent[]
            {
                 GEnum.NamedEvent.SysData_Certification_Rsp,
                 GEnum.NamedEvent.SysData_PhoneNoBind_Rsp,
                 GEnum.NamedEvent.SysData_User_MjHallInfoUpdate,
                 GEnum.NamedEvent.SysData_PlayerData_Update,
            };
        }
        protected override void OnNetWorkDataCallBack(GEnum.NamedEvent msgEnum, object[] data)
        {
            switch (msgEnum)
            {
                case GEnum.NamedEvent.SysData_Certification_Rsp: //实名认证
                    {
                        int code = (int)data[0];
                        UI.Certification.BindRsp(code == 0);
                        if (code == 0)
                        {
                            UI.RefreshUI();
                        }
                    }
                    break;
                case GEnum.NamedEvent.SysData_PhoneNoBind_Rsp: //手机号绑定
                    {
                        int code = (int)data[0];
                        UI.BindPhoneNo.BindRsp(code == 0);
                        if (code == 0)//绑定成功
                        {
                            UI.RefreshUI();
                            //UI.LoadPop(WindowUIType.SystemPopupWindow, "", "绑定成功", new string[] { "取消", "确认" }, RefreshUI);
                        }
                    }
                    break;
                case GEnum.NamedEvent.SysData_User_MjHallInfoUpdate:
                    {
                        UI.RefreshUI();
                    }
                    break;
                case GEnum.NamedEvent.SysData_PlayerData_Update:
                    {
                        if (data.Length >= 1)
                        {
                            long userId = (long)data[0];
                            if (this.UserId == userId)
                            {
                                UI.RefreshUI();
                            }
                        }
                    }
                    break;
            }
        }
        #endregion
    }
}