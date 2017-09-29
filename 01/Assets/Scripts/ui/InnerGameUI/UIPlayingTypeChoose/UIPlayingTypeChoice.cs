/**
 * @Author JEFF
 *
 *
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MahjongPlayType;
namespace projectQ
{
    public class UIPlayingTypeChoice : UIViewBase
    {
        #region 组件引用
        public UIChooseItem TemplateItem;
        public UIGrid Grid;
        public GameObject SubmitBtn;
        public GameObject CancelBtn;
        public UISprite SubmitGraySprite;
        #endregion

        #region 数据
        private MjCanNaoMoData _Data;
        private List<UIChooseItem> _ChooseItems = new List<UIChooseItem>();
        #endregion


        #region Override
        public override void Init()
        {
            if (!NullHelper.IsObjectIsNull(SubmitBtn))
            {
                UIEventListener.Get(SubmitBtn).onClick = OnSubmit;
            }
            if (!NullHelper.IsObjectIsNull(CancelBtn))
            {
                UIEventListener.Get(CancelBtn).onClick = OnCancel;
            }
        }
        private void OnCancel(GameObject obj)
        {
            for (int i = 0; i < _ChooseItems.Count; i++)
            {
                _ChooseItems[i].ResetState();
            }
            OnSubmit(obj);
        }
        private void OnSubmit(GameObject obj)
        {

            if (_Data != null)
            {
                EventDispatcher.FireEvent(GEnum.NamedEvent.UIPlayingTypeChooseOver, _Data);
            }
            this.Close();
        }
        public override void OnHide()
        {
        }
        public override void OnShow()
        {
            if (NullHelper.IsObjectIsNull(_Data) || NullHelper.IsObjectIsNull(Grid) || NullHelper.IsObjectIsNull(TemplateItem))
            {
                return;
            }
            if (_Data.dataList != null)
            {
                SetSubmitGray();
                for (int i = 0; i < _Data.dataList.Count; i++)
                {
                    UIChooseItem item = GameTools.InstantiatePrefabAndReturnComp<UIChooseItem>(TemplateItem.gameObject, Grid.transform, true, true);
                    if (!NullHelper.IsObjectIsNull(item))
                    {
                        item.FillItem(_Data.dataList[i], OnClickChooseItem);
                        item.gameObject.SetActive(true);
                        _ChooseItems.Add(item);
                    }
                }
            }
            Grid.Reposition();
        }
        private void SetSubmitGray()
        {
            bool shouldGray = true;
            for (int i = 0; i < this._Data.dataList.Count; i++)
            {
                if (this._Data.dataList[i].chooseState)
                {
                    shouldGray = false;
                }
            }
            SubmitBtn.GetComponent<UIDefinedButton>().isEnabled = !shouldGray;
        }
        private void OnClickChooseItem(MjCanNaoMoData.CommonData data)
        {
            if (_Data.dataList != null && _Data.dataList.Contains(data))
            {
                SetSubmitGray();
                EventDispatcher.FireEvent(GEnum.NamedEvent.UIChooseItemClickNotify, data);
            }
        }
        #endregion

        public override void OnPushData(object[] data)
        {
            if (!NullHelper.IsInvalidIndex(0, data))
            {
                _Data = (MjCanNaoMoData)data[0];
            }
        }


    }
}


