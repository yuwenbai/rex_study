/**
 * 作者：周腾
 * 作用：
 * 日期：
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivityNameItem : MonoBehaviour {

    public GameObject selectObj;
    public UILabel selectActivityNameLab;
    public UILabel activityNameLab;
    public GameObject redPointObj;
    public delegate void OnActivityNameClick(int index);
    public OnActivityNameClick dele_activityNameClick;
    [HideInInspector]
    public int index;
    #region Self Function
    /// <summary>
    /// 初始化活动名字
    /// </summary>
    /// <param name="activityName"></param>
    public void InitActivityName(int index,string activityName)
    {
        if (index == 0)
            ShowSelect();
        else
            HideSelect();

        this.index = index;
        activityNameLab.text = activityName;
        selectActivityNameLab.text = activityName;
    }
    /// <summary>
    /// 显示红点
    /// </summary>
    public void ShowRedPoint()
    {
        NGUITools.SetActive(redPointObj, true);
    }
    /// <summary>
    /// 隐藏红点
    /// </summary>
    public void HideRedPoint()
    {
        NGUITools.SetActive(redPointObj, false);
    }
    /// <summary>
    /// 显示选中
    /// </summary>
    public void ShowSelect()
    {
        NGUITools.SetActive(selectObj, true);
        NGUITools.SetActive(selectActivityNameLab.gameObject, true);
        NGUITools.SetActive(activityNameLab.gameObject, false);
    }
    /// <summary>
    /// 隐藏选中
    /// </summary>
    public void HideSelect()
    {
        NGUITools.SetActive(selectObj, false);

        NGUITools.SetActive(selectActivityNameLab.gameObject, false);
        NGUITools.SetActive(activityNameLab.gameObject, true);
    }

    private void OnItemClick(GameObject go)
    {
        if (dele_activityNameClick != null)
        {
            dele_activityNameClick(index);
        }
    }

    #endregion

    #region LifiCycle 
    public ActivityNameItem(){
#if __DEBUG_LIFE_CYCLE
#endif
	}
			
	// Use this for per initialization
	void Awake () {
        UIEventListener.Get(gameObject).onClick = OnItemClick;
	}
	
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	
	
	void LateUpdate(){
	
	}
	
	void FixedUpdate(){
	}
	
	void OnDestroy(){
#if __DEBUG_LIFE_CYCLE
#endif
	}
	
	#endregion //LifiCycle 
	
}
