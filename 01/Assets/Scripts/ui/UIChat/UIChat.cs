

using Msg;
using System;
/**
* 作者：周腾
* 作用：
* 日期：
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace projectQ
{
    public enum ChatType
    {
        TextType,
        EmojType,
    }
    public class UIChat : UIViewBase
    {
        public UIChatModel Model { get { return _model as UIChatModel; } }
        public UIScrollView textScrollView;
        public UIGrid textGrid;
        public GameObject textItem;
        private bool textIsInitOver;
        public GameObject bgObj;
        public UIScrollView emojScrollView;
        public UIGrid emojGrid;
        public GameObject emojItem;
        private bool emojIsInitOver;
        private bool isUiChatOpen = false;
        private MahjongPositionPlayerInfo infos;

        public UILabel textLabel;
        public UILabel emojLabel;
        #region Toggle Function
        public void IsActive()
        {
            if (UIToggle.current.value)
            {
                QLoger.LOG("value is true");
                ShowPanel(ChatType.TextType);
            }
            else
            {
                QLoger.LOG("value is false");
                ShowPanel(ChatType.EmojType);
            }

            textLabel.text = "文字";
            emojLabel.text = "表情";
        }
        #endregion
        private List<TextConfig> contents = new List<TextConfig>();
        private List<TextConfig> emojList = new List<TextConfig>();
        #region override
        public override void Init()
        {
            UIChatManager.isOpenPage = false;
            textIsInitOver = false;
            emojIsInitOver = false;
            bgObj.SetActive(false);

            Model.Text_LoadXml();
            List<TextConfig> list = Model.GetDataByType("CHAT_MSG");
            for (int i = 0; i < list.Count; i++)
            {
                contents.Add(list[i]);
            }
            list = Model.GetDataByType("CHAT_ANIMATION");
            for (int i = 0; i < list.Count; i++)
            {
                emojList.Add(list[i]);
            }
        }

        public override void OnShow()
        {
            EventDispatcher.AddEvent(GEnum.NamedEvent.EMjControlCloseMaskUI, HideUI);
            EventDispatcher.AddEvent(GEnum.NamedEvent.SysUI_Chat_CloseUI, HideUI);
        }
        
        public override void OnPushData(object[] data)
        {
            if (data.Length == 0)
            {
                CheckShowOrHide(false);
            }
            else
            {
                isUiChatOpen = (bool)data[0];
                infos = data[1] as MahjongPositionPlayerInfo;
                CheckShowOrHide(isUiChatOpen);
            }
        }
        public override void OnHide()
        {
            UIChatManager.isOpenPage = false;
            //gameObject.SetActive(false);
            bgObj.SetActive(false);
            RemoveEvent();
        }

        private void HideUI(object[] values)
        {
            if (textIsInitOver == false && emojIsInitOver == false)
            {
                return;
            }

            if (this.gameObject != null && this != null)
                OnHide();
        }

        protected override void OnClose()
        {
            textIsInitOver = false;
            emojIsInitOver = false;
            UIChatManager.isOpenPage = false;
            RemoveEvent();
        }

        private void RemoveEvent()
        {
            EventDispatcher.RemoveEvent(GEnum.NamedEvent.EMjControlCloseMaskUI, HideUI);
            EventDispatcher.RemoveEvent(GEnum.NamedEvent.SysUI_Chat_CloseUI, HideUI);
        }
        #endregion

        #region Self Function
        //public void ReceiveData(object[] data)
        //{
        //    isUiChatOpen = (bool)data[0];
        //    infos = data[1] as MahjongPositionPlayerInfo;
        //    CheckShowOrHide(isUiChatOpen);

        //}
        void CheckShowOrHide(bool isOpen)
        {
            if (isOpen)
            {
                bgObj.SetActive(true);
                ShowPanel(ChatType.TextType);
            }
            else
            {
                bgObj.SetActive(false);
            }
        }

        void CreatItem(List<TextConfig> list, UIGrid grid, GameObject item)
        {
            for (int i = 0; i < list.Count; i++)
            {
                GameObject go = NGUITools.AddChild(grid.gameObject, item);
                go.transform.localPosition = Vector3.zero;
                go.SetActive(true);
                TextChatItem text = go.GetComponent<TextChatItem>();
                if (text != null)
                {
                    text.InitItem(list[i].ID, list[i].Value);
                    text.dele_textClick = OnTextItemClick;
                }
                else
                {
                    EmojChatItem emo = go.GetComponent<EmojChatItem>();
                    if (emo != null)
                    {
                        emo.InitItem(list[i].ID, list[i].Value);
                        emo.dele_emojClick = OnEmojClick;
                    }
                }
            }
            grid.Reposition();
        }

        public void ShowPanel(ChatType type)
        {
            switch (type)
            {
                case ChatType.TextType:
                    if (textIsInitOver)
                    {
                        return;
                    }
                    else
                    {
                        CreatItem(contents, textGrid, textItem);
                        textScrollView.ResetPosition();
                        textIsInitOver = true;
                    }
                    break;
                case ChatType.EmojType:
                    if (emojIsInitOver)
                    {
                        return;
                    }
                    else
                    {
                        CreatItem(emojList, emojGrid, emojItem);
                        emojScrollView.ResetPosition();
                        emojIsInitOver = true;
                    }
                    break;

            }
        }
       
        private void OnEmojClick(string chatMsg)
        {
            QLoger.LOG("emoj" + chatMsg);
            Model.OnSendReq(chatMsg);
            OnHide();
            //Close();
        }

        public void OnTextItemClick(string chatMsg)
        {
            QLoger.LOG("text = " + chatMsg);
            Model.OnSendReq(chatMsg);
			OnHide();
            //Close();
        }

        public TextConfig GetConfigByID(string id)
        {
            for (int i = 0; i < contents.Count; i++)
            {
                if (contents[i].ID == id)
                    return contents[i];
            }
            for (int i = 0; i < emojList.Count; i++)
            {
                if (emojList[i].ID == id)
                    return emojList[i];
            }
            return null;
        }

      public  void ShowChat(long userID,int seatId,string chatMsg)
        {
            for (int i = 0; i < infos.playerInfoArray.Length; i++)
            {
               
                if (userID == (long)infos.playerInfoArray[i].userId)
                {
                    for (int j = 0; j < emojList.Count; j++)
                    {
                        string str = emojList[j].ID;// + "1";
                        if (chatMsg == str)
                        {
                            string msg = str;//.Substring(0, str.Length - 1);
                            TextConfig tc = GetConfigByID(msg);
                            if (tc != null)
                                infos.playerInfoArray[i].ShowEmoj(tc.Value);
                        }
                    }
                    for (int k = 0; k < contents.Count; k++)
                    {
                        if (chatMsg.Equals(contents[k].ID))
                        {
                            TextConfig tc = GetConfigByID(chatMsg);
                            if (tc != null)
                            {
                                infos.playerInfoArray[i].ShowChat(tc.Value);
                                MusicCtrl.Instance.Music_VoicePlay(int.Parse(tc.ID), userID);
                            }
                        }
                        
                    }
                    QLoger.LOG("UIChat chatMsg = " + chatMsg);
                    //QLoger.LOG("Model.GetData = " + Model.GetDataById(3).value);
                    //if (chatMsg.Equals(Model.GetDataById(3).value))
                    //{
                    //    infos.playerInfoArray[i].InitRecord();
                    //}
                    //if (chatMsg == "INITSDK")
                    //{
                    //    infos.playerInfoArray[i].InitRecord(userID, seatId);
                    //}

                }
                if (chatMsg == "INITSDK")
                {
                    infos.playerInfoArray[i].InitRecord(userID, seatId);
                }

            }


        }
        #endregion
    }
}