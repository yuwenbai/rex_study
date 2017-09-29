/****************************************************
 *  Copyright(C) 2015 智幻点击
 *	版权所有。
 *	作者:于秋辰
 *	创建时间：5/10/2016
 *	文件名：  AtlasConfigEditor.cs
 *	文件功能描述：
 *  创建标识：yqc.5/10/2016
 *	创建描述：
 *
 *  修改标识：
 *  修改描述：
 *
 *
 *
 *****************************************************/
using UnityEngine;
using UnityEditor;
using System.Collections;


public class AtlasConfigEditor : EditorWindow
{
    string atlasPath = "Assets/Resources";
    string prefabPath = "Assets/Resources";
    void OnGUI()
    {
        #region 图集路径
        EditorGUILayout.BeginHorizontal();
		EditorGUILayout.PrefixLabel("图集搜索路径");
        atlasPath = EditorGUILayout.TextField(atlasPath);
		EditorGUILayout.EndHorizontal();
        #endregion

        #region 图集路径
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.PrefixLabel("Prefab搜索路径");
        prefabPath = EditorGUILayout.TextField(prefabPath);
        EditorGUILayout.EndHorizontal();
        #endregion

        #region 按钮
        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("保存"))
        {
            PlayerPrefs.SetString("Editor_AtlasPath", atlasPath);
            PlayerPrefs.SetString("Editor_PrefabPath", prefabPath);
            GUIAtlasDataManager.Instance.AtlasPath = atlasPath;
            GUIAtlasDataManager.Instance.PrefabPath = prefabPath;
        }
        if (GUILayout.Button("还原"))
        {
            atlasPath = GUIAtlasDataManager.Instance.AtlasPath;
            prefabPath = GUIAtlasDataManager.Instance.PrefabPath;
        }
        EditorGUILayout.EndHorizontal();
        #endregion
    }
}
