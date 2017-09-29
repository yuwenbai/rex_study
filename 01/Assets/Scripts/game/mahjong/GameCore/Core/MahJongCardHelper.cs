/**
 * @Author JEFF
 *
 *
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MahJongCardHelper  {
    public void SetCardsFloatOn(List<MahJongCard> cards, bool isFloatOn, bool floatEnable,bool needAnim=true)
    {
        if (NullHelper.IsObjectIsNull(cards))
        {
            return;
        }
        for (int i = 0; i < cards.Count; i++)
        {
            if (isFloatOn)
            {
                if (cards[i].IsFloatOn)
                {
                    cards[i].SetCardFloat(floatEnable);
                }
            }
            else
            {
                cards[i].SetCardFloat(floatEnable);
            }

        }
    }
}
