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

    public class UIMahjongChooseChi : MonoBehaviour
    {

        public UISprite[] spriteArray = null;

        private MjActionCodeChi chiInfo = null;
        private System.Action<MjActionCodeChi> callBack = null;

        public void IniChooseChi(MjActionCodeChi chiInfo, System.Action<MjActionCodeChi> ClickCall)
        {
            this.chiInfo = chiInfo;
            List<int> chiList = chiInfo.chiList;
            chiList.Sort();

            int chiID = chiInfo.chiCode;

            for (int i = 0; i < chiList.Count; i++)
            {
                CardHelper.SetRecordUI(spriteArray[i], chiList[i], chiList[i] == chiID);
            }

            callBack = ClickCall;
            UIEventListener.Get(this.gameObject).onClick = ClickChoose;

        }

        private void ClickChoose(GameObject obj)
        {
            if (callBack != null)
            {
                callBack(chiInfo);
            }
        }

    }



}
