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
    public class UIMahjongBtnChooseItem : MonoBehaviour
    {
        public int btnValue = 0;

        public void SetBtnValue(int curValue)
        {
            this.btnValue = curValue;
        }

        public int GetBtnValue()
        {
            return btnValue;
        }

    }

}

