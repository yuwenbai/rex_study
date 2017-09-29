using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UI.ScrollViewTool;

namespace projectQ
{
    public class UIClienteleView : UIViewBase
    {
        private UIClienteleViewModel m_Model
        {
            get { return _model as UIClienteleViewModel; }
        }

        private List<long> m_OnLinePlayerID;
        private List<long> m_RelevancePlayerID;

        public UILabel currentPeo;
        public GameObject closeBtn;
        public UISprite onlineNor;
        public UISprite onlineSel;
        public GameObject onlineBtn;
        public UISprite relevanceNor;
        public UISprite relevanceSel;
        public GameObject relevanceBtn;
        public ScrollViewWrapContent scroll;
        private ScrollViewMgr<ClienteleItemData> scrollViewMgr = new ScrollViewMgr<ClienteleItemData>();

        private int m_CurPanelState = 1;

        public override void Init()
        {
            UIEventListener.Get(closeBtn).onClick = OnClickClose;
            UIEventListener.Get(onlineBtn).onClick = OnClickOnLine;
            UIEventListener.Get(relevanceBtn).onClick = OnClickRelevance;

            EventDispatcher.AddEvent(GEnum.NamedEvent.ERCCloseingAndRelushUI, RefreshPlayerData);
        }

        public override void OnPushData(object[] data)
        {
            if (m_Model.onlineData == null || m_Model.onlineData.Count == 0)
            {
                LoadSendLoading();
                ModelNetWorker.Instance.SendRoomPlayersReq();
            }
        }

        public override void OnShow()
        {
            scrollViewMgr.Init(scroll);

            ShowPlayerData();
        }

        public override void OnHide()
        {

        }

        public void RefreshPlayerData(object[] values = null)
        {
            StopSendLoading();
            ShowPlayerData();
        }

        private void ShowPlayerData()
        {
            switch (m_CurPanelState)
            {
                case 1:
                    ShowOnlinePlayer();
                    break;
                case 2:
                    ShowRelevancePlayer();
                    break;
                default:
                    break;
            }
        }

        private void ShowOnlinePlayer()
        {
            if (m_Model.onlineData != null)
            {
                scrollViewMgr.RefreshScrollView(m_Model.onlineData);
                currentPeo.text = string.Format("当前人数：{0}", m_Model.onlineData.Count);
            }
            else
                currentPeo.text = "当前人数：0";
        }

        private void ShowRelevancePlayer()
        {
            if (m_Model.relevanceData != null)
            {
                scrollViewMgr.RefreshScrollView(m_Model.relevanceData);
                currentPeo.text = string.Format("当前人数：{0}", m_Model.relevanceData.Count);
            }
            else
                currentPeo.text = "当前人数：0";
        }

        private void OnClickOnLine(GameObject obj)
        {
            if(m_CurPanelState != 1)
            {
                m_CurPanelState = 1;
                onlineNor.alpha = 0;
                onlineSel.alpha = 1;
                relevanceNor.alpha = 1;
                relevanceSel.alpha = 0;
                ShowPlayerData();
            }
        }

        private void OnClickRelevance(GameObject obj)
        {
            if (m_CurPanelState != 2)
            {
                m_CurPanelState = 2;
                onlineNor.alpha = 1;
                onlineSel.alpha = 0;
                relevanceNor.alpha = 0;
                relevanceSel.alpha = 1;
                ShowPlayerData();
            }
        }

        private void OnClickClose(GameObject go)
        {
            Close();
        }

        protected override void OnClose()
        {
            EventDispatcher.RemoveEvent(GEnum.NamedEvent.ERCCloseingAndRelushUI, RefreshPlayerData);
        }
    }
}
