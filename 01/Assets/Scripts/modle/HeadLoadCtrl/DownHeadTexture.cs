/****************************************************
*
*  管理微信头像
*  作者: lyb
*  日期：2017年7月25日
*
*****************************************************/

using System.IO;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace projectQ
{
    public class HeadTextureInfo
    {
        public Texture2D Tex2d;
        public int UseCount;

        public HeadTextureInfo(Texture2D tex2d, int useCount)
        {
            this.Tex2d = tex2d;
            this.UseCount = useCount;
        }
    }

    public class HeadTextureData
    {
        public string headUrl;
        public DownHeadTexture.WeChatHeadTexDelegate headTexBack;

        public HeadTextureData(string headUrl, DownHeadTexture.WeChatHeadTexDelegate headTexBack)
        {
            this.headUrl = headUrl;
            this.headTexBack = headTexBack;
        }
    }

    public class DownHeadTexture : BaseHeadLoad
    {
        public delegate void WeChatHeadTexDelegate(Texture2D HeadTexture, string HeadName);
        public WeChatHeadTexDelegate WeChatHeadGetCallBack;

        private static DownHeadTexture _Instance;
        public static DownHeadTexture Instance
        {
            get { return _Instance; }
        }

        /// <summary>
        /// 当前下载个数
        /// </summary>
        private int downNum = 0;

        /// <summary>
        /// 头像下载列表
        /// </summary>
        private List<HeadTextureData> HeadTextureDownList;

        void Awake()
        {
            _Instance = this;
        }

        void Start()
        {
            XmlPath = GameConfig.Instance.WeChat_HeadTextureXmlPath;
            HeadTextureDownList = new List<HeadTextureData>();
        }

        void OnDestroy()
        {
            _Instance = null;
            HeadDic = null;
        }

        #region lyb 活动界面根据传过来的url获取图片--------------------------

        public void Activity_TextureGet(string texUrl, WeChatHeadTexDelegate texBack)
        {
            if (GameConfig.Instance.IsDown == "0")
            {
                QLoger.LOG(" #[活动图片加载]# - 本地活动图片资源加载完毕，回调 ");

                Texture2D aTex = Texture_DefaultSet(GameAssetCache.Texture_Activity_Path);

                C2CHeadTextureCallBack(aTex, texBack);
            }
            else
            {
                Texture2D activityTex = null;

                if (GameConfig.Instance.DownResourceDic.ContainsKey(texUrl))
                {
                    QLoger.LOG(" #[活动图片加载]# - 下载下来的图片中有该资源，回调 ");

                    activityTex = GameConfig.Instance.DownResourceDic[texUrl];
                }
                else
                {
                    QLoger.LOG(" #[活动图片加载]# - 下载下来的图片中没有该资源，拿取本地资源回调 ");

                    activityTex = Texture_DefaultSet(GameAssetCache.Texture_Activity_Path);
                }

                C2CHeadTextureCallBack(activityTex, texBack);
            }
        }

        #endregion ----------------------------------------------------------

        #region lyb 根据微信传过来的Url获取对应的头像------------------------

        /*
		public IEnumerator downLoadTexture(string url, WeChatHeadTexDelegate headTexBack, UITexture texOwn = null)
        {
			bool isDownLoad = false ;
			if (!string.IsNullOrEmpty(url) && url.StartsWith("http://")) {
				WWW www = new WWW(url);
				Texture2D tempImage;
				yield return www;
				QLoger.LOG ("头像下载中" + url);
				if (www.isDone && www.error == null) {
					tempImage = www.texture;
					if (tempImage != null) {
						isDownLoad = true;
						C2CHeadTextureCallBack (tempImage, headTexBack, texOwn);
					}

					QLoger.LOG ("下载成功");
				} else {
					QLoger.LOG("下载失败");
				}
			}

			if(!isDownLoad){
				Debug.Log ("使用默认头像");

				Texture2D tex2d = Texture_DefaultSet(GameAssetCache.Texture_Hand_Path);
				C2CHeadTextureCallBack(tex2d, headTexBack, texOwn);
			}
		}    
        */

        public void WeChat_HeadTextureGet(string headUrl, WeChatHeadTexDelegate headTexBack)
        {
            if (string.IsNullOrEmpty(headUrl) || !headUrl.StartsWith("http://"))
            {
                WeChat_HeadTextureCheck(headUrl, headTexBack);
            }
            else
            {
                string headName = Texture_HeadNameSet(headUrl);

                QLoger.LOG(" #[微信头像]# - 下载头像的名字 = " + headName);

                if (HeadDic.ContainsKey(headName))
                {
                    WeChat_HeadTextureCheck(headUrl, headTexBack);
                }
                else
                {
                    QLoger.LOG(" #[微信头像]# - 添加到下载队列 ");

                    HeadTextureDownList.Add(new HeadTextureData(headUrl, headTexBack));

                    WeChat_HeadTextureSelect();
                }
            }
        }

        /// <summary>
        /// 从当前下载列表中选出执行下载
        /// </summary>
        void WeChat_HeadTextureSelect()
        {
            if (downNum <= HeadTexDownMax)
            {
                if (HeadTextureDownList.Count > 0)
                {
                    QLoger.LOG(" #[微信头像]# - 下载队列++ ");

                    downNum++;

                    HeadTextureData hData = HeadTextureDownList[0];

                    HeadTextureDownList.Remove(HeadTextureDownList[0]);

                    WeChat_HeadTextureCheck(hData.headUrl, hData.headTexBack);
                }
                else
                {
                    downNum = 0;
                }
            }
        }

        void WeChat_HeadTextureCheck(string headUrl, WeChatHeadTexDelegate headTexBack)
        {
            if (string.IsNullOrEmpty(headUrl) || !headUrl.StartsWith("http://"))
            {
                Texture2D tex2d = Texture_DefaultSet(GameAssetCache.Texture_Hand_Path);

                C2CHeadTextureCallBack(tex2d, headTexBack);
            }
            else
            {
                string headName = Texture_HeadNameSet(headUrl);

                string fileName = GameConfig.Instance.WeChat_HeadTexturePath + "/" + headName;

                if (HeadDic.ContainsKey(headName))
                {
                    QLoger.LOG(" #[微信头像]# - 该头像本地有缓存,返回 ");

                    C2CHeadTextureCallBack(HeadDic[headName].Tex2d, headTexBack, headName);

                    HeadTexture_SetValue(headName, HeadDic[headName].Tex2d);
                }
                else
                {
                    if (File.Exists(fileName))
                    {
                        QLoger.LOG(" #[微信头像]# - 该头像本地存在但无缓存,加载本地头像 ");

                        string url = "file://" + fileName;

#if UNITY_EDITOR || UNITY_STANDALONE_WIN
                        url = "file:///" + fileName;
#endif
                        StartCoroutine(DownLoadTexture(headName, url, false, headTexBack));
                    }
                    else
                    {
                        QLoger.LOG(" #[微信头像]# - 该头像不存在,去链接地址下载 headUrl = " + headUrl);

                        StartCoroutine(DownLoadTexture(headName, headUrl, true, headTexBack));
                    }
                }
            }
        }

        #endregion ----------------------------------------------------------

        /// <summary>
        /// isDown 是否开启下载
        /// </summary>
        IEnumerator DownLoadTexture(string headName, string url, bool isDown, WeChatHeadTexDelegate headTexBack)
        {
            WWW www = new WWW(url);
            Texture2D tempImage;
            yield return www;

            if (www.isDone && www.error == null)
            {
                tempImage = www.texture;

                if (tempImage != null)
                {
                    if (isDown)
                    {
                        byte[] data = tempImage.EncodeToPNG();

                        string fileName = GameConfig.Instance.WeChat_HeadTexturePath + "/" + headName;

                        DebugPro.Log(DebugPro.EnumLog.HeadUrl, "WWW保存头像 URL", url, " 本地地址", fileName);

                        File.WriteAllBytes(fileName, data);

                        HeadTexture_XmlAddSet(headName);
                    }

                    HeadTexture_SetValue(headName, tempImage);

                    QLoger.LOG(" #[微信头像]# - 头像获取成功,回调 ");
                }
                else
                {
                    tempImage = Texture_DefaultSet(GameAssetCache.Texture_Hand_Path);
                    QLoger.LOG(" #[微信头像]# - 加载失败,回调默认头像 ");
                }
            }
            else
            {
                tempImage = Texture_DefaultSet(GameAssetCache.Texture_Hand_Path);

                QLoger.LOG(" #[微信头像]# - 传过来的URL没有下载到资源，把本地的预设头像返回 ");

                if (www.error != null)
                {
                    QLoger.LOG(" #[微信头像]# - error = " + www.error);
                }
            }

            C2CHeadTextureCallBack(tempImage, headTexBack, headName);

            QLoger.LOG(" #[微信头像]# - 下载队列-- ");

            downNum--;

            WeChat_HeadTextureSelect();
        }

        #region lyb 回调方法统一调用-----------------------------------------

        /// <summary>
        /// 事件统一回调
        /// </summary>
        void C2CHeadTextureCallBack(Texture2D tex2d, WeChatHeadTexDelegate headTexBack, string headName = "")
        {
            if (tex2d != null)
            {
                if (headTexBack != null)
                {
                    headTexBack(tex2d, headName);
                }
            }
        }

        #endregion ----------------------------------------------------------
    }
}