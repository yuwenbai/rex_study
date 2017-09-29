/****************************************************
*
*  加载本地的资源到内存中
*  作者: lyb
*  日期：2017年7月25日
*
*****************************************************/

using UnityEngine;
using System.Collections;

namespace projectQ
{
    public class Resource_DownClient : BaseResourceLoad
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

        void Start()
        {
            TextureClient_Load();
        }

        void Update()
        {
            if (isDown)
            {
                if (downNum >= allNum)
                {
                    DebugPro.LogBundle(" #[资源加载]# DownResourceClient - 本地图片资源加载完成，进入xml版本检测流程 ");

                    isDown = false;

                    Flow_XmlUpdate();
                }
            }
        }

        #region lyb 把本地资源加载到指定列表中存储--------

        /// <summary>
        /// 获取本地资源的Url地址
        /// </summary>
        public void TextureClient_Load()
        {
            allNum = GameConfig.Instance.TextureList_Client.Count;

            isDown = true;

            EventDispatcher.FireEvent(EventKey.Bundle_SliderValueShow_Event, BundleStepEnum.Bundle_TextureLoad);

            TextureClient_PathGet();

            DebugPro.LogBundle(" #[资源加载]# - PathHelper.PersistentPath = " + PathHelper.PersistentPath);
        }

        void TextureClient_PathGet()
        {
            if (downNum < allNum)
            {
                string[] values = GameConfig.Instance.TextureList_Client[downNum].ToString().Split(new char[] { '@' });

                string str = values[1].Substring(0, values[1].LastIndexOf('?'));

                string url = "file://" + PathHelper.PersistentPath + "/" + str;

#if UNITY_EDITOR || UNITY_STANDALONE_WIN
                url = "file:///" + PathHelper.PersistentPath + "/" + str;
#endif

                EventDispatcher.FireEvent(EventKey.Bundle_LoadName_Event, 4, values[0]);

                StartCoroutine(DownLoadTexture(url, values[0]));
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
                    if (!GameConfig.Instance.DownResourceDic.ContainsKey(code))
                    {
                        GameConfig.Instance.DownResourceDic.Add(code, tempImage);
                    }

                    downNum++;

                    EventDispatcher.FireEvent(EventKey.Bundle_SliderValue_Event,
                        BundleStepEnum.Bundle_TextureLoad, downNum, allNum);

                    TextureClient_PathGet();
                }
                else
                {
                    QLoger.LOG(LogType.EError, " 请您稍后重试[7] www.error = " + "下载下来的图片为空");

                    BundleErrorPrompt(ResourceConfig.Bundle_Text509);
                }
            }
            else
            {
                QLoger.LOG(LogType.EError, " 请您稍后重试[3] www.error = " + www.error);

                BundleErrorPrompt(ResourceConfig.Bundle_Text510);
            }
        }

        #endregion ---------------------------------------

        #region lyb 进入下一步流程------------------------

        void Flow_XmlUpdate()
        {
            gameObject.AddComponent<XmlUpdate>();
        }

        #endregion ---------------------------------------
    }
}