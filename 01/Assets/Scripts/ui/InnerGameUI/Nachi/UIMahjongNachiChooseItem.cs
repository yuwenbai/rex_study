/**
 * @Author JEFF
 *
 *
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using projectQ;
public class UIMahjongNachiChooseItem : MonoBehaviour
{

    #region 组件
    public UISprite CardSprite;
    #endregion

    #region 数据
    private System.Action<GameObject> _ClickCallBack;
    private int _CardId;
    public int CardID
    {
        get { return _CardId; }
    }
    #endregion

    #region 方法

    void Awake()
    {
        UIEventListener.Get(this.gameObject).onClick = ClickCard;
    }
    public void FillItem(int cardID, System.Action<GameObject> action)
    {
        _CardId = cardID;
        _ClickCallBack = action;
        RefreshUI();
    }
    private void ClickCard(GameObject obj)
    {
        RefreshUI();
        if (_ClickCallBack != null)
        {
            _ClickCallBack(obj);
        }
    }
    private void RefreshUI()
    {
        if (CardSprite != null)
        {
            CardHelper.SetRecordUI(CardSprite, _CardId, false);
        }

    }
    #endregion
}
