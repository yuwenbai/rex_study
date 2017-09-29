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
    public class UISiJiaMaiMaFirstPai : MonoBehaviour
    {
        private UISprite spriteIcon = null;
        private int mjCode = -1;

        public void IniData(int mjCode)
        {
            this.mjCode = mjCode;
            spriteIcon = GetComponent<UISprite>();
            if (spriteIcon)
            {
                spriteIcon.spriteName = "mj_icon_Back";
            }
        }
    }
}
