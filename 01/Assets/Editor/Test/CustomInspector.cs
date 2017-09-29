using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using projectQ;

[CustomEditor(typeof(AnimatorHand_Main))]
public class CustomInspector : Editor {

    private AnimatorHand_Main animatorHand_Main;
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        animatorHand_Main = (AnimatorHand_Main)target;
        EditorGUILayout.BeginVertical();
        //空两行
        EditorGUILayout.Space();
        EditorGUILayout.Space();

        EditorGUILayout.LabelField("Base Info");

        GUILayout.Label("文件名:", EditorStyles.boldLabel);


        if (GUILayout.Button("选择模型"))
        {
            string path = EditorUtility.OpenFilePanel("Load png Textures of Directory", "", "");
            int n = path.LastIndexOf('/') + 1;
            string nn = path.Substring(n);

            int l = nn.LastIndexOf(".") ;
            string nnn = nn.Substring(0,l);

            string newPath = animatorHand_Main.prefabPath + nnn;
            Debug.Log("rextest new path is " + newPath);
            GameObject prefab = ResourcesDataLoader.Load<GameObject>(newPath);
            animatorHand_Main.OBJ = prefab;
        }


        //EditorGUILayout.ObjectField()

        //GUILayout.

        EditorGUILayout.EndVertical();
    }
    private void OnGUI()
    {
        GUILayout.Label("文件名:", EditorStyles.boldLabel);
    }
}
