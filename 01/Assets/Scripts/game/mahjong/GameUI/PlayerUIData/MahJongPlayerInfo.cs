
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using gamelink;
/// <summary>
/// UI展示玩家信息
/// </summary>
namespace projectQ
{
    public class MahJongPlayerInfo : MonoBehaviour
    {
        public UILabel DuAnGangLabel;

        public GameObject dabianEndPos;

        private string headUrl;
        private string rememberHead;

        private Transform m_IconEffectTrans;
        Transform iconEffect
        {
            get
            {
                if (m_IconEffectTrans == null)
                    m_IconEffectTrans = this.transform.Find("EffectPos");
                return m_IconEffectTrans;
            }
        }

        [HideInInspector]
        public int seatId;
        [HideInInspector]
        public ulong userId;
        #region 播放语音时
        public UISprite sp1;//最小的（播放气泡）
        public UISprite sp2;//中间的（播放气泡）
        public UISprite sp3;//最大的（播放气泡）
        #endregion
        public GameObject playVoice;
        public UITexture texture_PlayerIcon;
        [Tooltip("准备中")]
        public GameObject SpritePrepare;

        public GameObject showMahjonPos;
        public GameObject showSiJiaMaiMaPos;
        [HideInInspector]
        public bool isBanker = false;

        public UILabel popText;
        public GameObject popObj;
        public UISprite emjoTexture;
        public TweenPosition dabianTween;

        public Animation emojAni;

        private bool isShowPlay;
        private enum mahjongPlayerState
        {
            Lose = -1,
            Normal = 0,
            WaitingHu = 1
        }

        public GameObject HeadItem;
        public UILabel bottomType;

        private MahjongShowHead m_MajHead;
        private MahjongShowHead majHead
        {
            get
            {
                CheckInitHead();
                return m_MajHead;
            }
            set
            {
                m_MajHead = value;
            }
        }

        private void Awake()
        {
            //UIEventListener.Get(obj_ClickIcon).onClick = OnIconClick;
            InitDuAnGang();
            //RecoreObj.gameObject.SetActive(false);
        }
        #region 语音相关
        private int m_PlayerSeatID = 0;

        /// <summary>
        /// 初始化录音
        /// </summary>
        public void InitRecord(long usingID, int playerSeatId)
        {
            //QLoger.LOG("playerInfo InitRecord");

            m_PlayerSeatID = playerSeatId;
            RecordManager.Instance.LoginRecord();//LoginVoice();
        }

        void OnPlayVoice(object[] values)
        {
            bool isplay = (bool)values[0];
            string user = (string)values[1];

            if (isplay && user == userId.ToString())
            {
                if (!playVoice.gameObject.activeSelf)
                    playVoice.gameObject.SetActive(true);
            }
            else
            {
                if (playVoice.gameObject.activeSelf)
                    playVoice.gameObject.SetActive(false);
            }
        }
        #endregion
        #region 游戏信息

        /// <summary>
        /// 头像点击
        /// </summary>
        /// <param name="go"></param>
        void OnIconClick(GameObject go)
        {
            object[] data = new object[2];
            data[0] = userId;
            data[1] = headUrl;
            _R.ui.OpenUI("UIUserInfo", data);
        }

        private void CheckInitHead()
        {
            if (m_MajHead == null)
            {
                GameObject obj = NGUITools.AddChild(this.gameObject, HeadItem);
                m_MajHead = obj.GetComponent<MahjongShowHead>();
            }
        }

        /// <summary>
        /// 初始化信息
        /// </summary>
        /// <param name="seatId"></param>
        /// <param name="userId"></param>
        /// <param name="score"></param>
        /// <param name="nickName"></param>
        public void IniPlayerInfo(int seatId, ulong userId, int score, string nickName)
        {
            majHead.ClearGameShow();
            majHead.ChangeScoreLabel(score);

            this.seatId = seatId;
            this.userId = userId;
            fromX = emjoTexture.transform.localPosition.x;
            fromY = emjoTexture.transform.localPosition.y;


            PlayerDataModel playerModel = MemoryData.PlayerData.get((long)userId);
            if (playerModel != null)
                OnChangeOnlineState(new object[] { playerModel.playerDataMj.seatID, playerModel.PlayerDataBase.IsOnline });

            EventDispatcher.AddEvent(GEnum.NamedEvent.EMjControlOnlineState, OnChangeOnlineState);

            EventDispatcher.AddEvent(GEnum.NamedEvent.SysUI_Record_ing, OnPlayVoice);

            ShowMahjong(seatId, true, false, new List<int>());
            ShowMahjongType(new List<string[,]>());
        }
        /// <summary>
        /// 刷新人物分数
        /// </summary>
        /// <param name="score"></param>
        public void RefreshScore(int score)
        {
            majHead.ChangeScoreLabel(score);
        }
        /// <summary>
        /// 设置icon
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="url"></param>
        public void SetIcon(ulong userId, string url)
        {
            majHead.SetIcon(url, userId);
        }

        //显示闹
        public void ShowNao(bool showAni)
        {
            majHead.ShowNao(showAni);
        }

        /// <summary>
        /// 显示缺的类型
        /// </summary>
        /// <param name="type"></param>
        public void ShowQueType(EnumMjHuaType type, bool showAni)
        {
            majHead.ShowLeftUpMark(type, showAni);
        }

        /// <summary>
        /// 清除所有信息
        /// </summary>
        public void ClearPlayerAllMessage()
        {
            ShowMahjongType(new List<string[,]>());
            majHead.LeaveRoom();
        }
        /// <summary>
        /// 改变人物分数
        /// </summary>
        /// <param name="curMoney"></param>
        public void ChangePlayerMoney(int curMoney)
        {
            majHead.ChangeScoreLabel(curMoney);
        }
        /// <summary>
        /// 庄
        /// </summary>
        /// <param name="isZhuang"></param>
        public void ShowZhuang(bool isZhuang, bool showAni)
        {
            if (isZhuang)
                majHead.ShowRightUpMark(showAni);
        }
        /// <summary>
        /// 显示明楼还是听牌
        /// </summary>
        /// <param name="isMingLou"></param>
        public void MingLou(string iconName, bool showAni)
        {
            majHead.ShowCenterUpMark(iconName, showAni);
        }

        /// <summary>
        /// 显示头像中上方图片
        /// </summary>
        /// <param name="type"></param>
        /// <param name="showAni"></param>
        public void ShowCenter(EnumMjSpecialCheck type, bool showAni)
        {
            majHead.ShowCenterUpMark(type, showAni);
        }

        /// <summary>
        /// 展示下炮/下炮的数量
        /// </summary>
        /// <param name="xiaPao"></param>
        /// <param name="num"></param>
        public void ShowPaoNum(EnumMjSpecialCheck paoEnum, int num, bool showAni, bool showRight = false, int seatID = 0)
        {
            if (showRight)
            {
                ShowRightPaoNum(paoEnum, num, seatID);
            }
            else
            {
                majHead.ShowPaoNum(paoEnum, num, showAni);
            }
        }

        public void ShowRightPaoNum(EnumMjSpecialCheck paoEnum, int num, int seatID = 0)
        {
            majHead.ShowRightPaoNum(paoEnum, num, bottomType.gameObject, seatID);
        }

        public void ShowDuanMen(int seatID, string spName)
        {
            majHead.ShowDuanMen(seatID, spName);
        }

        public void ShowCenterDown(int seatID, string spName, int showNum, bool isShowAni)
        {
            majHead.ShowCenterDownMark(seatID, spName, showNum, isShowAni);
        }

        private bool m_ShowPlayState = false;

        /// <summary>
        /// 显示出牌的动画
        /// </summary>
        public void ShowPlayAni()
        {
            majHead.ShowPlayAni(true);
        }
        /// <summary>
        /// 不显示出牌的动画
        /// </summary>
        public void HidePlayAni()
        {
            majHead.ShowPlayAni(false);
        }


        public void ShowIconEffect(GameObject effectObj)
        {
            if (effectObj != null)
            {
                effectObj.transform.SetParent(iconEffect, false);
                GameObjectHelper.NormalizationTransform(effectObj.transform);
                effectObj.SetActive(true);
            }
        }

        /// <summary>
        /// 房主
        /// </summary>
        public void ShowFangZhu()
        {

        }
        #endregion
        #region 聊天

        /// <summary>
        /// 显示文字聊天
        /// </summary>
        /// <param name="msg"></param>
        public void ShowChat(string msg)
        {
            emjoTexture.gameObject.SetActive(false);
            popObj.SetActive(true);
            popText.text = msg;

            Invoke("HideChatPop", 2f);

        }
        private bool isShowDaBian = false;
        private bool showOther = false;
        private string emojName;
        private string emojName2;
        private float fromX = 0f;
        private float fromY = 0f;
        private float toX = Screen.width * 0.5f;
        private float toY = Screen.height * 0.5f;
        /// <summary>
        /// 显示表情聊天
        /// </summary>
        /// <param name="msg"></param>
        public void ShowEmoj(string msg)
        {
            popObj.SetActive(false);
            if (msg == "icon_biaoqing_xihongshi")
            {
                //emjoTexture.gameObject.SetActive(true);
                //emjoTexture.spriteName = "icon_biaoqing_dabian1";
                if (!emojAni.gameObject.activeSelf)
                {
                    emojAni.gameObject.SetActive(true);
                    emojAni.wrapMode = WrapMode.ClampForever;
                    emojAni.Play();
                    StartCoroutine(OnPlayEnd());
                }
                //dabianTween.to.x = dabianEndPos.transform.localPosition.x;
                //dabianTween.to.y = dabianEndPos.transform.localPosition.y;
                //dabianTween.ResetToBeginning();
                //dabianTween.PlayForward();
                //MusicCtrl.Instance.MusicPlayUI()
            }
            else
            {
                emojName = msg + "1";
                emojName2 = msg + "2";
                emjoTexture.transform.localPosition = new Vector3(dabianTween.to.x, dabianTween.to.y, 0);
                emjoTexture.gameObject.SetActive(true);
                showOther = true;
            }

            GEnum.SoundEnum playType = GEnum.SoundEnum.desk_biaoqing_fanqie;
            switch (msg)
            {
                case "icon_biaoqing_xihongshi":
                    playType = GEnum.SoundEnum.desk_biaoqing_fanqie;
                    break;
                case "icon_biaoqing_hecha":
                    playType = GEnum.SoundEnum.desk_biaoqing_hecha;
                    break;
                case "icon_biaoqing_qiaozhuo":
                    playType = GEnum.SoundEnum.desk_biaoqing_qiaozhuo;
                    break;
                case "icon_biaoqing_zan":
                    playType = GEnum.SoundEnum.desk_biaoqing_dianzan;
                    break;
            }
            EventDispatcher.FireEvent(MJEnum.MusicEnum.ME_PlaySoundFX.ToString(), playType);
        }


        float timer = 0;

        private IEnumerator OnPlayEnd()
        {
            yield return new WaitForSeconds(1.3f);
            emojAni.gameObject.SetActive(false);
        }

        void HideEmoj()
        {
            emjoTexture.gameObject.SetActive(false);
        }
        void HideChatPop()
        {
            popObj.SetActive(false);
        }
        #endregion
        private float showPlayTimer;

        private void Update()
        {
            #region 播放语音气泡展示

            if (playVoice.gameObject.activeSelf)
            {
                showPlayTimer += Time.deltaTime;

                if (showPlayTimer < 0.9 && showPlayTimer > 0)
                {
                    sp1.gameObject.SetActive(showPlayTimer > 0);
                    sp2.gameObject.SetActive(showPlayTimer >= 0.3);
                    sp3.gameObject.SetActive(showPlayTimer >= 0.6);
                }
                else if (showPlayTimer >= 0.9 && showPlayTimer < 1)
                {
                    sp1.gameObject.SetActive(false);
                    sp2.gameObject.SetActive(false);
                    sp3.gameObject.SetActive(false);
                    showPlayTimer = 0f;
                }
            }
            else
            {
                sp1.gameObject.SetActive(false);
                sp2.gameObject.SetActive(false);
                sp3.gameObject.SetActive(false);
                showPlayTimer = 0f;

            }
            #endregion

            #region 表情动画
            if (showOther)
            {
                emjoTexture.transform.localPosition = new Vector3(fromX, fromY, 0);
                timer += Time.deltaTime;
                if (timer > 0 && timer < 0.5)
                {
                    emjoTexture.spriteName = emojName;
                }
                else if (timer >= 0.5 && timer < 1)
                {
                    emjoTexture.spriteName = emojName2;
                }
                else if (timer >= 1 && timer < 1.5)
                {
                    emjoTexture.spriteName = emojName;
                }
                else if (timer >= 1.5 && timer < 2)
                {
                    emjoTexture.spriteName = emojName2;
                }
                else
                {
                    emjoTexture.spriteName = "";
                    showOther = false;
                    emjoTexture.gameObject.SetActive(false);
                    timer = 0f;

                }

                if (emjoTexture != null && emjoTexture.gameObject.activeSelf)
                    emjoTexture.MakePixelPerfect();

            }
            #endregion
        }

        void OnChangeOnlineState(object[] values)
        {
            int seatID = (int)values[0];
            bool state = (bool)values[1];

            if (seatId == seatID)
            {
                if ((long)userId == MemoryData.UserID)
                    return;
                majHead.ChangeOnLineState(state);
            }
        }

        public Transform GetIconTransform()
        {
            return majHead.transform;
        }

        /// <summary>
        /// 设置准备中 的显示和隐藏
        /// </summary>
        /// <param name="isActive"></param>
        public void SetPrepareActive(bool isActive)
        {
            if (SpritePrepare == null)
                SpritePrepare = transform.Find("SpritePrepare").gameObject;
            SpritePrepare.SetActive(isActive);
        }
        private void OnDestroy()
        {
            EventDispatcher.RemoveEvent(GEnum.NamedEvent.EMjControlOnlineState, OnChangeOnlineState);
            EventDispatcher.RemoveEvent(GEnum.NamedEvent.SysUI_Record_ing, OnPlayVoice);
            _EventHelper.RemoveAllEvent();
        }




        public void ShowMahjong(int uiSeatID, bool isShow, bool isPlayAni, List<int> mahjongList)
        {
            majHead.ShowMahjong(showMahjonPos, uiSeatID, isShow, isPlayAni, mahjongList);
        }


        private void ClearShowMahjong()
        {
            majHead.ClearShowMahjong(showMahjonPos);
        }


        public void ShowMahjongType(List<string[,]> values)
        {
            if (bottomType == null)
                return;

            if (values.Count < 1)
            {
                bottomType.text = "";
                return;
            }

            string result = "";

            for (int i = 0; i < values.Count; i++)
            {
                string[,] str = values[i];
                result = string.Format("{0}{1} {2}", result, str[0, 0], str[0, 1]);

                if (i < values.Count - 1)
                    result += "\n";
            }

            bottomType.text = result;
        }


        #region 赌暗杠相关
        private EventDispatcheHelper _EventHelper = new EventDispatcheHelper();
        private void InitDuAnGang()
        {
            if (!NullHelper.IsObjectIsNull(DuAnGangLabel))
            {
                GameObjectHelper.SetEnable(DuAnGangLabel.gameObject, false);
            }
            _EventHelper.AddEvent(MJEnum.DuAnGangEvents.DAG_UIAlert.ToString(), AlertDuAnGang);
            _EventHelper.AddAllEvent();
        }
        private void AlertDuAnGang(object[] param)
        {
            if (NullHelper.IsObjectIsNull(param) || NullHelper.IsInvalidIndex(1, param))
            {
                return;
            }
            int seatID = (int)param[0];
            string msg = (string)param[1];
            if (!NullHelper.IsObjectIsNull(DuAnGangLabel))
            {
                if (seatID == seatId)
                {
                    if (string.IsNullOrEmpty(msg))
                    {
                        GameObjectHelper.SetEnable(DuAnGangLabel.gameObject, false);
                        return;
                    }
                    GameObjectHelper.SetEnable(DuAnGangLabel.gameObject, true);
                    DuAnGangLabel.text = msg;
                    if (msg == UIConstStringDefine.GiveUpDuAnGang)
                    {//放弃赌暗杠
                        this.StartCoroutine(HideDuAnGangLabel());
                    }
                }

            }

        }
        private IEnumerator HideDuAnGangLabel()
        {
            yield return new WaitForSeconds(ConstDefine.DAG_HideAlertInfoDelayTime);
            GameObjectHelper.SetEnable(DuAnGangLabel.gameObject, false);
        }
        #endregion

        #region  四家买马首次显示4张牌
        public void ShowSiJiaMaiMa(int uiSeatID, List<int> mjPaiList)
        {
            if (mjPaiList != null && mjPaiList.Count > 0)
            {
                showSiJiaMaiMaPos.SetActive(true);
                UISijiaMaiMaFirstInfo info = showSiJiaMaiMaPos.GetComponent<UISijiaMaiMaFirstInfo>();
                info.InitData(uiSeatID, mjPaiList);
            }
        }

       #endregion


    }
}
