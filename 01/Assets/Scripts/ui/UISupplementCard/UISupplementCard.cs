/**
* @Author 周腾
*
*
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace projectQ
{
    public class UISupplementCard : UIViewBase
    {
        public GameObject closeBtn;
        public List<SupplementItem> itemList;
        private List<int>[] arrya = new List<int>[4];
        private int selfSeatID;
        private int zhuangSeatID;

        #region Override
        public override void Init()
        {
            EventDispatcher.AddEvent(GEnum.NamedEvent.EMjControlSetUIClose, SetClose);
            if (closeBtn != null)
            {
                UIEventListener.Get(closeBtn).onClick = CloseBtn;
            }
            else
            {
                DebugPro.DebugError("closeBtn GameObject is null");
            }
        }

        public override void OnHide()
        {

        }

        protected override void OnClose()
        {
            EventDispatcher.RemoveEvent(GEnum.NamedEvent.EMjControlSetUIClose, SetClose);
            base.OnClose();
        }

        private void SetClose(object[] vars)
        {
            CloseBtn(null);
        }

        public override void OnPushData(object[] data)
        {
            if (data != null)
            {
                selfSeatID = (int)data[0];
                zhuangSeatID = (int)data[1];
                arrya = (List<int>[])data[2];
            }
        }


        public override void OnShow()
        {
            if (itemList == null || itemList.Count <= 0)
            {
                DebugPro.DebugError("itemList is null or itemList is null, UISupplementCard  server data error");
                return;
            }

            MaskClickClose = true;
            MjDeskInfo info = MemoryData.DeskData.GetOneDeskInfo(MjDataManager.Instance.MjData.curUserData.selfDeskID);
            if (info == null)
            {
                DebugPro.DebugError("info is null,UISupplementCard  error");
                return;
            }
            long[] userIDs = info.GetAllPlayerID();
            if (userIDs == null || userIDs.Length <= 0)
            {
                DebugPro.DebugError("userIDs is null,UISupplementCard  error");
                return;
            }
            ClearUIInfo();
            PlayerDataModel playerModel;
            long userIDItem;
            int seatIDItem;
            int uiseatID = 0;
            for (int i = 0; i < userIDs.Length; i++)
            {
                userIDItem = userIDs[i];
                playerModel = MemoryData.PlayerData.get(userIDItem);
                seatIDItem = playerModel.playerDataMj.SeatID;
                uiseatID = CardHelper.GetMJUIPosByServerPos(seatIDItem, selfSeatID);
                if (uiseatID < 0 || uiseatID >= itemList.Count)
                {
                    DebugPro.DebugError("uiseatID is error:", uiseatID, "UISupplementCard  error");
                    return;
                }
                if (seatIDItem < 1 || seatIDItem >= arrya.Length + 1)
                {
                    DebugPro.DebugError("seatIDItem is error:", seatIDItem, "UISupplementCard  error");
                    return;
                }
                itemList[uiseatID].InitSupplementItem(playerModel.PlayerDataBase.HeadURL, playerModel.PlayerDataBase.Name, arrya[seatIDItem - 1]);
            }

            ShowZhuangItemInfo();
        }
        private void ShowZhuangItemInfo()
        {
            int zhuangUISeatID = CardHelper.GetMJUIPosByServerPos(zhuangSeatID, selfSeatID);
            if (zhuangUISeatID < 0 || zhuangUISeatID >= itemList.Count)
            {
                DebugPro.DebugError("zhuangUISeatID is error:", zhuangUISeatID, "UISupplementCard  error");
                return;
            }
            itemList[zhuangUISeatID].SetZhuangActive();
        }
        /// <summary>
        /// 清楚UI上的数据
        /// </summary>
        private void ClearUIInfo()
        {
            for (int i = 0; i < itemList.Count; i++)
            {
                itemList[i].ClearInfo();
            }
        }
        void CloseBtn(GameObject go)
        {
            this.Close();
        }
        #endregion
    }
}
