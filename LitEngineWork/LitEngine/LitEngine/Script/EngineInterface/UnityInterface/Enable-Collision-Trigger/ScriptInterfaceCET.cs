﻿using UnityEngine;
using System;
namespace LitEngine
{
    namespace ScriptInterface
    {
        public class ScriptInterfaceCET : ScriptInterfaceCETBase
        {
            #region mymethod
            protected Action<Collision> mOnCollisionEnter;
            protected Action<Collision> mOnCollisionExit;
            protected Action<Collider> mOnTriggerEnter;
            protected Action<Collider> mOnTriggerExit;
            #endregion

            #region 构造
            public ScriptInterfaceCET()
            {

            }
            override public void ClearScriptObject()
            {
                mOnCollisionEnter = null;
                mOnCollisionExit = null;
                mOnTriggerEnter = null;
                mOnTriggerExit = null;
                base.ClearScriptObject();
            }
            override protected void InitParamList()
            {
                base.InitParamList();
                mOnCollisionEnter = mCodeTool.GetCSLEDelegate<Action<Collision>, Collision>("OnCollisionEnter", mScriptType, ScriptObject);
                mOnCollisionExit = mCodeTool.GetCSLEDelegate<Action<Collision>, Collision>("OnCollisionExit", mScriptType, ScriptObject);
                mOnTriggerEnter = mCodeTool.GetCSLEDelegate<Action<Collider>, Collider>("OnTriggerEnter", mScriptType, ScriptObject);
                mOnTriggerExit = mCodeTool.GetCSLEDelegate<Action<Collider>, Collider>("OnTriggerExit", mScriptType, ScriptObject);

            }
            #endregion
            #region Unity 

            protected void OnDisable()
            {
                CallScriptFunctionByName("OnDisable");
                UnRegAll();
            }

            protected void OnEnable()
            {
                CallScriptFunctionByName("OnEnable");
                RegAll();
            }

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

            protected void OnTriggerEnter(Collider other)
            {
                if (mOnTriggerEnter == null) return;
                if (mTriggerTarget != null && mTriggerTarget != other.transform) return;
                if (!other.name.Equals(TriggerTargetName)) return;

                if (mTriggerEnterTimer > Time.realtimeSinceStartup) return;
                mTriggerEnterTimer = Time.realtimeSinceStartup + mTriggerEnterInterval;
                mOnTriggerEnter(other);
            }
            protected void OnTriggerExit(Collider other)
            {
                if (mOnTriggerExit == null) return;
                if (mTriggerTarget != null && mTriggerTarget != other.transform) return;
                if (!other.name.Equals(TriggerTargetName)) return;
                mOnTriggerExit(other);
            }

            override protected void OnDestroy()
            {
                base.OnDestroy();
            }
            
            #endregion

        }
    }
   
}

