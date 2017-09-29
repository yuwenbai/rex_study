/**
 * @Author lyb
 *  馆长桌卡模块
 *
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace projectQ
{
    public class UIRoomCardMaster : UIViewBase
    {
        public UIRoomCardMasterModel Model
        {
            get { return _model as UIRoomCardMasterModel; }
        }

        /// <summary>
        /// 桌卡ID
        /// </summary>
        public int RoomCardID;
        /// <summary>
        /// 关闭面板按钮
        /// </summary>
        public GameObject CloseBtn;
        /// <summary>
        /// 加号按钮
        /// </summary>
        public GameObject RoomCardAddBtn;
        /// <summary>
        /// 减号按钮
        /// </summary>
        public GameObject RoomCardDownBtn;
        /// <summary>
        /// 桌卡数量
        /// </summary>
        public UILabel RoomCardNumLab;
        /// <summary>
        /// 桌卡获得数量
        /// </summary>
        public UILabel RoomCardCount;
        /// <summary>
        /// 购买按钮
        /// </summary>
        public GameObject RoomCardBuyBtn;
        /// <summary>
        /// 购买按钮钱数
        /// </summary>
        public UILabel RoomCardBuyBtnLab;

        public shop RoomCardData;

        /// <summary>
        /// 桌卡数量
        /// </summary>
        private int _RoomCardNum = 1;
        public int RoomCardNum
        {
            get { return _RoomCardNum; }
            set { _RoomCardNum = value; }
        }

        #region override-------------------------------------------------

        public override void Init()
        {            
            UIEventListener.Get(CloseBtn).onClick = OnCloseBtnClick;
            UIEventListener.Get(RoomCardBuyBtn).onClick = OnBuyBtnClick;
            UIEventListener.Get(RoomCardAddBtn).onClick = OnRoomCardAddClick;
            UIEventListener.Get(RoomCardDownBtn).onClick = OnRoomCardDownClick;
        }

        public override void OnHide(){}

        public override void OnShow()
        {
            ModelNetWorker.Instance.C2SRoomCardListReq();
        }

        #endregion------------------------------------------------------

        /// <summary>
        /// 关闭按钮
        /// </summary>
        private void OnCloseBtnClick(GameObject go)
        {
            this.Close();
        }

        /// <summary>
        /// 购买按钮
        /// </summary>
        private void OnBuyBtnClick(GameObject go)
        {
            ModelNetWorker.Instance.C2SRoomCardBuyReq(RoomCardID , RoomCardNum);
        }

        /// <summary>
        /// 增加桌卡数量按钮
        /// </summary>
        private void OnRoomCardAddClick(GameObject go)
        {
            RoomCardNum++;

            RoomCardValueSet();
        }

        /// <summary>
        /// 减少桌卡数量按钮
        /// </summary>
        private void OnRoomCardDownClick(GameObject go)
        {
            RoomCardNum--;
            if (RoomCardNum <= 1)
            {
                RoomCardNum = 1;
            }

            RoomCardValueSet();
        }

        /// <summary>
        /// 数据填充
        /// </summary>
        public void RoomCardValueSet()
        {
            RoomCardNumLab.text = RoomCardNum.ToString();
            
            int cardCount = (int.Parse(RoomCardData.ItemNum)) * RoomCardNum;
            RoomCardCount.text = string.Format(RoomCardData.Notes, cardCount.ToString());

            int buyNum = (int.Parse(RoomCardData.MoneyNum)) * RoomCardNum;
            RoomCardBuyBtnLab.text = string.Format(RoomCardData.ButtonDesc, buyNum.ToString());
        }
    }
}


