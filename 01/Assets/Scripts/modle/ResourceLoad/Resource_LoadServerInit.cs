/****************************************************
*
*  读取服务器的xml中的版本号信息
*  作者: lyb
*  日期：2017年7月25日
*
*****************************************************/

using UnityEngine;
using System.Collections;

namespace projectQ
{
    public class Resource_LoadServerInit : BaseResourceLoad
    {
        void Start()
        {
            InitData();
        }

        #region lyb ----------初始化本地文本资源，获取服务器资源xml路径-----------------------------

        void InitData()
        {
            Xml_ResourceUrlGet();

            if (GameConfig.Instance.Xml_ServerUrl != "")
            {
                if (GameConfig.Instance.IsDown == "1")
                {
                    EventDispatcher.FireEvent(EventKey.Bundle_LoadName_Event, 1, "...");
                    StartCoroutine(GetServerXml());
                }
                else
                {
                    EventDispatcher.FireEvent(EventKey.Bundle_UnEncryZipFinish_Event);
                }
            }
            else
            {
                QLoger.LOG(LogType.ELog, " 资源配置错误 ");

                BundleErrorPrompt(ResourceConfig.Bundle_Text514);
            }
        }

        #endregion ----------------------------------------------------------------------------------

        #region lyb ----------初始化本地xml文件，获取资源链接地址，进行资源更新----------------------

        void Xml_ResourceUrlGet()
        {
            XmlBundleConfigData data = new XmlBundleConfigData();

            XmlData.XmlInit<XmlBundleConfigData>(data);

            string bundleType = "0";

#if __BUNDLE_TEST_SERVER
            bundleType = "-1";
#elif __BUNDLE_PRE_OUTER_SERVER
            bundleType = "2";
#elif __BUNDLE_OUTER_SERVER
            bundleType = "1";
#elif __BUNDLE_IOS_SERVER
            bundleType = "3";
#endif

            foreach (BundleConfigData cData in XmlBundleConfigData.ConfigData)
            {
                if (cData.BundleType.Equals(bundleType))
                {
                    GameConfig.Instance.IsDown = cData.IsDown;
                    GameConfig.Instance.Resource_ServerPath = cData.ServerSite;
                    GameConfig.Instance.Xml_ServerUrl = cData.ServerSettingUrl;
                }
            }

#if __BUNDLE_DOWN
            GameConfig.Instance.IsDown = "1";
#endif

            DebugPro.LogBundle(" #[资源加载]# LoadVersionServer - 从本地xml文件中获取服务器资源地址 = " + GameConfig.Instance.Xml_ServerUrl);
        }

        #endregion ----------------------------------------------------------------------------------

        #region lyb ----------读取服务器上的版本控制xml----------------------------------------------

        IEnumerator GetServerXml()
        {
            WWW www = new WWW(GameConfig.Instance.Xml_ServerUrl);
            yield return www;

            if (www.isDone && www.error == null)
            {
                LoadXml_Server(www);
            }
            else
            {
                QLoger.LOG(LogType.ELog, " 请您稍后重试[0] www.error = " + www.error);

                BundleErrorPrompt(ResourceConfig.Bundle_Text515);
            }
        }

        /// <summary>
        /// 解析服务器上获得的xml
        /// </summary>
        void LoadXml_Server(WWW www)
        {
            if (string.IsNullOrEmpty(www.text))
            {
                BundleErrorPrompt(ResourceConfig.Bundle_Text516);
                return;
            }

            Xml_Load(XmlLoadEnum.XmlLoad_Server, www.text);

            Flow_ResourceClientCheck();
        }

        #endregion -----------------------------------------------

        #region lyb 进入下一步流程--------------------------------

        void Flow_ResourceClientCheck()
        {
            gameObject.AddComponent<Resource_CheckClient>();
        }

        #endregion -----------------------------------------------
    }
}