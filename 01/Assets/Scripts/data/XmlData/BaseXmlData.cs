/**
 * @Author lyb
 * Xml数据类基类
 *
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;

namespace projectQ
{
    #region Xml共有数据类-------------------------------------------------------------------------

    public class XmlCommonData
    {
        /// <summary>
        /// Xml名字
        /// </summary>
        public string XmlName;
        /// <summary>
        /// Xml内容
        /// </summary>
        public string XmlValue;
    }

    #endregion -----------------------------------------------------------------------------------

    public abstract class BaseXmlData
    {
        public abstract void Init();

        /// <summary>
        /// 当前解析Xml的存储数据字典
        /// Key值代表的是第几个子节点 ，Value值代表的是子节点中的每一条数据
        /// </summary>
        public Dictionary<int, List<XmlCommonData>> XmlDataDic = new Dictionary<int, List<XmlCommonData>>();

        #region lyb----------------Xml解析共有类--------------------------------------------------

        /// <summary>
        /// 初始化Xml
        /// </summary>
        public void XmlInit(string xmlName)
        {
            XmlDocument xml = new XmlDocument();

            if (xmlName == "BundleConfig")
            {
                string xmldata = Resources.Load(XmlFilePath_Get(xmlName, 0)).ToString();
                xml.LoadXml(xmldata);
            }
            else
            {
                if (GameConfig.Instance.IsDown == "0")
                {
                    string xmldata = Resources.Load(XmlFilePath_Get(xmlName, 0)).ToString();
                    xml.LoadXml(xmldata);
                }
                else
                {
                    XmlReaderSettings set = new XmlReaderSettings();
                    set.IgnoreComments = true;
                    XmlReader reader = XmlReader.Create(XmlFilePath_Get(xmlName, 1), set);
                    xml.Load(reader);
                    reader.Close();
                }
            }

            XmlNodeValue_Get(xml);
        }


        /// <summary>
        /// 获取Xml路径地址
        /// index = 0 读取本地路径的xml
        /// index = 1 读取下载下来的路径的xml
        /// </summary>
        private string XmlFilePath_Get(string xmlName, int index)
        {
            string path = "";

            if (index == 0)
            {
                path = "ClientConfig/XmlCreatFile/" + xmlName;
            }
            else
            {
                //path = GameConfig.Instance.XmlLocal_Path + "/" + xmlName + ".xml";
                path = GameConfig.Instance.XmlUnZip_Path + "/" + xmlName + ".xml";
            }

            return path;
        }

        #endregion -------------------------------------------------------------------------------

        #region lyb----------------公共方法-------------------------------------------------------

        /// <summary>
        /// 获取xml Node的数据
        /// </summary>
        void XmlNodeValue_Get(XmlDocument xml)
        {
            XmlDataDic = new Dictionary<int, List<XmlCommonData>>();

            //得到objects节点下的所有子节点
            if (xml.SelectSingleNode("objects") == null) return;
            XmlNodeList xmlNodeList = xml.SelectSingleNode("objects").ChildNodes;

            int index = 0;
            foreach (XmlNode xnode in xmlNodeList)
            {
                index++;
                List<XmlCommonData> commonList = new List<XmlCommonData>();

                for (int i = 0; i < xnode.ChildNodes.Count; i++)
                {
                    XmlCommonData commonData = new XmlCommonData();
                    commonData.XmlName = xnode.ChildNodes[i].Name;
                    commonData.XmlValue = xnode.ChildNodes[i].InnerText;

                    commonList.Add(commonData);
                }

                XmlDataDic.Add(index, commonList);
            }
        }

        #endregion -------------------------------------------------------------------------------

        #region lyb----------------Xml解析共有类--------------------------------------------------

        /// <summary>
        /// 初始化Xml
        /// </summary>
        public XmlDocument XmlInit(XmlDataEnum xmlData)
        {
            XmlDocument xml = new XmlDocument();

            if (xmlData == XmlDataEnum.XML_BUNDLE_CONFIG)
            {
                string xmldata = Resources.Load(XmlPathGet(xmlData, 0)).ToString();
                xml.LoadXml(xmldata);
            }
            else
            {
                if (GameConfig.Instance.IsDown == "0")
                {
                    string xmldata = Resources.Load(XmlPathGet(xmlData, 0)).ToString();
                    xml.LoadXml(xmldata);
                }
                else
                {
                    XmlReaderSettings set = new XmlReaderSettings();
                    set.IgnoreComments = true;
                    XmlReader reader = XmlReader.Create(XmlPathGet(xmlData, 1), set);
                    xml.Load(reader);
                    reader.Close();
                }
            }

            XmlNodeValue_Get(xml);

            return xml;
        }

        /// <summary>
        /// 获取Xml路径地址
        /// index = 0 读取本地路径的xml
        /// index = 1 读取下载下来的路径的xml
        /// </summary>
        private string XmlPathGet(XmlDataEnum xmlData, int index)
        {
            string path = "";

            switch (xmlData)
            {
                case XmlDataEnum.XML_BUNDLE_CONFIG:
                    if (index == 0)
                    {
                        path = "ClientConfig/BundleConfig";
                    }
                    else
                    {
                        path = GameConfig.Instance.XmlLocal_Path + "/BundleConfig.xml";
                    }
                    break;
                case XmlDataEnum.XML_GAMECONFIG_REGION:
                    if (index == 0)
                    {
                        path = "ClientConfig/XmlCreatFile/GameConfigRegion";
                    }
                    else
                    {
                        //path = GameConfig.Instance.XmlLocal_Path + "/GameConfigRegion.xml";
                        path = GameConfig.Instance.XmlUnZip_Path + "/GameConfigRegion.xml";
                    }
                    break;
                case XmlDataEnum.XML_GAMECONFIG_FASHION:
                    if (index == 0)
                    {
                        path = "ClientConfig/XmlCreatFile/GameConfigFashion";
                    }
                    else
                    {
                        //path = GameConfig.Instance.XmlLocal_Path + "/GameConfigFashion.xml";
                        path = GameConfig.Instance.XmlUnZip_Path + "/GameConfigFashion.xml";
                    }
                    break;
            }

            return path;
        }

        #endregion -------------------------------------------------------------------------------
    }
}