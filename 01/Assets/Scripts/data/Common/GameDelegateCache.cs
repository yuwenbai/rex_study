/****************************************************
*
*  游戏全局变量定义
*  作者: lyb
*  日期：2017年7月25日
*
*****************************************************/

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace projectQ
{
    public class GameDelegateCache : SingletonTamplate<GameDelegateCache>
    {
        #region 全局定时器。。每帧执行一次

        public delegate void OnAddPerInvokeMethodEvent();
        public event OnAddPerInvokeMethodEvent InvokeMethodEvent;
        /// <summary>
        /// 全局定时器。。每帧执行一次
        /// </summary>
        public static void C2CRunPerInvokeMethodEvent()
        {
            if (GameDelegateCache.Instance.InvokeMethodEvent != null)
                GameDelegateCache.Instance.InvokeMethodEvent();
        }

        #endregion

        #region 音乐停止事件相应

        public delegate void OnMusicStopDelegate(string name);
        public event OnMusicStopDelegate MusicStopDelegate;
        /// <summary>
        /// 音乐停止事件相应
        /// </summary>
        public static void C2CMusicStopDelegate(string name)
        {
            if (GameDelegateCache.Instance.MusicStopDelegate != null)
            {
                GameDelegateCache.Instance.MusicStopDelegate(name);
            }
        }

        #endregion

        #region 音乐停止事件相应

        public delegate void OnSetEffectStateEvent();
        public event OnSetEffectStateEvent SetEffectStateEvent;
        /// <summary>
        /// 特效状态显示控制
        /// </summary>
        public static void C2CSetEffectStateEvent()
        {
            if (GameDelegateCache.Instance.SetEffectStateEvent != null)
            {
                GameDelegateCache.Instance.SetEffectStateEvent();
            }                
        }

        #endregion
    }
}