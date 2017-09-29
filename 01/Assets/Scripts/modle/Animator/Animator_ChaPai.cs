/**
* @Author lyb
* 插牌动画控制脚本
*
*/

using UnityEngine;

namespace projectQ
{
    public class Animator_ChaPai : BaseAnimator
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
        public void Init_Animator_ChaPai(Vector3 p0, Vector3 p1, GameObject mainObj, int extend)
        {
            this.extend = extend;

            HandMainScript = mainObj.GetComponent<Animator_Hand>();

            A_mation = gameObject.GetComponent<Animator>();

            A_mation.CrossFade("chapaiIdle", 0);

            StData = SetAnimatorData<AnimatorHandData>(AnimatorEnum.Anim_chapai, HandMainScript.AnimSex);
            if (StData != null)
            {
                gameObject.transform.localPosition = StData.StartV3;
                gameObject.transform.localScale = StData.ScaleV3;
            }

            this.p0 = p0;
            this.p1 = p1;

            Animator_MoveBegin(p0, StData.TimeMoveP0, AnimatorEventEnum.Anim_chapai_move1);
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
            if (eventEnum == AnimatorEventEnum.Anim_chapai_move1)
            {
                this.C2CAnimEventSend(AnimatorEventEnum.Anim_chapai_move1);

                QLoger.LOG(" #[动画-手插牌]# 移动到指定位置1结束 ");

                string eventStr = "Anim_chapai_motion1" + HandMainScript.HandIndex;

                //StartCoroutine(UITools.WaitExcution(Animator_EventFinish, 0.75f, eventStr));
                AnimTimeTick.Instance.SetAction(Animator_EventFinish, 0.75f, 1, eventStr);

                A_mation.CrossFade("chapai", 0);
            }
            else if (eventEnum == AnimatorEventEnum.Anim_chapai_up1)
            {
                Vector3 upV3 = new Vector3(p1.x, p1.y + StData.UpHight, p1.z);

                Animator_MoveBegin(upV3, StData.TimeMoveP1, AnimatorEventEnum.Anim_chapai_up2);
            }
            else if (eventEnum == AnimatorEventEnum.Anim_chapai_up2)
            {
                Animator_MoveBegin(p1, 0.2f, AnimatorEventEnum.Anim_chapai_move2);
            }
            else if (eventEnum == AnimatorEventEnum.Anim_chapai_move2)
            {
                this.C2CAnimEventSend(AnimatorEventEnum.Anim_chapai_move2);

                QLoger.LOG(" #[动画-手插牌]# 移动到指定位置2结束 ");

                //StartCoroutine(UITools.WaitExcution(AnimatorMoveInit_DelayPlay, StData.DelayMoveInitTime));
                AnimTimeTick.Instance.SetAction(AnimatorMoveInit_DelayPlay, StData.DelayMoveInitTime,1);


            }
            else if (eventEnum == AnimatorEventEnum.Anim_chapai_moveInit)
            {
                this.C2CAnimEventSend(AnimatorEventEnum.Anim_chapai_moveInit);

                QLoger.LOG(" #[动画-手插牌]# 移动到初始位置结束 ");
            }
        }

        /// <summary>
        /// 动画结束，往外发送事件
        /// </summary>
        void Animator_EventFinish(string str)
        {
            string eventStr = "Anim_chapai_motion1" + HandMainScript.HandIndex;

            if (str.Equals(eventStr))
            {
                this.C2CAnimEventSend(AnimatorEventEnum.Anim_chapai_motion1);

                QLoger.LOG(" #[动画-手插牌]# 手的动画播放完毕，回调事件！参数 = " + str);

                if (p0 != p1)
                {
                    //StartCoroutine(UITools.WaitExcution(Animator_DelayPlay, StData.DelayTime));
                    AnimTimeTick.Instance.SetAction(Animator_DelayPlay, StData.DelayTime,1);
                }
                else
                {
                    Animator_MoveEventFinish(AnimatorEventEnum.Anim_chapai_move2);
                }
            }
        }

        /// <summary>
        /// 到达P1前的延迟执行方法
        /// </summary>
        void Animator_DelayPlay(string param)
        {
            QLoger.LOG(" #[动画-手插牌]# 延迟完毕,抬起胳膊 ");

            Vector3 upV3 = new Vector3(p0.x, p0.y + StData.UpHight, p0.z);

            Animator_MoveBegin(upV3, 0.2f, AnimatorEventEnum.Anim_chapai_up1);
        }

        /// <summary>
        /// 回到初始位置前的延迟执行方法
        /// </summary>
        void AnimatorMoveInit_DelayPlay(string param)
        {
            QLoger.LOG(" #[动画-手插牌]# 回到初始位置前的延迟完毕,执行后续方法 ");

            Animator_Pos(HandMainScript.HandIndex);
        }

        /// <summary>
        /// 初始化位置
        /// </summary>
        void Animator_Pos(int index)
        {
            AnimatorHandPos.PosDataBase posData = Animator_PosInit(index);

            if (posData != null)
            {
                Animator_MoveBegin(posData.FromV3, StData.TimeMoveInit, AnimatorEventEnum.Anim_chapai_moveInit);

                //A_mation.CrossFade("chapaiIdle", 0);
            }
        }

        /// <summary>
        /// 发送事件
        /// </summary>
        void C2CAnimEventSend(AnimatorEventEnum eventEnum)
        {
            Transform tran = HandMainScript.Model_HandChaPoint;
            int index = HandMainScript.HandIndex;
            C2CAnimatorEventSend(eventEnum, tran, index, extend);
        }
    }
}