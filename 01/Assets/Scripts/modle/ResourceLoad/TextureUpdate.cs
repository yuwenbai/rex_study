/****************************************************
*
*  本地图片加载完毕，进入Texture的版本比较更新流程
*  作者: lyb
*  日期：2017年7月25日
*
*****************************************************/

using System.IO;
using UnityEngine;
using System.Collections;

namespace projectQ
{
    public class TextureUpdate : BaseResourceLoad
    {
        /// <summary>
        /// 需要更新的资源列表
        /// </summary>
        private ArrayList TextureUpdateList = new ArrayList();

        /// <summary>
        /// 如果有更新记录当前下载了多少文件
        /// </summary>
        private int downNum = 0;

        private bool isDown = false;

        void Start()
        {
            ServerCompareClient();
        }

        void Update()
        {
            if (isDown)
            {
                if (downNum >= TextureUpdateList.Count)
                {
                    DebugPro.LogBundle(" #[资源加载]# TextureUpdate - 图片资源更新完成，将其读进内存中 ");

                    isDown = false;

                    Flow_DownResourceClient();
                }
            }
        }

        /// <summary>
        /// 服务器和客户端资源比对
        /// </summary>
        public void ServerCompareClient()
        {
            foreach (string urlStr in GameConfig.Instance.TextureList_Server)
            {
                string[] values = urlStr.Split(new char[] { '@' });

                string checkName = ResourceUpdate_Check(GameConfig.Instance.TextureList_Client, values[0], values[1]);

                if (!string.IsNullOrEmpty(checkName))
                {
                    //说明在本地列表中发现了需要更新的资源或者新添加的资源

                    string path = GameConfig.Instance.ResourceLocal_Path + "/" + name + ".png";

                    if (File.Exists(path))
                    {
                        File.Delete(path);
                    }

                    TextureUpdateList.Add(urlStr);
                }
            }

            if (TextureUpdateList.Count > 0)
            {
                DebugPro.LogBundle(" #[资源加载]# TextureUpdate - 图片资源有更新，进入下载流程 ");

                isDown = true;

                TextureUpdate_Down();
            }
            else
            {
                DebugPro.LogBundle(" #[资源加载]# TextureUpdate - 图片没有更新，把图片加载到内存中 ");

                Flow_DownResourceClient();
            }
        }

        #region lyb ----------资源更新----------------------------------------------------------------

        /// <summary>
        /// 获取需要更新的Url地址
        /// </summary>
        void TextureUpdate_Down()
        {
            if (downNum < TextureUpdateList.Count)
            {
                string[] values = TextureUpdateList[downNum].ToString().Split(new char[] { '@' });

                EventDispatcher.FireEvent(EventKey.Bundle_LoadName_Event, 2, values[0]);

                StartCoroutine(DownLoadTexture(values[1], values[0]));
            }
        }

        IEnumerator DownLoadTexture(string url, string code)
        {
            string tUrl = GameConfig.Instance.Resource_ServerPath + url;
            WWW www = new WWW(tUrl);
            Texture2D tempImage;
            yield return www;

            if (www.isDone && www.error == null)
            {
                tempImage = www.texture;

                if (tempImage != null)
                {
                    byte[] data = tempImage.EncodeToPNG();

                    File.WriteAllBytes(GameConfig.Instance.ResourceLocal_Path + "/" + code + ".png", data);

                    if (!GameConfig.Instance.DownResourceDic.ContainsKey(code))
                    {
                        GameConfig.Instance.DownResourceDic.Add(code, tempImage);
                    }

                    downNum++;

                    EventDispatcher.FireEvent(EventKey.Bundle_SliderValue_Event,
                        BundleStepEnum.Bundle_TextureDown, downNum, TextureUpdateList.Count);

                    TextureUpdate_Down();
                }
            }
            else
            {
                QLoger.LOG(LogType.EError, " 请您稍后重试[4] www.error = " + www.error);

                BundleErrorPrompt(ResourceConfig.Bundle_Text517);
            }
        }

        #endregion -----------------------------------------------------------------------------------

        #region lyb ----------进入Xml更新流程---------------------------------------------------------

        void Flow_DownResourceClient()
        {
            gameObject.AddComponent<Resource_DownClient>();
        }

        #endregion -----------------------------------------------------------------------------------
    }
}