/**
* @Author Xin.Wang
*
*
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace projectQ
{
    public class UIMahjongResult : UIViewBase
    {
        public enum e_ShowType
        {
            OpenInWithout = 1,              //外部打开
            OpenInPlaying = 2,              //游戏中打开
            OpenInGameOver = 3,             //正常结束
            OpenInGZSee = 4,                //馆长查看
        }

        private e_ShowType m_ShowType;

        #region override
        public override void OnPushData(object[] data)
        {
            if (data != null && data.Length > 0)
            {
                _deskID = (int)data[0];
                _evetFire = false;
                _clickMaskClose = false;
                if(data.Length > 1)
                {
                    m_ShowType = (e_ShowType)((int)data[1]);
                }
                if (data.Length > 2)
                {
                    _clickMaskClose = (bool)data[2];
                }
                if (data.Length > 3)
                {
                    _evetFire = (bool)data[3];
                }
            }
        }

        public override void Init()
        {
            MaskClickClose = ((m_ShowType == e_ShowType.OpenInGameOver) || (m_ShowType == e_ShowType.OpenInPlaying));//_clickMaskClose;
            UIEventListener.Get(btn_Close).onClick = OnClickClose;
            if (m_ShowType == e_ShowType.OpenInGameOver)//(!_evetFire)
            {
                EventDispatcher.AddEvent(GEnum.NamedEvent.EMjControlCloseMaskUI, CloseUI);
            }
        }

        public override void OnHide()
        {

        }

        public override void OnShow()
        {
            IniData();
        }

        protected override void OnClose()
        {
            if (m_ShowType != e_ShowType.OpenInPlaying)//(_evetFire)
            {
                EventDispatcher.FireEvent(GEnum.NamedEvent.EMjControlClickCloseResult);
            }
            else
            {
                EventDispatcher.RemoveEvent(GEnum.NamedEvent.EMjControlCloseMaskUI, CloseUI);
            }

            ClearData();
            base.OnClose();
        }
        #endregion


        private void CloseUI(object[] vars)
        {


            this.Close();
        }


        #region property
        public GameObject btn_Share = null;
        public GameObject btn_Close = null;

        public GameObject obj_ShowOne = null;
        public UILabel label_TypeOne = null;

        public GameObject obj_ShowTwo = null;
        public UILabel Label_Type = null;
        public UILabel label_Odd = null;

        public GameObject obj_ShowThree = null;
        public UILabel m_Label_Up = null;
        public UILabel m_Label_Down = null;
        public UILabel m_Label_Type = null;

        public UIMahjongResultPlayer[] playerInfo = null;

        public UIGrid grid_Detail = null;
        public GameObject item_Detail = null;

        public UILabel label_Process = null;
        public UILabel m_Label_Time = null;
        #endregion


        private int _deskID = -1;
        private GameResult resultData = null;
        private bool _evetFire = false;
        private bool _clickMaskClose = false;
        private bool _xunshiState = false;

        public void RefreshData(int deskID)
        {
            StopSendLoading();
            _deskID = deskID;
            IniData();
        }


        private void IniData()
        {
            if (_deskID < 0)
            {
                return;
            }

            resultData = MemoryData.ResultData.get(_deskID);
            if (resultData == null)
            {
                return;
            }

            btn_Share.SetActive(true);
            UIEventListener.Get(btn_Share).onClick = OnClickShare;


            obj_ShowOne.SetActive(false);
            obj_ShowTwo.SetActive(false);

            //show two
            obj_ShowTwo.SetActive(resultData.resultType == EnumGameResultType.NormalType);
            obj_ShowThree.SetActive(resultData.resultType == EnumGameResultType.WinPayType);

            Label_Type.text = CardHelper.GetGameMessageByType(resultData.gameTypeSub);
            label_Odd.text = "桌号：" + resultData.deskID;

            m_Label_Up.text = CardHelper.GetGameMessageByType(resultData.gameTypeSub);
            m_Label_Down.text = "桌号：" + resultData.deskID;
            m_Label_Type.text = "赢家付卡";

            //ini player
            MjTitleInfo titleInfo = null;

            int selfSeatID = 0;
            int ownerSeatID = 0;
            for (int i = 0; i < resultData.resultPlayerList.Count; i++)
            {
                GameResultPlayer playerInfo = resultData.resultPlayerList[i];

                if (playerInfo.userID == resultData.ownerUserID)
                {
                    ownerSeatID = playerInfo.seatID;
                }

                if (playerInfo.userID == MemoryData.UserID)
                {
                    selfSeatID = playerInfo.seatID;
                }
            }

            int showSeatID = selfSeatID;
            if (showSeatID == 0)
            {
                showSeatID = 1;
                _xunshiState = true;
            }

            bool showTime = false;
            int timeNum = 0;

            for (int i = 0; i < resultData.resultPlayerList.Count; i++)
            {
                GameResultPlayer modelInfo = resultData.resultPlayerList[i];
                titleInfo = resultData.GetTitleInfoBySeat(modelInfo.seatID);
                if (titleInfo.titleList.Count > 0)
                    timeNum++; 
            }
            showTime = timeNum > 1;

            for (int i = 0; i < resultData.resultPlayerList.Count; i++)
            {
                GameResultPlayer modelInfo = resultData.resultPlayerList[i];

                titleInfo = resultData.GetTitleInfoBySeat(modelInfo.seatID);

                int uiSeat = CardHelper.GetMJUIPosByServerPos(modelInfo.seatID, showSeatID);
                bool isOwner = modelInfo.seatID == ownerSeatID && resultData.resultType != EnumGameResultType.WinPayType;

                GameResultCostData removeData = null;

                if (resultData.resultCostDic.ContainsKey(modelInfo.seatID))
                {
                    removeData = resultData.resultCostDic[modelInfo.seatID];
                }

                playerInfo[uiSeat].IniPlayerInfo(
                     modelInfo.nickName, titleInfo.score, titleInfo.titleList, modelInfo.HeadUrl, !_xunshiState, isOwner, (int)m_ShowType, removeData, showTime, OnClickDaYingJiaRule);
            }

            ShowMessage(timeNum > 0);

            //ini detail
            int overRound = resultData.maxBouts;
            if (resultData.bureauDetailList != null && resultData.bureauDetailList.Count != 0 && resultData.bureauDetailList.Count < resultData.maxBouts)
            {
                overRound = resultData.bureauDetailList.Count;
                if (MaskClickClose)
                {
                    overRound++;
                }
            }


            label_Process.text = "局数: " + overRound + "/" + resultData.maxBouts;
            label_Process.gameObject.SetActive(true);
            
            m_Label_Time.text = string.Format("对局时间:{0}", ((long)resultData.recordTime).ToTimeString());

            UITools.CreateChild<MjBureauDetialInfo>(grid_Detail.transform, item_Detail, resultData.bureauDetailList, CreateBureauCallBack, true);
            grid_Detail.Reposition();

            EventDispatcher.AddEvent(GEnum.NamedEvent.ERCCloseingAndRelushUI, RelushUI);
        }

        private void OnClickDaYingJiaRule()
        {
            this.LoadUIMain("UIDaYingJiaRule", resultData);
        }

        //显示大赢家提示
        private void ShowMessage(bool haveDaYingJia)
        {
            if (m_ShowType != e_ShowType.OpenInGameOver)
                return;

            if (!MjDataManager.Instance.CheckCostDataContain())
                return;

            var data = MjDataManager.Instance.GetCostData();
            if (data == null)
                return;

            string showResult = "";
            if (!haveDaYingJia)
            {
                showResult = string.Format("本局无大赢家，您最后进入牌桌，支付本局桌卡{0}，剩余桌卡{1}", data.CostNum, data.RestNum);
            }
            else if (data.isLastUser)
            {
                showResult = string.Format("您在大赢家中最后进入牌桌，已支付本局桌卡x{0}，剩余桌卡x{1}", data.CostNum, data.RestNum);
            }
            else
            {
                showResult = string.Format("您是大赢家，已支付本局桌卡x{0}，剩余桌卡x{1}", data.CostNum, data.RestNum);
            }

            this.LoadTip(showResult);
        }

        private void RelushUI(object[] values)
        {
            StopSendLoading();
        }

        private void CreateBureauCallBack(GameObject obj, MjBureauDetialInfo detailInfo)
        {
            int index = resultData.bureauDetailList.IndexOf(detailInfo);

            UIMahjongResultDetail detail = obj.GetComponent<UIMahjongResultDetail>();

            MjBureauInfo info = detailInfo.bureauInfo;

            int showSeatID = resultData.selfSeatID;


            if (showSeatID <= 0)
            {
                showSeatID = 1;
            }

            if (showSeatID > 0)
            {
                string bereauInfo = CardHelper.GetBereauInfo(info.nBureauCount);

                List<int> winIndex = new List<int>();
                int zimoIndex = -1;
                int paoIndex = -1;

                int[] scoreInfo = new int[info.detailList.Count];
                for (int i = 0; i < scoreInfo.Length; i++)
                {
                    MjScore curScore = info.detailList[i];

                    int uiSeat = CardHelper.GetMJUIPosByServerPos(curScore.seatID, showSeatID);

                    if (resultData.showType == 1 && curScore.isDianPao)
                    {
                        paoIndex = uiSeat;
                    }
                    scoreInfo[uiSeat] = curScore.score;
                }


                if (resultData.showType == 1 && info.nWinSeatID != null && info.nWinSeatID.Count > 0)
                {
                    for (int i = 0; i < info.nWinSeatID.Count; i++)
                    {
                        int uiIndexWin = CardHelper.GetMJUIPosByServerPos(info.nWinSeatID[i], showSeatID);
                        winIndex.Add(uiIndexWin);

                        if (paoIndex < 0)
                        {
                            zimoIndex = uiIndexWin;
                        }
                    }
                }
				//rextest
                bool bShowReplay = FakeReplayManager.Instance.CheckReplayData(_deskID, index);
                detail.IniDetailInfo(bereauInfo, scoreInfo, winIndex, zimoIndex, paoIndex, false, index, (int)m_ShowType, OnClickItem, OnClickPlayBack, bShowReplay);

            }

        }


        #region Btn click
        private void OnClickShare(GameObject obj)
        {
            if (_deskID > 0)
            {
                Vector2 startPoint = new Vector2(0.0f, 0.0f);
                Vector2 shotSize = new Vector2(Screen.width, Screen.height);
                Tools_TexScreenshot.Instance.Texture_Screenshot(startPoint, shotSize, TexShotCallBack);
            }
        }

        /// <summary>
        /// 截屏压缩回调
        /// </summary>
        void TexShotCallBack(bool isBol)
        {
            if (isBol)
            {
                string base64Str = Tools_TexScreenshot.Instance.Texture_ToBase64();

                if (!string.IsNullOrEmpty(base64Str))
                {
                    string timeStr = ((long)resultData.recordTime).ToTimeFormatString();
                    timeStr = timeStr.Replace(" ", "");
                    string[] values = timeStr.Split(new char[] { '@' });
                    string resultTime = values[0] + " " + values[1];

                    WXShareParams shareParams = new WXShareParams("WEVHAT_SHARE_GAME_RESULT", base64Str);
                    shareParams.InsertUrlParams(new object[] { _deskID.ToString() });
                    shareParams.InsertDescParams(new object[] { _deskID.ToString(), resultTime });
                    SDKManager.Instance.SDKFunction("WEVHAT_SHARE_GAME_RESULT", shareParams);
                }
            }
        }

        private void OnClickClose(GameObject obj)
        {
            this.Close();
        }

        private void OnClickItem(int index)
        {
            if (!_xunshiState)
            {
                MjBureauDetialInfo detailInfo = resultData.bureauDetailList[index];
                int curBureau = detailInfo.bureauInfo.nBureauCount;
                MjBalanceNew balanceInfo = MemoryData.BalanceData.get(this._deskID, curBureau);
                if (balanceInfo != null)
                {
                    balanceInfo.showByResult = true;
                }

                int openBlanceType = 1;


                switch (m_ShowType)
                {
                    case e_ShowType.OpenInWithout:
                        {
                            openBlanceType = 1;
                            bool bShowReplay = FakeReplayManager.Instance.CheckReplayData(_deskID, index);
                            if (!bShowReplay)
                            {
                                openBlanceType = 2;
                            }
                        }
                        break;
                    case e_ShowType.OpenInGZSee:
                    case e_ShowType.OpenInPlaying:
                    case e_ShowType.OpenInGameOver:
                        openBlanceType = 2;
                        break;
                }
                LoadUIMain("UIMahjongBalanceNew", this._deskID, curBureau, balanceInfo, openBlanceType);
            }
        }
        private void OnClickPlayBack(int index)
        {
                //播放当前选中的局
                Debug.Log("OnReplayClickItem index is " + index);
                //rextest
                FakeReplayManager.Instance.RequestReplayFrame(_deskID, index + 1);
        }

        #endregion


        private void ClearData()
        {
            grid_Detail.transform.DestroyChildren();
            _deskID = -1;
            resultData = null;
            _evetFire = false;
        }


    }
}

