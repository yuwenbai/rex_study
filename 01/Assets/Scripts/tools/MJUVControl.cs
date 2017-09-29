using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using projectQ;

public class MJUVControl : MonoBehaviour
{

    public enum MjShowType
    {
        Normal,
        HeihgtLight,
        LowLight,
    }

    public MeshFilter _mesh;

    public MeshRenderer _renderer = null;



    public int idx;
    public Vector2 offset;
    public Vector2 offset_px;
    //[SerializeField]
    Vector2 offset_py = new Vector2(0, 0.005f);


    Vector2[] uv = null;


    public MjShowType showType = MjShowType.Normal;

    public MahongCardShadow _shadow_control;


    void Awake()
    {
        //_mesh = transform.GetComponent<MeshFilter>();
        //_renderer = transform.GetComponent<MeshRenderer>();

        //StringBuilder sb = new StringBuilder();

        //for (int i = 0; i < uv.Length; i++)
        //{
        //    sb.Append(uv[i].x + "," + uv[i].y + "\n");
        //}

        //QLoger.ERROR(sb.ToString());
#if __DEBUG_MJ
		//this.idx =  UnityEngine.Random.Range (1, 43);

#endif

    }

    /*void OnEnable(){
		this.updateUV(this.idx);
	}

	void Update(){	
		this.updateUV(this.idx);
	}*/

    void updateUV(int myIdx)
    {
        if (uv == null)
        {
            uv = new Vector2[_mesh.mesh.uv.Length];
            Array.Copy(_mesh.mesh.uv, uv, _mesh.mesh.uv.Length);
        }

        this.idx = myIdx;
        if (this.idx > 42)
        {
            this.idx = this.idx % 42;
        }

        var n_uv = new Vector2[uv.Length];
        Array.Copy(uv, n_uv, n_uv.Length);

        for (int i = 0; i < n_uv.Length; i++)
        {
            if (n_uv[i].x > 0.8 && n_uv[i].y < 0.2)
            {

            }
            else
            {
                n_uv[i].x += offset.x * (this.idx % 8);
                n_uv[i].y += offset.y * (this.idx / 8);

                if (this.idx >= 39 && this.idx <= 42)
                {
                    if (this.idx == 42)
                    {
                        n_uv[i].x += offset_px.x;
                        n_uv[i].y += offset_px.y;
                    }
                    else
                    {
                        n_uv[i].x += offset_py.x;
                        n_uv[i].y += offset_py.y;
                    }

                }
            }
        }
        _mesh.mesh.uv = n_uv;

    }



    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    /*void Update()
    {
        //this.updateUV(idx);

		QLoger.ERROR(
			"->" + this.transform.rotation.eulerAngles.x + "," +
			this.transform.rotation.eulerAngles.y + "," +
			this.transform.rotation.eulerAngles.z  + ","
		);
    }*/

    public void IniCard(int cardID)
    {
        if (cardID > 0)
        {
            updateUV(cardID + (-1));
        }
    }


    public static bool isInit = false;
    public static Dictionary<MJUVControl, bool> _buffe_uvc = new Dictionary<MJUVControl, bool>();

    public static void ChangeShadowAction(object[] v)
    {
        var isStart = (bool)v[0];
        var uvc = v[1] as MJUVControl;
        if (!isInit)
        {
            if (!_buffe_uvc.ContainsKey(uvc))
            {
                _buffe_uvc.Add(uvc, isStart);
            }
            else
            {
                _buffe_uvc[uvc] = isStart;
            }
        }
        else
        {
            uvc.setShadow(!isStart);
        }
    }

    /// <summary>
    /// Sets the shadow.
    /// </summary>
    /// <param name="isOpen">If set to <c>true</c> is open.</param>



    public void setShadow(bool isOpen = false)
    {
        if (this._shadow_control != null)
        {
            if (isOpen)
            {
                this._shadow_control.ShowShadow();

            }
            else
            {
                this._shadow_control.HiddenShadow();

            }
        }
    }


    /// <summary>
    /// Sets the type of the color.
    /// </summary>
    /// <param name="type">Type.</param>
    public void setShowType(MjShowType type)
    {

        if (this.showType == type)
        {
            return;
        }
        var old_type = showType;
        this.showType = type;

        switch (this.showType)
        {
            default:
                break;
            case MjShowType.HeihgtLight:
                this._renderer.sharedMaterial = MahjongCardController.Instance.material_height;
                break;

            case MjShowType.LowLight:
                this._renderer.sharedMaterial = MahjongCardController.Instance.material_lowlight;
                break;

            case MjShowType.Normal:
                this._renderer.sharedMaterial = MahjongCardController.Instance.material_normal;
                break;
        }

        if (old_type == MjShowType.HeihgtLight &&
            this.showType != MjShowType.HeihgtLight)
        {

            MahjongCardController.Instance._hight_light_mj_count--;

        }
        else if (this.showType == MjShowType.HeihgtLight)
        {
            MahjongCardController.Instance._hight_light_mj_count++;
        }
    }


    /// <summary>
    /// 把麻将调整显示层设置为桌子层或者还原为其该有的层 Table=8 , Mahjong=9 , Shadow=10 , Trans_TableMahjong=11
    /// </summary>
    public void SetMjAndShadowLayer(bool toTableLayer = true)
    {
        if (toTableLayer)
        {
            NGUITools.SetLayer(this.gameObject, 11);
            NGUITools.SetLayer(this._shadow_control.ShadowObj, 11);
        }
        else
        {
            NGUITools.SetLayer(this.gameObject, 9);
            NGUITools.SetLayer(this._shadow_control.ShadowObj, 10);
        }

    }
}
