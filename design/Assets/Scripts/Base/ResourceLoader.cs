using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

class ResourceLoader : SingletonTamplate<ResourceLoader>
{

    public void init()
    {

    }
    public T Load<T>(string path) where T : UnityEngine.Object
    {
        return Resources.Load<T>(path);
    }
    static IEnumerator LoadBundleWWW_URL<T>(string url, Action<object> func)
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
}