using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class main : MonoBehaviour {

	// Use this for initialization
	void Start () {
        Debug.Log("rextest main android test");
        phone apple = new applephone();
        Decoder decoder = new Sticker(apple);
        decoder.print();
        Decoder accessories = new Accessories(apple);
        accessories.print();

        facade _facade = new facade();
        _facade.registerfacade("shuxue");


        Receiver receiver = new Receiver();
        Commond commond = new ConcreteCommand(receiver);
        Invoker i = new Invoker(commond);
        i.ExecuteCommond();


        TenXun tenXun = new TenXunGame("TenXun Game");
        tenXun.addObserver(new Subscriber("Learning Hard"));
        tenXun.addObserver(new Subscriber("Tom"));
        tenXun.update();

        StrategyPattern _StrategyPattern = new StrategyPattern();
        _StrategyPattern.init();

    }

    // Update is called once per frame
    void Update () {
		
	}
}
