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
    public class UIMahjongXingPaiChooseItem : MonoBehaviour
    {
        public GameObject obj_Fx = null;

        public void IniXingPaiTexiao(bool state)
        {
            if (obj_Fx != null)
            {
                obj_Fx.SetActive(state);
            }
        }


    }

}

