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
    public class UIServerSelect : MonoBehaviour
    {
        public UIGrid GridObj;
        public UILogin LoginMain;

        private List<ServerList> sListData = new List<ServerList>();

        void Start()
        {
            SListDataInit();

            int serverId = 0;
            if (serverId > 0)
            {
                foreach (ServerList sList in sListData)
                {
                    if (sList.ID == serverId.ToString())
                    {
                        OnServerListClickCallBack(sList);
                        break;
                    }
                }
            }
            else
            {
                OnServerListClickCallBack(sListData[0]);
            }

            gameObject.SetActive(false);
        }

        /// <summary>
        /// 初始化服务器列表
        /// </summary>
        public void ServerSelectInit()
        {
            SListDataInit();

            UITools.CreateChild<ServerList>(GridObj.transform, null, sListData, (go, serverData) =>
            {
                UIServerList serverList = go.GetComponent<UIServerList>();
                serverList.ServerListInit(serverData);

                serverList.OnClickCallBack = OnServerListClickCallBack;
            });
            GridObj.Reposition();
        }

        /// <summary>
        /// 初始化数据
        /// </summary>
        void SListDataInit()
        {
            if (sListData.Count <= 0)
            {
                List<BaseXmlBuild> buildList = MemoryData.XmlData.XmlBuildDataDic["ServerList"];

                int sType = 0;
#if __BUNDLE_OUTER_SERVER
                sType = 1;
#elif __BUNDLE_PRE_OUTER_SERVER
                sType = 2;
#elif __BUNDLE_IOS_SERVER
                sType = 1;
#endif
                foreach (BaseXmlBuild build in buildList)
                {
                    ServerList info = (ServerList)build;

                    if (sType == 0 || int.Parse(info.sType) == sType)
                    {
                        sListData.Add(info);
                    }
                }
            }
        }

        /// <summary>
        /// 服务器选择成功返回
        /// </summary>
        void OnServerListClickCallBack(ServerList sData)
        {
            LoginMain.OnServerSelectSucc(sData);
            gameObject.SetActive(false);
        }
    }
}
