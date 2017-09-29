/**
 * @Author JEFF
 *
 *
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using projectQ;
/// <summary>
/// 补花放毛区
/// </summary>
[System.Serializable]
public class PlayerMjHuaZone : BasePlayerMjZone
{

    #region 组件
    public Transform State_HuaCard = null;
    #endregion

    #region 数据
    private List<MahJongCard> _HuaCards = new List<MahJongCard>();
    #endregion

    #region 清理逻辑
    public override void ClearUp()
    {
        base.ClearUp();
        //  MahjongCardController.Instance.DepoolMahjongCard(_HuaCards);
        _HuaCards.Clear();
        if (!NullHelper.IsObjectIsNull(State_HuaCard))
        {
            State_HuaCard.DestroyChildren();
        }

    }
    #endregion

    #region 重连逻辑
    public override void OnReconnect(object param)
    {
        base.OnReconnect(param);
        ClearUp();
    }
    #endregion

    #region 功能逻辑
    //向补花区创建花牌
    public void CreateCardsToFlower(List<int> flowers, bool newAnim = false)
    {
        if (NullHelper.IsObjectIsNull(State_HuaCard))
        {
            return;
        }
        int flowerCode = flowers[0];
        MahJongCard card = MahjongCardController.Instance.GetMahjongCardByID(flowerCode);
        if (card == null)
        {
            return;
        }
        card.selfTransform.SetParent(State_HuaCard, false);
        card.selfTransform.localPosition = Vector3.zero;
        card.selfTransform.localRotation = Quaternion.Euler(Vector3.zero);
        _HuaCards.Add(card);
        if (newAnim)
        {
            if (flowers != null && flowers.Count > 0)
            {
                bool isHua = flowerCode > 7;
                card.selfObj.SetActive(true);
                EventDispatcher.FireEvent(GEnum.NamedEvent.EMJFxHua, card.selfTransform.position, isHua, _SeatUIIndex);
            }
        }
        else
        {
            //新动画
            FireAnimEvent_Flower();
        }

    }
    //补花 
    public void RemoveHua(bool anim)
    {
        if (anim)
        {
            if (!NullHelper.IsObjectIsNull(MonoHelper))
            {
                base.MonoHelper.StartCoroutine(IERemoveHua(ConstDefine.Mj_AnimFx_Hua * 0.5f));
            }
        }
        else
        {
            if (!NullHelper.IsObjectIsNull(State_HuaCard) && State_HuaCard.childCount > 0)
            {
                State_HuaCard.DestroyChildren();
            }
        }
    }

    private IEnumerator IERemoveHua(float time)
    {
        yield return new WaitForSeconds(time);
        if (!NullHelper.IsObjectIsNull(State_HuaCard) && State_HuaCard.childCount > 0)
        {
            State_HuaCard.DestroyChildren();
        }
    }
    /// <summary>
    /// 补花事件开始
    /// </summary>
    /// <param name="parentNew"></param>
    public void AnimFlowerCardStart(Transform parentNew)
    {
        if (NullHelper.IsObjectIsNull(State_HuaCard))
        {
            return;
        }
        int flowerCount = State_HuaCard.childCount;
        if (flowerCount > 0)
        {
            Transform obj = State_HuaCard.GetChild(0);
            if (obj != null)
            {
                MahJongCard card = obj.GetComponent<MahJongCard>();
                if (card != null)
                {
                    MahJongCard cloneCard = GameObject.Instantiate(card.selfObj).GetComponent<MahJongCard>();
                    cloneCard.SetClickInfo(false, -1);

                    cloneCard.selfTransform.SetParent(parentNew, false);
                    cloneCard.selfTransform.localPosition = Vector3.zero;
                    cloneCard.selfObj.SetActive(true);
                    cloneCard.FireShadowEvent(true);
                }

            }
        }
    }

    /// <summary>
    /// 补花事件结束
    /// </summary>
    /// <param name="parentNew"></param>
    public void AnimFlowerCardEnd(Transform parentNew)
    {
        if (NullHelper.IsObjectIsNull(State_HuaCard))
        {
            return;
        }
        Transform obj = State_HuaCard.GetChild(0);

        MahJongCard card = obj.GetComponent<MahJongCard>();
        if (card != null)
        {
            card.selfObj.SetActive(true);
            parentNew.DestroyChildren();

            card.FireShadowEvent(false);

            int flowerCode = card.cardID;
            bool isHua = flowerCode > 7;
            EventDispatcher.FireEvent(GEnum.NamedEvent.EMJFxHua, card.selfTransform.position, isHua, _SeatUIIndex);
        }

    }
    private void FireAnimEvent_Flower()
    {
        //fireEvent
        Transform huaTrans = State_HuaCard.GetChild(0);
        if (huaTrans != null)
        {
            MahJongCard card = huaTrans.GetComponent<MahJongCard>();
            if (card != null)
            {
                Vector3 posStart = card.selfTransform.position;
                DebugPro.DebugInfo("==uiindex:", this._SeatUIIndex, "posstart:", posStart);
                EventDispatcher.FireEvent(MJEnum.HandModelEvent.HME_Play.ToString(), AnimatorEnum.Anim_chupai, posStart, posStart, this._SeatUIIndex, (int)MjHandAnimType.Flower);
            }
        }

    }
    #endregion
}
