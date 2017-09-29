/****************************************************
*
*  图片资源更新完毕，进入xml的版本比较更新流程
*  作者: lyb
*  日期：2017年7月25日
*
*****************************************************/

using System;
using System.IO;
using System.Net;
using UnityEngine;
using System.Collections;
using System.ComponentModel;
using System.Collections.Generic;

namespace projectQ
{
    public class XmlUpdate : BaseResourceLoad
    {
        /// <summary>
        /// 需要更新的资源列表
        /// </summary>
        private ArrayList XmlUpdateList = new ArrayList();

        /// <summary>
        /// 如果有更新记录当前下载了多少文件
        /// </summary>
        private int downNum = 0;

        private bool isDown = false;
        private string downName = "";

        /// <summary>
        /// 资源加载错误列表
        /// </summary>
        private List<string> BundleErr = new List<string>();

        void Start()
        {
            Xml_ServerCompareClient();
        }

        void Update()
        {
            if (isDown)
            {
                if (downNum >= XmlUpdateList.Count)
                {
                    DebugPro.LogBundle(" #[资源加载Xml]# XmlUpdate - 本地xml资源更新完成，进入游戏 ");

                    isDown = false;

                    Flow_FinishGameStart();
                }
                else
                {
                    EventDispatcher.FireEvent(EventKey.Bundle_SliderValue_Event,
                        BundleStepEnum.Bundle_XmlDown, downNum, XmlUpdateList.Count);

                    EventDispatcher.FireEvent(EventKey.Bundle_LoadName_Event, 3, downName);
                }
            }
        }

        /// <summary>
        /// 服务器和客户端资源比对
        /// </summary>
        public void Xml_ServerCompareClient()
        {
            foreach (string urlStr in GameConfig.Instance.XmlList_Server)
            {
                string[] values = urlStr.Split(new char[] { '@' });

                string checkName = ResourceUpdate_Check(GameConfig.Instance.XmlList_Client, values[0], values[1]);

                if (!string.IsNullOrEmpty(checkName))
                {
                    //说明在本地列表中发现了需要更新的资源或者新添加的资源

                    string path = GameConfig.Instance.XmlLocal_Path + "/" + checkName + ".zip.dat";

                    if (File.Exists(path))
                    {
                        File.Delete(path);
                    }

                    if (checkName.Equals("New"))
                    {
                        DebugPro.LogBundle(" #[资源加载Xml]# XmlUpdate - 新的xml数据表 = " + values[0]);
                    }
                    else
                    {
                        DebugPro.LogBundle(" #[资源加载Xml]# XmlUpdate - 需要更新下载的xml = " + checkName);
                    }

                    XmlUpdateList.Add(urlStr);
                }
            }

            if (XmlUpdateList.Count > 0)
            {
                DebugPro.LogBundle(" #[资源加载Xml]# XmlUpdate - Xml有更新,进入下载流程 ");

                isDown = true;

                EventDispatcher.FireEvent(EventKey.Bundle_SliderValueShow_Event, BundleStepEnum.Bundle_XmlDown);

                XmlUpdate_Down();
            }
            else
            {
                DebugPro.LogBundle(" #[资源加载Xml]# XmlUpdate - Xml没有更新,进入游戏 ");

                Flow_FinishGameStart();
            }
        }

        #region lyb ----------Xml文件下载-------------------------------------------------------------

        /// <summary>
        /// 获取Xml的Url地址
        /// </summary>
        void XmlUpdate_Down()
        {
            if (BundleErr.Count > 0)
            {
                BundleErrorPrompt(ResourceConfig.Bundle_Text518);

                BundleErr = new List<string>();

                return;
            }

            if (downNum < XmlUpdateList.Count)
            {
                string[] values = XmlUpdateList[downNum].ToString().Split(new char[] { '@' });

                downName = values[0];

                // 文件下载的Url地址
                string downUrl = GameConfig.Instance.Resource_ServerPath + values[1];

                // 文件保存的本地地址
                string localUrl = GameConfig.Instance.XmlLocal_Path + "/" + values[0] + ".zip.dat";

                WebClient webClient = new WebClient();

                webClient.DownloadFileCompleted += new AsyncCompletedEventHandler(XmlDownLoadCallBack);

                webClient.DownloadFileAsync(new Uri(downUrl), localUrl);
            }
        }

        private void XmlDownLoadCallBack(object sender, AsyncCompletedEventArgs e)
        {
            downNum++;

            if (sender != null && sender is WebClient)
            {
                ((WebClient)sender).CancelAsync();
                ((WebClient)sender).Dispose();
            }

            if (e.Error != null)
            {
                BundleErr.Add(e.Error.ToString());

                QLoger.LOG(LogType.EError, " #[资源加载失败]# - 资源下载失败 e.Error = " + e.Error);
            }

            XmlUpdate_Down();
        }

        #endregion -----------------------------------------------------------------------------------

        #region lyb ----------资源更新结束------------------------------------------------------------

        /// <summary>
        /// 更新结束，进行数据处理
        /// </summary>
        void Flow_FinishGameStart()
        {
            string path = GameConfig.Instance.Xml_ClientPath;

            if (File.Exists(path))
            {
                File.Delete(path);
            }

            using (System.Net.WebClient wc = new System.Net.WebClient())
            {
                wc.DownloadFile(GameConfig.Instance.Xml_ServerUrl, GameConfig.Instance.Xml_ClientPath);
            }

            Tools_XmlOperation.Xml_UpdateVersion(GameConfig.Instance.Xml_ClientPath, Application.version);

            EventDispatcher.FireEvent(EventKey.Bundle_ResourceDownFinish_Event);
        }

        #endregion -----------------------------------------------------------------------------------
    }
}