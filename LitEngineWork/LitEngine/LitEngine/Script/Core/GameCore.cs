using UnityEngine;
namespace LitEngine
{
    using Loader;
    public class GameCore : CoreBase
    {

        #region 类变量
        private AppCore mParentCore;
        protected bool mIsInited = false;
        public string AppName
        {
            get;
            private set;
        }
        #endregion

        #region 管理器
        public ScriptManager SManager
        {
            get;
            private set;
        }
        public LoaderManager LManager
        {
            get;
            private set;
        }
        public GameUpdateManager GManager
        {
            get;
            private set;
        }

        public CoroutineManager CManager
        {
            get;
            private set;
        }
        #endregion
        #region 初始化
        protected GameCore()
        {

        }
        protected GameCore(AppCore _core,string _appname)
        {
            AppName = _appname;
            mParentCore = _core;
        }
        public void InitGameCore(UseScriptType _scripttype)
        {
            if(!CheckInited(false))
            {
                DLog.LOG(DLogType.Error, "不允许重复初始化GameCore,请检查代码");
                return;
            }

            SManager = new ScriptManager(_scripttype);

            GameObject tobj = new GameObject("LoadManager-"+ AppName);
            GameObject.DontDestroyOnLoad(tobj);
            LManager = tobj.AddComponent<LoaderManager>();

            tobj = new GameObject("GameUpdateManager-" + AppName);
            GameObject.DontDestroyOnLoad(tobj);
            GManager = tobj.AddComponent<GameUpdateManager>();

            tobj = new GameObject("CoroutineManager-" + AppName);
            GameObject.DontDestroyOnLoad(tobj);
            CManager = tobj.AddComponent<CoroutineManager>();

            mIsInited = true;
        }

        public bool CheckInited(bool _need)
        {
            if (mIsInited != _need)
            {
                DLog.LOG(DLogType.Error, string.Format("GameCore的初始化状态不正确:Inited = {0} needstate = {1}", mIsInited, _need));
                return false;
            }
            return true ;
        }


        #endregion
        #region 释放
        
        override protected void DisposeNoGcCode()
        {
            ScriptInterface.BehaviourInterfaceBase[] tobjs = Component.FindObjectsOfType<ScriptInterface.BehaviourInterfaceBase>();
            for (int i = tobjs.Length - 1; i >= 0; i--)
            {
                if (!tobjs[i].mAppName.Equals(AppName)) continue;
                tobjs[i].ClearScriptObject();
                Component.DestroyImmediate(tobjs[i]);
            }

            GManager.DestroyManager();
            CManager.DestroyManager();
            LManager.DestroyManager();
            SManager.DestroyManager();

           

            GManager = null;
            LManager = null;
            SManager = null;
            CManager = null;
        }
        #endregion

    }
}
