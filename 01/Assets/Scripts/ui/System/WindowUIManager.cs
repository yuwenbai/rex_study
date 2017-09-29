using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace projectQ
{
    public class WindowUIData
    {
        public WindowUIType windowType;
        public WindowUIRank windowRank = WindowUIRank.Normal;
        public bool isCloseButton = false;
        public string titleText = null;
        public string contentText = null;
        public string[] buttonTexts = null;
        public System.Action<int> btnCall = null;
        public float timeValue = -1;
        public bool timeLeftShow = true;
        public int sortNum = 0;                                         //弹窗排序

        public int depth;

        //Tip用============
        /// <summary>
        /// 停留时间
        /// </summary>
        public float StayTime = 1.2f;
    }

    public enum WindowUIType
    {
        SystemPopupWindow,
        ErrorPopupWindow,
        SystemTipPopupWindow,
        SystemPopupPlusWindow,
        SystemTimeWindow,
    }

    //Normal can wait ,special show immediately and clear all wait
    public enum WindowUIRank
    {
        Normal,
        Special
    }

    public class WindowUIManager
    {
        #region Singleton

        private WindowUIManager() { }

        private static WindowUIManager _instance = null;
        public static WindowUIManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new WindowUIManager();
                    _instance.windowDataQueue = new Queue<WindowUIData>();
                    _instance.sortNum = 0;
                }
                return _instance;
            }
        }

        #endregion

        [HideInInspector]
        public Queue<WindowUIData> windowDataQueue = null;

        [HideInInspector]
        private bool _hasShowed = false;

        [HideInInspector]
        public WindowUIBase curOpenWindow = null;

        [HideInInspector]
        public int sortNum = 0;


        //打开一个窗口或者缓存进打开队列
        public void CreateOrAddWindow(
            WindowUIType windowType,
            string title, string content, string[] buttonTexts, System.Action<int> btnCall,
            WindowUIRank windowRank = WindowUIRank.Normal, bool isCloseButton = false
            )
        {
            WindowUIData windowData = new WindowUIData();
            windowData.windowType = windowType;
            windowData.windowRank = windowRank;
            windowData.titleText = title;
            windowData.contentText = content;
            windowData.buttonTexts = buttonTexts;
            windowData.btnCall = btnCall;
            windowData.isCloseButton = isCloseButton;
            windowData.sortNum = this.sortNum;
            this.sortNum += 1;

            if (windowData.windowRank == WindowUIRank.Special)
            {
                //clear all and show 
                this.CloseAll(true);
                //windowDataQueue.Clear();
                //CloseCurWindow();

                ShowWindowByType(windowData);
            }
            else
            {
                if (_hasShowed)
                {
                    //add data to queue
                    windowDataQueue.Enqueue(windowData);
                }
                else
                {
                    ShowWindowByType(windowData);
                }
            }
        }
        public void CreateErrorWindow(int errorCode, System.Action<int> action)
        {
            string content = ErrorCodeHelper.GetResultCodeString(errorCode);
            CreateOrAddWindow(WindowUIType.ErrorPopupWindow, "提示", content, new string[] { "确定" }, action);
        }

        public void CreateByErrorCode(int resultCode)
        {
            this.CreateTip(ErrorCodeHelper.GetResultCodeString(resultCode));
        }

        public void CreateTip(string content,float stayTime = 1.2f)
        {
            WindowUIData windowData = new WindowUIData();
            windowData.windowType = WindowUIType.SystemTipPopupWindow;
            windowData.contentText = content;
            windowData.StayTime = stayTime;

            ShowWindowByType(windowData);
        }

        /// <summary>
        /// 创建有倒计时时间的系统框
        /// title    标题  
        /// content  文本  
        /// tValue     消失的时间 
        /// btnCall  时间到了回调方法
        /// </summary>
        public void CreateTimeWindow(string title, string content, float tValue, string[] buttonName, bool leftShow, System.Action<int> btnCall,
            WindowUIRank windowRank = WindowUIRank.Normal)
        {
            WindowUIData windowData = new WindowUIData();
            windowData.windowType = WindowUIType.SystemTimeWindow;
            windowData.titleText = title;
            windowData.contentText = content;
            windowData.timeValue = tValue;
            windowData.buttonTexts = buttonName;
            windowData.btnCall = btnCall;
            windowData.timeLeftShow = leftShow;
            windowData.windowRank = windowRank;
            windowData.sortNum = this.sortNum;
            this.sortNum += 1;

            if (windowRank == WindowUIRank.Special)
            {
                this.CloseAll(true);
                ShowWindowByType(windowData);
            }
            else
            {
                if (_hasShowed)
                {
                    //add data to queue
                    windowDataQueue.Enqueue(windowData);
                }
                else
                {
                    ShowWindowByType(windowData);
                }
            }

        }




        private void ShowWindowByType(WindowUIData windowData)
        {
            switch (windowData.windowType)
            {
                case WindowUIType.SystemPopupWindow:
                    {
                        _hasShowed = true;
                        GameObject obj = ResourcesDataLoader.LoadUI<GameObject>("System/SystemPopupWindow");
                        obj = NGUITools.AddChild(_R.ui.UIPop, obj);
                        SystemPopupWindow window = obj.GetComponent<SystemPopupWindow>();
                        curOpenWindow = window;
                        window.Init(ClickCloseCall);
                        window.SetData(windowData);
                        window.Open();
                        window.PlaySound(true);
                    };
                    break;
                case WindowUIType.ErrorPopupWindow:
                    {
                        _hasShowed = true;
                        GameObject obj = ResourcesDataLoader.LoadUI<GameObject>("System/SystemPopupWindow");
                        obj = NGUITools.AddChild(_R.ui.UIPop, obj);
                        SystemPopupWindow window = obj.GetComponent<SystemPopupWindow>();
                        curOpenWindow = window;
                        window.Init(ClickCloseCall);
                        window.SetData(windowData);
                        window.Open();
                        window.PlaySound(true);

                    }
                    break;
                case WindowUIType.SystemTipPopupWindow:
                    {
                        GameObject obj = ResourcesDataLoader.LoadUI<GameObject>("System/SystemTipPop");
                        obj = NGUITools.AddChild(_R.ui.UIPop, obj);
                        WindowUIBase window = obj.GetComponent<WindowUIBase>();
                        //curOpenWindow = window;
                        window.Init(null);
                        window.SetData(windowData);
                        window.Open();
                    }
                    break;
                case WindowUIType.SystemPopupPlusWindow:
                    {
                        _hasShowed = true;

                        GameObject obj = ResourcesDataLoader.LoadUI<GameObject>("System/SystemPopupPlusWindow");
                        obj = NGUITools.AddChild(_R.ui.UIPop, obj);
                        SystemPopupWindow window = obj.GetComponent<SystemPopupWindow>();
                        curOpenWindow = window;
                        window.Init(ClickCloseCall);
                        window.SetData(windowData);
                        window.Open();
                        window.PlaySound(true);

                    }
                    break;
                case WindowUIType.SystemTimeWindow:
                    {
                        _hasShowed = true;
                        GameObject obj = ResourcesDataLoader.LoadUI<GameObject>("System/SystemTimeWindow");
                        obj = NGUITools.AddChild(_R.ui.UIPop, obj);
                        SystemTimeWindow window = obj.GetComponent<SystemTimeWindow>();
                        curOpenWindow = window;
                        window.Init(ClickCloseCall);
                        window.SetData(windowData);
                        window.Open();
                        window.PlaySound(true);

                    }
                    break;
            }
        }

        public bool CheckHasWindow()
        {
            return _hasShowed;
        }

        private void ClickCloseCall(int uiNum)
        {
            //CloseCurWindow();
            if (_hasShowed && curOpenWindow != null)
            {
                if (curOpenWindow.SelfData.sortNum != uiNum)
                {
                    return;
                }

                curOpenWindow.CloseCurWindow();
                curOpenWindow = null;
                _hasShowed = false;

                if (windowDataQueue.Count > 0)
                {
                    WindowUIData windowData = windowDataQueue.Dequeue();
                    ShowWindowByType(windowData);
                }
            }
        }

        //关闭当前打开的窗体
        public void CloseCurWindow()
        {
            if (_hasShowed && curOpenWindow != null)
            {
                curOpenWindow.CloseCurWindow();
                curOpenWindow = null;
                _hasShowed = false;

                if (windowDataQueue.Count > 0)
                {
                    WindowUIData windowData = windowDataQueue.Dequeue();
                    ShowWindowByType(windowData);
                }
            }
        }


        /// <summary>
        /// 关闭所有
        /// </summary>
        public void CloseAll(bool isClose = false)
        {
            windowDataQueue.Clear();
            if (!isClose)
            {
                if (curOpenWindow != null && curOpenWindow.SelfData != null && curOpenWindow.SelfData.windowRank == WindowUIRank.Special)
                {
                    return;
                }
            }
            CloseCurWindow();
            //windowDataQueue.Clear();
        }

        //隐藏当前打开的窗体
        public void HideCurWindow()
        {
            curOpenWindow.Hide();
        }

        //重新显示当前打开的窗体
        public void ReShowCurWindow()
        {
            curOpenWindow.Show();
        }
    }
}