/**
 * @Author jl
 *  活动奖励显示
 *
 */

using UnityEngine;

namespace projectQ
{
    public class UIActivityLotteryReward : UIViewBase
    {
        public UISprite RewardIcon;
        public GameObject CloseBtn;

        public GameObject NormalRewardPanel;
        public UILabel NormalRewardName;
        public GameObject BtnWriteUserInfo;

        public GameObject BigRewardPanel;
        public UILabel BigRewardName;
        public GameObject BtnShare;

        private AwardConfigData RewardInfo;

        public override void OnPushData(object[] data)
        {
            RewardInfo = (AwardConfigData)data[0];
            RewardIcon.spriteName = RewardInfo.ResUrl;
            if (RewardInfo.AwardType != (int)AwardType.RoomCard)
            {
                SetBigRewardData();
            }
            else
            {
                SetNormalRewardData();
            }
        }

        public override void Init()
        {
            UIEventListener.Get(CloseBtn).onClick = CloseBtnClick;
            UIEventListener.Get(BtnShare).onClick = OnClickShare;
            UIEventListener.Get(BtnWriteUserInfo).onClick = OnClickWriteUserInfo;
        }

        //普通奖励
        private void SetNormalRewardData()
        {
            NormalRewardPanel.SetActive(true);
            BigRewardPanel.SetActive(false);
            NormalRewardName.text = string.Format("{0} x{1}", RewardInfo.AwardName, RewardInfo.AwardCount);
        }

        //大奖励
        private void SetBigRewardData()
        {
            NormalRewardPanel.SetActive(false);
            BigRewardPanel.SetActive(true);
            BigRewardName.text = string.Format("恭喜您获得大奖\"{0}\"", RewardInfo.AwardName);
        }

        public override void OnHide() { }

        public override void OnShow() { }

        private void OnClickShare(GameObject go)
        {
            Texture2D tex2d = ResourcesDataLoader.Load<Texture2D>("Texture/Tex_Common/Tex_LotteryShare");
            string base64Str = Tools_TexScreenshot.Instance.Texture_LocalToBase64(tex2d);

            if (!string.IsNullOrEmpty(base64Str))
            {
                WXShareParams shareParams = new WXShareParams("WECHAT_SHARE_LOTTERY", base64Str);
                SDKManager.Instance.SDKFunction("WECHAT_SHARE_LOTTERY", shareParams);
            }
        }

        private void OnClickWriteUserInfo(GameObject go)
        {
            ModelNetWorker.Instance.C2SPickupAwardReq(RewardInfo.AwardID); //请求领取奖励

            WebSDKParams shareParams = new WebSDKParams("WEB_OPEN_LOTTERY");
            shareParams.InsertUrlParams(new object[] { RewardInfo.AwardID });
            SDKManager.Instance.SDKFunction("WEB_OPEN_LOTTERY", shareParams);

            this.Close();
        }

        /// <summary>
        /// 关闭按钮
        /// </summary>
        private void CloseBtnClick(GameObject go)
        {
            if(RewardInfo.AwardType == (int)AwardType.Object)
            {
                string[] btnNames = new string[2];
                btnNames[0] = "取消";
                btnNames[1] = "确定";
                LoadPop(WindowUIType.SystemPopupWindow, "提示", "是否放弃领取获得的奖品", btnNames, 
                    (int index) => 
                    {
                        if (index != 0)
                        {
                            this.Close();
                        }
                    }
                );
            }
            else
            {
                this.Close();
            }
        }

        protected override void OnClose()
        {
            base.OnClose();
        }

        private void CloseUI(object[] vars)
        {
            this.Close();
        }
    }
}