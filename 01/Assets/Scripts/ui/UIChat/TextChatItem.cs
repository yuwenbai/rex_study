

using System;
/**
* 作者：周腾
* 作用：
* 日期：
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextChatItem : MonoBehaviour {
    public UILabel label;
    private string chatMsg;
    public delegate void TextClick(string content);
    public TextClick dele_textClick;
    public void InitItem(string id, string content)
    {
        chatMsg = id;
        label.text = content;
    }
	
    #region LifiCycle 
    public TextChatItem(){
#if __DEBUG_LIFE_CYCLE
#endif
	}
			
	// Use this for per initialization
	void Awake () {
        UIEventListener.Get(gameObject).onClick = OnItemClick;
	}

    private void OnItemClick(GameObject go)
    {
        UIToggle tog = go.GetComponent<UIToggle>();
        if (tog != null)
            tog.value = true;

        if (dele_textClick != null)
        {
            dele_textClick(chatMsg);
        }
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
