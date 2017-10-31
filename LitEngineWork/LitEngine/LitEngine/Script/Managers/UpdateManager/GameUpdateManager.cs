using UnityEngine;
using System.Collections.Generic;
namespace LitEngine
{
    public class GameUpdateManager : MonoManagerBase
    {
        void OnDestroy()
        {
            Clear();
        }
        public bool mIsFixedUpdate = true;
        public bool mIsUpdate = true;
        public bool mIsLateUpdate = true;
        public bool mIsGUIUpdate = true;


        private List<UpdateBase> mUpdateList = new List<UpdateBase>();
        private int mUpdateCount = 0;
        private List<UpdateBase> mFixedUpdateList = new List<UpdateBase>();
        private int mFixedUpdateCount = 0;
        private List<UpdateBase> mLateUpdateList = new List<UpdateBase>();
        private int mLateUpdateCount = 0;
        private List<UpdateBase> mOnGUIList = new List<UpdateBase>();
        private int mOnGUICount = 0;
        #region 注册
        public void RegUpdate(UpdateBase _act)
        {
            if (_act == null)
                return;
            if (!mUpdateList.Contains(_act))
            {
                mUpdateList.Add(_act);
                mUpdateCount++;
            }
        }
        public void RegLateUpdate(UpdateBase _act)
        {
            if (_act == null)
                return;
            if (!mLateUpdateList.Contains(_act))
            {
                mLateUpdateList.Add(_act);
                mLateUpdateCount++;
            }
        }
        public void RegFixedUpdate(UpdateBase _act)
        {
            if (_act == null)
                return;
            if (!mFixedUpdateList.Contains(_act))
            {
                mFixedUpdateList.Add(_act);
                mFixedUpdateCount++;
            }
        }
        public void RegGUIUpdate(UpdateBase _act)
        {
            if (_act == null)
                return;
            if (!mOnGUIList.Contains(_act))
            {
                mOnGUIList.Add(_act);
                mOnGUICount++;
            }
        }
        #endregion

        #region 销毁
        protected void Clear()
        {
            mUpdateList.Clear();
            mFixedUpdateList.Clear();
            mLateUpdateList.Clear();
            mOnGUIList.Clear();
            mUpdateCount = 0;
            mFixedUpdateCount = 0;
            mLateUpdateCount = 0;
            mOnGUICount = 0;
        }
        public void UnRegUpdate(UpdateBase _act)
        {
            if (_act == null) return;
            if (mUpdateList.Contains(_act))
            {
                mUpdateList.Remove(_act);
                mUpdateCount--;
            }

        }
        public void UnRegLateUpdate(UpdateBase _act)
        {
            if (_act == null) return;
            if (mLateUpdateList.Contains(_act))
            {
                mLateUpdateList.Remove(_act);
                mLateUpdateCount--;
            }
        }
        public void UnRegFixedUpdate(UpdateBase _act)
        {
            if (_act == null) return;
            if (mFixedUpdateList.Contains(_act))
            {
                mFixedUpdateList.Remove(_act);
                mFixedUpdateCount--;
            }
        }
        public void UnGUIUpdate(UpdateBase _act)
        {
            if (_act == null) return;
            if (mOnGUIList.Contains(_act))
            {
                mOnGUIList.Remove(_act);
                mOnGUICount--;
            }
        }
        #endregion

        #region Updates
        void Update()
        {
            if (!mIsUpdate) return;
            for (int i = mUpdateCount - 1; i >= 0; i--)
                mUpdateList[i].RunDelgete();
        }

        void LateUpdate()
        {
            if (!mIsLateUpdate) return;
            for (int i = mLateUpdateCount - 1; i >= 0; i--)
                mLateUpdateList[i].RunDelgete();
        }

        void FixedUpdate()
        {
            if (!mIsFixedUpdate) return;
            for (int i = mFixedUpdateCount - 1; i >= 0 ; i--)
                mFixedUpdateList[i].RunDelgete();
        }

        #endregion

        #region OnRender
        void OnGUI()
        {
            if (!mIsGUIUpdate) return;
            for (int i = mOnGUICount - 1; i >= 0 ; i--)
                mOnGUIList[i].RunDelgete();
        }
        #endregion

        override public void DestroyManager()
        {
            Clear();   
            base.DestroyManager();
        }
    }
}

