/****************************************************
*
*  加载本地的头像资源到内存中
*  版本需求游戏开始不需要加载到内存中，该方法保留直接返回
*  作者: lyb
*  日期：2017年7月25日
*
*****************************************************/

using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace projectQ
{
    public class HeadTextureLoad : BaseResourceLoad
    {
        /// <summary>
        /// 记录当前下载了多少文件
        /// </summary>
        private int downNum = 0;
        /// <summary>
        /// 记录当前总共多少个
        /// </summary>
        private int allNum;

        private bool isDown = false;

        /// <summary>
        /// 获取本地的头像路径列表
        /// </summary>
        private List<string> HeadTextureList = new List<string>();

        void Start()
        {
            DownHeadTexture.Instance.HeadDic = new Dictionary<string, HeadTextureInfo>();

            if (!Directory.Exists(GameConfig.Instance.WeChat_HeadTexturePath))
            {
                Directory.CreateDirectory(GameConfig.Instance.WeChat_HeadTexturePath);
            }

            EventDispatcher.FireEvent(EventKey.Bundle_HeadTextureLoadFinish_Event);

            //HeadTextureClient_Load();
        }

        void Update()
        {
            if (isDown)
            {
                if (downNum >= allNum)
                {
                    QLoger.LOG(" #[头像加载]# HeadTextureLoad - 本地头像图片资源加载完成 ");

                    isDown = false;

                    EventDispatcher.FireEvent(EventKey.Bundle_HeadTextureLoadFinish_Event);
                }
            }
        }

        #region lyb ----------把本地资源加载到指定列表中存储----------------------------------------

        /// <summary>
        /// 获取本地头像资源
        /// </summary>
        public void HeadTextureClient_Load()
        {
            if (!Directory.Exists(GameConfig.Instance.WeChat_HeadTexturePath))
            {
                Directory.CreateDirectory(GameConfig.Instance.WeChat_HeadTexturePath);
            }

            HeadTextureList = new List<string>();

            string HeadTex_Path = GameConfig.Instance.WeChat_HeadTexturePath;

            DirectoryInfo folderDir = new DirectoryInfo(HeadTex_Path);

            foreach (FileInfo file in folderDir.GetFiles())
            {
                if (file.Extension != ".xml")
                {
                    HeadTextureList.Add(file.Name);
                }
            }

            if (HeadTextureList.Count <= 0)
            {
                Tools_FileOperation.Files_DeleteAll(GameConfig.Instance.WeChat_HeadTexturePath);
            }

            downNum = 0;

            allNum = HeadTextureList.Count;

            isDown = true;

            HeadTexture_PathGet();
        }

        void HeadTexture_PathGet()
        {
            if (downNum < allNum)
            {
                string url = "file://" + GameConfig.Instance.WeChat_HeadTexturePath + "/" + HeadTextureList[downNum];

#if UNITY_EDITOR || UNITY_STANDALONE_WIN
                url = "file:///" + GameConfig.Instance.WeChat_HeadTexturePath + "/" + HeadTextureList[downNum];
#endif
                EventDispatcher.FireEvent(EventKey.Bundle_LoadName_Event, 7, HeadTextureList[downNum]);

                StartCoroutine(DownLoadTexture(url, HeadTextureList[downNum]));
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
                    DownHeadTexture.Instance.HeadTexture_SetValue(code, tempImage);

                    downNum++;

                    HeadTexture_PathGet();
                }
                else
                {
                    BundleErrorPrompt("网络不给力，请检查网络");
                }
            }
            else
            {
                QLoger.LOG(" 请您稍后重试[h1] www.error = " + www.error);

                BundleErrorPrompt("网络不给力，请检查网络");
            }
        }

        #endregion ---------------------------------------
    }
}