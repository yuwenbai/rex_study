/****************************************************
 *  Copyright(C) 2015 智幻点击
 *	版权所有。
 *	作者:于秋辰
 *	创建时间：5/10/2016
 *	文件名：  ReplaceImgrEditor.cs
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
using System.Collections.Generic;

public class ReplaceImgrEditor : EditorWindow
{

    Vector2 scrollPos;
    Vector2 scrollPos2;
    bool showOldImgPrefab = false;
    int oldReplaceAtlasIndex = -1;
    int newReplaceAtlasIndex = -1;
    string oldSpriteName = null;
    string newSpriteName = null;
    public Rect rect;
    bool oldReplacePrefabAllToggle = false;
    bool[] oldReplacePrefabToggles = new bool[1];

    void OnGUI()
    {
        scrollPos = EditorGUILayout.BeginScrollView(scrollPos);
        do
        {
            if (GUILayout.Button("搜索图集"))
            {
                GUIAtlasDataManager.Instance.Refresh();
            }
            string[] atlasPaths = GUIAtlasDataManager.Instance.GetAtlasPaths();
            if (atlasPaths == null || atlasPaths.Length == 0) break;

            #region 更换Prefab的图集的图片
            Rect replcaeRect = EditorGUILayout.BeginHorizontal();
            #region 选择图集
            //设置旧图集的图片
            oldReplaceAtlasIndex = EditorGUILayout.Popup(oldReplaceAtlasIndex, atlasPaths);
            GUIAtlasData oldAtlasData = null;
            if (oldReplaceAtlasIndex >= 0 && oldReplaceAtlasIndex < atlasPaths.Length)
            {
                oldAtlasData = GUIAtlasDataManager.Instance.GetAtlasDataByPath(atlasPaths[oldReplaceAtlasIndex]);
                if (GUILayout.Button(string.IsNullOrEmpty(oldSpriteName) ? "请选择图片" : oldSpriteName))
                {
                    NGUISettings.selectedSprite = oldSpriteName;
                    NGUISettings.atlas = oldAtlasData.MyAtlas;
                    SpriteSelector.Show(delegate(string spriteName)
                    {
                        oldSpriteName = spriteName;
                    });
                }
            }
            EditorGUILayout.Space();

            //设置新图集的图片
            newReplaceAtlasIndex = EditorGUILayout.Popup(newReplaceAtlasIndex, atlasPaths);
            GUIAtlasData newAtlasData = null;
            if (newReplaceAtlasIndex >= 0 && newReplaceAtlasIndex < atlasPaths.Length)
            {
                newAtlasData = GUIAtlasDataManager.Instance.GetAtlasDataByPath(atlasPaths[newReplaceAtlasIndex]);
                if (GUILayout.Button(string.IsNullOrEmpty(newSpriteName) ? "请选择图片" : newSpriteName))
                {
                    NGUISettings.selectedSprite = newSpriteName;
                    NGUISettings.atlas = newAtlasData.MyAtlas;
                    SpriteSelector.Show(delegate(string spriteName)
                    {
                        newSpriteName = spriteName;
                    });
                }
            }
            #endregion
            EditorGUILayout.EndHorizontal();

            #region 被替换图片 所关联的Prefab
            showOldImgPrefab = EditorGUILayout.Foldout(showOldImgPrefab, "被替换图片管理的Prefab");
            if (showOldImgPrefab)
            {
                List<GameObject> oldPrefabList = GUIAtlasDataManager.Instance.GetPrefabListByImgName(oldAtlasData, oldSpriteName);
                if (oldPrefabList != null && oldPrefabList.Count > 0)
                {
                    if (oldReplacePrefabToggles.Length != oldPrefabList.Count)
                        oldReplacePrefabToggles = new bool[oldPrefabList.Count];

                    bool tempOldReplacePrefabAllToggle = EditorGUILayout.ToggleLeft("以下哪些Prefab被替换", oldReplacePrefabAllToggle);
                    if (tempOldReplacePrefabAllToggle != oldReplacePrefabAllToggle)
                    {
                        oldReplacePrefabAllToggle = tempOldReplacePrefabAllToggle;
                        for (int i = 0; i < oldReplacePrefabToggles.Length; ++i)
                        {
                            oldReplacePrefabToggles[i] = oldReplacePrefabAllToggle;
                        }
                    }

                    scrollPos2 = EditorGUILayout.BeginScrollView(scrollPos2, GUILayout.Height(250));
                    for (int i = 0; i < oldPrefabList.Count; ++i)
                    {
                        EditorGUILayout.BeginHorizontal();
                        oldReplacePrefabToggles[i] = EditorGUILayout.Toggle(oldReplacePrefabToggles[i]);
                        if (!oldReplacePrefabToggles[i])
                            oldReplacePrefabAllToggle = false;

                        EditorGUILayout.ObjectField(oldPrefabList[i], typeof(GameObject), true);
                        EditorGUILayout.EndHorizontal();
                    }
                    EditorGUILayout.EndScrollView();
                    EditorGUILayout.Space();
                    replcaeRect.y += 280;
                }

                if (oldAtlasData != null
                    && newAtlasData != null
                    && !string.IsNullOrEmpty(oldSpriteName)
                    && !string.IsNullOrEmpty(newSpriteName)
                    && oldPrefabList != null
                    && oldPrefabList.Count > 0)
                {
                    if (GUILayout.Button("替换图片"))
                    {
                        List<GameObject> tempList = new List<GameObject>();
                        for (int i = 0; i < oldReplacePrefabToggles.Length; ++i)
                        {
                            if (oldReplacePrefabToggles[i])
                            {
                                tempList.Add(oldPrefabList[i]);
                            }
                        }
                        GUIAtlasDataManager.Instance.ReplaceImg(oldAtlasData, oldSpriteName, newAtlasData, newSpriteName, tempList);
                        oldPrefabList = null;
                    }
                }
            }

            #endregion
            #endregion

            #region 显示图片
            for (int i = 0; i < 2; ++i)
            {
                string tempSpriteName = i == 0 ? oldSpriteName : newSpriteName;
                GUIAtlasData tempAtlasData = i == 0 ? oldAtlasData : newAtlasData;
                if (tempAtlasData != null && !string.IsNullOrEmpty(tempSpriteName))
                {
                    float size = replcaeRect.width / 2 - 40f;
                    rect = new Rect(replcaeRect.width / 2 * i + 10f, replcaeRect.y + 80f, size, size);

                    Texture2D tex = tempAtlasData.MyAtlas.texture as Texture2D;
                    UISpriteData sprite = tempAtlasData.MyAtlas.GetSprite(tempSpriteName);
                    if (sprite == null) continue;

                    if (Event.current.type == EventType.Repaint)
                    {
                        // On top of the button we have a checkboard grid
                        NGUIEditorTools.DrawTiledTexture(rect, NGUIEditorTools.backdropTexture);
                        Rect uv = new Rect(sprite.x, sprite.y, sprite.width, sprite.height);
                        uv = NGUIMath.ConvertToTexCoords(uv, tex.width, tex.height);

                        // Calculate the texture's scale that's needed to display the sprite in the clipped area
                        float scaleX = rect.width / uv.width;
                        float scaleY = rect.height / uv.height;

                        // Stretch the sprite so that it will appear proper
                        float aspect = (scaleY / scaleX) / ((float)tex.height / tex.width);
                        Rect clipRect = rect;

                        if (aspect != 1f)
                        {
                            if (aspect < 1f)
                            {
                                // The sprite is taller than it is wider
                                float padding = size * (1f - aspect) * 0.5f;
                                clipRect.xMin += padding;
                                clipRect.xMax -= padding;
                            }
                            else
                            {
                                // The sprite is wider than it is taller
                                float padding = size * (1f - 1f / aspect) * 0.5f;
                                clipRect.yMin += padding;
                                clipRect.yMax -= padding;
                            }
                        }

                        GUI.DrawTextureWithTexCoords(clipRect, tex, uv);

                        // Draw the selection
                        if (NGUISettings.selectedSprite == sprite.name)
                        {
                            NGUIEditorTools.DrawOutline(rect, new Color(0.4f, 1f, 0f, 1f));
                        }
                    }
                }
            }
            #endregion
        } while (false);
        EditorGUILayout.EndScrollView();
    }

    /// <summary>
    /// 替换图片 将使用原图集的Sprite的图片改成新图集的
    /// </summary>
    /// <param name="oldAtlas">原图集</param>
    /// <param name="newAtlas">新图集</param>
    /// <param name="oldImg">原图集图片的名字</param>
    /// <param name="newImg">新图集图片的名字</param>
    private void ReplaceSpriteImg(UIAtlas oldAtlas, UIAtlas newAtlas, string oldImg, string newImg)
    {
        //搜索原图集
        SpriteSelector comp = ScriptableWizard.DisplayWizard<SpriteSelector>("Select a Sprite");
    }
}
