/****************************************************
 *  Copyright(C) 2015 智幻点击
 *	版权所有。
 *	作者:于秋辰
 *	创建时间：5/5/2016
 *	文件名：  EditorHelpers.cs
 *	文件功能描述：
 *  创建标识：yqc.5/5/2016
 *	创建描述：
 *
 *  修改标识：
 *  修改描述：
 *
 *
 *
 *****************************************************/
using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
public class EditorHelpers
{
    public static List<KeyValuePair<string,T>> CollectAll<T>(string path, string suffix, bool isChild) where T : UnityEngine.Object
    {
        List<KeyValuePair<string, T>> l = new List<KeyValuePair<string, T>>();
        string[] files = Directory.GetFiles(path);
        foreach (string file in files)
        {
            if (file.Contains(".meta") || (!string.IsNullOrEmpty(suffix) && !file.Contains(suffix))) continue;
            T asset = (T)AssetDatabase.LoadAssetAtPath(file, typeof(T));
            if (asset != null)
                l.Add(new KeyValuePair<string,T>(file.Replace("\\","/"),asset));
        }
        if(isChild)
        {
            string[] dirs = Directory.GetDirectories(path);
            foreach (string dir in dirs)
            {
                l.AddRange(CollectAll<T>(dir, suffix, isChild));
            }
        }
        return l;
    }

    public static List<KeyValuePair<string, T>> CollectAll2<T>(string path, string suffix, bool isChild) where T : UnityEngine.MonoBehaviour
    {
        List<KeyValuePair<string, T>> l = new List<KeyValuePair<string, T>>();
        string[] files = Directory.GetFiles(path);
        foreach (string file in files)
        {
            if (file.Contains(".meta") || (!string.IsNullOrEmpty(suffix) && !file.Contains(suffix))) continue;
            GameObject asset = (GameObject)AssetDatabase.LoadAssetAtPath(file, typeof(GameObject));

            if(asset != null)
            {
                T[] list = asset.transform.GetComponentsInChildren<T>(true);

                if (list != null)
                {
                    for (int i = 0; i < list.Length; i++)
                    {
                        l.Add(new KeyValuePair<string, T>(file.Replace("\\", "/"), list[i]));
                    }
                }
            }
        }
        if (isChild)
        {
            string[] dirs = Directory.GetDirectories(path);
            foreach (string dir in dirs)
            {
                l.AddRange(CollectAll2<T>(dir, suffix, isChild));
            }
        }
        return l;
    }

    public static List<string> CollectAll(string path, string suffix, bool isChild)
    {
        List<string> l = new List<string>();
        string[] files = Directory.GetFiles(path);
        foreach (string file in files)
        {
            if (file.Contains(".meta") || (!string.IsNullOrEmpty(suffix) && !file.Contains(suffix))) continue;
            l.Add(file.Replace("\\", "/"));
        }
        if (isChild)
        {
            string[] dirs = Directory.GetDirectories(path);
            foreach (string dir in dirs)
            {
                l.AddRange(CollectAll(dir, suffix, isChild));
            }
        }
        return l;
    }
}
