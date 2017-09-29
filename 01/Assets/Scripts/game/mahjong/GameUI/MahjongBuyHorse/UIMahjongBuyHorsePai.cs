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
    public class UIMahjongBuyHorsePai : MonoBehaviour
    {
        public GameObject obj_WinOrLose = null;
        private UISprite spriteIcon = null;

        private EnumMjBuyhorseStateType horseState = EnumMjBuyhorseStateType.BuyHorseNull;
        private bool isSelf = false;
        private int mjCode = -1;

        public void IniData(int mjCode, EnumMjBuyhorseStateType horseState, bool isSelf,bool isOneBuyHorse)
        {
            this.mjCode = mjCode;
            this.horseState = horseState;
            this.isSelf = isSelf;
            spriteIcon = GetComponent<UISprite>();
            if (spriteIcon)
            {
                if(isOneBuyHorse)
                {
                    CardHelper.SetRecordUI(spriteIcon, mjCode);
                }
                else
                {
                    float alpha=SiJiaMaiMaiAlphaStateType(horseState);
                    CardHelper.SetRecordUI(spriteIcon, mjCode, true, alpha);
                }
            }
        }

        private float SiJiaMaiMaiAlphaStateType(EnumMjBuyhorseStateType horseState)
        {
            float alpha = -1;
            if (horseState == EnumMjBuyhorseStateType.BuyHorseNull)
            {
                alpha = 0.75f;
            }else if(horseState==EnumMjBuyhorseStateType.BuyHorseLose)
            {
                alpha = 0.5f;
            }else
            {
                alpha = 1f;
            }
            return alpha;
        }
        public void ShowMessage()
        {
            obj_WinOrLose.SetActive(false);

            switch (horseState)
            {
                case EnumMjBuyhorseStateType.BuyHorseNull:
                    {
                        CardHelper.SetRecordUI(spriteIcon, mjCode, true);
                    }
                    break;
                case EnumMjBuyhorseStateType.BuyHorseWin:
                    {
                        SetIconBack(true);
                    }
                    break;
                case EnumMjBuyhorseStateType.BuyHorseLose:
                    {
                        SetIconBack(false);
                    }
                    break;
            }
        }

        private void SetIconBack(bool isHorse)
        {
            obj_WinOrLose.SetActive(true);
            UISprite sprite = obj_WinOrLose.GetComponent<UISprite>();
            sprite.spriteName = isHorse ? "desk_bj_kaimahuangguang" : "desk_bj_kaimalanguang";
        }


    }
}
