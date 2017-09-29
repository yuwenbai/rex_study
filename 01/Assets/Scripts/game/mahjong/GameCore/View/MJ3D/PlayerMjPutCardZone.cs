/**
 * @Author JEFF
 *
 *
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using projectQ;

public class MjPutCardZoneReconnectData : MjZoneReconnectData
{
    public List<int> PutList;
    public int TingPutIndex;
    public MjPutCardZoneReconnectData(List<int> putList, int tingPutIndex)
    {
        PutList = putList;
        TingPutIndex = tingPutIndex;
    }
}

[System.Serializable]
public class PlayerMjPutCardZone : BasePlayerMjZone
{
    #region 组件
    public Transform State_PutCard = null;
    #endregion

    #region 数据
    private List<MahJongCard> _PutCards = new List<MahJongCard>();
    private int _NowRowBase = 6;
    //1： 直接出牌 2 推牌
    private int curPutType = 1;
    public int NowRowBase
    {
        get { return _NowRowBase; }
        set { _NowRowBase = value; }
    }
    #endregion


    #region 清理逻辑
    public override void ClearUp()
    {
        base.ClearUp();
        MahjongCardController.Instance.DepoolMahjongCard(_PutCards);
        _PutCards.Clear();
        if (!NullHelper.IsObjectIsNull(State_PutCard))
        {
            State_PutCard.DestroyChildren();
        }
    }
    #endregion



    #region 重连逻辑
    public override void OnReconnect(object param)
    {
        base.OnReconnect(param);
        MjPutCardZoneReconnectData data = param as MjPutCardZoneReconnectData;
        if (NullHelper.IsObjectIsNull(data))
        {
            return;
        }
        RefreshPut(data.PutList, data.TingPutIndex);
    }
    #endregion

    #region 功能逻辑
    private void RefreshPut(List<int> putList, int tingPutIndex)
    {
        ClearUp();

        for (int i = 0; i < putList.Count; i++)
        {
            bool isClose = false;
            if (i == tingPutIndex)
            {
                isClose = true;
            }
            CreateACardToPut(putList[i], isClose, -1);
            CreateACardToPutState(putList[i], isClose, false);
        }
    }
    /// <summary>
    /// 创建一张牌到出牌区
    /// </summary>
    public void CreateACardToPut(int cardID, bool isClose, int putSeatID)
    {
        MahJongCard card = MahjongCardController.Instance.GetMahjongCardByID(isClose ? -1 : cardID);
        _PutCards.Add(card);
        int nowCount = 0;
        if (putSeatID > 0)
        {
            nowCount = MjDataManager.Instance.GetPlayerPutCount(putSeatID) - 1;
        }
        else
        {
            nowCount = _PutCards.Count - 1;
        }

        bool isMan = nowCount >= 3 * _NowRowBase;

        float localPoxY = -1f;
        int valueY = -1;
        //set y
        if (isMan)
        {
            valueY = (nowCount - 3 * _NowRowBase) / (11 - _NowRowBase);
            if (valueY > 2)
            {
                valueY -= 3;
            }
        }
        else
        {
            valueY = nowCount / _NowRowBase;
            if (valueY > 2)
            {
                valueY -= 3;
            }
        }

        localPoxY = valueY * (GameConst.mahJongLenght + 0.05f);

        //set x
        float localPosX = -1f;
        if (isMan)
        {
            localPosX = ((nowCount - 3 * _NowRowBase) % (11 - _NowRowBase) + _NowRowBase) * (GameConst.mahJongWidth + 0.01f);
        }
        else
        {
            localPosX = (nowCount % _NowRowBase) * (GameConst.mahJongWidth + 0.01f);
        }

        Vector3 aimPos = Vector3.right * localPosX + Vector3.up * localPoxY;

        card.selfTransform.SetParent(State_PutCard, false);
        card.selfTransform.localRotation = Quaternion.Euler(isClose ? Vector3.left * 180f : Vector3.zero);
        card.selfTransform.localPosition = aimPos;
    }
    /// <summary>
    /// 动画创建出牌区一张牌
    /// </summary>
    /// <param name="cardID"></param>
    /// <param name="anim"></param>
    public void CreateACardToPutState(int cardID, bool isClose, bool anim = true)
    {
        if (_PutCards == null || _PutCards.Count == 0)
        {
            return;
        }

        MahJongCard card = _PutCards[_PutCards.Count - 1];

        if (anim)
        {
            Vector3 aimPos = card.selfTransform.localPosition;

            Vector3 startPos = Vector3.right * (aimPos.x + 1 * GameConst.mahJongWidth) + Vector3.up * (aimPos.y + 1 * GameConst.mahJongLenght);
            Vector3 wordStart = Vector3.zero;
            Vector3 wordEnd = Vector3.zero;
            wordStart = State_PutCard.TransformPoint(startPos);
            wordEnd = State_PutCard.TransformPoint(aimPos);
            curPutType = 1;
            //int rageValue = Random.Range(0, 10);
            //if (rageValue > 7)
            //{
            //    //tuipai
            //    curPutType = 2;
            //}

            if (curPutType == 2)
            {
                //tuipai
                EventDispatcher.FireEvent(MJEnum.HandModelEvent.HME_Play.ToString(), AnimatorEnum.Anim_chupai, wordStart, wordEnd, _SeatUIIndex, (int)MjHandAnimType.Put);
            }
            else
            {
                EventDispatcher.FireEvent(MJEnum.HandModelEvent.HME_Play.ToString(), AnimatorEnum.Anim_chupai, wordEnd, wordEnd, _SeatUIIndex, (int)MjHandAnimType.Put);
            }

        }
        else
        {
            card.selfObj.SetActive(true);
        }
    }
    /// <summary>
    /// 删除出牌区的最后一张牌
    /// </summary>
    public void RemoveLastPutCard(int mjCode)
    {
        if (_PutCards.Count > 0)
        {
            MahJongCard card = _PutCards[_PutCards.Count - 1];
            if (card.cardID == mjCode)
            {
                MahjongCardController.Instance.DepoolMahjongCard(card);
                _PutCards.Remove(card);
            }
            else
            {
                QLoger.ERROR("不一样的牌");
            }
        }
    }


    public Vector3 GetLastPutCardPos(int mjCode)
    {
        Vector3 pos = Vector3.zero;

        if (_PutCards.Count > 0)
        {
            MahJongCard card = _PutCards[_PutCards.Count - 1];
            if (card.cardID == mjCode || mjCode == -1)
            {
                return card.selfTransform.position;
            }
        }

        return pos;
    }
    /// <summary>
    /// 出牌(补花，胡牌)事件开始
    /// </summary>
    public void AnimPutCardStart(Transform parentNew)
    {
        int putCount = _PutCards.Count;
        if (putCount > 0)
        {
            MahJongCard card = _PutCards[putCount - 1];

            MahJongCard cloneCard = GameObject.Instantiate(card.selfObj).GetComponent<MahJongCard>();
            cloneCard.SetClickInfo(false, -1);
            card = null;

            cloneCard.selfTransform.SetParent(parentNew, false);
            cloneCard.selfTransform.localPosition = Vector3.zero;
            cloneCard.selfObj.SetActive(true);
            cloneCard.FireShadowEvent(true);
            if (curPutType == 2)
            {
                EventDispatcher.FireEvent(MJEnum.MusicEnum.ME_PlaySoundFX.ToString(), GEnum.SoundEnum.desk_dapai);
            }
        }

    }
    /// <summary>
    /// 出牌(补花，胡牌)事件结束
    /// </summary>
    public void AnimPutCardEnd(Transform parentNew)
    {
        int putCount = _PutCards.Count;
        if (putCount > 0)
        {
            MahJongCard card = _PutCards[putCount - 1];
            if (card != null)
            {
                card.selfObj.SetActive(true);
                parentNew.DestroyChildren();
                card.FireShadowEvent(false);

                if (curPutType != 2)
                {
                    EventDispatcher.FireEvent(MJEnum.MusicEnum.ME_PlaySoundFX.ToString(), GEnum.SoundEnum.desk_dapai);
                }
                EventDispatcher.FireEvent(GEnum.NamedEvent.EMjControlSetTipPos, card.selfTransform.position);
                card = null;
            }
        }

    }
    public override void SetCardHeighLight(bool state, int mjCode)
    {
        base.SetCardHeighLight(state, mjCode);
        if (_PutCards != null && _PutCards.Count > 0)
        {
            for (int i = 0; i < _PutCards.Count; i++)
            {
                _PutCards[i].SetCardHeighLight(state, mjCode);
            }
        }
    }
    #endregion
}
