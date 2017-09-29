using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace projectQ
{
    public class UIDefinedButton : UIButton
    {
        public string ButtonID;

        void initButton()
        {
            bool has = false;
            for (int i = 0; i < this.onClick.Count; i++)
            {
                if (this.onClick[i] != null & "fireOnClickAction".Equals(this.onClick[i].methodName) &&
                    this.onClick[i].target.Equals(this) && (this.onClick[i].parameters == null ||
                    this.onClick[i].parameters.Length == 0))
                {
                    has = true;
                }
            }

            if (!has)
            {
                this.onClick.Add(new EventDelegate(this, "fireOnClickAction"));
            }

            this.ClickCountReset();
        }

        void Awake()
        {
            //if(WaitTime < 0.25f)
            //    WaitTime = 0.25f;
            this.initButton();
            if (this.tweenTarget != null)
            {
                //没有TweenTarget,就不要变色计算
                sprite = this.tweenTarget.GetComponent<UISprite>();
                if (sprite != null)
                {
                    //没有sprite,就不要变色计算
                    subWidget = new List<UIWidget>();
                    foreach (var v in this.transform.GetComponentsInChildren<UIWidget>())
                    {
                        subWidget.Add(v);
                    }
                }
            }
        }

        public void addDepth(int deep)
        {
            for (int i = 0; i < this.subWidget.Count; i++)
            {
                subWidget[i].depth += deep;
            }
        }

        List<UIWidget> subWidget;

        UISprite sprite;

        //void ButtonUpdate()
        //{
        //    ClickCountUpdate();
        //    TimeUpdate();
        //if (sprite != null && subWidget.Count > 0)
        //{
        //    for (int i = 0; i < subWidget.Count; i++)
        //    {
        //        if (subWidget[i] != null)
        //            subWidget[i].color = sprite.color;
        //    }
        //}
        //}

        public void playSound()
        {
            MusicCtrl.Instance.Music_SoundPlay(SoundSelect);

            /*
            UIPlaySound sound = this.gameObject.GetComponent<UIPlaySound>();
            if (sound == null)
            {
                sound = gameObject.AddComponent<UIPlaySound>();
            }

            AudioClip audio = ResourcesDataLoader.Load<AudioClip>(GameAssetCache.M_Sound_01_Path);

            sound.audioClip = audio;
            sound.Play();
            */
        }

        public void fireOnClickAction()
        {
            ClickCount();

            TimeClick();

            this.playSound();

            UserActionManager.AddCodingString(this.gameObject.name);

            QLoger.LOG(fullName(this.gameObject));
        }

        string fullName(GameObject go)
        {
            if (go != null && go.transform.parent != null)
            {
                return string.Format("{0}/{1}", fullName(go.transform.parent.gameObject), go.name);
            }
            return go.name;
        }



        [Tooltip("点击按钮播放的哪个音效")]
        public GEnum.SoundEnum SoundSelect = GEnum.SoundEnum.btn_select1;

        private string LabelUpdateTimeDefault;
        private float LastClickTime;
        //private bool IsCanClick = true;

        System.Text.StringBuilder timeSb = new System.Text.StringBuilder();

        //加入按钮等待时间功能=================================================
        private Collider buttonCol = null;
        #region 按钮的Time
        [Tooltip("等待时间 多久后才可以再次点击")]
        public float WaitTime;

        [Tooltip("把时间放在Label中")]
        public UILabel LabelUpdateTime;

        /// <summary>
        /// 检查
        /// </summary>
        /// <returns></returns>
        private bool TimeCheck()
        {
            return (Time.realtimeSinceStartup - LastClickTime) > WaitTime;
        }

        /// <summary>
        /// 点击
        /// </summary>
        private void TimeClick()
        {
            if (WaitTime != 0)
            {
                //IsCanClick = false;
                LabelUpdateTimeDefault = LabelUpdateTime == null ? "" : LabelUpdateTime.text;
                LastClickTime = Time.realtimeSinceStartup;
                GameDelegateCache.Instance.InvokeMethodEvent -= this.TimeUpdate;
                GameDelegateCache.Instance.InvokeMethodEvent += this.TimeUpdate;
            }
        }

        /// <summary>
        /// 更新
        /// </summary>
        private void TimeUpdate()
        {
            if (WaitTime != 0 /*&& !IsCanClick*/)
            {
                ResetButtonEnabled();
                if (TimeCheck())
                {
                    //IsCanClick = true;
                    GameDelegateCache.Instance.InvokeMethodEvent -= this.TimeUpdate;

                    if (LabelUpdateTime != null) //修改时间
                    {
                        LabelUpdateTime.text = LabelUpdateTimeDefault;
                    }
                }
                else
                {
                    if (LabelUpdateTime != null) //修改时间
                    {
                        float time = WaitTime - (Time.realtimeSinceStartup - LastClickTime);

                        if (time < 0)
                        {
                            time = 0;
                        }

                        timeSb.Length = 0;
                        timeSb.Append(Mathf.RoundToInt(time)).Append("s");
                        LabelUpdateTime.text = timeSb.ToString();
                    }
                }
            }
        }

        #endregion

        //加入按钮的点击次数功能===============================================
        #region 点击次数
        [Tooltip("最大点击次数")]
        public int MaxClickCount;
        [Tooltip("是否Enabel 重置")]
        public bool isOnEnableResetMaxClickCount = true;

        //剩余点击次数
        private int ResidueClickCount;
        //禁止点击采用Collider 还是 buttonEnable
        public bool IsButtonEnabled = true;

        /// <summary>
        /// 检查次数
        /// </summary>
        /// <param name="isSubClickCount"></param>
        /// <returns></returns>
        private bool ClickCountCheck()
        {
            return MaxClickCount == 0 || ResidueClickCount > 0;
        }

        private void ClickCount()
        {
            ResidueClickCount--;

            if (!ClickCountCheck())
            {
                StartCoroutine(ChlickCountCoroutine());
            }
        }
        private IEnumerator ChlickCountCoroutine()
        {
            yield return null;
            ResetButtonEnabled();
        }
        public void ClickCountReset()
        {
            ResidueClickCount = MaxClickCount;
        }

        private bool CheckButtonIsCanClick()
        {
            return this.ClickCountCheck() && this.TimeCheck();
        }
        #endregion

        //设置是否可点击
        private void ResetButtonEnabled()
        {
            bool flag = CheckButtonIsCanClick();
            if (IsButtonEnabled)
            {
                base.isEnabled = flag;
            }
            else
            {
                if (buttonCol == null)
                    buttonCol = gameObject.GetComponent<Collider>();

                if (buttonCol.enabled != flag)
                    buttonCol.enabled = flag;
            }
        }


        protected override void OnEnable()
        {
            base.OnEnable();
            if (isOnEnableResetMaxClickCount)
            {
                ClickCountReset();
            }
            ResetButtonEnabled();
        }

        private void OnDestroy()
        {
            GameDelegateCache.Instance.InvokeMethodEvent -= this.TimeUpdate;
        }
    }
}