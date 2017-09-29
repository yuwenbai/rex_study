/**
 * @Author Xin.Wang
 *
 *
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace projectQ
{
    public class AnimPlayController
    {
        public float playTime = 0;
        public string animName;
        public object[] parameters;

        public int controllerName = 0;
        public float tickTime = 0f;
        public bool hasFire = false;
        public bool timeOver = false;

        public AnimPlayController(float playTime, string animName, object[] vars)
        {
            IniAnimController(playTime, animName, vars);
        }
        public AnimPlayController(int name)
        {
            controllerName = name;
        }

        public void IniAnimController(float playTime, string animName, object[] vars)
        {
            this.playTime = playTime;
            this.animName = animName;
            this.parameters = vars;
        }

        public void UpdateTick(float timeDetal)
        {
            tickTime += timeDetal;
            timeOver = tickTime >= playTime;
        }

        public void ResetAnimController()
        {
            hasFire = false;
            tickTime = 0f;
            timeOver = false;
            controllerName = 0;
            playTime = 0;
            animName = null;
            parameters = null;
        }

    }


    public class AnimPlayManager : MonoBehaviour
    {
        #region single

        private static AnimPlayManager _instance;
        public static AnimPlayManager Instance
        {
            get { return _instance; }
        }

        #endregion

        private const int ANIMSUB_MAX = 5;
        private AnimPlayerSub[] _animSub = new AnimPlayerSub[ANIMSUB_MAX];

        //播放动画开关
        public bool canPlayAnim = false;
        public bool waitSubStop = false;

        private IDictionary<string, System.Action<object[]>> _animEvent = null;

        public void RegisterAnimListener(GEnum.NameAnim animName, System.Action<object[]> action)
        {
            RegisterAnimListener(animName.ToString(), action);
        }

        public void RegisterAnimListener(string animName, System.Action<object[]> action)
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

            for (int i = 0; i < _animSub.Length; i++)
            {
                if (_animSub[i] != null)
                {
                    _animSub[i].RegisterAnimListener(animName, SubAnimPlayCall);
                }
            }

        }


        public void RemoveAnimListener(GEnum.NameAnim animName, System.Action<object[]> action)
        {
            RemoveAnimListener(animName.ToString(), action);
        }

        public void RemoveAnimListener(string animName, System.Action<object[]> action)
        {
            if (_animEvent.ContainsKey(animName))
            {
                _animEvent[animName] -= action;
                if (_animEvent[animName] == null)
                {
                    _animEvent.Remove(animName);
                }
            }

            for (int i = 0; i < _animSub.Length; i++)
            {
                if (_animSub[i] != null)
                {
                    _animSub[i].RemoveAnimListener(animName, SubAnimPlayCall);
                }
            }
        }


        private void SubAnimPlayCall(string animName, object[] vars)
        {
            if (_animEvent.ContainsKey(animName) && _animEvent[animName] != null)
            {
                _animEvent[animName](vars);
            }
        }

        /// <summary>
        /// 播放个人动画操作逻辑
        /// </summary>
        /// <param name="seatID"></param>
        /// <param name="playTime"></param>
        /// <param name="animName"></param>
        /// <param name="vars"></param>
        public void PlayAnim(int seatID, float playTime, GEnum.NameAnim animName, params object[] vars)
        {
            PlayAnim(true, seatID, playTime, animName.ToString(), vars);
        }

        public void PlayAnim(int seatID, float playTime, string animName, params object[] vars)
        {
            PlayAnim(true, seatID, playTime, animName, vars);
        }

        public void PlayAnimFx(int seatID, float playTime, GEnum.NameAnim animName, params object[] vars)
        {
            PlayAnim(false, seatID, playTime, animName.ToString(), vars);
        }

        public void PlayAnimFx(int seatID, float playTime, string animName, params object[] vars)
        {
            PlayAnim(false, seatID, playTime, animName, vars);
        }

        public void PlayAnim(bool isInStack, int seatID, float playTime, string animName, params object[] vars)
        {
            if (seatID > 0 && seatID < 5)
            {
                _animSub[seatID].PlayAnim(isInStack, playTime, animName, vars);
            }
        }


        public void PlayAnimCenter(float playTime, GEnum.NameAnim animName, params object[] vars)
        {
            PlayAnimCenter(playTime, animName.ToString(), vars);
        }

        public void PlayAnimCenter(float playTime, string animName, params object[] vars)
        {
            _animSub[0].PlayAnim(true, playTime, animName, vars);
        }


        public void ClearAllAnim()
        {
            for (int i = 0; i < _animSub.Length; i++)
            {
                if (_animSub[i] != null)
                {
                    _animSub[i].ClearAnim();
                }
            }
        }

        #region LifiCycle 

        // Use this for per initialization
        void Awake()
        {
            _instance = this;
            _animEvent = new Dictionary<string, System.Action<object[]>>();
            //create

            GameObject objP = new GameObject("MahjongAnimManager");
            Transform transPa = objP.transform;
            transPa.SetParent(this.transform);
            for (int i = 0; i < 5; i++)
            {
                string nameStr = i == 0 ? "AnimPlayCenter" : "AnimPlaySub" + i;
                GameObject obj = new GameObject(nameStr);
                _animSub[i] = obj.AddComponent<AnimPlayerSub>();
                obj.transform.SetParent(transPa);
            }

            DG.Tweening.DOTween.Init();
            DG.Tweening.DOTween.defaultUpdateType = DG.Tweening.UpdateType.Fixed;
        }

        // Update is called once per frame
        void FixedUpdate()
        {
            if (!canPlayAnim)
            {
                return;
            }

            if (waitSubStop)
            {
                bool stillWait = false;
                for (int i = 1; i < ANIMSUB_MAX; i++)
                {
                    bool isPlaying = _animSub[i].GetAnimPlayState();
                    if (isPlaying)
                    {
                        stillWait = true;
                        break;
                    }
                }

                waitSubStop = stillWait;
            }

            //sub 多余的判断 animsub 在此处必定有值
            /* if (_animSub != null)
             {

             }*/
            for (int i = 0; i < ANIMSUB_MAX; i++)
            {
                if (_animSub[i] == null || i == 0 && waitSubStop) continue;
                _animSub[i].UpdateAnimData();
            }

        }

        #endregion //LifiCycle 


    }
}

