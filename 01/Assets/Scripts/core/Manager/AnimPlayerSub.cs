/**
 * @Author Xin.Wang
 * 麻将动画播放控制类
 *
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace projectQ
{
    public class AnimPlayerSub : MonoBehaviour
    {
        private class AnimPlayerControler
        {
            /// <summary>
            /// 动画控制器池
            /// </summary>
            private Queue<AnimPlayController> _controllerPool = new Queue<AnimPlayController>();

            /// <summary>
            /// 待播放堆栈动画
            /// </summary>
            private Queue<AnimPlayController> stackAnim = new Queue<AnimPlayController>();
            /// <summary>
            /// 正在播放的堆栈动画
            /// </summary>
            public AnimPlayController curPlayStackAnim = null;
            /// <summary>
            /// 正在播放的无堆栈动画
            /// </summary>
            public List<AnimPlayController> curPlayListAnimList = new List<AnimPlayController>();

            /// <summary>
            /// 总播放状态
            /// </summary>
            public bool _isPlaying
            {
                get
                {
                    bool state = false;
                    state = statckPlaying ||
                        stackAnim.Count != 0
                        || curPlayListAnimList.Count != 0;

                    return state;
                }
            }

            /// <summary>
            /// 堆栈是否在播放
            /// </summary>
            public bool statckPlaying
            {
                get
                {
                    return curPlayStackAnim != null;
                }
            }

            /// <summary>
            /// 动画播放总开关
            /// </summary>
            private bool lockPlay = false;

            /// <summary>
            /// 获取名字
            /// </summary>
            private int _curControlName = 0;
            public int GetCurControlName
            {
                get
                {
                    return _curControlName == int.MaxValue ? 0 : ++_curControlName;
                }
            }

            /// <summary>
            /// 获得控制组件
            /// </summary>
            /// <returns></returns>
            private AnimPlayController GetAController()
            {
                if (_controllerPool.Count == 0)
                {
                    return new AnimPlayController(GetCurControlName);
                }

                AnimPlayController curReturn = _controllerPool.Dequeue();
                curReturn.controllerName = GetCurControlName;
                return curReturn;
            }
            /// <summary>
            /// 归还组件
            /// </summary>
            /// <param name="controller"></param>
            public void ReturnAController(AnimPlayController controller)
            {
                if (_controllerPool.Count < 20)
                {
                    controller.ResetAnimController();
                    _controllerPool.Enqueue(controller);
                }
                else
                {
                    controller = null;
                }
            }


            /// <summary>
            /// 获取并且初始化组件
            /// </summary>
            /// <param name="playTime"></param>
            /// <param name="animName"></param>
            /// <param name="vars"></param>
            /// <returns></returns>
            private AnimPlayController GetAndIniControl(float playTime, string animName, object[] vars)
            {
                AnimPlayController curControl = GetAController();
                curControl.IniAnimController(playTime, animName, vars);
                return curControl;
            }

            /// <summary>
            /// 播放堆栈动画
            /// </summary>
            /// <param name="playTime"></param>
            /// <param name="animName"></param>
            /// <param name="vars"></param>
            public void PlayAnimSubByStack(float playTime, string animName, object[] vars)
            {
                AnimPlayController curControl = GetAndIniControl(playTime, animName, vars);
                stackAnim.Enqueue(curControl);
            }

            /// <summary>
            /// 播放不堆栈动画 
            /// </summary>
            /// <param name="playTime"></param>
            /// <param name="animName"></param>
            /// <param name="vars"></param>
            public void PlayAnimSubByList(float playTime, string animName, object[] vars)
            {
                AnimPlayController curControl = GetAndIniControl(playTime, animName, vars);
                curPlayListAnimList.Add(curControl);
            }


            public void ClearPlayState()
            {
                lockPlay = true;

                //清理栈
                stackAnim.Clear();
                curPlayStackAnim = null;
                //清理无栈
                curPlayListAnimList.Clear();

                lockPlay = false;
            }

            public void DoAnimRefresh(System.Action callBack)
            {
                if (lockPlay)
                {
                    return;
                }

                //检测堆栈
                if (!statckPlaying)
                {
                    if (stackAnim.Count > 0)
                    {
                        curPlayStackAnim = stackAnim.Dequeue();
                    }
                }
                else
                {
                    curPlayStackAnim.UpdateTick(Time.deltaTime);
                }

                if (callBack != null)
                {
                    callBack();
                }

            }
        }

        private System.Action mCallBackDelg = null;
        private void Awake()
        {
            mCallBackDelg = RefreshCallBack;
            _animEvent = new Dictionary<string, System.Action<string, object[]>>();
        }

        private IDictionary<string, System.Action<string, object[]>> _animEvent = null;
        public void RegisterAnimListener(string animName, System.Action<string, object[]> action)
        {
            if (_animEvent.ContainsKey(animName))
            {
                _animEvent[animName] -= action;
                _animEvent[animName] += action;
            }
            else
            {
                _animEvent.Add(animName, action);
            }

        }
        public void RemoveAnimListener(string animName, System.Action<string, object[]> action)
        {
            if (_animEvent.ContainsKey(animName))
            {
                _animEvent[animName] -= action;
                if (_animEvent[animName] == null)
                {
                    _animEvent.Remove(animName);
                }
            }
        }

        private AnimPlayerControler controlLogic = new AnimPlayerControler();

        public void PlayAnim(bool isLogic, float playTime, GEnum.NameAnim animName, params object[] vars)
        {
            PlayAnim(isLogic, playTime, animName.ToString(), vars);
        }

        public void PlayAnim(bool isInStack, float playTime, string animName, params object[] vars)
        {
            if (isInStack)
            {
                controlLogic.PlayAnimSubByStack(playTime, animName, vars);
            }
            else
            {
                controlLogic.PlayAnimSubByList(playTime, animName, vars);
            }
        }


        private void FireAnimPlay(AnimPlayController _curPlayController)
        {
            if (_curPlayController.hasFire)
            {
                return;
            }

            _curPlayController.hasFire = true;
            string animName = _curPlayController.animName;
            object[] vars = _curPlayController.parameters;

            if (_animEvent.ContainsKey(animName) && _animEvent[animName] != null)
            {
                _animEvent[animName](animName, vars);
            }
        }


        private void RefreshCallBack()
        {
            //检测压栈
            if (controlLogic.curPlayStackAnim != null)
            {
                FireAnimPlay(controlLogic.curPlayStackAnim);
                if (controlLogic.curPlayStackAnim.timeOver)
                {
                    controlLogic.ReturnAController(controlLogic.curPlayStackAnim);
                    controlLogic.curPlayStackAnim = null;
                }
            }

            //检测不压栈
            if (controlLogic.curPlayListAnimList != null && controlLogic.curPlayListAnimList.Count > 0)
            {
                for (int i = controlLogic.curPlayListAnimList.Count - 1; i >= 0; i--)
                {
                    AnimPlayController control = controlLogic.curPlayListAnimList[i];
                    FireAnimPlay(control);
                    controlLogic.ReturnAController(control);
                    controlLogic.curPlayListAnimList.RemoveAt(i);
                }
            }
        }

        public void ClearAnim()
        {
            controlLogic.ClearPlayState();
        }

        public void UpdateAnimData()
        {
           controlLogic.DoAnimRefresh(mCallBackDelg);//把函数指针转化成action委托时，会产生1k左右的gc，建议不要在update中直接使用函数指针作为委托传入
        }


        public bool GetAnimPlayState()
        {
            return controlLogic._isPlaying;
        }
    }

}
