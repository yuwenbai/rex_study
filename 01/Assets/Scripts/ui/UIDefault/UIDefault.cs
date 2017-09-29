

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
    public class UIDefault : UIViewBase
    {
        public UIDefaultModel Model
        {
            get { return _model as UIDefaultModel; }
        }

        /// <summary>
        /// 进入准备房间
        /// </summary>
        /// <param name="deskInfo"></param>
        public void JoinDesk()
        {
            if (_R.scene.GetCurrSceneName() != "MahJong")
            {
                _R.flow.SetQueueForce(QFlowManager.FlowType.ChangeScene, EnumChangeSceneType.Main_To_Game);
                //_R.scene.SetScene(new MahjongScene(), "MahJong");
            }
            else
            {
                if (!_R.ui.IsShowUI("UIPrepareGame"))
                {
                    LoadUIMain("UIPrepareGame");
                }
                //关掉其他的UI
                _R.ui.CloseUI("UICreateRoom");
                _R.ui.CloseUI("UICreateRoomRule");
                //_R.ui.ClearAll(new List<string>() { "UIMahjongMain", "UIPrepareGame" , "UIMahjongGameStart" });
            }
        }
        public void JoinMain(EnumChangeSceneType key)
        {
            if (_R.scene.GetCurrSceneName() != "Main")
            {
                _R.flow.SetQueue(QFlowManager.FlowType.ChangeScene, key);
                //_R.scene.SetScene(new MainScene(), "Main");
            }
            else
            {
                LoadUIMain("UIMain");
            }
        }

        public void OpenLogin(bool isForce)
        {
            if(!_R.ui.IsShowUI("UILogin") || MemoryData.GameStateData.LoadingActive)
            {
                if(isForce)
                {
                    _R.flow.SetQueueForce(QFlowManager.FlowType.ChangeScene, EnumChangeSceneType.Main_To_Login);
                }
                else
                {
                    _R.flow.SetQueue(QFlowManager.FlowType.ChangeScene, EnumChangeSceneType.Main_To_Login);
                }
            }
			else
            {
				_R.ui.IsShowUI("UILogin");
            }
        }

        #region override
        public override void Init()
        {
        }

        public override void OnHide()
        {
        }

        public override void OnShow()
        {
        }
        #endregion


        //private void OnGUI()
        //{
        //    if(GUILayout.Button("打开Loading"))
        //    {
        //        EventDispatcher.FireSysEvent(GEnum.NamedEvent.SysUI_SmallLoading_Open, "Test");
        //    }
        //    if (GUILayout.Button("关闭Loading"))
        //    {
        //        EventDispatcher.FireSysEvent(GEnum.NamedEvent.SysUI_SmallLoading_Close, "Test");
        //    }
        //}
    }
}