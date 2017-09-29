using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
[CustomEditor(typeof(TestAssets))]
public class TestAssetsInspector : Editor {


    public SerializedProperty bulletType;

    public SerializedProperty speed;

    public SerializedProperty damage;

    private void OnEnable()
    {
        bulletType = serializedObject.FindProperty("bulletType");

        speed = serializedObject.FindProperty("speed");

        damage = serializedObject.FindProperty("damage");
    }
    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        EditorGUI.indentLevel = 1;
        EditorGUILayout.PropertyField(bulletType, new GUIContent("type"));
        GUILayout.Space(5);
        EditorGUILayout.PropertyField(speed, new GUIContent("speed"));
        GUILayout.Space(5);
        EditorGUILayout.PropertyField(damage, new GUIContent("damage"));
        GUILayout.Space(10);

        if (GUI.changed)
        {
            EditorUtility.SetDirty(target);
        }

        serializedObject.ApplyModifiedProperties();
    }
}
