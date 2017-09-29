/**
 * @Author YQC
 *
 *
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace projectQ
{
    /// <summary>
    /// 帧率控制器
    /// </summary>
    public class FrameRateMgr : SingletonTamplate<FrameRateMgr>
    {
        #region 帧率控制
        float lastFrameTime = 0f;
        const float MaxTimeOut = 30f;
        const int MaxFrameRate = 29;
        const int MinFrameRate = 19;
        const int LowerFrameRate = 19;
        private bool IsAutoUpdateFrameRate = true;
        /// <summary>
        /// 初始化帧数控制器
        /// </summary>
        public void InitChangeFrame()
        {
            UICamera.onPress = onPressFrame;
            GameDelegateCache.Instance.InvokeMethodEvent -= ChangeFrameRate;
            GameDelegateCache.Instance.InvokeMethodEvent += ChangeFrameRate;
        }

        /// <summary>
        /// 降低帧率
        /// </summary>
        public void LowerFrame()
        {
            IsAutoUpdateFrameRate = false;
            UpdateFrameRate(LowerFrameRate);
        }
        public void RevertFrame()
        {
            IsAutoUpdateFrameRate = true;
            UpdateFrameRate(MaxFrameRate);
        }



        private void ChangeFrameRate()
        {
            //30秒无点击事件则降低帧率
            if (IsAutoUpdateFrameRate && Time.realtimeSinceStartup - lastFrameTime > MaxTimeOut && Application.targetFrameRate > MinFrameRate)
            {
                QLoger.LOG("帧率修改"+ MinFrameRate);
                lastFrameTime = Time.realtimeSinceStartup + 999;
                UpdateFrameRate(MinFrameRate);
            }
        }

        private void onPressFrame(GameObject go, bool isPress)
        {
            if (Application.targetFrameRate < MaxFrameRate)
            {
                UpdateFrameRate(MaxFrameRate);
                lastFrameTime = Time.realtimeSinceStartup;
            }
        }

        /// <summary>
        /// 修改帧率
        /// </summary>
        /// <param name="frameRate"></param>
        public void UpdateFrameRate(int frameRate)
        {
            DebugPro.Log(DebugPro.EnumLog.System, "帧率修改", frameRate);
            Application.targetFrameRate = frameRate;
        }
        #endregion
    }
}