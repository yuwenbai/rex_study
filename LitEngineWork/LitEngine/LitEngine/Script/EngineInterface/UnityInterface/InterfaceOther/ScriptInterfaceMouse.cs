using System;
namespace LitEngine
{
    namespace ScriptInterface
    {
        public class ScriptInterfaceMouse : BehaviourInterfaceBase
        {
            #region mymethod
            protected Action mOnMouseDown;
            protected Action mOnMouseDrag;
            protected Action mOnMouseEnter;
            protected Action mOnMouseExit;
            protected Action mOnMouseOver;
            #endregion
            #region 脚本初始化以及析构
            public ScriptInterfaceMouse()
            {

            }

            override public void ClearScriptObject()
            {
                mOnMouseDown = null;
                mOnMouseDrag = null;
                mOnMouseEnter = null;
                mOnMouseExit = null;
                mOnMouseOver = null;
                base.ClearScriptObject();
            }
            override protected void InitParamList()
            {
                base.InitParamList();
                mOnMouseDown = mCodeTool.GetCSLEDelegate<Action>("OnMouseDown", mScriptType, ScriptObject);
                mOnMouseDrag = mCodeTool.GetCSLEDelegate<Action>("OnMouseDrag", mScriptType, ScriptObject);
                mOnMouseEnter = mCodeTool.GetCSLEDelegate<Action>("OnMouseEnter", mScriptType, ScriptObject);
                mOnMouseExit = mCodeTool.GetCSLEDelegate<Action>("OnMouseExit", mScriptType, ScriptObject);
                mOnMouseOver = mCodeTool.GetCSLEDelegate<Action>("OnMouseOver", mScriptType, ScriptObject);
            }
            #endregion
            #region Unity 
            protected void OnMouseDown()
            {
                if (mOnMouseDown != null)
                    mOnMouseDown();
            }
            protected void OnMouseDrag()
            {
                if (mOnMouseDrag != null)
                    mOnMouseDrag();
            }
            protected void OnMouseEnter()
            {
                if (mOnMouseEnter != null)
                    mOnMouseEnter();
            }
            protected void OnMouseExit()
            {
                if (mOnMouseExit != null)
                    mOnMouseExit();
            }
            protected void OnMouseOver()
            {
                if (mOnMouseOver != null)
                    mOnMouseOver();
            }

            override protected void OnDestroy()
            {
                base.OnDestroy();
            }
            #endregion
        }
    }
    
}
