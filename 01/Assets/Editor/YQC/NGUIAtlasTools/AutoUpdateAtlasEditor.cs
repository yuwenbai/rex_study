/****************************************************
 *  Copyright(C) 2015 智幻点击
 *	版权所有。
 *	作者:于秋辰
 *	创建时间：5/5/2016
 *	文件名：  AutoUpdateAtlas.cs
 *	文件功能描述：
 *  创建标识：yqc.5/5/2016
 *	创建描述：自动更新图集
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

//图集数据
public class GUIAtlasData
{
    //图集路径
    public string AtlasPath = null;
    //图集
    public UIAtlas MyAtlas = null;
    //图集图片名称
    public List<string> ImgNames = null;
    //使用该图集的Prefab
    public List<GameObject> PrefabList = null;
    //图片与Prefab的对应
    public Dictionary<string, List<GameObject>> ImgPrefabMap = null;

    //刷新PrefabList
    public void RefreshPrefabList(List<KeyValuePair<string,GameObject>> AllPrefabList)
    {
        PrefabList = new List<GameObject>();
        for (int i = 0; i < AllPrefabList.Count; ++i)
        {
            string[] rely = AssetDatabase.GetDependencies(new string[] { AllPrefabList[i].Key });
            for (int j = 0; j < rely.Length; ++j)
            {
                if (rely[j] == AtlasPath)
                {
                    PrefabList.Add(AllPrefabList[i].Value);
                }
            }
        }
    }

    //刷新图片与Prefab的对应
    public void RefreshImgPrefabMap()
    {
        BetterList<string> imgList = MyAtlas.GetListOfSprites();
        ImgPrefabMap = new Dictionary<string, List<GameObject>>();
        //将图片Name放入Map的Key里
        for (int i = 0; i < imgList.size; ++i)
        {
            ImgPrefabMap.Add(imgList[i], new List<GameObject>());
        }

        //放入Prefab
        if (PrefabList == null) return;
        //循环Prefab
        for (int i = 0; i < PrefabList.Count; ++i)
        {
            //Object[] aaa = EditorUtility.CollectDependencies(new Object[] { PrefabList[i] });

            //取得Prefab里的所有UISprite
            UISprite[] spr = PrefabList[i].transform.GetComponentsInChildren<UISprite>(true);
            if (spr == null || spr.Length == 0) continue;

            //循环UISprite 
            for (int j = 0; j < spr.Length; ++j)
            {
                //检查图集相同
                if (spr[j].atlas == MyAtlas)
                {
                    //查找图片名字记录
                    if (ImgPrefabMap.ContainsKey(spr[j].spriteName))
                    {
                        if (ImgPrefabMap[spr[j].spriteName].IndexOf(PrefabList[i]) < 0)
                        {
                            ImgPrefabMap[spr[j].spriteName].Add(PrefabList[i]);
                        }
                    }
                    else
                    {
                        if (!ImgPrefabMap.ContainsKey(spr[j].spriteName + "_没找到"))
                        {
                            ImgPrefabMap.Add(spr[j].spriteName + "_没找到", new List<GameObject>());
                        }
                        ImgPrefabMap[spr[j].spriteName + "_没找到"].Add(PrefabList[i]);
                    }
                }
            }
        }
    }
}
//管理图集数据
public class GUIAtlasDataManager{
    private static GUIAtlasDataManager _instance;
    public static GUIAtlasDataManager Instance
    {
        get {
            if (_instance == null)
            {
                _instance = new GUIAtlasDataManager();
            }
            return _instance; }
    }

    public string AtlasPath = "Assets/Resources/Prefab";
    public string PrefabPath = "Assets/Resources/Prefab";
    //图集List
    public List<GUIAtlasData> AllAtlasList = new List<GUIAtlasData>();
    //所有的Prefab
    public List<KeyValuePair<string, GameObject>> AllPrefabList = new List<KeyValuePair<string, GameObject>>();


    #region API
    //刷新数据
    public void Refresh()
    {
        RefreshAtlas();
        RefreshPrefab();
    }
    //取得路径
    public string[] GetAtlasPaths()
    {
        string[] paths = new string[AllAtlasList.Count];
        for (int i = 0; i < AllAtlasList.Count; ++i)
        {
            paths[i] = AllAtlasList[i].AtlasPath;
        }
        return paths;
    }

    //根据名字获得数据
    public GUIAtlasData GetAtlasDataByName(string atlasName)
    {
        return AllAtlasList.Find(delegate(GUIAtlasData data)
            {
                return data.MyAtlas.name == atlasName;
            });  
    }

    //根据路径获得数据
    public GUIAtlasData GetAtlasDataByPath(string path)
    {
        return AllAtlasList.Find(delegate(GUIAtlasData data)
        {
            return data.AtlasPath == path;
        });
    }

    //取得使用图集的Prefab
    public List<GameObject> GetAtlasPrefabList(GUIAtlasData atlasData)
    {
        if (atlasData == null)
        {
            return null;
        }
        if (atlasData.PrefabList == null)
        {
            atlasData.RefreshPrefabList(AllPrefabList);
        }
        return atlasData.PrefabList;
    }

    //取得AtlasImgPrefabMap
    public Dictionary<string,List<GameObject>> GetAtlasImgPrefabMap(GUIAtlasData atlasData)
    {
        if (atlasData == null) return null;
        if (atlasData.ImgPrefabMap == null)
        {
            GetAtlasPrefabList(atlasData);
            atlasData.RefreshImgPrefabMap();
        }
        return atlasData.ImgPrefabMap;
    }

    //根据图片名称取得依赖的Prefab列表
    public List<GameObject> GetPrefabListByImgName(GUIAtlasData atlasData,string imgName)
    {
        if (atlasData != null && !string.IsNullOrEmpty(imgName))
        {
            Dictionary<string,List<GameObject>> tempMap = this.GetAtlasImgPrefabMap(atlasData);
            if(tempMap != null)
            {
                if(tempMap.ContainsKey(imgName))
                {
                    return tempMap[imgName];
                }
            }
        }
        return null;
    }

    public void ReplaceImg(GUIAtlasData oldAtlasData,string oldImgName,GUIAtlasData newAtlasData,string newImgName,List<GameObject> PrefabList)
    {
        for(int i=0; i<PrefabList.Count; ++i)
        {
            EditorUtility.SetDirty(PrefabList[i]);
            UISprite[] tempSpr = PrefabList[i].transform.GetComponentsInChildren<UISprite>(true);
            if(tempSpr != null)
            {
                for(int j=0; j<tempSpr.Length;++j)
                {
                    if(tempSpr[j].atlas == oldAtlasData.MyAtlas 
                        && tempSpr[j].spriteName == oldImgName)
                    {
                        tempSpr[j].atlas = newAtlasData.MyAtlas;
                        tempSpr[j].spriteName = newImgName;
                    }
                }
            }
        }
        oldAtlasData.PrefabList = null;
        oldAtlasData.ImgPrefabMap = null;
    }
    #endregion


    #region private
    //刷新图集
    private void RefreshAtlas()
    {
        AllAtlasList.Clear();
        //搜索图集 根据路径
        List<KeyValuePair<string, UIAtlas>> tempList = EditorHelpers.CollectAll<UIAtlas>(AtlasPath, ".prefab", true);
        for (int i = 0; i < tempList.Count; ++i)
        {
            GUIAtlasData data = new GUIAtlasData();
            data.AtlasPath = tempList[i].Key;
            data.MyAtlas = tempList[i].Value;
            AllAtlasList.Add(data);
        }
    }
    //刷新Prefab
    private void RefreshPrefab()
    {
        AllPrefabList.Clear();
        AllPrefabList = EditorHelpers.CollectAll<GameObject>(PrefabPath, ".prefab", true);
    }

    #endregion

}


public class AutoUpdateAtlasEditor : EditorWindow{

    Dictionary<string,Texture> NewTextureMap = null;
    List<string> CheckErrorNames = null;

    [MenuItem("Custom/Tools/NGUI图集工具", false, 1)]
    static void ShowEditor()
    {
        YQCEditorCommon.ShowWindow(YQCEditorCommon.YQCEditorWindowTypes.UpdateAtlas);
    }

    Vector2 scrollPos;
    bool showPrefab;
    bool showImg;
    bool showImgCheck;
    string NewImgPath = "Assets/_Img";
    int AtlasIndex = -1;

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

            #region 图集展示
            AtlasIndex = EditorGUILayout.Popup(AtlasIndex, atlasPaths);
            GUIAtlasData atlasData = null;
            if(AtlasIndex >= 0 && AtlasIndex < atlasPaths.Length)
                //数据
                atlasData = GUIAtlasDataManager.Instance.GetAtlasDataByPath(atlasPaths[AtlasIndex]);

            if (atlasData == null) break;
            #endregion 



            #region 展示依赖Prefab
            //数据
            List<GameObject> PrefabList = GUIAtlasDataManager.Instance.GetAtlasPrefabList(atlasData);
            if (PrefabList != null)
            {
                showPrefab = EditorGUILayout.Foldout(showPrefab, "显示依赖Prefab");
                if (showPrefab)
                {
                    for (int i = 0; i < PrefabList.Count; ++i)
                    {
                        EditorGUILayout.ObjectField(new GUIContent(PrefabList[i].name), PrefabList[i], typeof(GameObject));
                    }
                }
            }
            #endregion

            #region 图片使用情况
            //数据
            Dictionary<string,List<GameObject>> ImgPrefabMap = GUIAtlasDataManager.Instance.GetAtlasImgPrefabMap(atlasData);
            if (ImgPrefabMap != null)
            {
                showImg = EditorGUILayout.Foldout(showImg, "显示图片使用情况");
                if (showImg)
                {
                    foreach (var img in ImgPrefabMap)
                    {
                        EditorGUILayout.BeginHorizontal();
                        EditorGUILayout.TextField(img.Key);
                        EditorGUILayout.LabelField(img.Value.Count.ToString());
                        EditorGUILayout.EndHorizontal();
                    }

			        #region 删除未使用图片
			if (GUILayout.Button("删除未使用图片"))
			{
				List<string> removeImg = new List<string>();
				foreach(var img in ImgPrefabMap)
				{
					if(img.Value.Count == 0)
					{
						removeImg.Add(img.Key);
					}
				}

				List<UIAtlasMaker.SpriteEntry> sprites = new List<UIAtlasMaker.SpriteEntry>();
				UIAtlasMaker.ExtractSprites(atlasData.MyAtlas, sprites);
				
				for (int i = sprites.Count; i > 0; )
				{
					UIAtlasMaker.SpriteEntry ent = sprites[--i];
					if (removeImg.Contains(ent.name))
						sprites.RemoveAt(i);
				}
				UIAtlasMaker.UpdateAtlas(atlasData.MyAtlas, sprites);
				removeImg.Clear();

			}
			#endregion
                }
            }
            #endregion

            #region 新图片检查
            showImgCheck = EditorGUILayout.Foldout(showImgCheck, "新图片检查");
            if (showImgCheck)
            {
                //新图片路径
                EditorGUILayout.BeginHorizontal();
                NewImgPath = EditorGUILayout.TextField(NewImgPath);
                if(GUILayout.Button("检查图片"))
                {
                    CheckImg(NewImgPath);
                }
                EditorGUILayout.EndHorizontal();
                if(NewTextureMap != null) 
                {
                    //如果没有错误图片的话
                    if (CheckErrorNames == null || CheckErrorNames.Count == 0)
                    {
                        if (ImgPrefabMap != null)
                        {
                            foreach (var imgItem in NewTextureMap)
                            {
                                EditorGUILayout.BeginHorizontal();
                                EditorGUILayout.TextField(imgItem.Key);
                                //检查是添加还是更新 有哪些Prefab 关联
                                if (ImgPrefabMap.ContainsKey(imgItem.Key))
                                {
                                    EditorGUILayout.LabelField("更新");
                                    EditorGUILayout.EndHorizontal();
                                    if (ImgPrefabMap[imgItem.Key].Count > 0)
                                    {
                                        for (int i = 0; i < ImgPrefabMap[imgItem.Key].Count; ++i)
                                        {
                                            if(i % 3 == 0)
                                                EditorGUILayout.BeginHorizontal();

                                            EditorGUILayout.ObjectField(ImgPrefabMap[imgItem.Key][i], typeof(GameObject), true);

                                            if (i == ImgPrefabMap[imgItem.Key].Count - 1 || i % 3 == 2)
                                                EditorGUILayout.EndHorizontal();
                                        }
                                    }
                                }
                                else
                                {
                                    EditorGUILayout.LabelField("新增");
                                    EditorGUILayout.EndHorizontal();
                                }
                            }
                        }
                    } 
                    else
                    {
                        foreach (var name in CheckErrorNames)
                        {
                            EditorGUILayout.BeginHorizontal();
                            EditorGUILayout.TextField(name);
                            EditorGUILayout.LabelField("重复");
                            EditorGUILayout.EndHorizontal();
                        }
                    }
                }
            }
            #endregion

            #region 更新图集
            if (NewTextureMap != null && NewTextureMap.Count > 0 && (CheckErrorNames == null || CheckErrorNames.Count == 0))
            {
                if(GUILayout.Button("更新图集"))
                {
                    List<Texture> tempTextureList = new List<Texture>();
                    foreach (var newTexture in NewTextureMap)
                    {
                        tempTextureList.Add(newTexture.Value);
                    }
                    UpdateAtlas(atlasData.MyAtlas, tempTextureList);
                }
            }
            #endregion
        } while (false);
        EditorGUILayout.EndScrollView();
    }

    //检查图片 重名警告
    private void CheckImg(string imgPath)
    {
        CheckErrorNames = new List<string>();
        NewTextureMap = new Dictionary<string, Texture>();
        List<KeyValuePair<string, Texture>> temp = EditorHelpers.CollectAll<Texture>(imgPath,".png",true);

        for (int i = 0; i < temp.Count; ++i)
        {
            if (NewTextureMap.ContainsKey(temp[i].Value.name))
            {
                if (!CheckErrorNames.Contains(temp[i].Value.name))
                {
                    CheckErrorNames.Add(temp[i].Value.name);
                }
            }
            else
            {
                NewTextureMap.Add(temp[i].Value.name, temp[i].Value);
            }
        }
    }

    //更新图集
    private void UpdateAtlas(UIAtlas uiatls,List<Texture> textures)
    {
        List<UIAtlasMaker.SpriteEntry> sprites = UIAtlasMaker.CreateSprites(textures);

        if (sprites.Count > 0)
        {
            UIAtlasMaker.ExtractSprites(uiatls, sprites);
            UIAtlasMaker.UpdateAtlas(uiatls, sprites);
        }
    }
}
