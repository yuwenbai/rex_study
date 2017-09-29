
/**
* @Author YQC
*
*
*/
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace projectQ
{
    public class OptionPopupItemData
    {
        public MahjongPlayOption optionPlay;

        public bool IsSelected;
        public Action<OptionPopupItemData> clickCallBack;
        public void CallBack()
        {
            if (clickCallBack != null)
                clickCallBack(this);
        }
    }
    public class OptionPopupListItem : NGUIPopupListItemBase
    {
        public new OptionPopupItemData UIData
        {
            get { return base.UIData as OptionPopupItemData; }
        }
        public UILabel labelName;
        public UISprite spriteSelected;

        public override string Name
        {
            get
            {
                return labelName.text;
            }
        }

        #region override
        public override void Refresh()
        {
            labelName.text = this.UIData.optionPlay.Name;
            RefreshSelectUI(this.UIData.IsSelected);
        }

        public override void RefreshSelectUI(bool isSelected)
        {
            spriteSelected.enabled = isSelected;
        }

        public override void Selected()
        {
            spriteSelected.enabled = true;
            this.UIData.IsSelected = true;
            base.Selected();
            this.UIData.CallBack();
        }
        #endregion

        #region Event
        private void OnLabelClick(GameObject go)
        {
            this.OnItemClick();
            this.Selected();
        }
        #endregion

        #region 生命周期
        public void Awake()
        {
            UIEventListener.Get(labelName.gameObject).onClick = OnLabelClick;
        }
        #endregion
    }
}