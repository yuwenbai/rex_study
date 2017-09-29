/**
 * @Author YQC
 *
 *
 */

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace projectQ
{
    public enum EnumChangeSceneType
    {
        /// <summary>
        /// 游戏准备中 -> 主页
        /// </summary>
        GamePrepare_To_Main,

        /// <summary>
        /// 主页 -> 登录
        /// </summary>
        Main_To_Login,

        /// <summary>
        /// 主页 -> 游戏
        /// </summary>
        Main_To_Game,

        /// <summary>
        /// 游戏 -> 主页
        /// </summary>
        Game_To_Main,
    }
    /// <summary>
    /// 流程数据类
    /// </summary>
    public class QFlowManagerData
    {
        public delegate void GoToFunction(QFlowManager.EnumFlowKey lastKey, object[] data);
        public delegate bool CallbackFunction(ref QFlowManager.EnumFlowKey key, ref object[] data);

        //执行方法
        public GoToFunction GotoFunc;
        //回复的执行方法
        public CallbackFunction CallBackFunc;

        //方法的Key
        public QFlowManager.EnumFlowKey Key;
        //回复的Key
        public QFlowManager.EnumFlowKey CallBackKey;

        /// <summary>
        /// 构造方法
        /// </summary>
        public QFlowManagerData(QFlowManager.EnumFlowKey key,GoToFunction GotoFunc,
            QFlowManager.EnumFlowKey CallBackKey = QFlowManager.EnumFlowKey.None, CallbackFunction CallBackFunc = null)
        {
            this.Key = key;
            this.GotoFunc = GotoFunc;
            this.CallBackKey = CallBackKey;
            this.CallBackFunc = CallBackFunc;
        }
    }

    /// <summary>
    /// 流程控制器
    /// </summary>
    public class QFlowManager : BaseManager
    {
        public enum FlowType
        {
            None,
            InitLoginNew,
            InitLogin,

            LoginEnd,
            MahjongReconnect,
            //JoinMahjongScene,
            JoinGame,
            JoinMainLoading,
            JoinMainTween,
            JoinMainScene,
            JoinMain2,
            ChangeScene,

            /// <summary>
            /// 执行微信数据方法
            /// </summary>
            InitDataExecute,
        }
        //所有步骤集合
        private Dictionary<string, QFlowManagerData> FlowMap = new Dictionary<string, QFlowManagerData>();
        //流程队列
        private Queue<QFlowManagerData> FuncQueue = new Queue<QFlowManagerData>();
        //当前的执行流程
        private QFlowManagerData _currExecute = null;
        public QFlowManagerData CurrExecute { get { return _currExecute; } }
        //当前执行流程Type
        private FlowType _currFlow = FlowType.None;
        public FlowType CurrFlow { get { return _currFlow; } }

        //当前流程数据
        private Dictionary<string, object> _currFlowData = null;

        #region API
        public void SetQueueForce(FlowType type,params object[] data)
        {
            if (_currExecute != null)
            {
                DebugPro.Log(DebugPro.EnumLog.System, "警告_流程冲突 强制执行流程", "现未完成流程", this.CurrFlow.ToString(), "要设置的流程", type.ToString());
            }
            ClearQueue();
            var flowInitFunc = GetFlowInitFunction(type);
            if (flowInitFunc != null)
                flowInitFunc(data);
            AppendQueue(type);
            GotoCall(EnumFlowKey.None, data);
        }
        /// <summary>
        /// 设置队列
        /// </summary>
        /// <param name="type"></param>
        public void SetQueue(FlowType type,params object[] data)
        {
            if (_currExecute != null)
            {
                DebugPro.Log(DebugPro.EnumLog.System, "警告_流程冲突", "现未完成流程", this.CurrFlow.ToString(), "要设置的流程",type.ToString());

                if(type != FlowType.LoginEnd)
                    return;
            }
            SetQueueForce(type,data);
        }

        /// <summary>
        /// 续接队列
        /// </summary>
        /// <param name="type"></param>
        /// <param name="data"></param>
        public void AppendQueue(FlowType type)
        {
            this._currFlow = type;
            PushFlowQueue(type);
        }

        #endregion

        #region 私有方法
        #region 处理队列
        /// <summary>
        /// 回调
        /// </summary>
        /// <param name="key"></param>
        /// <param name="data"></param>
        private void CallBack(string key,object[] data)
        {

            if(_currExecute != null && (_currExecute.Key.ToString() == key || _currExecute.CallBackKey.ToString() == key))
            {
                QLoger.LOG("流程回复__" + key);
                EnumFlowKey fk = _currExecute.Key;
                if (_currExecute.CallBackFunc != null)
                {
                    if(!_currExecute.CallBackFunc(ref fk, ref data))
                    {
                        return;
                    }
                }

                GotoCall(fk, data);
            }
        }

        private void GotoCall(EnumFlowKey key, object[] data)
        {
            if (FuncQueue.Count > 0)
            {
                _currExecute = FuncQueue.Dequeue();
                QLoger.LOG("流程发送__" + _currExecute.Key);
                _currExecute.GotoFunc(key, data);
            }
            else
            {
                QLoger.LOG("流程结束");
                ClearQueue();
            }
        }

        /// <summary>
        /// 内部使用的跳到下一个
        /// </summary>
        private void JumpNext(params object[] data)
        {
            GotoCall(_currExecute == null ? EnumFlowKey.None : _currExecute.Key, data);
        }

        /// <summary>
        /// 延迟跳到下一个
        /// </summary>
        /// <param name="time">延迟时间</param>
        /// <param name="data"></param>
        /// <returns></returns>
        private IEnumerator JumpNextDelay(float time,params object[] data)
        {
            yield return new WaitForSeconds(time);
            GotoCall(_currExecute == null ? EnumFlowKey.None : _currExecute.Key, data);
        }

        /// <summary>
        /// 注册的回调消息
        /// </summary>
        /// <param name="data"></param>
        private void CallBackEventAction(object[] data)
        {
            if(data.Length == 2)
            {
                CallBack(data[1] as string, null);
            }
            else if(data.Length > 2)
            {
                object[] temp = new object[data.Length - 2];
                for (int i = 0; i < temp.Length; ++i)
                {
                    temp[i] = data[i + 2];
                }
                CallBack(data[1] as string, temp);
            }
            else
            {
                QLoger.LOG("流程控制器 收到长度错误消息 "+ data.ToString());
            }
        }

        /// <summary>
        /// 加入队列
        /// </summary>
        /// <param name="func"></param>
        private void PushDequeue(QFlowManagerData data)
        {
            FuncQueue.Enqueue(data);
        }

        /// <summary>
        /// 清理队列
        /// </summary>
        private void ClearQueue()
        {
            //清除当前流程
            _currExecute = null;
            _currFlow = FlowType.None;
            _currFlowData = new Dictionary<string, object>();
            FuncQueue.Clear();
        }
       
        #endregion

        #region 处理Map
        private void AddFlowData(QFlowManagerData data)
        {
            string key = data.Key.ToString();
            if (this.FlowMap.ContainsKey(key))
            {
                this.FlowMap[key] = data;
            }
            else
            {
                this.FlowMap.Add(key, data);
            }
        }

        private QFlowManagerData GetFlowData(EnumFlowKey key)
        {
            if(this.FlowMap.ContainsKey(key.ToString()))
            {
                return this.FlowMap[key.ToString()];
            }
            return null;
        }

        #endregion

        #region 当前队列的数据Map
        private void SetCurrMapData(string key,object value)
        {
            if(this._currFlowData.ContainsKey(key))
            {
                this._currFlowData[key] = value;
            }
            else
            {
                this._currFlowData.Add(key, value);
            }
        }
        private T GetCurrMapData<T>(string key)
        {
            if(this._currFlowData.ContainsKey(key))
            {
                return (T) _currFlowData[key];
            }
            return default(T);
        }
        #endregion

        #endregion

        #region 流程队列
        private void PushFlowQueue(FlowType type)
        {
            switch(type)
            {
                case FlowType.InitLoginNew:
                    InitLoginNewFlow();
                    break;
                case FlowType.InitLogin:
                    InitLoginFlow();
                    break;
                //case FlowType.AutoLogin:
                //    AutoLoginFlow();
                //    break;
                case FlowType.LoginEnd:
                    LoginFlow();
                    break;
                case FlowType.MahjongReconnect:
                    MahjongReconnectFlow();
                    break;
                case FlowType.JoinMainLoading:
                    JoinMainLoadingFlow();
                    break;
                case FlowType.JoinMainTween:
                    JoinMainTweenFlow();
                    break;
                case FlowType.JoinMainScene:
                    JoinMainSceneFlow();
                    break;
                case FlowType.JoinMain2:
                    JoinMain2Flow();
                    break;
                case FlowType.ChangeScene:
                    ChangeSceneFlow();
                    break;
                case FlowType.JoinGame:
                    JoinGameFlow();
                    break;
                //case FlowType.JoinMahjongScene:
                //    JoinMahjongSceneFlow();
                //    break;
                case FlowType.InitDataExecute:
                    InitDataExecuteFlow();
                    break;
            }
        }
        private Action<object[]> GetFlowInitFunction(FlowType type)
        {
            switch(type)
            {
                case FlowType.LoginEnd:
                    return LoginFlow_Init;
                case FlowType.MahjongReconnect:
                    return MahjongReconnect_Init;
            }
            return null;
        }
        /// <summary>
        /// 登录流程
        /// </summary>
        private void LoginFlow()
        {
            //请求数据
            PushDequeue(GetFlowData(EnumFlowKey.InitRequestSend));
            //分支
            PushDequeue(GetFlowData(EnumFlowKey.Branch_LoginEnd));
        }
        private void LoginFlow_Init(object[] data)
        {
            //if (data.Length > 0)
            //    this.SetCurrMapData("IsHideConnect", data[0]);
        }

        private void MahjongReconnectFlow()
        {
            //分支
            PushDequeue(GetFlowData(EnumFlowKey.Branch_LoginEnd));
        }
        private void MahjongReconnect_Init(object[] data)
        {
            //是否在游戏中
            this.SetCurrMapData("IsGameIn", MemoryData.GameStateData.IsMahjongGameIn);
            //是否需要切换Mahjong场景
            this.SetCurrMapData("IsLoadMahjongScene", MemoryData.GameStateData.IsLoadMahjongScene);
            //是否在重连中
            this.SetCurrMapData("IsReconnect",true);
        }

        #region 进入主页流程三兄弟
        /// <summary>
        /// 进入主页流程 loading流程
        /// </summary>
        private void JoinMainLoadingFlow()
        {   
            //Kill掉小Loading
            PushDequeue(GetFlowData(EnumFlowKey.KillSmallLoading));
            //开启loading
            PushDequeue(GetFlowData(EnumFlowKey.LoadingStart));
            //关闭Login
            PushDequeue(GetFlowData(EnumFlowKey.CloseUILogin));
            //打开主场景
            PushDequeue(GetFlowData(EnumFlowKey.OpenMainScene));
            //延迟loading
            PushDequeue(GetFlowData(EnumFlowKey.LoadingDelay));
            //打开主页面
            PushDequeue(GetFlowData(EnumFlowKey.MainStart));
            //关闭loading
            PushDequeue(GetFlowData(EnumFlowKey.LoadingEnd));
            //主界面动画打开
            PushDequeue(GetFlowData(EnumFlowKey.PlayUIMainStartTween));
            //分支 检查是否要有自动加入牌桌操作
            PushDequeue(GetFlowData(EnumFlowKey.Branch_CheckInitData));
        }

        /// <summary>
        /// 进入主页流程 切换场景使用 有Loading 无动画
        /// </summary>
        private void JoinMainSceneFlow()
        {
            PushDequeue(GetFlowData(EnumFlowKey.KillSmallLoading));
            //延迟loading
            PushDequeue(GetFlowData(EnumFlowKey.LoadingDelay));
            //打开主页面
            PushDequeue(GetFlowData(EnumFlowKey.MainStart));
            //关闭loading
            PushDequeue(GetFlowData(EnumFlowKey.LoadingEnd));
            //主界面动画打开
            PushDequeue(GetFlowData(EnumFlowKey.PlayUIMainStartTween));
            //分支 检查是否要有自动加入牌桌操作
            PushDequeue(GetFlowData(EnumFlowKey.Branch_CheckInitData));
        }

        /// <summary>
        /// 进入主页动画流程
        /// </summary>
        private void JoinMainTweenFlow()
        {
            PushDequeue(GetFlowData(EnumFlowKey.OpenSmallLoading));
            //登录关闭动画
            PushDequeue(GetFlowData(EnumFlowKey.PlayUILoginEndTween));
            //打开主页面
            PushDequeue(GetFlowData(EnumFlowKey.MainStart));
            //Kill掉小Loading
            PushDequeue(GetFlowData(EnumFlowKey.KillSmallLoading));
            //关闭登录
            PushDequeue(GetFlowData(EnumFlowKey.CloseUILogin));
            //主界面动画打开
            PushDequeue(GetFlowData(EnumFlowKey.PlayUIMainStartTween));
            //分支 检查是否要有自动加入牌桌操作
            PushDequeue(GetFlowData(EnumFlowKey.Branch_CheckInitData));
        }

       
        #endregion
        /// <summary>
        /// 进入主页流程 下集
        /// </summary>
        private void JoinMain2Flow()
        {
            ////首次弹抽奖
            PushDequeue(GetFlowData(EnumFlowKey.FirstActivity));
            //正式进入主页Branch_CheckInitData
            PushDequeue(GetFlowData(EnumFlowKey.MainInit));
            //正式进入主页
            PushDequeue(GetFlowData(EnumFlowKey.Branch_CheckInitData));

            //打开查看成绩界面
            PushDequeue(GetFlowData(EnumFlowKey.Open_DeskViewRecord));
        }

        /// <summary>
        /// 初始化数据执行流程
        /// </summary>
        private void InitDataExecuteFlow()
        {
            //开启遮罩
            PushDequeue(GetFlowData(EnumFlowKey.OpenFullMask));
            //进桌
            PushDequeue(GetFlowData(EnumFlowKey.InitDataExecute));
            //关闭遮罩
            PushDequeue(GetFlowData(EnumFlowKey.CloseFullMask));
        }

        /// <summary>
        /// 进入游戏流程
        /// </summary>
        private void JoinGameFlow()
        {
            //PushDequeue(GetFlowData(EnumFlowKey.LoadingStart));
            //关闭loading
            //PushDequeue(GetFlowData(EnumFlowKey.LoadingEnd));
            //进入游戏中
            PushDequeue(GetFlowData(EnumFlowKey.JoinGameIn));
        }

        ///// <summary>
        ///// 进入麻将场景流程
        ///// </summary>
        //private void JoinMahjongSceneFlow()
        //{
        //    //打开麻将场景
        //    PushDequeue(GetFlowData(EnumFlowKey.OpenMahjongScene));
        //    //关闭loading
        //    PushDequeue(GetFlowData(EnumFlowKey.LoadingEnd));
        //}


        /// <summary>
        /// 初始化登录
        /// </summary>
        private void InitLoginFlow()
        {
            //开启全屏阻挡
            PushDequeue(GetFlowData(EnumFlowKey.OpenFullMask));

            //分支 检查初始化数据
            PushDequeue(GetFlowData(EnumFlowKey.Branch_CheckInitData));
        }

        /// <summary>
        /// 自动登录流程
        ///// </summary>
        //private void AutoLoginFlow()
        //{
        //    //自动登录
        //    PushDequeue(GetFlowData(EnumFlowKey.AutoLogin));
        //    //关闭Mask
        //    PushDequeue(GetFlowData(EnumFlowKey.CloseFullMask));
        //}

        
        private void ChangeSceneFlow()
        {
            //改变场景初始化
            PushDequeue(GetFlowData(EnumFlowKey.ChangeSceneInit));
            //切换场景分支
            PushDequeue(GetFlowData(EnumFlowKey.Branch_ChangeScene));
            ////开启loading
            //PushDequeue(GetFlowData(EnumFlowKey.LoadingStart));
        }

        private void InitLoginNewFlow()
        {
            PushDequeue(GetFlowData(EnumFlowKey.Branch_InitLoginByOpenId));
        }

        #endregion

        #region 步骤
        #region 新初始化登录流程步骤 ====================================

        /// <summary>
        /// 分支
        /// </summary>
        private void Branch_InitLoginByOpenId(EnumFlowKey key, object[] data)
        {
            string openId = MemoryData.LoginDataMgr.LoginData.GetOpenId();
            QLoger.LOG("自动登录ID" + openId);
#if __DEBUG_NOT_AUTO_LOGIN
            openId = null;
#endif
            PushDequeue(GetFlowData(EnumFlowKey.CloseEffect));
            if (string.IsNullOrEmpty(openId) || MemoryData.LoginDataMgr.LoginData.LoginType == SysLoginData.EnumLoginType.NormalLogin)
            {
                EventDispatcher.FireEvent(EventKey.Bundle_LoadingNameShow_Event, "进入游戏");
                EventDispatcher.FireSysEvent(GEnum.NamedEvent.SysUI_Loading_Change, 0.4f,99f);
                //如果是空的话 就是正常流程
                //打开UILogin
                PushDequeue(GetFlowData(EnumFlowKey.OpenLogin));
                //关闭Loading
                PushDequeue(GetFlowData(EnumFlowKey.LoadingEnd));
                _R.Instance.StartCoroutine(JumpNextDelay(0.4f));
            }
            else
            {
                //如果有值的话 
                //连接服务器
                PushDequeue(GetFlowData(EnumFlowKey.LinkServer));
                PushDequeue(GetFlowData(EnumFlowKey.Branch_LinkServer));
                JumpNext();
            }
        }
        //自动连接服务器
        private void LinkServer(EnumFlowKey key, object[] data)
        {
            EventDispatcher.FireEvent(EventKey.Bundle_LoadingNameShow_Event, "连接服务器中...");
            EventDispatcher.FireSysEvent(GEnum.NamedEvent.SysUI_Loading_Change, 1f, 80f);
            //if (LoginEnterNetWork.Instance.IsConnect)
            //{
            //    _R.Instance.StartCoroutine(UITools.WaitExcution(delegate()
            //    {
            //        OnLinkServerCallBack(true);
            //    },0.4f));
            //}
            //else
            {
                LoginEnterNetWork.Instance.Connect(OnLinkServerCallBack);
            }
        }
        private bool LinkServer_CallBack(ref EnumFlowKey key, ref object[] data)
        {
            this.SetCurrMapData("LinkServer", (bool)data[0]);
            return true;
        }

        private void OnLinkServerCallBack(bool isSuccess)
        {
            EventDispatcher.FireSysEvent(GEnum.NamedEvent.SysFlow_Flow_CallBack, "LinkServer", isSuccess);
        }

        private void Branch_LinkServer(EnumFlowKey key, object[] data)
        {
            bool isSuccess = this.GetCurrMapData<bool>("LinkServer");

            if(isSuccess)
            {
                EventDispatcher.FireEvent(EventKey.Bundle_LoadingNameShow_Event, "连接服务器成功");
                EventDispatcher.FireSysEvent(GEnum.NamedEvent.SysUI_Loading_Change, 1f, 90f);
                //连接服务器成功 后登录
                PushDequeue(GetFlowData(EnumFlowKey.Branch_AutoLogin));
            }
            else
            {
                EventDispatcher.FireEvent(EventKey.Bundle_LoadingNameShow_Event, "进入游戏");
                EventDispatcher.FireSysEvent(GEnum.NamedEvent.SysUI_Loading_Change, 1f, 99f);
                //开启登录
                PushDequeue(GetFlowData(EnumFlowKey.OpenLogin));
                //关闭Loading
                PushDequeue(GetFlowData(EnumFlowKey.LoadingEnd));
            }
            JumpNext();
        }

      

        private void Branch_AutoLogin(EnumFlowKey key, object[] data)
        {
            EventDispatcher.FireEvent(EventKey.Bundle_LoadingNameShow_Event, "开始自动登录");
            EventDispatcher.FireSysEvent(GEnum.NamedEvent.SysUI_Loading_Change, 1f, 99f);

            EventDispatcher.FireSysEvent(GEnum.NamedEvent.SysLoginRequest, SysLoginData.EnumLoginType.AutoLogin);
        }
       
        //自动登录回调
        private bool Branch_AutoLogin_CallBack(ref QFlowManager.EnumFlowKey key, ref object[] data)
        {
            bool isSuccess = (bool) data[0];
            if (isSuccess)
            {
                EventDispatcher.FireEvent(EventKey.Bundle_LoadingNameShow_Event, "自动登录成功");
                EventDispatcher.FireSysEvent(GEnum.NamedEvent.SysUI_Loading_Change, 1f, 99f);

                AppendQueue(FlowType.LoginEnd);
            }
            else
            {
                EventDispatcher.FireEvent(EventKey.Bundle_LoadingNameShow_Event, "进入游戏");
                EventDispatcher.FireSysEvent(GEnum.NamedEvent.SysUI_Loading_Change, 0.4f, 99f);
                //开启登录
                PushDequeue(GetFlowData(EnumFlowKey.OpenLogin));
                //关闭Loading
                PushDequeue(GetFlowData(EnumFlowKey.LoadingEnd));
            }

            return true;
        }
#endregion

        /// <summary>
        /// 初始化数据 执行操作
        /// </summary>
        private void InitDataExecute(EnumFlowKey key, object[] data)
        {
            MemoryData.InitData.Execute();
            JumpNext();
        }

        /// <summary>
        /// 开启全屏幕遮挡
        /// </summary>
        private void OpenFullMask(EnumFlowKey key, object[] data)
        {
            _R.ui.SetUIBgActive(true);
            JumpNext();
        }

        /// <summary>
        /// 关闭全屏幕遮挡
        /// </summary>
        private void CloseFullMask(EnumFlowKey key, object[] data)
        {
            _R.ui.SetUIBgActive(false);
            JumpNext();
        }

        /// <summary>
        /// 分支 检查初始化数据
        /// </summary>
        private void Branch_CheckInitData(EnumFlowKey key, object[] data)
        {
            DebugPro.Log(DebugPro.EnumLog.System,"流程判断 Branch_CheckInitData", this.CurrFlow);
            switch(this.CurrFlow)
            {
                case FlowType.InitLogin: //登录之前用来检测是否是自动登录
                    {
                        PushDequeue(GetFlowData(EnumFlowKey.CloseFullMask));
                    }
                    break;
                case FlowType.JoinMainLoading:     //进入主页正常流程 检测是否微信进桌
                case FlowType.JoinMainTween:     //进入主页正常流程 检测是否微信进桌
                case FlowType.JoinMainScene:
                    {
                        if(MemoryData.InitData.GetKey() == WXOpenParaEnum.SHARE_INVITE_PLAY)
                        {
                            PushDequeue(GetFlowData(EnumFlowKey.MainInit));
                            InitDataExecuteFlow();
                        }
                        else
                        {
                            AppendQueue(FlowType.JoinMain2);
                        }
                    }
                    break;
                case FlowType.JoinMain2:
                    {
                        if(MemoryData.InitData.GetKey() == WXOpenParaEnum.SHARE_INVITE_FRIEND_TO_GET_REWARD 
                            || MemoryData.InitData.GetKey() == WXOpenParaEnum.SHARE_INVITE_FRIEND_GO_MYMUSEUM
                            )
                        {
                            InitDataExecuteFlow();
                        }
                    }
                    break;
            }
            JumpNext();
        }

        /// <summary>
        /// 自动登录
        /// </summary>
        private void AutoLogin(EnumFlowKey key, object[] data)
        {
            LoginEnterNetWork.Instance.AutoLogin();
            JumpNext();
        }

        /// <summary>
        /// 开启Loading
        /// </summary>
        private void LoadingStart(EnumFlowKey key, object[] data)
        {
            //开启大loading时候必回关闭小Loading
            EventDispatcher.FireSysEvent(GEnum.NamedEvent.SysUI_SmallLoading_Kill);
            CommonTools.Tools_SetEffectState(false);
            int loadingKey = this.GetCurrMapData<int>("UIResourceLoad_InitData");

            bool isHideConnect = this.GetCurrMapData<bool>("IsHideConnect");
            if(isHideConnect)
            {
                JumpNext();
            }
            else
            {
                _R.ui.OpenUI("UIResourceLoad", "",1);
            }
        }

        //初始化请求发送
        private void InitRequestSend(EnumFlowKey key, object[] data)
        {
            if(data != null && data.Length > 0)
            {
                this.SetCurrMapData("IsReconnect", data[0]);
            }
            else
            {
                this.SetCurrMapData("IsReconnect", false);
            }


            var uidefault = _R.ui.GetUI("UIDefault") as UIDefault;


            if (data.Length >= 2 && (bool)data[1])
            {
                uidefault.Model.InitRequestSend(true);
            }
            else
            {
                uidefault.Model.InitRequestSend(false);
            }

            if (MemoryData.GameStateData.IsJoinMahjongSceneNew)
            {
                MemoryData.GameStateData.IsJoinMahjongSceneNew = false;
                this.SetCurrMapData("IsLoadMahjongScene", MemoryData.GameStateData.IsLoadMahjongScene);
                this.SetCurrMapData("IsGameIn", MemoryData.GameStateData.IsMahjongGameIn);
                JumpNext();
            }
        }
        private bool InitRequestSend_CallBack(ref QFlowManager.EnumFlowKey key, ref object[] data)
        {
            if(data != null && data.Length > 0)
            {
                string k = data[0] as string;
                if(k == "LoadMahjongScene")
                {
                    this.SetCurrMapData("IsLoadMahjongScene", MemoryData.GameStateData.IsLoadMahjongScene);
                    this.SetCurrMapData("IsGameIn", MemoryData.GameStateData.IsMahjongGameIn);
                    MemoryData.GameStateData.IsJoinMahjongSceneNew = false;

                    return true;
                }
            }
            return false;
        }

        //加载主页面
        private void MainStart(EnumFlowKey key, object[] data)
        {
            bool isJump = false;
            if(!_R.ui.IsShowUI("UIMain"))
            {
                _R.ui.OpenUI("UIMain");
            }
            else
            {
                isJump = true;
                //TempOpenUI("UIMain");
                //TempOpenUI("UICuratorList");
                //TempOpenUI("UIClienteleView");
            }
            if (!_R.ui.IsShowUI("UIFriendList"))
            {
                _R.ui.OpenUI("UIFriendList");
            }
            if (isJump)
            {
                JumpNext();
            }
        }
        private void TempOpenUI(string uiName)
        {
            if (_R.ui.IsShowUI(uiName))
            {
                var ui = _R.ui.GetUI(uiName);
                int depth = ui.GetDepth();
                _R.ui.OpenUI(uiName);
                ui.SetDepth(depth);
            }
        }

        //关闭Loading
        private void LoadingEnd(EnumFlowKey key, object[] data)
        {
            bool isHideConnect = this.GetCurrMapData<bool>("IsHideConnect");
            if (isHideConnect)
            {
                JumpNext();
            }
            else
            {
                if(_R.ui.IsShowUI("UIResourceLoad"))
                {
                    EventDispatcher.FireSysEvent(GEnum.NamedEvent.SysUI_Loading_Change, 0f, 100f);
                }
                else
                {
                    JumpNext();
                }
            }
        }
        private bool LoadingEnd_CallBack(ref QFlowManager.EnumFlowKey key, ref object[] data)
        {
            _R.Instance.StartCoroutine(UITools.WaitExcution(() => {
                CommonTools.Tools_SetEffectState(true);
            },1));
            return true;
        }

        private void LoadingDelay(EnumFlowKey key, object[] data)
        {
            bool isR = this.GetCurrMapData<bool>("IsReconnect");
            if(isR)
            {
                EventDispatcher.FireSysEvent(GEnum.NamedEvent.SysUI_Loading_Change, 0.01f, 99.9f);
                JumpNext();
            }
            else
            {
                EventDispatcher.FireSysEvent(GEnum.NamedEvent.SysUI_Loading_Change, 0.5f, 99.9f);
                _R.Instance.StartCoroutine(JumpNextDelay(0.5f));
            }
        }

        //首次打开地图
        private void FirstMap(EnumFlowKey key, object[] data)
        {
            if (MemoryData.PlayerData.MyPlayerModel.PlayerDataBase.RegionID == 0)
            {
                _R.ui.OpenUI("UIMap");
            }
            else
            {
                JumpNext();
            }
        }

        //第一次登录后消息弹出
        void OnFirstMessage(int index)
        {
            JumpNext();
        }
        private void FirstMessage(EnumFlowKey key, object[] data)
        {
            Action<int> onFirstMessage = OnFirstMessage;
            EventDispatcher.FireSysEvent(GEnum.NamedEvent.SysUI_MsgPopAll, onFirstMessage);
        }

        //每日抽奖
        private void FirstActivity(EnumFlowKey key, object[] data)
        {
#if !__BUNDLE_IOS_SERVER
            if (MemoryData.PlayerData.MyPlayerModel.PlayerDataBase.TodayFirstLogin)
            {
                // _R.ui.OpenUI("UIActivity", "FirstLogin");
                EventDispatcher.FireSysEvent(GEnum.NamedEvent.SysUI_OpenBannerReq, null);
                MemoryData.PlayerData.MyPlayerModel.PlayerDataBase.TodayFirstLogin = false;
            }
            else
#endif
            {
                JumpNext();
            }
        }
        //打开战绩查看界面
        private void Open_DeskViewRecord(EnumFlowKey key, object[] data)
        {
            if (MemoryData.PlayerData.MyPlayerModel.PlayerDataBase.DeskViewRecordDeskId > 0 )
            {
                EventDispatcher.FireSysEvent(GEnum.NamedEvent.SysUI_UIReplayCtrl_Play_ViewRecord, MemoryData.PlayerData.MyPlayerModel.PlayerDataBase.DeskViewRecordDeskId);
                MemoryData.PlayerData.MyPlayerModel.PlayerDataBase.DeskViewRecordDeskId = -1;
            }
            JumpNext();
        }

        ////主页init之前 初始化数据执行
        //private void InitDataExecuteMainInitPre(EnumFlowKey key, object[] data)
        //{
        //    if(MemoryData.InitData.GetKey() == WXOpenParaEnum.SHARE_INVITE_FRIEND_GO_MYMUSEUM
        //        || MemoryData.InitData.GetKey() == WXOpenParaEnum.SHARE_INVITE_FRIEND_TO_GET_REWARD
        //        )
        //    {
        //        InitDataExecuteFlow();
        //    }
        //    JumpNext();
        //}

        //主页Init
        private void MainInit(EnumFlowKey key, object[] data)
        {
            MemoryData.GameStateData.SetCurrUISign(SysGameStateData.EnumUISign.MainIn);
            //MemoryData.OtherData.CheckGPSServer();
            JumpNext();
        }


        //登录结束后的分支 
        private void Branch_LoginEnd(EnumFlowKey key,object[] data)
        {
            //是否在游戏中
            bool IsGameIn = this.GetCurrMapData<bool>("IsGameIn");
            //是否需要切换Mahjong场景
            bool IsLoadMahjongScene = this.GetCurrMapData<bool>("IsLoadMahjongScene");


            //是否在重连中
            bool isReconnect = this.GetCurrMapData<bool>("IsReconnect");
            if(isReconnect || IsLoadMahjongScene || IsGameIn)
            {
                this.SetCurrMapData("ReconnectTarget", IsLoadMahjongScene || IsGameIn ? 1: 0);
                PushDequeue(GetFlowData(EnumFlowKey.ReconnectLogin));
            }
            else
            {
                PushDequeue(GetFlowData(EnumFlowKey.Branch_JoinMainCheckRoom));
            }
            JumpNext();
        }


        //进入游戏中
        private void JoinGameIn(EnumFlowKey key, object[] data)
        {
            //MemoryData.GameStateData.IsReconectMahjongGameIn = true;
            MemoryData.GameStateData.SetCurrUISign(SysGameStateData.EnumUISign.GameIn);
            JumpNext();
        }

        //打开麻将场景
        private void OpenMahjongScene(EnumFlowKey key, object[] data)
        {
            if (_R.scene.GetCurrSceneName() != "MahJong")
            {
                _R.scene.SetScene(new MahjongScene(), "MahJong");
                EventDispatcher.FireSysEvent(GEnum.NamedEvent.SysUI_Loading_Change, 0.5f, 50f);
            }
            else
            {
                _R.Instance.StartCoroutine(this.JumpNextDelay(0.5f));
            }
        }

        //打开主场景
        private void OpenMainScene(EnumFlowKey key, object[] data)
        {
            if(_R.scene.GetCurrSceneName() != "Main")
            {
                EventDispatcher.FireSysEvent(GEnum.NamedEvent.SysUI_Loading_Change, 0.5f, 50f);
                _R.scene.SetScene(new MainScene(), "Main");
            }
            else
            {
                EventDispatcher.FireSysEvent(GEnum.NamedEvent.SysUI_Loading_Change, 0.2f, 99f);
                _R.Instance.StartCoroutine(this.JumpNextDelay(0.2f));
            }
        }
        private bool OpenScene_CallBack(ref EnumFlowKey key,ref object[] data)
        {
            EventDispatcher.FireSysEvent(GEnum.NamedEvent.SysUI_Loading_Change, 0.2f, 99f);

            _R.Instance.StartCoroutine(this.JumpNextDelay(0.1f));
            return false;
        }

        private void ChangeSceneInit(EnumFlowKey key, object[] data)
        {
            EnumChangeSceneType type;
            string sceneName = string.Empty;
            if (data != null && data.Length > 0)
            {
                type = (EnumChangeSceneType)data[0];
            }
            else
            {
                type = this.GetCurrMapData<EnumChangeSceneType>("ChangeSceneType");
            }

            switch(type)
            {
                case EnumChangeSceneType.Main_To_Login:
                    sceneName = "Login";
                    break;

                case EnumChangeSceneType.Game_To_Main:
                case EnumChangeSceneType.GamePrepare_To_Main:
                    sceneName = "Main";
                    break;

                case EnumChangeSceneType.Main_To_Game:
                    sceneName = "MahJong";
                    break;
            }

            this.SetCurrMapData("ChangeSceneName", sceneName);
            this.SetCurrMapData("IsChangeScene", sceneName != _R.scene.GetCurrSceneName());

            JumpNext();
        }

        //切换场景分支
        private void Branch_ChangeScene(EnumFlowKey key, object[] data)
        {
            bool IsChangeScene = this.GetCurrMapData<bool>("IsChangeScene");
            string ChangeSceneName = this.GetCurrMapData<string>("ChangeSceneName");

            if (IsChangeScene || MemoryData.GameStateData.BigLoadingActive)
            {
                PushDequeue(GetFlowData(EnumFlowKey.LoadingStart));
            }
            else
            {
                PushDequeue(GetFlowData(EnumFlowKey.OpenSmallLoading));
            }


            switch (ChangeSceneName)
            {
                case "Main":
                    PushDequeue(GetFlowData(EnumFlowKey.OpenMainScene));
                    AppendQueue(FlowType.JoinMainScene);
                    this.SetCurrMapData("UIResourceLoad_InitData", 1);
                    break;
                case "Login": 
                    PushDequeue(GetFlowData(EnumFlowKey.OpenMainScene));
                    PushDequeue(GetFlowData(EnumFlowKey.OpenLogin));
                    //PushDequeue(GetFlowData(EnumFlowKey.LoadingEnd));
                    this.SetCurrMapData("UIResourceLoad_InitData", 0);
                    break;
                case "MahJong":  
                    PushDequeue(GetFlowData(EnumFlowKey.OpenMahjongScene));
                    //PushDequeue(GetFlowData(EnumFlowKey.LoadingEnd));
                    this.SetCurrMapData("UIResourceLoad_InitData", 1);
                    break;
            }


            if (IsChangeScene || MemoryData.GameStateData.BigLoadingActive)
            {
                PushDequeue(GetFlowData(EnumFlowKey.LoadingEnd));
            }
            PushDequeue(GetFlowData(EnumFlowKey.KillSmallLoading));
            JumpNext();
        }

        /// <summary>
        /// 打开登录界面
        /// </summary>
        private void OpenLogin(EnumFlowKey key, object[] data)
        {
            _R.ui.OpenUI("UILogin");
            MemoryData.GameStateData.SetCurrUISign(SysGameStateData.EnumUISign.Login);
        }


        /// <summary>
        /// 播放登录结束动画
        /// </summary>PlayUILoginEndTween
        private void PlayUILoginEndTween(EnumFlowKey key, object[] data)
        {
            EventDispatcher.FireSysEvent(GEnum.NamedEvent.SysUI_PlayUILoginEndTween);
        }


        /// <summary>
        /// 播放主页启动动画
        /// </summary>
        private void PlayUIMainStartTween(EnumFlowKey key, object[] data)
        {
            bool flag = this.GetCurrMapData<bool>("PlayUIMainStartTween_flag");

            EventDispatcher.FireSysEvent(GEnum.NamedEvent.SysUI_PlayUIMainStartTween, flag);
        }

        /// <summary>
        /// 分支 进入主界面之前要检查是否要开启loading
        /// </summary>
        private void Branch_JoinMainCheckRoom(EnumFlowKey key, object[] data)
        {
            if(MemoryData.PlayerData.MyPlayerModel.PlayerDataBase.IsHaveMjRoom()
                || !_R.ui.IsShowUI("UILogin"))
            {
                this.SetCurrMapData("PlayUIMainStartTween_flag", false);
                //进入Loading流程
                AppendQueue(FlowType.JoinMainLoading);
            }
            else
            {
                this.SetCurrMapData("PlayUIMainStartTween_flag", true);
                //进入有动画的流程
                AppendQueue(FlowType.JoinMainTween);
            }
            JumpNext();
        }

        /// <summary>
        /// 关闭登录界面
        /// </summary>
        private void CloseUILogin(EnumFlowKey key, object[] data)
        {
            if(_R.ui.GetUI("UILogin") != null)
            {
                _R.ui.GetUI("UILogin").Close();
            }
            else
            {
                JumpNext();
            }
        }

        /// <summary>
        /// 打开小Loading
        /// </summary>
        private void OpenSmallLoading(EnumFlowKey key, object[] data)
        {
            EventDispatcher.FireSysEvent(GEnum.NamedEvent.SysUI_SmallLoading_Open,"");
            JumpNext();
        }

        /// <summary>
        /// 关闭小Loading
        /// </summary>
        private void KillSmallLoading(EnumFlowKey key, object[] data)
        {
            EventDispatcher.FireSysEvent(GEnum.NamedEvent.SysUI_SmallLoading_Kill);
            JumpNext();
        }

        public enum LoadingType
        {
            BigLoading,
            SmallLoading,
        }
        private void OpenSmallOrBigLoading(EnumFlowKey key, object[] data)
        {
            switch(this.GetCurrMapData<LoadingType>("LoadingType"))
            {
                case LoadingType.BigLoading:
                    this.LoadingStart(key, data);
                    break;
                case LoadingType.SmallLoading:
                    this.OpenSmallLoading(key, data);
                    break;
            }
        }
        private void CloseSmallOrBigLoading(EnumFlowKey key, object[] data)
        {
            switch (this.GetCurrMapData<LoadingType>("LoadingType"))
            {
                case LoadingType.BigLoading:
                    this.LoadingEnd(key, data);
                    break;
                case LoadingType.SmallLoading:
                    this.OpenSmallLoading(key, data);
                    break;
            }
        }

        private void CloseEffect(EnumFlowKey key, object[] data)
        {
            CommonTools.Tools_SetEffectState(false);
            JumpNext();
        }


        /// <summary>
        /// 重连登录
        /// </summary>
        private void ReconnectLogin(EnumFlowKey key, object[] data)
        {
            _R.ui.OpenUI("UIFriendList");
            int ReconnectTarget = this.GetCurrMapData<int>("ReconnectTarget");
            //是否需要进入MahjongScene

            switch (ReconnectTarget)
            {
                //进入主页
                case 0:
                    this.SetCurrMapData("ChangeSceneType", EnumChangeSceneType.Game_To_Main);
                    AppendQueue(FlowType.ChangeScene);
                    break;

                //进入麻将场景
                case 1:
                    this.SetCurrMapData("ChangeSceneType", EnumChangeSceneType.Main_To_Game);
                    AppendQueue(FlowType.ChangeScene);
                    break;
            }

            if (this.GetCurrMapData<bool>("IsGameIn"))
            {
                PushDequeue(GetFlowData(EnumFlowKey.JoinGameIn));
            }
            JumpNext();
        }


        #endregion

        #region 对应关系
        public enum EnumFlowKey
        {
            None = 0,

            InitDataExecute,//初始化数据执行操作

            OpenFullMask,
            CloseFullMask,
            //AutoLogin,              //自动登录
            CloseUILogin,
            LoadingStart,
            LoadingEnd,
            InitRequestSend,        //初始化数据
            MainStart,              //主页面开始
            LoadingDelay,           //Loading 延迟
            FirstMap,               //首次打开地图
            FirstMessage,           //首次发送消息
            FirstActivity,          //首次摇奖
            //CheckInitData,          //主页初始化之前
            MainInit,               //主页面初始化
            //JoinDesk,               //进入牌桌
            //JoinDesk_CallBack,      //进入牌桌的回复
            JoinGameIn,             //进入游戏中
            OpenMahjongScene,       //打开麻将场景
            OpenMainScene,          //打开主场景
            ChangeSceneInit,        //切换场景初始化

            OpenLogin,              //打开登录界面
            LinkServer,             //连接服务器
            LinkServer_CallBack,
            ReconnectLogin,

            /// <summary>
            /// 打开小loading
            /// </summary>
            OpenSmallLoading,

            /// <summary>
            /// 关闭小loading
            /// </summary>
            KillSmallLoading,       

            /// <summary>
            /// 播放登录结束动画
            /// </summary>
            PlayUILoginEndTween,

            /// <summary>
            /// 播放主页启动动画
            /// </summary>
            PlayUIMainStartTween,
            
            /// <summary>
            /// 关闭特效
            /// </summary>
            CloseEffect,

            //分支
            Branch_LoginEnd,        //登录结束后的分支
            Branch_CheckInitData,   //检查初始化数据
            Branch_ChangeScene,     //切换场景分支
            Branch_JoinMainCheckRoom,//进入主界面时检查是否麻将馆要开启Loading 分支
            Branch_InitLoginByOpenId,//初始化登录流程根据openId
            Branch_LinkServer,
            Branch_AutoLogin,
            Branch_AutoLogin_CallBack,

            //无功能性 回复Key使用
            Open_UILogin,           //登录界面打开
            Close_UILogin,          //登录界面关闭
            Open_UIResourceLoad,    //loading打开
            Close_UIResourceLoad,   //loading关闭
            Open_UIMain,            //主界面打开
            Close_UIMap,            //地图关闭
            Close_UIActivity,       //摇奖关闭

            OpenScene_MahJong,      //打开麻将场景
            CloseScene_MahJong,     //关闭麻将场景
            OpenScene_Main,         //打开主场景
            CloseScene_Main,        //关闭主场景
            Close_UIBanner,         //关闭UIBanner
            Open_DeskViewRecord,    //打开对应牌桌战绩
        }
        private void InitMap()
        {
            FlowMap.Clear();

            //初始化执行操作
            this.AddFlowData(new QFlowManagerData(EnumFlowKey.InitDataExecute, InitDataExecute));
            //开启全屏遮挡
            this.AddFlowData(new QFlowManagerData(EnumFlowKey.OpenFullMask, OpenFullMask));
            //关闭全屏遮挡
            this.AddFlowData(new QFlowManagerData(EnumFlowKey.CloseFullMask, CloseFullMask));
            //分支 检查初始化数据
            this.AddFlowData(new QFlowManagerData(EnumFlowKey.Branch_CheckInitData, Branch_CheckInitData));
            //自动登录
            //this.AddFlowData(new QFlowManagerData(EnumFlowKey.AutoLogin, AutoLogin));
            

            this.AddFlowData(new QFlowManagerData(EnumFlowKey.LoadingStart, LoadingStart, EnumFlowKey.Open_UIResourceLoad));
            this.AddFlowData(new QFlowManagerData(EnumFlowKey.LoadingEnd,           LoadingEnd,    EnumFlowKey.Close_UIResourceLoad,LoadingEnd_CallBack));
            this.AddFlowData(new QFlowManagerData(EnumFlowKey.InitRequestSend,      InitRequestSend ,EnumFlowKey.None,InitRequestSend_CallBack));
            this.AddFlowData(new QFlowManagerData(EnumFlowKey.MainStart,            MainStart,     EnumFlowKey.Open_UIMain));

            this.AddFlowData(new QFlowManagerData(EnumFlowKey.FirstMap,             FirstMap,      EnumFlowKey.Close_UIMap));
            this.AddFlowData(new QFlowManagerData(EnumFlowKey.FirstMessage,         FirstMessage));
            this.AddFlowData(new QFlowManagerData(EnumFlowKey.FirstActivity,        FirstActivity, EnumFlowKey.Close_UIBanner));
            this.AddFlowData(new QFlowManagerData(EnumFlowKey.MainInit, MainInit));
            this.AddFlowData(new QFlowManagerData(EnumFlowKey.Open_DeskViewRecord, Open_DeskViewRecord));

            //this.AddFlowData(new QFlowManagerData(EnumFlowKey.JoinDesk, JoinDesk, EnumFlowKey.JoinDesk_CallBack, JoinDesk_CallBack));


            //分支 登录成功后
            this.AddFlowData(new QFlowManagerData(EnumFlowKey.Branch_LoginEnd, Branch_LoginEnd));
            //进入游戏
            this.AddFlowData(new QFlowManagerData(EnumFlowKey.JoinGameIn, JoinGameIn));
            //打开麻将场景
            this.AddFlowData(new QFlowManagerData(EnumFlowKey.OpenMahjongScene, OpenMahjongScene, EnumFlowKey.OpenScene_MahJong, OpenScene_CallBack));
            //打开主场景
            this.AddFlowData(new QFlowManagerData(EnumFlowKey.OpenMainScene, OpenMainScene,EnumFlowKey.OpenScene_Main, OpenScene_CallBack));

            this.AddFlowData(new QFlowManagerData(EnumFlowKey.ChangeSceneInit, ChangeSceneInit));
            this.AddFlowData(new QFlowManagerData(EnumFlowKey.Branch_ChangeScene, Branch_ChangeScene));

            this.AddFlowData(new QFlowManagerData(EnumFlowKey.OpenLogin, OpenLogin, EnumFlowKey.Open_UILogin));

            //
            
            this.AddFlowData(new QFlowManagerData(EnumFlowKey.PlayUILoginEndTween, PlayUILoginEndTween));
            this.AddFlowData(new QFlowManagerData(EnumFlowKey.PlayUIMainStartTween, PlayUIMainStartTween));
            this.AddFlowData(new QFlowManagerData(EnumFlowKey.Branch_JoinMainCheckRoom, Branch_JoinMainCheckRoom));
            this.AddFlowData(new QFlowManagerData(EnumFlowKey.CloseUILogin, CloseUILogin, EnumFlowKey.Close_UILogin));
            this.AddFlowData(new QFlowManagerData(EnumFlowKey.LoadingDelay, LoadingDelay));
            
            this.AddFlowData(new QFlowManagerData(EnumFlowKey.OpenSmallLoading, OpenSmallLoading));
            this.AddFlowData(new QFlowManagerData(EnumFlowKey.KillSmallLoading, KillSmallLoading));

            
            this.AddFlowData(new QFlowManagerData(EnumFlowKey.Branch_InitLoginByOpenId, Branch_InitLoginByOpenId));
            this.AddFlowData(new QFlowManagerData(EnumFlowKey.LinkServer, LinkServer, EnumFlowKey.LinkServer_CallBack, LinkServer_CallBack));
            this.AddFlowData(new QFlowManagerData(EnumFlowKey.Branch_LinkServer, Branch_LinkServer));
            this.AddFlowData(new QFlowManagerData(EnumFlowKey.Branch_AutoLogin, Branch_AutoLogin, EnumFlowKey.Branch_AutoLogin_CallBack, Branch_AutoLogin_CallBack));
            
            this.AddFlowData(new QFlowManagerData(EnumFlowKey.CloseEffect, CloseEffect));
            this.AddFlowData(new QFlowManagerData(EnumFlowKey.ReconnectLogin, ReconnectLogin));
            
        }
#endregion

#region override
        public override void Init()
        {
            InitMap();
            EventDispatcher.AddEvent(GEnum.NamedEvent.SysFlow_Flow_CallBack, CallBackEventAction);
        }

        public override void Dispose()
        {
            EventDispatcher.AddEvent(GEnum.NamedEvent.SysFlow_Flow_CallBack, CallBackEventAction);
        }
#endregion
    }
}