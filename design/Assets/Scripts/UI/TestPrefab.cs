using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestPrefab : MonoBehaviour
{
    public GameObject aaa;
    private void OnEnable()
    {
        Debug.Log("rextest TestPrefab OnEnable!!! ");
    }
    private void Start()
    {
        Debug.Log("rextest TestPrefab Start!!! ");
        //for (int i = 0; i < 100000; ++i)
        //{
        //    var sprite = Instantiate(aaa);
        //    sprite.transform.parent = transform;
        //    var sharedMaterial = sprite.GetComponent<SpriteRenderer>().sharedMaterial;
        //}
    }
    // Update is called once per frame
    void Update()
    {
        //当用户按下手机的返回键或home键退出游戏
        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.Home))
        {
            Application.Quit();
        }
    }

    void OnGUI()
    {
        if (GUILayout.Button("OPEN WEBVIEW", GUILayout.Height(100)))
        {
            //注释1
            //AndroidJavaClass jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            //  	 	 AndroidJavaObject jo = jc.GetStatic<AndroidJavaObject>("currentActivity");
            //    	 jo.Call("StartWebView","");
            UnityEngine.SceneManagement.SceneManager.LoadSceneAsync("design");
        }

    }
}

