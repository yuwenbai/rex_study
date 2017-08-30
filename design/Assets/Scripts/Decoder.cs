using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Decoder : phone
{

    private phone _phone;
	// Use this for initialization
	void Start () {
		
	}
    public Decoder(phone p)
    {
        this._phone = p;
    }
	
	// Update is called once per frame
	void Update () {
		
	}
    public override void print()
    {
        Debug.Log("rextest decoder print");
        if (this._phone)
        {
            this._phone.print();
        }
    }
}
