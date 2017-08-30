using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Accessories : Decoder {

    public Accessories(phone p) : base(p)
    {

    }
    public override void print()
    {
        base.print();
        addAccessories();
    }
    void addAccessories()
    {
        Debug.Log("rextest addAccessories");
    }
}
