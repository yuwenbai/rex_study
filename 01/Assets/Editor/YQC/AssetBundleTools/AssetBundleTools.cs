/****************************************************
 *  Copyright(C) 2015 智幻点击
 *	版权所有。
 *	作者:于秋辰
 *	创建时间：5/12/2016
 *	文件名：  AssetBundleTools.cs
 *	文件功能描述：
 *  创建标识：yqc.5/12/2016
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

public class BundleVertexNode : VertexNode
{
    private Object _obj = null;
    public Object Obj {
        get {
            if (_obj == null)
            {
                _obj = (Object)AssetDatabase.LoadAssetAtPath(base.data.ToString(), typeof(Object));
            }
            return _obj;
        }
    }
    private GUIContent _content;
    public GUIContent content{
        get {
            if (_content == null && this.Obj != null)
            {
                _content = new GUIContent(this.Obj.name, AssetDatabase.GetCachedIcon(data.ToString()));
            }
            return _content;
        }
    }

    public bool isSelected = false;
}

//目录结构
public class NodeDirectory{
    //根目录 如果为空就是没有 
    public static NodeDirectory RootDir;
    public static int min;
    //文件名
    public string name;
    public GUIContent content;
    //折叠标签
    public bool showFoldout = false;
    public bool isSelected = false;
    //层级
    public int layer = 0;
    //子目录结构
    public Dictionary<string, NodeDirectory> subDirMap;
    public List<BundleVertexNode> nodeList;

    //构造函数
    public NodeDirectory(string name, int layer)
    {
        this.name = name;
        this.content = GetGUIContent(this.name);
        this.layer = layer;

        subDirMap = new Dictionary<string, NodeDirectory>();
        nodeList = new List<BundleVertexNode>();
    }
    private GUIContent GetGUIContent(string path)
    {
        Object asset = AssetDatabase.LoadAssetAtPath(path, typeof(Object));
        if (asset)
        {
            return new GUIContent(asset.name, AssetDatabase.GetCachedIcon(path));
        }
        return null;
    }
    public static void CreateDir(List<BundleVertexNode> nodeList)
    {
        //for (int i = 0; i < nodeList.Count; ++i )
        //{
        //    Debug.Log(nodeList[i].data.ToString());
        //}
        RootDir = new NodeDirectory("Assets", 0);

        for(int i=0; i<nodeList.Count; ++i)
        {
            //if(nodeList[i].inEdgeList.Count < 2 && nodeList[i].inEdgeList.Count != 0) continue;
            if (nodeList[i].inEdgeList.Count == 1) continue;

            NodeDirectory tempDir = RootDir;
            //路径dirIndex
            string[] dirs = nodeList[i].data.ToString().Split('/');
            string fileDir;
            for(int j=1; j<dirs.Length-1; ++j)
            {
                fileDir = string.Join("/", dirs, 0, j+1);

                if (!tempDir.subDirMap.ContainsKey(fileDir))
                {
                    tempDir.subDirMap.Add(fileDir, new NodeDirectory(fileDir, j));
                    if (dirs.Length < min) min = dirs.Length;
                }
                tempDir = tempDir.subDirMap[fileDir];
            }
            tempDir.nodeList.Add(nodeList[i]);
        }
    }
}

public class AssetBundleTools : EditorWindow {
    FileGraphSearch<BundleVertexNode> fgs = new FileGraphSearch<BundleVertexNode>();
    [MenuItem("Custom/Tools/AssetBundle/文件搜索", false, 1)]
    static void ShowEditor()
    {
         EditorWindow.GetWindow(typeof(AssetBundleTools), false, "文件搜索", true);
    }
    Vector2 scrollVector;
    bool showPrefab = false;
    bool isAllSelect = false;
    void OnGUI()
    {
        GUI.enabled = true;
        EditorGUIUtility.SetIconSize(Vector2.one * 16);
        if(GUILayout.Button("刷新"))
        {
            SearchPath();
            NodeDirectory.RootDir = null;
        }
        if(fgs.adj != null)
        {
            scrollVector = GUILayout.BeginScrollView(scrollVector);
            bool tempIsAllSelect = GUILayout.Toggle(isAllSelect, "全选");
            if (tempIsAllSelect != isAllSelect)
            {
                isAllSelect = tempIsAllSelect;
                AllSelect(isAllSelect);
            }

            if (NodeDirectory.RootDir == null)
            {
                NodeDirectory.CreateDir(fgs.adj.adjList);
            }
            //绘制
            DrawData(NodeDirectory.RootDir);
            GUILayout.EndScrollView();

            if (GUILayout.Button("修改"))
            {
                
            }
        }
    }

    //绘制目录
    void DrawData(NodeDirectory data)
    {
        if (data.name != null)
        {
            EditorGUI.indentLevel = data.layer;
            DrawGUIDir(data);
        }
        if(data.showFoldout)
        {
            foreach (var item in data.subDirMap)
            {
                NodeDirectory child = item.Value;
                if (child.name != null)
                {
                    EditorGUI.indentLevel = child.layer;
                    DrawData(child);
                }
            }
            for (int i=0; i < data.nodeList.Count; ++i)
            {
                DrawGUIFile(data.nodeList[i]);
            }
        }
    }

    //绘制文件夹
    void DrawGUIDir(NodeDirectory data)
    {
        GUIStyle style = "Label";
        Rect rt = GUILayoutUtility.GetRect(data.content, style);
        Rect rt2 = new Rect(rt);
        
        rt.x += (10 * EditorGUI.indentLevel) + 10;
        rt2.x += (25 * EditorGUI.indentLevel) + 25;

        bool tempSelected = GUI.Toggle(rt2, data.isSelected, data.content);
        if(tempSelected != data.isSelected)
        {
            data.isSelected = tempSelected;
            SelectDirChild(data.isSelected, data);
        }
        data.showFoldout = EditorGUI.Foldout(rt, data.showFoldout,"");
    }

    //绘制文件
    void DrawGUIFile(BundleVertexNode data)
    {
        GUIStyle style = "Label";
        Rect rt = GUILayoutUtility.GetRect(data.content, style);
        rt.x += 25 * (EditorGUI.indentLevel+1)+25;
        data.isSelected = GUI.Toggle(rt, data.isSelected, data.content);
        
    }
    void SearchPath() 
    {
        fgs = new FileGraphSearch<BundleVertexNode>();
        fgs.Search("Assets/Resources");
    }

    //选中子项所有
    void SelectDirChild(bool selected,NodeDirectory dir)
    {
        dir.isSelected = selected;
        foreach(var item in dir.subDirMap)
        {
            item.Value.isSelected = selected;
            SelectDirChild(selected, item.Value);
        }
        for(int i=0; i<dir.nodeList.Count; ++i)
        {
            dir.nodeList[i].isSelected = selected;
        }
    }
    void AllSelect(bool isSelect)
    {
        for (int i = 0; i < fgs.adj.adjList.Count; ++i)
        {
            BundleVertexNode temp = fgs.adj.adjList[i];
            temp.isSelected = isSelect;
        }
    }
    void UnAllSelect()
    {
        isAllSelect = false;
    }
}
