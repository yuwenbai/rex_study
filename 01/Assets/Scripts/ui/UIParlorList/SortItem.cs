/**
 * 作者：周腾
 * 作用：
 * 日期：
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UI.ScrollViewTool;
using System;

namespace projectQ
{
    public class SortItemData
    {
        //public int index;
        //public string cityName;
        //public int cityId;
        //public int regionId;
     
        public int cityId;
        public string cityName;
        public bool IsSelect = false;
        public Action<SortItemData, GameObject> OnClick;
    }
    public class SortItem : ScrollViewItemBase<SortItemData>
    {

        public UISprite bgNoSelect;
        public UILabel textLabel;
        public UISprite bgSprite;
        public GameObject clickObj;
        //private int index;
        private string typeName;
        private int playTypeId;
        public delegate void OnItemClick(int index, string text);
        public OnItemClick dele_ItemClick;

        public void InitItem(int typeId, string text)
        {
            this.playTypeId = typeId;
            this.typeName = text;
            textLabel.text = text;
        }

        public void ShowSelect()
        {
            bgSprite.gameObject.SetActive(true);
            bgNoSelect.gameObject.SetActive(false);
            //textLabel.text = "[753802]" + typeName + "[-]";
            //textLabel.alpha = 1f;
            //  bgSprite.spriteName = "friend_button_02";
            //bgSprite.alpha = 1;
        }
        public void ShowNoSelect()
        {

            bgSprite.gameObject.SetActive(false);
            bgNoSelect.gameObject.SetActive(true);
            //textLabel.alpha = 0.5f;
            //textLabel.text = "[FFFFFF]" + typeName + "[-]";
            //  bgSprite.spriteName = "friend_button_01";
            //bgSprite.alpha = 0.2f;
        }
        private void Awake()
        {
            UIEventListener.Get(clickObj).onClick = ItemClick;
        }

        void ItemClick(GameObject go)
        {
            
            if (dele_ItemClick != null)
            {
                dele_ItemClick(playTypeId, typeName);
            }
            if (this.UIData != null)
            {
               
                if (this.UIData.OnClick != null)
                {
                    this.UIData.OnClick(this.UIData, gameObject);
                }
            }
          
        }
        //public void ShowOrHide(int cityId)
        //{
        //    if (this.cityID == cityId)
        //    {
        //        ShowSelect();
        //    }
        //    else
        //    {
        //        ShowNoSelect();
        //    }
        //}
        public override void Refresh()
        {
            this.playTypeId = UIData.cityId;
          
            this.typeName = UIData.cityName;
            textLabel.text = UIData.cityName;
            //bgSprite.spriteName = "friend_button_01";
            if (UIData.IsSelect)
            {
                ShowSelect();
            }
            else
            {
                ShowNoSelect();
            }
        }
    }
}
