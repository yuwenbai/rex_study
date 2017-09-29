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
    public class SupplementCard : MonoBehaviour
    {
        public UISprite CardSprite;

        private GameObject _obj = null;
        public GameObject selfObj
        {
            get
            {
                if (_obj == null)
                {
                    _obj = this.gameObject;
                }
                return _obj;
            }
        }

        
        public void InitCard(int cardId)
        {
            if (CardSprite != null)
            {
                CardHelper.SetRecordUI(CardSprite, cardId);
            }
            else
            {
                DebugPro.DebugError("cardSp GameObject is null");
            }
          
        }

    }


}
