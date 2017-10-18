using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestAnimator : MonoBehaviour {

    // Use this for initialization

    private Animator A_mation;
    void Start () {
        A_mation =  GetComponent<Animator>();
        A_mation.speed = 0.1f;
    }
	
	// Update is called once per frame
	void Update () {
		
	}
    public void PlayAnim()
    {
        A_mation.CrossFade("chapaiIdle",0);
    }

}
