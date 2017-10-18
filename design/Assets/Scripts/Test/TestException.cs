using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TestException : MonoBehaviour {

    public Text label;
    public GameObject AnimObj;
    void Awake()
    {
        Application.logMessageReceived += HandleException;
        DontDestroyOnLoad(gameObject);
    }

    void HandleException(string condition, string stackTrace, LogType type)
    {
        if (type == LogType.Exception)
        {
            //handle here
            Debug.Log("rextest log exception " + stackTrace);
        }
    }
    // Use this for initialization
    void Start () {
        
	}

    // Update is called once per frame
    void OnGUI()
    {
        if (GUI.Button(new Rect(50, 300, 128, 64), "点我就可以"))
        {
            ////GameObject.Find("aaa").SetActive(true);
            //var player = ResourceLoader.Instance.Load<GameObject>("Mesh/Cube");
            //for (int i = 0; i < 10; ++i)
            //{
            //    int _a = UnityEngine.Random.Range(0, 4);
            //    int _b = UnityEngine.Random.Range(0, 4);
            //    var obj = Instantiate(player);
            //    obj.GetComponent<Transform>().localScale = new Vector3(_a, _b, 1);
            //    obj.GetComponent<MeshRenderer>().material.color = new Color(1, _a, 1);
            //    Debug.Log("rextest add cobe " + i);
            //    nCubeNum += 1;
            //}
            //label.text = "" + nCubeNum;

            if (AnimObj)
            {
                AnimObj.GetComponent<TestAnimator>().PlayAnim();
            }

        }
    }
	void Update () {
    }
}
