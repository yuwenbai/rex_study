

using System;
/**
* @Author jl
* 转盘抽奖
*
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace projectQ
{
    public enum AwardType
    {
        RoomCard = 1, //房卡
        Object = 2, //实物
    }

    public class UIActivityLottery : UIViewBase
    {
        public UIActivityLotteryModel Model
        {
            get { return _model as UIActivityLotteryModel; }
        }

        public GameObject BtnClose;
        public GameObject BtnRule;
        public GameObject BtnLottery;
        public GameObject RotateObj;

        public GameObject RewardItem;

        public UISprite LotterySprite;

        public GameObject RewardPanel;

        public GameObject Effect; //中奖特效
        public Animation LightEffect; //灯光特效(三种特效)

        private float RotateTime = 5.0f; //持续时间
        private int RewardIndex;  //目标索引
        private int RewardCount = 12; //奖励数量

        private bool IsRotate = false; //是否在旋转中
        private bool IsAdded = false;

        private List<AwardConfigData> RewardList;

        public override void Init()
        {
            Effect.SetActive(false);

            UIEventListener.Get(BtnLottery).onClick = OnClickLottery; 
            UIEventListener.Get(BtnRule).onClick = OnClickRule;

            UIEventListener.Get(BtnClose).onClick = OnClickClose;
        }

        public override void OnShow()
        {
            ModelNetWorker.Instance.C2SAwardConfigReq(Msg.LotteryTypeDef.Lottery_Game); //请求抽奖配置数据
        }

        public void SetData(List<AwardConfigData> rewardList)
        {
            RewardList = rewardList;

            int rewardDataCount = RewardList.Count;

            int angle = 0;

            for (int i = 0; i < RewardCount; i++)
            {
                GameObject rewardObj = UITools.CloneObject(RewardItem, RewardPanel);
                Transform trans = rewardObj.transform;
                trans.localRotation = Quaternion.AngleAxis(angle, Vector3.forward);
                if (trans != null)
                {
                    if(i < rewardDataCount)
                    {
                        AwardConfigData rewardData = RewardList[i];
                        trans.FindChild("Icon").GetComponent<UISprite>().spriteName = rewardData.ResUrl;
                        trans.FindChild("Name").GetComponent<UILabel>().text = rewardData.AwardName;
                        trans.FindChild("Count").GetComponent<UILabel>().text = rewardData.AwardCount > 1 ? "X" + rewardData.AwardCount : "";
                    }
                    else
                    {
                        trans.FindChild("Name").GetComponent<UILabel>().text = "无奖励";
                        trans.FindChild("Count").GetComponent<UILabel>().text = "";
                    }
                }
                angle -= 360/ RewardCount;
            }

            RewardItem.SetActive(false);

            LightEffect.Play("eff_UIActivityLottery_LightStandby_ani");

            RefreshUI();
        }

        /// <summary>
        /// 刷新界面
        /// </summary>
        public void RefreshUI()
        {
            UIWidget widget = LotterySprite.GetComponent<UIWidget>();
            if (MemoryData.PlayerData.MyPlayerModel.PlayerDataBase.lotteryTimes == 0)
            {
                LotterySprite.spriteName = "lottery_button_01";
                widget.width = 72;
                widget.height = 74;
                BtnLottery.GetComponent<UIDefinedButton>().enabled = true;
            }
            else
            {
                LotterySprite.spriteName = "lottery_Button_cj2";
                widget.width = 106;
                widget.height = 36;
                BtnLottery.GetComponent<UIDefinedButton>().enabled = false;
            }
            LotterySprite.transform.localPosition = Vector3.zero;
        }

        /// <summary>
        /// 抽奖按钮点击
        /// </summary>
        void OnClickLottery(GameObject go)
        {
            if (IsRotate || MemoryData.PlayerData.MyPlayerModel.PlayerDataBase.lotteryTimes != 0) return;

            IsRotate = true;
            BtnLottery.GetComponent<Animation>().Play();
            BtnLottery.GetComponent<UIDefinedButton>().enabled = false;

            ModelNetWorker.Instance.C2SLotteryReq(Msg.LotteryTypeDef.Lottery_Game); //请求抽奖结果
        }

        /// <summary>
        /// 打开活动规则界面
        /// </summary>
        void OnClickRule(GameObject go)
        {
            LoadUIMain("UIActivityLotteryRule");
        }

        /// <summary>
        /// 开始抽奖
        /// </summary>
        public void Lottery_RotationBegin(int index)
        {
            LightEffect.Play("eff_UIActivityLottery_LightStart_ani");

            RewardIndex = index;

            int angle = 0 - (index * (360/RewardCount));


            TweenRotation twr = RotateObj.GetComponent<TweenRotation>();
            if (twr == null)
            {
                twr = RotateObj.AddComponent<TweenRotation>();
            }
            
            AnimationCurveAsset asset = GameAssetCache.LoadAnimationCurve(GameAssetCache.LotteryAnimation);
            twr.animationCurve = asset.Curve;

            twr.ResetToBeginning();
            twr.from = new Vector3(0, 0, RotateObj.transform.localEulerAngles.z);
            twr.to = new Vector3(0, 0, -1800 + angle);
            twr.duration = RotateTime;

            if (!IsAdded)
            {
                twr.onFinished.Add(new EventDelegate(TweenFinish));
            }

            twr.Play(true);

            IsAdded = true;
        }

        /// <summary>
        /// 动画回调
        /// </summary>
        private void TweenFinish()
        {
            RefreshUI();

            LightEffect.Play("eff_UIActivityLottery_LightEnd_ani");

            int angle = 0 - (RewardIndex * (360 / RewardCount));
            Effect.transform.localRotation = Quaternion.AngleAxis(angle, Vector3.forward); //特效
            Effect.SetActive(true);

            AwardConfigData rewardInfo = MemoryData.SysActivityData.GetAwardList()[RewardIndex];
            if (rewardInfo.AwardType == (int)AwardType.RoomCard)
            {
                ModelNetWorker.Instance.C2SPickupAwardReq(rewardInfo.AwardID); //请求领取奖励
            }

            StartCoroutine(UITools.WaitExcution(LoadRewardUI, 1.7f));
        }

        /// <summary>
        /// 打开奖励获得界面
        /// </summary>
        private void LoadRewardUI()
        {
            IsRotate = false;
            LoadUIMain("UIActivityLotteryReward", MemoryData.SysActivityData.GetAwardList()[RewardIndex]);
        }

        private void OnClickClose(GameObject go)
        {
            if (IsRotate)
            {
                LoadTip("正在抽奖中");
            }
            else
            {
                this.Close();
            }
        }

        public override void OnHide()
        {
        }
        
    }
}