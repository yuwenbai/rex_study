/**
 * @Author JEFF
 *
 *
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using projectQ;


public class MjWallZoneReconnectData : MjZoneReconnectData
{
    public int CodeCount;
    public MjWallZoneReconnectData(int codeCount)
    {
        CodeCount = codeCount;
    }
}
[System.Serializable]
public class PlayerMjWallZone : BasePlayerMjZone
{
    #region 组件
    public Transform State_CodeCard = null;
    public Transform State_CodeCardSub = null;
    #endregion

    #region 数据
    private MahJongCard[] _ArrayCodeCard = null;
    private float _CodeMoveRate = 0f;
    #endregion



    #region 清理逻辑
    public override void ClearUp()
    {
        base.ClearUp();
        if (!NullHelper.IsObjectIsNull(State_CodeCardSub))
        {
            State_CodeCardSub.SetParent(State_CodeCard, false);
            State_CodeCardSub.DestroyChildren();
        }
        _ArrayCodeCard = null;
        _CodeMoveRate = 0f;
    }
    #endregion

    #region 重连逻辑
    public override void OnReconnect(object param)
    {
        base.OnReconnect(param);
        if (!NullHelper.IsObjectIsNull(State_CodeCardSub) && !NullHelper.IsObjectIsNull(State_CodeCard))
        {
            this.ClearUp();
            State_CodeCardSub.gameObject.SetActive(true);
        }

        MjWallZoneReconnectData data = param as MjWallZoneReconnectData;
        if (NullHelper.IsObjectIsNull(data))
        {
            return;
        }
        _ArrayCodeCard = new MahJongCard[data.CodeCount];
        _CodeMoveRate = (18 - data.CodeCount / 2) / 2f;

        int startIndex = 1;
        if (data.CodeCount > 0)
        {
            for (int i = startIndex; i <= data.CodeCount; i++)
            {
                MahJongCard card = MahjongCardController.Instance.GetMahjongCardByID(-1);
                AddACardToCodeList(card, i, true);
            }
        }

        RefreshShadowCode();
    }
    #endregion

    #region 功能
    //public override void SetCardHeighLight(bool state, int mjCode)
    //{

    //}
    /// <summary>
    /// 创建码牌区
    /// </summary>
    /// <param name="codeNum"></param>
    /// <param name="showAnim"></param>
    /// <param name="startIndex"></param>
    public void CreateCodeCard(int codeNum, Transform parent)
    {
        if (_ArrayCodeCard == null)
        {
            _ArrayCodeCard = new MahJongCard[codeNum];
            _CodeMoveRate = (18 - codeNum / 2) / 2f;
        }
        if (!NullHelper.IsObjectIsNull(State_CodeCardSub))
        {
            State_CodeCardSub.gameObject.SetActive(false);
        }
        for (int i = 1; i <= codeNum; i++)
        {
            MahJongCard card = MahjongCardController.Instance.GetMahjongCardByID(-1);
            AddACardToCodeList(card, i, true);
        }

        AnimCodeIni(parent);
    }
    public void AnimCodeIni(Transform parent)
    {
        if (!NullHelper.IsObjectIsNull(State_CodeCardSub))
        {
            State_CodeCardSub.gameObject.SetActive(true);
            State_CodeCardSub.SetParent(parent, false);
        }
        RefreshCodeLayer(true);
    }
    public void AnimCodeEnd(Transform parent)
    {
        if (!NullHelper.IsObjectIsNull(State_CodeCardSub) && !NullHelper.IsObjectIsNull(State_CodeCard))
        {
            State_CodeCardSub.SetParent(State_CodeCard, false);
        }
        RefreshCodeLayer(false);
    }
    private void RefreshShadowCode()
    {
        if (_ArrayCodeCard != null && _ArrayCodeCard.Length > 0)
        {
            for (int i = 0; i < _ArrayCodeCard.Length; i++)
            {
                _ArrayCodeCard[i].FireShadowEvent(false);
                _ArrayCodeCard[i].SetCardLayer(false);
            }
        }
    }
    private void RefreshCodeLayer(bool state)
    {
        if (_ArrayCodeCard != null && _ArrayCodeCard.Length > 0)
        {
            for (int i = 0; i < _ArrayCodeCard.Length; i++)
            {
                if (_ArrayCodeCard[i] != null)
                {
                    _ArrayCodeCard[i].SetCardLayer(state);
                }
            }
        }
    }
    private bool CheckCodeIndexTrue(int index)
    {
        if (index < 0 || _ArrayCodeCard == null || index >= _ArrayCodeCard.Length)
        {
            return false;
        }
        return true;
    }
    public bool CheckCurBuCardCountain(int index)
    {
        if (!CheckCodeIndexTrue(index))
        {
            return false;
        }

        return _ArrayCodeCard[index] != null;
    }
    public void SetCurBuCardCanGet(int index, bool canGet)
    {
        if (!CheckCodeIndexTrue(index))
        {
            return;
        }

        if (_ArrayCodeCard[index] != null)
        {
            _ArrayCodeCard[index].SetCardCanGet(true);
        }
    }
    public bool CheckCurBuCardCanGet(int index)
    {
        if (!CheckCodeIndexTrue(index))
        {
            return false;
        }

        return _ArrayCodeCard[index] != null && _ArrayCodeCard[index].CanGetInCode;
    }
    //翻开一张牌
    public void OpenACard(int index, int mjCode = -1, bool canGet = false)
    {
        if (!CheckCodeIndexTrue(index))
        {
            return;
        }


        MahJongCard openCard = _ArrayCodeCard[index];
        if (openCard == null)
        {
            QLoger.ERROR("翻开一张牌的时候 ，index位置的card 是null + index : " + index);
            return;
        }

        if (mjCode > 0)
        {
            MahjongCardController.Instance.ChangeMhajongCardByID(mjCode, openCard);
        }

        openCard.SetCardOpenInCode(canGet, MahjongTimeConfig.Instance.MjSystemTime.PartTime.Mj_Anim3d_OpenCard);
    }

    public MahJongCard GetBuCardByIndex(int index, bool removeList)
    {
        if (!CheckCodeIndexTrue(index))
        {
            return null;
        }

        MahJongCard card = _ArrayCodeCard[index];
        if (card == null)
        {
            QLoger.ERROR("获取一张牌墙的牌出错 ，index位置的card 是null + index : " + index);
            return card;
        }
        if (removeList)
        {
            _ArrayCodeCard[index] = null;
        }
        return card;
    }

    //直接消掉
    public void RemoveBuCardByIndex(int index)
    {
        if (!CheckCodeIndexTrue(index))
        {
            return;
        }

        MahJongCard card = _ArrayCodeCard[index];
        if (card == null)
        {
            QLoger.ERROR("消除一张牌墙的牌出错 ，index位置的card 是null + index : " + index);
            return;
        }
        _ArrayCodeCard[index] = null;
        MahjongCardController.Instance.DepoolMahjongCard(card);
    }
    //创建牌到码牌区 
    public void AddACardToCodeList(MahJongCard card, int index, bool isShow, bool isOpen = false)
    {
        if (NullHelper.IsObjectIsNull(State_CodeCardSub))
        {
            return;
        }
        if (card != null)
        {
            card.selfTransform.SetParent(State_CodeCardSub, false);
            card.selfTransform.localRotation = Quaternion.Euler(isOpen ? (Vector3.right * 180f) : Vector3.zero);
            card.selfTransform.localPosition = Vector3.forward * (((index - 1) % 2) * GameConst.mahJongHeight) +
                Vector3.right * ((((index - 1) / 2) + _CodeMoveRate) * GameConst.mahJongWidth);

            _ArrayCodeCard[index - 1] = card;
            if (isShow)
            {
                card.selfObj.SetActive(isShow);
            }
        }
    }

    public void RefreshOpenACard(int index, int mjCode = -1, bool canGet = false)
    {
        MahJongCard openCard = _ArrayCodeCard[index];
        if (openCard == null)
        {
            QLoger.ERROR("翻开一张牌的时候 ，index位置的card 是null + index : " + index);
            return;
        }

        if (mjCode > 0)
        {
            MahjongCardController.Instance.ChangeMhajongCardByID(mjCode, openCard);
        }

        openCard.SetCardOpenInCode(canGet, 0f);
    }

    public void RefreshCodeShow()
    {
        if (_ArrayCodeCard != null)
        {
            for (int i = 0; i < _ArrayCodeCard.Length; i++)
            {
                if (_ArrayCodeCard[i] != null)
                {
                    _ArrayCodeCard[i].selfObj.SetActive(true);
                }
            }
        }
    }
    #endregion



    #region 现在废弃功能
    ///// <summary>
    ///// 从码牌区正向摸n张牌
    ///// </summary>
    //public int RemoveCardsFromCode(int index, int num)
    //{
    //    MahJongCard card = null;
    //    int removeCount = 0;
    //    for (int i = index; i < _ArrayCodeCard.Length; i++)
    //    {
    //        card = _ArrayCodeCard[i];
    //        if (card == null)
    //        {
    //            continue;
    //        }
    //        MahjongCardController.Instance.DepoolMahjongCard(card);
    //        _ArrayCodeCard[i] = null;
    //        removeCount++;
    //        if (removeCount == num)
    //        {
    //            break;
    //        }
    //    }
    //    return removeCount;
    //}
    #endregion
}
