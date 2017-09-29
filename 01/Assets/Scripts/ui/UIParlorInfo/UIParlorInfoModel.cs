

using System;
/**
* @Author JEFF
*
*
*/
using System.Collections;
using System.Collections.Generic;
using Msg;
using UnityEngine;

namespace projectQ
{
    public class UIParlorInfoModel : UIModelBase
    {
        public MjRoom data;
        private UIParlorInfo UI
        {
            get { return base._ui as UIParlorInfo; }
        }

        public void OnBindHall()
        {
            ModelNetWorker.Instance.FMjRoomBindReq(this.data.RoomID);
        }

        #region override
        protected override GEnum.NamedEvent[] FocusNetWorkData()
        {
            return new GEnum.NamedEvent[]
            {
                GEnum.NamedEvent.SysData_MjHall_BindUpdate,
            };
        }
        protected override void OnNetWorkDataCallBack(GEnum.NamedEvent msgEnum, object[] data)
        {
            switch(msgEnum)
            {
                case GEnum.NamedEvent.SysData_MjHall_BindUpdate:
                    if((int)data[0] == 0)
                    {
                        bool isFirst = (bool)data[1];
                        if(UI.HideCallBack == null)
                        {
                            UI.Hide();
                            string[] btnNames = new string[1];
                            btnNames[0] = "确定";
                            //if (isFirst)
                            //{
                            //    UI.LoadPop(WindowUIType.SystemPopupWindow, "关联成功", "首次关联麻将馆，馆主赠送10张桌卡", btnNames, OnBtnClick);
                            //}
                            //else
                            //{
                                UI.LoadPop(WindowUIType.SystemPopupWindow, "关联成功", "关联棋牌室成功", btnNames, OnBtnClick);
                            //}

                            if (!MemoryData.GameStateData.IsWxShareInvitePlay)
                            {
                                UI.LoadUIMain("UIMain", UIMain.EnumUIMainState.LinkedMjHall);
                            }
                            MemoryData.GameStateData.IsWxShareInvitePlay = false;
                        }
                        else
                        {
                            UI.Hide();
                        }
                    }
                    else
                    {
                        //关联失败
                        UI.LoadPop(WindowUIType.SystemPopupWindow, "关联失败", "请联系客服 微信:slj-qipai" + '\n'+ "客服热线:4000-348-369",
                            new string[] { "确定" },delegate(int index)
                            {

                            });
                    }
                    break;
            }
        }

        void OnBtnClick(int index)
        {

        }
        #endregion
    }
}
