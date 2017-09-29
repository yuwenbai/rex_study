using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using LitJson;
namespace projectQ {
    public partial class MKey {
        ////游戏运行状态 0未登录 1已登录 
        //public const string GAME_RUN_STATE = "GAME_RUN_STATE";
        //断线重连 int 仅1次之后 踢回
        //public const string GAME_RECONNECT_COUNT = "GAME_RECONNECT_COUNT";
        //游戏配置属性
        public const string GMAE1_EACH_PLAY_TIME = "GMAE1_EACH_PLAY_TIME";
        public const string GMAE1_EACH_STEP_SIZE = "GMAE1_EACH_STEP_SIZE";

        //用户属性
        public const string USER_LOGIN_STATE = "USER_LOGIN_STATE";

        public const string USER_DATA = "USER_DATA";        //用户数据

        //public const string USER_CURR_DESK_ID = "USER_CURR_DESK_ID";          //当前牌桌ID

        public const string USER_INVITE_CODE = "USER_INVITE_CODE";

        //游戏数据
        //public const string GAME_SELECT_ROUND = "GAME_SELECT_ROUND";

        //登陆服务器自动登陆用户名密码
        //public const string LS_AUTO_USERNAME = "LS_AUTO_USERNAME";
        //public const string LS_AUTO_PWD = "LS_AUTO_PWD";
        //public const string LS_AUTO_LOGIN_TYPE = "LS_AUTO_LOGIN_TYPE";

        //网关服务器数据
        public const string AS_TOKEN = "LOGIN_TOKEN";
        public const string AS_IPADDR = "LOGIN_IPADDR";
        public const string AS_PORT = "LOGIN_PORT";
        public const string AS_SERVER_ID = "AS_SERVER_ID";
        public const string AS_SERVER_TYPE = "AS_SERVER_TYPE";


        //额外的数据

        /// <summary>
        /// 大奖赛数据
        /// </summary>
        public const string GRAND_PRIX_DATA = "GRAND_PRIX_DATA";

        /// <summary>
        /// 走马灯数据
        /// </summary>
        public const string LOOPING_MSG_DATA = "LOOPING_MSG_DATA";

        /// <summary>
        /// 用户其他数据
        /// </summary>
        public const string USER_GAME_DATA = "USER_GAME_DATA";
        public const string USER_SIGN_DAY = "USER_SIGN_DAY";        //累计签到天数
        public const string USER_IS_SIGNED = "USER_IS_SIGNED";
        public const string USER_GAME_STATE = "USER_GAME_STATE";    //用户状态数据

        /// <summary>
        /// 麻将馆数据
        /// </summary>
        public const string MJ_HALL_DATA = "MJ_HALL_DATA";

        /// <summary>
        /// 地图数据
        /// </summary>
        public const string MJ_AREA_DATA = "MJ_AREA_DATA";

        /// <summary>
        /// 好友数据
        /// </summary>
        public const string USER_FRIEND = "USER_FRIEND";

        /// <summary>
        /// 其他用户数据
        /// </summary>
        public const string USER_OTHER_USER = "USER_OTHER_USER";

        /// <summary>
        /// 自己的用户数据
        /// </summary>
        //public const string USER_MY_USER = "USER_MY_USER";

        /// <summary>
        /// 登录的OpenId
        /// </summary>
        //public const string LOGIN_OPEND_ID = "LOGIN_OPEND_ID";

        /// <summary>
        /// 防沉迷系统时间
        /// </summary>
        public const string USER_WALLOW_TIME = "USER_WALLOW_TIME";
    }


    public partial class MemoryData : SingletonTamplate<MemoryData> {


        static public bool IS_OFFLINE_GAME = false;
		static private readonly string[] REMAIN_DATA = new string[] {
			MKey.USER_XML_DATA 
            ,MKey.SYS_INIT_DATA
            ,MKey.MJ_PLAY_DATA_XML
            ,MKey.SYS_LOGIN_DATA
			,MKey.NAMED_GAMEDATA_TIME_COUNT
            ,MKey.SYS_GAMESTATE_DATA
        };

        public MemoryData() {
            m_data = new Dictionary<string, object>();
            m_attributChangeAction = changeNotfy;


			//初始化默认存档
			int optionValue = 0;
			optionValue = PlayerPrefsTools.GetInt("OPTION_MUSIC");
			if (optionValue == 0) {
				PlayerPrefsTools.SetInt("OPTION_MUSIC",2);
			}

			optionValue = PlayerPrefsTools.GetInt("OPTION_EFFECT");
			if (optionValue == 0) {
				PlayerPrefsTools.SetInt("OPTION_EFFECT",2);
			}

			optionValue = PlayerPrefsTools.GetInt("OPTION_SINGLE");
			if (optionValue == 0) {
				PlayerPrefsTools.SetInt("OPTION_SINGLE",2);
			}

			optionValue = PlayerPrefsTools.GetInt("OPTION_LANGUAGE");
			if (optionValue == 0) {
				PlayerPrefsTools.SetInt("OPTION_LANGUAGE",2);
			}
        }

        private System.Action<string> m_attributChangeAction;

        private IDictionary<string, object> m_data;


        public object this[string index] {
            get {
                object oj = null;
                if (!m_data.TryGetValue(index, out oj)) {
                    //QLoger.ERROR("获取属性出错，没有{0}属性存在，返回NULL",index);
                }
                return oj;
            }
            set { m_data[index] = value; }
        }

        public void changeNotfy(string name) {

        }

        static public void SetBool(string key, bool v) {
            Instance[key] = v;
        }

        static public bool GetBool(string key) {
            if (Instance.m_data.ContainsKey(key)) {
                return Get<bool>(key);
            } else {
                return false;
            }
        }

        static public T Get<T>(string key) {
            try {
                return (T)Instance[key];
            } catch (System.Exception ex) {
                //QLoger.ERROR("获取内存数据出错，无法获取类型{0}的数据(类型不匹配:{1})",typeof(T),ex);
            }
            return default(T);
        }

        static public void Set(string k, object v) {

            if (k == null)
                return;

            bool change = !Instance.m_data.ContainsKey(k);

            if (!change) {
                if(Instance[k] == null)
                {
                    change = v != null;
                }
                else
                {
                    change = !Instance[k].Equals(v);
                }
            }
            Instance[k] = v;
            if (change) {
                Instance.m_attributChangeAction(k);
            }
        }
        ///// <summary>
        ///// 游戏UI标记 当前在哪个界面 阶段
        ///// </summary>
        //static public string GameUISign
        //{
        //    set
        //    {
        //        if (value != GameUISign)
        //        {
        //            MemoryData.Set(MKey.GAME_UI_SIGN, value);
        //            EventDispatcher.FireSysEvent(GEnum.NamedEvent.SysGame_UI_Sign_Update);
        //        }
        //    }
                    
        //    get { return MemoryData.Get<string>(MKey.GAME_UI_SIGN); }
        //}

       

        static public SysMjRoomData MjHallData
        {
            get
            {
                SysMjRoomData data = MemoryData.Get<SysMjRoomData>(MKey.MJ_HALL_DATA);
                if (data == null)
                {
                    data = new SysMjRoomData();
                    MemoryData.Set(MKey.MJ_HALL_DATA, data);
                }
                return data;
            }
        }
        static public SysAreaData AreaData
        {
            get
            {
                SysAreaData data = MemoryData.Get<SysAreaData>(MKey.MJ_AREA_DATA);
                if (data == null)
                {
                    data = new SysAreaData();
                    MemoryData.Set(MKey.MJ_AREA_DATA, data);
                }
                return data;
            }
        }

        static public void RegiestAttributChangeAction(System.Action<string> action) {
            Instance.m_attributChangeAction += action;
        }

        static public void UnRegiestAttributChangeAction(System.Action<string> action) {
            Instance.m_attributChangeAction -= action;
        }

		static private void Init(string[] key , object[] vars) {
			for (int i = 0; i < key.Length; i++) {
				MemoryData.Set (key [i], vars [i]);
			}
        }

        static public void Reset() {
			object[] vars = new object[REMAIN_DATA.Length];
			for (int i = 0; i < REMAIN_DATA.Length; i++) {
				vars[i] = MemoryData.Get<object>(REMAIN_DATA[i]);
			}
            
			MemoryData.Instance.m_data.Clear();
			MemoryData.Init(REMAIN_DATA , vars);
        }


        static public UserGameState UserGameState {
            get {
                var data = MemoryData.Get<UserGameState>(MKey.USER_GAME_STATE);
                if (data == null) {
                    data = new UserGameState();
                    MemoryData.Set(MKey.USER_GAME_STATE, data);
                }

                return data ;
            }
        }


        static public long UserID
        {
            get { return MemoryData.PlayerData.MyUserID; }
        }


        static public SysFriendData FriendData {
            get { 
                SysFriendData data = MemoryData.Get<SysFriendData>(MKey.USER_FRIEND);
                if (data == null) {
                    data = new SysFriendData ();
                    MemoryData.Set(MKey.USER_FRIEND, data);
                }
                return data;
            }
        }

        static public float GameWidth;
        static public float GameHeigh;
        public static void SetGameProperty(float heigh, float width)
        {
            GameWidth = width;
            GameHeigh = heigh;
        }

        
       
    
    }

    /// <summary>
    /// 服务器用户状态
    /// </summary>
    public class UserGameState {

        /// <summary>
        /// 1 非法状态
        /// 2 大厅
        /// 3 游戏牌桌
        /// </summary>
        private int _mainState;
        private int _perMainStat;

        public int PerState {
            get {
                return _perMainStat;
            }
        }

        public int MainState {
            get { return _mainState; }
            set {
                if (_mainState != value) {
                    _perMainStat = _mainState;
                    _mainState = value;
                }
            }
        }
    }


}

