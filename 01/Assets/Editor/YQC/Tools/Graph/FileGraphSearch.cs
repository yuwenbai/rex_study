/****************************************************
 *  Copyright(C) 2015 智幻点击
 *	版权所有。
 *	作者:于秋辰
 *	创建时间：5/11/2016
 *	文件名：  FileGraphSearch.cs
 *	文件功能描述：
 *  创建标识：yqc.5/11/2016
 *	创建描述：文件搜索
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
using System.IO;

public class FileGraphSearch<T> where T:VertexNode, new(){
    public GraphAdjList<T> adj;

    //搜索目录下所有文件
    public void Search(string path)
    {
        //获得所有文件路径
        List<string> fileList = EditorHelpers.CollectAll(path, ".prefab", true);
        List<T> tempList = new List<T>(fileList.Count);

        for(int i=0; i<fileList.Count; ++i)
        {
            T node = new T();
            node.data = fileList[i];
            tempList.Add(node);
        }
        string[] strArray = new string[1];
        for (int i = 0; i < tempList.Count; ++i)
        {
            strArray[0] = (string)tempList[i].data;
            //得到依赖的所有资源
            string[] DepndPaths = AssetDatabase.GetDependencies(strArray);
            for (int j = 0; j < DepndPaths.Length; ++j)
            {
                if (!DepndPaths[j].Contains(".prefab")) continue;
                if (DepndPaths[j].Equals(tempList[i].data.ToString())) continue;
                T find = tempList.Find(delegate(T data)
                {
                    return (string)data.data == DepndPaths[j];
                });
                //没找到的话就创建一个
                if (find == null)
                {
                    find = new T();
                    find.data = DepndPaths[j];
                    tempList.Add(find);
                }
                tempList[i].outEdgeList.Add(new EdgeNode(find));
            }
        }

        TrimEdgeList(tempList);
    }

    //整理边列表
    public void TrimEdgeList(List<T> list)
    {
        for (int i = 0; i < list.Count; ++i)
        {
            TrimEdgeOut(list[i]);
        }

        adj = new GraphAdjList<T>();
        adj.adjList = list;
        //根据出度整理入度
        for (int i = 0; i < list.Count; ++i )
        {
            foreach (var outEdge in list[i].outEdgeList)
            {
                outEdge.node.inEdgeList.Add(new EdgeNode(list[i]));
            }
        }
    }

    //整理出度
    public void TrimEdgeOut(T data)
    {
        var tempList = new List<EdgeNode>(data.outEdgeList);
        for (int i = 0; i < tempList.Count; ++i)
        {
            for (int j = 0; j < tempList[i].node.outEdgeList.Count; ++j)
            {
                if (data.outEdgeList.Remove(tempList[i].node.outEdgeList[j]))
                {
                    //Debug.Log("删除 " + data.data.ToString() + "  " + tempList[i].node.outEdgeList[j].node.data.ToString());
                }
            }
        }
    }
}
