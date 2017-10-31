using UnityEngine;
using ILRuntime.CLR.TypeSystem;
using System;
namespace LitEngine
{
    namespace ScriptInterface
    {
        public class BehaviourInterfaceBase : MonoBehaviour
        {
            #region 成员
            protected const string cNeedSetAppName = "需要设置AppName";
            protected bool mIsDestory = false;
            protected bool mInitScript = false;

            public string mAppName = cNeedSetAppName;
            protected CodeToolBase mCodeTool = null;
            protected GameUpdateManager mUpdateManager = null;

            public string mScriptClass = "";
            protected IType mScriptType;
            protected object mObject = null;

            public object ScriptObject
            {
                get { return mObject; }
            }

            #region update
            protected UpdateObject mUpdateDelegate = null;
            protected UpdateObject mFixedUpdateDelegate = null;
            protected UpdateObject mLateUpdateDelegate = null;
            protected UpdateObject mOnGUIDelegate = null;
            protected UpdateGroup mUpdateGroup = null;

            #endregion
            #endregion

            #region 设置update间隔
            public UpdateGroup OpenGroup(float _time)
            {
                if (mUpdateGroup == null)
                    mUpdateGroup = new UpdateGroup();
                mUpdateGroup.MaxTime = _time;

                mUpdateManager.UnRegUpdate(mUpdateGroup);
                mUpdateManager.RegUpdate(mUpdateGroup);
                return mUpdateGroup;
            }
            public void SetUpdateInterval(float _time)
            {
                if (mUpdateDelegate != null)
                    mUpdateDelegate.MaxTime = _time;
            }
            public void SetFixedUpdateInterval(float _time)
            {
                if (mFixedUpdateDelegate != null)
                    mFixedUpdateDelegate.MaxTime = _time;
            }
            public void SetLateUpdateInterval(float _time)
            {
                if (mLateUpdateDelegate != null)
                    mLateUpdateDelegate.MaxTime = _time;
            }
            public void SetOnGUIInterval(float _time)
            {
                if (mOnGUIDelegate != null)
                    mOnGUIDelegate.MaxTime = _time;
            }
            #endregion

            #region 脚本初始化以及析构

            public BehaviourInterfaceBase()
            {

            }
            ~BehaviourInterfaceBase()
            {
            }

            virtual public void ClearScriptObject()
            {
                if(mIsDestory)
                {
                    return;
                }
                mIsDestory = true;
                UnRegAll();
                mUpdateGroup = null;
                mOnGUIDelegate = null;
                mLateUpdateDelegate = null;
                mFixedUpdateDelegate = null;
                mUpdateDelegate = null;
                mObject = null;
                mScriptType = null;
                mCodeTool = null;
                mUpdateManager = null;
                mInitScript = false;
            }

            virtual protected void InitParamList()
            {
                mUpdateDelegate = mCodeTool.GetUpdateObjectAction("Update", mScriptClass, ScriptObject);
                mFixedUpdateDelegate = mCodeTool.GetUpdateObjectAction("FixedUpdate", mScriptClass, ScriptObject);
                mLateUpdateDelegate = mCodeTool.GetUpdateObjectAction("LateUpdate", mScriptClass, ScriptObject);
                mOnGUIDelegate = mCodeTool.GetUpdateObjectAction("OnGUI", mScriptClass, ScriptObject);
            }
            virtual public void InitScript(string _class,string _AppName)
            {
                if (_class.Length == 0 || mInitScript) return;
                if(_AppName.Equals(cNeedSetAppName))
                {
                    DLog.LOGColor(DLogType.Error,string.Format( "必须设置正确的AppName,主App可等于空字串.Class = {0},GameObject = {1}", _class,gameObject.name), LogColor.YELLO);
                    return;
                }
                try {

                    mAppName = _AppName.Length > 0 ? _AppName:AppCore.sMainGameKey;
                    DLog.LOGFormat(DLogType.Log, "App's name :", mAppName);
                    mCodeTool = AppCore.App[mAppName].SManager.CodeTool;
                    mUpdateManager = AppCore.App[mAppName].GManager;

                    mScriptClass = _class;
                    mScriptType = mCodeTool.GetLType(mScriptClass);
                    mObject = mCodeTool.GetCSLEObjectParmasByType(mScriptType, this);
                    InitParamList();
                    CallScriptFunctionByName("Awake");
                    mInitScript = true;
                    mIsDestory = false;
                }
                catch (Exception _erro)
                {
                    DLog.LOG(DLogType.Error, string.Format("脚本初始化出错:Class = {0},AppName = {1},GameObject = {2},InitScript ->{3}", mScriptClass, mAppName,gameObject.name, _erro.ToString()));
                }
                
                
            }
            #endregion
            #region 调用脚本函数

            virtual public object CallScriptFunctionByNameStringPam(string _FunctionName, string _stringprams)
            {
                return CallScriptFunctionByNamePram(_FunctionName, _stringprams);
            }

            virtual public object CallScriptFunctionByName(string _FunctionName)
            {
                return CallScriptFunctionByNameParams(_FunctionName);
            }

            virtual public object CallScriptFunctionByNameUObject(string _FunctionName, UnityEngine.Object _UObject)
            {
                return CallScriptFunctionByNamePram(_FunctionName, _UObject);
            }


            virtual public object CallScriptFunctionByNamePram(string _FunctionName, object _value)
            {
                return CallScriptFunctionByNameParams(_FunctionName, _value);
            }

            virtual public object CallScriptFunctionByNameParams(string _FunctionName, params object[] _prams)
            {
                try {
                    if (mObject == null) return null;
                    int tpramcount = _prams != null ? _prams.Length : 0;
                    return mCodeTool.CallMethod(mCodeTool.GetLMethod(mScriptType, _FunctionName, tpramcount), mObject, _prams);
                }
                catch (Exception _erro)
                {
                    DLog.LOG(DLogType.Error, string.Format("函数调用出错:Class:{0} GameObject:{1} CallScriptFunctionByNameParams ->{2}",mScriptClass,gameObject.name, _erro.ToString()));
                }
                return null;
            }
            #endregion
            #region Unity 
            virtual protected void Awake()
            {
                InitScript(mScriptClass, mAppName);
            }
            virtual protected void Start()
            {
                CallScriptFunctionByName("Start");
                RegAll();
            }
            virtual protected void OnDestroy()
            {
                CallScriptFunctionByName("OnDestroy");
                ClearScriptObject();
            }
            #endregion
            #region 注册与卸载
            virtual protected void UnRegAll()
            {

                if (mUpdateGroup != null)
                    mUpdateManager.UnRegUpdate(mUpdateGroup);

                if (mUpdateDelegate != null)
                    mUpdateManager.UnRegUpdate(mUpdateDelegate);

                if (mFixedUpdateDelegate != null)
                    mUpdateManager.UnRegFixedUpdate(mFixedUpdateDelegate);

                if (mLateUpdateDelegate != null)
                    mUpdateManager.UnRegLateUpdate(mLateUpdateDelegate);

                if (mOnGUIDelegate != null)
                    mUpdateManager.UnGUIUpdate(mOnGUIDelegate);

            }
            virtual protected void RegAll()
            {
                if (mUpdateGroup != null)
                    mUpdateManager.RegUpdate(mUpdateGroup);
                if (mUpdateDelegate != null)
                    mUpdateManager.RegUpdate(mUpdateDelegate);

                if (mFixedUpdateDelegate != null)
                    mUpdateManager.RegFixedUpdate(mFixedUpdateDelegate);

                if (mLateUpdateDelegate != null)
                    mUpdateManager.RegLateUpdate(mLateUpdateDelegate);

                if (mOnGUIDelegate != null)
                    mUpdateManager.RegGUIUpdate(mOnGUIDelegate);
            }
            #endregion
        }
    }
    
}
