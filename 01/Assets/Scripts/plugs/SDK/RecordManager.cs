

using gamelink;
/**
* @Author JEFF
*
*
*/
using System ;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using projectQ;

public class RcordData
{
	/// <summary>
	/// 玩家姓名
	/// </summary>
	public string uname = "USER" + UnityEngine.Random.Range(0, int.MaxValue);
	/// <summary>
	/// 频道名
	/// </summary>
	public string channelName = "CHANNEL_ELEVEN";
	/// <summary>
	/// 发送目标名字
	/// </summary>
	public string target;
	/// <summary>
	/// 目标对象
	/// </summary>
	public GLTarget gTarget;
	/// <summary>
	/// 录音文件
	/// </summary>
	public string rfile;
	/// <summary>
	/// 录音时长
	/// </summary>
	public uint duration;
	/// <summary>
	/// 录音内容，转文字用
	/// </summary>
	public string voiceContent;
	public string text;

	/// <summary>
	/// 消息列表
	/// </summary>
	public List<GLMessage> messagesList = new List<GLMessage>();

	/// <summary>
	/// The is playing.
	/// </summary>
	public bool isPlaying = false ;

	/// <summary>
	/// The recording volume.
	/// </summary>
	public float recording_volume; 

	/// <summary>
	/// The duration of the recording.
	/// </summary>
	public uint recording_duration;
}

public class RecordManager : MonoBehaviour
{

    public RcordData data;
    bool isNeedUpload = false;
    public System.Action<bool> DL_IS_RECODING;
    public System.Action<string, bool> DL_IS_PLAYING;
    public System.Action<float,uint> DL_RECORDING_VOICE_CHANGE;
    public System.Action<string> DL_ERROR;
    public System.Action<bool , uint> DL_RECORDING_LENGTH_ERROR ; //bool true : 过长，FALSE，过短

    private static RecordManager _Instance = null;

    [HideInInspector]
    public ulong m_SpeakPlayer;

    private e_RecordStateControl m_CurrentState;
    private e_RecordStateControl m_ChangeToState;
    private e_RecordStateStage m_StateStage;
    private RecordStateBase m_StateBase;
    private Dictionary<e_RecordStateControl, RecordStateBase> m_Dict_State;

    public static RecordManager Instance
    {
        get
        {
            return _Instance;
        }
    }

    #region 状态控制变量
    /// <summary>
    /// 是否初始化
    /// </summary>
    private bool m_IsInit = false;

    /// <summary>
    /// 是否登入成功
    /// </summary>
    private bool m_IsLogin = false;

    /// <summary>
    /// 是否正在等待登入
    /// </summary>
    private bool m_WaitLogin = false;

    /// <summary>
    /// 是否正在录音
    /// </summary>
    private bool m_IsRecord = false;

    /// <summary>
    /// 如果点击了录音
    /// </summary>
    private bool m_IsClickRecord = false;

    /// <summary>
    /// 正在播放其他人的录音
    /// </summary>
    private bool m_IsWaitRecord = false;

    /// <summary>
    /// 正在播放自己的录音
    /// </summary>
    private bool m_IsPlayingMyRecord = false;

    /// <summary>
    /// 等待上传完成（再播放）
    /// </summary>
    private bool m_IsWaitUpOver = false;

    /// <summary>
    /// 如果点击过Stop
    /// </summary>
    private bool m_IsClickStop = false;

    /// <summary>
    /// 等待停止播放完成再开始执行录音
    /// </summary>
    private bool m_IsWaitStopOver = false;

    /// <summary>
    /// 紧急停止录音（当抬手是OnStart还未返回时使用，ios崩溃的问题使用）
    /// </summary>
    private bool m_UrgencyStop = false;

    /// <summary>
    /// 是否刚刚开始播放录音（ios崩溃的问题使用）
    /// </summary>
    private bool m_IsInitPlay = false;
    #endregion

    private Coroutine m_Coroutine;

    public GLMessage CurrentMessage = null;
    private bool m_IsPlaying = false;
    private float m_PlayerWaitTime = 30;
    private float m_LimitTime = 31;

    private string appKey = "glb4x7lky5poj004nv5dy3";

    //private GLTarget m_CurrentTarget;

    void Awake()
    {
        _Instance = this;

        m_CurrentState = e_RecordStateControl.None;
        m_ChangeToState = e_RecordStateControl.None;
        m_Dict_State = new Dictionary<e_RecordStateControl, RecordStateBase>();
    }

    void Start()
    {
        this.data = new RcordData();
        InitSDK();
    }

    #region Init相关
    //初始化SDK
    private void InitSDK()
    {
#if UNITY_EDITOR || UNITY_STANDALONE_WIN
        return;
#endif
        m_ChangeToState = e_RecordStateControl.Init;
        ShowDebug("InitSDK");


        return;
        bool isInitOk = GameLink.Init(appKey);
        if(isInitOk)
        {
            GameLink.EnableLog(true);
            GameLink.ClearCache();
            GameLink.EnableVoiceRecognition(true);
            GameLink.AddObserver(this);
            m_IsInit = true;
            ShowDebug("初始化成功");
        }
        else
        {
            ShowDebug("初始化失败");
            QLoger.ERROR("语音SDK初始化失败");
        }
    }

    #endregion

    #region 登陆相关
    /// <summary>
    /// 外部调用设置用户信息
    /// </summary>
    /// <param name="uid">用户ID</param>
    /// <param name="channel">用户频道</param>
    public void SetUserInfo(string uid, string channel)
    {
        this.data.uname = uid;
        this.data.channelName = channel;
    }

    /// <summary>
    /// 调用登陆
    /// </summary>
    public void LoginRecord()
    {
#if UNITY_EDITOR || UNITY_STANDALONE_WIN
        return;
#endif
        if (m_ChangeToState == e_RecordStateControl.Init || m_ChangeToState == e_RecordStateControl.Logout)
            m_ChangeToState = e_RecordStateControl.Login;
        //ShowDebug("LoginRecord ");
        return;
        if (!m_IsInit)
        {
            InitSDK();
        }

        if (m_IsLogin || m_WaitLogin)
            return;

        m_WaitLogin = true;
        GameLink.SetProfile(name, "Avatar", "ExtraInfo");
        GameLink.Login(this.data.uname);
        ShowDebug("Login");
    }

    //登陆回调
    //void OnLogin(GLTarget target, GLError error)
    //{
    //    if (error == null || error.Code == GLErrorCode.OnlineOrLogining)
    //    {
    //        //m_CurrentTarget = target;
    //        ShowDebug("OnLogin");

    //        JoinChannel();
    //        if (m_IsWaitRecord)
    //        {
    //            StartCoroutine(WaitStart());
    //        }
    //    }
    //    else
    //    {
    //        m_WaitLogin = false;
    //        ShowDebug("OnLogin", error);
    //        if (this.DL_ERROR != null)
    //        {
    //            this.DL_ERROR(error.Code.ToString());
    //        }
    //    }
    //}
    
    public void setDelegate(System.Action<bool> recoder,
        System.Action<string, bool> play,
        System.Action<float, uint> VoiceChanged,
        System.Action<bool, uint> lengthError,
        System.Action<string> error)
    {
#if UNITY_EDITOR || UNITY_STANDALONE_WIN
        return;
#endif
        this.DL_IS_PLAYING = play;
		this.DL_IS_RECODING = recoder;
        this.DL_RECORDING_VOICE_CHANGE = VoiceChanged;
        this.DL_ERROR = error;
		this.DL_RECORDING_LENGTH_ERROR = lengthError;
        ShowDebug("setDelegateINIT");
    }

    //加入频道
    private void JoinChannel()
    {
        m_IsLogin = true;
        m_WaitLogin = false;
        GameLink.JoinChannel(this.data.channelName);
    }

    //void OnJoinChannel(GLTarget channel, GLError error)
    //{
    //    if(error == null)
    //    {
    //        m_IsLogin = true;
    //        this.data.gTarget = channel;
    //        ShowDebug(string.Format("OnJoinChannel  ID:{0}",this.data.channelName));
    //    }
    //    else
    //    {
    //        ShowDebug("OnJoinChannel", error);
    //    }
    //}

#endregion

    #region 录音相关
    private IEnumerator WaitStart()
    {
        int i = 0;
        while (i < 8)
        {
            i++;
            yield return new WaitForEndOfFrame();
        }

        m_IsClickRecord = false;
        StartRecord();
    }

    /// <summary>
    /// 调用开始录音
    /// </summary>
    public void StartRecord()
    {
#if UNITY_EDITOR || UNITY_STANDALONE_WIN
        return;
#endif
        ShowDebug("开始录音");
        m_ChangeToState = e_RecordStateControl.Record;
        return;
        if (m_IsRecord || m_IsClickRecord || m_IsWaitUpOver)
            return;
        
        m_IsClickRecord = true;

        if(m_IsInitPlay)
        {
            StartCoroutine(WaitStart());
            return;
        }

        if (!m_IsLogin)
        {
            m_IsWaitRecord = true;
            LoginRecord();
            ShowDebug("开始录音前先登录");
            return;
        }
        
        if(m_IsPlaying || m_IsPlayingMyRecord)
        {
            ShowDebug("开始录音前停止播放");
            m_IsWaitStopOver = true;
            GameLink.StopPlay();
            return;
        }

        m_IsWaitRecord = false;
        ShowDebug("Start");
        GameLink.StartRecord();
    }

    ////开始录音回调
    //void OnStartRecord(GLError error)
    //{
    //    m_IsClickRecord = false;

    //    if (error == null)
    //    {
    //        ShowDebug("OnStartRecord");
    //        if (m_UrgencyStop)
    //        {
    //            isNeedUpload = false;
    //            m_IsRecord = true;
    //            m_IsPlayingMyRecord = false;
    //            this.data.rfile = "";
    //            StopRecord();
    //        }
    //        else
    //        {
    //            m_IsRecord = true;
    //            isNeedUpload = true;
    //            m_IsPlayingMyRecord = false;
    //            this.data.rfile = "";
    //            if (this.DL_IS_RECODING != null)
    //            {
    //                this.DL_IS_RECODING(true);
    //            }
    //        }
    //    }
    //    else
    //    {
    //        isNeedUpload = false;
    //        m_IsRecord = false;
    //        ShowDebug("OnStartRecord", error);
    //        if (this.DL_IS_RECODING != null)
    //        {
    //            this.DL_ERROR(error.Code.ToString());
    //        }
    //        LogoutRecord();
    //    }

    //    m_UrgencyStop = false;
    //}

    ////录音中回调（跟Update一样会被持续调用）
    //void OnRecording(float volume, uint duration)
    //{
    //    this.ShowDebug("OnRecording ");
    //    if (this.DL_RECORDING_VOICE_CHANGE != null)
    //    {
    //        this.DL_RECORDING_VOICE_CHANGE(volume, duration);
    //    }
    //}
    
    /// <summary>
    /// 调用停止录音
    /// </summary>
    public void StopRecord(bool isCancle = false)
    {
#if UNITY_EDITOR || UNITY_STANDALONE_WIN
        return;
#endif
        if (m_StateBase.recordState == e_RecordStateControl.Record)
        {
            RecordRecord play = m_StateBase as RecordRecord;
            if (play != null)
            {
                play.Stop(!isCancle);
            }
            ShowDebug("Stop");
        }
        else
        {
            m_ChangeToState = e_RecordStateControl.Play;
        }
        return;
        if (m_IsClickRecord && !m_IsRecord)
        {
            m_UrgencyStop = true;
            return;
        }

        if(!m_IsRecord)
            return;
        
        m_IsClickStop = true;
        isNeedUpload = !isCancle;
        GameLink.StopRecord();
    }


    //停止录音回调
    //void OnStopRecord(String file, String voiceContent, uint duration, GLError error)
    //{
    //    if (null == error)
    //    {
    //        this.ShowDebug("OnStopRecord ");
    //        m_IsRecord = false;
    //        m_IsWaitRecord = false;
    //        if (this.DL_IS_RECODING != null)
    //        {
    //            this.DL_IS_RECODING(false);
    //        }

    //        if (isNeedUpload && m_IsLogin)
    //        {
    //            this.data.rfile = file;
    //            this.data.duration = duration;
    //            this.data.voiceContent = voiceContent;
    //            this.data.text = "";

    //            if (duration > 30000 || duration < 950 || !m_IsClickStop)
    //            {
    //                if (this.DL_RECORDING_LENGTH_ERROR != null)
    //                {
    //                    this.DL_RECORDING_LENGTH_ERROR(duration < 950, duration);
    //                }

    //                this.ShowDebug("OnStopRecord 太短");
    //                this.data.rfile = "";

    //                return;
    //            }
    //            else
    //            {
    //                this.ShowDebug("UploadVoice uploading 上传");
    //                GameLink.UploadVoice(this.data.rfile);
    //                m_IsWaitUpOver = true;

    //                if (m_Coroutine != null)
    //                {
    //                    StopCoroutine(m_Coroutine);
    //                    m_Coroutine = null;
    //                }
    //                m_Coroutine = StartCoroutine(WaitUpDataOver());
    //            }
    //            m_IsClickStop = false;
    //        }
    //        else
    //        {
    //            ShowDebug("不需要上传或者未登录");
    //            return;
    //        }
    //    }
    //    else
    //    {
    //        this.ShowDebug("OnStopRecord " , error);
    //        if (this.DL_IS_RECODING != null)
    //        {
    //            this.DL_IS_RECODING(false);
    //        }
    //        if (this.DL_ERROR != null)
    //        {
    //            this.DL_ERROR(error.Code.ToString());
    //        }
    //        m_IsRecord = false;
    //        LogoutRecord();
    //    }
    //}
    IEnumerator WaitUpDataOver()
    {
        int i = 0;
        int limit = 10;
        while (i < limit && m_IsWaitUpOver)
        {
            i++;
            yield return new WaitForSeconds(1);
        }

        if (i == limit && m_IsWaitUpOver)
        {
            LogoutRecord();
            StopCoroutine(m_Coroutine);
            m_Coroutine = null;
            yield return null;
        }
    }

    //上传录音中
    //void OnUploadVoice(System.String file, System.String url, GLError error)
    //{
    //    if (null == error)
    //    {
    //        this.ShowDebug("OnUploadVoice OK");
    //        GameLink.SendVoice(this.data.gTarget, url, this.data.duration,
    //            this.data.text, this.data.voiceContent);
    //    }
    //    else
    //    {
    //        this.ShowDebug("OnUploadVoice fail" + projectQ.CommonTools.ReflactionObject(error));
    //        if (this.DL_ERROR != null)
    //        {
    //            this.DL_ERROR(error.Code.ToString());
    //        }
    //    }

    //    m_IsWaitUpOver = false;

    //    if (this.DL_IS_RECODING != null)
    //    {
    //        this.DL_IS_RECODING(false);
    //    }
    //}

    //下载录音中
    void OnDownloadVoice(GLMessage message, GLError error)
    {
        if (error == null)
        {
            this.ShowDebug("OnDownloadVoice ok ");
            this.data.messagesList.Add(message);
        }
        else
        {
            this.ShowDebug("OnDownloadVoice failed " + projectQ.CommonTools.ReflactionObject(error));
        }
    }

    //    void OnPlayStop(GLMessage message)
    //    {
    //#if UNITY_EDITOR || UNITY_STANDALONE_WIN
    //        return;
    //#endif
    //        this.ShowDebug("OnPlayStop " + message.Sender.Account);
    //        if (this.DL_IS_PLAYING != null)
    //        {
    //            this.DL_IS_PLAYING(message.Sender.Account, false);
    //        }
    //        this.data.isPlaying = false;
    //        m_IsPlaying = false;
    //        CurrentMessage = null;
    //        this.data.rfile = "";

    //        if (!string.IsNullOrEmpty(this.data.rfile) && !m_IsWaitStopOver)
    //        {
    //            this.ShowDebug("停止完成，开始播放我的录音");
    //            GameLink.PlayVoice(this.data.rfile);
    //            return;
    //        }
    //        else if (m_IsWaitStopOver)
    //        {
    //            this.ShowDebug("停止完成，开始录音");
    //            m_IsWaitStopOver = false;
    //            StartCoroutine(WaitStart());
    //            return;
    //        }
    //    }

    //    void OnPlayStop(String file)
    //    {
    //#if UNITY_EDITOR || UNITY_STANDALONE_WIN
    //        return;
    //#endif
    //        this.ShowDebug("OnPlayStop 我的录音");
    //        if (this.DL_IS_PLAYING != null)
    //        {
    //            this.DL_IS_PLAYING(this.data.uname, false);
    //        }
    //        this.data.isPlaying = false;
    //        m_IsPlayingMyRecord = false;
    //        CurrentMessage = null;
    //        this.data.rfile = "";

    //        if (m_IsWaitStopOver)
    //        {
    //            this.ShowDebug("停止完成，开始录音");
    //            m_IsWaitStopOver = false;
    //            StartCoroutine(WaitStart());
    //            return;
    //        }
    //    }

    private Dictionary<uint, uint> m_IdList = new Dictionary<uint, uint>();

    void OnReceiveMessage(GLMessage message)
    {
#if UNITY_EDITOR || UNITY_STANDALONE_WIN
        return;
#endif
        this.ShowDebug("OnReceiveMessage " + message.Sender.Account);
        if (message.Type == GLMessageType.Voice)
        {
            if (!m_IdList.ContainsKey(message.Id))
            {
                GameLink.DownloadVoice(message);
                m_IdList[message.Id] = message.Id;
            }
            this.ShowDebug("DownloadVoice " + message.Sender.Account);
        }
    }

//    void OnPlayStart(GLMessage message, GLError error)
//    {
//#if UNITY_EDITOR || UNITY_STANDALONE_WIN
//        return;
//#endif
//        m_IsInitPlay = false;
//        if (error == null)
//        {
//            this.ShowDebug("OnPlayStart ");
//            if (this.DL_IS_PLAYING != null)
//            {
//                this.DL_IS_PLAYING(message.Sender.Account, true);
//            }
//            CurrentMessage = message;
//            m_IsPlaying = true;
//        }
//        else
//        {
//            this.ShowDebug("OnPlayStart " + message.Sender.Account + projectQ.CommonTools.ReflactionObject(error));
//            if (this.DL_ERROR != null)
//            {
//                this.DL_ERROR(error.Code.ToString());
//            }
//        }
//    }

//    void OnPlayStart(String file, GLError error)
//    {
//#if UNITY_EDITOR || UNITY_STANDALONE_WIN
//        return;
//#endif
//        m_IsInitPlay = false;
//        if (error == null)
//        {
//            this.ShowDebug("OnPlayStart My");
//            if (this.DL_IS_PLAYING != null)
//            {
//                this.DL_IS_PLAYING(this.data.uname, true);
//            }
//            m_PlayerWaitTime = this.data.duration + 2f;
//            m_IsPlayingMyRecord = true;
//            m_IsPlaying = false;
//        }
//        else
//        {
//            this.ShowDebug("OnPlayStart " + projectQ.CommonTools.ReflactionObject(error));
//            if (this.DL_ERROR != null)
//            {
//                this.DL_ERROR(error.Code.ToString());
//            }
//        }
//    }

    private bool CheckStateOver(e_RecordStateControl state)
    {
        return (m_StateBase.recordState == state) && (m_StateBase.state == e_RecordStateStage.End || m_StateBase.state == e_RecordStateStage.Erro);
    }

    private bool CheckCurStateIsOver()
    {
        return m_StateBase.state == e_RecordStateStage.End || m_StateBase.state == e_RecordStateStage.Erro;
    }

    void Update()
    {
#if UNITY_EDITOR || UNITY_STANDALONE_WIN
        return;
#endif

        m_CurrentState = e_RecordStateControl.None;

        switch (m_ChangeToState)
        {
            case e_RecordStateControl.Init:
                if (m_StateBase == null)
                    m_CurrentState = e_RecordStateControl.Init;
                break;

            case e_RecordStateControl.Login:
                if (m_StateBase == null)
                    m_CurrentState = e_RecordStateControl.Login;
                else if (CheckStateOver(e_RecordStateControl.Init) || CheckStateOver(e_RecordStateControl.Logout))
                    m_CurrentState = e_RecordStateControl.Login;
                else if (m_StateBase.recordState == e_RecordStateControl.Login)
                {
                    if (CheckCurStateIsOver())
                        m_ChangeToState = e_RecordStateControl.Play;
                }
                break;

            case e_RecordStateControl.Record:
                if(m_StateBase == null)
                    m_CurrentState = e_RecordStateControl.Record;
                else if (m_StateBase.recordState == e_RecordStateControl.Login)
                {
                    ShowDebug("Record：Login");
                    if (CheckCurStateIsOver())
                        m_CurrentState = e_RecordStateControl.Record;
                    else if (m_StateBase.state == e_RecordStateStage.Erro)
                        m_ChangeToState = e_RecordStateControl.Logout;
                }
                else if (m_StateBase.recordState == e_RecordStateControl.Init || m_StateBase.recordState == e_RecordStateControl.Logout)
                {
                    ShowDebug("Record：Init||Logout");
                    if (CheckCurStateIsOver())
                    {
                        m_CurrentState = e_RecordStateControl.Login;
                    }
                }
                else if (m_StateBase.recordState == e_RecordStateControl.Play)
                {
                    ShowDebug("Record：Play");
                    if (!CheckStateOver(e_RecordStateControl.Play))
                    {
                        RecordPlay rp = m_StateBase as RecordPlay;
                        if (rp != null)
                            rp.StopPlay();
                    }
                    else
                    {
                        if (m_StateBase.state == e_RecordStateStage.Erro)
                            m_ChangeToState = e_RecordStateControl.Logout;
                        else
                            m_CurrentState = e_RecordStateControl.Record;
                    }
                }
                else if (m_StateBase.recordState == e_RecordStateControl.Record)
                {
                    ShowDebug(string.Format("Record：Record{0}", m_StateBase.state.ToString()));
                    if (CheckCurStateIsOver())
                    {
                        if (m_StateBase.state == e_RecordStateStage.Erro)
                            m_ChangeToState = e_RecordStateControl.Logout;
                        else
                            m_ChangeToState = e_RecordStateControl.Play;
                    }
                    else if (m_StateBase.state == e_RecordStateStage.Special)
                        ShowDebug("上传中");
                }

                break;

            case e_RecordStateControl.Play:
                if (m_StateBase == null)
                    m_CurrentState = e_RecordStateControl.Play;
                else if (m_StateBase.recordState == e_RecordStateControl.Init || m_StateBase.recordState == e_RecordStateControl.Logout)
                {
                    if (CheckStateOver(e_RecordStateControl.Init) || CheckStateOver(e_RecordStateControl.Logout))
                        m_CurrentState = e_RecordStateControl.Login;
                }
                else if (m_StateBase.recordState == e_RecordStateControl.Login)
                {
                    if (CheckCurStateIsOver())
                        m_CurrentState = e_RecordStateControl.Play;
                    else if (m_StateBase.state == e_RecordStateStage.Erro)
                        m_ChangeToState = e_RecordStateControl.Logout;
                }
                else if (m_StateBase.recordState == e_RecordStateControl.Record)
                {
                    if (CheckCurStateIsOver())
                        m_CurrentState = e_RecordStateControl.Play;
                    else if (m_StateBase.state == e_RecordStateStage.Erro)
                        m_ChangeToState = e_RecordStateControl.Logout;
                }
                else if(m_StateBase.recordState == e_RecordStateControl.Play)
                {
                    if(CheckCurStateIsOver())
                        m_CurrentState = e_RecordStateControl.Play;
                    else if (m_StateBase.state == e_RecordStateStage.Erro)
                        m_ChangeToState = e_RecordStateControl.Logout;
                }
                break;

            case e_RecordStateControl.Logout:
                if (m_StateBase == null)
                    m_CurrentState = e_RecordStateControl.Logout;
                else if (m_StateBase.recordState == e_RecordStateControl.Play)
                {
                    if (!CheckCurStateIsOver())
                    {
                        RecordPlay rp = m_StateBase as RecordPlay;
                        if (rp != null)
                            rp.StopPlay();
                    }
                    else
                        m_CurrentState = e_RecordStateControl.Logout;
                }
                else if (m_StateBase.recordState == e_RecordStateControl.Record)
                {
                    if (!CheckCurStateIsOver())
                    {
                        RecordRecord rp = m_StateBase as RecordRecord;
                        if (rp != null)
                            rp.Stop(false);
                    }
                    else
                        m_CurrentState = e_RecordStateControl.Logout;
                }
                else if (m_StateBase.recordState != e_RecordStateControl.Logout && CheckCurStateIsOver())
                {
                    m_CurrentState = e_RecordStateControl.Logout;
                }
                break;
        }

        switch (m_CurrentState)
        {
            case e_RecordStateControl.Init:
                if (!m_Dict_State.ContainsKey(e_RecordStateControl.Init))
                    m_Dict_State[e_RecordStateControl.Init] = new RecordInit();

                m_StateBase = m_Dict_State[e_RecordStateControl.Init];
                m_StateBase.InitState(new object[] { appKey });
                GameLink.AddObserver(this);
                break;
            case e_RecordStateControl.Login:
                if (!m_Dict_State.ContainsKey(e_RecordStateControl.Login))
                    m_Dict_State[e_RecordStateControl.Login] = new RecordLogin();

                m_StateBase = m_Dict_State[e_RecordStateControl.Login];
                m_StateBase.InitState(new object[] { name, this.data.uname, this.data.channelName });
                break;
            case e_RecordStateControl.Record:
                if (!m_Dict_State.ContainsKey(e_RecordStateControl.Record))
                    m_Dict_State[e_RecordStateControl.Record] = new RecordRecord();

                m_StateBase = m_Dict_State[e_RecordStateControl.Record];
                m_StateBase.InitState();
                break;
            case e_RecordStateControl.Play:
                if (!m_Dict_State.ContainsKey(e_RecordStateControl.Play))
                    m_Dict_State[e_RecordStateControl.Play] = new RecordPlay();

                m_StateBase = m_Dict_State[e_RecordStateControl.Play];
                m_StateBase.InitState();
                break;
            case e_RecordStateControl.Logout:
                if (!m_Dict_State.ContainsKey(e_RecordStateControl.Logout))
                    m_Dict_State[e_RecordStateControl.Logout] = new RecordLogout();

                m_StateBase = m_Dict_State[e_RecordStateControl.Logout];
                m_StateBase.InitState();
                break;
        }

        if (this.DL_RECORDING_VOICE_CHANGE != null && m_StateBase.recordState == e_RecordStateControl.Record && m_StateBase.state == e_RecordStateStage.Other)
        {
            GameLink.GetRecordingState(ref this.data.recording_volume, ref this.data.recording_duration);
            this.DL_RECORDING_VOICE_CHANGE(this.data.recording_volume, this.data.recording_duration);
        }
        return;

#if UNITY_EDITOR || UNITY_STANDALONE_WIN
        return;
#endif
        //临时使用的，用来绕过PlayStop偶尔没有执行到的方法
        if (m_IsPlaying || m_IsPlayingMyRecord)
        {
            m_PlayerWaitTime -= Time.deltaTime;
            if (m_PlayerWaitTime <= 0)
            {
                //m_PlayerWaitTime = m_LimitTime;
                //if (m_IsPlaying && CurrentMessage != null)
                //    OnPlayStop(CurrentMessage);
                //else if (m_IsPlayingMyRecord)
                //    OnPlayStop("");
            }
        }

        this.playMessage();

        if (m_IsRecord && this.DL_RECORDING_VOICE_CHANGE != null)
        {
            GameLink.GetRecordingState(ref this.data.recording_volume, ref this.data.recording_duration);
            this.DL_RECORDING_VOICE_CHANGE(this.data.recording_volume, this.data.recording_duration);
        }
    }

    void playMessage()
    {

#if UNITY_EDITOR || UNITY_STANDALONE_WIN
        return;
#endif
        if (!string.IsNullOrEmpty(this.data.rfile) && (m_StateBase.state == e_RecordStateStage.End || m_StateBase.state == e_RecordStateStage.Erro))
        {
            m_CurrentState = e_RecordStateControl.Play;
        }
        return;
        if (!string.IsNullOrEmpty(this.data.rfile) && !m_IsPlayingMyRecord && !m_IsWaitUpOver && !m_IsClickRecord && !m_IsRecord)
        {
            if (m_IsPlaying)
            {
                this.ShowDebug("播放前先停止");
                GameLink.StopPlay();
            }
            else
            {
                this.ShowDebug("播放我的录音");
                m_IsPlayingMyRecord = true;
                m_IsInitPlay = true;
                GameLink.PlayVoice(this.data.rfile);
            }
        }

        if (this.data.isPlaying || m_IsPlayingMyRecord || m_IsRecord || m_IsWaitUpOver || m_IsClickRecord)
        {
            return;
        }

        if (this.data.messagesList.Count <= 0)
        {
            return;
        }

        this.data.isPlaying = true;
        var v = this.data.messagesList[0];
        m_IsInitPlay = true;
        GameLink.PlayVoice(v);
        this.data.messagesList.RemoveAt(0);
    }

    #endregion

    #region 登出相关
    /// <summary>
    /// 外部调用登出
    /// </summary>
    public void LogoutRecord()
    {
#if UNITY_EDITOR || UNITY_STANDALONE_WIN
        return;
#endif
        m_ChangeToState = e_RecordStateControl.Logout;
        this.ShowDebug("调用登出");

        return;
        GameLink.Logout();
    }

    //登出回调
    //void OnLogout(GLError error)
    //{
    //    if (error == null)
    //    {
    //        this.ShowDebug("登出成功");
    //        if (m_Coroutine != null)
    //        {
    //            StopCoroutine(m_Coroutine);
    //            m_Coroutine = null;
    //        }
    //        GameLink.StopPlay();
    //        GameLink.StopRecord();
    //        this.data.messagesList.Clear();
    //    }
    //    else
    //    {
    //        this.ShowDebug("登出失败");

    //        this.data.rfile = "";
    //        if (m_Coroutine != null)
    //        {
    //            StopCoroutine(m_Coroutine);
    //            m_Coroutine = null;
    //        }

    //        if (this.DL_IS_PLAYING != null)
    //        {
    //            this.DL_IS_PLAYING(this.data.uname, false);
    //            if (CurrentMessage != null)
    //                this.DL_IS_PLAYING(CurrentMessage.Sender.Account, false);
    //        }

    //        if (this.DL_IS_RECODING != null)
    //        {
    //            this.DL_IS_RECODING(false);
    //        }
    //    }

    //    this.m_IsLogin = false;
    //    this.m_IsRecord = false;
    //    this.m_IsPlaying = false;
    //    this.m_IsPlayingMyRecord = false;
    //    this.m_IsWaitRecord = false;
    //    this.m_IsWaitUpOver = false;
    //    this.m_IsWaitStopOver = false;
    //    this.m_IsClickStop = false;
    //    this.m_UrgencyStop = false;
    //    this.m_IsClickRecord = false;
    //    this.m_IsInitPlay = false;
    //}
    #endregion

    string m_lastMsg = "";
    #region Denig
    public void ShowDebug(string msg, GLError erro = null)
    {
        if (m_lastMsg == msg)
        {
            return;
        }
        m_lastMsg = msg;

        if (erro != null)
        {
            setMsg(string.Format("亲加SDKLog：{0}    [FF0000]erro:{1}", msg, erro.Code.ToString()));
            QLoger.ERROR(msg);
        }
        else
            setMsg(string.Format("[00FF00]{0}", msg));
    }

    void setMsg(string msg)
    {
        QLoger.LOG(msg);
        UserActionManager.AddLocalLog("Log3", msg);
        this.msg += ">" + msg + "   ";
    }
    string msg = "";

#if __DEBUG
    void OnGUI()
    {
        if (false)
        {
            GUI.TextArea(new Rect(10, 50, Screen.width - 500, Screen.height / 5), msg);
        }
    }
#endif
#endregion

}

public enum e_RecordStateControl
{
    None,
    Init,           //初始化状态
    Login,          //登陆状态
    Record,         //录音状态
    Play,           //播放状态
    Logout,         //登出状态
}

public enum e_RecordStateStage
{
    None,
    Start,
    Other,
    Special,
    End,
    Erro,
}

public abstract class RecordStateBase
{
    protected e_RecordStateControl m_RecordState;
    public e_RecordStateControl recordState
    {
        get
        {
            return m_RecordState;
        }
    }

    protected e_RecordStateStage m_State = e_RecordStateStage.None;
    public e_RecordStateStage state
    {
        get
        {
            return m_State;
        }
    }

    public abstract void InitState(object[] value = null);
}

public class RecordInit: RecordStateBase
{
    public RecordInit()
    {
        m_RecordState = e_RecordStateControl.Init;
    }

    public override void InitState(object[] value = null)
    {
#if UNITY_EDITOR || UNITY_STANDALONE_WIN
        return;
#endif
        string appKey = "";
        if (value != null)
            appKey = (string)value[0];

        RecordManager.Instance.ShowDebug("Init      " + appKey);
        m_State = e_RecordStateStage.Start;

        bool isInitOk = GameLink.Init(appKey);
        if (isInitOk)
        {
            GameLink.EnableLog(true);
            GameLink.ClearCache();
            GameLink.EnableVoiceRecognition(true);
            //GameLink.AddObserver(this);
            m_State = e_RecordStateStage.End;
        }
        else
        {
            RecordManager.Instance.ShowDebug("InitLose");
            QLoger.ERROR("语音SDK初始化失败");
            m_State = e_RecordStateStage.Erro;
        }
    }

}

public class RecordLogin: RecordStateBase
{
    private string m_ChannelName = "";

    public RecordLogin()
    {
        m_RecordState = e_RecordStateControl.Login;
        GameLink.AddObserver(this);
    }

    public override void InitState(object[] value = null)
    {
#if UNITY_EDITOR || UNITY_STANDALONE_WIN
        return;
#endif
        if (value.Length < 3 || m_State == e_RecordStateStage.Start)
            return;
        m_State = e_RecordStateStage.Start;
        string name = (string)value[0];
        string uname = (string)value[1];
        m_ChannelName = (string)value[2];


        RecordManager.Instance.ShowDebug("RecordLogin");
        GameLink.SetProfile(name, "Avatar", "ExtraInfo");
        GameLink.Login(uname);
    }

    void OnLogin(GLTarget target, GLError error)
    {
        RecordManager.Instance.ShowDebug("OnLogin");
        if (error == null || error.Code == GLErrorCode.OnlineOrLogining)
        {
            JoinChannel();
        }
        else
        {
            RecordManager.Instance.ShowDebug("OnLoginErro ", error);
            m_State = e_RecordStateStage.Erro;
        }
    }

    //加入频道
    private void JoinChannel()
    {
        GameLink.JoinChannel(m_ChannelName);
    }

    void OnJoinChannel(GLTarget channel, GLError error)
    {
        RecordManager.Instance.ShowDebug("OnJoinChannel");
        if (error == null)
        {
            RecordManager.Instance.data.gTarget = channel;
            m_State = e_RecordStateStage.End;
        }
        else
        {
            RecordManager.Instance.ShowDebug("OnJoinChannelErro");
            m_State = e_RecordStateStage.Erro;
        }
    }

    private void RemoveObserver()
    {
        GameLink.RemoveObserver(this);
    }
}

public class RecordRecord : RecordStateBase
{
    private bool m_isNeedUpload = false;


    public RecordRecord()
    {
        m_RecordState = e_RecordStateControl.Record;
        GameLink.AddObserver(this);
    }

    public override void InitState(object[] value = null)
    {
#if UNITY_EDITOR || UNITY_STANDALONE_WIN
        return;
#endif
        if (m_State == e_RecordStateStage.Special)
        {
            RecordManager.Instance.ShowDebug("正在停止录音");
            return;
        }

        RecordManager.Instance.ShowDebug("Record");

        m_State = e_RecordStateStage.Start;
        m_isNeedUpload = true;
        GameLink.StartRecord();
    }
    
    void OnStartRecord(GLError error)
    {
        RecordManager.Instance.ShowDebug("OnStartRecord");
        if (error == null)
        {
            if (m_State == e_RecordStateStage.Special)
            {
                Stop();
            }
            else
            {
                m_State = e_RecordStateStage.Other;
                if (RecordManager.Instance.DL_IS_RECODING != null)
                {
                    RecordManager.Instance.DL_IS_RECODING(true);
                }
            }
        }
        else
        {
            RecordManager.Instance.ShowDebug("Record", error);
            if (RecordManager.Instance.DL_IS_RECODING != null)
            {
                RecordManager.Instance.DL_ERROR(error.Code.ToString());
            }
            m_isNeedUpload = false;
            m_State = e_RecordStateStage.Erro;
        }
    }

    public void Stop(bool isUpload = true)
    {
        if(m_State == e_RecordStateStage.Erro || m_State == e_RecordStateStage.End)
        {
            RecordManager.Instance.ShowDebug("StopErro");
            return;
        }

        m_isNeedUpload = isUpload;
        m_State = e_RecordStateStage.Special;
        GameLink.StopRecord();
    }


    //停止录音回调
    void OnStopRecord(String file, String voiceContent, uint duration, GLError error)
    {
        RecordManager.Instance.ShowDebug("OnStopRecord");
        if (null == error)
        {
            if (RecordManager.Instance.DL_IS_RECODING != null)
            {
                RecordManager.Instance.DL_IS_RECODING(false);
            }

            if (m_isNeedUpload)
            {
                RecordManager.Instance.data.rfile = file;
                RecordManager.Instance.data.duration = duration;
                RecordManager.Instance.data.voiceContent = voiceContent;
                RecordManager.Instance.data.text = "";

                if (duration > 30000 || duration < 950)
                {
                    if (RecordManager.Instance.DL_RECORDING_LENGTH_ERROR != null)
                    {
                        RecordManager.Instance.DL_RECORDING_LENGTH_ERROR(duration < 950, duration);
                    }

                    RecordManager.Instance.data.rfile = "";

                    m_State = e_RecordStateStage.End;
                    return;
                }
                else
                {
                    m_State = e_RecordStateStage.Special;
                    GameLink.UploadVoice(RecordManager.Instance.data.rfile);
                }
            }
            else
            {
                m_State = e_RecordStateStage.End;
                return;
            }
        }
        else
        {
            m_State = e_RecordStateStage.Erro;
            if (RecordManager.Instance.DL_IS_RECODING != null)
            {
                RecordManager.Instance.DL_IS_RECODING(false);
            }
            if (RecordManager.Instance.DL_ERROR != null)
            {
                RecordManager.Instance.DL_ERROR(error.Code.ToString());
            }
        }
    }
    
    //上传录音中
    void OnUploadVoice(System.String file, System.String url, GLError error)
    {
        RecordManager.Instance.ShowDebug("OnUploadVoice");
        if (null == error)
        {
            GameLink.SendVoice(RecordManager.Instance.data.gTarget, url, RecordManager.Instance.data.duration,
                RecordManager.Instance.data.text, RecordManager.Instance.data.voiceContent);

            m_State = e_RecordStateStage.End;
        }
        else
        {
            RecordManager.Instance.ShowDebug("OnUploadVoice",error);
            m_State = e_RecordStateStage.Erro;
            if (RecordManager.Instance.DL_ERROR != null)
            {
                RecordManager.Instance.DL_ERROR(error.Code.ToString());
            }
        }

        if (RecordManager.Instance.DL_IS_RECODING != null)
        {
            RecordManager.Instance.DL_IS_RECODING(false);
        }
    }


    private void RemoveObserver()
    {
        GameLink.RemoveObserver(this);
    }
}

public class RecordPlay : RecordStateBase
{
    public RecordPlay()
    {
        m_RecordState = e_RecordStateControl.Play;
        GameLink.AddObserver(this);
    }

    public override void InitState(object[] value = null)
    {
#if UNITY_EDITOR || UNITY_STANDALONE_WIN
        return;
#endif
        if (!string.IsNullOrEmpty(RecordManager.Instance.data.rfile) && m_State!= e_RecordStateStage.Start)
        {
            RecordManager.Instance.ShowDebug("RecordPlay");
            m_State = e_RecordStateStage.Start;
            GameLink.PlayVoice(RecordManager.Instance.data.rfile);
        }
        else if(RecordManager.Instance.data.messagesList.Count > 0 && m_State != e_RecordStateStage.Start)
        {
            RecordManager.Instance.ShowDebug("RecordPlay");
            m_State = e_RecordStateStage.Start;
            var v = RecordManager.Instance.data.messagesList[0];
            GameLink.PlayVoice(v);
            RecordManager.Instance.data.messagesList.RemoveAt(0);
        }
        else
        {
            m_State = e_RecordStateStage.End;
        }
    }

    void OnPlayStart(GLMessage message, GLError error)
    {
        RecordManager.Instance.ShowDebug("OnPlayStart");
        if (error == null)
        {
            if (RecordManager.Instance.DL_IS_PLAYING != null)
            {
                RecordManager.Instance.DL_IS_PLAYING(message.Sender.Account, true);
            }
            RecordManager.Instance.CurrentMessage = message;
        }
        else
        {
            m_State = e_RecordStateStage.Erro;
            if (RecordManager.Instance.DL_ERROR != null)
            {
                RecordManager.Instance.DL_ERROR(error.Code.ToString());
            }
        }
    }

    void OnPlayStart(String file, GLError error)
    {
        RecordManager.Instance.ShowDebug("OnPlayStart");
        if (error == null)
        {
            if (RecordManager.Instance.DL_IS_PLAYING != null)
            {
                RecordManager.Instance.DL_IS_PLAYING(RecordManager.Instance.data.uname, true);
            }
        }
        else
        {
            m_State = e_RecordStateStage.Erro;
            if (RecordManager.Instance.DL_ERROR != null)
            {
                RecordManager.Instance.DL_ERROR(error.Code.ToString());
            }
        }
    }

    public void StopPlay()
    {
        if(m_State == e_RecordStateStage.Other)//m_State == e_RecordStateStage.End || m_State == e_RecordStateStage.Erro || 
        {
            return;
        }

        m_State = e_RecordStateStage.Other;
        GameLink.StopPlay();
    }

    void OnPlayStop(GLMessage message)
    {
        RecordManager.Instance.ShowDebug("OnPlayStop");
        m_State = e_RecordStateStage.End;
        if (RecordManager.Instance.DL_IS_PLAYING != null)
        {
            RecordManager.Instance.DL_IS_PLAYING(message.Sender.Account, false);
        }
        RecordManager.Instance.CurrentMessage = null;
        RecordManager.Instance.data.rfile = "";
    }

    void OnPlayStop(String file)
    {
        RecordManager.Instance.ShowDebug("OnPlayStop");
        m_State = e_RecordStateStage.End;
        if (RecordManager.Instance.DL_IS_PLAYING != null)
        {
            RecordManager.Instance.DL_IS_PLAYING(RecordManager.Instance.data.uname, false);
        }
        RecordManager.Instance.CurrentMessage = null;
        RecordManager.Instance.data.rfile = "";
    }

    private void RemoveObserver()
    {
        GameLink.RemoveObserver(this);
    }
}

public class RecordLogout : RecordStateBase
{
    public RecordLogout()
    {
        m_RecordState = e_RecordStateControl.Logout;
        GameLink.AddObserver(this);
    }

    public override void InitState(object[] value = null)
    {
#if UNITY_EDITOR || UNITY_STANDALONE_WIN
        return;
#endif
        m_State = e_RecordStateStage.Start;
        GameLink.Logout();
    }

    //登出回调
    void OnLogout(GLError error)
    {
        RecordManager.Instance.ShowDebug("OnLogout");
        if (error == null)
        {
            m_State = e_RecordStateStage.End;
            GameLink.StopPlay();
            GameLink.StopRecord();
            RecordManager.Instance.data.messagesList.Clear();
        }
        else
        {
            m_State = e_RecordStateStage.Erro;
            RecordManager.Instance.data.rfile = "";

            if (RecordManager.Instance.DL_IS_PLAYING != null)
            {
                RecordManager.Instance.DL_IS_PLAYING(RecordManager.Instance.data.uname, false);
                if (RecordManager.Instance.CurrentMessage != null)
                    RecordManager.Instance.DL_IS_PLAYING(RecordManager.Instance.CurrentMessage.Sender.Account, false);
            }

            if (RecordManager.Instance.DL_IS_RECODING != null)
            {
                RecordManager.Instance.DL_IS_RECODING(false);
            }
        }
    }

    private void RemoveObserver()
    {
        GameLink.RemoveObserver(this);
    }
}
