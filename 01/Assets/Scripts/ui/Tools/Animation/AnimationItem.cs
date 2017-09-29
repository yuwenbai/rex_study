/**
 * @Author YQC
 *
 *
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AnimationOrTween;

namespace projectQ.animation
{
    public class AnimationItemData
    {
        public AnimationItem item;
        public string groupName;
        public bool forward;
        public AnimationItem.OnItemFinishedDelegate func;

        //public void OnFunction()
        //{
        //    if (func != null)
        //        func();
        //}
    }
    [AddComponentMenu("工具/动画组件/AnimationItem")]
    public class AnimationItem : MonoBehaviour
    {
        public delegate void OnFinishedDelegate();
        public delegate void OnItemFinishedDelegate(bool isOk);

        public static List<AnimationItemData> AnimationQueue = new List<AnimationItemData>();
        public static Dictionary<string, List<AnimationItem>> AnimationItemAll = new Dictionary<string, List<AnimationItem>>();
        //动画是否开启
        public static AnimationItemData CurrExcute = null;
        public static OnFinishedDelegate OnAnimationFinished;
        //private Direction CurrDirStatus;


        [Tooltip("动画路径")]
        public string AnimationPath;
        [Tooltip("分组名称")]
        public string GroupName;
        [Tooltip("是否自动播放")]
        public bool IsAutoPlay;
        [Tooltip("是否自动倒播")]
        public bool IsAutoRever;

        //动画名称
        private string AnimationName;
        //动画组件
        private Animation Anim;

        public OnItemFinishedDelegate onFinished;

        #region Static
        public static bool PlayAnimUI(string groupName, bool forward, OnItemFinishedDelegate func)
        {
            bool isPlay = false;
            if (AnimationItem.AnimationItemAll.ContainsKey(groupName))
            {
                var animItems = AnimationItem.AnimationItemAll[groupName];

                //判断是否需要执行动画
                for (int i = 0; i < animItems.Count; ++i)
                {
                    if (animItems[i].CheckPlay(forward))
                    {
                        isPlay = true;
                    }
                }

                if (isPlay)
                {
                    AnimationItemData data = new AnimationItemData();
                    data.func = func;
                    data.forward = forward;
                    data.groupName = groupName;

                    if(CheackRepetition(data))
                    {
                        if(data.func != null)
                            data.func(true);
                    }
                    else
                    {
                        if (CurrExcute != null)
                        {
                            AnimationQueue.Add(data);
                        }
                        else
                        {
                            Play(data);
                        }
                    }
                }
            }

            if (!isPlay && func != null)
                func(true);
            return isPlay;
        }

        public static void RemoveAnim(string groupName)
        {
            if (CurrExcute != null && CurrExcute.groupName == groupName)
            {
                if(CurrExcute.item != null)
                {
                    if(CurrExcute.item.Anim != null)
                    {
                        CurrExcute.item.Anim.Stop();
                    }
                    else
                    {
                        if (CurrExcute.func != null)
                            CurrExcute.func(true);
                    }
                }
                CurrExcute = null;
            }
            var temp = AnimationQueue.FindAll((find) => {
                return find.groupName == groupName;
            });
            for (int i = 0; i < temp.Count; i++)
            {
                AnimationQueue.Remove(temp[i]);
            }

        }
        private static bool CheackRepetition(AnimationItemData data)
        {
            if(CurrExcute != null)
            {
                if (CurrExcute.forward == data.forward && CurrExcute.groupName == data.groupName)
                    return true;
            }
            if(AnimationQueue != null && AnimationQueue.Count > 0)
            {
                foreach (var item in AnimationQueue)
                {
                    if (CurrExcute.forward == data.forward && CurrExcute.groupName == data.groupName)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        
        //public static bool PlayAnim(AnimationItem ai,bool forward,System.Action func)
        //{
        //    AnimationItemData data = new AnimationItemData();
        //    data.func = func;
        //    data.forward = forward;
        //    data.item = ai;

        //    //if (CurrExcute != null)
        //    //{
        //    //    AnimationQueue.Enqueue(data);
        //    //}
        //    //else
        //    //{
        //    Play(data);
        //    //}

        //    return true;
        //}

        private static void Play(AnimationItemData data)
        {
            CurrExcute = data;
            List<AnimationItem> aniItemList;
            if (data.item == null)
            {
                aniItemList = AnimationItem.AnimationItemAll[data.groupName];
            }
            else
            {
                aniItemList = new List<AnimationItem>() { data.item };
            }
            float animLength = 0f;
            AnimationItem timeMaxItem = null;


            List<AnimationItem> playList = new List<AnimationItem>();
            for (int i=0; i<aniItemList.Count; ++i)
            {
                AnimationState animState = aniItemList[i].Anim[aniItemList[i].AnimationName];
                if (aniItemList[i].CheckPlay(data.forward))
                {
                    playList.Add(aniItemList[i]);
                    if (animState.length > animLength)
                    {
                        animLength = animState.length;
                        timeMaxItem = aniItemList[i];
                    }
                }
            }

            if(playList.Count > 0)
            {
                for (int i=0; i< playList.Count; ++i)
                {
                    if (playList[i] == timeMaxItem)
                    {
                        playList[i].Play(data.forward, PlayEnd);
                    }
                    else
                    {
                        playList[i].Play(data.forward, null);
                    }
                }
            }
            else
            {
                PlayEnd(true);
            }
        }

        private static void PlayEnd(bool isOk)
        {
            if(CurrExcute != null && CurrExcute.func != null)
            {
                CurrExcute.func(isOk);
            }
            CurrExcute = null;

            if (AnimationQueue.Count > 0)
            {
                var item = AnimationQueue[0];
                AnimationQueue.RemoveAt(0);
                Play(item);
            }
            else
            {
                if(OnAnimationFinished != null)
                    OnAnimationFinished();
                OnAnimationFinished = null;
            }
        }

        #endregion

        #region API
        public bool IsPlaying = false;
        /// <summary>
        /// 播放
        /// </summary>
        /// <param name="isFr"></param>
        public bool Play(bool forward,OnItemFinishedDelegate onFinished)
        {
            if (Anim != null)
            {
                IsPlaying = true;
                //CurrDirStatus = forward ? Direction.Forward : Direction.Reverse;
                this.onFinished = onFinished;
                this.AddEvent(forward);
                this.Play(forward);
                return true;
            }
            else
            {
                if (onFinished != null)
                {
                    onFinished(true);
                }
                this.onFinished = null;
            }
            return false;
        }

        /// <summary>
        /// 取得小动画
        /// </summary>
        /// <param name="clipName"></param>
        /// <returns></returns>
        public AnimationState GetAnimationState(string clipName)
        {
            return Anim[clipName];
        }

        public bool CheckPlay(bool forward)
        {
            return (this.isActiveAndEnabled && (forward && this.IsAutoPlay) || (!forward && this.IsAutoRever));
        }

        #endregion
        /// <summary>
        /// 运行
        /// </summary>
        /// <param name="forward"></param>
        private void Play(bool forward)
        {
            if (Anim == null) return;
            foreach (AnimationState state in Anim)
            {
                if (string.IsNullOrEmpty(AnimationName) || state.name == AnimationName)
                {
                    float speed = Mathf.Abs(state.speed);
                    state.speed = speed * (forward ? 1 : -1);

                    if(forward)
                    {
                        state.time = 0f;
                    }
                    else
                    {
                        state.time = state.length;
                    }
                }
            }

            Anim.Play();
        }
        /// <summary>
        /// 添加一个事件
        /// </summary>
        /// <param name="forward"></param>
        private void AddEvent(bool forward)
        {
            ClearEvent();
            if(Anim != null)
            {
                AnimationEvent evt = new AnimationEvent();
                evt.time = forward ? Anim[AnimationName].length : 0;
                evt.functionName = "OnAnimationItemFinished";
                evt.stringParameter = GetInstanceID().ToString();
                Anim.GetClip(AnimationName).AddEvent(evt);
            }
        }
        /// <summary>
        /// 清除事件
        /// </summary>
        private void ClearEvent()
        {
            if(Anim != null && Anim.GetClip(AnimationName) != null)
            {
                Anim.GetClip(AnimationName).events = null;
            }
            //AnimationEvent[] events = Anim.GetClip(AnimationName).events;
            //Anim.GetClip(AnimationName).
            //for(int i=0; i<events.Length; ++i)
            //{
            //    if(events[i].stringParameter == GetInstanceID().ToString())
            //    {
            //        events[i] = null;
            //    }
            //}
        }


        /// <summary>
        /// 默认GroupName
        /// </summary>
        private string GetDefaultGroupName()
        {
            UIViewBase ui = transform.GetComponentInParent<UIViewBase>();
            if (ui != null)
                return ui.name;
            return string.Empty;
        }

        #region 生命周期
        private void Awake()
        {
            //设置默认名字
            if(string.IsNullOrEmpty(GroupName))
                GroupName = this.GetDefaultGroupName();



            if (string.IsNullOrEmpty(AnimationPath)) return;
            AnimationName = AnimationPath.Substring(AnimationPath.LastIndexOf("/") + 1);

            AnimationClip clip = ResourcesDataLoader.Load<AnimationClip>(AnimationPath);
            if(clip == null)
            {
                QLoger.ERROR("动画文件找不到" + AnimationPath);
                return;
            }
            AnimationClip clip2 = AnimationClip.Instantiate(clip);
            clip2.name = AnimationName;
            Anim = gameObject.AddComponent<Animation>();
            Anim.AddClip(clip2, AnimationName);
            Anim.clip = clip2;

            if(!string.IsNullOrEmpty(GroupName))
            {
                if (!AnimationItemAll.ContainsKey(GroupName))
                {
                    AnimationItemAll.Add(GroupName, new List<AnimationItem>());
                }
                if (AnimationItemAll[GroupName].IndexOf(this) == -1)
                {
                    AnimationItemAll[GroupName].Add(this);
                }
            }

        }
        private void OnDisable()
        {
            transform.localScale = Vector3.one;
        }
        private void OnDestroy()
        {
            ClearEvent();
            if (AnimationItemAll.ContainsKey(GroupName) && AnimationItemAll[GroupName].IndexOf(this) != -1)
            {
                AnimationItemAll[GroupName].Remove(this);
            }
            if (onFinished != null)
            {
                onFinished(false);
                onFinished = null;
            }
        }
        #endregion

        public void OnAnimationItemFinished(string id)
        {
            if(id == GetInstanceID().ToString())
            {
                if (onFinished != null)
                    onFinished(true);
                onFinished = null;
            }
        }
    }
}
