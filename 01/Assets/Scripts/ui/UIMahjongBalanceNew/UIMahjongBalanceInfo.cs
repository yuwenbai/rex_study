using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace projectQ
{
    public class UIMahjongBalanceInfo : MonoBehaviour
    {
        #region 通用属性
        #region 头像变量
        public Transform HeadCont;
        public UITexture playerIcon;
        public UILabel playerName;
        public UISprite playerZhuang;
        public UISprite playerChengbao;
        public UILabel playerCostType;
        public UILabel playerCostNum;
        public UILabel playerCostType2;
        #endregion
        public UIWidget playerPan;
        public UISprite playerHu;
        public UILabel playerHuType1;
        public UIScrollView playerScrow;
        public UILabel playerHuType2;
        public BoxCollider playerBox;
        public BestRecordController playerMahjongPanel;
        public UIGrid playerGrid;

        private e_UIMjBalanceShowType m_ShowType;
        private int m_GameType;
        private string m_HeadUrl;
        private MjBalanceNew.BalancePlayerInfo m_Data;
        private int m_ZhuangID;
        private MjBalanceNew.MjHorseInfo m_HorseBaseData;
        private MjBalanceNew.MjHorseInfo.BuyHorseItem m_HorseData;

        public UISprite m_MahjongItem;
        public UISprite m_MahjongItem2;
        public UISprite m_FontSprite;
        public UISprite m_MarkSprite;
        public UILabel m_UILabel_SpecialMj;

        public UIGrid m_Grid_Special;
        #endregion

        public void ShowData(MjBalanceNew.BalancePlayerInfo data, e_UIMjBalanceShowType showType, int zhuangId, int state, int gameType, MjBalanceNew.MjHorseInfo horseData)
        {
            m_ShowType = showType;
            m_GameType = gameType;
            m_ZhuangID = zhuangId;
            m_Data = data;
            m_HorseBaseData = horseData;
            if (horseData != null)
                m_HorseData = horseData.buyHorseSiJiaData.horseItemList.Find(x => x.seatID == data.userSeat);

            if (m_Data == null)
                return;

            playerPan.alpha = 1;
            //normalPanel.alpha = 1;// m_ShowType == 1 ? 1 : 0;
            //specialPanel.alpha = 0;// m_ShowType != 1 ? 1 : 0;

            ShowPlayerIcon();
            ShowMahjongData();
            ShowMahjongPlayerScore();
        }

        public void ChangeMarkSprite(int index)
        {
            if(m_MarkSprite != null)
            {
                string spName = "";
                switch (index)
                {
                    case 0:
                        spName = "desk_icon_shangjia1";
                        break;
                    case 1:
                        spName = "desk_icon_duijia1";
                        break;
                    case 2:
                        spName = "desk_icon_xiajia1";
                        break;
                    case 3:
                        spName = "desk_icon_wo1";
                        break;
                }

                m_MarkSprite.spriteName = spName;
                m_MarkSprite.MakePixelPerfect();
            }
        }

        #region 显示玩家头像相关信息
        public void ShowHeadPostion()
        {
            switch (m_ShowType)
            {
                case e_UIMjBalanceShowType.Normal:
                case e_UIMjBalanceShowType.Special:
                    if (m_Data.checkShowList == null || m_Data.checkShowList.Count == 0)
                        HeadCont.transform.localPosition = new Vector3(-472, 8, 0);
                    else if (m_Data.checkShowList.Count == 1)
                        HeadCont.transform.localPosition = new Vector3(-506, 8, 0);
                    else
                        HeadCont.transform.localPosition = new Vector3(-532, 8, 0);
                    break;
                case e_UIMjBalanceShowType.GuangDongZhuaMa:
                    HeadCont.transform.localPosition = new Vector3(-530, 8, 0);
                    break;
                default:
                    HeadCont.transform.localPosition = new Vector3(-547.6f, 8, 0);
                    break;
            }
        }

        public void ShowPlayerIcon()
        {
            ShowHeadPostion();
            switch (m_ShowType)
            {
                case e_UIMjBalanceShowType.Normal:
                case e_UIMjBalanceShowType.Special:
                case e_UIMjBalanceShowType.GuangDongZhuaMa:
                    bool iszhuang = m_ZhuangID == m_Data.userSeat;
                    playerZhuang.gameObject.SetActive(iszhuang);

                    if (m_Data.checkShowList != null && m_Data.checkShowList.Count > 0)
                    {
                        bool isMore = m_Data.checkShowList.Count > 1;
                        if (isMore)
                        {
                            string strRes = "";
                            for (int n = 0; n < m_Data.checkShowList.Count; n++)
                            {
                                string[,] str = CardHelper.BalanceGetSpecailType(m_Data.checkShowList[n].checkType, m_Data.checkShowList[n].checkValue);

                                strRes += string.Format("{0} {1}{2}", str[0, 0], str[0, 1], n < m_Data.checkShowList.Count - 1 ? "\n" : "");
                            }
                            playerCostType2.text = strRes;

                            playerCostType.text = "";
                            playerCostNum.text = "";
                        }
                        else
                        {
                            string[,] str = CardHelper.BalanceGetSpecailType(m_Data.checkShowList[0].checkType, m_Data.checkShowList[0].checkValue);

                            playerCostType.text = str[0, 0];
                            playerCostNum.text = str[0, 1];
                            playerCostType2.text = "";
                        }
                    }
                    else
                    {
                        playerCostType.text = "";
                        playerCostNum.text = "";
                        playerCostType2.text = "";
                    }
                    break;
                //case e_UIMjBalanceShowType.GuangDongZhuaMa:
                //    break;
                default:
                    playerZhuang.gameObject.SetActive(false);

                    playerCostType.text = "";
                    playerCostNum.text = "";
                    playerCostType2.text = "";
                    break;
            }

            playerChengbao.gameObject.SetActive(m_Data.isChengbao);
            if (m_HeadUrl != m_Data.userHead)
            {
                m_HeadUrl = m_Data.userHead;
                playerIcon.mainTexture = Resources.Load<Texture>(GameAssetCache.Texture_Hand_Path);
                DownHeadTexture.Instance.WeChat_HeadTextureGet(m_Data.userHead, SetPlayerIcon);
            }
            playerName.text = m_Data.userNick;
        }

        void SetPlayerIcon(Texture2D HeadTexture, string headName)
        {
            QLoger.LOG(" 头像赋值 --  " + HeadTexture.name);
            playerIcon.mainTexture = HeadTexture;
        }
        #endregion

        #region 显示玩家麻将信息
        private void ShowMjPostion()
        {
            switch (m_ShowType)
            {
                case e_UIMjBalanceShowType.Normal:
                case e_UIMjBalanceShowType.Special:
                    playerMahjongPanel.transform.localPosition = new Vector3(-361.4f, -34.6f, 0);
                    playerHuType1.transform.localPosition = new Vector3(-361, 23, 0);
                    playerScrow.transform.localPosition = new Vector3(-211, -45, 0);
                    break;
                case e_UIMjBalanceShowType.GuangDongZhuaMa:
                    playerMahjongPanel.transform.localPosition = new Vector3(-463f, -34.6f, 0);
                    playerHuType1.transform.localPosition = new Vector3(-463, 23, 0);
                    playerScrow.transform.localPosition = new Vector3(-314, -45, 0);
                    break;
                default:
                    playerMahjongPanel.transform.localPosition = new Vector3(-482, -34.4f, 0);
                    playerHuType1.transform.localPosition = new Vector3(-481, 23, 0);
                    playerScrow.transform.localPosition = new Vector3(-332, -45, 0);
                    break;
            }
        }

        private void ShowMahjongData()
        {
            playerMahjongPanel.IniBestRecord(m_Data.handRecord);

            if (m_UILabel_SpecialMj != null)
                m_UILabel_SpecialMj.text = "";

            ShowMjPostion();

            switch (m_ShowType)
            {
                case e_UIMjBalanceShowType.Normal:
                case e_UIMjBalanceShowType.Special:
                case e_UIMjBalanceShowType.GuangDongZhuaMa:
                    if (m_Data.huList != null && m_Data.huList.Count > 0)
                    {
                        var trans = CreatItem(playerMahjongPanel.transform, m_Data.huList[0]);
                        SetHuSpecialTip(trans, m_Data.handRecord, m_Data.huList[0]);
                        //坐标
                        trans.localPosition = new Vector3(563, -2, 0);
                    }

                    ChangeLabelText(playerScrow, playerHuType1, playerHuType2, 40, playerBox);

                    ShowSpecialMj();
                    break;
                default:
                    if (m_Data.huList != null && m_Data.huList.Count > 0)
                    {
                        int count = m_Data.huList.Count;
                        int starPos = 563;

                        for (int i = 0; i < count; i++)
                        {
                            var trans = CreatItem(playerMahjongPanel.transform, m_Data.huList[i]);
                            SetHuSpecialTip(trans, m_Data.handRecord, m_Data.huList[i]);
                            //坐标
                            if (i > 12)
                            {
                                trans.localPosition = new Vector3(1043 - 40 * (i - 13), -72, 0);
                            }
                            else
                            {
                                trans.localPosition = new Vector3(starPos + i * 40, -2, 0);
                            }
                        }
                    }

                    ChangeLabelText(playerScrow, playerHuType1, playerHuType2, 50, playerBox);
                    break;
            }
        }

        private void SetHuSpecialTip(Transform paiItem, BestMjRecord recordData, int curCardID)
        {
            if (recordData.specailType != EnumMjSpecialType.Null && recordData.specialCardID.Contains(curCardID))
            {
                //设置特殊
                Transform chidTrans = paiItem.GetChild(0);
                if (chidTrans != null)
                {
                    UISprite childSp = chidTrans.GetComponent<UISprite>();
                    if (childSp != null)
                    {
                        string showName = CardHelper.GetHunSpriteName((int)(recordData.specailType), recordData.mjType);
                        childSp.spriteName = showName;
                        childSp.gameObject.SetActive(true);
                    }
                }

            }
        }

        private void ChangeLabelText(UIScrollView scro, UILabel labe1, UILabel labe2, int numLi, BoxCollider colider)
        {
            string str = "";
            string str2 = "";
            bool isChangeShow = false;

            #region old
            /*if (m_Data.detailList != null && m_Data.detailList.Count > 0)
            {
                #region old
                //List<string> list = CommonTools.BalanceGetScoreDetailList(m_Data.detailList, m_Data.userSeat, m_GameType);
                //bool isLineTwo = false;
                //for (int i = 0; i < list.Count; i++)
                //{
                //    str += string.Format("{0}{1}", list[i], "  ");
                //    if (str.Length > 47 && !isLineTwo)
                //    {
                //        str += "\n";
                //        isLineTwo = true;
                //    }
                //}
                #endregion
                List<string> list = CommonTools.BalanceGetScoreDetailList(m_Data.detailList, m_Data.userSeat, m_GameType);
                for (int i = 0; i < list.Count; i++)
                {
                    str += string.Format("{0}{1}", list[i], "；");
                    isChangeShow = str.Length > numLi;
                }

                str = string.Format("[FED852]{0}[-]", str);
            }

            if (m_Data.detailListCut != null && m_Data.detailListCut.Count > 0)
            {
                List<string> list = CommonTools.BalanceGetScoreDetailList(m_Data.detailListCut, m_Data.userSeat, m_GameType);
                for (int i = 0; i < list.Count; i++)
                {
                    str2 += string.Format("{0}{1}", list[i], "；");
                    if (!isChangeShow)
                        isChangeShow = str2.Length > numLi;
                }

                str2 = string.Format("[B3C3D7]{0}[-]", str2);
            }*/
            #endregion
            isChangeShow = GetChangeShowLabel(m_Data.detailList, "[FED852]", isChangeShow, numLi, out str);
            isChangeShow = GetChangeShowLabel(m_Data.detailListCut, "[B3C3D7]", isChangeShow, numLi, out str2);

            if (scro.gameObject.activeSelf != isChangeShow)
                scro.gameObject.SetActive(isChangeShow);
            if (isChangeShow)
                scro.ResetPosition();

            if (labe1.gameObject.activeSelf == isChangeShow)
                labe1.gameObject.SetActive(!isChangeShow);

            string result = str;
            if (!string.IsNullOrEmpty(str2))
                result += string.Format("{0}{1}", string.IsNullOrEmpty(str) ? "" : "\n", str2);
            if (isChangeShow)
            {
                labe2.text = result;

                colider.size = new Vector3(labe2.localSize.x, 50, 0);
                colider.center = new Vector3(labe2.localSize.x / 2, 0, 0);
            }
            else
            {
                labe1.text = result;
            }
        }

        private bool GetChangeShowLabel(List<MjDetaildedScore> dataList, string color, bool isChangeShow, int numLimit, out string result)
        {
            result = "";
            if (dataList != null && dataList.Count > 0)
            {
                List<string> list = CardHelper.BalanceGetScoreDetailList(dataList, m_Data.userSeat, m_GameType);
                for (int i = 0; i < list.Count; i++)
                {
                    result += string.Format("{0}{1}", list[i], "；");
                    if (!isChangeShow)
                        isChangeShow = result.Length > numLimit;
                }

                result = string.Format("{0}{1}[-]", color, result);
            }
            return isChangeShow;
        }

        private Transform CreatItem(Transform parent, int id)
        {
            var obj = NGUITools.AddChild(parent.gameObject, m_MahjongItem.gameObject);
            UISprite sp = obj.transform.GetComponent<UISprite>();
            sp.alpha = 1;
            CardHelper.SetRecordUI(sp, id);
            return obj.transform;
        }

        private void ShowSpecialMj()
        {
            //条件判断是否显示特殊牌张
            if (m_HorseData == null || m_Grid_Special == null)
            {
                if (m_UILabel_SpecialMj != null)
                    m_UILabel_SpecialMj.text = "";
                return;
            }
            if (m_HorseData.cardIDList.Count < 1 || m_HorseData.cardIDList.Count != m_HorseData.horseStateList.Count)
            {
                if (m_UILabel_SpecialMj != null)
                    m_UILabel_SpecialMj.text = "";
                return;
            }

            for (int i = 0; i < m_HorseData.cardIDList.Count; i++)
            {
                GameObject obj = NGUITools.AddChild(m_Grid_Special.gameObject, m_MahjongItem2.gameObject);
                obj.SetActive(true);

                UISprite mjSp = obj.GetComponent<UISprite>();

                float al = 0;

                switch (m_HorseData.horseStateList[i])
                {
                    case (int)EnumMjBuyhorseStateType.BuyHorseNull:
                        al = 0.75f;
                        break;
                    case (int)EnumMjBuyhorseStateType.BuyHorseWin:
                        al = 1;
                        break;
                    case (int)EnumMjBuyhorseStateType.BuyHorseLose:
                        al = 0.5f;
                        break;
                }

                CardHelper.SetRecordUI(mjSp, m_HorseData.cardIDList[i], true, al);

                if(m_UILabel_SpecialMj != null)
                {
                    var seatData = m_HorseBaseData.buyHorseSiJiaData.GetBuyHorseSiJiaShowData(m_Data.userSeat);
                    if(seatData != null)
                    {
                        var winData = seatData.GetShowItemByType((int)EnumMjBuyhorseStateType.BuyHorseWin);
                        var loseData = seatData.GetShowItemByType((int)EnumMjBuyhorseStateType.BuyHorseLose);
                        string showStr = "";
                        int winNum = 0;
                        int winScore = 0;
                        int loseNum = 0;
                        int loseScore = 0;
                        if (winData != null && winData.cardAmount > 0)
                        {
                            winNum = winData.cardAmount;
                            winScore = winData.scoreAmount;
                        }
                        if(loseData != null && loseData.cardAmount > 0)
                        {
                            loseNum = loseData.cardAmount;
                            loseScore = loseData.scoreAmount;
                        }
                        showStr = string.Format("[FED852]{0}x{1}  {2}[-]", "买中赢家", winNum, winScore);
                        showStr = string.Format("{0}{1}", showStr, "\n");
                        showStr = string.Format("{0}[B3C3D7]{1}x{2}  {3}[-]", showStr, "买中输家", loseNum, loseScore);
                        m_UILabel_SpecialMj.text = showStr;
                    }

                    float xPos = 267;
                    //m_HorseData.cardIDList.Count
                    xPos += ((4 - m_HorseData.cardIDList.Count) * m_Grid_Special.cellWidth);
                    m_UILabel_SpecialMj.transform.localPosition = new Vector3(xPos, -45, 0);
                }

                #region 废弃
                //CardHelper.SetRecordUI(mjSp, m_HorseData.cardIDList[i], m_HorseData.horseStateList[i] == (int)EnumMjBuyhorseStateType.BuyHorseNull);
                UISprite mjBg = obj.transform.Find("HgihtLight").GetComponent<UISprite>();
                if (m_HorseData.horseStateList[i] == (int)EnumMjBuyhorseStateType.BuyHorseWin)
                {
                    mjBg.spriteName = "desk_bj_kaimahuangguang";
                    mjBg.gameObject.SetActive(true);
                }
                else if (m_HorseData.horseStateList[i] == (int)EnumMjBuyhorseStateType.BuyHorseLose)
                {
                    mjBg.spriteName = "desk_bj_kaimalanguang";
                    mjBg.gameObject.SetActive(true);
                }
                #endregion
            }
        }
        #endregion

        #region 显示玩家分数
        private void ShowMahjongPlayerScore()
        {
            switch (m_ShowType)
            {
                case e_UIMjBalanceShowType.Normal:
                case e_UIMjBalanceShowType.GuangDongZhuaMa:
                    playerHu.gameObject.SetActive(true);
                    if (m_Data.huList.Count != 0 && m_Data.huNum == 0)
                    {
                        playerHu.spriteName = "deskRecord_icon_hu";
                        playerHu.width = 56;
                        playerHu.height = 48;
                    }
                    else if (m_Data.huNum != 0)
                    {
                        playerHu.spriteName = "deskRecord_icon_zimo";
                        playerHu.width = 86;
                        playerHu.height = 46;
                    }
                    else if (m_Data.paoNum != 0 && m_Data.huNum == 0)
                    {
                        playerHu.spriteName = "deskRecord_icon_dp";
                        playerHu.width = 90;
                        playerHu.height = 44;
                    }
                    else
                    {
                        playerHu.gameObject.SetActive(false);
                    }
                    //playerHu.MakePixelPerfect();
                    if(m_ShowType == e_UIMjBalanceShowType.Normal)
                    {
                        playerGrid.transform.localPosition = new Vector3(541, -1.9f, 0);
                        playerHu.transform.localPosition = new Vector3(424, 0, 0);
                    }
                    else
                    {
                        playerGrid.transform.localPosition = new Vector3(597, -1.9f, 0);
                        playerHu.transform.localPosition = new Vector3(480, 0, 0);
                    }
                    break;
                default:
                    playerHu.gameObject.SetActive(false);
                    playerGrid.transform.localPosition = new Vector3(565.2f, -1.9f, 0);
                    playerHu.transform.localPosition = new Vector3(424, 0, 0);
                    break;
            }

            SetScore(playerGrid, m_Data.scoreCur);
        }

        string fuName = string.Empty;
        string numTitle = string.Empty;

        private void SetScore(UIGrid grid, int score)
        {
            grid.transform.DestroyChildren();

            fuName = string.Empty;
            numTitle = string.Empty;

            if (score >= 0)
            {
                fuName = "desk_icon_jiafenjian";
                numTitle = "desk_icon_jiafen{0}";
            }
            else
            {
                fuName = "desk_icon_jianfenjian";
                numTitle = "desk_icon_jianfen{0}";
            }

            string scoreName = score.ToString();
            if (score >= 0)
            {
                scoreName = "+" + scoreName;
            }
            char[] strChar = scoreName.ToCharArray();

            int length = strChar.Length;
            Vector2 numScale = new Vector2(44, 62);
            int gridWidth = 34;

            if(length == 5)
            {
                gridWidth = 26;
                numScale = new Vector2(36, 51);
            }
            else if(length == 6)
            {
                gridWidth = 21;
                numScale = new Vector2(31, 44);
            }

            if (strChar != null && length > 0)
            {
                for (int i = 0; i < length; i++)
                {
                    if (i == 0)
                    {
                        if (score < 0)
                        {
                            CreateNumber(grid, i, strChar[i], fuName, numScale);
                        }
                    }
                    else
                    {
                        CreateNumber(grid, i, strChar[i], numTitle, numScale);
                    }
                }
            }

            grid.cellWidth = gridWidth;
            grid.Reposition();
        }

        private void CreateNumber(UIGrid grid, int index, char charNum, string formatStr,Vector2 numSpScale)
        {
            GameObject obj = GameTools.InstantiatePrefab(m_FontSprite.gameObject, grid.transform, true, true);
            obj.name = index.ToString();
            UISprite sprite = obj.GetComponent<UISprite>();

            if (sprite != null)
            {
                string spriteName = string.Format(formatStr, charNum);
                sprite.spriteName = spriteName;
                //sprite.MakePixelPerfect();
                sprite.width = (int)numSpScale.x;
                sprite.height = (int)numSpScale.y;
            }
            obj.SetActive(true);
        }
        #endregion

        public void ChangeScrollView(int parentDepth)
        {
            if(playerScrow != null && playerScrow.gameObject.activeSelf)
            {
                UIPanel pan = playerScrow.GetComponent<UIPanel>();
                pan.depth = parentDepth + 1;
            }
        }
    }
}
