/****************************************************
 *  Copyright(C) 2015 智幻点击
 *	版权所有。
 *	作者:CEPH
 *	创建时间：11/30/2015
 *	文件名：  NewBehaviourScript.cs @ herocraft151104
 *	文件功能描述：
 *  创建标识：ceph.11/30/2015
 *	创建描述：
 *
 *  修改标识：
 *  修改描述：
 *
 *****************************************************/

using UnityEngine;
using System.Collections;


public class FPS : MonoBehaviour {

    public UILabel _fps_lable;
    string _info = "";

    string info {
        get { return _info; }
        set { bool dirty = !(_info.Equals(value));
            _info = value;
            if (dirty) { UpdateFps(); }
        }
    }
    float _lower_fps = 0;

	float _updateInterval = 1f;//设定更新帧率的时间间隔为1秒  
	float _accum = .0f;//累积时间  
	int _frames = 0;//在_updateInterval时间内运行了多少帧  
    
	float _timeLeft;
    float _lastRealTime;
	public static string GameInfo = "" ;

    private void Awake()
    {
#if !__DEBUG
        Destroy(_fps_lable.gameObject);
        this.enabled = false;
#endif
    }
    //RobotControl rc = null ;
    void Start () {
		_timeLeft = _updateInterval;

//#if __SWICH_OFF
        
//#else
//        rc = this.gameObject.AddComponent<RobotControl>();
//        rc.enabled = false;
//#endif

        stype = new GUIStyle();
        stype.fontSize = 28;
        stype.normal.textColor = Color.green;

        _lastRealTime = Time.realtimeSinceStartup;

        //this.AddNGUIFps();

    }
    
    GUIStyle stype;

#if USE_ONGUI
    void OnGUI(){


		GUI.Label(new Rect(0,Screen.height - 28 ,200,28),info + GameInfo, stype);
        if (!GameApplication.Instance.EnterBattle && rc != null) {
#if UNITY_EDITOR || NEED_DEBUG
            rc.enabled = true;
#endif
        } 
    }

#endif


    void AddNGUIFps() {
        GameObject go = new GameObject();
        go.name = "FPS";
        go.transform.parent = this.transform;
        UIPanel panel = go.AddComponent<UIPanel>();
        panel.depth = 9999;
        _fps_lable = go.AddComponent<UILabel>();
        _fps_lable.fontSize = 22;
        _fps_lable.text = "";
        _fps_lable.alignment = NGUIText.Alignment.Left;
        _fps_lable.transform.position = new Vector3(0, Screen.width - _fps_lable.fontSize + 1, 0);
    }

    void UpdateFps() {
        if (_fps_lable != null ) {
            if (this.info != null) {
                _fps_lable.text = this.info ;
            }
        }
    }

    // Update is called once per frame  
    void Update () {

#if UNITY_EDITOR || NEED_GM
        //rc.enabled = !rc.forbiden;
#endif

        float deltaTime = Time.realtimeSinceStartup - _lastRealTime;
        _lastRealTime = Time.realtimeSinceStartup;

        _timeLeft -= deltaTime;

        _accum += deltaTime;  
		++_frames;//帧数  

        if (deltaTime != 0) {
            float fps = 1 / deltaTime;
            if (fps < _lower_fps) {
                _lower_fps = fps;
            }
        }

        if (_timeLeft <= 0) {  
			float fps = (_frames / _accum );
            if (fps >= 20) {
                stype.normal.textColor = Color.green;
                if (_fps_lable != null) {
                    _fps_lable.color = Color.green;
                }
            } else {
                stype.normal.textColor = Color.red;
                if (_fps_lable != null) {
                    _fps_lable.color = Color.red;
                }
            }
			//this.info_per_time = System.String.Format("{0:F1}", fps);//保留1位小数  

            _timeLeft = _updateInterval;  
			_accum = .0f;  
			_frames = 0;

            this.info = System.String.Format("{0:F1}/{1:F1}/{2:F1}", fps, _lower_fps, (projectQ.PingManager.Instance.NetDelay * 1000f));
            _lower_fps = fps;
        }
    }  
}
