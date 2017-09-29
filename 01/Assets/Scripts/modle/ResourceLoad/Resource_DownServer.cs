/****************************************************
*
*  下载服务器上的资源
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
    public class Resource_DownServer : BaseResourceLoad
    {
        private int tex_downNum = 0;
        private int tex_allNum;
        private bool isTexDown = false;

        private int xml_downNum = 0;
        private int xml_allNum;
        private bool isXmlDown = false;

        /// <summary>
        /// 资源加载错误列表
        /// </summary>
        private List<string> BundleErr = new List<string>();

        void Start()
        {
            tex_allNum = GameConfig.Instance.TextureList_Server.Count;

            isTexDown = true;

            EventDispatcher.FireEvent(EventKey.Bundle_SliderValueShow_Event, BundleStepEnum.Bundle_TextureDown);

            TextureServer_Down();
        }

        void Update()
        {
            if (isTexDown)
            {
                if (tex_downNum >= tex_allNum)
                {
                    DebugPro.LogBundle(" #[资源加载]# DownResourceServer - 从服务器把图片资源下载完成，进入下载xml文件过程 ");

                    isTexDown = false;

                    XmlServer_DownBegin();
                }
            }

            if (isXmlDown)
            {
                if (xml_downNum >= xml_allNum)
                {
                    DebugPro.LogBundle(" #[资源加载Xml]# DownResourceServer - 从服务器上把xml下载完成，进入游戏 ");

                    isXmlDown = false;

                    DownResourceServerFinish();
                }
                else
                {
                    EventDispatcher.FireEvent(EventKey.Bundle_SliderValue_Event,
                        BundleStepEnum.Bundle_XmlDown, xml_downNum, xml_allNum);

                    string[] values = GameConfig.Instance.XmlList_Server[xml_downNum].ToString().Split(new char[] { '@' });

                    EventDispatcher.FireEvent(EventKey.Bundle_LoadName_Event, 3, values[0]);
                }
            }
        }

        #region lyb 资源下载完毕-----------------------------------------

        /// <summary>
        /// 资源下载完成
        /// </summary>
        void DownResourceServerFinish()
        {
            using (System.Net.WebClient wc = new System.Net.WebClient())
            {
                wc.DownloadFile(GameConfig.Instance.Xml_ServerUrl, GameConfig.Instance.Xml_ClientPath);
            }

            Tools_XmlOperation.Xml_UpdateVersion(GameConfig.Instance.Xml_ClientPath, Application.version);

            EventDispatcher.FireEvent(EventKey.Bundle_ResourceDownFinish_Event);
        }

        #endregion ------------------------------------------------------

        #region lyb Texture文件下载--------------------------------------

        /// <summary>
        /// 获取Url地址
        /// </summary>
        void TextureServer_Down()
        {
            if (tex_downNum < tex_allNum)
            {
                string[] values = GameConfig.Instance.TextureList_Server[tex_downNum].ToString().Split(new char[] { '@' });

                EventDispatcher.FireEvent(EventKey.Bundle_LoadName_Event, 2, values[0]);

                StartCoroutine(DownLoadTexture(GameConfig.Instance.Resource_ServerPath + values[1], values[0]));
            }
        }

        IEnumerator DownLoadTexture(string url, string code)
        {
            WWW www = new WWW(url);
            Texture2D tempImage;
            yield return www;

            if (www.isDone && www.error == null)
            {
                tempImage = www.texture;

                if (tempImage != null)
                {
                    byte[] data = tempImage.EncodeToPNG();

                    File.WriteAllBytes(GameConfig.Instance.ResourceLocal_Path + "/" + code + ".png", data);

                    GameConfig.Instance.DownResourceDic.Add(code, tempImage);

                    tex_downNum++;

                    EventDispatcher.FireEvent(EventKey.Bundle_SliderValue_Event,
                        BundleStepEnum.Bundle_TextureDown, tex_downNum, tex_allNum);

                    TextureServer_Down();
                }
                else
                {
                    QLoger.LOG(LogType.EError, " 请您稍后重试[5] www.error = " + "下载下来的图片为空");

                    BundleErrorPrompt(ResourceConfig.Bundle_Text511);
                }
            }
            else
            {
                QLoger.LOG(LogType.EError, " 请您稍后重试[2] www.error = " + www.error);

                BundleErrorPrompt(ResourceConfig.Bundle_Text512);
            }
        }

        #endregion ------------------------------------------------------

        #region lyb Xml文件下载------------------------------------------

        void XmlServer_DownBegin()
        {
            xml_downNum = 0;

            xml_allNum = GameConfig.Instance.XmlList_Server.Count;

            isXmlDown = true;

            EventDispatcher.FireEvent(EventKey.Bundle_SliderValueShow_Event, BundleStepEnum.Bundle_XmlDown);

            DebugPro.LogBundle(" #[资源加载Xml]# DownResourceServer - 当前Xml资源下载总数量 = " + xml_allNum);

            XmlServer_Down();
        }

        /// <summary>
        /// 获取Xml的Url地址
        /// </summary>
        void XmlServer_Down()
        {
            if (BundleErr.Count > 0)
            {
                BundleErrorPrompt(ResourceConfig.Bundle_Text513);

                BundleErr = new List<string>();

                return;
            }

            if (xml_downNum < GameConfig.Instance.XmlList_Server.Count)
            {
                string[] values = GameConfig.Instance.XmlList_Server[xml_downNum].ToString().Split(new char[] { '@' });

                // 文件下载的Url地址
                string downUrl = GameConfig.Instance.Resource_ServerPath + values[1];

                // 文件保存的本地地址
                string localUrl = GameConfig.Instance.XmlLocal_Path + "/" + values[0] + ".zip.dat";

                WebClient webClient = new WebClient();

                webClient.DownloadFileCompleted += new AsyncCompletedEventHandler(XmlDownLoadCallBack);

                webClient.DownloadFileAsync(new Uri(downUrl), localUrl);
            }
        }

        void XmlDownLoadCallBack(object sender, AsyncCompletedEventArgs e)
        {
            xml_downNum++;

            DebugPro.LogBundle(" #[资源加载Xml]# DownResourceServer - 资源下载成功 当前下载的数量 = " + xml_downNum);

            if (sender != null && sender is WebClient)
            {
                ((WebClient)sender).CancelAsync();
                ((WebClient)sender).Dispose();
            }

            if (e.Error != null)
            {
                BundleErr.Add(e.Error.ToString());

                QLoger.LOG(LogType.EError, " #[资源加载Xml失败]# DownResourceServer - 资源下载失败 e.Error = " + e.Error);
            }

            XmlServer_Down();
        }

        #endregion ------------------------------------------------------
    }
}