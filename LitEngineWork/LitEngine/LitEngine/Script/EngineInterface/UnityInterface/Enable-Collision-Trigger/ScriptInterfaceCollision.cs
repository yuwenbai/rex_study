using UnityEngine;
using System;
namespace LitEngine
{
    namespace ScriptInterface
    {
        public class ScriptInterfaceCollision : ScriptInterfaceCETBase
        {
            #region mymethod
            protected Action<Collision> mOnCollisionEnter;
            protected Action<Collision> mOnCollisionExit;
            #endregion
            #region 脚本初始化以及析构
            public ScriptInterfaceCollision()
            {

            }
            override public void ClearScriptObject()
            {
                mOnCollisionEnter = null;
                mOnCollisionExit = null;
                base.ClearScriptObject();
            }
            override protected void InitParamList()
            {
                base.InitParamList();
                mOnCollisionEnter = mCodeTool.GetCSLEDelegate<Action<Collision>, Collision>("OnCollisionEnter", mScriptType, ScriptObject);
                mOnCollisionExit = mCodeTool.GetCSLEDelegate<Action<Collision>, Collision>("OnCollisionExit", mScriptType, ScriptObject);
            }
            #endregion
            #region Unity 
            protected void OnCollisionEnter(Collision collision)
            {
                if (mOnCollisionEnter == null) return;
                if (!IsInTagList(collision.gameObject)) return;
                if (mCollEnterTimer > Time.realtimeSinceStartup) return;
                mCollEnterTimer = Time.realtimeSinceStartup + mCollEnterInterval;
                mOnCollisionEnter(collision);
            }

            protected void OnCollisionExit(Collision collision)
            {
                if (mOnCollisionExit == null) return;
                if (!IsInTagList(collision.gameObject)) return;
                mOnCollisionExit(collision);
            }

            override protected void OnDestroy()
            {
                base.OnDestroy();
            }
            
            #endregion
        }
    }
    
}
