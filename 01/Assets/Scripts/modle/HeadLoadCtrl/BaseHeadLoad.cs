/****************************************************
*
*  微信头像加载工具类
*  作者: lyb
*  日期：2017年7月25日
*
*****************************************************/

using System.IO;
using System.Xml;
using UnityEngine;
using System.Collections.Generic;

namespace projectQ
{
    public abstract class BaseHeadLoad : MonoBehaviour
    {
        /// <summary>
        /// 允许的最大头像数量
        /// </summary>
        private const int WeChatNumMax = 100;
        /// <summary>
        /// 允许字典中的最大缓存头像数量
        /// </summary>
        private const int HeadDicNumMax = 30;
        /// <summary>
        /// 微信头像保存的xml的节点名字
        /// </summary>
        private const string XmlNodeName = "WeChatHead";

        /// <summary>
        /// 允许最大下载并发数
        /// </summary>
        public const int HeadTexDownMax = 2;

        /// <summary>
        /// 微信头像存储的xml文件路径
        /// </summary>
        private string _XmlPath;
        public string XmlPath
        {
            get { return _XmlPath; }
            set { _XmlPath = value; }
        }

        /// <summary>
        /// 当局游戏中下载的头像缓存
        /// </summary>
        private Dictionary<string, HeadTextureInfo> _HeadDic = new Dictionary<string, HeadTextureInfo>();
        public Dictionary<string, HeadTextureInfo> HeadDic
        {
            get { return _HeadDic; }
            set { _HeadDic = value; }
        }

        #region lyb 获取本地的默认图片----------------------------------

        /// <summary>
        /// 获取本地的默认图片
        /// </summary>
        public Texture2D Texture_DefaultSet(string texPath)
        {
            Texture2D tex2d = ResourcesDataLoader.Load<Texture2D>(texPath);

            if (tex2d != null)
            {
                return tex2d;
            }

            return null;
        }

        #endregion -----------------------------------------------------

        #region lyb 解析发过来的头像链接，获取头像名字------------------

        /// <summary>
        /// 解析发过来的头像链接，获取头像名字
        /// </summary>
        public string Texture_HeadNameSet(string headUrl)
        {
            string headName = "";

            string[] values = headUrl.Split(new char[] { '/' });
            if (values.Length > 1)
            {
                headName = values[values.Length - 1];
                if (headName.Trim().Length <= 1)
                {
                    if (values.Length >= 2)
                    {
                        headName = values[values.Length - 2];
                    }
                }
            }
            else
            {
                headName = values[0];
            }

            return headName;
        }

        #endregion ------------------------------------------------------

        #region lyb 往字典里存储当前头像的索引或者增加使用次数-----------

        /// <summary>
        /// 头像字典赋值
        /// headName 头像名字 key键
        /// tex2d 头像texture
        /// </summary>
        public void HeadTexture_SetValue(string headName, Texture2D tex2d)
        {
            if (!HeadDic.ContainsKey(headName))
            {
                //字典里没有该头像
                if (HeadDic.Count >= HeadDicNumMax)
                {
                    HeadDic.Remove(HeadTexture_UseMinGet());
                }
                HeadDic.Add(headName, new HeadTextureInfo(tex2d, 0));
            }
            else
            {
                //字典里有该头像，使用次数++
                HeadDic[headName].UseCount++;
            }
        }

        /// <summary>
        /// 获取头像使用次数最小的头像名字
        /// </summary>
        private string HeadTexture_UseMinGet()
        {
            string useMinName = "";

            string minName = "";
            int min = 999999;

            foreach (KeyValuePair<string, HeadTextureInfo> kvp in HeadDic)
            {
                int count = kvp.Value.UseCount;

                if (min > count)
                {
                    min = count;
                    minName = kvp.Key;
                }
            }

            return useMinName;
        }

        #endregion ------------------------------------------------------

        #region lyb 把下载下来的头像信息写入xml存储----------------------

        /// <summary>
        /// 把下载下来的头像信息写入xml存储
        /// </summary>
        public void HeadTexture_XmlAddSet(string hName)
        {
            if (!File.Exists(XmlPath))
            {
                // 创建xml文件
                Tools_XmlOperation.Xml_Creat(XmlPath);
                Tools_XmlOperation.Xml_CreatElement(XmlPath, XmlNodeName);
            }

            if (WeChatHeadTexNum_Check(hName) == null)
            {
                Tools_XmlOperation.Xml_Add(XmlPath, XmlNodeName, hName, "");
            }

            WeChatHeadTex_Check();
        }

        #endregion ----------------------------------------------------------

        #region lyb 检测本地的图片是否超出指定张数，超出的话把第一张删掉-----

        public void WeChatHeadTex_Check()
        {
            string headName = WeChatHeadTexNum_Check();
            if (headName != null)
            {
                // 说明有值要执行删除操作
                string path = GameConfig.Instance.WeChat_HeadTexturePath + "/" + headName;

                if (File.Exists(path))
                {
                    File.Delete(path);
                }

                Tools_XmlOperation.Xml_Delete(XmlPath, XmlNodeName, headName);
            }
        }

        /// <summary>
        /// 检查xml数据。。如果超出范围则把照片名字返回回来
        /// </summary>
        private string WeChatHeadTexNum_Check(string hName = "")
        {
            if (File.Exists(XmlPath))
            {
                XmlDocument xml = new XmlDocument();

                xml.Load(XmlPath);

                XmlNodeList xmlNodeList = xml.SelectSingleNode("objects").ChildNodes;

                foreach (XmlElement element in xmlNodeList)
                {
                    if (element.GetAttribute("id").Equals(XmlNodeName))
                    {
                        if (string.IsNullOrEmpty(hName))
                        {
                            //如果传进来的是空串 则判断当前有无超过最大范围值
                            if (element.ChildNodes.Count > WeChatNumMax)
                            {
                                //超出最大头像数量，把第一个头像删掉
                                XmlElement xl2 = element.ChildNodes[0] as XmlElement;

                                return xl2.GetAttribute("name");
                            }
                        }
                        else
                        {
                            //否则 判断传进来的头像是否存在于xml中
                            foreach (XmlElement nodes in element.ChildNodes)
                            {
                                if (nodes.GetAttribute("name").Equals(hName))
                                {
                                    return hName;
                                }
                            }
                        }
                    }
                }
            }
            return null;
        }

        #endregion ----------------------------------------------------------
    }
}