/**
* 作者：周腾
* 作用：
* 日期：2017.4.17
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Msg;
namespace projectQ
{
    public class UIDesktopFunction : MonoBehaviour//: UIViewBase
    {
        public GameObject obj_chatBtn;
        public GameObject obj_backBtn;
        public GameObject obj_ruleBtn;
        public GameObject obj_settingBtn;
        public GameObject obj_voiceBtn;
        public GameObject obj_historyBtn;
        public GameObject obj_tingBtn;
        public GameObject obj_ChangeSort;           //改变手牌排序

        public GameObject obj_VoiceObj;
        public UISprite sp_voice1;
        public UISprite sp_voice2;
        public UISprite sp_voice3;
        public UISprite sp_voice4;
        public UISprite sp_voice5;

        private bool m_IsWaitTime;

        public MahjongPositionPlayerInfo mahjongPositionPlayerInfo;
        private int isUp;
        private int totalCardNum;
        private int bupaiNum;
        private bool m_IsHaveMusic;
        #region 混牌GameObject
        public UISprite hunPaiIcon;
        public GameObject obj_hunPai;
        public GameObject hunLight1;
        public GameObject hunLight2;
        public TweenAlpha hunTweenAlpha;
        public UILabel hunPaiLabel;
        private bool showHunPai;
        private bool isFirstChangeHun = false;
        private List<int> hunPaiList;
        #endregion
        #region 剩余张数GameObject
        public UILabel remaindCardNum;
        private UILabel removeNum;
        #endregion
        #region 局数GameObject
        public UILabel remaindRoundNum;
        #endregion
        #region 补牌GameObject
        public GameObject obj_BuPai;
        public UILabel label_BuPaiNum;
        public GameObject bupaiEff;
        #endregion
        #region 四家买马GameObject
        public GameObject[] sjmmObj;
        public GameObject[] sjmmGrid;
        #endregion

        public GameObject StageType;
        private UILabel m_StageTypeLb;

        private bool m_IsPlaying;
        private bool m_IsRecord;
        public UISprite sign;
        #region Self Function

        /// <summary>
        /// 返回
        /// </summary>
        /// <param name="obj"></param>
        void OnBackBtnClick(GameObject obj)
        {

            WindowUIManager.Instance.CreateOrAddWindow(WindowUIType.SystemPopupWindow,
                "提示", "是否发起解散牌桌请求？", new string[] { "取消", "确认" }, OnClickConfirmClose);
        }
        /// <summary>
        /// 补牌
        /// </summary>
        /// <param name="obj"></param>
        void OnBuPaiClick(GameObject obj)
        {
            EventDispatcher.FireEvent(GEnum.NamedEvent.EMjControlClickBuBtn);
            //ui.LoadUIMain("UISupplementCard");
        }
        /// <summary>
        /// 返回回调
        /// </summary>
        /// <param name="index"></param>
        private void OnClickConfirmClose(int index)
        {
            if (index == 1)
            {
                UIChatManager.isOpenPage = false;
                object[] data = new object[2];
                data[0] = UIChatManager.isOpenPage;
                data[1] = mahjongPositionPlayerInfo;
                ui.LoadUIMain("UIChat", data);
                //确定
                EventDispatcher.FireEvent(GEnum.NamedEvent.EMjControlSendClose);
            }
            else
            {
                EventDispatcher.FireEvent(GEnum.NamedEvent.EMahjongContinue, false);
            }
        }

        /// <summary>
        /// 规则
        /// </summary>
        /// <param name="obj"></param>
        void OnRuleBtnClick(GameObject obj)
        {
            int deskID = MjDataManager.Instance.MjData.curUserData.selfDeskID;
            MjDeskInfo deskInfo = MemoryData.DeskData.GetOneDeskInfo(deskID);
            ui.LoadUIMain("UICreateRoomShow", deskInfo);
            //ui.LoadUIMain("UIDeskRule");
        }
        /// <summary>
        /// 设置
        /// </summary>
        /// <param name="obj"></param>
        void OnSettingBtnClick(GameObject obj)
        {
            ui.LoadUIMain("UIOption");
        }

        /// <summary>
        /// 战绩
        /// </summary>
        /// <param name="obj"></param>
        void OnHistoryBtnClick(GameObject obj)
        {
            UIButton button = obj_historyBtn.GetComponent<UIButton>();
            if (button.isEnabled)
            {
                ui.LoadUIMain("UIMahjongResult", MjDataManager.Instance.MjData.curUserData.selfDeskID, 2, true, false);
            }
        }
        /// <summary>
        /// 聊天
        /// </summary>
        /// <param name="obj"></param>
        void OnChatBtnClick(GameObject obj)
        {
            //uichat
            UIChatManager.isOpenPage = !UIChatManager.isOpenPage;
            object[] data = new object[2];
            data[0] = UIChatManager.isOpenPage;
            data[1] = mahjongPositionPlayerInfo;
            ui.LoadUIMain("UIChat", data);

        }
        /// <summary>
        /// 上滑监听事件
        /// </summary>
        void OnVoiceUpSlider()
        {
            this.isUp = 1;
        }

        private UIButton m_TingBtn;

        /// <summary>
        /// 点击听提示
        /// </summary>
        /// <param name="obj"></param>
        void OnTingBtnClick(GameObject obj)
        {
            if (m_TingBtn == null)
                m_TingBtn = obj_tingBtn.GetComponent<UIButton>();
            if (m_TingBtn.isEnabled)
                EventDispatcher.FireEvent(GEnum.NamedEvent.EMjControlClickTingTip);
        }


        /// <summary>
        /// 点击改变手牌排序 
        /// </summary>
        /// <param name="obj"></param>
        private void OnChangeBtnClick(GameObject obj)
        {
            MusicCtrl.Instance.Music_SoundPlay(GEnum.SoundEnum.desk_qiepai);
            EventDispatcher.FireEvent(GEnum.NamedEvent.EMjControlClickChangeSort);
        }
        #endregion


        public void InitPlayerUI()
        {
            UIEventListener.Get(obj_chatBtn).onClick = OnChatBtnClick;
            UIEventListener.Get(obj_backBtn).onClick = OnBackBtnClick;
            UIEventListener.Get(obj_ruleBtn).onClick = OnRuleBtnClick;
            UIEventListener.Get(obj_settingBtn).onClick = OnSettingBtnClick;
            UIEventListener.Get(obj_voiceBtn).onPress = OnVoicePress;
            UIEventListener.Get(obj_historyBtn).onClick = OnHistoryBtnClick;
            UIEventListener.Get(obj_tingBtn).onClick = OnTingBtnClick;
            UIEventListener.Get(obj_ChangeSort).onClick = OnChangeBtnClick;
            UIEventListener.Get(obj_BuPai).onClick = OnBuPaiClick;
            //obj_voiceBtn.GetComponent<CheckIsStopRecord>().Upslider = this.OnVoiceUpSlider;
            InitData();
        }

        /// <summary>
        /// 切换摆牌
        /// </summary>
        /// <param name="showOrHide"></param>
        public void SetSortHandState(bool showOrHide)
        {
            UIButton button = obj_ChangeSort.GetComponent<UIButton>();
            if (button != null)
            {
                button.isEnabled = showOrHide;
            }

        }

        public void SetCheckTingState(bool state)
        {
            UIButton button = obj_tingBtn.GetComponent<UIButton>();
            if (button != null)
            {
                button.isEnabled = state;
            }
        }

        public void SetCheckHistoryState(bool state)
        {
            UIButton button = obj_historyBtn.GetComponent<UIButton>();
            if (button != null)
            {
                button.isEnabled = state;
            }
        }

        private bool m_IsPress;
        private Vector3 m_InputPostion;

        /// <summary>
        /// 语音按钮按下，抬起监听
        /// </summary>
        /// <param name="go"></param>
        /// <param name="isPress"></param>
        void OnVoicePress(GameObject go, bool isPress)
        {
            if (m_IsWaitTime)
                return;

            if (isPress)
            {
                m_IsPress = true;
                //m_IsHaveMusic = MusicCtrl.Instance.musicMp3.volume > 0;
                //ChangeVoiceVolume(false);
                //RecordManager.Instance.LoginRecord();//.LoginVoice();
                StartRecord();
                m_InputPostion = Input.mousePosition;
            }
            else
            {
                if (!m_IsPress)
                    return;

                m_IsPress = false;
                //ChangeVoiceVolume(true);// m_IsHaveMusic);

                Vector3 curPos = Input.mousePosition;
                float cha = curPos.y - m_InputPostion.y;
                if (cha > 80)
                {
                    isUp = 1;
                    Debug.LogError("赏花");
                }

                if (isUp == 1)
                {
                    CancleRecord();
                    isUp = 0;
                    return;
                }
                else
                {
                    StopRecord();
                }

                m_IsWaitTime = true;
                StartCoroutine(WaitTime());
            }
        }

        IEnumerator WaitTime()
        {
            yield return new WaitForSeconds(1f);
            m_IsWaitTime = false;
        }

        private bool m_IsInitOver = false;

        /// <summary>
        /// 刷新数据
        /// </summary>
        public void RefreshData()
        {
            obj_hunPai.SetActive(false);
            obj_BuPai.SetActive(false);
            bupaiEff.SetActive(false);
            ShowTotalCard(0);
            InitData();
            ShowRemaindRound();
            SetScore();

            SetDeleget();
            ShowStage(false, false, "");
            ShowRemoveTotalCard(0);
            ClearFirstPai();
        }
        void InitData()
        {

            MjPlayerInfo[] infos = MjDataManager.Instance.GetAllPlayerInfoByDeskID(MjDataManager.Instance.MjData.curUserData.selfDeskID);
            long[] ids = MjDataManager.Instance.GetAllPlayerIDByDeskID(MjDataManager.Instance.MjData.curUserData.selfDeskID);
            mahjongPositionPlayerInfo.InitPlayerInfo(ids, MjDataManager.Instance.MjData.curUserData.selfDeskID);

            SetSortHandState(false);
            SetCheckTingState(false);
        }

        UIPrepareGame ui = null;
        public void Init(UIPrepareGame UI)
        {
            ui = UI;
            InitPlayerUI();
        }
        /// <summary>
        /// 总牌张数
        /// </summary>
        /// <param name="number">剩余牌数量</param>
        public void ShowTotalCard(int number)
        {
            totalCardNum = number;
            if (number == 0)
            {
                remaindCardNum.text = "";
            }
            else
            {
                if (totalCardNum < 0)
                    totalCardNum = 0;
                remaindCardNum.text = totalCardNum.ToString();
            }
        }
        /// <summary>
        /// 加，减牌
        /// </summary>
        /// <param name="number"></param>
        public void ShowCutTotalCard(int number)
        {
            totalCardNum += number;
            if (totalCardNum < 0)
                totalCardNum = 0;
            if (remaindCardNum != null)
                remaindCardNum.text = (totalCardNum).ToString();
        }

        /// <summary>
        /// 显示当前剩余后面的减少数量
        /// </summary>
        /// <param name="number"></param>
        public void ShowRemoveTotalCard(int number)
        {
            if (removeNum == null)
            {
                removeNum = remaindCardNum.transform.GetChild(0).GetComponent<UILabel>();
            }

            if (removeNum == null)
                return;

            if (number < 1)
            {
                removeNum.text = "";
                return;
            }

            removeNum.text = string.Format("(-{0})", number);
        }

        /// <summary>
        /// 局数
        /// </summary>
        /// <param name="showAin">是否播放动画，默认播放</param>
        public void ShowRemaindRound()
        {
            if (MjDataManager.Instance.MjData.curUserData.selfDeskID == 0)
            {
                return;
            }
            MjDeskInfo deskInfo = MemoryData.DeskData.GetOneDeskInfo(MjDataManager.Instance.MjData.curUserData.selfDeskID);
            remaindRoundNum.text = deskInfo.bouts + "/" + deskInfo.rounds + "局";
        }
        /// <summary>
        /// 混牌
        /// </summary>
        /// <param name="cardID">牌ID</param>
        /// <param name="isHunPai">是不是混牌,true混牌</param>
        /// <param name="showAni">是否播放动画，默认播放</param>
        public void ShowHunPai(List<int> cardID, string showName, bool showAni = true)
        {
            obj_hunPai.SetActive(true);
            Animation hunAni = obj_hunPai.GetComponent<Animation>();
            if (showAni)
            {
                hunLight1.SetActive(true);
                hunLight2.SetActive(true);
                hunAni.enabled = true;
                hunAni.Play();

                EventDispatcher.FireEvent(MJEnum.MusicEnum.ME_PlaySoundFX.ToString(), GEnum.SoundEnum.desk_hunhou);
            }
            else
            {
                hunLight1.SetActive(false);
                hunLight2.SetActive(false);
                hunAni.enabled = false;

            }
            hunPaiLabel.text = showName;
            CardHelper.SetRecordUI(hunPaiIcon, cardID[0]);
            if (cardID.Count > 1)
            {
                hunPaiList = cardID;
                showHunPai = true;
                isFirstChangeHun = true;
            }
        }

        /// <summary>
        /// 更新人数分数
        /// </summary>
        /// <param name="uiChairID"></param>
        /// <param name="score"></param>
        public void RefreshPlayerScore(int uiChairID, int score)
        {
            mahjongPositionPlayerInfo.ChangeScore(uiChairID, score);
        }
        /// <summary>
        /// 定庄
        /// </summary>
        /// <param name="uiChairID"></param>
        public void SwitchZhuang(int uiChairID, bool showAni = true)
        {
            mahjongPositionPlayerInfo.ShowZhuang(uiChairID, showAni);
        }
        /// <summary>
        /// 缺的结果
        /// </summary>
        /// <param name="uiChairID"></param>
        /// <param name="queType"></param>
        /// <param name="showAni"></param>
        public void ShowQueType(int uiChairID, int queType, bool showAni = true)
        {
            mahjongPositionPlayerInfo.ShowQue(uiChairID, queType, showAni);
            //测试
            //ShowTing(uiChairID, true, showAni);
        }

        public void ShowNao(int uiChairID, bool showAni = true)
        {
            mahjongPositionPlayerInfo.ShowNao(uiChairID, showAni);
        }

        /// <summary>
        /// 设置人物积分
        /// </summary>
        public void SetScore()
        {
            mahjongPositionPlayerInfo.SetScore();
        }

        /// <summary>
        /// 显示下跑还是下炮
        /// </summary>
        /// <param name="uiChairID"></param>
        /// <param name="xiaPao"></param>
        /// <param name="showAni"></param>
        public void XiaPaoXiaPao(int uiChairID, EnumMjSpecialCheck paoEnum, int paoNum, bool showAni = true, bool showRight = false)
        {
            mahjongPositionPlayerInfo.ShowPaoNum(uiChairID, paoEnum, paoNum, showAni, showRight);
        }

        /// <summary>
        /// 显示断门
        /// </summary>
        /// <param name="uiChairID"></param>
        /// <param name="spName"></param>
        public void ShowDuanMen(int uiChairID, string spName)
        {
            mahjongPositionPlayerInfo.ShowDuanMen(uiChairID, spName);
        }


        /// <summary>
        /// 显示中下方补牌标记
        /// </summary>
        public void ShowCenterDown(int uiChairID, string spName, int showNum, bool isShowAni)
        {
            mahjongPositionPlayerInfo.ShowCenterDown(uiChairID, spName, showNum, isShowAni);
        }

        /// <summary>
        /// 在玩家的头像上显示特效
        /// </summary>
        /// <param name="mjType"></param>
        /// <param name="uiSeatID"></param>
        /// <param name="autoDesTime"></param>
        public void ShowEffectToIcon(EnumMjOpenMaType mjType, int uiSeatID, float autoDesTime = 1.0f)
        {
            mahjongPositionPlayerInfo.ShowEffectBySeatID(mjType, uiSeatID, autoDesTime);
        }

        public Transform GetTransformOfSeatID(int uiSeatID)
        {
            return mahjongPositionPlayerInfo.GetTransformOfSeatID(uiSeatID);
        }

        /// <summary>
        /// 听牌
        /// </summary>
        /// <param name="uiChairID"></param>
        /// <param name="isTing"></param>
        /// <param name="showAni"></param>
        public void ShowTing(int uiChairID, string iconName, bool showAni = true)
        {
            mahjongPositionPlayerInfo.ShowTingPai(uiChairID, iconName, showAni);
        }

        /// <summary>
        /// 显示头像中上方标记
        /// </summary>
        /// <param name="uiChairID"></param>
        /// <param name="type"></param>
        /// <param name="showAni"></param>
        public void ShowCenterUp(int uiChairID, EnumMjSpecialCheck type, bool showAni = true)
        {
            mahjongPositionPlayerInfo.ShowCenterUp(uiChairID, type, showAni);
        }

        private float hunPaiTimer = 0f;
        private int index = 0;
        private bool changeCard = false;
        private void Update()
        {
            if (showHunPai)
            {
                hunPaiTimer += Time.deltaTime;
                if (isFirstChangeHun)
                {
                    if (hunPaiTimer >= 4)
                    {
                        PlayTween();
                        hunPaiTimer = 0;
                        changeCard = true;
                        isFirstChangeHun = false;
                    }
                }
                else
                {
                    if (hunPaiTimer >= 5.5)
                    {
                        PlayTween();
                        hunPaiTimer = 0;
                        changeCard = true;
                    }
                }

            }
        }
        void PlayTween()
        {
            //hunTweenAlpha.ResetToBeginning();
            hunTweenAlpha.PlayForward();
            return;

        }
        public void HunChangeOver()
        {
            hunTweenAlpha.PlayReverse();

        }
        public void ChangeCard()
        {
            if (changeCard)
            {
                index++;
                if (index == hunPaiList.Count)
                {
                    index = 0;
                }
                CardHelper.SetRecordUI(hunPaiIcon, hunPaiList[index]);
                changeCard = false;
            }

            return;
        }
        /// <summary>
        /// 显示补牌
        /// </summary>
        /// <param name="paiNum"></param>
        /// <param name="showAni"></param>
        public void ShowBuPai(int paiNum, bool showAni = true)
        {

            Animation bupaiAni = obj_BuPai.GetComponent<Animation>();
            if (showAni)
            {

                //1,两套，从无到有，显示Obj
                //2,刷新
                if (obj_BuPai.activeSelf)
                {
                    obj_BuPai.SetActive(true);
                    bupaiEff.SetActive(true);
                    bupaiEff.GetComponent<ParticleSystem>().Stop();
                    bupaiEff.GetComponent<ParticleSystem>().Play();
                    bupaiNum = paiNum;
                    label_BuPaiNum.text = bupaiNum.ToString();
                }
                else
                {
                    obj_BuPai.SetActive(true);
                    if (bupaiAni != null)
                    {
                        bupaiAni.enabled = true;
                        bupaiAni.Play();
                        //if (!bupaiAni.isPlaying)
                        //{
                        bupaiEff.SetActive(true);
                        bupaiEff.GetComponent<ParticleSystem>().Stop();
                        bupaiEff.GetComponent<ParticleSystem>().Play();
                        //bupaiNum = paiNum;
                        bupaiNum = paiNum;
                        label_BuPaiNum.text = paiNum.ToString();
                        //}
                    }
                }
            }
            else
            {
                obj_BuPai.SetActive(true);
                bupaiAni.enabled = false;
                if (obj_BuPai.activeSelf)
                {
                    bupaiEff.SetActive(true);
                    bupaiEff.GetComponent<ParticleSystem>().Stop();
                    bupaiEff.GetComponent<ParticleSystem>().Play();
                    bupaiNum = paiNum;
                    label_BuPaiNum.text = bupaiNum.ToString();
                }
                else
                {
                    obj_BuPai.SetActive(true);
                    bupaiEff.SetActive(false);
                    //bupaiNum = bupaiNum + paiNum;
                    bupaiNum = paiNum;
                    label_BuPaiNum.text = bupaiNum.ToString();
                }

            }

        }
        /// <summary>
        /// 增加补牌的数量
        /// </summary>
        /// <param name="paiNum"></param>
        /// <param name="showAni"></param>
        public void AddBuPai(int paiNum, bool showAni = true)
        {
            bupaiEff.GetComponent<ParticleSystem>().Stop();
            bupaiEff.GetComponent<ParticleSystem>().Play();
            bupaiNum = bupaiNum + paiNum;
            label_BuPaiNum.text = bupaiNum.ToString();
        }
        /// <summary>
        /// 正在出牌的玩家
        /// </summary>
        /// <param name="uiSeatID"></param>
        /// <param name="showAni"></param>
        public void IsPlayIng(int uiSeatID, bool showAni = true)
        {
            mahjongPositionPlayerInfo.IsPlaying(uiSeatID, showAni);
        }


        /// <summary>
        /// 显示麻将牌
        /// </summary>
        public void ShowMahjong(int uiSeatID, bool isShow, bool isPlayAni, List<int> mahjongList)
        {
            mahjongPositionPlayerInfo.ShowMahjong(uiSeatID, isShow, isPlayAni, mahjongList);
        }

        /// <summary>
        /// 显示下飘标记文字
        /// </summary>
        /// <param name="uiSeatID"></param>
        /// <param name="values"></param>
        public void ShowMahjongType(int uiSeatID, List<string[,]> values)
        {
            mahjongPositionPlayerInfo.ShowMahjongType(uiSeatID, values);
        }
        /// <summary>
        /// 四家买马首次显示4张牌
        /// </summary>
        public void ShowSiJiaMaiMaFirstPai(int uiSeatID, List<int> mjPaiList)
        {
            mahjongPositionPlayerInfo.ShowSiJiaMaiMa(uiSeatID, mjPaiList);
        }
        /// <summary>
        /// 四家买马数据刷新清空
        /// </summary>
        /// <param name="uiSeatID"></param>
        /// <param name="mjPaiList"></param>
        private void ClearFirstPai()
        {
            for (int i = 0; i < 4; i++)
            {
                if (sjmmGrid != null && sjmmGrid [i]!= null)
                {
                    sjmmGrid[i].transform.DestroyChildren();
                }
                if(sjmmObj!=null&&sjmmObj[i]!=null)
                {
                    sjmmObj[i].SetActive(false);
                }
            }
        }
        #region 风圈标识显示
        public void ShowFengQuanIdentifyling(int identify, int deskID)
        {
            int curdeskID = MjDataManager.Instance.MjData.curUserData.selfDeskID;
            if (identify < 0)
            {
                sign.gameObject.SetActive(false);
            }
            else if (identify > 0)
            {
                sign.gameObject.SetActive(true);
            }
            if (deskID == curdeskID)
            {
                SetFengQuanIdentifyName(identify);
            }
        }
        private void SetFengQuanIdentifyName(int identify)
        {

            if (identify == 1)
            {
                sign.spriteName = "desk_icon_dong";
            }
            else if (identify == 2)
            {
                sign.spriteName = "desk_icon_nan";
            }
            else if (identify == 3)
            {
                sign.spriteName = "desk_icon_xi";
            }
            else
            {
                sign.spriteName = "desk_icon_bei";
            }
        }
        #endregion

        #region  风圈标识刷新
        public void RefreshFengQuanIdentifyling()
        {
            if (sign != null)
                sign.gameObject.SetActive(false);
        }
        #endregion

        #region 语音相关
        private bool m_WaitLeave;

        void SetDeleget()
        {
            m_IsInitOver = true;
            SetRecordDelegate();
            //RecordManager.Instance.InitGameLinkSDK();
        }

        /// <summary>
        /// 设置语音代理
        /// </summary>
        public void SetRecordDelegate()
        {
            if (MjDataManager.Instance.MjData.curUserData.selfDeskID == 0)
                return;

            QLoger.LOG("playerInfo SetRecordDelegate");

            RecordManager.Instance.setDelegate(IsRecoding, IsPlaying, RecoringVoiceChange, RecordLengthErr, RecoringErr);
            RecordManager.Instance.SetUserInfo(MemoryData.UserID.ToString(), MjDataManager.Instance.MjData.curUserData.selfDeskID.ToString());//setInfo

            EventDispatcher.AddEvent(GEnum.NamedEvent.GameStateChange, OnGameDataChange);
        }

        private void OnGameDataChange(object[] value)
        {
            if (RecordManager.Instance != null && MemoryData.GameStateData.IsPause)
            {
                RecordManager.Instance.LogoutRecord();
            }
        }

        /// <summary>
        /// 开始录音
        /// </summary>
        public void StartRecord()
        {
            RecordManager.Instance.StartRecord();//record();
            EventDispatcher.FireSysEvent(GEnum.NamedEvent.SysUI_Record_InitBegin);
        }

        /// <summary>
        /// 停止录音
        /// </summary>
        public void StopRecord()
        {
            RecordManager.Instance.StopRecord();//record();
            //EventDispatcher.FireSysEvent(GEnum.NamedEvent.SysUI_Record_InitBegin);
        }

        /// <summary>
        /// 取消录音
        /// </summary>
        public void CancleRecord()
        {
            //QLoger.LOG("playerInfo CancleRecord");

            RecordManager.Instance.StopRecord(true);//record(false);
            obj_VoiceObj.SetActive(false);
            EventDispatcher.FireEvent(GEnum.NamedEvent.SysUI_Record_Err, "取消发送语音");
        }

        /// <summary>
        /// 是否正在录音
        /// </summary>
        /// <param name="isPlaying"></param>
        void IsRecoding(bool isRecoding)
        {
            m_IsRecord = isRecoding;
            if (isRecoding)
            {
                obj_VoiceObj.SetActive(true);
            }
            else
            {
                obj_VoiceObj.SetActive(false);
            }
            ChangeVoiceVolume();
        }

        void IsPlaying(string userID, bool isPlaying)
        {
            m_IsPlaying = isPlaying;
            EventDispatcher.FireEvent(GEnum.NamedEvent.SysUI_Record_ing, isPlaying, userID);
            ChangeVoiceVolume();
        }

        private void ChangeVoiceVolume()
        {
            MusicCtrl.Instance.Music_BackVolumeChange(!m_IsPlaying && !m_IsRecord);
            MusicCtrl.Instance.Music_SoundVolumeChange(!m_IsPlaying && !m_IsRecord);
        }

        /// <summary>
        /// 音量大小和时长
        /// </summary>
        /// <param name="f"></param>
        /// <param name="i"></param>
        void RecoringVoiceChange(float volume, uint duration)
        {
            sp_voice1.alpha = volume > 0 ? 1 : 0;
            sp_voice2.alpha = volume > 0.2 ? 1 : 0;
            sp_voice3.alpha = volume > 0.4 ? 1 : 0;
            sp_voice4.alpha = volume > 0.6 ? 1 : 0;
            sp_voice5.alpha = volume > 0.8 ? 1 : 0;
        }

        /// <summary>
        /// 错误信息
        /// </summary>
        /// <param name="err"></param>
        void RecoringErr(string err)
        {
            QLoger.LOG("playerInfo RecoringErr = " + err);
            //EventDispatcher.FireEvent(GEnum.NamedEvent.SysUI_Record_Err, err);
            obj_VoiceObj.SetActive(false);

            switch (err)
            {
                case "MicphoneAccessFailed":
                case "MicphoneBusy":
                    RecordManager.Instance.StopRecord(true);
                    return;
                case "NetworkError":
                    //RecordManager.Instance.ForceEnd();
                    m_IsWaitTime = true;
                    m_IsPress = false;
                    RecordManager.Instance.LogoutRecord();
                    StartCoroutine(WaitTime());
                    return;
                default:
                    break;
            }

            ResetVoiceSDK();
        }

        void RecordLengthErr(bool longOrShort, uint duration)
        {
            QLoger.LOG("playerInfo RecordLengthErr" + longOrShort + "duration" + duration);
            EventDispatcher.FireEvent(GEnum.NamedEvent.SysUI_Record_LongORShort, longOrShort);
            obj_VoiceObj.SetActive(false);
        }

        void ResetVoiceSDK()
        {
            if (m_WaitLeave)
                return;
            m_WaitLeave = true;
            //RecordManager.Instance.leave();
        }

        private void LeaveCallBack()
        {
            m_WaitLeave = false;
            //RecordManager.Instance.InitGameLinkSDK();
            //RecordManager.Instance.LoginVoice();
        }

        #endregion

        #region 事件系统
        public UIDesktopFunction()
        {
            AddEvents();
        }
        private EventDispatcheHelper _MahjongEventHelper = new EventDispatcheHelper();
        private void AddEvents()
        {
            _MahjongEventHelper.AddEvent(GEnum.NamedEvent.EMjControlSetRestCount, EMjControlRestChange);
            _MahjongEventHelper.AddAllEvent();
        }
        private void EMjControlRestChange(object[] obj)
        {
            int loseCount = (int)obj[0];
            ShowCutTotalCard(true ? -loseCount : loseCount);
        }
        void OnDestroy()
        {
            _MahjongEventHelper.RemoveAllEvent();
        }
        #endregion


        public void ShowStage(bool isShow, bool isShowAin, string typeName)
        {
            UILabel label = StageType.gameObject.GetComponentInChildren<UILabel>();
            if (label != null)
            {
                label.text = typeName;
            }
            StageType.gameObject.SetActive(isShow);
        }
    }

}