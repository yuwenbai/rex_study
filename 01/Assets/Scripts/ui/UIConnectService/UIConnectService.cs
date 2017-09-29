

using System;
/**
* @Author 周腾
*
*
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace projectQ
{
    public class UIConnectService : UIViewBase
    {

        public GameObject closeBtn;

        public override void Init()
        {
            UIEventListener.Get(closeBtn).onClick = OnCloseBtnClick;
        }

        public override void OnHide()
        {
           
        }

        public override void OnShow()
        {
            
        }



        private void OnCloseBtnClick(GameObject go)
        {
            this.Close();
        }
    }
}
