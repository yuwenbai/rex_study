/**
 * @Author JEFF
 *
 *
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using projectQ;
public class UINiuPai : UIViewBase
{

    #region 组件
    public GameObject PassBtn;
    public GameObject NiuBtn;
    #endregion

    #region 属性
    #endregion

    #region 方法

    public override void Init()
    {
        if (!NullHelper.IsObjectIsNull(PassBtn))
        {
            UIEventListener.Get(PassBtn).onClick = OnClickPassBtn;
        }
        if (!NullHelper.IsObjectIsNull(NiuBtn))
        {
            UIEventListener.Get(NiuBtn).onClick = OnClickNiuBtn;
        }
    }
    public void RefreshUI()
    { }
    public override void OnShow()
    {

    }
    public override void OnHide()
    {

    }
    private void OnClickPassBtn(GameObject obj)
    {
        MahjongPlayType.RequestNiuPai data = CreateRequestData();
        // data.Data = new List<List<int>>();
        data.Data = MjDataManager.Instance.MjData.ProcessData.processNiuPai.NotifyNiuPaiData.Data;
        data.SelectedID = 2;
        EventDispatcher.FireEvent(MJEnum.NiuPaiEvent.NPE_LogicRepNiuPai.ToString());
    }
    private void OnClickNiuBtn(GameObject obj)
    {
        MahjongPlayType.RequestNiuPai data = CreateRequestData();
        data.SelectedID = 1;     
        data.Data = MjDataManager.Instance.MjData.ProcessData.processNiuPai.NotifyNiuPaiData.Data;
        EventDispatcher.FireEvent(MJEnum.NiuPaiEvent.NPE_LogicRepNiuPai.ToString());
    }
    private MahjongPlayType.RequestNiuPai CreateRequestData()
    {
        MahjongPlayType.RequestNiuPai data = new MahjongPlayType.RequestNiuPai();
        MjDataManager.Instance.MjData.ProcessData.processNiuPai.RequestNiuPai = data;
        data.SeatID = MjDataManager.Instance.MjData.curUserData.selfSeatID;
        data.DeskID = MjDataManager.Instance.MjData.curUserData.selfDeskID;
        data.IsNiuPai = MjDataManager.Instance.MjData.ProcessData.processNiuPai.NotifyNiuPaiData.IsNiuPai;
        data.Data = MjDataManager.Instance.MjData.ProcessData.processNiuPai.NotifyNiuPaiData.Data;
        return data;
    }
    #endregion
}
