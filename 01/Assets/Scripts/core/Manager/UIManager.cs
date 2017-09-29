
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine.UI;
using projectQ.animation;

namespace projectQ
{
    public class UIManager : BaseManager
    {
        /// <summary>
        /// 缓存UI最大数量
        /// </summary>
        public int CacheUIMaxCount = 10; 
        public int Depth = 100;
        public const int DepthStart = 100;
        public const int DepthMax = 2666;//数字吉利
		public const int DepthInterval = 10;

        private GameObject _uiRoot;
        public GameObject UIRoot
        {
            get { return _uiRoot; }
        }
        private GameObject _uiNormal;
        public GameObject UINormal
        {
            get { return _uiNormal; }
        }
        private GameObject _uiPop;
        public GameObject UIPop
        {
            get { return _uiPop; }
        }
        private GameObject _uiBg;
        public GameObject UIBg
        {
            get { return _uiBg; }
        }
        private GameObject _uiAnimationPanel;
        public GameObject UIAnimationPanel
        {
            get { return _uiAnimationPanel; }
        }
        private UICamera _uiCamera;
        public UICamera UICamera
        {
            get { return _uiCamera; }
        }

        //等待打开
        private Queue<string> OpenQueue = new Queue<string>();
        //UI打开记录
        private List<string> UIList = new List<string>();
        //隐藏的UI
        private List<string> UIHideList = new List<string>();
        //打开的UI
        private List<string> UIShowList = new List<string>();
        
        //正在加载的UI
        private Dictionary<string, GameObject> UILoadMap = new Dictionary<string, GameObject>();
        //全部UI和数据
        private Dictionary<string, UIViewBase> UIMap = new Dictionary<string, UIViewBase>();

        private Action<GameObject, object[]> LoadUIPrefabCallBack = null;
        private Action<GameObject, object[]> SubLoadUIPrefabCallBack = null;

        /// <summary>
        /// 全屏UI打开数量
        /// </summary>
        public int FullScreenCount
        {
            get
            {
                int result = 0;
                for (int i = 0; i < UIShowList.Count; i++)
                {
                    string uiName = UIShowList[i];
                    if (UIMap.ContainsKey(uiName))
                    {
                        var tempUI = UIMap[uiName];
                        if(tempUI != null && tempUI.IsFullSceneUI)
                        {
                            result++;
                        }
                    }
                }
                return result;
            }
        }

        #region UI打开
        public void OpenUI(string uiName,params object[] uiData)
        {
            try
            {
                if (AnimationItem.CurrExcute != null)
                {
                    AnimationItem.OnAnimationFinished += () =>
                    {
                        OpenUI(uiName, uiData);
                    };
                    return;
                }

                if (UIMap.ContainsKey(uiName))
                {
                    ShowUI(uiName, false, uiData);
                }
                else if (UILoadMap.ContainsKey(uiName))
                {
                    return;
                }
                else
                {
                    UILoadMap.Add(uiName, null);
                    LoadUI(uiName, LoadUIPrefabCallBack, uiData);
                }
            }
            catch (System.Exception ex)
            {
                QLoger.LOG(ex.ToString());
            }
        }
        public void OpenSubUI(string uiName)
        {
            LoadUI(uiName, SubLoadUIPrefabCallBack, null);
        }
        public void OpenPopUI(string uiName,params object[] uiData)
        {
            //OpenQueue.Enqueue(uiName,uinam)
        }

        //加载UI
        private void LoadUI(string uiName, Action<GameObject, object[]> func,object[] uiData)
        {
            string prefabPath = uiName;
            int index = prefabPath.IndexOf("_");
            if (index > 0)
            {
                prefabPath = prefabPath.Substring(0, index) + "/" + prefabPath.Substring(index+1);
            }
            else
            {
                prefabPath += "/" + prefabPath;
            }
            DebugPro.Log("Prefab路径" + prefabPath);
            //ResourcesDataLoader.AsyncLoadUIPrefab(_R.Instance, prefabPath, func, , uiData);
            func(ResourcesDataLoader.LoadUI<GameObject>(prefabPath), new object[2] { uiName, uiData });
        }

        //UI加载完毕的回掉函数
        private void OnLoadUIPrefabCallBack(GameObject prefab, object[] data)
        {
            string uiName = data[0] as string;
            object[] uiData = null;
            if (data.Length > 1)
                uiData = data[1] as object[];
            UILoadMap.Remove(uiName);
            NGUITools.AddChild(_uiNormal, prefab);
            ShowUI(uiName,true, uiData);
            //if (FakeReplayManager.Instance.ReplayState)
            //{
            //    _R.ui.CloseUI("UIPrepareGame");
            //    _R.ui.OpenUI("UIReplayCtrl");
            //}
        }

        //UI加载完毕的回掉函数
        private void OnSubLoadUIPrefabCallBack(GameObject prefab, object[] data)
        {
            string uiName = data[0] as string;
            UILoadMap.Remove(uiName);
            string mainUIName = uiName.Substring(0, uiName.IndexOf("_"));
            if (UIMap.ContainsKey(mainUIName))
            {
                UIMap[mainUIName].OnSubUICallBack(uiName.Substring(uiName.IndexOf("_")+1),prefab);
            }
        }

        private void ShowUI(string uiName, bool isInit, object[] uiData)
        {
            try
            {
                if (UIMap.ContainsKey(uiName))
                {
                    UIViewBase ui = UIMap[uiName];
                    if (ui.isClearGoBack)
                        UIList.Clear();

                    if (ui.isAddGobackList)
                    {
                        if (UIList.IndexOf(uiName) != -1)
                        {
                            UIList.Remove(uiName);
                        }
                        UIList.Add(uiName);
                    }

                    //加入展示列表
                    if (UIShowList.IndexOf(uiName) == -1)
                    {
                        UIShowList.Add(uiName);
                    }



                    //清除隐藏列表
                    if (UIHideList.IndexOf(uiName) == -1)
                        UIHideList.Remove(uiName);

                    //增加深度
                    if (ui.isAutoUpdateDepth)
                        AddDepth(ui);


                    //展示
                    NGUITools.SetActive(ui.gameObject, true, false);

                    if (uiData != null)
                    {
                        ui.OnPushData(uiData);
                    }
                    if (isInit)
                    {
                        ui.Init();
                    }

                    if (ui.IsMask)
                        ui.ShowMask();

                    ui.OnShow();

                    RefreshCacheUI();

                    EventDispatcher.FireSysEvent(GEnum.NamedEvent.SysUI_StateUpdate_Open, uiName);

                    //播放音效
                    ui.PlaySound(true);
                    this.PlayAnimation(uiName, true, ui.OnShowAndAnimationEnd);
                }
            }
            catch (System.Exception ex)
            {
                QLoger.ERROR("UIManager ShowUI报错"+ex.ToString());
            }
        }


        #endregion

        #region 很多
        /// <summary>
        /// 取得所有打开的UI列表
        /// </summary>
        public List<string> GetUIShowList()
        {
            return this.UIShowList;
        }


        /// <summary>
        /// Android返回按键响应事件
        /// </summary>
        public void OnAndroidGoBackButton()
        {
            _R.Instance.StartCoroutine(OnAndroidGoBackButtonCoroutine());
        }
        private IEnumerator OnAndroidGoBackButtonCoroutine()
        {
            yield return null;
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            DebugPro.LogError(UIList.ToString());
            //当我存在一个以上的UI时候 并且 没有弹出框
            if (
                UIList.Count > 0
                && WindowUIManager.Instance.curOpenWindow == null   //小窗口时不能返回
                && AnimationItem.CurrExcute == null                 //播放动画时不能返回
                && !MemoryData.GameStateData.LoadingActive           //开loading时不能返回
                && _R.flow.CurrExecute == null                      //当前有流程不能返回
                && !SDKManager.Instance.IsOpenWebView()
                )
            {
                var ui = GetGoBackUI();
                if (ui != null)
                {
                    ui.GoBack();
                }
            }
        }

        private UIViewBase GetGoBackUI()
        {
            string uiName = UIList[UIList.Count - 1];
            UIViewBase ui = this.GetUI(uiName);
            if (ui == null) return null;

            if (ui.IsCanGoBack) return ui;

            if(UIList.IndexOf("UIPrepareGame") != -1)
            {
                for (int i = UIList.Count - 1; i >= 0; i--)
                {
                    var temp = this.GetUI(UIList[i]);
                    if(temp.IsCanGoBack)
                    {
                        if (UIList[i] == "UIPrepareGame")
                        {
                            return temp;
                        }
                        else
                        {
                            return null;
                        }
                    }
                }
            }
            return null;
        }

        /// <summary>
        /// 取得UI
        /// </summary>
        /// <param name="uiName"></param>
        /// <returns></returns>
        public UIViewBase GetUI(string uiName)
        {
            if(UIMap.ContainsKey(uiName))
            {
                return UIMap[uiName];
            }
            return null;
        }

        /// <summary>
        /// 是否正在显示中
        /// </summary>
        /// <param name="uiName"></param>
        /// <returns></returns>
        public bool IsShowUI(string uiName)
        {
            return UIMap.ContainsKey(uiName) && UIShowList.IndexOf(uiName) >= 0;
        }

        /// <summary>
        /// 关闭UI
        /// </summary>
        /// <param name="uiName"></param>
        public void CloseUI(string uiName)
        {
            bool isFullSceene = false;
            if (UIMap.ContainsKey(uiName))
            {
                isFullSceene = UIMap[uiName].IsFullSceneUI;
                //先播放音效
                UIMap[uiName].PlaySound(false);
                EventDispatcher.FireSysEvent(GEnum.NamedEvent.SysUI_StateUpdate_CloseOrHide, uiName);
                UIMap[uiName].Close();
            }
            if(UIShowList.IndexOf(uiName) >= 0)
            {
                UIShowList.Remove(uiName);
            }
            UIHideList.Remove(uiName);
            ClearUIList(uiName);
        }

        /// <summary>
        /// 清除所有  但不包括DontClose为true的
        /// </summary>
        public void ClearAll(List<string> exceptUIList = null)
        {
            List<string> removeList = new List<string>();
            String[] arr = new string[UIMap.Keys.Count];
            UIMap.Keys.CopyTo(arr, 0);
            for(int i=0; i< arr.Length; ++i)
            {
                if (UIMap.ContainsKey(arr[i]) && !UIMap[arr[i]].DontClose 
                    //并且不在排除列表中
                    && (exceptUIList == null || exceptUIList.IndexOf(arr[i]) == -1))
                {
                    removeList.Add(arr[i]);
                    UIMap[arr[i]].Close(false);
                }
            }

            string tempUIName = null;
            for (int i = 0; i < removeList.Count; ++i)
            {
                tempUIName = removeList[i];
                if (UIMap.ContainsKey(tempUIName))
                    UIMap[tempUIName].Close(false);

                if (UIList.IndexOf(tempUIName) != -1)
                    UIList.Remove(tempUIName);

                if (UIShowList.IndexOf(tempUIName) != -1)
                {
                    UIShowList.Remove(tempUIName);
                }

                if (UIHideList.IndexOf(tempUIName) != -1)
                    UIHideList.Remove(tempUIName);

                if (UILoadMap.ContainsKey(tempUIName))
                    UILoadMap.Remove(tempUIName);
            }
        }

        /// <summary>
        /// 返回上一个UI
        ///// </summary>
        //public void GoBack()
        //{
        //	if(UIList.Count > 1)
        //	{
        //        string tempUIName = UIList[UIList.Count - 1];
        //        UIList.Remove(tempUIName);
        //        HideUI(tempUIName);

        //        if(UIList.Count > 0)
        //        {
        //            tempUIName = UIList[UIList.Count - 1];

        //            if (UIShowList.IndexOf(tempUIName) == -1)
        //            {
        //                OpenUI(tempUIName);
        //            }
        //            else
        //            {
        //                ShowUI(tempUIName, false, null);
        //            }
        //        }
        //	}
        //    //else if(UIList.Count == 0)
        //    //{
        //    //    ShowUI(UIList[0], false,null);
        //    //}
        //}

        /// <summary>
        /// 隐藏UI
        /// </summary>
        /// <param name="uiName"></param>
        public void HideUI(string uiName)
        {
            if (!UIMap.ContainsKey(uiName)) return;
            var ui = UIMap[uiName];
            if (ui.isAutoRemove)
            {
                if(UIHideList.IndexOf(uiName) == -1)
            	    UIHideList.Add(uiName);
            }
            UIShowList.Remove(uiName);
            UIMap[uiName].OnHide();
            //UI关闭音效
            ui.PlaySound(false);

            PlayAnimation(uiName,false,(isOk) => {
                if(isOk)
                {
                    EventDispatcher.FireSysEvent(GEnum.NamedEvent.SysUI_StateUpdate_CloseOrHide, uiName);

                    if (!UIMap.ContainsKey(uiName)) return;

                    if (ui.View != null)
                    {
                        ui.View.SetActive(false);
                    }
                    ClearUIList(uiName);

                    RefreshCacheUI();

                    if (ui.isAutoUpdateDepth)
                        SubDepth(UIMap[uiName]);
                }
            });
        }

        /// <summary>
        /// 清除UI列表
        /// </summary>
        /// <param name="uiName"></param>
        private void ClearUIList(string uiName)
        {
            int index = UIList.IndexOf(uiName);
            if (index > 0 && index == UIList.Count - 1)
            {
                string tempName = UIList[index - 1];
                if (UIMap[tempName].isClearGoBack)
                {
                    UIList.Clear();
                    UIList.Add(tempName);
                }
            }
        }

        /// <summary>
        /// 刷新缓存的UI 就是太多了清清很久没用到的UI
        /// </summary>
        private void RefreshCacheUI()
        {
			while(CacheUIMaxCount < UIHideList.Count)
			{
				if(UIHideList.Count == 0) return ;
				string uiName = UIHideList[0];
				UIHideList.RemoveAt(0);

				UIMap[uiName].Close();
			}
        }

        //注册UI
        public void RegisterUI(string uiName, UIViewBase ui)
        {
            if (UIMap.ContainsKey(uiName))
                UIMap[uiName] = ui;
            else
                UIMap.Add(uiName, ui);
        }

        //注销UI
        public void UnRegisterUI(string uiName)
        {
            if (UIMap.ContainsKey(uiName))
            {
                UIMap.Remove(uiName);
            }
            if (UIList.IndexOf(uiName) >= 0)
                UIList.Remove(uiName);
            if (UIHideList.IndexOf(uiName) >= 0)
                UIHideList.Remove(uiName);

            if (UIShowList.IndexOf(uiName) >= 0)
                UIShowList.Remove(uiName);
        }
        #endregion

        #region 背景管理
        public void SetUIBgActive(bool isActive)
        {
            if (_uiBg.activeSelf != isActive)
                _uiBg.SetActive(isActive);
        }
        #endregion

        #region 动画管理
        public void PlayAnimation(AnimationItem item,bool forward, AnimationItem.OnItemFinishedDelegate func)
        {
            bool flag = item.Play(forward, (isOk) =>
            {
                func(isOk);
                SetAnimationActive(false);
            });
            SetAnimationActive(flag);
        }
        public void PlayAnimation(string uiName, bool forward, AnimationItem.OnItemFinishedDelegate func)
        {
            bool flag = AnimationItem.PlayAnimUI(uiName, forward, (isOk) => {
                if(func != null)
                    func(isOk);
                SetAnimationActive(false);
            });

            SetAnimationActive(flag);
        }
        private void SetAnimationActive(bool isActive)
        {
            if(isActive != UIAnimationPanel.activeSelf)
            {
                UIAnimationPanel.SetActive(isActive);
            }
        }
        #endregion

        #region 深度管理
        public int GetDepth(string uiName)
        {
            if(UIMap.ContainsKey(uiName) && UIMap[uiName] != null)
            {
                return UIMap[uiName].GetDepth();
            }
            return 0;
        }
        public void AddDepth(string uiName)
        {
            if(UIMap.ContainsKey(uiName))
            {
                AddDepth(UIMap[uiName]);
            }
        }
        private void AddDepth(UIViewBase ui, bool isCheckDepth = true){
            ui.SetDepth(Depth);
            Depth += DepthInterval;

            if(isCheckDepth)
                CheckDepth();
        }

		private void SubDepth(UIViewBase ui){
			if (ui.GetDepth() == Depth - DepthInterval)
			{
	        	Depth -= DepthInterval;
			}
        } 

        private void CheckDepth()
        {
            if(Depth >= DepthMax)
            {
                //从小到大排序
                UIShowList.Sort((name1, name2) =>
                {
                    UIViewBase temp1 = null;
                    UIViewBase temp2 = null;
                    if (UIMap.ContainsKey(name1))
                        temp1 = UIMap[name1];
                    if (UIMap.ContainsKey(name2))
                        temp2 = UIMap[name2];

                    if (temp1 == null) return -1;
                    if (temp2 == null) return 1;

                    return temp1.GetDepth().CompareTo(temp2.GetDepth());
                });
                Depth = DepthStart;
                for (int i = 0; i < UIShowList.Count; i++)
                {
                    if(!UIMap.ContainsKey(UIShowList[i]) || UIMap[UIShowList[i]] == null)
                    {
                        UIShowList.RemoveAt(i);
                        i--;
                        continue;
                    }
                    var ui = UIMap[UIShowList[i]];
                    if (!ui.isAutoUpdateDepth) continue;

                    AddDepth(ui,false);
                }
            }
        }
        #endregion

        #region override
        public override void Init()
        {
            LoadUIPrefabCallBack = OnLoadUIPrefabCallBack;
            SubLoadUIPrefabCallBack = OnSubLoadUIPrefabCallBack;

            this._uiRoot = GameObject.Find("UIRoot").gameObject;
            this._uiNormal = this._uiRoot.transform.Find("UINormal").gameObject;
            this._uiPop = this._uiRoot.transform.Find("UIPop").gameObject;
            this._uiAnimationPanel = this._uiRoot.transform.Find("UIAnimationPanel").gameObject;
            this._uiBg = this._uiRoot.transform.Find("UIBg").gameObject;
            this._uiCamera = this._uiRoot.transform.FindChild("Camera").GetComponent<UICamera>();
            OpenUI("UIDefault");
            OpenUI("UILoading");
            _R.scene.SetScene(new MainScene(), null);
        }

        public override void Dispose()
        {

        }
        #endregion
    }
}