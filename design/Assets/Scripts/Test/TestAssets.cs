using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// 子弹类型枚举
public enum BulletType
{
    DirectAttack = 0,  // 直接攻击
    Phony,             // 假子弹
    Real,              // 真子弹
    Track,             // 追踪子弹
}
[Serializable]
public class TestAssets : ScriptableObject {

    // 子弹类型
    public BulletType bulletType = BulletType.DirectAttack;

    public int speed  = 10;

    public int damage = 5;
}
