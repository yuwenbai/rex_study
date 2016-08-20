using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class TestPool : MonoBehaviour {
    public Transform Root;
    public GameObject prefab;
    public float loopcount;
    public float prefabcount;
    List<GameObject> objects;

    void TestCaseWithoutPool()
    {
        for (int j = 0; j < loopcount; ++j)
        {
            for (int i = 0; i < prefabcount; ++i)
            {
                GameObject go = Instantiate(prefab);
                go.transform.SetParent(Root);
                objects.Add(go);
            }

            for (int i = 0; i < prefabcount; ++i)
            {
                GameObject.Destroy(objects[i]);
            }
            objects.Clear();
        }
            
    }
	// Use this for initialization
	void Start () {
        objects = new List<GameObject>();
        System.DateTime starttime = System.DateTime.Now;
        TestCaseWithoutPool();
        System.DateTime endtime = System.DateTime.Now;
        string  totalms = (endtime - starttime).Milliseconds.ToString();
        Debug.Log("test case result pool  take "+totalms+" ms !");


    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
