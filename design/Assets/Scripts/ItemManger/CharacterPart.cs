using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

class CharacterPart
{
    public CharacterPart(ItemEnum.ItemEnumPart type , string name)
    {
        partType = type;
        partName = name;
        //初始化obj
        //初始化mesh
        //初始化skeleton
        Init();
    }
    private void Init()
    {
        var res = Resources.Load("Model/CharacterPrefab/" + partName);
        Obj = GameObject.Instantiate(res) as GameObject;
    }
    public void ChangeObj(string name)
    {
        partName = name;

        if (Obj)
        {
            GameObject.DestroyImmediate(Obj);
        }
        Init();
    }
    public ItemEnum.ItemEnumPart partType;
    public ItemEnum.ItemEnumPart PartType
    {
        get;
        set;
    }
    private string partName;
    public string PartName
    {
        get
        {
            return partName;
        }
    }

    public GameObject Obj
    {
        get;
        set;
        //set
        //{
        //    obj = value;
        //}
    }
}
