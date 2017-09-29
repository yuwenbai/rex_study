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
    public class UIMahjongXingPaiTipsPos : MonoBehaviour
    {
        public Transform trans_Center = null;

        public Transform[] trans_Pos = null;


        public void ClearMessage()
        {
            ClearMessageCenter();

            if (trans_Pos != null)
            {
                for (int i = 0; i < trans_Pos.Length; i++)
                {
                    ClearMessageBySeat(i);
                }
            }
        }

        public void ClearMessageCenter()
        {
            if (trans_Center != null)
            {
                trans_Center.DestroyChildren();
            }
        }

        public void ClearMessageBySeat(int uiSeatID)
        {
            if (trans_Pos != null)
            {
                trans_Pos[uiSeatID].DestroyChildren();
            }
        }

    }

}
