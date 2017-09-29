using System;
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
    public abstract class UIMainBase : MonoBehaviour
    {
        [HideInInspector]
        public UIMain ui;

        public virtual void Init(UIMain ui)
        {
            this.ui = ui;
        }

        public virtual void RefreshUI(UIMain.EnumUIMainState state)
        {
        }
    }

    public partial class UIMain : UIViewBase
    {
        private UIMainModel Model
        {
            get { return _model as UIMainModel; }
        }

        public enum EnumUIMainState
        {
            None,           //默认进入
            NormalMain,     //普通人的主页
            LinkMain,       //关联人的主页
            NotLinkMjHall,  //关联解除中的麻将馆页面
            LinkedMjHall,   //已关联的麻将馆页面
            Master,         //麻将馆管主的首页
            MasterCheck,    //麻将馆管主查看页面
        }

        public enum HallBehaviorState //麻将馆行为
        {
            None,
            Enter,  //进入麻将馆
            Leave,  //退出麻将馆
        }

        public EnumUIMainState CurrState = EnumUIMainState.None;
        public UIMainStateBase StateScript;

        public HallBehaviorState hallBehaState = HallBehaviorState.None;
        public int _roomId = 0;

        //当前麻将馆ID
        public int RoomId
        {
            set
            {
                if (_roomId != value)
                {
                    this.EnterMahjongHall(false);
                    _roomId = value;
                    this.EnterMahjongHall(true);
                }
            }

            get { return _roomId; }
        }

        public MjRoom CurrMjHalll;

        #region LeftTop
        public UIMainPlayInfo PlayInfoScript;
        #endregion

        #region Top
        public UIMainTop TopScript;
        #endregion

        #region Right
        public UIMainRight RightButtonScript;
        #endregion

        #region Down
        public UIMainDown DownScript;
        #endregion

        #region Center
        public UIMainCenter CenterScript;
        #endregion

        #region Other
        public UIMainReferral ReferralScript;
        public NGUIBroadcasts BroadcastsScript;
        #endregion

        public void ChangeState(EnumUIMainState state)
        {
            switch (state)
            {
                case EnumUIMainState.None:
                    UIMainStateBase.ChangeState(new UIMainStateNone(), this);
                    break;
                case EnumUIMainState.NormalMain:
                    UIMainStateBase.ChangeState(new UIMainStateNormalMain(), this);
                    break;
                case EnumUIMainState.LinkMain:
                    UIMainStateBase.ChangeState(new UIMainStateLinkMain(), this);
                    break;
                case EnumUIMainState.NotLinkMjHall:
                    UIMainStateBase.ChangeState(new UIMainStateNotLinkMjHall(), this);
                    break;
                case EnumUIMainState.LinkedMjHall:
                    UIMainStateBase.ChangeState(new UIMainStateLinkedMjHall(), this);
                    break;
                case EnumUIMainState.Master:
                    UIMainStateBase.ChangeState(new UIMainStateMaster(), this);
                    break;
                case EnumUIMainState.MasterCheck:
                    UIMainStateBase.ChangeState(new UIMainStateMasterCheck(), this);
                    break;
            }
        }

        public void RefreshUI(EnumUIMainState state)
        {
            if (state == EnumUIMainState.NormalMain || state == EnumUIMainState.LinkMain)
            {

                BroadcastsScript.Root.transform.localPosition = new Vector3(0.0f, -172.0f, 0.0f);
                BroadcastsScript.SetBroadcastShow(true);
                MusicCtrl.Instance.Music_BackChangePlay(GameAssetCache.M_Back_01_Path);
            }
            else
            {
                BroadcastsScript.Root.transform.localPosition = new Vector3(0.0f, -200.0f, 0.0f);
                BroadcastsScript.SetBroadcastShow(false);
                MusicCtrl.Instance.Music_BackChangePlay(GameAssetCache.M_Back_01_Path);
            }

            PlayInfoScript.RefreshUI(state);
            TopScript.RefreshUI(state);
            RightButtonScript.RefreshUI(state);
            CenterScript.RefreshUI(state);
            DownScript.RefreshUI(state);
            ReferralScript.RefreshUI(state);

            Model.RefreshBroadcasts();

            EventDispatcher.FireEvent(GEnum.NamedEvent.SysTools_FunctionShow);

            RightButtonScript.BtnDataGrid.Reposition();
        }

        /// <summary>
        /// 进入麻将馆
        /// </summary>
        public void EnterMahjongHall(bool isEnter)
        {
            if (isEnter)
            {
                if (this.hallBehaState != HallBehaviorState.Enter && this.RoomId != 0)
                {
                    MemoryData.GameStateData.CurrMjRoomId = this.RoomId;
                    ModelNetWorker.Instance.FMjRoomEnterReq(RoomId);
                    hallBehaState = HallBehaviorState.Enter;
                }
            }
            else
            {
                if (this.hallBehaState != HallBehaviorState.Leave && this.RoomId != 0)
                {
                    MemoryData.GameStateData.CurrMjRoomId = 0;
                    ModelNetWorker.Instance.FMjRoomLeaveReq(RoomId);
                    hallBehaState = HallBehaviorState.Leave;
                }
            }
        }

        #region override

        public override void OnPushData(object[] data)
        {
            if (data != null && data.Length > 0)
            {
                if (data.Length > 1)
                {
                    CurrMjHalll = (MjRoom)data[1];
                    RoomId = CurrMjHalll.RoomID;
                }
                this.CurrState = (EnumUIMainState)data[0];
            }
            else
            {
                this.CurrState = EnumUIMainState.None;
            }
        }

        public override void Init()
        {
            MemoryData.GameStateData.CurrMjRoomId = 0;
            InitUI();
            PlayInfoScript.Init(this);
            CenterScript.Init(this);
            TopScript.Init(this);
            DownScript.Init(this);
            ReferralScript.Init(this);
            RightButtonScript.Init(this);
            this.BroadcastsScript = transform.GetComponent<NGUIBroadcasts>();
        }

        public override void OnShow()
        {
            if (this.StateScript == null)
            {
                ChangeState(EnumUIMainState.None);
            }
            else
            {
                if (this.StateScript.SelfState != this.CurrState)
                {
                    if (this.CurrState != EnumUIMainState.None)
                    {
                        ChangeState(this.CurrState);
                    }
                    else
                    {
                        this.StateScript.RefreshUI();
                    }
                }
                else
                {
                    this.StateScript.RefreshUI();
                }
            }
        }

        public override void OnHide()
        {
        }

        public override void GoBack()
        {
            this.PlayInfoScript.OnButtonGoBackClick(null);
        }

        #endregion

        #region 控制动画运动 --------------------------------------------------------

        /// <summary>
        /// 登录界面的组件做tween动画
        /// isTween 是否需要播放动画
        /// </summary>
        public void GameHallObjTweenBegin(bool isTween)
        {
            PlayInfoScript.HeadBtnTweenBegin(isTween);
            CenterScript.CenterBtnTweenBegin(isTween);
        }

        #endregion ------------------------------------------------------------------
    }
}