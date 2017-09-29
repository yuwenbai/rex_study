using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UI.ScrollViewTool;
using System;

namespace projectQ
{
    public class ClienteleItemData
    {
        public PlayerDataModel player;
        public Action<ClienteleItemData, GameObject> OnClick;
    }

    public class ClienteleItem : ScrollViewItemBase<ClienteleItemData>
    {
        public UITexture icon;
        public UILabel playerName;
        public UILabel playerID;
        public UILabel playerState;

        private string headUrl = "";
        private string rememberHead;

        public override void Refresh()
        {
            if (this.UIData == null)
                return;

            playerName.text = UIData.player.PlayerDataBase.Name;
            SetIcon((ulong)UIData.player.PlayerDataBase.UserID, UIData.player.PlayerDataBase.HeadURL);
            playerID.text = UIData.player.PlayerDataBase.UserID.ToString();
            string stateStr = "";
            switch ((int)this.UIData.player.ClienteleDataState.ClienteleState)
            {
                case 0:
                    stateStr = "[a0a0a0]离线";
                    break;
                case 1:
                    stateStr = "[00ff00]游戏中";
                    break;
                case 2:
                    stateStr = "[ffffff]空闲";
                    break;
            }
            playerState.text = stateStr;
        }

        public void SetIcon(ulong userId, string url)
        {
            if (headUrl != url)
            {
                headUrl = url;

                string headname = DownHeadTexture.Instance.Texture_HeadNameSet(url);
                if (rememberHead == headname)
                {
                    return;
                }
                if (string.IsNullOrEmpty(headname))
                {
                    headname = "Texture_head_01";
                }
                rememberHead = headname;

                icon.mainTexture = Resources.Load<Texture>(GameAssetCache.Texture_Hand_Path);
                DownHeadTexture.Instance.WeChat_HeadTextureGet(url, SetPlayerIcon);
            }
        }

        void SetPlayerIcon(Texture2D HeadTexture, string headName)
        {
            QLoger.LOG(" 头像赋值 --  " + HeadTexture.name);

            if (string.IsNullOrEmpty(headName) || rememberHead != headName)
            {
                return;
            }
            icon.mainTexture = HeadTexture;
        }

        public void OnButtonClick(GameObject go)
        {
            object[] data = new object[2];
            data[0] = this.UIData.player.PlayerDataBase.UserID;
            data[1] = headUrl;
            _R.ui.OpenUI("UIUserInfo", data);
            //if (this.UIData.OnClick != null)
            //{
            //    this.UIData.OnClick(this.UIData, gameObject);
            //}
        }

        private void Awake()
        {
            UIEventListener.Get(gameObject).onClick = OnButtonClick;
        }
    }
}
