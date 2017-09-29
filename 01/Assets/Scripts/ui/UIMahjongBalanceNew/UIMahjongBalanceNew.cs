using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace projectQ
{
    public class UIMahjongBalanceNew : UIViewBase
    {
        private enum e_ShowType
        {
            OpenInWithout = 1,              //外部打开
            OpenInGameOver = 2,             //正常结束
        }

        //四个玩家的数据
        public List<GameObject> balanceInfoPointList;
        //本局麻将类型
        public UILabel typeName;
        //胡牌类型
        public UILabel awardName;
        public GameObject awardScrow;
        public UILabel awardName2;
        public UIButton closeBtn;
        public UILabel closeLabel;

        public UITexture liujvSprite;
        public UITexture shengSprite;
        public UITexture fuSprite;

        public GameObject leftSpecialMj;
        public GameObject rightSpecialMj;
        public GameObject specialItem;

        public GameObject infoItem;

        //回放相关按钮
        public GameObject obj_PlayBacks;
        public GameObject btn_Close;
        public GameObject btn_PlayBack;

        public UILabel label_Time;

        private object[] m_SendData = null;

        private List<UIMahjongBalanceInfo> m_balanceInfoList;
        //牌局类型
        private MjBalanceNew m_PanelData;
        private bool m_IsInit = false;
        private float m_CloseTime;

        private e_ShowType m_ShowType;

        #region 重写
        public override void Init()
        {
        }

        public override void OnShow()
        {
            m_ShowType = e_ShowType.OpenInWithout;
        }

        public override void OnHide()
        {
        }
        #endregion

        public override void OnPushData(object[] data)
        {
            InitEvent();

            if (data.Length > 2 && data[2] != null)
            {
                MjBalanceNew panelData = data[2] as MjBalanceNew;

                if (panelData == null || panelData.playerInfoList.Count != 4)
                {
                    Close();
                    return;
                }

                EventDispatcher.FireEvent(GEnum.NamedEvent.SysUI_Chat_CloseUI);

                m_PanelData = panelData;
                ShowPanelData();
            }
            else
            {
                m_SendData = data;
                m_ShowType = e_ShowType.OpenInGameOver;
            }
            if (data.Length > 3)
                m_ShowType = (e_ShowType)((int)data[3]);

            if (FakeReplayManager.Instance.ReplayState)
            {
                Debug.Log("rextest -------------ReplayState " + 1111);
            }
            obj_PlayBacks.gameObject.SetActive(m_ShowType == e_ShowType.OpenInWithout && ConstDefine.ShowPlayBack);// && FakeReplayManager.Instance.ReplayState
            closeBtn.gameObject.SetActive((m_ShowType == e_ShowType.OpenInGameOver || !ConstDefine.ShowPlayBack) && !FakeReplayManager.Instance.ReplayState);// || !FakeReplayManager.Instance.ReplayState
            if (FakeReplayManager.Instance.ReplayState)
            {
                EventDispatcher.FireSysEvent(GEnum.NamedEvent.SysUI_UIReplayCtrl_Play_Over);
            }
            //switch (m_ShowType)
            //{
            //    case e_ShowType.OpenInWithout:
            //        obj_PlayBacks.gameObject.SetActive(true);
            //        closeBtn.gameObject.SetActive(false);
            //        break;
            //    case e_ShowType.OpenInGameOver:
            //        obj_PlayBacks.gameObject.SetActive(false);
            //        closeBtn.gameObject.SetActive(true);
            //        break;
            //}
        }

        protected override void OnShowAndAnimationEnd()
        {
            if (m_SendData != null)
            {
                if (m_SendData.Length >= 2)
                {
                    int deskID = (int)m_SendData[0];
                    int curBureau = (int)m_SendData[1];
                    EventDispatcher.FireEvent(GEnum.NamedEvent.EMjControlSendGetBalanceNew, deskID, curBureau);
                }
            }
        }

        public void RefreshPanelData(MjBalanceNew panelData)
        {
            if (panelData == null)
            {
                Close();
                return;
            }
            m_PanelData = panelData;
            ShowPanelData();
        }

        private void InitInfo()
        {
            if(m_balanceInfoList == null || m_balanceInfoList.Count == 0)
            {
                if (m_balanceInfoList == null)
                    m_balanceInfoList = new List<UIMahjongBalanceInfo>();

                for (int i = 0; i < balanceInfoPointList.Count; i++)
                {
                    GameObject obj = NGUITools.AddChild(balanceInfoPointList[i].gameObject, infoItem);
                    UIMahjongBalanceInfo info = obj.GetComponent<UIMahjongBalanceInfo>();
                    if (info != null)
                        m_balanceInfoList.Add(info);
                }
            }
        }

        private void ShowPanelData()
        {
            m_SendData = null;

            InitInfo();

            #region 通过玩家自己的坐标来显示各个玩家的坐标
            List<MjBalanceNew.BalancePlayerInfo> list = new List<MjBalanceNew.BalancePlayerInfo>() { null, null, null, null };

            MjBalanceNew.BalancePlayerInfo infodata = m_PanelData.playerInfoList.Find(x => x.userID == MemoryData.UserID);

            for (int i = 0; i < m_PanelData.playerInfoList.Count; i++)
            {
                int seatid = CardHelper.GetMJUIPosByServerPos(m_PanelData.playerInfoList[i].userSeat, infodata.userSeat);
                list[3 - seatid] = m_PanelData.playerInfoList[i];
            }

            int myDepth = 0;
            UIPanel pan = this.GetComponent<UIPanel>();
            if (pan != null)
                myDepth = pan.depth;

            int huState = 0;
            for (int i = 0; i < m_balanceInfoList.Count; i++)
            {
                e_UIMjBalanceShowType showType = (e_UIMjBalanceShowType)m_PanelData.showType;
                bool isGuangDongMa = showType != e_UIMjBalanceShowType.XueLiuXueZhan && (m_PanelData.horseInfo != null && m_PanelData.horseInfo.buyHorseSiJiaData != null);
                if (isGuangDongMa)
                {
                    showType = e_UIMjBalanceShowType.GuangDongZhuaMa;
                }
                var data = isGuangDongMa ? m_PanelData.horseInfo : null;

                m_balanceInfoList[i].ShowData(list[i], showType, m_PanelData.dealerSeatID, huState, m_PanelData.gameType, data);
                m_balanceInfoList[i].ChangeMarkSprite(i);

                m_balanceInfoList[i].ChangeScrollView(myDepth);

                if (list[i].userID == MemoryData.UserID)
                    ShowBgSprite(m_PanelData.isDraw, list[i].scoreCur);
            }
            #endregion

            typeName.text = CardHelper.BalanceGetGameTypeName(m_PanelData.gameTypeSub);

            string result = string.Format("桌号:{0}      ", m_PanelData.deskID);
            MahjongPlayOption option = null;

            MemoryData.MahjongPlayData.SortOption(ref m_PanelData.gameRulerType, m_PanelData.gameTypeSub); //排序

            for (int i = 0; i < m_PanelData.gameRulerType.Count; i++)
            {
                option = MemoryData.MahjongPlayData.GetMahjongPlayOption(m_PanelData.gameTypeSub, m_PanelData.gameRulerType[i]);
                if (option != null)
                {
                    result += option.Name + " ";
                }
            }
            awardName.text = result;
            awardName.gameObject.SetActive(awardName.localSize.x <= 1182);
            awardScrow.gameObject.SetActive(awardName.localSize.x > 1182);
            if (awardName.localSize.x > 1182)
            {
                awardName2.text = result;
            }


            if (m_PanelData.showTime <= 0)
                closeBtn.gameObject.SetActive(false);

            m_CloseTime = m_PanelData.showTime;
            m_IsInit = true;
            if (!m_PanelData.showByResult)
                closeLabel.text = m_PanelData.curBureau == m_PanelData.maxBureau ? "关闭" : "下一局";
            else
                closeLabel.text = "关闭";
                
            ShowDownMj(leftSpecialMj, EnumMjOpenMaType.ZhuaMa);
            ShowDownMj(rightSpecialMj, EnumMjOpenMaType.JiangMa);
            ShowDownMj(rightSpecialMj, EnumMjOpenMaType.BaoZhaMa);

            ShowGameTime();
        }

        private void ShowGameTime()
        {
            if (label_Time == null || m_PanelData.gameEndTime <= 0)
                return;
            label_Time.text = string.Format("{0}", ((long)m_PanelData.gameEndTime).ToTimeString());//对局时间:
        }

        private void ShowDownMj(GameObject parent, EnumMjOpenMaType type)
        {
            if (m_PanelData.horseInfo != null && m_PanelData.horseInfo.buyHorseDataDic.ContainsKey((int)type))
            {
                var data = m_PanelData.horseInfo.buyHorseDataDic[(int)type];
                if (data.horseItemList != null && data.horseItemList.Count > 0)
                    ShowSpecialMJ(parent, new object[] { data.horseItemList, data.gameType });
                else
                    ShowSpecialMJ(parent, null);
            }
        }

        private void ShowSpecialMJ(GameObject parent,object[] obj)
        {
            if (parent != null)
            {
                if (parent.activeSelf != false)
                {
                    if (parent.transform.childCount > 0)
                        parent.transform.DestroyChildren();
                    if (obj == null)
                    {
                        parent.SetActive(false);
                        return;
                    }
                }
            }

            UIMahjongBalanceSpecialMjBtn btnsPan = null;
            if (parent.transform.childCount > 0)
            {
                btnsPan = parent.transform.GetChild(0).GetComponent<UIMahjongBalanceSpecialMjBtn>();
            }
            if(btnsPan == null)
            {
                btnsPan = NGUITools.AddChild(parent, specialItem.gameObject).GetComponent<UIMahjongBalanceSpecialMjBtn>();
            }
            if (btnsPan == null)
                return;

            btnsPan.gameObject.SetActive(true);
            btnsPan.InitData(this);
            btnsPan.InitSetData(obj);
        }

        //显示是否流局
        private void ShowBgSprite(bool isLiu, int score)
        {

            liujvSprite.alpha = isLiu ? 1 : 0;
            shengSprite.alpha = !isLiu && score > 0 ? 1 : 0;
            fuSprite.alpha = !isLiu && score < 0 ? 1 : 0;
        }

        private void InitEvent()
        {
            UIEventListener.Get(closeBtn.gameObject).onClick = OnClickMouse;
            UIEventListener.Get(btn_Close).onClick = OnClickMouse;
            UIEventListener.Get(btn_PlayBack).onClick = OnClickPlayBack;
        }

        private void OnClickMouse(GameObject obj)
        {
            m_IsInit = false;
            if (m_PanelData != null && !m_PanelData.showByResult)
            {
                bool isClose = m_PanelData.curBureau == m_PanelData.maxBureau;
                //if (m_PanelData.curBureau != m_PanelData.maxBureau)
                EventDispatcher.FireEvent(GEnum.NamedEvent.EMjControlClickCloseBalance, isClose);
            }

            m_SendData = null;
            Close();
        }

        //点击查看回放
        private void OnClickPlayBack(GameObject obj)
        {
            //rextest
			FakeReplayManager.Instance.RequestReplayFrame(m_PanelData.deskID, m_PanelData.curBureau);
        }
    }

    public enum e_UIMjBalanceShowType
    {
        Normal = 1,
        XueLiuXueZhan = 2,
        Special = 3,

        GuangDongZhuaMa,
    }
}
