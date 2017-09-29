/**
 * @Author JEFF
 *
 *
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using projectQ;
using DG.Tweening;
public enum UIOpenMaItemState
{
    Down,
    Flipping,
    Up,
}

public class UIOpenMaItem : MonoBehaviour
{
    #region 组件
    public UISprite CardSprite;
    public UISprite CardBackSprite;
    public UISprite HightlightSprite;
    #endregion

    #region 数据
    private EnumMjBuyhorseStateType _EnumMjBuyhorseStateType;
    private int _CardId;
    #endregion

    public void InitCard(int cardId, EnumMjBuyhorseStateType type)
    {
        _CardId = cardId;
        _EnumMjBuyhorseStateType = type;
    }



    public void FlipCard()
    {

        if (CardSprite == null || CardBackSprite == null)
        {
            DebugPro.DebugError("CardSprite is null or CardBackSprite is null");
            return;
        }
        CardHelper.SetRecordUI(CardSprite, _CardId, false);
        Sequence moveSequence = DOTween.Sequence();
        moveSequence.Append(CardSprite.transform.DOScaleX(1, ConstDefine.OpenMaFlippingTime));
        DOTween.To((value) => { CardSprite.alpha = value; }, 0, 1, ConstDefine.OpenMaFlippingTime);
        moveSequence.PlayForward();

        Sequence moveSequence1 = DOTween.Sequence();
        moveSequence1.Append(CardBackSprite.transform.DOScaleX(0, ConstDefine.OpenMaFlippingTime - 0.1f));
        DOTween.To((value) => { CardBackSprite.alpha = value; }, 1f, 0, ConstDefine.OpenMaFlippingTime - 0.1f);
        moveSequence1.PlayForward();
    }

    public void SetCardAlpha()
    {

        if (_EnumMjBuyhorseStateType == EnumMjBuyhorseStateType.BuyHorseNull)
        {
            if (NullHelper.IsObjectIsNull(CardSprite))
            {
                return;
            }
            CardHelper.SetRecordUI(CardSprite, _CardId, true);
        }
        else
        {

            if (!NullHelper.IsObjectIsNull(HightlightSprite))
            {
                HightlightSprite.gameObject.SetActive(true);
            }
        }

    }
}
