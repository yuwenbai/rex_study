/**
 * @Author Xin.Wang
 *
 *
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace projectQ
{
    public class UIMahjongChooseGang : MonoBehaviour
    {
        public UISprite[] spriteArray = null;

        private int gangID = -1;
        private System.Action<int> callBack = null;

        public void IniChooseGang(int gangID, System.Action<int> ClickCall)
        {
            this.gangID = gangID;

            for (int i = 0; i < 4; i++)
            {
                CardHelper.SetRecordUI(spriteArray[i], gangID);
            }

            callBack = ClickCall;
            UIEventListener.Get(this.gameObject).onClick = ClickChoose;
        }

        private void ClickChoose(GameObject obj)
        {
            if (callBack != null)
            {
                callBack(gangID);
            }
        }



    }

}

