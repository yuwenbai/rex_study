/**
 * @Author lyb
 *  动画事件添加回调
 *
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace projectQ
{
    public class Animator_Event : MonoBehaviour
    {
        /// <summary>
        /// 动画
        /// </summary>
        private Animator A_mation;

        public void Awake()
        {
            A_mation = gameObject.GetComponent<Animator>();
        }

        /// <summary>
        /// 清除动画事件
        /// </summary>
        public void Animator_EventClear(string animName)
        {
            RuntimeAnimatorController m_AnimController = A_mation.runtimeAnimatorController;

            for (int i = 0; i < m_AnimController.animationClips.Length; i++)
            {
                if (m_AnimController.animationClips[i].name.Equals(animName))
                {
                    m_AnimController.animationClips[i].events = null;
                }
            }
        }

        /// <summary>
        /// 添加动画事件
        /// </summary>
        /// <param name="animTime">在哪个时间点添加事件</param>
        /// <param name="paramenterStr">事件结束传递的参数</param>
        public void Animator_EventAdd(float animTime, string paramenterStr, string animName)
        {
            if (A_mation == null)
            {
                return;
            }

            Animator_EventClear(animName);

            RuntimeAnimatorController m_AnimController = A_mation.runtimeAnimatorController;
            AnimationEvent aEvent = new AnimationEvent();

            aEvent.time = animTime;
            aEvent.functionName = "Animator_EventFinish";
            aEvent.stringParameter = paramenterStr;

            for (int i = 0; i < m_AnimController.animationClips.Length; i++)
            {
                if (m_AnimController.animationClips[i].name.Equals(animName))
                {
                    m_AnimController.animationClips[i].AddEvent(aEvent);
                }
            }
        }
    }
}