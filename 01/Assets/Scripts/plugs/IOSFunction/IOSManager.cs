using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices;
/// <summary>
/// IOS manager. 
/// </summary>
public class IOSManager  {
	
	private static IOSManager _Instance=null;

	public static IOSManager  Instance
	{
		get{
			if (_Instance == null) {
				_Instance = new IOSManager ();
			}
			return _Instance;
		}
	}

	#region login 

	[DllImport ("__Internal")]
	private static extern void IOSWXInstalled ();

	[DllImport ("__Internal")]
	private static extern void IOSWXLogin ();

	[DllImport ("__Internal")]
	private static extern void GetOpenPara ();

	[DllImport ("__Internal")]
	private static extern void WXSharing (string message);

	[DllImport ("__Internal")]
	private static extern void PutStringToClipboard (string message);

	[DllImport ("__Internal")]
	private static extern void IOSWXPay(string json);

	[DllImport ("__Internal")]
	private static extern void ChageToOrientationByUnity (int orientaion);

	[DllImport ("__Internal")]
	private static extern bool IsOpenLocationService();

	[DllImport ("__Internal")]
	private static extern bool IsAuthoriseLocationService();

	[DllImport ("__Internal")]
	private static extern void OpenWebViewController(string url);

	[DllImport ("__Internal")]
	private static extern void UnityRequestStartUpdateGPS ();

	[DllImport ("__Internal")]
	private static extern void UnityRequestStopUpdateGPS ();

	[DllImport ("__Internal")]
	private static extern int UnityRequestNetworkState ();

	[DllImport ("__Internal")]
	private static extern int UnityRequestWiFiStrenth ();

	[DllImport ("__Internal")]
	private static extern int UnityRequestBatteryLevel();


	public void LogIn()
	{
		Debug.Log ("unity begin wxlogin");
		IOSWXLogin ();
	}
	public void LoginCallback(string respData)
	{
		Debug.Log ("wxlogin respData is :"+respData);
	}

	public void CheckWXInstalled()
	{
		Debug.Log ("unity check wx installed");
		IOSWXInstalled ();
	}
	public void IOSGetOpenPara()
	{
		Debug.Log ("unity GetOpenPara");
		GetOpenPara ();
	}
	public void IOSWXSharing(string message)
	{
		Debug.Log ("unity IOSWXSharing");
		WXSharing (message);
	}

	public void IOSPutStringToClipboard(string message)
	{
		PutStringToClipboard (message);
	}

	public void WXPay(string json)
	{
		Debug.Log ("=====WXPay:"+json);
		IOSWXPay (json);
	}

	/// <summary>
	/// 切换屏幕方向
	/// </summary>
	/// <param name="orientaion">Orientaion.</param>
	public void ChangeOrientionByUnity(int orientaion) {
		Debug.Log ("=====ChangeOrientionByUnity:"+ orientaion.ToString());
		ChageToOrientationByUnity (orientaion);
	}

	/// <summary>
	/// 打开iOS端的WebView
	/// </summary>
	/// <param name="url">webView的链接地址</param>
	public void openWebView (string url){
		OpenWebViewController (url);
	}

	/// <summary>
	/// 获取设备定位服务开启状态
	/// </summary>
	/// <returns><c>true</c>, if location service open status was opened, <c>false</c> otherwise.</returns>
	public bool checkLocationServiceOpenStatus (){
	
		return IsOpenLocationService ();
	}

	/// <summary>
	/// 获取应用定位服务授权状态
	/// </summary>
	/// <returns><c>true</c>, if location service authorise status was Authorised, <c>false</c> otherwise.</returns>
	public bool checkLocationServiceAuthoriseStatus (){

		return IsAuthoriseLocationService ();
	}
		

	/// <summary>
	/// 开始获取GPS信息
	/// </summary>
	public void startUpdateLocation() {
		UnityRequestStartUpdateGPS ();
	}

	/// <summary>
	/// 停止获取GPS信息
	/// </summary>
	public void stopUpdateLocation() {
		UnityRequestStopUpdateGPS ();
	}

	/// <summary>
	/// 获取当前网络类型（None，2G，3G，4G，WiFi）
	/// </summary>
	/// <returns>The network state.</returns>
	public int getNetworkState () {
	
		return UnityRequestNetworkState ();
	}

	/// <summary>
	/// Gets the wifi strenth.
	/// </summary>
	/// <returns>The wifi strenth.</returns>
	public int getWiFiStrenth () {

		return UnityRequestWiFiStrenth ();
	}

	/// <summary>
	/// 获取当前电量：电量（0~1.0）* 100
	/// </summary>
	/// <returns>The battery level.</returns>
	public int getBatteryLevel () {
		return UnityRequestBatteryLevel ();
	}

	#endregion


	#region
	#if UNITY_IOS 
	private UniWebView _webView;
	private string _errorMessage;

	public void OpenWebView(string _errorMessage, string url)
	{
		projectQ.FrameRateMgr.Instance.LowerFrame();
		this._webView = GetWebView();
		_webView.url = url;  
		_webView.OnLoadComplete += OnLoadComplete;
		_webView.OnReceivedMessage += OnReceivedMessage;
		_webView.OnEvalJavaScriptFinished += OnEvalJavaScriptFinished;
		_webView.CleanCache();
		_webView.CleanCookie();
		_webView.Load();
		_errorMessage = null;
	}
		
	public UniWebView GetWebView()
	{
		this.InitWebView();
		Debug.LogError("[WEW-VIEW]创建WEB VIEW");
		return _webView;
	}

	public void InitWebView() {

		if (_webView == null) {
			GameObject go = new GameObject();
			go.transform.parent = null;
			_webView = go.AddComponent<UniWebView>();
			Debug.LogError("[WEW-VIEW]初始化WEB VIEW");
		}
	}



	void OnLoadComplete(UniWebView webView, bool success, string errorMessage)
    {
        if (success)
        {
            webView.Show();
        }
        else
        {
            _errorMessage = errorMessage;
        }
    }
		
    void OnEvalJavaScriptFinished(UniWebView webView, string result){}



    void OnReceivedMessage(UniWebView webView, UniWebViewMessage message)
    {
        if (string.Equals(message.path, "root"))
        {
			if (string.Equals (message.args ["action"], "close")) {
				HiddenWebView ();
			}
        }
    }

	public void HiddenWebView()
	{
		projectQ.FrameRateMgr.Instance.RevertFrame();
		if (_webView != null)
		{
			_webView.Stop();
			GameObject.Destroy(_webView.gameObject);
			_webView = null;
		}
	}
	#endif
	#endregion
}
