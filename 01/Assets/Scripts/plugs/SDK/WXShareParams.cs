/**
 * @Author JEFF
 *
 *
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using projectQ;
public enum WXType
{//只能在后面追加类型
	None=0,
	WXSceneTimeline=1,//朋友圈
	WXSceneSession=2,//聊天界面
	WXSceneFavorite=3,//收藏

}
public enum WXShareType
{//只能在后面追加类型
	None=0,
	WXText=1,//文字
	WXPicture=2,//图片
	WXAudio=3,//音乐
}


public class WXShareParams : SDKParams
{
	#region 属性
    private string _Key;
    public string KEY
    {
        get { return _Key; }
    }
	private string _title;
	public string Title
	{
		get { return _title; }
	}
	private string _desc;
	public string Desc
	{
		get { return _desc; }
	}
	private string _bitMapStr="";
	public string BitMapStr
	{
		get { return _bitMapStr; }
	}
	private WXType _WXType = WXType.None;
	public WXType WXType
	{
		get { return _WXType; }
	}
	private WXShareType m_WXShareType = WXShareType.None;
	public WXShareType WXShareType
	{
		get { return m_WXShareType; }
	}
	#endregion


	public WXShareParams(string linkConfKey,string url=null,string title=null,string desc=null)
	{
        _Key = linkConfKey;
		LinkConf linkConf = SDKManager.Instance.GetDataByKey(linkConfKey);
		if (string.IsNullOrEmpty(url))
		{
			_url = linkConf.Url;
		}
		else
		{
			_url = url;
		}
		if (string.IsNullOrEmpty(title))
		{
			_title = linkConf.Title;
		}
		else
		{
			_title = title;
		}
		if (string.IsNullOrEmpty(desc))
		{
			_desc = linkConf.Desc;
		}
		else
		{
			_desc = desc;
		}   
		_WXType = (WXType)int.Parse(linkConf.WX_Type);
		m_WXShareType = (WXShareType)int.Parse(linkConf.WX_ShareType);
	}
	public WXShareParams(string linkConfKey, WXType type, WXShareType shareType)
	{
        _Key = linkConfKey;
		LinkConf linkConf = SDKManager.Instance.GetDataByKey(linkConfKey);
		_url = linkConf.Url;
		_title = linkConf.Title;
		_desc = linkConf.Desc;
		_WXType = type;
		m_WXShareType = shareType;
	}
	public WXShareParams(string linkConfKey, string bitMapStr)
	{
        _Key = linkConfKey;
		LinkConf linkConf = SDKManager.Instance.GetDataByKey(linkConfKey);
		_url = linkConf.Url;
		_bitMapStr = bitMapStr;
		_WXType = (WXType)int.Parse(linkConf.WX_Type); 
		m_WXShareType = WXShareType.WXPicture;
	}
    //public WXShareParams(string bitMapStr, WXType type)
    //{
    //    _bitMapStr = bitMapStr;
    //    _WXType = type;
    //    m_WXShareType = WXShareType.WXPicture;
    //}

	public string InserTitleParams(params object[] titleParams)
	{
		if(titleParams==null||titleParams.Length<=0)
		{
			return _title;
		}
		try
		{
			_title = string.Format(_title, titleParams);
		}
		catch { }

		return _title;
	}
	public string InsertDescParams(params object[] descParams)
	{
		if(descParams==null||descParams.Length<=0)
		{
			return _desc;
		}
		try
		{
			_desc = string.Format(_desc, descParams);
		}
		catch { }

		return _desc;
	}
}
