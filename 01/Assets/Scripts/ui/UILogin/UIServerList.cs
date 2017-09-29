/**
 * @Author lyb
 *
 *
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace projectQ
{
    public class UIServerList : MonoBehaviour
    {
        public delegate void ServerListClickDelegate(ServerList sData);
        public ServerListClickDelegate OnClickCallBack;

        public string ServerId;
        public UILabel ServerIp;
        public UILabel ServerPort;
        public UILabel ServerName;

        /// <summary>
        /// 服务器数据类
        /// </summary>
        private ServerList serverData;

        void Start(){}

        /// <summary>
        /// 服务器列表初始化
        /// </summary>
        public void ServerListInit(ServerList sData)
        {
            serverData = sData;
            ServerId = serverData.ID;
            ServerIp.text = serverData.Ip;
            ServerPort.text = serverData.Port;
            ServerName.text = serverData.Name;
        }

        void OnClick()
        {
            if (OnClickCallBack != null)
            {
                OnClickCallBack(serverData);
            }
        }
    }
}

