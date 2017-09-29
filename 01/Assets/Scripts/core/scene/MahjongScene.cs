/**
 * @Author Xin.Wang
 *
 *
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace projectQ
{
    public class MahjongScene : SceneBase
    {
        public MahjongScene()
        {
            StateName = "MahJong";
        }

        public override void StateBegin()
        {

            if (SDKManager.Instance != null)
            {

                SDKManager.Instance.HiddenWebView();
                _R.Instance.StartCoroutine(UITools.WaitExcution(() => {
                    EventDispatcher.FireEvent(GEnum.NamedEvent.SysWebView_Close);
                }, 4f));
            }
            //ini yuyin
            EventDispatcher.FireEvent(GEnum.NamedEvent.EYuyinState, true);
            base.StateBegin();
            //回放状态下不弹出准备界面
            //if (!FakeReplayManager.Instance.ReplayState)
                _R.ui.OpenUI("UIPrepareGame");
            //else
            //{
            //    if (!_R.ui.IsShowUI(GameConst.path_MahjongUIMain))
            //    {
            //        _R.ui.OpenUI(GameConst.path_MahjongUIMain);
            //    }
            //}
        }

        public override void StateEnd()
        {
            base.StateEnd();
            EventDispatcher.FireEvent(GEnum.NamedEvent.EMahjongOutScene);
            _R.ui.ClearAll();

            //NC 主动登出语音SDK
            if (RecordManager.Instance != null)
            {
                RecordManager.Instance.LogoutRecord();//leave();
            }
        }
    }
}

