using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Car))]
[CanEditMultipleObjects]
public class CarEditor : Editor {

    private Car _car;

    public override void OnInspectorGUI()
    {
        _car = (Car)target;

        _car.WheelCount = EditorGUILayout.IntSlider("WheelNumber", _car.wheelCount, 0, 20);
        //当Inspector 面板发生变化时保存数据
        if (GUI.changed)
        {
            EditorUtility.SetDirty(target);
        }
    }
}
