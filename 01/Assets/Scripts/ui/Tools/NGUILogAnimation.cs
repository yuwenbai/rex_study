/****************************************************
 *  Copyright(C) 2015 智幻点击
 *	版权所有。
 *	作者:CEPH
 *	创建时间：1/15/2016
 *	文件名：  NGUILogAnimation.cs
 *	文件功能描述：
 *  创建标识：yqc.1/15/2016
 *	创建描述：NGUI切换texture
 *
 *  修改标识：
 *  修改描述：
 *
 *
 *
 *****************************************************/
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class NGUILogAnimationData
{
    public Texture Bg;
    public float ToOpenTime;
    public float ToCloseTime;
}
public class NGUILogAnimation : MonoBehaviour {
    public delegate void logAnimationExit();
    //结束回调函数
    public logAnimationExit ExitAction;
    //切换的List
    public List<NGUILogAnimationData> BgList = new List<NGUILogAnimationData>();

    [HideInInspector]
    public UITexture texture;

   
    private int currIndex = 0;
    private bool isShow = false;


    void Awake() {
        init();
    }
	void Start () {
        
	}

    private void init()
    {
        //如果没有手动赋值的话
        if (BgList.Count == 0)
        {
            projectQ.QLoger.ERROR("切换页面map为空");
        }
        texture = GetComponent<UITexture>();

        //for(int i=0; i<BgListTime.Count ; ++i)
        //{
        //    string[] arr = BgListTime[i].Split(',');

        //    BgTime.Add(new float[] { float.Parse(arr[0]), float.Parse(arr[1]) });
        //}
    }


    public void Show()
    {
        //切换贴图
        texture.mainTexture = BgList[currIndex].Bg;
        isShow = true;
        if(BgList[currIndex].ToOpenTime == 0)
        {
            OnTweenUpdate(1);
            OnTweenEnd();
        }
        else
        {
            OnTweenUpdate(0);
            UpdateValue(0, 1, BgList[currIndex].ToOpenTime);
        }
    }
    public void Hide()
    {
        isShow = false;

        if (BgList[currIndex].ToCloseTime == 0)
        {
            OnTweenUpdate(0);
            OnTweenEnd();
        }
        else
        {
            UpdateValue(1, 0.2f, BgList[currIndex].ToCloseTime);
        }
    }

    //添加一个定时器组件
    private void UpdateValue(float start,float end,float time)
    {
        Hashtable hash = new Hashtable();
        hash.Add("from", start);
        hash.Add("to", end);
        hash.Add("time", time);
        hash.Add("delay", 0f);
        hash.Add("onupdate", "OnTweenUpdate");
        hash.Add("onupdatetarget", this.gameObject);

        hash.Add("oncomplete", "OnTweenEnd");
        hash.Add("oncompletetarget", this.gameObject);

        iTween.ValueTo(texture.gameObject, hash);
    }

    void OnTweenUpdate(float value)
    {
        texture.alpha = value;
    }

    void OnTweenEnd()
    {
        //显示结束
        if (isShow)
        {
            Hide(); 
        }
        else //hide结束
        { 
            if (++currIndex >= BgList.Count)
            {
                if (ExitAction != null)
                {
                    ExitAction();
                }
                return;
            }
            else
            {
                Show();
            }
        }
    }
}
