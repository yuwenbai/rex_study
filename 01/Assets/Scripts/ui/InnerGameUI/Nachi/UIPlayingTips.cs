/**
 * @Author JEFF
 *
 *
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using projectQ;
public enum UIPlayingTipsType
{
    Null,
    TipsWithThreeMarks,//message+三个小叹号
    OnlyTips
}
public class UIPlayingTips : UIViewBase
{
    #region 组件
    public UILabel CenterAlignTipsLabel;
    public UILabel RightAlignTipsLabel;
    //三个小叹号
    public UILabel AnimLabel_1;
    public UILabel AnimLabel_2;
    public UILabel AnimLabel_3;
    #endregion

    #region 属性
    #endregion

    #region 方法

    public override void Init()
    {
        SetAnim(false);
        EnableLabel(CenterAlignTipsLabel,false);
        EnableLabel(RightAlignTipsLabel, false);
    }
     public override void OnShow()
    {

    }
    public override void OnHide()
    {

    }

    private void EnableLabel(UILabel label,bool enable)
    {
        if (!NullHelper.IsObjectIsNull(label))
        {
            GameObjectHelper.SetEnable(label.gameObject, enable);
        }
    }
    /// <summary>
    /// 显示对应的tips信息
    /// </summary>
    /// <param name="text">显示的文本内容</param>
    /// <param name="hasMarks">是否有三个小点点的动画</param>
    /// <param name="localPosition">相对于屏幕中心的偏移量</param>
    public void RefreshUI(string text, UIPlayingTipsType type, Vector2 localPosition)
    {
        this.transform.localPosition = localPosition;
        if (type == UIPlayingTipsType.OnlyTips)
        {
            if (!NullHelper.IsObjectIsNull(CenterAlignTipsLabel))
            {
                EnableLabel(CenterAlignTipsLabel, true);
                CenterAlignTipsLabel.text = text;
            }
        }
        else
        {
            if (!NullHelper.IsObjectIsNull(RightAlignTipsLabel))
            {
                EnableLabel(RightAlignTipsLabel,true);
                RightAlignTipsLabel.text = text;
            }
            this.StartCoroutine(PlayingAnim());
        }
    
    }

    private IEnumerator PlayingAnim()
    {
        if (NullHelper.IsObjectIsNull(AnimLabel_1) || NullHelper.IsObjectIsNull(AnimLabel_2) || NullHelper.IsObjectIsNull(AnimLabel_3))
        {
            yield break;
        }
        while (true)
        {
            AnimLabel_1.gameObject.SetActive(true);
            AnimLabel_2.gameObject.SetActive(false);
            AnimLabel_3.gameObject.SetActive(false);
            yield return new WaitForSeconds(0.3f);
            AnimLabel_2.gameObject.SetActive(true);
            yield return new WaitForSeconds(0.3f);
            AnimLabel_3.gameObject.SetActive(true);
            yield return new WaitForSeconds(0.3f);
        }
    }
    void OnDisable()
    {
        SetAnim(false);
    }
    private void SetAnim(bool enable)
    {
        if (NullHelper.IsObjectIsNull(AnimLabel_1) || NullHelper.IsObjectIsNull(AnimLabel_2) || NullHelper.IsObjectIsNull(AnimLabel_3))
        {
            return;
        }
        AnimLabel_1.gameObject.SetActive(enable);
        AnimLabel_2.gameObject.SetActive(enable);
        AnimLabel_3.gameObject.SetActive(enable);
    }
    #endregion
}
