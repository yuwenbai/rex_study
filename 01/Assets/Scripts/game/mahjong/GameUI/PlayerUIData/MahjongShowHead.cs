using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace projectQ
{
    public class MahjongShowHead : MonoBehaviour
    {
        #region 属性
        #region 标记相关
        //头像上的标记
        public GameObject MarkItem;

        #region 中下放标记显示
        private Transform m_CenterDown;
        private UISprite m_CDShowType;
        private UILabel m_CDShowNum;
        #endregion
        #endregion

        #region 玩家头像相关
        private Transform m_HeadBG;
        private UITexture m_PlayerIcon;
        private Animation m_HeadAni1;
        private Animation m_HeadAni2;
        //标记挂点
        private Transform m_RightUp;
        private Transform m_CenterUp;
        private Transform m_LeftUp;

        private string m_HeadUrl;
        private ulong m_UserID;
        private string m_RememberHead;
        #endregion

        private UILabel m_PlayerScore;

        #region 跑分相关
        private GameObject m_Pao;
        private UISprite m_PaoSprite;
        private UISprite m_PaoNum;
        #endregion

        private UISprite m_OnlineState;

        public UIDeskShowMahjong showMahjonObj;

        #region 断门
        private Transform m_DuanMen;
        private UISprite m_DuanMenType;
        #endregion

        #endregion

        #region 初始化
        private void Awake()
        {
            InitGameObject();
        }

        //初始化私有变量
        private void InitGameObject()
        {
            m_HeadBG = this.transform.Find("Head_Board");
            if (m_HeadBG != null)
            {
                m_PlayerIcon = m_HeadBG.Find("icon").GetComponent<UITexture>();
                m_OnlineState = m_HeadBG.Find("onlineState").GetComponent<UISprite>();
                m_HeadAni1 = m_HeadBG.GetComponent<Animation>();
            }

            m_HeadAni2 = this.transform.Find("eff_par_ani (4)").GetComponent<Animation>();

            m_RightUp = this.transform.Find("rightUp");
            m_CenterUp = this.transform.Find("centerUp");
            m_LeftUp = this.transform.Find("leftUp");

            m_PlayerScore = this.transform.Find("score/money_num").GetComponent<UILabel>();

            m_Pao = this.transform.Find("paoBg").gameObject;
            m_PaoSprite = m_Pao.transform.Find("pao").GetComponent<UISprite>();
            m_PaoNum = m_Pao.transform.Find("pao/num").GetComponent<UISprite>();

            m_RightPaoGrid = this.transform.Find("rightPaoGrid").GetComponent<UITable>();

            m_DuanMen = this.transform.Find("DuanMen");
            if(m_DuanMen != null)
            {
                m_DuanMenType = m_DuanMen.Find("Bg/Type").GetComponent<UISprite>();
            }

            m_CenterDown = this.transform.Find("CenterDown");
            if(m_CenterDown!= null)
            {
                m_CDShowType = m_CenterDown.Find("ShowType").GetComponent<UISprite>();
                m_CDShowNum = m_CenterDown.Find("ShowNumber").GetComponent<UILabel>();
            }

    }
        #endregion

        #region 显示标记
        #region 显示和创建标记
        /// <summary>
        /// 显示左上角标记：万筒条等
        /// </summary>
        /// <param name="type"></param>
        /// <param name="showAni"></param>
        public void ShowLeftUpMark(EnumMjHuaType type, bool showAni)
        {
            string spriteName = null;
            spriteName = MjDataManager.Instance.GetSpriteNameByType(type);
            CreatMark(m_LeftUp, spriteName, showAni);
        }

        /// <summary>
        /// 显示右上角标记：庄等
        /// </summary>
        /// <param name="showAni"></param>
        public void ShowRightUpMark(bool showAni)
        {
            CreatMark(m_RightUp, "desk_icon_zhuang", showAni);
        }

        /// <summary>
        /// 显示中上方标记：明楼听牌等
        /// </summary>
        /// <param name="isTing"></param>
        /// <param name="showAni"></param>
        public void ShowCenterUpMark(string spriteName, bool showAni)
        {
            CreatMark(m_CenterUp, spriteName, showAni);
        }


        public void ShowCenterUpMark(EnumMjSpecialCheck type, bool showAni)
        {
            string spriteName = null;
            spriteName = MjDataManager.Instance.GetSpriteNameByType(type);
            CreatMark(m_CenterUp, spriteName, showAni);
        }

        /// <summary>
        /// 显示闹（临时的，之后合并上去）
        /// </summary>
        /// <param name="showAni"></param>
        public void ShowNao(bool showAni)
        {
            CreatMark(m_LeftUp, "desk_icon_nao", showAni);
        }

        private void CreatMark(Transform parent, string spName, bool isShowAni)
        {
            int count = parent.childCount;
            GameObject creatObj = null;
            if (count < 1)
                creatObj = NGUITools.AddChild(parent.gameObject, MarkItem);
            else
                creatObj = parent.GetChild(0).gameObject;

            creatObj.SetActive(true);

            UISprite sprite = creatObj.GetComponent<UISprite>();
            sprite.spriteName = spName;
            sprite.MakePixelPerfect();

            Animation spAni = creatObj.GetComponent<Animation>();
            GameObject spEff = creatObj.transform.Find("eff_pao").gameObject;
            if (spAni == null)
            {
                return;
            }

            spAni.enabled = isShowAni;
            if (spEff != null)
                spEff.SetActive(isShowAni);
            if (isShowAni)
                EventDispatcher.FireEvent(GEnum.NamedEvent.EMjMusicSound, GEnum.SoundEnum.desk_dingque);
        }
        #endregion

        #region 清理标记
        private void ClearAllMark()
        {
            ClearMark(m_RightUp);
            ClearMark(m_CenterUp);
            ClearMark(m_LeftUp);
        }

        private void ClearMark(Transform clearTarget)
        {
            int count = clearTarget.childCount;
            for (int i = 0; i < count; i++)
            {
                GameObject obj = clearTarget.GetChild(i).gameObject;
                if (obj.activeSelf)
                    obj.SetActive(false);
                //Destroy(clearTarget.GetChild(0));
            }
        }
        #endregion
        #endregion

        #region 显示分数
        public void ChangeScoreLabel(int score)
        {
            m_PlayerScore.text = score.ToString();
        }
        #endregion

        #region 显示跑分
        public void ShowPaoNum(EnumMjSpecialCheck paoEnum, int num, bool showAni)
        {
            m_PaoNum.spriteName = string.Format("mj_icon_pao{0}", num);

            m_PaoSprite.spriteName = MjDataManager.Instance.GetSpriteNameByType(paoEnum);
            Animation paoAni = m_Pao.GetComponent<Animation>();
            GameObject paoEff = m_Pao.transform.Find("eff_pao").gameObject;
            if (paoAni == null)
            {
                return;
            }

            m_Pao.SetActive(true);
            paoAni.enabled = showAni;
            paoEff.SetActive(showAni);
        }
        #endregion

        #region 显示玩家头像
        public void SetIcon(string url, ulong userID)
        {
            if (m_HeadUrl != url)
            {
                m_HeadUrl = url;
                m_UserID = userID;

                string headname = DownHeadTexture.Instance.Texture_HeadNameSet(url);
                if (m_RememberHead == headname)
                {
                    return;
                }
                if (string.IsNullOrEmpty(headname))
                {
                    headname = "Texture_head_01";
                }

                m_RememberHead = headname;

                m_PlayerIcon.mainTexture = Resources.Load<Texture>(GameAssetCache.Texture_Hand_Path);
                DownHeadTexture.Instance.WeChat_HeadTextureGet(url, SetPlayerIcon);
            }

            UIEventListener.Get(m_HeadBG.gameObject).onClick = OnIconClick;
        }

        void OnIconClick(GameObject go)
        {
            object[] data = new object[2];
            data[0] = m_UserID;
            data[1] = m_HeadUrl;
            _R.ui.OpenUI("UIUserInfo", data);
        }

        void SetPlayerIcon(Texture2D HeadTexture, string headName)
        {
            QLoger.LOG(" 头像赋值 --  " + HeadTexture.name);

            if (string.IsNullOrEmpty(headName) || m_RememberHead != headName)
            {
                return;
            }
            m_PlayerIcon.mainTexture = HeadTexture;
        }


        /// <summary>
        /// 显示出牌的动画
        /// </summary>
        public void ShowPlayAni(bool isShow = true)
        {
            if (m_HeadAni1 != null)
            {
                m_HeadAni1.enabled = isShow;
                if (isShow)
                    m_HeadAni1.Play();
            }
            if (!isShow)
                m_HeadAni1.gameObject.GetComponent<UISprite>().alpha = 1;

            if(m_HeadAni2.gameObject.activeSelf != isShow)
            {
                m_HeadAni2.gameObject.SetActive(isShow);
                if (m_HeadAni2 != null)
                {
                    m_HeadAni2.enabled = isShow;
                    if (isShow)
                        m_HeadAni2.Play();
                }
            }
        }
        #endregion

        #region 显示离线状态
        public void ChangeOnLineState(bool isOnline)
        {
            if (isOnline && m_OnlineState.alpha > 0)
            {
                m_OnlineState.alpha = 0;
                m_PlayerIcon.color = Color.white;
            }
            else if (!isOnline && m_OnlineState.alpha <= 0)
            {
                m_OnlineState.alpha = 1;
                m_PlayerIcon.color = new Color(0.5f, 0.5f, 0.5f);
            }
        }
        #endregion

        #region 重置显示
        public void ClearGameShow()
        {
            ClearAllMark();
            m_Pao.gameObject.SetActive(false);
            GameObject paoEff = m_Pao.transform.Find("eff_pao").gameObject;
            paoEff.SetActive(false);
            m_PlayerScore.text = "";
            ShowPlayAni(false);
            ClearRightPaoNum();
            ShowDuanMen(0, "");
            ClearCenterDownMark();
            this.gameObject.SetActive(true);
        }

        public void LeaveRoom()
        {
            ClearGameShow();
            SetIcon("", 0);
            this.gameObject.SetActive(false);
        }
        #endregion

        #region 听牌麻将显示
        //显示听牌麻将
        public void ShowMahjong(GameObject showMahjonPos, int uiSeatID, bool isShow, bool isPlayAni, List<int> mahjongList)
        {
            if (showMahjonPos == null || showMahjonObj == null)
                return;

            ClearShowMahjong(showMahjonPos);

            if (mahjongList.Count == 0 || !isShow)
            {
                return;
            }

            GameObject creatObj = NGUITools.AddChild(showMahjonPos, showMahjonObj.gameObject);
            creatObj.transform.localPosition = Vector3.zero;
            UIDeskShowMahjong showData = creatObj.GetComponent<UIDeskShowMahjong>();

            showData.InitShow(uiSeatID, mahjongList);
        }

        //清楚听牌麻将
        public void ClearShowMahjong(GameObject showMahjonPos)
        {
            showMahjonPos.transform.DestroyChildren();
            //int count = showMahjonPos.transform.childCount;
            //for (int i = 0; i < count; i++)
            //{
            //    GameObject obj = showMahjonPos.transform.GetChild(0).gameObject;
            //    UIDeskShowMahjong deskdata = obj.GetComponent<UIDeskShowMahjong>();
            //    deskdata.StopAllCoroutines();
            //    DestroyImmediate(obj);
            //}
        }
        #endregion

        #region 右侧跑分显示
        private Dictionary<int, PaoNumData> m_ShowEnumList = new Dictionary<int, PaoNumData>();

        public GameObject rightPaoItem;
        private UITable m_RightPaoGrid;
        private float m_MaxWidth;
        private int m_ThisSeatID = -1;

        public void ShowRightPaoNum(EnumMjSpecialCheck paoEnum, int num, GameObject parent, int uiSeatID)
        {
            if (paoEnum == EnumMjSpecialCheck.Null)
            {
                ClearRightPaoNum();
                return;
            }

            m_ThisSeatID = uiSeatID;
            float width = 0;
            GameObject obj = null;
            if (m_ShowEnumList.ContainsKey((int)paoEnum))
            {
                PaoNumData data = m_ShowEnumList[(int)paoEnum];

                if (num > 0)
                {
                    data.SetNum(paoEnum, num);
                    obj = data.showObj;
                }
                else
                {
                    ClearRightPaoNum(paoEnum);
                }
            }
            else
            {
                if (rightPaoItem == null || m_RightPaoGrid == null)
                    return;

                if (num > 0)
                {
                    obj = NGUITools.AddChild(m_RightPaoGrid.gameObject, rightPaoItem);
                    PaoNumData data = new PaoNumData(obj);
                    data.SetNum(paoEnum, num);

                    m_ShowEnumList[(int)paoEnum] = data;

                    width = data.curWidth;
                    m_RightPaoGrid.transform.parent = parent.transform;
                }
            }

            if (m_MaxWidth < width)
                m_MaxWidth = width;

            RefreshRightPos();
        }

        public void ClearRightPaoNum(EnumMjSpecialCheck paoEnum = EnumMjSpecialCheck.Null)
        {
            if (m_ShowEnumList == null || m_ShowEnumList.Count == 0)
                return;

            if (paoEnum == EnumMjSpecialCheck.Null)
            {
                foreach (var item in m_ShowEnumList)
                {
                    if (item.Value.showObj != null)
                    {
                        Destroy(item.Value.showObj);
                    }
                }

                m_ShowEnumList.Clear();
                m_MaxWidth = -1;
            }
            else
            {
                if (m_ShowEnumList.ContainsKey((int)paoEnum))
                {
                    DestroyImmediate(m_ShowEnumList[(int)paoEnum].showObj);
                    m_ShowEnumList.Remove((int)paoEnum);

                    RefreshRightPos();
                }
            }
        }

        private void RefreshRightPos()
        {
            m_RightPaoGrid.Reposition();

            float xPos = m_MaxWidth / 2 + 20;
            switch (m_ThisSeatID)
            {
                case 0:
                case 3:
                    xPos *= 1;
                    break;
                case 1:
                case 2:
                    xPos *= -1;
                    break;
            }
            m_RightPaoGrid.transform.localPosition = new Vector3(xPos, 0, 0);
        }

        private class PaoNumData
        {
            public int curNum;
            public GameObject showObj;
            public EnumMjSpecialCheck curType;

            public float curWidth;

            private UISprite typeSp;
            private UISprite numSp;

            public PaoNumData(GameObject obj)
            {
                showObj = obj;
            }

            public void SetNum(EnumMjSpecialCheck type, int num)
            {
                curNum = num;
                curType = type;

                if (typeSp == null && showObj != null)
                {
                    typeSp = showObj.transform.Find("pao").GetComponent<UISprite>();
                    numSp = typeSp.transform.Find("num").GetComponent<UISprite>();
                }

                numSp.spriteName = string.Format("mj_icon_pao{0}", num);
                typeSp.spriteName = MjDataManager.Instance.GetSpriteNameByType(type);

                numSp.MakePixelPerfect();
                typeSp.MakePixelPerfect();

                curWidth = numSp.width + typeSp.width + 24;
            }
        }

        #endregion

        #region 断门显示
        public void ShowDuanMen(int uiSeatID, string spName)
        {
            if (m_DuanMen == null)
                return;

            bool isShow = !string.IsNullOrEmpty(spName);
            if (m_DuanMen.gameObject.activeSelf != isShow)
                m_DuanMen.gameObject.SetActive(isShow);

            if (!isShow)
            {
                return;
            }
            else
            {
                switch (uiSeatID)
                {
                    case 0:
                    case 3:
                        m_DuanMen.transform.localPosition = new Vector3(50, -15, 0);
                        break;
                    default:
                        m_DuanMen.transform.localPosition = new Vector3(-300, -15, 0);
                        break;
                }
            }

            m_DuanMenType.spriteName = spName;
        }
        #endregion

        #region 下方标记
        public void ShowCenterDownMark(int seatID, string typeSp, int showNum,bool isShowAni)
        {
            if (m_CenterDown == null)
                return;

            if (string.IsNullOrEmpty(typeSp))
            {
                ClearCenterDownMark();
                return;
            }

            if (m_CenterDown.gameObject.activeSelf != true)
                m_CenterDown.gameObject.SetActive(true);

            m_CDShowType.spriteName = typeSp;
            m_CDShowType.MakePixelPerfect();

            m_CDShowNum.text = showNum.ToString();
        }

        private void ClearCenterDownMark()
        {
            if (m_CenterDown == null)
                return;

            if (m_CenterDown.gameObject.activeSelf != false)
                m_CenterDown.gameObject.SetActive(false);
        }
        #endregion

    }
}
