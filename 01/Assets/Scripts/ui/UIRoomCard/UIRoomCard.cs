/**
 * @Author lyb
 *  桌卡模块
 *
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace projectQ
{
    public class UIRoomCard : UIViewBase
    {
        public UIRoomCardModel Model
        {
            get { return _model as UIRoomCardModel; }
        }
        
        /// <summary>
        /// 关闭面板按钮
        /// </summary>
        public GameObject CloseBtn;
        /// <summary>
        /// 創建桌卡面板
        /// </summary>
        public UIGrid RoomCaardPanelObj;
        /// <summary>
        /// 滑动控件
        /// </summary>
        public UIScrollView ScrollViewObj;

        #region override-------------------------------------------------

        public override void Init()
        {            
            UIEventListener.Get(CloseBtn).onClick = OnCloseBtnClick;
        }

        public override void OnHide(){}

        public override void OnShow()
        {
            Model.RoomCard_LoadXml();

            LoadSendLoading();

            //EventDispatcher.FireSysEvent(GEnum.NamedEvent.EGetGameData, GEnum.NamedGameData.EC2SRoomCardList);
            ModelNetWorker.Instance.C2SRoomCardListReq();
        }

        #endregion------------------------------------------------------

        /// <summary>
        /// 返回按钮
        /// </summary>
        private void OnCloseBtnClick(GameObject go)
        {
            this.Close();
        }
    }
}


