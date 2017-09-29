/****************************************************
 *  Copyright(C) 2015 智幻点击
 *	版权所有。
 *	作者:于秋辰
 *	创建时间：5/10/2016
 *	文件名：  EditorWindow.cs
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

public static class YQCEditorCommon
{

    public enum YQCEditorWindowTypes
    {
        UpdateAtlas = 0,
        ReplaceImg = 1,
        ConfigEditor = 2,
    }
    public static EditorWindow ShowWindow(YQCEditorWindowTypes ewt = YQCEditorWindowTypes.UpdateAtlas)
    {
        System.Type[] allEditorTypes = new System.Type[] { typeof(AutoUpdateAtlasEditor), typeof(ReplaceImgrEditor), typeof(AtlasConfigEditor) };
        string[] titles = new string[] { "图集更新" ,"图片转换","配置"};
        EditorWindow ufes = EditorWindow.GetWindow<AutoUpdateAtlasEditor>(titles[0], false, allEditorTypes);
        Rect rt = ufes.position;
        rt.xMin = 100;
        rt.yMin = 100;
        rt.width = 1000;
        rt.height = 800;
        EditorWindow.GetWindow<ReplaceImgrEditor>(titles[1], false, allEditorTypes);
        EditorWindow.GetWindow<AtlasConfigEditor>(titles[2], false, allEditorTypes);
        return EditorWindow.GetWindow(allEditorTypes[(int)ewt]);
    }
}
