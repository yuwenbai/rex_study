/**
* @Author lyb
*
*
*/
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace projectQ
{
    public class UIOption : UIViewBase
    {
        public UIOptionModel Model
        {
            get { return _model as UIOptionModel; }
        }

        /// <summary>
        /// 关闭按钮
        /// </summary>
        public GameObject CloseButton;
        /// <summary>
        /// 在线客服按钮
        /// </summary>
        public GameObject CustomerBtn;
        /// <summary>
        /// 解除关联按钮
        /// </summary>
        public GameObject RelieveBtn;
        /// <summary>
        /// 当前版本号
        /// </summary>
        public UILabel VersionStrLab;
        /// <summary>
        /// 设置按钮列表
        /// </summary>
        public List<UIOptionList> OptionSingleList;

        public GameObject VersionObj;

        public GameObject ButtonOpenGPS;

        #region override ------------------------------------------------------------

        public override void Init()
        {
            Model.OptionSingleInit();

            UIEventListener.Get(CloseButton).onClick = OnCloseBtnClick;
            UIEventListener.Get(CustomerBtn).onClick = OnCustomerBtnClick;
            UIEventListener.Get(RelieveBtn).onClick = OnRelieveBtnClick;
            UIEventListener.Get(VersionObj).onClick = OnVersionBtnClick;
            UIEventListener.Get(ButtonOpenGPS).onClick = OnButtonOpenGPSClick;

            EventDispatcher.AddEvent(GEnum.NamedEvent.EMjControlCloseMaskUI, CloseSelf);
        }

        public override void OnShow()
        {
            Model.OptionInit();

            if (MemoryData.PlayerData.MyPlayerModel.PlayerDataBase.BindRoomID == 0 || MemoryData.GameStateData.CurrUISign > SysGameStateData.EnumUISign.MainIn)
            {
                RelieveBtn.SetActive(false);
            }
            else
            {
                //RelieveBtn.SetActive(true);
                RelieveBtn.SetActive(false);
            }
        }

        public override void OnHide() { }

        protected override void OnClose()
        {
            EventDispatcher.RemoveEvent(GEnum.NamedEvent.EMjControlCloseMaskUI, CloseSelf);
            base.OnClose();
        }

        #endregion-------------------------------------------------------------------

        #region 按钮点击回调方法-----------------------------------------------------

        /// <summary>
        /// 点击版本号提示当前链接的服务器，端口
        /// </summary>
        /// <param name="go"></param>
        void OnVersionBtnClick(GameObject go)
        {
            string ip = LoginEnterNetWork.Instance.GatewayServerIp;
            string port = LoginEnterNetWork.Instance.GatewayServerPort.ToString();
            
#if __OPEN_USER_ACTION
            LoadTip("当前服务器:" + ip + ";端口:" + port);
            this.LoadUIMain("UILogInfo");
#endif
        }
        /// <summary>
        /// 在线客服
        /// </summary>
        void OnCustomerBtnClick(GameObject button)
        {
            WebSDKParams param = new WebSDKParams("WEB_OPEN_CS_SERVICE");
            param.InsertUrlParams(MemoryData.UserID, MemoryData.PlayerData.MyPlayerModel.PlayerDataBase.Name);
            SDKManager.Instance.SDKFunction("WEB_OPEN_CS_SERVICE", param);
            //SDKManager.Instance.SDKFunction(FunctionEnumSDKMananger.SERVICE_INFO);

            /*
            this.LoadPop(WindowUIType.SystemPopupWindow, "联系客服", "活动未开始，敬请期待！", new string[] { "确定" }
            , (index) => { });
            */
        }

        /// <summary>
        /// 解除关联按钮
        /// </summary>
        void OnRelieveBtnClick(GameObject button)
        {
            this.LoadPop(WindowUIType.SystemPopupWindow, "提示", "是否确认解除关联", new string[] { "取消", "确认" }
            , delegate (int index)
            {
                if (index == 1)
                {
                    ModelNetWorker.Instance.FMjRoomUnBindReq(null);
                    this.Close();
                }
            });
        }

        void OnCloseBtnClick(GameObject button)
        {
            this.Close();
        }

        private void CloseSelf(object[] vars)
        {
            this.Close();
        }

        int btnGpsClickCount = 0;
        private void OnButtonOpenGPSClick(GameObject go)
        {
#if __DEBUG ||  __OPEN_USER_ACTION || _PRE_DISTRUBUTE_
            btnGpsClickCount++;
            if(btnGpsClickCount > 3)
            {
                this.LoadUIMain("UIGPS");
            }
#endif
        }

        #endregion-------------------------------------------------------------------
    }
}