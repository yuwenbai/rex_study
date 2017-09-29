

using System;
/**
* 作者：周腾
* 作用：红包
* 日期：
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace projectQ
{
    public class UIRedPackge : UIViewBase
    {
        public UIRedPackgeModel Model { get { return _model as UIRedPackgeModel; } }
        public GameObject closeBtn;
        public UILabel labelCard;
        public GameObject senBtnObj;
        public UIInput inputSendTalk;
        public UIInput inputCardForNormal;
        public UIInput inputNumsForNormal;
        private int ticket;
        private int num;
        private string msg;
        public void InitMyCardLabel()
        {
            labelCard.text = MemoryData.PlayerData.MyPlayerModel.PlayerDataBase.TotalTicket.ToString();
        }

        void OnSendBtnClick(GameObject go)
        {
            CheckNormalInput();

        }
        /// <summary>
        /// 检查普通红包输入是否正确
        /// </summary>
        void CheckNormalInput()
        {
            if (string.IsNullOrEmpty(inputCardForNormal.value))//没输入桌卡;
            {
                WindowUIManager.Instance.CreateTip("请输入桌卡数量");
                return;
            }
            else if (int.Parse(inputCardForNormal.value) <= 0)//桌卡数量小于等于0
            {
                WindowUIManager.Instance.CreateTip("桌卡数量必须大于0");
                return;
            }
            else if (string.IsNullOrEmpty(inputNumsForNormal.value))
            {
                WindowUIManager.Instance.CreateTip("请输入红包数量");
                return;
            }
            else if (int.Parse(inputNumsForNormal.value) <= 0)
            {
                WindowUIManager.Instance.CreateTip("红包数量必须大于0");
                return;
            }
            //else if (int.Parse(inputNumsForNormal.value) > int.Parse(inputCardForNormal.value))
            //{
            //    WindowUIManager.Instance.CreateTip("桌卡数量必须大于等于红包数量");
            //    return;
            //}
            else
            {
                SendRedPacket();
            }
        }
        void OnCloseBtnClick(GameObject go)
        {
            ClearInput();
            this.Hide();
            //this.Close();
        }
        /// <summary>
        /// 发送红包
        /// </summary>
        void SendRedPacket()
        {
            this.ticket = int.Parse(inputCardForNormal.value);
            this.num = int.Parse(inputNumsForNormal.value);
            this.msg = inputSendTalk.value;
            if ((ticket * num) > MemoryData.PlayerData.MyPlayerModel.PlayerDataBase.TotalTicket)
            {
                //桌卡数量不足
                string[] btnNames = new string[2];
                btnNames[0] = "取消";
                btnNames[1] = "确定";
                this.LoadPop(WindowUIType.SystemPopupWindow, "提示", "桌卡数量不足，请购买桌卡后，继续游戏", btnNames, OnBtnClick);
            }
            else
            {
                //可以发
                Model.OnSendRedPacketReq(MemoryData.PlayerData.MyPlayerModel.PlayerDataBase.UserID, ticket, num, msg);
            }
           
            //ClearInput();
        }
        void OnBtnClick(int btnIndex)
        {
            if (btnIndex == 1)
            {
                LoadUIMain("UIRoomCardMaster");
            }
        }

        void ClearInput()
        {
            inputCardForNormal.value = "";
            inputNumsForNormal.value = "";
            inputSendTalk.value = "";
            ticket = 0;
            num = 0;
            msg = "";
        }
        public void SendRedRsp(int bagId, string msg)
        {
            WXShareParams shareParams = new WXShareParams("WECHAT_SHARE_SEND_RED_BAG");
            shareParams.InsertUrlParams(new object[] { bagId.ToString() });
            SDKManager.Instance.SDKFunction("WECHAT_SHARE_SEND_RED_BAG", shareParams);
        }
        public override void Init()
        {
            UIEventListener.Get(senBtnObj).onClick = OnSendBtnClick;
            UIEventListener.Get(closeBtn).onClick = OnCloseBtnClick;
        }

        public override void OnShow()
        {
            InitMyCardLabel();
        }

        public override void OnHide()
        {

        }
    }
}
