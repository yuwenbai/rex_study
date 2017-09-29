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
    public class UIMahjongShowTing : MonoBehaviour
    {
        public UISprite sprite_Pai = null;
        public UILabel label_Odd = null;
        public UILabel label_Rest = null;

        public int selfCode = -1;
        public int selfRest = -1;
        public int curOdd = -1;

        private bool showFen = false;

        public void IniShowTing(int mjCode, int oddCount, int restCount)
        {
            selfCode = mjCode;
            selfRest = restCount;
            curOdd = oddCount;
            showFen = MjDataManager.Instance.MjData.ProcessData.CheckStateInfo(EnumMjBeforeDispatch.ISDuoHu);

            CardHelper.SetRecordUI(sprite_Pai, mjCode, restCount <= 0);
            label_Odd.text = CardHelper.GetGameOdd(oddCount, false, showFen);
            label_Rest.text = CardHelper.GetCardRest(restCount);
        }

        public void RefreshShowTing(int curDown, int oddS = -1)
        {
            showFen = MjDataManager.Instance.MjData.ProcessData.CheckStateInfo(EnumMjBeforeDispatch.ISDuoHu);

            selfRest -= curDown;
            if (selfRest < 0)
            {
                selfRest = 0;
            }

            label_Rest.text = CardHelper.GetCardRest(selfRest);
            if (selfRest == 0)
            {
                sprite_Pai.alpha = 0.5f;
            }

            if (oddS > 0)
            {
                label_Odd.text = CardHelper.GetGameOdd(oddS, false, showFen);
            }
        }

    }

}
