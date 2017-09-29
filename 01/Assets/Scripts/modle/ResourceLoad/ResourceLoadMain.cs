/****************************************************
*
*  资源加载总的控制脚本
*  作者: lyb
*  日期：2017年7月25日
*
*****************************************************/

using UnityEngine;

namespace projectQ
{
    public class ResourceLoadMain : MonoBehaviour
    {
        /// <summary>
        /// 资源加载的主Obj
        /// </summary>
        private GameObject BundleObj;

        void OnEnable()
        {
            EventDispatcher.AddEvent(EventKey.Bundle_Restart_Event, Bundle_Restart);
            EventDispatcher.AddEvent(EventKey.Bundle_Load_Event, Bundle_Load);
            EventDispatcher.AddEvent(EventKey.Bundle_LoadName_Event, Bundle_LoadName);
            EventDispatcher.AddEvent(EventKey.Bundle_ResourceDownFinish_Event, C2CResourceDownFinish);
            EventDispatcher.AddEvent(EventKey.Bundle_UnEncryZipFinish_Event, C2CResourceUnEncryZipFinish);
            EventDispatcher.AddEvent(EventKey.Bundle_HeadTextureLoadFinish_Event, C2CHeadTextureLoadFinish);
            EventDispatcher.AddEvent(EventKey.Bundle_MusicDownBegin_Event, C2CMusicDownBegin);
            EventDispatcher.AddEvent(EventKey.Bundle_MusicDownFinish_Event, C2CMusicDownFinish);
            EventDispatcher.AddEvent(EventKey.Bundle_MusicDownAddMemory_Event, C2CMusicDownAddMeMory);

            Init();
        }

        void OnDisable()
        {
            EventDispatcher.RemoveEvent(EventKey.Bundle_Restart_Event, Bundle_Restart);
            EventDispatcher.RemoveEvent(EventKey.Bundle_Load_Event, Bundle_Load);
            EventDispatcher.RemoveEvent(EventKey.Bundle_LoadName_Event, Bundle_LoadName);
            EventDispatcher.RemoveEvent(EventKey.Bundle_ResourceDownFinish_Event, C2CResourceDownFinish);
            EventDispatcher.RemoveEvent(EventKey.Bundle_UnEncryZipFinish_Event, C2CResourceUnEncryZipFinish);
            EventDispatcher.RemoveEvent(EventKey.Bundle_HeadTextureLoadFinish_Event, C2CHeadTextureLoadFinish);
            EventDispatcher.RemoveEvent(EventKey.Bundle_MusicDownBegin_Event, C2CMusicDownBegin);
            EventDispatcher.RemoveEvent(EventKey.Bundle_MusicDownFinish_Event, C2CMusicDownFinish);
            EventDispatcher.RemoveEvent(EventKey.Bundle_MusicDownAddMemory_Event, C2CMusicDownAddMeMory);
        }

        void Init()
        {
            Bundle_ObjDestory();

            Bundle_ObjCreat();

            Bundle_DataSet<Resource_SliderValue>();
        }

        #region 资源加载开始--------------------------

        void Bundle_Load(object[] data)
        {
            _R.ui.OpenUI("UIResourceLoad");

            Bundle_DataSet<Resource_LoadServerInit>();
        }

        #endregion -----------------------------------

        #region 调用音乐音效下载接口------------------

        void C2CMusicDownBegin(object[] data)
        {
            if (data.Length > 0)
            {
                Bundle_ObjCreat();

                MusicDownTypeEnum downType = (MusicDownTypeEnum)data[0];

                if (downType == MusicDownTypeEnum.MusicDown_Null)
                {
                    Bundle_DataSet<BundleMusicInit>();
                }
                else
                {
                    Bundle_DataSet<BundleMusicCheck>().MusicDown_CheckInit(downType);
                }
            }
        }

        void C2CMusicDownFinish(object[] data)
        {
            if (data.Length > 0)
            {
                MusicDownTypeEnum downType = (MusicDownTypeEnum)data[0];

                if (downType == MusicDownTypeEnum.MusicDown_Bg)
                {
                    MusicCtrl.Instance.Music_BackChangePlay(GameAssetCache.M_Back_01_Path);

                    EventDispatcher.FireEvent(EventKey.Bundle_MusicDownBegin_Event, MusicDownTypeEnum.MusicDown_Sound);
                }
                else if (downType == MusicDownTypeEnum.MusicDown_Sound)
                {
                    StartCoroutine(UITools.WaitExcution(Init_AutomaticLogin, 1.0f));
                }
                else if (downType == MusicDownTypeEnum.MusicDown_VoiceMandarin)
                {

                }
                else if (downType == MusicDownTypeEnum.MusicDown_VoiceHeNan)
                {

                }
            }
            else
            {
                MusicCtrl.Instance.Music_BackChangePlay(GameAssetCache.M_Back_01_Path);

                StartCoroutine(UITools.WaitExcution(Init_AutomaticLogin, 1.0f));
            }
        }

        void C2CMusicDownAddMeMory(object[] data)
        {
            MusicDownTypeEnum downType = (MusicDownTypeEnum)data[0];

            Bundle_DataSet<BundleMusicLoad>().MusicLoad_Init(downType);
        }

        #endregion -----------------------------------

        #region 资源加载完毕，解密完毕事件回调--------

        /// <summary>
        /// 资源下载完毕
        /// </summary>
        void C2CResourceDownFinish(object[] data)
        {
            Bundle_DataSet<UnEncryptZip>();
        }

        /// <summary>
        /// 解密、解压缩完毕
        /// </summary>
        void C2CResourceUnEncryZipFinish(object[] data)
        {
            _R.Instance.ResourceLoadFinishScriptsAdd();

            Bundle_DataSet<HeadTextureLoad>();
        }

        /// <summary>
        /// 加载本地头像资源完毕
        /// </summary>
        void C2CHeadTextureLoadFinish(object[] data)
        {
            EventDispatcher.FireEvent(EventKey.Bundle_MusicDownBegin_Event, MusicDownTypeEnum.MusicDown_Null);
        }

        /// <summary>
        /// 检测是否有登录的Openid，如果有则开始自动登录
        /// </summary>
        void Init_AutomaticLogin()
        {
            Bundle_ObjDestory();

            _R.flow.SetQueue(QFlowManager.FlowType.InitLoginNew);
        }

        #endregion -----------------------------------

        #region Common--------------------------------

        /// <summary>
        /// 重新调用资源下载的接口
        /// </summary>
        void Bundle_Restart(object[] data)
        {
            Init();

            GameConfig.Instance.BundleDataClear();

            MemoryData.Set(MKey.USER_XML_DATA, null);

            _R.ui.OpenUI("UIResourceLoad");

            Bundle_DataSet<Resource_LoadServerInit>();
        }

        /// <summary>
        /// 创建Bundle挂载脚本
        /// </summary>
        void Bundle_ObjCreat()
        {
            if (BundleObj == null)
            {
                GameObject prefab = ResourcesDataLoader.Load<GameObject>(GameAssetCache.Prefab_BundleBox_Path);

                BundleObj = NGUITools.AddChild(gameObject, prefab);
            }
        }

        /// <summary>
        /// 销毁Bundle挂载脚本
        /// </summary>
        void Bundle_ObjDestory()
        {
            if (BundleObj != null)
            {
                GameObject.Destroy(BundleObj);

                BundleObj = null;
            }
        }

        public T Bundle_DataSet<T>() where T : BaseResourceLoad
        {
            if (BundleObj == null)
            {
                Bundle_ObjCreat();
            }

            T data = BundleObj.GetComponent<T>();
            if (data == null)
            {
                data = BundleObj.AddComponent<T>();
            }

            return data;
        }

        #endregion -----------------------------------

        #region 资源加载名字显示----------------------

        void Bundle_LoadName(object[] data)
        {
            string loadName = "";

            int index = (int)data[0];
            if (index == 1)
            {
                loadName = "正在读取服务器上的配置文件";
            }
            else if (index == 2)
            {
                loadName = "正在下载图片文件：" + (string)data[1];
            }
            else if (index == 3)
            {
                loadName = "正在下载Xml文件：" + (string)data[1];
            }
            else if (index == 4)
            {
                loadName = "正在加载图片文件：" + (string)data[1];
            }
            else if (index == 5)
            {
                loadName = "正在解密Xml文件：" + (string)data[1];
            }
            else if (index == 6)
            {
                loadName = "正在解压缩Xml文件：" + (string)data[1];
            }
            else if (index == 7)
            {
                loadName = "正在加载本地头像文件：" + (string)data[1];
            }
            else if (index == 8)
            {
                loadName = "正在下载音乐文件：" + (string)data[1];
            }

            //EventDispatcher.FireSysEvent(EventKey.Bundle_LoadingNameShow_Event, loadName);
        }

        #endregion -----------------------------------

        #region lyb 强制更新弹框提示------------------

        /// <summary>
        /// 需要进行大版本更新网站下载
        /// </summary>
        public static void AppDownPrompt()
        {
            MessageBoxMgr mBox = new MessageBoxMgr();
            mBox.Show("提示", ResourceConfig.Bundle_Text500, null,
                () =>
                {
                    //前往下载
                    Application.OpenURL(GameConfig.Instance.AppDownUrl);
                },
                () =>
                {
                    //关闭游戏
                    Application.Quit();
                }
                );
        }

        #endregion -----------------------------------

        #region lyb 游戏内自更新提示------------------

        /// <summary>
        /// 游戏内自更新
        /// </summary>
        public static void AppDownSelfPrompt()
        {
            WindowUIManager.Instance.CreateOrAddWindow(WindowUIType.SystemPopupWindow, "版本更新",
                ResourceConfig.Bundle_Text501, new string[] { "下载" }, delegate (int index)
                {
#if UNITY_ANDROID
                    BundleDownApkMgr bDown = new BundleDownApkMgr();
                    bDown.Show();
#elif UNITY_IOS
                    //前往下载
                    Application.OpenURL(GameConfig.Instance.AppDownUrl);
#endif
                });
        }

        /// <summary>
        /// 如果游戏包存在则执行安装操作
        /// </summary>
        public static void AppDownExistPrompt()
        {
            WindowUIManager.Instance.CreateOrAddWindow(WindowUIType.SystemPopupWindow, "版本更新",
                ResourceConfig.Bundle_Text502, new string[] { "安装" }, delegate (int index)
                {
                    AppDownInstall();
                });
        }

        /// <summary>
        /// 游戏包下载完成执行安装操作
        /// </summary>
        public static void AppDownInstall()
        {
            DebugPro.LogBundle(" #【下载Apk】# --------- 执行安装操作 ");
            SDKData.InstallAPK data = new SDKData.InstallAPK();
            data.path = GameConfig.Instance.ApkLocal_Path;
            data.name = GameConfig.Instance.ApkLocal_Name;
            ////DO install apk
            SDKManager.Instance.InstallAPK(data.toString());
        }

        #endregion -----------------------------------
    }
}