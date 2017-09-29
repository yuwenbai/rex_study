
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
namespace projectQ
{
    public class UIInnerGameAnim : UIViewBase
    {
        #region 组件引用
        public Transform EffectTransform;
        #endregion

        #region 数据
        private List<GamblingAnimBase> _animList = new List<GamblingAnimBase>();

        #endregion


        #region Override
        public override void Init()
        {

        }

        public override void OnHide()
        {
        }
        public override void OnShow()
        {
            for (int i = 0; i < _animList.Count; i++)
            {
                _animList[i].OnShow();
            }
        }
        #endregion

        public override void OnPushData(object[] data)
        {
            _animList.Add(new GamblingAnimZhama(EffectTransform, data, OnceAnimOver));
        }
        
        private void OnceAnimOver(GamblingAnimBase anim)
        {
            _animList.Remove(anim);
            if (_animList.Count <= 0)
            {
                this.Close();
            }
        }
    }
}

