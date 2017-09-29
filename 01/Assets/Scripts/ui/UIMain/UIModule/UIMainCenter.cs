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
    public class UIMainCenter : UIMainBase
    {
        public enum ButtonEnum
        {
            CreateRoom,
            JoinRoom
        }

        public GameObject CenterBtnPanel;
        public GameObject ButtonCreateRoom;
        public GameObject ButtonJoinRoom;
        public GameObject Effect_CreateRoom;
        public GameObject Effect_JoinRoom;
        public System.Action<ButtonEnum> OnButtonClick;

        public override void RefreshUI(UIMain.EnumUIMainState state)
        {
            ButtonCreateRoom.SetActive(state != UIMain.EnumUIMainState.MasterCheck);
            ButtonJoinRoom.SetActive(state != UIMain.EnumUIMainState.MasterCheck);
        }

        /// <summary>
        /// 创建房间 和 进入房间按钮
        /// </summary>
        public void SetData(System.Action<ButtonEnum> func)
        {
            this.OnButtonClick = func;
        }

        private void Awake()
        {
            UIEventListener.Get(ButtonCreateRoom).onClick = OnButtonCreateRoomClick;
            UIEventListener.Get(ButtonJoinRoom).onClick = OnButtonJoinRoomClick;
        }

        void OnEnable()
        {
            Effect_ShowSet(false);
        }

        private void OnButtonCreateRoomClick(GameObject go)
        {
            if (this.OnButtonClick == null)
            {
                EventDispatcher.FireSysEvent(GEnum.NamedEvent.OpenUI_UICreateRoom);
                //this.ui.LoadUIMain("UICreateRoom");
            }
            else
            {
                this.OnButtonClick(ButtonEnum.CreateRoom);
            }
        }

        private void OnButtonJoinRoomClick(GameObject go)
        {
            if (this.OnButtonClick == null)
            {
                this.ui.LoadUIMain("UIJoinRoom");
            }
            else
            {
                this.OnButtonClick(ButtonEnum.JoinRoom);
            }
        }


        #region 控制动画运动 --------------------------------------------------------

        /// <summary>
        /// 登录界面的按钮做tween动画
        /// </summary>
        public void CenterBtnTweenBegin(bool isTween)
        {
            if (isTween)
            {
                ButtonCreateRoom.transform.localPosition = new Vector3(-210.0f, -300.0f, 0.0f);
                Vector3 endV31 = new Vector3(-210.0f, -35.0f, 0.0f);
                TweenPosition.Begin(ButtonCreateRoom, 0.5f, endV31);

                ButtonJoinRoom.transform.localPosition = new Vector3(210.0f, -300.0f, 0.0f);
                Vector3 endV32 = new Vector3(210.0f, -35.0f, 0.0f);
                TweenPosition tp = TweenPosition.Begin(ButtonJoinRoom, 0.55f, endV32);

                EventDelegate.Add(tp.onFinished, () =>
                {
                    CenterBtnTweenFinished();
                });

                tp.PlayForward();
            }
            else
            {
                CenterBtnTweenFinished();
            }
        }

        void CenterBtnTweenFinished()
        {
            QLoger.LOG(" 大厅界面中间按钮 -- 动画播放完毕 ");

            if (CenterBtnPanel.GetComponent<UIPanel>() != null)
            {
                CenterBtnPanel.GetComponent<UIPanel>().clipping = UIDrawCall.Clipping.None;
                CenterBtnPanel.GetComponent<UIPanel>().Refresh();

                Destroy(CenterBtnPanel.GetComponent<UIPanel>());
            }

            Effect_ShowSet(true);

            EventDispatcher.FireSysEvent(GEnum.NamedEvent.SysFlow_Flow_CallBack, "PlayUIMainStartTween");
        }

        /// <summary>
        /// 控制特效的显示隐藏
        /// </summary>
        void Effect_ShowSet(bool isShow)
        {
            Effect_CreateRoom.SetActive(isShow);
            Effect_JoinRoom.SetActive(isShow);
        }

        #endregion ------------------------------------------------------------------
    }
}