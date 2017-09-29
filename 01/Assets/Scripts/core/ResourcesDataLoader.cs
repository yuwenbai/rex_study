using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ResourcesDataLoader{
    public const string UIPrefabDir = "UIPrefab/";

    /// <summary>
    /// 通用异步加载
    /// </summary>
    public static void AsyncLoad<T>(MonoBehaviour mono, string path, Action<T, object[]> callBack, params object[] parg) where T : UnityEngine.Object
    {
        mono.StartCoroutine(LoadResources<T>(path, callBack, parg));
    }
    /// <summary>
    /// 通用同步加载
    /// </summary>
    public static T Load<T>(string path) where T : UnityEngine.Object
    {
        return Resources.Load<T>(path);
    }

    /// <summary>
    /// UI加载
    /// </summary>
    public static void AsyncLoadUIPrefab(MonoBehaviour mono, string path, Action<GameObject, object[]> callBack, params object[] parg)
    {
        AsyncLoad<GameObject>(mono, UIPrefabDir + path, callBack, parg);
    }
    /// <summary>
    /// UI同步加载
    /// </summary>
    public static T LoadUI<T>(string path) where T : UnityEngine.Object
    {
        return Load<T>(UIPrefabDir + path);
    }

    public static void LoadWWW(MonoBehaviour mono ,string url)
    {

    }
    static IEnumerator LoadWWW_URL<T>(string url, Action<object> func)
    {
        WWW www = new WWW(url);
        yield return www;
       
        switch (typeof(T).ToString())
        {
            case "byte":
                func(www.bytes);
                break;
            case "Bundle":
                func(www.assetBundle);
                break;
            case "Texture":
                func(www.texture);
                break;
            case "string":
                func(www.text);
                break;
        }
    }



    static IEnumerator LoadResources<T>(string path, Action<T, object[]> callBack, object[] parg) where T : UnityEngine.Object
    {
        ResourceRequest request = Resources.LoadAsync<T>( path);
        yield return request;
        if (callBack != null)
            callBack(request.asset as T, parg);
    }
}
