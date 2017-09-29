/**
 * @Author JEFF
 *
 *
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MahjongPlayType;
using projectQ;

[System.Serializable]
public class ButtonSkinItem
{
    public EnumChooseType ChooseType;
    public GameObject BtnObj;
    public GameObject ChooseSprite;
    public void EnableChoose(bool enable)
    {
        GameObjectHelper.SetEnable(ChooseSprite, enable);
    }
}

public class UIChooseItem : MonoBehaviour
{
    #region 组件
    public UILabel TextLabel;
    public List<ButtonSkinItem> Buttons = new List<ButtonSkinItem>();
    private ButtonSkinItem _ShowItem;
    #endregion

    #region 数据
    private MjCanNaoMoData.CommonData _MjCanNaoMoData;
    private System.Action<MjCanNaoMoData.CommonData> _ClickCallBack;
    private bool _initState = false;
    #endregion
    public void ResetState()
    {
        if (!NullHelper.IsObjectIsNull(_MjCanNaoMoData))
        {
            _MjCanNaoMoData.chooseState = _initState;
        }
    }

    public void FillItem(MjCanNaoMoData.CommonData data, System.Action<MjCanNaoMoData.CommonData> action)
    {
        if (NullHelper.IsObjectIsNull(data))
        {
            return;
        }
        _ClickCallBack = action;
        _MjCanNaoMoData = data;
        if (!NullHelper.IsObjectIsNull(TextLabel))
        {
            TextLabel.text = data.chooseName;
        }
        for (int i = 0; i < Buttons.Count; i++)
        {
            if (!NullHelper.IsObjectIsNull(Buttons[i].BtnObj))
            {
                if (Buttons[i].ChooseType == data.chooseType)
                {
                    _initState = data.chooseState;
                    _ShowItem = Buttons[i];
                    _ShowItem.EnableChoose(data.chooseState);
                    _ShowItem.BtnObj.gameObject.SetActive(true);
                    UIEventListener.Get(_ShowItem.BtnObj).onClick = ClickButton;
                }
                else
                {
                    Buttons[i].BtnObj.gameObject.SetActive(false);
                }
            }

        }


    }
    private void ClickButton(GameObject obj)
    {
        _MjCanNaoMoData.chooseState = !_MjCanNaoMoData.chooseState;
        _ShowItem.EnableChoose(_MjCanNaoMoData.chooseState);
        if (_ClickCallBack != null)
        {
            _ClickCallBack(_MjCanNaoMoData);
        }

    }
}
