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
        obj = GameObject.Instantiate(res) as GameObject;
    }
    public void ChangeObj(string name)
    {
        partName = name;

        if (obj)
        {
            GameObject.DestroyImmediate(obj);
        }
        Init();
    }
    private ItemEnum.ItemEnumPart partType;
    public ItemEnum.ItemEnumPart PartType
    {
        get
        {
            return partType;    
        }
    }
    private string partName;
    public string PartName
    {
        get
        {
            return partName;
        }
    }

    private GameObject obj;
    public GameObject Obj
    {
        get
        {
            return obj;
        }
        //set
        //{
        //    obj = value;
        //}
    }
}
