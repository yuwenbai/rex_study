/**
 * @Author JEFF
 *
 *
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using projectQ;

public class MjHuZoneReconnectData : MjZoneReconnectData
{
    public List<int> HuList;
    public MjHuZoneReconnectData( List<int> putList)
    {
        HuList = putList;
    }
}

[System.Serializable]
public class PlayerMjHuZone : BasePlayerMjZone
{
    #region 组件
    public Transform State_HuCard = null;

    #endregion

    #region 数据
    private List<MahJongCard> _HuCards = new List<MahJongCard>();
    #endregion

    #region 清理逻辑
    public override void ClearUp()
    {
        base.ClearUp();
        
        MahjongCardController.Instance.DepoolMahjongCard(_HuCards);
        _HuCards.Clear();
        if (!NullHelper.IsObjectIsNull(State_HuCard))
        {
            State_HuCard.DestroyChildren();
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
    //向胡牌区添加一张牌 
    public void AddACardToHu(MahJongCard card, bool needAnim = true)
    {
        card.selfTransform.SetParent(State_HuCard, false);
        int curCount = _HuCards.Count;
        int countBase = 4;

        int zheng = (curCount / countBase);
        int yu = curCount % countBase;

        float posY = zheng * GameConst.mahJongHeight;
        float posX = yu * GameConst.mahJongWidth;
        card.selfTransform.localPosition = Vector3.forward * posY + Vector3.right * posX;
        card.selfTransform.localRotation = Quaternion.Euler(Vector3.zero);
        if (zheng == 0)
        {
            card.SetCardLight(true);
        }
        _HuCards.Add(card);

        if (!needAnim)
        {
            card.selfObj.SetActive(true);
        }
        else
        {
            FireAnimEvent_Hu();
        }

    }
    private void FireAnimEvent_Hu()
    {
        base.MonoHelper.StartCoroutine(waitToPlayHu());
    }
    private IEnumerator waitToPlayHu()
    {
        yield return new WaitForSeconds(0.9f);

        int curIndex = _HuCards.Count - 1;
        if (curIndex >= 0)
        {
            Vector3 posStart = _HuCards[curIndex].selfTransform.position;
            EventDispatcher.FireEvent(MJEnum.HandModelEvent.HME_Play.ToString(), AnimatorEnum.Anim_chupai, posStart, posStart, base._SeatUIIndex, (int)MjHandAnimType.Hu);
        }
    }
    /// <summary>
    /// 胡牌事件开始
    /// </summary>
    /// <param name="parentNew"></param>
    public void AnimHuCardStart(Transform parentNew)
    {
        int huCount = _HuCards.Count;
        if (huCount > 0)
        {
            MahJongCard card = _HuCards[huCount - 1];

            MahJongCard cloneCard = GameObject.Instantiate(card.selfObj).GetComponent<MahJongCard>();
            cloneCard.SetClickInfo(false, -1);

            cloneCard.selfTransform.SetParent(parentNew, false);
            cloneCard.selfTransform.localPosition = Vector3.zero;
            cloneCard.selfObj.SetActive(true);
            cloneCard.FireShadowEvent(true);
        }
    }

    /// <summary>
    /// 胡牌事件结束
    /// </summary>
    /// <param name="parentNew"></param>
    public void AnimHuCardEnd(Transform parentNew)
    {
        int huCount = _HuCards.Count;
        if (huCount > 0)
        {
            MahJongCard card = _HuCards[huCount - 1];
            if (card != null)
            {
                card.selfObj.SetActive(true);
                parentNew.DestroyChildren();
                card.FireShadowEvent(false);
                EventDispatcher.FireEvent(GEnum.NamedEvent.EMjFxHu, card.selfTransform.position);
            }
        }
    }
    public override void SetCardHeighLight(bool state, int mjCode)
    {
        base.SetCardHeighLight(state, mjCode);
        if (_HuCards != null && _HuCards.Count > 0)
        {
            for (int i = 0; i < _HuCards.Count; i++)
            {
                _HuCards[i].SetCardHeighLight(state, mjCode);
            }
        }
    }
    #endregion
}
