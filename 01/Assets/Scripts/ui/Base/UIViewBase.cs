using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Text;
using projectQ.animation;

namespace projectQ
{
    public abstract class UIViewBase : MonoBehaviour
    {
        abstract public void Init();
        abstract public void OnShow();
        abstract public void OnHide();

        [HideInInspector]
        public GameObject View;
        [HideInInspector]
        public UIModelBase _model;

        private UIPanel _uiPanel;
        private int UIDepth = -999;
        private int CurrMaxDepth = -999;

        [Tooltip("自动删除")]
        public bool isAutoRemove = true;
        [Tooltip("是否清空Goback列表")]
        public bool isClearGoBack = false;
        [Tooltip("是否添加到GoBack列表")]
        public bool isAddGobackList = true;
        [Tooltip("是否自动设置Depth")]
        public bool isAutoUpdateDepth = true;
        [Tooltip("是否常驻不被删除")]
        public bool DontClose = false;
        [Tooltip("是否需要Mask")]
        public bool IsMask = true;

        [Tooltip("Mask透明度")]
        public float MaskAlpha = 0.68f;

        [Tooltip("Mask点击是否关闭整个UI")]
        public bool MaskClickClose = false;
        [Tooltip("打开UI播放音效")]
        public GEnum.SoundEnum OpenUISoundSelect = GEnum.SoundEnum.btn_null;
        [Tooltip("关闭UI播放音效")]
        public GEnum.SoundEnum CloseUISoundSelect = GEnum.SoundEnum.btn_null;

        [Tooltip("是否是全屏UI")]
        public bool IsFullSceneUI = false;

        [Tooltip("是否可以返回")]
        public bool IsCanGoBack = false;

        private void Awake()
        {
            name = name.Substring(0, name.Length - 7);
            View = gameObject;

            _model = transform.GetComponent<UIModelBase>();
            if (_model != null)
                _model._ui = this;

            _R.ui.RegisterUI(name, this);
        }

        #region 可选继承
        protected virtual void OnClose()
        {

        }

        protected virtual GameObject OnSubUILoaded(string subName, GameObject go)
        {
            return NGUITools.AddChild(gameObject, go);
        }

        public virtual void OnPushData(object[] data)
        {

        }

        public virtual void MaskClick(GameObject go)
        {
            if (MaskClickClose)
                this.Close();
        }

        /// <summary>
        /// ui打开并且动画结束完毕后 可以替代下Onshow
        /// </summary>
        protected virtual void OnShowAndAnimationEnd()
        {

        }

        public virtual void PlaySound(bool IsOpen)
        {
            MusicCtrl.Instance.Music_SoundPlay(IsOpen ? OpenUISoundSelect : CloseUISoundSelect);
        }


        public virtual void GoBack()
        {
            this.Close();
        }
        #endregion

        #region 加载
        /// <summary>
        /// 加载UI
        /// </summary>
        /// <param name="uiName"></param>
        public void LoadUIMain(string uiName, params object[] data)
        {
            _R.ui.OpenUI(uiName, data);
        }

        /// <summary>
        /// 加载子UI
        /// </summary>
        /// <param name="subName"></param>
        public void LoadSubUI(string subName)
        {
            _R.ui.OpenSubUI(name + "_" + subName);
        }

        /// <summary>
        /// 弹出框
        /// </summary>
        public void LoadPop(WindowUIType type, string title, string content, string[] btnNames, Action<int> buttonAction, bool isClose = false)
        {
            WindowUIManager.Instance.CreateOrAddWindow(type, title, content, btnNames, buttonAction, WindowUIRank.Normal, isClose);
        }
        /// <summary>
        /// 弹出错误信息
        /// </summary>
        public void LoadError(int resultCode, Action<int> buttonAction)
        {
            WindowUIManager.Instance.CreateErrorWindow(resultCode, buttonAction);
        }

        /// <summary>
        /// 弹出Tip条
        /// </summary>
        /// <param name="content"></param>
        public void LoadTip(string content)
        {
            WindowUIManager.Instance.CreateTip(content);
        }
        public void LoadTip(int resultCode)
        {
            WindowUIManager.Instance.CreateByErrorCode(resultCode);
        }

        public void OnSubUICallBack(string subName, GameObject go)
        {
            GameObject goo = OnSubUILoaded(subName, go);
            if (isAutoUpdateDepth)
                RefreshDepthChilds(goo.transform);
        }

        /// <summary>
        /// 打开加载发送消息的小Loading条
        /// </summary>
        public GameObject LoadSendLoading()
        {
            GameObject prefab = ResourcesDataLoader.Load<GameObject>(GameAssetCache.Prefab_SendLoading_Path);

            GameObject sendLoadObj = NGUITools.AddChild(gameObject, prefab);

            return sendLoadObj;
        }

        /// <summary>
        /// 把Loading条关闭
        /// </summary>
        public void StopSendLoading()
        {
            EventDispatcher.FireEvent(GEnum.NamedEvent.LoadingSendClose_Event);
        }

        #endregion

        #region 管理
        /// <summary>
        /// ui打开并且动画结束完毕后 可以替代下Onshow
        /// </summary>
        public void OnShowAndAnimationEnd(bool isOk)
        {
            if (isOk)
            {
                this.OnShowAndAnimationEnd();
            }
        }
        /// <summary>
        /// 隐藏/关闭
        /// </summary>
        public void Hide(string uiName = null)
        {
            _R.ui.HideUI(uiName == null ? name : uiName);
        }

        ///// <summary>
        ///// 回退
        ///// </summary>
        //public void GoBack()
        //{
        //    if(!this.isClearGoBack)
        //        _R.ui.GoBack();
        //}
        bool isClose = false;
        /// <summary>
        /// 关闭
        /// </summary>
	    public void Close(bool isPlayAnimation = true)
        {
            if (isClose)
            {
                return;
            }

            AnimationItem.RemoveAnim(this.name);

            isClose = true;

            StopSendLoading();

            if (isPlayAnimation)
            {
                _R.ui.PlayAnimation(this.name, false, _close);
            }
            else
            {
                _close(true);
            }
        }

        private void _close(bool isOk)
        {
            if (isOk)
            {
                _R.ui.UnRegisterUI(this.name);
                this.OnClose();
                EventDispatcher.FireSysEvent(GEnum.NamedEvent.SysUI_StateUpdate_CloseOrHide, name);
                Destroy(gameObject);
            }
        }
        #endregion

        #region Mask
        UIWidget BaseMask = null;
        public void ShowMask(int depth = -99)
        {
            BaseMask = UITools.ShowMask(gameObject, BaseMask, depth, MaskAlpha);
            UIEventListener.Get(BaseMask.gameObject).onClick = OnMaskClick;
        }

        public void HideMask()
        {
            if (BaseMask != null)
                BaseMask.enabled = false;
        }
        /// <summary>
        /// 背景板的点击事件
        /// </summary>
        /// <param name="go"></param>
        private void OnMaskClick(GameObject go)
        {
            MaskClick(go);
        }
        #endregion

        #region 层级管理
        public int GetDepth()
        {
            return UIDepth;
        }

        public void SetDepth(int depth)
        {
            if (_uiPanel == null)
                _uiPanel = transform.GetComponent<UIPanel>();

            UIDepth = depth;
            RefreshDepthChilds();
        }


        private void RefreshDepthChilds(Transform tf = null)
        {
            if (tf == null)
                tf = transform;

            if (tf != transform)
                RefreshDepthChilds(tf, CurrMaxDepth + 1);

            RefreshDepthChilds(transform, UIDepth);
        }

        /// <summary>
        /// 刷新所有子panle
        /// </summary>
        private void RefreshDepthChilds(Transform tf, int startDepth)
        {
            List<UIPanel> panels = UITools.GetChildPanel(tf);
            //UIPanel[] panels = tf.GetComponentsInChildren<UIPanel>();
            if (panels == null)
                return;

            panels.Sort((x, y) =>
            {
                return x.depth.CompareTo(y.depth);
            });

            for (int i = 0; i < panels.Count; ++i)
            {
                panels[i].depth = startDepth + i;
            }

            CurrMaxDepth = startDepth + panels.Count;
        }

        #endregion
    }
}
