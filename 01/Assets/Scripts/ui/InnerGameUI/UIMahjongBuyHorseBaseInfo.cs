/**
 * @Author Hailong.Zhang
 *
 *
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace projectQ
{

    public abstract class UIMahjongBuyHorseBaseInfo : MonoBehaviour
    {
        public UITexture texture_Head = null;
        public GameObject obj_Zhuang = null;
        public GameObject obj_Pai = null;
        public UIGrid grid_Pai = null;

        protected List<UIMahjongBuyHorsePai> paiList = new List<UIMahjongBuyHorsePai>();
        protected List<int> mjCodeList = null;
        protected List<EnumMjBuyhorseStateType> horseTypeList = null;
        protected bool isSelf = false;

        protected void DownHeadCall(Texture2D text, string headName)
        {
            if (texture_Head != null)
            {
                texture_Head.mainTexture = text;
            }

        }

        public void ShowMessage()
        {
            if (paiList != null && paiList.Count > 0)
            {
                for (int i = 0; i < paiList.Count; i++)
                {
                    paiList[i].ShowMessage();
                }
            }
        }

    }

}
