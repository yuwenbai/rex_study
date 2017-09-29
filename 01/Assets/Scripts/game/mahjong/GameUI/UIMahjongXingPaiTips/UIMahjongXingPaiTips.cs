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

    public class UIMahjongXingPaiTips : UIViewBase
    {
        public override void Init()
        {

        }

        public override void OnHide()
        {

        }

        public override void OnShow()
        {
            IniData();
        }


        //tip base
        //吃 碰 杠 胡 自摸 听 明楼 放毛
        public UIMahjongXingPaiTipsPos pos_Base = null;
        public GameObject obj_TipBase = null;


        //tip put
        public UIMahjongXingPaiTipsPos pos_Put = null;
        public GameObject obj_TipPut = null;


        //tip special
        public UIMahjongXingPaiTipsPos pos_Special = null;
        public GameObject obj_tipSpecail = null;

        public UIMahjongXingPaiTipsPos pos_StandingSpecial = null;
        //public GameObject obj_tipsState = null;
        //public GameObject obj_tipsStateUpdate = null;
        public GameObject obj_tipsStateNew = null;

        public UIMahjongXingPaiTipsPos pos_Score = null;
        public GameObject obj_tipScore = null;
        public GameObject obj_tipTypeScore = null;

        public UIMahjongXingPaiTipsPos pos_State = null;
        public GameObject obj_tipState = null;

        public GameObject obj_tipBiaoyan = null;            // 表演
        public GameObject obj_tipBiaoyanTip = null;         //表演提示

        private void IniData()
        {
            StopAllCoroutines();
            ClearTipBase();
            pos_Put.ClearMessage();
            ClearTipSpecial();
            CloseSpecialState(EnumMjSpecialCheck.Null, true);
            ClearTipScore();
            pos_State.ClearMessage();
            ShowBiaoyanTip(false);
        }


        public void ClearMessage()
        {
            IniData();
        }


        #region Tip Base

        private System.Action<List<int>, EnumMjOpAction> showBaseCall = null;
        private bool curShowBase = false;
        private float showBaseTime = -1f;
        private float showBaseDelay = 0f;
        private IEnumerator ieTipBase = null;

        /// <summary>
        /// 展示打牌的基本提示
        /// </summary>
        /// <param name="uiseatID"></param>
        /// <param name="actionType"></param>
        /// <param name="showTime"></param>
        /// <param name="showCall"></param>
        public void ShowTipsBase(List<int> uiseatID, EnumMjOpAction actionType, float showTimeDelay, float showTime = 1.5f, System.Action<List<int>, EnumMjOpAction> showCall = null)
        {
            if (curShowBase)
            {
                if (ieTipBase != null)
                {
                    if (ieTipBase != null)
                    {
                        StopCoroutine(ieTipBase);
                    }
                    ClearTipBase();
                }
            }

            //MJTODO: 根据玩法 读取配置
            //string baseTipName = null;
            string pathBase = GameConst.path_Anim_MjXingPaiConfirm;
            string pathSub = null;
            GEnum.SoundEnum musicType = GEnum.SoundEnum.desk_teshu;
            switch (actionType)
            {
                case EnumMjOpAction.MjOp_Peng:
                    {
                        //baseTipName = "state_icon_peng";
                        musicType = GEnum.SoundEnum.btn_null;
                        pathSub = "Peng";
                    }
                    break;
                case EnumMjOpAction.MjOp_Gang:
                    {
                        //baseTipName = "state_icon_gang";
                        pathSub = "Gang";
                        musicType = GEnum.SoundEnum.btn_null;
                    }
                    break;
                case EnumMjOpAction.MjOp_Ting:
                    {
                        pathSub = "Ting";
                    }
                    break;
                case EnumMjOpAction.MjOp_Minglou:
                    {
                        pathSub = "Minglou";
                    }
                    break;
                case EnumMjOpAction.MjOp_HuPai:
                    {
                        //baseTipName = "state_icon_hu";
                        pathSub = "Hu";
                    }
                    break;
                case EnumMjOpAction.MjOp_PiCi:
                    {
                        pathSub = "pici";
                    }
                    break;
                case EnumMjOpAction.MjOp_CiHu:
                    {
                        pathSub = "Ci";
                    }
                    break;
                case EnumMjOpAction.MjOp_Chi:
                    {
                        //baseTipName = "state_icon_chi";

                        pathSub = "Chi";
                        musicType = GEnum.SoundEnum.btn_null;
                    }
                    break;
                case EnumMjOpAction.MjOp_Mao:
                    {
                        //baseTipName = "state_icon_fangmao";
                        pathSub = "Fangmao";
                    }
                    break;
                case EnumMjOpAction.MjOp_Zimo:
                    {
                        //baseTipName = "state_icon_zimo";
                        pathSub = "Zimo";
                    }
                    break;
                case EnumMjOpAction.MjOp_Guafeng:
                    {
                        pathSub = "guafeng";
                        musicType = GEnum.SoundEnum.btn_null;
                    }
                    break;
                case EnumMjOpAction.MjOp_Xiayu:
                    {
                        pathSub = "xiayu";
                        musicType = GEnum.SoundEnum.btn_null;
                    }
                    break;
                case EnumMjOpAction.MjOp_ChangeFlower:
                    {
                        pathSub = "buhua";
                    }
                    break;
                case EnumMjOpAction.MjOp_ChangeMao:
                    {
                        pathSub = "bumao";
                    }
                    break;
                case EnumMjOpAction.MjOp_Biaoyan:
                    {
                        pathSub = "biaoyanchenggong";
                    }
                    break;
                case EnumMjOpAction.MjOp_NiuPaiBuHua:
                    pathSub = "niupai";
                    break;

                case EnumMjOpAction.MjOp_HuPiaoGenPiao:
                    {
                        pathSub = "Piao";
                    }
                    break;
                case EnumMjOpAction.MjOp_Minglao:
                    {
                        pathSub = "Minglao";
                    }
                    break;
                case EnumMjOpAction.MjOp_Minglv:
                    {
                        pathSub = "minglv";
                    }
                    break;
                case EnumMjOpAction.MjOp_MinglouMu:
                    {
                        pathSub = "MingMulou";
                    }
                    break;
                case EnumMjOpAction.MjOp_AnlouMu:
                    {
                        pathSub = "Anmulou";
                    }
                    break;
                case EnumMjOpAction.MjOp_AnlouShou:
                    {
                        pathSub = "Anlou";
                    }
                    break;
            }


            pathBase = string.Format(pathBase, pathSub);
            showBaseTime = showTime;
            showBaseCall = showCall;
            showBaseDelay = Mathf.Max(0f, showTimeDelay);

            ieTipBase = IECloseTipBase(uiseatID, pathBase, actionType, musicType);
            StartCoroutine(ieTipBase);
        }


        private IEnumerator IECloseTipBase(List<int> seats, string pathBase, EnumMjOpAction actionType, GEnum.SoundEnum musicType)
        {
            yield return new WaitForSeconds(showBaseDelay);

            if (seats != null)
            {
                for (int i = 0; i < seats.Count; i++)
                {
                    Transform transSelf = GameTools.InstantiatePrefab(pathBase, pos_Base.trans_Pos[seats[i]], true, true);
                    if (transSelf != null)
                    {
                        transSelf.gameObject.SetActive(true);
                        EventDispatcher.FireEvent(MJEnum.MusicEnum.ME_PlaySoundFX.ToString(), musicType);
                    }
                }
            }

            curShowBase = true;
            yield return new WaitForSeconds(showBaseTime);
            if (showBaseCall != null)
            {
                showBaseCall(seats, actionType);
            }
            ClearTipBase();
        }


        private void ClearTipBase()
        {
            pos_Base.ClearMessage();
            curShowBase = false;
            showBaseCall = null;
            ieTipBase = null;
        }



        #endregion

        #region Tips Put
        /// <summary>
        /// 展示打牌的提示
        /// </summary>
        /// <param name="uiSeatID"></param>
        /// <param name="mjCode"></param>
        /// <param name="time"></param>
        /// <param name="specialType"></param>
        public void ShowTipsPut(List<int> uiSeatID, int mjCode, float time = 1.0f, EnumMjSpecialType specialType = EnumMjSpecialType.Null)
        {
            if (uiSeatID != null)
            {
                for (int i = 0; i < uiSeatID.Count; i++)
                {
                    Transform parent = pos_Put.trans_Pos[uiSeatID[i]];
                    parent.DestroyChildren();

                    GameObject spriteItem = GameTools.InstantiatePrefab(obj_TipPut, parent, true, true);
                    UISprite spriteIcon = spriteItem.GetComponent<UISprite>();
                    if (spriteIcon)
                    {
                        CardHelper.SetRecordUI(spriteIcon, mjCode);
                    }
                    DestroyAuto auto = spriteItem.GetComponent<DestroyAuto>();
                    auto.autoDesTime = time;
                    spriteItem.SetActive(true);
                }
            }
        }


        #endregion

        #region Tip Special Hu

        private EnumMjHuType curHuType = EnumMjHuType.Null;
        private bool curShowSpecial = false;
        private float showSpecialTime = -1f;
        private IEnumerator ieTipSpecialHu;
        private int specialShowSeat = -1;

        /// <summary>
        /// 展示胡牌提示
        /// </summary>
        /// <param name="uiSeatID"></param>
        /// <param name="isSelfMo"></param>
        /// <param name="huType"></param>
        /// <param name="showTime"></param>
        /// <param name="showTimeSub"></param>
        public void ShowTipsSpecialHu(List<int> uiSeatID, EnumMjOpAction actionType, EnumMjHuType huType, float showTimeDelay, float showTime = 1.5f, float showTimeSub = 1.5f, int specialShowSeat = -1)
        {
            curHuType = huType;
            showSpecialTime = showTimeSub;
            this.specialShowSeat = specialShowSeat;
            this.ShowTipsBase(uiSeatID, actionType, showTimeDelay, showTime, ShowTipsHuCall);
        }

        private void ShowTipsHuCall(List<int> uiSeatID, EnumMjOpAction actionType)
        {
            if (curShowSpecial)
            {
                if (ieTipSpecialHu != null)
                {
                    StopCoroutine(ieTipSpecialHu);
                    ClearTipSpecial();
                }
            }

            List<int> showSeatID = uiSeatID;

            //MJTODO: 根据玩法 读取配置
            string specialName = null;
            bool showMiddle = false;
            bool showAnim = true;
            GEnum.SoundEnum musicType = GEnum.SoundEnum.desk_gskh;
            switch (curHuType)
            {
                case EnumMjHuType.MjHuType_HaiDiLao:
                    {
                        showMiddle = true;
                        specialName = "Haidilaoyue";
                        musicType = GEnum.SoundEnum.desk_hdly;
                    }
                    break;
                case EnumMjHuType.MjHuType_QiangGangHu:
                    {
                        specialName = "Qiangganghu";
                        musicType = GEnum.SoundEnum.desk_qgh;
                    }
                    break;
                case EnumMjHuType.MjHuType_GangShangHua:
                    {
                        specialName = "Gangshangkaihua";
                        musicType = GEnum.SoundEnum.desk_gskh;
                    }
                    break;
                case EnumMjHuType.MjHuType_GunMany:
                    {
                        showSeatID.Clear();
                        if (specialShowSeat >= 0)
                        {
                            showSeatID.Add(specialShowSeat);
                        }
                        specialName = "Yipaoduoxiang";
                        musicType = GEnum.SoundEnum.desk_ypdx;
                    }
                    break;
                case EnumMjHuType.MjHuType_HuJiaoZhuanYi:
                    {
                        showSeatID.Clear();
                        if (specialShowSeat >= 0)
                        {
                            showSeatID.Add(specialShowSeat);
                        }
                        showAnim = false;
                        specialName = "hustate_hujiaozhuanyi";
                    }
                    break;
            }

            if (string.IsNullOrEmpty(specialName))
            {
                return;
            }


            if (showAnim)
            {
                string path = string.Format(GameConst.path_Fx_MjHuSub, specialName);

                if (showMiddle)
                {
                    GameObject obj = GameTools.InstantiatePrefab(path, pos_Special.trans_Center, true, true).gameObject;
                    obj.SetActive(true);
                }
                else
                {
                    if (showSeatID != null)
                    {
                        for (int i = 0; i < showSeatID.Count; i++)
                        {
                            GameObject obj = GameTools.InstantiatePrefab(path, pos_Special.trans_Pos[showSeatID[i]], true, true).gameObject;
                            obj.SetActive(true);
                        }
                    }

                }

                EventDispatcher.FireEvent(MJEnum.MusicEnum.ME_PlaySoundFX.ToString(), musicType);
            }
            else
            {
                if (showMiddle)
                {
                    SetSpriteToParent(obj_tipSpecail, pos_Special.trans_Center, specialName);
                }
                else
                {
                    if (showSeatID != null)
                    {
                        for (int i = 0; i < showSeatID.Count; i++)
                        {
                            SetSpriteToParent(obj_tipSpecail, pos_Special.trans_Pos[showSeatID[i]], specialName);
                        }
                    }
                }
            }

            ieTipSpecialHu = IECloseTipSpecial();
            StartCoroutine(ieTipSpecialHu);
        }


        private IEnumerator IECloseTipSpecial()
        {
            curShowSpecial = true;
            yield return new WaitForSeconds(showSpecialTime);
            ClearTipSpecial();
        }


        private void ClearTipSpecial()
        {
            pos_Special.ClearMessage();
            curShowSpecial = false;
            specialShowSeat = -1;
            ieTipSpecialHu = null;
        }


        #endregion

        #region Tip Special State
        private EnumMjSpecialCheck curStateType = EnumMjSpecialCheck.Null;

        /// <summary>
        /// 显示状态中
        /// </summary>
        /// <param name="specialType"></param>
        public void ShowSpecialState(EnumMjSpecialCheck specialType)
        {
            if (curStateType != EnumMjSpecialCheck.Null && curStateType != specialType)
            {
                CloseSpecialState(curStateType);
            }

            if (curStateType == specialType)
            {
                return;
            }

            this.curStateType = specialType;
            string specialName = null;

            List<int> uiSeats = MjDataManager.Instance.GetAllUiSeatCurDesk(true);
            if (uiSeats != null && uiSeats.Count > 0)
            {
                if (obj_tipsStateNew != null)
                {
                    UIMahjongXingPaiTipsState state = obj_tipsStateNew.GetComponent<UIMahjongXingPaiTipsState>();
                    state.IniState(curStateType, true);
                }

                for (int i = 0; i < uiSeats.Count; i++)
                {
                    SetSpriteToParent(obj_tipsStateNew, pos_StandingSpecial.trans_Pos[uiSeats[i]], specialName);
                }
            }
        }


        public void UpdateSpecialState(int seatID, EnumMjSpecialCheck specialType)
        {
            if (curStateType == EnumMjSpecialCheck.Null)
            {
                ShowSpecialState(specialType);
            }

            string specialName = null;

            if (obj_tipsStateNew != null)
            {
                UIMahjongXingPaiTipsState state = obj_tipsStateNew.GetComponent<UIMahjongXingPaiTipsState>();
                state.IniState(specialType, false);
            }

            pos_StandingSpecial.trans_Pos[seatID].DestroyChildren();
            SetSpriteToParent(obj_tipsStateNew, pos_StandingSpecial.trans_Pos[seatID], specialName);
        }


        public void CloseSpecialState(EnumMjSpecialCheck checkType, bool qiangzhiClear = false)
        {
            if (checkType == curStateType || qiangzhiClear)
            {
                curStateType = EnumMjSpecialCheck.Null;
                pos_StandingSpecial.ClearMessage();
            }
        }

        #endregion

        #region Tip Special Score
        private bool curShowSpecail = false;
        private float showScoreTime = -1f;
        private IEnumerator ieTipScore = null;

        /// <summary>
        /// 单纯显示积分
        /// </summary>
        /// <param name="uiSeatList"></param>
        /// <param name="scoreList"></param>
        /// <param name="showTime"></param>
        public void ShowTipScore(List<int> uiSeatList, List<int> scoreList, float showTime = -1f)
        {
            if (curShowSpecail)
            {
                if (ieTipScore != null)
                {
                    StopCoroutine(ieTipScore);
                }
                ClearTipScore();
            }

            if (uiSeatList != null && scoreList != null)
            {
                for (int i = 0; i < uiSeatList.Count; i++)
                {
                    if (scoreList[i] == 0)
                    {
                        continue;
                    }

                    GameObject scoreItem = GameTools.InstantiatePrefab(obj_tipScore, pos_Score.trans_Pos[uiSeatList[i]], true, true);
                    UIMahjongXingPaiTipsScore scoreCom = scoreItem.GetComponent<UIMahjongXingPaiTipsScore>();
                    if (scoreCom != null)
                    {
                        scoreCom.SetScore(scoreList[i]);
                    }
                    scoreItem.SetActive(true);
                }
            }

            if (showTime > 0)
            {
                showScoreTime = showTime;

                ieTipScore = IECloseTipScore();
                StartCoroutine(ieTipScore);
            }
        }


        /// <summary>
        /// 显示类型加积分
        /// </summary>
        /// <param name="uiTypeSeat"></param>
        /// <param name="scoreType"></param>
        /// <param name="uiSeatList"></param>
        /// <param name="scoreList"></param>
        public void ShowTipTypeScore(int uiTypeSeat, int scoreType, int typeScore, List<int> uiSeatList, List<int> scoreList, float showTime = -1f)
        {
            if (curShowSpecail)
            {
                if (ieTipScore != null)
                {
                    StopCoroutine(ieTipScore);
                }

                ClearTipScore();
            }

            ShowTipScore(uiSeatList, scoreList);

            GameObject scoreItem = GameTools.InstantiatePrefab(obj_tipTypeScore, pos_Score.trans_Pos[uiTypeSeat], true, true);
            UIMahjongXingPaiTipsScore scoreCom = scoreItem.GetComponent<UIMahjongXingPaiTipsScore>();
            if (scoreCom != null)
            {
                scoreCom.SetTypeWithScore(scoreType, typeScore);
            }
            scoreItem.SetActive(true);

            if (showTime > 0)
            {
                showScoreTime = showTime;

                ieTipScore = IECloseTipScore();
                StartCoroutine(ieTipScore);
            }

        }


        private IEnumerator IECloseTipScore()
        {
            curShowSpecail = true;
            yield return new WaitForSeconds(showScoreTime);
            ClearTipScore();
        }


        private void ClearTipScore()
        {
            pos_Score.ClearMessage();
            curShowSpecail = false;
            ieTipScore = null;
        }


        #endregion

        #region Tips Special State 表演
        /// <summary>
        /// 表演
        /// </summary>
        /// <param name="uiSeatID"></param>
        /// <param name="nValue"></param>
        public void ShowTipsStateBiao(int uiSeatID, int nValue)
        {
            pos_State.ClearMessageBySeat(uiSeatID);
            if (nValue <= 0 || nValue > 4)
            {
                //clear 
                return;
            }
            else
            {
                SetBiaoyanObj(nValue);
                SetSpriteToParent(obj_tipBiaoyan, pos_State.trans_Pos[uiSeatID], null);
            }
        }

        private void SetBiaoyanObj(int nValue)
        {
            UIMahjongXingPaiTipsScore scoreItem = obj_tipBiaoyan.GetComponent<UIMahjongXingPaiTipsScore>();
            if (scoreItem != null)
            {
                scoreItem.SetTypeValue(nValue);
            }
        }


        public void ShowBiaoyanTip(bool state)
        {
            obj_tipBiaoyanTip.SetActive(state);
        }


        #endregion

        #region Tips Special State Hu
        /// <summary>
        /// 血流血战中胡状态
        /// </summary>
        /// <param name="uiseatID"></param>
        public void ShowTipsState(int uiseatID)
        {
            if (pos_State.trans_Pos[uiseatID].childCount > 0)
            {
                return;
            }
            SetSpriteToParent(obj_tipState, pos_State.trans_Pos[uiseatID], null);
        }




        /// <summary>
        /// 查花猪查大叫
        /// </summary>
        /// <param name="dajiaoSeat"></param>
        /// <param name="huazhuSeat"></param>
        public void ShowTipsHuajiao(List<int> dajiaoSeat, List<int> huazhuSeat)
        {
            curShowSpecail = true;
            pos_Special.ClearMessage();

            if (dajiaoSeat != null && dajiaoSeat.Count > 0)
            {
                for (int i = 0; i < dajiaoSeat.Count; i++)
                {
                    SetSpriteToParent(obj_tipSpecail, pos_Special.trans_Pos[dajiaoSeat[i]], "state_icon_weitingpai");
                }
            }

            if (huazhuSeat != null && huazhuSeat.Count > 0)
            {
                for (int i = 0; i < huazhuSeat.Count; i++)
                {
                    SetSpriteToParent(obj_tipSpecail, pos_Special.trans_Pos[huazhuSeat[i]], "state_icon_huazhu");
                }
            }
        }

        #endregion

        #region Common Func

        private void SetSpriteToParent(GameObject objItem, Transform parent, string iconName)
        {
            GameObject spriteItem = GameTools.InstantiatePrefab(objItem, parent, true, true);
            UISprite spriteIcon = spriteItem.GetComponent<UISprite>();
            if (spriteIcon)
            {
                if (!string.IsNullOrEmpty(iconName))
                {
                    spriteIcon.spriteName = iconName;
                }
                spriteIcon.MakePixelPerfect();
            }
            spriteItem.SetActive(true);
        }

        #endregion


    }

}
