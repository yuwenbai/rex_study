using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Car : MonoBehaviour {


    public int wheelCount = 0;
    public int WheelCount
    {
        set {
            wheelCount = value;
            Debug.Log("rextest --------- " + wheelCount);
        }
    }
    private void Awake()
    {
    }
    private void Init()
    {

    }
    //private void onBtnClick()
    //{
    //    UnityEngine.SceneManagement.SceneManager.LoadScene("scene");
    //}
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
