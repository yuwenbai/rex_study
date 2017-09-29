/****************************************************
*
*  本地资源检测, 确定是直接下载还是走本地加载
*  作者: lyb
*  日期：2017年7月25日
*
*****************************************************/

using System.IO;

namespace projectQ
{
    public class Resource_CheckClient : BaseResourceLoad
    {
        void Start()
        {
            ResourceCheckInit();
        }

        void ResourceCheckInit()
        {
            this.Resource_FileCheck();

            if (!CheckVersion())
            {
                //大版本更新，网站下载
                ResourceLoadMain.AppDownPrompt();
            }
            else
            {
                if (!CheckVersionSelfUpdate())
                {
                    //游戏内自更新
                    ApkLocalCheck();
                }
                else
                {
                    Tools_FileOperation.Files_DeleteAll(GameConfig.Instance.ApkLocal_Path);

                    LocalFile_Check();
                }
            }
        }

        #region lyb 判断是本地安装还是下载apk---------------------

        void ApkLocalCheck()
        {
            string apkName = GameConfig.Instance.ApkLocal_Path + GameConfig.Instance.Version_Server + ".apk";

            if (File.Exists(apkName))
            {
                GameConfig.Instance.ApkLocal_Name = GameConfig.Instance.Version_Server + ".apk";

                string apkMd5 = Tools_Md5.MD5_File(apkName).ToLower();
                string remoteMd5 = GameConfig.Instance.ApkMd5.ToLower();
                if (apkMd5.Equals(remoteMd5))
                {
                    //apk - Md5值相同提示安装操作
                    //apk包存在执行安装操作
                    ResourceLoadMain.AppDownExistPrompt();
                }
                else
                {
                    //apk - Md5值不相同下载
                    ResourceLoadMain.AppDownSelfPrompt();
                }
            }
            else
            {
                //apk包不存在执行下载操作
                ResourceLoadMain.AppDownSelfPrompt();
            }
        }

        #endregion -----------------------------------------------

        #region lyb 判断当前资源加载是直接下载还是走对比更新------

        /// <summary>
        /// 判断当前资源更新的文件夹是否存在
        /// </summary>
        void LocalFile_Check()
        {
            if (File.Exists(GameConfig.Instance.Xml_ClientPath))
            {
                DebugPro.LogBundle(" #[资源加载]# ResourceClientCheck - 本地记录版本信息文件存在 ");
                ResourceLocal_Read();
            }
            else
            {
                DebugPro.LogBundle(" #[资源加载]# ResourceClientCheck - 本地记录版本信息文件不存在 ");
                Resource_Down();
            }
        }

        /// <summary>
        /// 本地记录版本信息文件存在, 读取，加载本地资源到列表
        /// </summary>
        void ResourceLocal_Read()
        {
            Xml_Load(XmlLoadEnum.XmlLoad_Client);

            bool isDown = ResourceCorrect_Check(GameConfig.Instance.TextureList_Client,
                    GameConfig.Instance.ResourceLocal_Path, ".png");
            if (isDown)
            {
                DebugPro.LogBundle(" #[资源加载]# ResourceClientCheck - 客户端上资源与本地存储资源不匹配，重新更新下载 ");
                Resource_Down();
            }
            else
            {
                DebugPro.LogBundle(" #[资源加载]# ResourceClientCheck - 进行图片资源版本比对 ");
                Flow_TextureUpdate();
            }
        }

        /// <summary>
        /// 本地记录版本信息文件不存在, 直接下载
        /// </summary>
        void Resource_Down()
        {
            Tools_FileOperation.Files_DeleteAll(GameConfig.Instance.ResourceLocal_Path);
            Tools_FileOperation.Files_DeleteAll(GameConfig.Instance.XmlLocal_Path);

            if (File.Exists(GameConfig.Instance.Xml_ClientPath))
            {
                File.Delete(GameConfig.Instance.Xml_ClientPath);
            }

            Flow_DownResourceServer();
        }

        #endregion -----------------------------------------------

        #region lyb 检测本地文件夹是否存在，不存在则创建----------

        void Resource_FileCheck()
        {
            if (!Directory.Exists(GameConfig.Instance.ResourceLocal_Path))
            {
                Directory.CreateDirectory(GameConfig.Instance.ResourceLocal_Path);
            }

            if (!Directory.Exists(GameConfig.Instance.XmlLocal_Path))
            {
                Directory.CreateDirectory(GameConfig.Instance.XmlLocal_Path);
            }

            if (!Directory.Exists(GameConfig.Instance.ApkLocal_Path))
            {
                Directory.CreateDirectory(GameConfig.Instance.ApkLocal_Path);
            }
        }

        #endregion -----------------------------------------------

        #region lyb 进入下一步流程--------------------------------

        void Flow_TextureUpdate()
        {
            gameObject.AddComponent<TextureUpdate>();
        }

        void Flow_DownResourceServer()
        {
            gameObject.AddComponent<Resource_DownServer>();
        }

        #endregion -----------------------------------------------
    }
}