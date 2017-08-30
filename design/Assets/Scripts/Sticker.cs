using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sticker : Decoder {

    public Sticker(phone p):base(p)
    {

    }
    public override void print()
    {
        base.print();
        addSticker();
    }
    public void addSticker()
    {
        Debug.Log("rextest sticker addSticker");
    }
}
