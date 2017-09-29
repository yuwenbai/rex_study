/**
 * @Author YQC
 *
 *
 */

using UnityEngine;
using UnityEditor;
using projectQ;

[CanEditMultipleObjects]
#if UNITY_3_5
[CustomEditor(typeof(UIButton))]
#else
[CustomEditor(typeof(UIDefinedButton), true)]
#endif
public class UIDefinedButtonEditor : UIButtonEditor
{
    protected override void DrawProperties()
    {
        base.DrawProperties();
        DrawWaitTime();
        DrawWaitTimeLabel();
        DrawSoundSelect();
        DrawMaxClickCount();
    }

    protected void DrawWaitTime()
    {
        GUILayout.BeginHorizontal();
        NGUIEditorTools.DrawProperty("按钮点击时间间隔", serializedObject, "WaitTime", GUILayout.Width(120f));
        GUILayout.Label("seconds");
        GUILayout.EndHorizontal();
        GUILayout.Space(3f);
    }

    protected void DrawWaitTimeLabel()
    {
        NGUIEditorTools.DrawProperty("修改时间的Label", serializedObject, "LabelUpdateTime");
    }

    protected void DrawSoundSelect()
    {
        NGUIEditorTools.DrawProperty("SoundSelect", serializedObject, "SoundSelect" , GUILayout.Width(200f));
    }
    protected void DrawMaxClickCount()
    {
        NGUIEditorTools.DrawProperty("最大点击次数", serializedObject, "MaxClickCount", GUILayout.Width(200f));
        NGUIEditorTools.DrawProperty("是否在Enable重置次数", serializedObject, "isOnEnableResetMaxClickCount", GUILayout.Width(200f));
        NGUIEditorTools.DrawProperty("禁用采用UIButton的isEnable", serializedObject, "IsButtonEnabled", GUILayout.Width(200f));
    }
}
