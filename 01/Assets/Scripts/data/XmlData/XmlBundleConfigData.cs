/**
 * @Author lyb
 *
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;

namespace projectQ
{
    public class BundleConfigData
    {
        /// <summary>
        /// 资源服务器
        /// 【0】外网 , 【1】内网
        /// </summary>
        public string BundleType;
        /// <summary>
        /// 【0】本地资源加载 , 【1】服务器资源加载
        /// </summary>
        public string IsDown;
        /// <summary>
        /// 资源服务器地址
        /// </summary>
        public string ServerSite;
        /// <summary>
        /// 资源加载配置文件路径
        /// </summary>
        public string ServerSettingUrl;
    }

    public class XmlBundleConfigData : BaseXmlData
    {
        #region Xml解析完成的数据类存储------------------------------------------------

        private static List<BundleConfigData> _ConfigData = new List<BundleConfigData>();
        /// <summary>
        /// 资源加载的服务器路径
        /// </summary>
        public static List<BundleConfigData> ConfigData
        {
            get { return _ConfigData; }
        }

        #endregion --------------------------------------------------------------------

        public override void Init()
        {
            _ConfigData = BundleConfig_LoadXml();
        }

        /// <summary>
        /// 读取 “新手指南” 的Xml文件
        /// </summary>
        public List<BundleConfigData> BundleConfig_LoadXml()
        {
            XmlInit(XmlDataEnum.XML_BUNDLE_CONFIG);

            List<BundleConfigData> ConfigDataList = new List<BundleConfigData>();

            foreach (List<XmlCommonData> cData in XmlDataDic.Values)
            {
                BundleConfigData configData = new BundleConfigData();

                for (int i = 0; i < cData.Count; i++)
                {
                    switch (cData[i].XmlName)
                    {
                        default: break;
                        case "BundleType":
                            {
                                configData.BundleType = cData[i].XmlValue;
                            }
                            break;
                        case "IsDown":
                            {
                                configData.IsDown = cData[i].XmlValue;
                            }
                            break;
                        case "ServerSite":
                            {
                                configData.ServerSite = cData[i].XmlValue;
                            }
                            break;
                        case "ServerSettingUrl":
                            {
                                configData.ServerSettingUrl = cData[i].XmlValue;
                            }
                            break;
                    }
                }

                ConfigDataList.Add(configData);
            }

            return ConfigDataList;
        }
    }
}