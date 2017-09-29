/**
 * @Author JEFF
 *
 *
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using projectQ;

public class ClipBoardSDK : SDKBase
{
	private System.Action<string> _Action;
	public void SendDataToClipboard(SDKData.ClipboardData data,System.Action<string> callback)
	{
		_Action = callback;
		#if UNITY_ANDROID
		AndroidManager.Instance.UnityCallExchange("PutStringToClipboard", data.ToString());
		#elif !UNITY_EDITOR&&UNITY_IOS
		//调用clipboard.h中的接口
		IOSManager.Instance.IOSPutStringToClipboard(data.ToString());
		QLoger.ERROR ("CopyToClipboard_______"+data.ToString());
		#endif
	}
	public void GetDataFromClipboard(string data)
	{
		if (_Action!=null)
		{
			_Action(data);
		}
		QLoger.LOG("==unity msg ==data===" + data);
	}

}
