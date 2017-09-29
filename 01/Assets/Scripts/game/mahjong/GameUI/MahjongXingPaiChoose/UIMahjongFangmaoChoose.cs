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

    public class UIMahjongFangmaoChoose : UIViewBase
    {
        public override void Init()
        {
            _EventHelper = new EventDispatcheHelper();
            AddEvents();
        }

        public override void OnHide()
        {

        }

        public override void OnShow()
        {
            SetParentState(needShow);
            IniShowBase();
        }

        public override void OnPushData(object[] data)
        {
            needShow = true;
            if (data.Length > 0)
            {
                needShow = (bool)data[0];
            }
        }

        protected override void OnClose()
        {
            _EventHelper.RemoveAllEvent();
        }

        public void SetParentState(bool state)
        {
            if (contentParent != null)
            {
                contentParent.SetActive(state);
            }
        }


        public GameObject contentParent = null;

        public GameObject btn_Fangmao = null;
        public GameObject btn_ConfirmFangmao = null;
        public GameObject btn_Cancel = null;
        public GameObject sp_tips = null;

        private List<int> maoList = null;

        public bool haveSend = false;
        private bool needShow = true;

        private void SetConfirmState(bool state, List<int> cardID)
        {
            btn_ConfirmFangmao.SetActive(state);
            maoList = cardID;
        }


        private void IniShowBase()
        {
            UIEventListener.Get(btn_Fangmao).onClick = OnClickFangMao;
            UIEventListener.Get(btn_ConfirmFangmao).onClick = OnClickConfirmFangMao;
            UIEventListener.Get(btn_Cancel).onClick = OnClickCancel;

            btn_Fangmao.SetActive(true);
            btn_ConfirmFangmao.SetActive(false);
            btn_Cancel.SetActive(true);
            sp_tips.SetActive(false);
        }


        private void OnClickFangMao(GameObject obj)
        {
            btn_Fangmao.SetActive(false);
            sp_tips.SetActive(true);
            EventDispatcher.FireEvent(GEnum.NamedEvent.EMjControlClickFangMao, 1);
            EventDispatcher.FireEvent(MJEnum.MusicEnum.ME_PlaySoundFX.ToString(), GEnum.SoundEnum.desk_gang);
        }

        private void OnClickConfirmFangMao(GameObject obj)
        {
            if (haveSend)
            {
                return;
            }
            haveSend = true;
            EventDispatcher.FireEvent(GEnum.NamedEvent.EMjControlClickFangMao, 2, maoList);
        }

        private void OnClickCancel(GameObject obj)
        {
            if (haveSend)
            {
                return;
            }
            haveSend = true;
            EventDispatcher.FireEvent(GEnum.NamedEvent.EMjControlClickFangMao, 3);
        }

        #region 事件系统
        private EventDispatcheHelper _EventHelper = null;

        private void AddEvents()
        {
            _EventHelper.AddEvent(GEnum.NamedEvent.EMjControlStateFangMao, EMjControlStateFangMao);             //放毛按钮状态 
            _EventHelper.AddAllEvent();
        }

        //操作事件，放毛按钮状态变更
        private void EMjControlStateFangMao(object[] obj)
        {
            bool state = (bool)obj[0];
            List<int> maoList = (List<int>)(obj[1]);
            SetConfirmState(state, maoList);
        }

        #endregion

    }

}

