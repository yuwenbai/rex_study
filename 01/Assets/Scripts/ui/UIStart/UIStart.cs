

using System;
/**
* @Author JEFF
*
*
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace projectQ
{
    public class UIStart : UIViewBase
    {
        public NGUILogAnimation logAnimation;

        private void EndAnimation()
        {
            EventDispatcher.FireEvent(EventKey.Bundle_Load_Event);
            this.Close();
        }
        public override void Init()
        {
            logAnimation.ExitAction = EndAnimation;//注册动画完成时间
        }

        public override void OnHide()
        {
        }

        public override void OnShow()
        {
            logAnimation.Show();
        }
    }
}
