/**
 * @Author YQC
 *
 *
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace projectQ
{
    //游戏状态数据
    public class SysGameStateData {
        //UI所在标记
        public enum EnumUISign
        {
            None = 0,
            Login,
            //登录前============================
            LoginSucceed,
            MainIn,

            PrepareGame_NotOpenDesk,
            PrepareGame,//
            //游戏中============================
            GameIn,             //游戏中
        }

        #region 当前游戏标记
        private EnumUISign _currUISign = EnumUISign.None;
        public EnumUISign CurrUISign
        {
            private set
            {
                if(_currUISign != value || value == EnumUISign.LoginSucceed)
                {
                    _currUISign = value;
                    QLoger.LOG("当前UI标记(CurrUISign):"+ _currUISign.ToString());
                    EventDispatcher.FireSysEvent(GEnum.NamedEvent.SysGame_UI_Sign_Update);
                }
            }
            get { return _currUISign; }
        }
        public void SetCurrUISign(EnumUISign uiSign)
        {
            UserActionManager.AddLocalTypeLog("Log1", "游戏状态变更 原"+ CurrUISign.ToString()+" 变更为:"+ uiSign.ToString());
            QLoger.LOG(CurrUISign.ToString() + "__" + uiSign.ToString());
            CurrUISign = uiSign;
            if(CurrUISign < EnumUISign.LoginSucceed)
            {
                CurrMjRoomId = 0;
            }
        }
        #endregion

        #region 当前麻将馆ID
        private int _currRoomId = 0;
        public int CurrMjRoomId
        {
            set
            {
                if (_currRoomId != value)
                {
                    _currRoomId = value;
                    EventDispatcher.FireSysEvent(GEnum.NamedEvent.SysGame_RoomId_Update);
                }
            }
            get { return _currRoomId; }
        }
        #endregion

        #region 当前断线重连状态
        public enum EnumNetReconnectState
        {
            None,
            Hide,//隐藏式连接
            Kick,//被踢下线
        }

        /// <summary>
        /// 当前重连状态
        /// </summary>
        public EnumNetReconnectState CurrReconnectState = EnumNetReconnectState.None;

        public int HideReconnectCount = 1;

        /// <summary>
        /// 重连次数
        /// </summary>
        public int ReconnectCount = 0;

        /// <summary>
        /// 是否是主动断开
        /// </summary>
        private bool _isInitiativeCloseConnect = false;
        public bool IsInitiativeCloseConnect
        {
            set {
                DebugPro.Log(DebugPro.EnumLog.NetWork, "Set 是否是主动断开 原值" , _isInitiativeCloseConnect.ToString(), value.ToString());
                _isInitiativeCloseConnect = value;
            }
            get {
                DebugPro.Log(DebugPro.EnumLog.NetWork, "Get 是否是主动断开", _isInitiativeCloseConnect.ToString());
                return _isInitiativeCloseConnect;
            }
        }

        #endregion

        #region 是否在麻将中
        //private bool _isReconectMahjongGameIn = false;
        //public bool IsReconectMahjongGameIn
        //{
        //    set { _isReconectMahjongGameIn = value; }
        //    get { return _isReconectMahjongGameIn; }
        //}
        private bool _isMahjongGameIn = false;
        //是否在麻将游戏中
        public bool IsMahjongGameIn
        {
            set
            {
                if(_isMahjongGameIn != value)
                {
                    _isMahjongGameIn = value;
                    EventDispatcher.FireSysEvent(GEnum.NamedEvent.SysGame_MahjongGameIn_Update);
                }
            }
            get { return _isMahjongGameIn; }
        }

        public bool IsJoinMahjongSceneNew = false;
        public bool IsLoadMahjongScene = false;
        #endregion

        #region 当前用户状态
        public bool IsExternalLinkJoinDesk = false;

        /// <summary>
        /// 是否是微信分享邀请的好友打牌进入的游戏
        /// </summary>
        public bool IsWxShareInvitePlay = false;
        /// <summary>
        /// 当前牌桌的桌主关联的麻将馆ID
        /// </summary>
        public int JoinDeskRoomId;
        #endregion

        #region Loading是否开启状态
        private bool _bigLoadingActive = false;
        private bool _smallLoadingActive = false;

        public bool BigLoadingActive
        {
            set{ _bigLoadingActive = value; }
            get{ return _bigLoadingActive; }
        }
        public bool SmallLoadingActive
        {
            set { _smallLoadingActive = value; }
            get { return _smallLoadingActive; }
        }
        public bool LoadingActive
        {
            get { return BigLoadingActive || SmallLoadingActive; }
        }
        #endregion

        #region 游戏是否在后台
        private bool m_IsPause = false;
        public bool IsPause
        {
            get
            {
                return m_IsPause;
            }
            set
            {
                m_IsPause = value;
                EventDispatcher.FireEvent(GEnum.NamedEvent.GameStateChange);
            }
        }
        #endregion

        #region 当前网络状态
        /// <summary>
        /// 游戏网络状态 (游戏逻辑层)
        /// </summary>
        public enum EnumGameNetWorkStatus
        {
            NotLink,
            InitLink,
            ReconnectLink,
            LinkOk,
        }
        private EnumGameNetWorkStatus _currGameNetWorkStatus = EnumGameNetWorkStatus.NotLink;
        public EnumGameNetWorkStatus CurrGameNetWorkStatus
        {
            set {
                UserActionManager.AddLocalTypeLog("Log1", "游戏网络状态变更 原:" + _currGameNetWorkStatus.ToString() + " 变更为:" + value.ToString());
                if (_currGameNetWorkStatus != value)
                {
                    _currGameNetWorkStatus = value;
                }
            }
            get { return _currGameNetWorkStatus; }
        }
        #endregion

        #region WebView
        public bool IsOpenWebView = false;
        #endregion
    }


    #region 内存数据
    public partial class MKey
    {
        public const string SYS_GAMESTATE_DATA = "SYS_GAMESTATE_DATA";
    }

    public partial class MemoryData
    {
        static public SysGameStateData GameStateData
        {
            get
            {
                SysGameStateData itemData = MemoryData.Get<SysGameStateData>(MKey.SYS_GAMESTATE_DATA);
                if (itemData == null)
                {
                    itemData = new SysGameStateData();
                    MemoryData.Set(MKey.SYS_GAMESTATE_DATA, itemData);
                }
                return itemData;
            }
        }
    }
    #endregion
}