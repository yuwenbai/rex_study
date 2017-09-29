

using System;
/**
* @Author YQC
*
*
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace projectQ
{
    public class UIUserInfo_Old_20170627 : UIViewBase
    {
        private UIUserInfoModel Model
        {
            get { return this._model as UIUserInfoModel; }
        }
        public GameObject Root;

        [Tooltip("微信名")]
        public UILabel LabelWXName;

        [Tooltip("用户ID")]
        public UILabel LabelUserId;

        [Tooltip("桌卡")]
        public UILabel LabelTickets;

        [Tooltip("盘数")]
        public UILabel[] LabelTotalBouts;

        [Tooltip("胜率")]
        public UILabel[] LabelWinRate;

        //[Tooltip("最大番数")]
        //public UILabel LabelMax;

        [Tooltip("关闭按钮")]
        public GameObject ButtonClose;
        [Tooltip("实名验证按钮")]
        public GameObject ButtonRealName;
        [Tooltip("绑定手机按钮")]
        public GameObject ButtonBindPhone;
        public List<GameObject> bindPhoneEffect;
        [Tooltip("修改账号按钮")]
        public GameObject ButtonChangeAccount;
        [Tooltip("加好友按钮")]
        public UISprite ButtonAddFriend;
        public GameObject buttonAdd;
        [Tooltip("最大牌型")]
        public UILabel LabelmaxCard;
        public GameObject SelfButtonGroup;
        public GameObject OtherPlayButtonGroup;
        public UIUserInfoRightCenterControll rigth;
        public GameObject noHuPai;
        //实名认证
        public UICertification Certification;
        //手机号绑定
        public UIBindPhoneNo BindPhoneNo;
        //最大牌型
        public BestRecordController bestRecord;

        [Tooltip("人物头像")]
        public UITexture HeadTex;
        private string headUrl;

        public UILabel centerLabelPanshu;
        public UILabel centerLabelShenglv;
        public UILabel centerLabelFanshu;

        public void RefreshUI()
        {
            var userInfo = Model.UserInfoData;
            if (userInfo == null) return;
            Debug.LogWarning("UIUserInfo user = " + Model.UserId + ";name = " + MemoryData.PlayerData.get(Model.UserId).PlayerDataBase.Name);
            this.LabelWXName.text = userInfo.PlayerDataBase.Name;
            this.LabelUserId.text = "ID:" + userInfo.PlayerDataBase.UserID;
            this.LabelTickets.text = "[C9D2F3]桌卡:[-]" + "[FFF799]" + userInfo.PlayerDataBase.TotalTicket + "[-]";
            for (int i = 0; i < LabelTotalBouts.Length; ++i)
            {
                LabelTotalBouts[i].text = "[C9D2F3]局数:[-]" + "[FFF799]" + userInfo.PlayerDataBase.TotalBouts + "[-]";
            }
            for (int i = 0; i < LabelWinRate.Length; ++i)
            {
                double d = (double)userInfo.PlayerDataBase.WinBouts / (double)userInfo.PlayerDataBase.TotalBouts;

                string shenglv = System.Convert.ToDouble(d).ToString("0.00");
                double sl = double.Parse(shenglv);
                sl = sl * 100;
                if (userInfo.PlayerDataBase.TotalBouts == 0)
                {
                    LabelWinRate[i].text = "[C9D2F3]胜率:[-][FFF799]0%[-]";
                }
                else
                {
                    LabelWinRate[i].text = "[C9D2F3]胜率:[-]" + "[FFF799]" + sl + "%[-]";
                }
            }
            DebugPro.Log(DebugPro.EnumLog.HeadUrl, "UIUserInfo__RefreshUI", MemoryData.PlayerData.get(Model.UserId).PlayerDataBase.HeadURL);
            DownHeadTexture.Instance.WeChat_HeadTextureGet(MemoryData.PlayerData.get(Model.UserId).PlayerDataBase.HeadURL, HeadTexCallBack);
            //if (Model.UserId == MemoryData.UserID)
            //{
            //    DownHeadTexture.Instance.WeChat_HeadTextureGet(MemoryData.PlayerData.MyPlayerModel.PlayerDataBase.HeadURL, HeadTexCallBack);
            //}
            //else
            //{
            //    DownHeadTexture.Instance.WeChat_HeadTextureGet(headUrl, HeadTexCallBack);
            //}
            SelfButtonGroup.SetActive(Model.UserId == MemoryData.UserID);
            OtherPlayButtonGroup.SetActive(Model.UserId != MemoryData.UserID);
            this.Certification.gameObject.SetActive(false);
            if (Model.UserId == MemoryData.UserID)
            {
                SelfButtonGroup.SetActive(true);
                OtherPlayButtonGroup.gameObject.SetActive(false);
                SelfUserInfo();
            }
            else
            {
                SelfButtonGroup.SetActive(false);
                OtherPlayButtonGroup.gameObject.SetActive(true);
                OtherUserInfo();
            }
        }

        public void ShowRoot(bool isShow)
        {
            //this.Root.SetActive(isShow);
        }

        /// <summary>
        /// 头像回调
        /// </summary>
        void HeadTexCallBack(Texture2D HeadTexture, string headName)
        {
            HeadTex.mainTexture = HeadTexture;
        }


        private void SelfUserInfo()
        {
            var info = MemoryData.PlayerData.MyPlayerModel.PlayerDataBase;

            if (info.PhoneNo.Length > 0)
            {
                this.ButtonBindPhone.GetComponent<UISprite>().spriteName = "public_button_04";
                //this.ButtonBindPhone.GetComponentInChildren<UILabel>().text = "绑定手机";
                this.ButtonBindPhone.GetComponent<BoxCollider>().enabled = false;
                foreach (var item in bindPhoneEffect)
                {
                    item.SetActive(false);
                }
            }
            else
            {
                this.ButtonBindPhone.GetComponent<UISprite>().spriteName = "public_button_05";
                //this.ButtonBindPhone.GetComponentInChildren<UILabel>().text = "绑定手机";
                this.ButtonBindPhone.GetComponent<BoxCollider>().enabled = true;
            }
            if (string.IsNullOrEmpty(info.IDCard) && string.IsNullOrEmpty(info.RealName))
            {
                ButtonRealName.GetComponent<UISprite>().spriteName = "prepare_button_02";
                ButtonRealName.GetComponent<BoxCollider>().enabled = true;
            }
            else
            {
                ButtonRealName.GetComponent<UISprite>().spriteName = "prepare_button_03";
                ButtonRealName.GetComponent<BoxCollider>().enabled = false;

            }

            if (MemoryData.GameStateData.CurrUISign >= SysGameStateData.EnumUISign.PrepareGame)
            {
                ButtonChangeAccount.SetActive(false);
            }
            else
            {
                ButtonChangeAccount.SetActive(true);
            }

        }
        private void OtherUserInfo()
        {
            var otherData = Model.UserInfoData as PlayerDataModel;
            if (otherData.FriendPlayerInfo.IsTemp)
            {
                if (Model.UserInfoData.PlayerDataBase.isAddFriend)
                {
                    buttonAdd.SetActive(true);
                    ButtonAddFriend.gameObject.SetActive(false);
                }
                else
                {
                    buttonAdd.SetActive(false);
                    ButtonAddFriend.gameObject.SetActive(true);
                    ButtonAddFriend.spriteName = "public_button_01";
                    ButtonAddFriend.GetComponentInChildren<UILabel>().text = "加为好友";
                }

            }
            else
            {
                ButtonAddFriend.spriteName = "public_button_02";
                ButtonAddFriend.GetComponentInChildren<UILabel>().text = "删除好友";
            }
        }
        #region event
        //加好友按钮
        private void OnButtonAddFriendClick(GameObject go)
        {
            var otherData = Model.UserInfoData as PlayerDataModel;
            if (otherData.FriendPlayerInfo.IsTemp)
            {
                Model.UserInfoData.PlayerDataBase.isAddFriend = true;
                ModelNetWorker.Instance.AddFriendReq(Model.UserId);
                buttonAdd.SetActive(true);
                ButtonAddFriend.gameObject.SetActive(false);
                this.Hide();
            }
            else
            {
                LoadPop(WindowUIType.SystemPopupWindow, "提示", "是否删除该好友", new string[] { "取消", "确认" }, OnDeleteFriend);
            }
        }

        void OnDeleteFriend(int btnIndex)
        {
            if (btnIndex == 1)
            {
                ModelNetWorker.Instance.DelFriendReq(Model.UserId);
                var otherData = Model.UserInfoData as PlayerDataModel;
                otherData.FriendPlayerInfo.IsTemp = true;
                otherData.PlayerDataBase.isAddFriend = false;
            }

            this.Close();
        }
        //关闭按钮
        private void OnButtonCloseClick(GameObject go)
        {
            //this.Hide();
            this.Close();
        }
        //实名认证


        private void CloseUI(object[] values)
        {
            this.Close();
        }


        private void OnButtonRealNameClick(GameObject go)
        {
            this.Certification.RefreshUI();
        }
        //绑定手机号
        private void OnButtonBindPhoneClick(GameObject go)
        {
            // SDKManager.Instance.SDKFunction("BIND_PHONE");
            //SDKManager.Instance.SDKFunction(FunctionEnumSDKMananger.BIND_PHONE);

            //绑定手机号
            this.BindPhoneNo.RefreshUI();
        }
        //更换账号
        private void OnButtonChangeAccountClick(GameObject go)
        {
            LoadPop(WindowUIType.SystemPopupWindow, "提示", "是否更换账号", new string[] { "取消", "确定" }, OnExitLoginTipButtonClick);
        }
        // 退出登录的按钮点击事件
        private void OnExitLoginTipButtonClick(int btnIndex)
        {
            if (btnIndex == 1)
            {
                _R.ui.ClearAll();
                EventDispatcher.FireSysEvent(GEnum.NamedEvent.SysUI_OpenLogin);
                //MemoryData.GameStateData.SetCurrUISign(SysGameStateData.EnumUISign.Login);
            }
        }
        #endregion

        #region override
        public override void Init()
        {
            UIEventListener.Get(ButtonAddFriend.gameObject).onClick = OnButtonAddFriendClick;
            UIEventListener.Get(ButtonClose).onClick = OnButtonCloseClick;
            UIEventListener.Get(ButtonRealName).onClick = OnButtonRealNameClick;
            UIEventListener.Get(ButtonBindPhone).onClick = OnButtonBindPhoneClick;
            UIEventListener.Get(ButtonChangeAccount).onClick = OnButtonChangeAccountClick;
            rigth.InitAllItem();
            rigth.dele_OnRigth = this.OnRightItemClick;
            rigth.OnItemClick(MemoryData.MahjongPlayData.LocalRegionAndFashionPlayList[0].ConfigId);
        }

        public override void OnHide()
        {
            EventDispatcher.RemoveEvent(GEnum.NamedEvent.EMjControlCloseMaskUI, CloseUI);
        }

        public override void OnShow()
        {
            this.MaskClickClose = true;
            RefreshUI();

            EventDispatcher.AddEvent(GEnum.NamedEvent.EMjControlCloseMaskUI, CloseUI);
        }

        public override void OnPushData(object[] data)
        {
            if (data != null && data.Length > 0)
            {
                Model.UserId = System.Convert.ToInt64(data[0]);
                headUrl = data[1] as string;
            }
            else
            {
                Model.UserId = MemoryData.UserID;
                headUrl = "";
            }
        }
        #endregion

        void OnRightItemClick(int playId)
        {
            bestRecord.Clear();
            centerLabelFanshu.text = "[C9D2F3]最大番数:[-][FFF799]0番[-]";
            centerLabelShenglv.text = "[C9D2F3]胜率:[-][FFF799]0%[-]";
            centerLabelPanshu.text = "[C9D2F3]局数:[-][FFF799]0[-]";
            NoBestRecord();
            var userInfo = Model.UserInfoData;
            if (userInfo == null) return;

            if (userInfo.PlayerDataBase.MjTimes != null)
            {
                if (userInfo.PlayerDataBase.MjTimes.Count != 0)
                {
                    for (int i = 0; i < userInfo.PlayerDataBase.MjTimes.Count; i++)
                    {
                        if (playId == userInfo.PlayerDataBase.MjTimes[i].MjType)
                        {
                            centerLabelPanshu.text = "[C9D2F3]局数:[-]" + "[FFF799]" + userInfo.PlayerDataBase.MjTimes[i].TotalBouts.ToString() + "[-]";
                            double d = (double)userInfo.PlayerDataBase.MjTimes[i].WinBouts / (double)userInfo.PlayerDataBase.MjTimes[i].TotalBouts;
                            string shenglv = System.Convert.ToDouble(d).ToString("0.00");
                            double sl = double.Parse(shenglv);
                            sl = sl * 100;
                            if (userInfo.PlayerDataBase.MjTimes[i].TotalBouts == 0)
                            {
                                centerLabelShenglv.text = "[C9D2FE]胜率:[-][FFF799]0%[-]";
                            }
                            else
                            {
                                centerLabelShenglv.text = "[C9D2F3]胜率:[-]" + "[FFF799]" + sl + "%[-]";
                            }

                            if (userInfo.PlayerDataBase.MjRecords.Count == 0)
                            {
                                NoBestRecord();
                            }
                            break;
                        }
                        else
                        {
                            NoBestRecord();
                            centerLabelShenglv.text = "[C9D2F3]胜率:[-][FFF799]0%[-]";
                            centerLabelPanshu.text = "[C9D2F3]局数:[-][FFF799]0[-]";
                            centerLabelFanshu.text = "[C9D2F3]最大番数:[-][FFF799]0番[-]";
                        }
                    }

                    for (int j = 0; j < userInfo.PlayerDataBase.MjRecords.Count; j++)
                    {

                        if (playId == userInfo.PlayerDataBase.MjRecords[j].mjType)
                        {
                            InitBestRecord(userInfo.PlayerDataBase.MjRecords[j]);
                            break;
                        }
                    }
                }
            }
        }

        void NoBestRecord()
        {
            centerLabelFanshu.text = "[C9D2F3]最大番数:[-][FFF799]0番[-]";
            this.LabelmaxCard.text = "[C9D2F3]最大牌型:[-][FFF799]无[-]";
            bestRecord.gameObject.SetActive(false);
            noHuPai.SetActive(true);
        }
        public void InitBestRecord(BestMjRecord record)
        {
            string gameType = CardHelper.BalanceGetGameType(record.mjType);
            centerLabelFanshu.text = "[C9D2F3]最大番数:[-]" + "[FFF799]" + record.oddsCount + "番[-]";

            if (record.paiType.Count > 0)
            {
                System.Text.StringBuilder sb = new System.Text.StringBuilder();

                if (record.paiType.Count == 1)
                {
                    sb.Append("[C9D2F3]最大牌型:[-]" + "[FFF799]" + CardHelper.BalanceGetPaiType(record.paiType[0], gameType) + "[-]");
                    this.LabelmaxCard.text = sb.ToString();
                    sb.Length = 0;
                }
                else
                {
                    sb.Append("[C9D2F3]最大牌型:[-]" + "[FFF799]" + CardHelper.BalanceGetPaiType(record.paiType[0], gameType) + "(");
                    for (int i = 1; i < record.paiType.Count; i++)
                    {
                        sb.Append(CardHelper.BalanceGetPaiType(record.paiType[i], gameType) + " ");
                    }
                    sb.Append(")[-]");
                    this.LabelmaxCard.text = sb.ToString();
                    sb.Length = 0;
                }

            }
            bestRecord.gameObject.SetActive(true);
            QLoger.LOG(bestRecord.gameObject.activeSelf);
            noHuPai.SetActive(false);
            bestRecord.IniBestRecord(record);
        }

    }
}