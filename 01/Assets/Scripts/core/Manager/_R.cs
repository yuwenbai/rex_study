using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace projectQ
{
    public class _R : MonoBehaviour
    {
        private static _R instance = null;

        public static _R Instance
        {
            get { return instance; }
        }

        private const int _target_fps = 29;
        private int _current_fps;
        public static string OSName = "";
        private void RegisterManagers()
        {
            //RegisterManager<CommandManager>();
            RegisterManager<UIManager>();
            RegisterManager<QSceneManager>();
            RegisterManager<QFlowManager>();

            _R.Instance.gameObject.AddComponent<NetWorkerImpl>();
            _R.Instance.gameObject.AddComponent<ResourceLoadMain>();

            // 录音处理
            gameObject.AddComponent<RecordManager>();

            GameObject obj = ResourcesDataLoader.Load<GameObject>(GameAssetCache.Prefab_MusicCtrl_Path);
            NGUITools.AddChild(gameObject, obj);

            ModelNetWorker.Instance.init();
            //LoginEnterNetWork.Instance.GatewayConnect();
            //ModleNetWorker.Instance.openLoginServer();

            UserActionManager.Init();
            UserActionManager.StartSend();


            //开启GPS
            //GPSDataManager.Instance.Init();
            GPSManager.Instance.Init();
            PingManager.Instance.Init();
            //GPSManager.Instance.OpenGPS();

            //网络消息 超时机制
            NetWorkMessageManager.Init();

            ScriptsDataSet<SDKManager>();



        }


#if __OPEN_USER_ACTION
		void HandleLog(string logString, string statckTrace, UnityEngine.LogType type){
			if(type == UnityEngine.LogType.Error ||
				type == UnityEngine.LogType.Exception){
				UserActionManager.AddLocalLog(type.ToString() , 
                    logString + "\nEX:\n"+statckTrace == null ? "" :statckTrace);
			}
		}
#endif

        public _R()
        {
            TcpDataRegist.Init();


        }

        /// <summary>
        /// 资源加载完毕加载该方法内的脚本和方法
        /// </summary>
        public void ResourceLoadFinishScriptsAdd()
        {
            // 初始化Xml数据
            MemoryData.XmlData.Init();
            // 微信头像管理
            ScriptsDataSet<DownHeadTexture>();
            //gameObject.AddComponent<DownHeadTexture>();
            // 截屏转换base64分享
            ScriptsDataSet<Tools_TexScreenshot>();
            //gameObject.AddComponent<Tools_TexScreenshot>();
            // 动画模型手控制
            ScriptsDataSet<AnimatorHand_Main>();
            //gameObject.AddComponent<AnimatorHand_Main>();
            // 麻将内部使用的动画播放脚本
            ScriptsDataSet<AnimPlayManager>();
            //麻将内部使用的时间控制脚本
            ScriptsDataSet<MahjongTimeConfig>();


            ScriptsDataSet<AnimTimeTick>();
            //gameObject.AddComponent<AnimPlayManager>();
            // android对接接口
            //gameObject.AddComponent<SDKManager>();
            SDKManager.Instance.WebWeChatData_LoadXml();
            //            SDKManager.Instance.InitWebView();
            SDKManager.Instance.SDKFunction(10);
        }

        public T ScriptsDataSet<T>() where T : MonoBehaviour
        {
            T data = gameObject.GetComponent<T>();
            if (data == null)
            {
                data = gameObject.AddComponent<T>();
            }

            return data;
        }


        public static UIManager ui
        {
            get
            {
                return GetManager<UIManager>();
            }
        }

        public static QSceneManager scene
        {
            get
            {
                return GetManager<QSceneManager>();
            }
        }

        public static QFlowManager flow
        {
            get
            {
                return GetManager<QFlowManager>();
            }
        }

        public static T GetManager<T>() where T : BaseManager
        {
            if (instance != null)
            {
                return instance._GetManager<T>();
            }
            return default(T);
        }

        private Dictionary<Type, BaseManager> managerDic = new Dictionary<Type, BaseManager>();
        private List<BaseManager> managerList = new List<BaseManager>();


        private const float DEFAULT_TIME_UPDATE = 0.17f;
        private float _update_left_time = DEFAULT_TIME_UPDATE;

        /// <summary>
        /// Update中执行定时器主逻辑
        /// </summary>
        void Update()
        {
            _update_left_time -= Time.deltaTime;
            if (_update_left_time <= 0)
            {
                _update_left_time = DEFAULT_TIME_UPDATE;
                UpdateLogic();
                PerInvokeMethod();
            }
        }
        private void UpdateLogic()
        {
            TimeTick.Instance.TickTick(Time.deltaTime);
        }
        void Awake()
        {
            //注册独立Log事件

#if __DISABLE_LOG
#else
            QLoger.SetLogUnity();
            QLoger.OPEN_DEBUG = true;
#endif

#if __OPEN_USER_ACTION
            //Application.RegisterLogCallback(HandleLog);
            Application.logMessageReceived += HandleLog;
            
            QLoger.SetLoggerAction(
                (p, v) =>
                {
                    UserActionManager.AddLocalLog(UnityEngine.LogType.Log.ToString(), ((v == null || v.Length <= 0) ? p : (string.Format(p, v))));
                },
                (p, v) =>
                {
                    UserActionManager.AddLocalLog(UnityEngine.LogType.Error.ToString(), ((v == null || v.Length <= 0) ? p : (string.Format(p, v))));
                });

#endif

            this._current_fps = _R._target_fps;

            UserActionManager.Add("100001");

            //设置手机屏幕不休眠
            Screen.sleepTimeout = SleepTimeout.NeverSleep;
            //Application.runInBackground = true;
            FrameRateMgr.Instance.UpdateFrameRate(this._current_fps);
            MemoryData.SetGameProperty(Screen.height, Screen.width);

            if (instance == null)
            {
                instance = this;
                RegisterManagers();
                Init();
            }
            else
            {
                Destroy(gameObject);
            }

            //以下时临时代码缓存一下图集，临时解决碰牌卡的问题 ,将在缓存机制建立和优化方案确定后进行改动，放在此处方便查找
            GameTools.TempCachePrefab("GamePrefab/Mahjong/Anim/XingPaiChoose/confirm_Peng");

        }

        private void setToSleep()
        {
            this._current_fps = 5;
            FrameRateMgr.Instance.UpdateFrameRate(this._current_fps);
        }

        private void setToAwake()
        {
            this._current_fps = _R._target_fps;
            FrameRateMgr.Instance.UpdateFrameRate(this._current_fps);
        }

        private void RegisterManager<T>() where T : BaseManager, new()
        {
            T manager = new T();
            managerDic[typeof(T)] = manager;
            managerList.Add(manager);
        }

        private void Init()
        {
            Input.multiTouchEnabled = false;
            DontDestroyOnLoad(this);
            for (int i = 0; i < managerList.Count; i++)
            {
                managerList[i].Init();
            }

            IniOtherModle();
            _R.ui.OpenUI("UIStart");

            //this.CancelInvoke("PerInvokeMethod");
            //this.InvokeRepeating("PerInvokeMethod", 0.2f, 0.2f);

#if __DEBUG || __DEBUG_AUTO_FRAMERATE
            //开启帧率控制器
            FrameRateMgr.Instance.InitChangeFrame();
#endif
        }

        private void IniOtherModle()
        {
            MahjongLogicNew.Instance.Ini();
        }

        public T _GetManager<T>() where T : BaseManager
        {
            BaseManager manager = null;
            managerDic.TryGetValue(typeof(T), out manager);
            return manager as T;
        }

        void OnDestroy()
        {
            //清理游戏中的资源
            UserActionManager.KillSend();
#if __OPEN_USER_ACTION
            Application.logMessageReceived -= HandleLog;
#endif
            QLoger.LOG("清理游戏资源完成");
        }

        //每秒执行
        void PerInvokeMethod()
        {
            try
            {

                GameDelegateCache.C2CRunPerInvokeMethodEvent();
                //健康提醒
                WallowTip();
                //网络状态监测
                NetworkTypeDetection();
            }
            catch (Exception ex)
            {
                QLoger.ERROR("定时器错误" + ex.ToString());
            }
        }


        int NetworkTypeDetectionCount = 0;
        int NotReachableCount = 0;
        int MaxNotReachableCount = 2;
        bool IsInitCheck = false;

        /// <summary>
        /// 网络状态监测
        /// </summary>
        private void NetworkTypeDetection()
        {
            if (NetworkTypeDetectionCount++ < 20) return;
            NetworkTypeDetectionCount = 0;
            var oldType = MemoryData.Get<NetworkReachability>("NetWorkType");
            var newType = Application.internetReachability;


            //DebugPro.Log(DebugPro.EnumLog.NetWork,"网络状态监测", "旧的", oldType.ToString(), "新的", newType.ToString());
            if (newType == NetworkReachability.NotReachable)
            {
                if (oldType == NetworkReachability.NotReachable && !IsInitCheck)
                {
                    if (++NotReachableCount >= MaxNotReachableCount)
                    {
                        IsInitCheck = true;
                        //发起请求
                        //DebugPro.Log(DebugPro.EnumLog.NetWork, "发起Ping百度请求");
                        StopCoroutine(PingConnect());
                        StartCoroutine(PingConnect());
                    }
                }
            }
            else
            {
                IsInitCheck = true;
                if (oldType != newType)
                {
                    MemoryData.Set("NetWorkType", newType);
                    //DebugPro.Log(DebugPro.EnumLog.NetWork, "网络状态监测到切换", "旧的", oldType.ToString(), "新的", newType.ToString());

                    if (oldType == NetworkReachability.NotReachable)
                        return;


                    string content = null;
                    if (newType == NetworkReachability.ReachableViaCarrierDataNetwork)
                    {
                        content = "您已切换至移动网络";
                    }
                    else if (newType == NetworkReachability.ReachableViaLocalAreaNetwork)
                    {
                        content = "您已切换至WiFi环境";
                    }
                    if (!string.IsNullOrEmpty(content))
                        WindowUIManager.Instance.CreateTip(content);
                }
            }
        }

        public enum PingState
        {
            PingIng,
            PingOK,
            CanNotConnectServer,
        }
        private const int CheckInterval = 20;
        public const string ServerIP = "https://www.baidu.com/";//"192.168.221.48";
        private PingState _PingServerState = PingState.PingIng;
        public PingState PingServerState
        {
            get { return _PingServerState; }
        }

        IEnumerator PingConnect()
        {
            _PingServerState = PingState.PingIng;
            //ResServer IP 
            string ResServerIP = ServerIP;
            //Ping網站 
            Ping ping = new Ping(ResServerIP);

            int nTime = 0;

            while (!ping.isDone)
            {
                yield return new WaitForSeconds(0.1f);

                if (nTime > CheckInterval) // time 2 sec, OverTime 
                {
                    nTime = 0;
                    PingFailed(ping.time);
                    yield break;
                }
                nTime++;
            }
            if (ping.isDone && ping.time != -1)
            {
                yield return ping.time;
                _PingServerState = PingState.PingOK;
                //QLoger.LOG(" NetworkTools  PingConnect success 连线成功: " + ping.time + "ServerIP:" + ServerIP);
            }
            else
            {
                PingFailed(ping.time);
            }
        }
        private void PingFailed(int pingTime)
        {
            //QLoger.ERROR(" NetworkTools  PingConnect failed 连线失败: " + pingTime + "ServerIP:" + ServerIP);
            _PingServerState = PingState.CanNotConnectServer;
            WindowUIManager.Instance.CreateOrAddWindow(WindowUIType.SystemPopupWindow, "提示", "当前没有网，请检测您的网络", new string[] { "确认" }, (index) =>
            {
                Application.Quit();
                //EventDispatcher.FireEvent(GEnum.NamedEvent.Bundle_Restart_Event);
            }, WindowUIRank.Special);
        }

        private void WallowTip()
        {
            float time = MemoryData.Get<float>(MKey.USER_WALLOW_TIME);
            if (time > 0 && Time.realtimeSinceStartup - time > 2 * 60 * 60)//Todo 健康提醒现在2分钟 俩小时
            {
                MemoryData.Set(MKey.USER_WALLOW_TIME, -1);
                WindowUIManager.Instance.CreateOrAddWindow(WindowUIType.SystemPopupWindow, "健康小贴士", "您的累计在线时长已超过两个小时，要注意休息喔", new string[] { "确认" }, WallowTipCallBack);
            }
        }

        private void WallowTipCallBack(int index)
        {
            MemoryData.Set(MKey.USER_WALLOW_TIME, Time.realtimeSinceStartup);
        }



        /// <summary>
        /// 游戏切换到后台后SLEEP
        /// </summary>
        public static void SetToSleep()
        {
            _R.Instance.setToSleep();
            //QLoger.LOG("[Unity _R] 检测到切换后台");
        }

        /// <summary>
        /// 游戏切换到前台时启动
        /// </summary>
        public static void SetToAwake()
        {
            _R.Instance.setToAwake();
            //QLoger.LOG("[Unity _R] 检测到切换前台");


        }

        void OnApplicationPause(bool paused)
        {
            MemoryData.GameStateData.IsPause = paused;
            //程序进入后台时
            if (paused)
            {
                ////如果是在游戏中
                ////------------------------------------
                //if (MjDataManager.Instance.MjData.gameStartNum)
                //{
                //    //ModelNetWorker.OpenReciveAndSend(false);

                //    Debug.LogError("清除缓存");
                //}
                //------------------------------------

                //可以添加本地消息
                QLoger.LOG("程序进入后台时------------------------------");

                //if (ResourceLoadMain.IsAppDown)
                //{
                //    //点击了要下载新的app选项，到后台的时候把游戏关掉
                //    ResourceLoadMain.IsAppDown = false;
                //
                //    Application.Quit();
                //}
            }
            else
            {
#if UNITY_IOS
                GPSManager.Instance.GPSServeResponse(true);
#endif
                //ModelNetWorker.OpenReciveAndSend(true);

                //没网的情况下判断
                if (MemoryData.GameStateData.CurrGameNetWorkStatus == SysGameStateData.EnumGameNetWorkStatus.NotLink
                    && MemoryData.GameStateData.CurrUISign > SysGameStateData.EnumUISign.LoginSucceed)// /* NetWorkerImpl.Instance.state != NetState.NetConnecting && NetWorkerImpl.Instance.state != NetState.NetConnected*/)
                {
                    UserActionManager.AddLocalTypeLog("Log1", "游戏从后台回到前台 并且网络已经断开");
                    EventDispatcher.FireSysEvent(GEnum.NamedEvent.ENetError_Client);
                }
                else
                {

                    if (MemoryData.GameStateData.CurrUISign >= SysGameStateData.EnumUISign.LoginSucceed)
                    {
                        QLoger.LOG("NetWorkerImpl.Instance.state=" + NetWorkerImpl.Instance.state.ToString());

                        //服务器用户状态 
                        if ((MemoryData.UserGameState.MainState == (int)Msg.UserState.State_Game
                            && MemoryData.GameStateData.CurrUISign != SysGameStateData.EnumUISign.PrepareGame)
                            ||
                            MjDataManager.Instance.MjData.gameStartNum //客户端用户状态
                            )
                        { //判断我的服务器状态或者客户端状态为牌桌中的时候发送桌子重连
                            ModelNetWorker.ClearReciveBuffer(); //清除所有服务器发送的返回数据
                            UserActionManager.AddLocalTypeLog("Log1", "游戏从后台回到前台 网络为断开 仅仅发送AskDeskDataReq");

                            //_R.flow.SetQueue(QFlowManager.FlowType.LoginEnd, true, true);
                        }

                        EventDispatcher.FireSysEvent(GEnum.NamedEvent.SysUI_SmallLoading_Open, "AskDeskDataReq", true);
                        ModelNetWorker.Instance.AskDeskDataReq();

                    }
                    else
                    {
                        EventDispatcher.FireSysEvent(GEnum.NamedEvent.SysUI_SmallLoading_Kill);
                    }

                }
                //程序从后台进入前台时
                QLoger.LOG("程序进入前台时++++++++++++++++++++++++++++++");
            }
        }


#if __DEBUG || __DEBUG_CLOSE_CONNECTION
        public void OnGUI()
        {
            if (GUI.Button(new Rect(10, 100, 120, 65), "断开网络"))
            {
                NetWorkerImpl.Instance.CloseConnection();
                UserActionManager.AddLocalTypeLog("Log1", "_R.OnGUI 主动调用关闭网络连接");

            }

            if (MemoryData.GameStateData.CurrGameNetWorkStatus == SysGameStateData.EnumGameNetWorkStatus.LinkOk)
            {
                if (GUI.Button(new Rect(10, 200, 120, 65), "发送重连消息"))
                {
                    UserActionManager.AddLocalTypeLog("Log1", "游戏从后台回到前台 网络为断开 仅仅发送AskDeskDataReq");
                    EventDispatcher.FireSysEvent(GEnum.NamedEvent.SysUI_SmallLoading_Open, "AskDeskDataReq", true);
                    ModelNetWorker.Instance.AskDeskDataReq();
                }
            }
            if (GUI.Button(new Rect(10, 300, 120, 65), "刷新回放数据"))
            {
                //BundleDownApkMgr mBox = new BundleDownApkMgr();
                //mBox.Show();
                //ActionManager _Amanager = new ActionManager();
                //BaseAction ta0 = new TestAction();
                //BaseAction ta1 = new TestAction1();
                //BaseAction ta2 = new TestAction2();
                ////ActionManager.Instance.AddAction(ta0);
                ////ActionManager.Instance.AddAction(ta1);
                //ActionManager.Instance.AddAction(ta2);

                //ParallelAction tap = new ParallelAction();
                //tap.addAction(ta0);
                //tap.addAction(ta1);
                //ActionManager.Instance.AddAction(tap);
                //BaseAction ta3 = new TestAction();
                //ActionManager.Instance.AddAction(ta3);
                FakeReplayManager.Instance.RequestReplayFrame(0,0);
            }

            //if (GUI.Button(new Rect(10, 400, 120, 65), "打开Web界面"))
            //{
            //    WebSDKParams shareParams = new WebSDKParams("WEB_OPEN_MY_CURATOR_PROGRESS");
            //    shareParams.InsertUrlParams(new object[] { 1000000, 1000000, 0 });
            //    SDKManager.Instance.SDKFunction("WEB_OPEN_MY_CURATOR_PROGRESS", shareParams);
            //}
            if (GUI.Button(new Rect(10, 400, 120, 65), "下一個動作"))
            {
            }
            if (GUI.Button(new Rect(10, 500, 120, 65), "加速"))
            {
                Time.timeScale = Time.timeScale == 1 ? 2 : 1;
            }
            //if (GUI.Button(new Rect(10, 400, 120, 65), "屏幕压缩截图"))
            //{
            //    Vector2 startPoint = new Vector2(0.0f, 0.0f);
            //    Vector2 shotSize = new Vector2(Screen.width, Screen.height);
            //    Tools_TexScreenshot.Instance.Texture_Screenshot(startPoint, shotSize, null);
            //}
        }
#endif
    }
}