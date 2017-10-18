using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
public class TestBundle : MonoBehaviour {

	// Use this for initialization
	void Start () {
        //AssetBundle.LoadFromFile();
        //AssetBundle.LoadFromMemoryAsync();
        Debug.Log("StreamingAssetsPath is " + Application.streamingAssetsPath);
        var path = Path.Combine(Application.streamingAssetsPath, "test1.test1_child");
        var myBundle = AssetBundle.LoadFromFile(path);
        if (myBundle == null)
        {
            Debug.Log("Failed To Load AssetBundle!!!");
            return;
        }
        var prefab = myBundle.LoadAsset("Hand_Man.prefab") as GameObject;
        Instantiate(prefab);

        myBundle.Unload(false);

        StartCoroutine(GetResource());
    }
    IEnumerator GetResource()
    {
        var tempstring = "http://192.168.221.48/ResPub/test/test2.test2_child";
        WWW www = WWW.LoadFromCacheOrDownload(tempstring, -1);
        yield return www;

        if (!string.IsNullOrEmpty(www.error))
        {
            Debug.Log("rextest " + www.error);
            yield return null;
        }
        var prefab1 = www.assetBundle.LoadAsset("Hand_WoMan.prefab") as GameObject;
        Instantiate(prefab1);
    }
	// Update is called once per frame
	void Update () {
		
	}
}
