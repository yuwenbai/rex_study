/**
* @Author YQC、GarFey
*
*/
using System;
using System.Collections.Generic;
using UnityEngine;

namespace projectQ
{
    public class UIUserInfo : UIViewBase
    {
        private UIUserInfoModel Model
        {
            get { return this._model as UIUserInfoModel; }
        }

        [Tooltip("微信名")]
        public UILabel LabelWXName;
        [Tooltip("用户ID")]
        public UILabel LabelUserId;
        [Tooltip("用戶IP")]
        public UILabel playerIP;
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
        [Tooltip("已发送按钮")]
        public GameObject buttonAdd;
        [Tooltip("修改帐号")]
        public GameObject ModificationAccBtn;
        public string tempTexture;
        //自身展示的按钮管理UI
        public GameObject SelfButtonGroup;
        //其它玩家展示的按钮管理UI
        public GameObject OtherPlayButtonGroup;
        //实名认证
        public UICertification Certification;
        //手机号绑定
        public UIBindPhoneNo BindPhoneNo;
        //大厅基本信息展示UI
        public GameObject UserBaseInfo_hall;
        //牌桌内基本信息展示UI
        public GameObject UserBaseInfo;
        [Tooltip("人物头像")]
        public UITexture HeadTex;
        //性别图标
        public GameObject SexIcon;
        //头像链接地址
        private string headUrl;
        //头像 位置
        public Transform iconFrame;
        //用户头像X轴坐标位置
        public const float positionX = -156;
        //用户头像Y轴坐标位置
        public const float positionY = 12f;
        //GPS 内容显示
        public UILabel GPS_labelInfo;

        /// <summary>
        /// 刷新UI信息
        /// </summary>
        public void RefreshUI()
        {
            var userInfo = Model.UserInfoData;
            if (userInfo == null) return;
            //微信名
            this.LabelWXName.text = userInfo.PlayerDataBase.Name;
            var info = MemoryData.PlayerData.get(Model.UserId).PlayerDataBase;

            //根据数据切换男女Icon，1 为 男，2 为女
            this.SexIcon.GetComponent<UISprite>().spriteName =
                (info.Sex == 1? "userinfo_icon_nan" : "userinfo_icon_nv");
            
            //IP显示，没有 则显示未知地址
            playerIP.text = string.Format("IP信息：\n{0}", userInfo.PlayerDataBase.ClientIp);
            if (String.IsNullOrEmpty(userInfo.PlayerDataBase.ClientIp))
            {
                playerIP.text = string.Format("IP信息：\n{0}", "未知地址");
            }
            //有手机号显示手机号 
            DisplayPhoneNo(userInfo);
            //根据 用户信息显示 相应的地理位置
            DisplayPositonInfo(userInfo);

            //更新头像  如果有变化 才会去 做更新
            if (!string.Equals(DownHeadTexture.Instance.Texture_HeadNameSet(MemoryData.PlayerData.get(Model.UserId).PlayerDataBase.HeadURL),tempTexture))
            {
                tempTexture = DownHeadTexture.Instance.Texture_HeadNameSet(MemoryData.PlayerData.get(Model.UserId).PlayerDataBase.HeadURL);
                DownHeadTexture.Instance.WeChat_HeadTextureGet(MemoryData.PlayerData.get(Model.UserId).PlayerDataBase.HeadURL, HeadTexCallBack);
            }
            //SelfButtonGroup.SetActive(Model.UserId == MemoryData.UserID);
            //OtherPlayButtonGroup.SetActive(Model.UserId != MemoryData.UserID);
            this.Certification.gameObject.SetActive(false);
            //当前用户
            if (Model.UserId == MemoryData.UserID)
            {
                SelfButtonGroup.SetActive(true);
                OtherPlayButtonGroup.SetActive(false);
                SelfUserInfo();
            }
            //其它玩家
            else
            {
                SelfButtonGroup.SetActive(false);
                OtherPlayButtonGroup.SetActive(true);
                OtherUserInfo();
            }
        }

        /// <summary>
        /// 手机号显示
        /// </summary>
        /// <param name="userInfo"></param>
        private void DisplayPhoneNo(PlayerDataModel userInfo)
        {
            this.LabelUserId.text = string.Format("{0}", userInfo.PlayerDataBase.Account);
            playerIP.transform.localPosition = new Vector3(playerIP.transform.localPosition.x,
                                                            -126f, playerIP.transform.localPosition.z);
            //目前需求不要手机号了
            /*if (String.IsNullOrEmpty(userInfo.PlayerDataBase.PhoneNo))
            {
                this.LabelUserId.text = string.Format("{0}",userInfo.PlayerDataBase.UserID);
                playerIP.transform.localPosition = new Vector3(playerIP.transform.localPosition.x,
                                                                -126f, playerIP.transform.localPosition.z);
            }
            else
            {
                this.LabelUserId.text = string.Format("{0}\n{1}",userInfo.PlayerDataBase.UserID, userInfo.PlayerDataBase.PhoneNo);
                playerIP.transform.localPosition = new Vector3(playerIP.transform.localPosition.x,
                                                                -140f, playerIP.transform.localPosition.z);
            }*/
        }

        /// <summary>
        /// 显示位置信息
        /// </summary>
        /// <param name="userInfo"></param>
        private void DisplayPositonInfo(PlayerDataModel userInfo)
        {
            var longitudeStr = userInfo.PlayerDataBase.Longitude > 0 ? "东经" : "西经";
            var latitudeStr = userInfo.PlayerDataBase.Latitude > 0 ? "北纬" : "南纬";
            if ((userInfo.PlayerDataBase.Longitude == 0.0F && userInfo.PlayerDataBase.Latitude == 0.0f))
            {
                this.GPS_labelInfo.text = string.Format("无法或未授权获取GPS信息");
            }
            else
            {
                this.GPS_labelInfo.text = string.Format("{0} {1:N2} {2} {3:N2}",
                    longitudeStr, userInfo.PlayerDataBase.Longitude,
                    latitudeStr, userInfo.PlayerDataBase.Latitude);
            }
        }

        /// <summary>
        /// 通过用户ID获取昵称
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <returns></returns>
        private string GetNameByUserId(long userId)
        {
            return MemoryData.PlayerData.get(userId).PlayerDataBase.Name;
        }

        /// <summary>
        /// 头像回调
        /// </summary>
        void HeadTexCallBack(Texture2D HeadTexture, string headName)
        {
            if (tempTexture == headName)
            {
                HeadTex.mainTexture = HeadTexture;
            }
        }

        /// <summary>
        /// 玩家本身信息设置
        /// </summary>
        private void SelfUserInfo()
        {
            var info = MemoryData.PlayerData.MyPlayerModel.PlayerDataBase;
            if (info.PhoneNo.Length > 0)
            {
                this.ButtonBindPhone.GetComponent<UISprite>().spriteName = "public_button_10";
                this.ButtonBindPhone.GetComponent<BoxCollider>().enabled = false;
                foreach (var item in bindPhoneEffect)
                {
                    item.SetActive(false);
                }
            }
            else
            {
                this.ButtonBindPhone.GetComponent<UISprite>().spriteName = "public_button_09";
                this.ButtonBindPhone.GetComponent<BoxCollider>().enabled = true;
            }
            if (string.IsNullOrEmpty(info.IDCard) && string.IsNullOrEmpty(info.RealName))
            {
                ButtonRealName.GetComponent<UISprite>().spriteName = "userinfo_button_bdsj";
                ButtonRealName.GetComponent<BoxCollider>().enabled = true;
            }
            else
            {
                ButtonRealName.GetComponent<UISprite>().spriteName = "userinfo_button_bdsj2";
                ButtonRealName.GetComponent<BoxCollider>().enabled = false;
            }

            if (MemoryData.GameStateData.CurrUISign >= SysGameStateData.EnumUISign.PrepareGame)
            {
                ButtonChangeAccount.SetActive(false);
                UserBaseInfo_hall.SetActive(false);
                UserBaseInfo.SetActive(true);
                iconFrame.localPosition = new Vector3(positionX, positionY, iconFrame.position.z);
            }
            else
            {
                ButtonChangeAccount.SetActive(true);
                UserBaseInfo_hall.SetActive(true);
                UserBaseInfo.SetActive(false);
                iconFrame.localPosition = new Vector3(positionX, positionY + 56f, iconFrame.position.z);
            }
        }

        /// <summary>
        /// 其它玩家 信息 设置
        /// </summary>
        private void OtherUserInfo()
        {
            var otherData = Model.UserInfoData as PlayerDataModel;
            if (MemoryData.GameStateData.CurrUISign >= SysGameStateData.EnumUISign.PrepareGame)
            {
                UserBaseInfo_hall.SetActive(false);
                UserBaseInfo.SetActive(true);
            }
            else
            {
                UserBaseInfo_hall.SetActive(true);
                UserBaseInfo.SetActive(false);
            }
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
                    ButtonAddFriend.GetComponent<UIButton>().normalSprite = "public_button_01";
                    ButtonAddFriend.GetComponentInChildren<UILabel>().text = "加为好友";
                }
            }
            else
            {
                ButtonAddFriend.spriteName = "public_button_02";
                ButtonAddFriend.GetComponent<UIButton>().normalSprite = "public_button_02";
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
                //this.Hide();
                this.Close();
            }
            else
            {
                LoadPop(WindowUIType.SystemPopupWindow, "提示", 
                    "是否删除该好友", new string[] { "取消", "确认" }, OnDeleteFriend);
            }
        }

        /// <summary>
        /// 删除好友
        /// </summary>
        /// <param name="btnIndex"></param>
        void OnDeleteFriend(int btnIndex)
        {
            if (btnIndex == 1)
            {
                var otherData = Model.UserInfoData as PlayerDataModel;
                otherData.FriendPlayerInfo.IsTemp = true;
                otherData.PlayerDataBase.isAddFriend = false;
                ModelNetWorker.Instance.DelFriendReq(Model.UserId);
            }

            this.Close();
        }
        //关闭按钮
        private void OnButtonCloseClick(GameObject go)
        {
            //this.Hide();
            this.Close();
        }
        //关闭方法
        private void CloseUI(object[] values)
        {
            this.Close();
        }
        /// <summary>
        /// 点击实名认证
        /// </summary>
        /// <param name="go"></param>
        private void OnButtonRealNameClick(GameObject go)
        {
            this.Certification.RefreshUI();
        }
        //绑定手机号
        private void OnButtonBindPhoneClick(GameObject go)
        {
            //绑定手机号
            this.BindPhoneNo.RefreshUI();
        }
        //更换账号
        private void OnButtonChangeAccountClick(GameObject go)
        {
            LoadPop(WindowUIType.SystemPopupWindow, "提示", 
                "是否更换账号", new string[] { "取消", "确定" }, OnExitLoginTipButtonClick);
        }
        //修改帐号
        private void OnModificationAccBtnClick(GameObject go)
        {
            _R.ui.OpenUI("UIModificationAccount");
        }
        
        // 退出登录的按钮点击事件
        private void OnExitLoginTipButtonClick(int btnIndex)
        {
            if (btnIndex == 1)
            {
                _R.ui.ClearAll();
                EventDispatcher.FireSysEvent(GEnum.NamedEvent.SysUI_OpenLogin);
            }
        }
        #endregion

        #region override
        /// <summary>
        /// 初始化注册按钮点击
        /// </summary>
        public override void Init()
        {
            UIEventListener.Get(ButtonAddFriend.gameObject).onClick = OnButtonAddFriendClick;
            UIEventListener.Get(ButtonClose).onClick = OnButtonCloseClick;
            UIEventListener.Get(ButtonRealName).onClick = OnButtonRealNameClick;
            UIEventListener.Get(ButtonBindPhone).onClick = OnButtonBindPhoneClick;
            UIEventListener.Get(ButtonChangeAccount).onClick = OnButtonChangeAccountClick;

            UIEventListener.Get(ModificationAccBtn).onClick = OnModificationAccBtnClick;
        }

        /// <summary>
        /// 隐藏UI
        /// </summary>
        public override void OnHide()
        {
            EventDispatcher.RemoveEvent(GEnum.NamedEvent.EMjControlCloseMaskUI, CloseUI);
        }

        /// <summary>
        /// 显示UI
        /// </summary>
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
    }
}