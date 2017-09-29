/**
* @Author lyb
* 转盘抽奖
*
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace projectQ
{
    public class UILottery : MonoBehaviour
    {
        /// <summary>
        /// 抽奖按钮
        /// </summary>
        public GameObject Lottery_Btn;
        /// <summary>
        /// 控制旋转的gameobject
        /// </summary>
        public GameObject Lottery_Target;
        /// <summary>
        /// 旋转的时间
        /// </summary>
        public float Lottery_Time = 5.0f;
        /// <summary>
        /// 目标块 编号 - 抽奖分为6块每块30度
        /// </summary>
        public int Lottery_index = 3;

        public void Start()
        {
            UIEventListener.Get(Lottery_Btn).onClick = OnAwardBtnClick;

            Lottery_Target.transform.localRotation = Quaternion.Euler(Vector3.zero);
        }

        #region 按钮回调方法-------------------------------

        /// <summary>
        /// 抽奖按钮点击
        /// </summary>
        void OnAwardBtnClick(GameObject go)
        {
            Lottery_RotationBegin();
        }

        #endregion-----------------------------------------

        #region 转盘旋转-----------------------------------

        void Lottery_RotationBegin()
        {
            int angle = 0 - (Lottery_index * 60);

            TweenRotation twr = Lottery_Target.GetComponent<TweenRotation>();
            if (twr == null)
            {
                twr = Lottery_Target.AddComponent<TweenRotation>();
            }

            twr.from = new Vector3(0, 0, Lottery_Target.transform.localEulerAngles.z);
            twr.to = new Vector3(0, 0, -1800 + angle);
            twr.duration = Lottery_Time;
            //twr.callWhenFinished = "DesOneTween";
            twr.onFinished.Add(new EventDelegate(TweenFinish));
            AnimationCurveAsset asset = GameAssetCache.LoadAnimationCurve(GameAssetCache.LotteryAnimation);
            twr.animationCurve = asset.Curve;
            twr.Play(true);
        }

        void TweenFinish()
        {
            QLoger.LOG(" #[转盘动画]# -- 动画播放完毕");
        }

        #endregion-----------------------------------------
    }
}