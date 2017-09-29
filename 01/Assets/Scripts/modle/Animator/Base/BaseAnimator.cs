

using DG.Tweening;
/**
* @Author lyb
* 动画控制基类
*
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace projectQ
{
    public abstract class BaseAnimator : MonoBehaviour
    {
        #region lyb----------------初始化位置----------------------------

        /// <summary>
        /// 4个位置的哪个位置
        /// </summary>
        public AnimatorHandPos.PosDataBase Animator_PosInit(int index)
        {
            string animSetpath = "Model/HandPos";

            AnimatorHandPos StPosData = Resources.Load<AnimatorHandPos>(animSetpath);

            if (StPosData != null)
            {
                return StPosData.HandPosList[index];
            }

            return null;
        }

        #endregion ------------------------------------------------------

        #region lyb----------------动画移动------------------------------

        /// <summary>
        /// 播放动画的同时进行移动
        /// </summary>
        public void Animator_Move(GameObject obj, float moveTime, Vector3 endPos, AnimatorEventEnum eventEnum, System.Action<AnimatorEventEnum> aCallBack)
        {
            //GameObject xx = GameObject.Find("xxxxxx");
            //xx.transform.position = endPos;

            DOTween.Kill(obj, false);

            Tweener tweener = obj.transform.DOMove(endPos, moveTime);

            //设置这个Tween不受Time.scale影响
            tweener.SetUpdate(true);

            //设置移动类型
            tweener.SetEase(Ease.Linear);

            tweener.OnComplete(() =>
            {
                aCallBack(eventEnum);
            });
        }

        #endregion ------------------------------------------------------

        #region lyb----------------发送事件出去--------------------------

        public void C2CAnimatorEventSend(AnimatorEventEnum eventEnum, Transform tran, int index, int extend)
        {
            EventDispatcher.FireEvent(MJEnum.HandModelEvent.HME_Event.ToString(), eventEnum, tran, index, extend);
        }

        #endregion ------------------------------------------------------

        #region lyb----------------添加动画事件--------------------------

        /// <summary>
        /// 添加事件
        /// </summary>
        public void Animator_EventAdd(float animTime, string paramenterStr, string animName)
        {
            QLoger.LOG(" #[手动画]# 添加动画事件 = " + animName);

            Animator_Event eventBack = gameObject.GetComponent<Animator_Event>();
            eventBack.Animator_EventAdd(animTime, paramenterStr, animName);
        }

        #endregion ------------------------------------------------------

        #region lyb----------------获取动画名字--------------------------

        /// <summary>
        /// 获取该动画配置文件的名字
        /// </summary>
        public T SetAnimatorData<T>(AnimatorEnum animEnum, SexEnum AnimSex) where T : Object
        {
            string animSetpath = "";

            switch (animEnum)
            {
                case AnimatorEnum.Anim_chipenggang:
                    if (AnimSex == SexEnum.Man)
                    {
                        animSetpath = "Model/Model_Man/Man_chipenggang";
                    }
                    else
                    {
                        animSetpath = "Model/Model_WoMan/WoMan_chipenggang";
                    }
                    break;
                case AnimatorEnum.Anim_chupai:
                    if (AnimSex == SexEnum.Man)
                    {
                        animSetpath = "Model/Model_Man/Man_chupai";
                    }
                    else
                    {
                        animSetpath = "Model/Model_WoMan/WoMan_chupai";
                    }
                    break;
                case AnimatorEnum.Anim_koupai:
                    if (AnimSex == SexEnum.Man)
                    {
                        animSetpath = "Model/Model_Man/Man_koupai";
                    }
                    else
                    {
                        animSetpath = "Model/Model_WoMan/WoMan_koupai";
                    }
                    break;
                case AnimatorEnum.Anim_tuipai:
                    if (AnimSex == SexEnum.Man)
                    {
                        animSetpath = "Model/Model_Man/Man_tuipai";
                    }
                    else
                    {
                        animSetpath = "Model/Model_WoMan/WoMan_tuipai";
                    }
                    break;
                case AnimatorEnum.Anim_chapai:
                    if (AnimSex == SexEnum.Man)
                    {
                        animSetpath = "Model/Model_Man/Man_chapai";
                    }
                    else
                    {
                        animSetpath = "Model/Model_WoMan/WoMan_chapai";
                    }
                    break;
                case AnimatorEnum.Anim_dianji:
                    if (AnimSex == SexEnum.Man)
                    {
                        animSetpath = "Model/Model_Man/Man_dianji";
                    }
                    else
                    {
                        animSetpath = "Model/Model_WoMan/WoMan_dianji";
                    }
                    break;
                case AnimatorEnum.Anim_zhuapai:
                    if (AnimSex == SexEnum.Man)
                    {
                        animSetpath = "Model/Model_Man/Man_zhuapai";
                    }
                    else
                    {
                        animSetpath = "Model/Model_WoMan/WoMan_zhuapai";
                    }
                    break;
                case AnimatorEnum.Anim_zhengli:
                    if (AnimSex == SexEnum.Man)
                    {
                        animSetpath = "Model/Model_Man/Man_zhengli";
                    }
                    else
                    {
                        animSetpath = "Model/Model_WoMan/WoMan_zhengli";
                    }
                    break;
            }

            T StData = Resources.Load<T>(animSetpath);

            return StData;
        }

        #endregion ------------------------------------------------------
    }
}