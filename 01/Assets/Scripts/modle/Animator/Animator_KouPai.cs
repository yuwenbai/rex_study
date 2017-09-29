/**
* @Author lyb
* 扣牌动画控制脚本
*
*/

using UnityEngine;

namespace projectQ
{
    public class Animator_KouPai : BaseAnimator
    {
        /// <summary>
        /// 动画
        /// </summary>
        private Animator A_mation;
        /// <summary>
        /// 当前动画配置文件的索引
        /// </summary>
        private AnimatorHandData StData;

        private Vector3 p0;
        private Vector3 p1;

        /// <summary>
        /// 手动画的主脚本
        /// </summary>
        private Animator_Hand HandMainScript;
        /// <summary>
        /// 扩展参数存储
        /// </summary>
        private int extend;

        /// <summary>
        /// 初始化出牌动画数据
        /// </summary>
        public void Init_Animator_KouPai(Vector3 p0, Vector3 p1, GameObject mainObj, int extend)
        {
            this.extend = extend;

            HandMainScript = mainObj.GetComponent<Animator_Hand>();

            A_mation = gameObject.GetComponent<Animator>();

            A_mation.CrossFade("koupaiIdle", 0);

            StData = SetAnimatorData<AnimatorHandData>(AnimatorEnum.Anim_koupai, HandMainScript.AnimSex);
            if (StData != null)
            {
                gameObject.transform.localPosition = StData.StartV3;
                gameObject.transform.localScale = StData.ScaleV3;
            }

            this.p0 = p0;
            this.p1 = p1;

            Animator_MoveBegin(p0, StData.TimeMoveP0, AnimatorEventEnum.Anim_koupai_move1);
        }

        /// <summary>
        /// 进行移动
        /// </summary>
        void Animator_MoveBegin(Vector3 endPos, float mTime, AnimatorEventEnum eventEnum)
        {
            Animator_Move(HandMainScript.gameObject, mTime, endPos, eventEnum, Animator_MoveEventFinish);
        }

        /// <summary>
        /// 移动结束回调
        /// </summary>
        void Animator_MoveEventFinish(AnimatorEventEnum eventEnum)
        {
            if (eventEnum == AnimatorEventEnum.Anim_koupai_move1)
            {
                this.C2CAnimEventSend(AnimatorEventEnum.Anim_koupai_move1);

                QLoger.LOG(" #[动画-手扣牌]# 移动到指定位置1结束 ");

                string eventStr = "Anim_koupai_motion1" + HandMainScript.HandIndex;

                //Animator_EventAdd(0.26f, eventStr, "koupai");

                //StartCoroutine(UITools.WaitExcution(Animator_EventFinish, 0.26f, eventStr));
                AnimTimeTick.Instance.SetAction(Animator_EventFinish,0.26f,1,eventStr);

                A_mation.CrossFade("koupai", 0);
            }
            else if (eventEnum == AnimatorEventEnum.Anim_koupai_move2)
            {
                this.C2CAnimEventSend(AnimatorEventEnum.Anim_koupai_move2);

                QLoger.LOG(" #[动画-手扣牌]# 移动到指定位置2结束 ");

                Animator_Pos(HandMainScript.HandIndex);
            }
            else if (eventEnum == AnimatorEventEnum.Anim_koupai_moveInit)
            {
                this.C2CAnimEventSend(AnimatorEventEnum.Anim_koupai_moveInit);

                QLoger.LOG(" #[动画-手扣牌]# 移动到初始位置结束 ");
            }
        }

        /// <summary>
        /// 动画结束，往外发送事件
        /// </summary>
        void Animator_EventFinish(string str)
        {
            string eventStr = "Anim_koupai_motion1" + HandMainScript.HandIndex;

            if (str.Equals(eventStr))
            {
                this.C2CAnimEventSend(AnimatorEventEnum.Anim_koupai_motion1);

                QLoger.LOG(" #[动画-手扣牌]# 手的动画播放完毕，回调事件！参数 = " + str);

                if (p0 != p1)
                {
                    //StartCoroutine(UITools.WaitExcution(Animator_DelayPlay, StData.DelayTime));
                    AnimTimeTick.Instance.SetAction(Animator_DelayPlay, StData.DelayTime,1);
                }
                else
                {
                    Animator_MoveEventFinish(AnimatorEventEnum.Anim_koupai_move2);
                }
            }
        }

        /// <summary>
        /// 延迟执行方法
        /// </summary>
        void Animator_DelayPlay(string param)
        {
            QLoger.LOG(" #[动画-手扣牌]# 延迟完毕,执行后续方法 ");

            Animator_MoveBegin(p1, StData.TimeMoveP1, AnimatorEventEnum.Anim_koupai_move2);
        }

        /// <summary>
        /// 初始化位置
        /// </summary>
        void Animator_Pos(int index)
        {
            AnimatorHandPos.PosDataBase posData = Animator_PosInit(index);

            if (posData != null)
            {
                Animator_MoveBegin(posData.FromV3, 0.5f, AnimatorEventEnum.Anim_koupai_moveInit);
            }
        }

        /// <summary>
        /// 发送事件
        /// </summary>
        void C2CAnimEventSend(AnimatorEventEnum eventEnum)
        {
            Transform tran = HandMainScript.Model_HandPoint;
            int index = HandMainScript.HandIndex;
            C2CAnimatorEventSend(eventEnum, tran, index, extend);
        }
    }
}