/**
 * 作者：周腾
 * 作用：
 * 日期：
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIUserAreaItem : MonoBehaviour {
    public UISprite bgSpriteSelect;
    public UILabel areaLabelSelect;
    public UISprite bgNoselect;
    public UILabel areaLabelNoSelect;
    private string playName;
    private int playId;
    public GameObject item;
    public delegate void OnItemClick(int itemIndex);
    public OnItemClick dele_ItemClick;
    private bool isShow;
    private void Awake()
    {
        UIEventListener.Get(item).onClick = ItemClick;
    }
    public void InitAreaItem(int playId, string playName)
    {
        areaLabelSelect.text = playName;
        areaLabelNoSelect.text = playName;
        this.playId = playId;
        this.playName = playName;
      
    }
    public void ShowSelect()
    {
        isShow = true;
        bgSpriteSelect.gameObject.SetActive(true);
        areaLabelSelect.gameObject.SetActive(true);
        bgNoselect.gameObject.SetActive(false);
        areaLabelNoSelect.gameObject.SetActive(false);

    }

    public void ShowNoSelect()
    {
        isShow = false;
        bgSpriteSelect.gameObject.SetActive(false);
        areaLabelSelect.gameObject.SetActive(false);
        bgNoselect.gameObject.SetActive(true);
        areaLabelNoSelect.gameObject.SetActive(true);

    }
    void ItemClick(GameObject go)
    {
        if (isShow)
        {
            return;
        }
        else
        {
            if (dele_ItemClick != null)
            {
                dele_ItemClick(playId);
            }
        }
        
    }
}
