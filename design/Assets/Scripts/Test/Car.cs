using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Car : MonoBehaviour {


    public int wheelCount = 0;
    public int WheelCount
    {
        set {
            wheelCount = value;
            Debug.Log("rextest --------- " + wheelCount);
        }
    }
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
