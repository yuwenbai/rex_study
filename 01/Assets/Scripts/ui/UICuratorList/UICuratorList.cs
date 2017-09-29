/**
 * @Author Xin.Wang
 *
 *
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UI.ScrollViewTool;
using System;

namespace projectQ
{
    public class UICuratorList : UIViewBase
    {
        public UILabel label_Commission = null;         //佣金 
        public UILabel label_TableCount = null;         //桌子数 
        public UILabel label_PlayerCount = null;        //当前人数 

        public UILabel label_Message = null;            //麻将馆message

        public ScrollViewWrapContent ParlorScrollView;
        private ScrollViewMgr<CuratorListData> scrollViewMgr = new ScrollViewMgr<CuratorListData>();

        public GameObject obj_Empty = null;             //没有桌子的状态
        public GameObject obj_HaveTable = null;

        private int roomID = -1;
        private List<CuratorListData> dataList = new List<CuratorListData>();
        private MjRoom roomInfo = null;

        public override void Init()
        {

        }

        public override void OnShow()
        {

        }

        public override void OnHide()
        {
            ClearMessage();
        }

        protected override void OnClose()
        {
            ClearMessage();
        }


        public override void OnPushData(object[] data)
        {
            //setData
            roomID = (int)data[0];
            SetData();
            SetBaseMessage();

            scrollViewMgr.Init(ParlorScrollView);
            scrollViewMgr.RefreshScrollView(dataList);
        }

        public void OnRefreshData(int roomID)
        {
            this.roomID = roomID;
            dataList.Clear();
            SetData();
            SetBaseMessage();

            scrollViewMgr.RefreshScrollView(dataList);
        }

        public void OnUpdateMessage(int deskCount, int members, int onlineNum)
        {
            SetBaseMessage();
        }


        private void SetData()
        {
            roomInfo = MemoryData.MjHallData.GetMjHallById(roomID);
            List<int> deskID = MemoryData.MjHallData.GetMjHallDeskList(roomID);
            bool haveData = (deskID != null && deskID.Count > 0);
            obj_Empty.SetActive(!haveData);
            obj_HaveTable.SetActive(haveData);

            if (haveData)
            {
                for (int i = 0; i < deskID.Count; i++)
                {
                    MjDeskInfo info = MemoryData.DeskData.GetOneDeskInfo(deskID[i]);

                    CuratorListData showData = new CuratorListData();
                    showData.deskInfo = info;
                    showData.clickBtnCall = OnClickView;
                    dataList.Add(showData);
                }
            }
        }

        private void SetBaseMessage()
        {
            if (roomInfo != null)
            {
                label_Commission.text = roomInfo.CommisionMoney.ToString();
                label_TableCount.text = roomInfo.roomDeskIDList.Count.ToString();
                label_PlayerCount.text = roomInfo.OnlineNum.ToString();
            }
        }


        private void OnClickView(int deskID)
        {
            MjDeskInfo info = MemoryData.DeskData.GetOneDeskInfo(deskID);
            if (info != null && info.bouts > 1 && info.rounds > 0)
            {
                long userID = MemoryData.PlayerData.MyPlayerModel.PlayerDataBase.UserID;
                LoadSendLoading();
                ModelNetWorker.Instance.FMjRoomViewDeskRecordReq(userID, roomInfo.RoomID, deskID);
                LoadUIMain("UIMahjongResult", deskID, 4);
            }
        }


        private void ClearMessage()
        {
            roomID = -1;
            roomInfo = null;
        }

    }

}

