/**
 * @Author lyb
 *  长按按钮事件检测
 *
 */

using System;
using UnityEngine;

namespace projectQ
{
    public class UIButtonOnPressMessage : MonoBehaviour
    {
        private bool isClick;

        private float clickTime = 1.0f;

        void Start() { }

        void Update()
        {
            if (isClick)
            {
                clickTime -= Time.deltaTime;

                if (clickTime <= 0)
                {
                    isClick = false;

                    QLoger.LOG(" ############### 发送消息 ");
                    EventDispatcher.FireSysEvent(GEnum.NamedEvent.SysUI_FriendAward_OnPress);
                }
            }
        }

        void OnPress(bool isPressed)
        {
            if (isPressed)
            {
                //QLoger.LOG(" ############################# 按住 ");

                if (!isClick)
                {
                    clickTime = 1.0f;
                    isClick = true;
                }
            }
            else
            {
                //QLoger.LOG(" ############################# 松手 ");

                if (isClick)
                {
                    clickTime = 1.0f;
                    isClick = false;
                }
            }
        }
    }
}
