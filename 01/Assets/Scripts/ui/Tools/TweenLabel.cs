

using System;
/**
* @Author YQC
*
*
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[AddComponentMenu("NGUI/Tween/Tween Position")]
public class TweenLabel : UITweener
{
    public List<TweenLabelData> listData;
    public UILabel TagetLabel;

    protected override void OnUpdate(float factor, bool isFinished)
    {
        if (listData.Count == 0 || TagetLabel == null) return;

        for (int i = 0; i < listData.Count; i++)
        {
            if(factor <= listData[i].Value)
            {
                if (TagetLabel.text != listData[i].Content)
                    TagetLabel.text = listData[i].Content;
                return;
            }
        }
    }
    protected override void Start()
    {
        base.Start();
        if (listData == null)
            listData = new List<TweenLabelData>();

        float total = 0.0f;
        for (int i = 0; i < listData.Count; i++)
        {
            total += listData[i].Weight;
        }
        float temp = 0f;
        for (int i = 0; i < listData.Count; i++)
        {
            temp += listData[i].Weight / total;
            listData[i].Value = temp;
        }
        if (TagetLabel == null)
            TagetLabel = GetComponent<UILabel>();
    }
}
[System.Serializable]
public class TweenLabelData
{
    public string Content = string.Empty;
    public float Weight = 1f;

    private float _value;
    public float Value
    {
        set { _value = value; }
        get { return _value; }
    }
}
