/**
 * @Author JEFF
 *
 *
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MahongCardShadow : MonoBehaviour
{

    enum MahongShadowType
    {
        None,
        FrontShadow,
        BackShadow,
        BelowShadow,
    }

    public GameObject ShadowObj;
    public GameObject MahongCardObj;
    public GameObject FrontShadow;
    public GameObject BackShadow;
    public GameObject BelowShadow;
    /// <summary>
    /// 设置阴影
    /// </summary>
    /// <param name="type">Type.</param>
	private MahongShadowType setShadow()
    {
        var z = (this.transform.rotation.eulerAngles.z + 360) % 360;
        if (z > 268 && z < 272)
        {
            //正立
            return MahongShadowType.BelowShadow;
        }

        var x = (this.transform.rotation.eulerAngles.x + 360) % 360;

        if (x > 88 && x < 92)
        {
            //背面
            return MahongShadowType.FrontShadow;
        }
        else if (x > 268 && x < 272)
        {
            //正面
            return MahongShadowType.BackShadow;
        }
        return MahongShadowType.None;
    }
    private MahongShadowType GetMahongCardTypeByAxes()
    {
        Vector3 upDir = MahongCardObj.transform.TransformDirection(Vector3.up);
        if (upDir.y >= 0.85f)
        {
            return MahongShadowType.BelowShadow;
        }
        upDir = MahongCardObj.transform.TransformDirection(Vector3.forward);
        if (upDir.y > 0.85f)
        {
            return MahongShadowType.BelowShadow;
        }
        upDir = MahongCardObj.transform.TransformDirection(Vector3.up);
        if (upDir.y >= 0.85f)
        {
            return MahongShadowType.BackShadow;
        }
        return MahongShadowType.None;
    }

    public void ShowShadow()
    {
        this.HiddenShadow();
        // switch(GetMahongCardTypeByAxes())
        switch (setShadow())
        {
            default: break;
            case MahongShadowType.FrontShadow:
                EnableShadow(FrontShadow, true);
                break;
            case MahongShadowType.BackShadow:
                EnableShadow(BackShadow, true);
                break;
            case MahongShadowType.BelowShadow:
                EnableShadow(BelowShadow, true);
                break;
        }
    }

    public void HiddenShadow()
    {

        EnableShadow(FrontShadow, false);
        EnableShadow(BackShadow, false);
        EnableShadow(BelowShadow, false);
    }


    private void EnableShadow(GameObject shadowObj, bool enable)
    {
        if (shadowObj != null && shadowObj.activeSelf != enable)
        {
            shadowObj.SetActive(enable);
        }
    }
    #region LifiCycle
    public MahongCardShadow()
    {
#if __DEBUG_LIFE_CYCLE
#endif
    }

    // Use this for per initialization
    void Awake()
    {
        EnableShadow(FrontShadow, false);
        EnableShadow(BackShadow, false);
        EnableShadow(BelowShadow, false);
    }

    // Update is called once per frame
    /*void Update()
    {
     //   ShowShadow();
    }

    void OnDestroy()
    {
#if __DEBUG_LIFE_CYCLE
#endif
    }*/


    #endregion //LifiCycle

}
