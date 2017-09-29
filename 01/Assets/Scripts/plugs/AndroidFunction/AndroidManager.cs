using System;
using System.Collections.Generic;
using UnityEngine;
using LitJson;

public class AndroidManager : projectQ.SingletonTamplate<AndroidManager>
{
    #region Class ExchangeData

    /// <summary>
    /// 平台交换数据 
    /// </summary>
    public class ExchangeData
    {
        const string k = "k";
        const string v = "v";

        private string _k;
        private string _v;

        public ExchangeData(string key, string var)
        {
            this._k = key;
            this._v = var;
        }

        public string ToJson()
        {
            JsonData data = new JsonData();
            data[k] = _k;
            data[v] = _v;
            return data.ToJson();
        }
    }

    #endregion

    #region Fields

    #region Public

    public const string UnityAndroidPath = "com.unity3d.player.UnityPlayer";

    public const string CurrentActivity = "currentActivity";

    public const string TargetFunctionName = "ResolutionJson";

    public static Action<string> ReceiveMessage;

    public static Action<Dictionary<string, string>> ContactCallBackAction;

    public static Action<string> GetParams;

    public static Dictionary<string, string> ContactNameNumDic = new Dictionary<string, string>();

    public static string strParams;

    //public GameObjectManager gameObjectManager;

	#if UNITY_ANDROID||UNITY_IPHONE
    private UniWebView _webView;
	private GameObject _block ;
	private ScreenOrientation _savedScreenOrientation = ScreenOrientation.Unknown ;
#endif
    private string _errorMessage;

    //public UniWebView webView;

    #endregion

    #endregion

    #region Methods

    #region Public

    public void CallExchange(string str)
    {
#if UNITY_ANDROID
        AndroidJavaClass jc = new AndroidJavaClass(UnityAndroidPath);
        AndroidJavaObject jo = jc.GetStatic<AndroidJavaObject>(CurrentActivity);
		jo.Call( TargetFunctionName,str); 
        
#else

#endif     
    }

    public void UnityCallExchange(string clazz, string func, string param)
    {
#if UNITY_ANDROID
		ExchangeData connectToAndroid = new ExchangeData(func, param);
		string jsonStr = connectToAndroid.ToJson();
		this.CallExchange(jsonStr);
#endif
    }

    public void UnityCallExchange(string func, string param)
    {
#if UNITY_ANDROID
        ExchangeData connectToAndroid = new ExchangeData(func, param);
        string jsonStr = connectToAndroid.ToJson();
        this.CallExchange(jsonStr);
#endif
    }

    /*public void SendPhoneNum(string phoneNum)
    {
        UnityCallExchange("onUnityClickSendMessage", phoneNum);
    }*/

    public void ShowToast(string str)
    {
        UnityCallExchange("ShowToast", str);
    }

    public void ReceiveSMS(string message)
    {
        ReceiveMessage.Invoke(message);
    }

    public void SendNetworkChangeMessge(string networkState)
    {
        UnityCallExchange("ShowToast", networkState);
    }

    public void OnGetContactButton()
    {
        UnityCallExchange("getPhoneContacts", "");
    }

    public void OpenApp(string str)
    {
        UnityCallExchange("OpenApp", str);
    }

    public void GetParamsFromApp()
    {
        UnityCallExchange("GetParams", "");
    }

    public void AddBroadcast()
    {
        UnityCallExchange("AddBroadcast", "");
    }

    public void RemoveBroadcast()
    {
        UnityCallExchange("RemoveBroadcast", "");
    }

    public void OnGetContactCallBack(string str)
    {
        string[] contactEvery = str.Split(new char[] { ';' });
        for (int i = 0; i < contactEvery.Length; i++)
        {
            if (string.IsNullOrEmpty(contactEvery[i]))
            {
                continue;
            }
            string[] contactId2Name = contactEvery[i].Split(new char[] { '&' });
            if (ContactNameNumDic.ContainsKey(contactId2Name[0]))
                continue;
            ContactNameNumDic.Add(contactId2Name[0], contactId2Name[1]);
        }
        ContactCallBackAction.Invoke(ContactNameNumDic);
    }

    //public void GetUrlParams(string str)
    //{
    //    gameObjectManager.SetText(str);
    //}

    public void sendCenterPhoneNum(string str)
    {
        UnityCallExchange("GetCenterPhoneNum", str);
    }

    public void sendKeyWords(string str)
    {
        UnityCallExchange("GetKeyWords", str);
    }

    public void ShareToFriendsText()
    {
        UnityCallExchange("ShareText", "Session");
    }

    public void ShareToFriendsPic()
    {
        UnityCallExchange("SharePicture", "Session");
    }

    public void ShareToFriendsUrl()
    {
        UnityCallExchange("ShareUrl", "Session");
    }

    public void ShareToTimelineText()
    {
        UnityCallExchange("ShareText", "Timeline");
    }

    public void ShareToTimelinePic()
    {
        UnityCallExchange("SharePicture", "Timeline");
    }

    public void ShareToTimelineUrl()
    {
        UnityCallExchange("ShareUrl", "Timeline");
    }

    public void ShareWebToWXSceneSession(string url, string title, string des, string img)
    {
        AndroidJavaClass jc = new AndroidJavaClass(UnityAndroidPath);
        AndroidJavaObject jo = jc.GetStatic<AndroidJavaObject>(CurrentActivity);
        jo.Call("ShareWebPageToSession", url, title, des, img);
    }

    public void ShareWebToTimeline(string url, string title, string des, string img)
    {
        AndroidJavaClass jc = new AndroidJavaClass(UnityAndroidPath);
        AndroidJavaObject jo = jc.GetStatic<AndroidJavaObject>(CurrentActivity);
        jo.Call("ShareWebPageToTimeline", url, title, des, img);
    }

    public void LogIn()
    {
        UnityCallExchange("WXLogin", "");
    }

    public void WXPay(string json)
    {
        UnityCallExchange("WXPay", json);
    }

    public void OpenPara()
    {
        UnityCallExchange("OpenPara", "");
    }

    public void WxInstalled()
    {
        UnityCallExchange("WXInstalled", "");
    }


    public bool IsOpenWebView()
    {
#if UNITY_ANDROID || UNITY_IPHONE
        return _webView != null;
#else
        return false;
#endif
    }

    public void InitWebView() {
#if UNITY_ANDROID || UNITY_IPHONE
        if (_webView == null) {
            GameObject go = new GameObject();
            go.transform.parent = null;
            _webView = go.AddComponent<UniWebView>();

            _webView.OnLoadComplete += OnLoadComplete;
            _webView.OnReceivedMessage += OnReceivedMessage;
            _webView.OnEvalJavaScriptFinished += OnEvalJavaScriptFinished;
			_webView.OnReceivedKeyCode += OnReceiveKeyMessage;

            if (_block == null) {
                //白背景
                _block = GameObject.Instantiate(Resources.Load("UIPrefab/System/SystemWriteBlock") as GameObject);
                _block.transform.parent = projectQ._R.ui.UIPop.transform;
                _block.transform.localPosition = Vector3.zero;
                _block.transform.localScale = Vector3.one;

                NGUITools.SetActive (_block, false);
            }

            Debug.LogError("[WEW-VIEW]初始化WEB VIEW");
        }
#endif


    }

#if UNITY_ANDROID || UNITY_IPHONE


    

    public UniWebView GetWebView()
	{
        this.InitWebView();
        NGUITools.SetActive (_block, true);
        Debug.LogError("[WEW-VIEW]创建WEB VIEW");
        return _webView;
	}

#endif

    public void HiddenWebView()
    {
#if UNITY_ANDROID || UNITY_IPHONE
        //提高帧数
        projectQ.FrameRateMgr.Instance.RevertFrame();

        Screen.orientation = this._savedScreenOrientation;
		Screen.orientation = ScreenOrientation.AutoRotation;
		Screen.autorotateToLandscapeLeft = true;
		Screen.autorotateToLandscapeRight = true;
		Screen.autorotateToPortrait = false;
		Screen.autorotateToPortraitUpsideDown = false;
        
		if (_block != null)
        {
			NGUITools.SetActive (_block, false);
		}
        
		if (_webView != null)
		{
            
            _webView.Stop();
            GameObject.Destroy(_webView.gameObject);
            _webView = null;

            //_webView.Hide();


        }
#endif
    }

    public void BackWebView()
    {
#if UNITY_ANDROID || UNITY_IPHONE
        if (_webView != null)
        {
            if (_webView.CanGoBack())
                _webView.GoBack();
            else
                this.HiddenWebView();
		}
#endif
    }

    public void OpenWebViewHor(string _errorMessage, string url)
    {
#if UNITY_ANDROID || UNITY_IPHONE
        this._webView = GetWebView();
        Screen.orientation = ScreenOrientation.Landscape;
        _webView.OnLoadComplete += OnLoadComplete;
        _webView.OnReceivedMessage += OnReceivedMessage;
        _webView.OnEvalJavaScriptFinished += OnEvalJavaScriptFinished;
        _webView.url = "file:///android_asset/index.html";
        _webView.url = url;
        _webView.insets = new UniWebViewEdgeInsets(0, 0, 0, 0);
        _webView.Load();
        _errorMessage = null;
#endif
    }

    //public void OpenWebViewHor(UniWebView _webView, string _errorMessage)
    //{
    //    Screen.orientation = ScreenOrientation.Landscape;
    //    _webView.OnLoadComplete += OnLoadComplete;
    //    _webView.OnReceivedMessage += OnReceivedMessage;
    //    _webView.OnEvalJavaScriptFinished += OnEvalJavaScriptFinished;
    //    _webView.url = "file:///android_asset/index.html";
    //    _webView.insets = new UniWebViewEdgeInsets(5, 5, 5, 5);
    //    _webView.Load();
    //    _errorMessage = null;
    //}

    public void OpenWebViewVar(string _errorMessage, string url)
    {
		#if UNITY_ANDROID || UNITY_IPHONE
		projectQ.FrameRateMgr.Instance.LowerFrame();
		#if UNITY_ANDROID
		this._savedScreenOrientation = Screen.orientation;
		Screen.orientation = ScreenOrientation.Portrait;
		#endif
		this._webView = GetWebView();
		_webView.url = url;
		//_webView.url = "file:///android_asset/index.html";

		var a = _block.GetComponent<projectQ.UniwebViewClose>();

		float height = 35;
		#if UNITY_IOS
		IOSManager.Instance.ChangeOrientionByUnity(1);
		height= 32;
//		IOSManager.Instance.checkLocationServiceOpenStatus();
		float width = Screen.width < Screen.height? Screen.width:Screen.height;
		if (width == 1080) {
		height= 25;
		}else if (width == 750 || width == 640 || width == 1536) {
		height= 32.5f;
		}
		#endif
		height = Screen.width * height / 1334 ;



//       NGUIDebug.Log(Screen.height + "-" + Screen.width + "--->" + height);
//		this._savedScreenOrientation = Screen.orientation;
//		Screen.orientation = ScreenOrientation.Portrait;
        
		//NGUIQLoger.LOG(string.Format("NOW{0}X{1}.OLD{2}X{3} : H{4}" , Screen.width , Screen.height
		//	,v.x , v.y , height));

#if !UNITY_EDITOR
		height *= 1.5f;
#endif
		_webView.insets = new UniWebViewEdgeInsets((int)height , 0, 0, 0);        
		_webView.CleanCache();
		_webView.CleanCookie();
		_webView.Load();

        _errorMessage = null;
#endif
    }

#if UNITY_ANDROID || UNITY_IPHONE
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
#endif

    public void OnScreenShotButton()
    {
        string now = System.DateTime.Now.ToString();
        now = now.Trim();
        now = now.Replace("/", "-");
        string filename = "ScreenShot" + now + ".png";
        if (Application.platform == RuntimePlatform.Android)
        {
            Texture2D texture = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);
            texture.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0);
            texture.Apply();
            byte[] bytes = texture.EncodeToPNG();
            string strToAndroid = System.Text.Encoding.Default.GetString(bytes);
            string destination = Application.temporaryCachePath;
            System.IO.File.WriteAllBytes(destination + "/" + filename, bytes);
        }
    }

#if UNITY_ANDROID || UNITY_IPHONE
    void OnEvalJavaScriptFinished(UniWebView webView, string result)
    {

    }
#endif

#if UNITY_ANDROID || UNITY_IPHONE
    void OnReceivedMessage(UniWebView webView, UniWebViewMessage message)
    {
        if (string.Equals(message.path, "root"))
        {
			if (string.Equals (message.args ["action"], "close")) {
				HiddenWebView ();
			}
        }
    }
#endif
#if UNITY_ANDROID || UNITY_IPHONE
    void OnReceiveKeyMessage(UniWebView webView,int keyCode)
    {
        if (keyCode == 4) //返回键keycode
        {
            if (webView.currentUrl == webView.url)
            {
                HiddenWebView();
            }
        }
    }
#endif
}
#endregion

#endregion
