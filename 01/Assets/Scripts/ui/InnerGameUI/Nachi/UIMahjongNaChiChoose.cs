/**
 * @Author JEFF
 *
 *
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using projectQ;
using MahjongPlayType;
public class UIMahjongNaChiChoose : UIViewBase
{
    #region 组件挂载
    public UIGrid Grid;
    #endregion

    #region 数据
    private List<GameObject> _Items = new List<GameObject>();
    #endregion
    #region

    public override void Init()
    {

    }
    public override void OnShow()
    {

    }
    public override void OnHide()
    {
       
    }
    public void RefreshUI()
    {
        MahjongPlayType.GangHouNaChiNotifyData serverData = MjDataManager.Instance.MjData.ProcessData.processGangHouNaChi.GangHouNaChiNotifyData;
        if (NullHelper.IsObjectIsNull(serverData))
        {
            return;
        }
        List<int> mjCodes = serverData.MjCodeList;
        if (NullHelper.IsListNullOrEmpty(mjCodes))
        {
            return;
        }

        if (NullHelper.IsInvalidIndex(0, mjCodes))
        {
            return;
        }
        if (NullHelper.IsObjectIsNull(Grid))
        {
            return;
        }
        Grid.transform.DestroyChildren();
        for (int i = 0; i < mjCodes.Count; i++)
        {
            if (mjCodes[i] <= 0)
            {
                DebugPro.DebugError("拿吃选牌的cardid errror:", mjCodes[i]);
                continue;
            }

            GameObject obj = PrefabCachePool.Intance.CreateGameObject(PrefabPathDefine.GangHouNaChiChooseItem);
            if (NullHelper.IsObjectIsNull(obj))
            {
                return;
            }
            UIMahjongNachiChooseItem item = obj.GetComponent<UIMahjongNachiChooseItem>();

            if (GameObjectHelper.SetParentAndEnableObj(obj, Grid.transform, true))
            {
                item.name = "UIMahjongNachiChooseItem_" + i;
                item.FillItem(mjCodes[i], OnClickChooseItem);
                _Items.Add(item.gameObject);
            }
        }
        Grid.Reposition();
    }


    private void OnClickChooseItem(GameObject obj)
    {
        if (_Items.Contains(obj))
        {
            int cardIndex = _Items.IndexOf(obj);
            if (NullHelper.IsObjectIsNull(MjDataManager.Instance.MjData.ProcessData.processGangHouNaChi))
            {
                return;
            }
            MahjongPlayType.GangHouNaChiUploadData data = new MahjongPlayType.GangHouNaChiUploadData();
            if (!NullHelper.IsInvalidIndex(cardIndex, MjDataManager.Instance.MjData.ProcessData.processGangHouNaChi.GangHouNaChiNotifyData.MjCodeList))
            {
                data.DeskID = MjDataManager.Instance.MjData.curUserData.selfDeskID;
                data.SeatID = MjDataManager.Instance.MjData.curUserData.selfSeatID;
                data.MjCode = MjDataManager.Instance.MjData.ProcessData.processGangHouNaChi.GangHouNaChiNotifyData.MjCodeList[cardIndex];
                data.MjIndex = cardIndex + 1;
            }
            MjDataManager.Instance.MjData.ProcessData.processGangHouNaChi.GangHouNaChiUploadData = data;
            EventDispatcher.FireEvent(MJEnum.GangHouNaChi.GHNC_LogicUploadResult.ToString());
        }
    }


    #endregion
}


