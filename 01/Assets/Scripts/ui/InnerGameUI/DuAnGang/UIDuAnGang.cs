/**
 * @Author JEFF
 *
 *
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using projectQ;
public class UIDuAnGang : UIViewBase
{
    #region　组件
    public GameObject CancelBtn;
    public GameObject WagerBtn;
    public UIMahjongNachiChooseItem TempleteCard;
    public UIGrid Grid;
    public UILabel GridTitle;
    public GameObject GridParent;
    public GameObject DuAnGangContent;
    public GameObject DuAnGangSuccObj;
    public GameObject DuAnGangFailObj;
    #endregion
    private List<int> _MJCodes = new List<int>();
    private List<UIMahjongNachiChooseItem> Cards = new List<UIMahjongNachiChooseItem>();

    #region 重写基类方法
    public override void Init()
    {
        GameObjectHelper.SetEnable(DuAnGangSuccObj, false);
        GameObjectHelper.SetEnable(DuAnGangFailObj, false);

        if (!NullHelper.IsObjectIsNull(CancelBtn))
        {
            UIEventListener.Get(CancelBtn).onClick = OnCancel;
        }
        if (!NullHelper.IsObjectIsNull(WagerBtn))
        {
            UIEventListener.Get(WagerBtn).onClick = OnWanger;
        }
    }
    public override void OnShow()
    {

    }
    public override void OnHide()
    {

    }
    public void RefreshUI()
    {
        MahjongPlayType.MahjongPlayOprateDuAnGang duAnGangData = MjDataManager.Instance.MjData.ProcessData.processDuAnGang;
        if (NullHelper.IsObjectIsNull(duAnGangData) || NullHelper.IsObjectIsNull(duAnGangData.NotifyDuAnGangData))
        {
            return;
        }
        _MJCodes = duAnGangData.NotifyDuAnGangData.MjCodeList;
        if (duAnGangData.NotifyDuAnGangData.SeatID != MjDataManager.Instance.MjData.curUserData.selfSeatID)
        {
            GameObjectHelper.SetEnable(DuAnGangContent, false);
        }
        else
        {
            GameObjectHelper.SetEnable(DuAnGangContent, true);
        }


    }
    #endregion

    public void RefreshResultUI()
    {
        string animPath = "";
        GameObject animParent = null;
        bool isAnim = false;
        MahjongPlayType.RspDuAnGangData data = MjDataManager.Instance.MjData.ProcessData.processDuAnGang.RspDuAnGangData;
        if (NullHelper.IsObjectIsNull(data))
        {
            return;
        }
        if (data.ResultCode == 1)
        {
            isAnim = true;
            GameObjectHelper.SetEnable(DuAnGangSuccObj, true);
            animPath = PrefabPathDefine.DAG_SuccAnim;
            animParent = DuAnGangSuccObj;
        }
        if (data.ResultCode == 3)
        {
            isAnim = true;
            GameObjectHelper.SetEnable(DuAnGangFailObj, true);
            animPath = PrefabPathDefine.DAG_FailAnim;
            animParent = DuAnGangFailObj;
        }
        if (isAnim == true)
        {
            GameObject obj = PrefabCachePool.Intance.CreateGameObject(animPath);
            if (!NullHelper.IsObjectIsNull(obj) )
            {
                if (!NullHelper.IsObjectIsNull(animParent))
                {
                    obj.transform.parent = animParent.transform;
                }
                else
                {
                    obj.transform.parent = this.transform;
                }
                GameObjectHelper.NormalizationTransform(obj.transform);
                obj.gameObject.SetActive(true);
            }
            this.StartCoroutine(HideResultUI());
        }

    }
    private IEnumerator HideResultUI()
    {
        yield return new WaitForSeconds(ConstDefine.DAG_HideDelayTime);
        GameObjectHelper.SetEnable(DuAnGangSuccObj, false);
        GameObjectHelper.SetEnable(DuAnGangFailObj, false);
    }
    private void LoadItems()
    {
        if (_MJCodes == null || _MJCodes.Count <= 0)
        {
            return;
        }
        if (!NullHelper.IsObjectIsNull(GridTitle))
        {
            GridTitle.text = UIConstStringDefine.DuAnGangCardTitle;
        }
        if (NullHelper.IsObjectIsNull(GridParent))
        {
            return;
        }
        GridParent.gameObject.SetActive(true);
        this.StartCoroutine(EnalbeChooseCard());
        if (NullHelper.IsObjectIsNull(TempleteCard) || NullHelper.IsObjectIsNull(Grid))
        {
            return;
        }
        for (int i = 0; i < _MJCodes.Count; i++)
        {
            UIMahjongNachiChooseItem item = GameObject.Instantiate<UIMahjongNachiChooseItem>(TempleteCard);
            if (NullHelper.IsObjectIsNull(item))
            {
                return;
            }
            Grid.AddChild(item.transform);
            GameObjectHelper.NormalizationTransform(item.transform);
            item.gameObject.SetActive(true);
            item.FillItem(_MJCodes[i], OnClickCard);
            Cards.Add(item);
        }
        Grid.Reposition();
    }
    private IEnumerator EnalbeChooseCard()
    {
        yield return new WaitForSeconds(1f);
        BoxCollider collider = GridParent.gameObject.GetComponent<BoxCollider>();
        collider.enabled = false;
    }
    private void OnClickCard(GameObject cardObj)
    {
        UIMahjongNachiChooseItem mjItem = cardObj.GetComponent<UIMahjongNachiChooseItem>();
        if (!NullHelper.IsObjectIsNull(mjItem))
        {
            SetUploadData(1, mjItem.CardID);
        }
        GameObjectHelper.SetEnable(DuAnGangContent, false);
    }
    private void OnCancel(GameObject obj)
    {
        GameObjectHelper.SetEnable(DuAnGangContent, false);
        SetUploadData(2, -1);
    }
    private void SetUploadData(int type, int mjCode)
    {
        MahjongPlayType.UploadDuAnGangData data = new MahjongPlayType.UploadDuAnGangData();

        data.DeskID = MjDataManager.Instance.MjData.curUserData.selfDeskID;
        data.SeatID = MjDataManager.Instance.MjData.curUserData.selfSeatID;
        data.Type = type;
        data.MjCode = mjCode;
        MjDataManager.Instance.MjData.ProcessData.processDuAnGang.UploadDuAnGangData = data;
        EventDispatcher.FireEvent(MJEnum.DuAnGangEvents.DAG_LogicUploadData.ToString());
    }
    private void OnWanger(GameObject obj)
    {
        GameObjectHelper.SetEnable(WagerBtn, false);
        LoadItems();
    }

}
