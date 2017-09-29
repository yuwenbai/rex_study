/**
 * @Author lyb
 * 没有登陆的时候点击按钮呼出登录提示界面
 *
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace projectQ
{
    public class UIUnLoginClick : MonoBehaviour
    {
        void Start() { }

        void OnClick()
        {
            WindowUIManager.Instance.CreateTip("您还没有登录，请登录后再试一次");

            /*
            WindowUIManager.Instance.CreateOrAddWindow(WindowUIType.SystemPopupWindow, 
                "提示", "用户未登录，您还没有登录，请登录后再试一次", new string[] { "确定" } , (index) => { });
            */
        }
    }
}

