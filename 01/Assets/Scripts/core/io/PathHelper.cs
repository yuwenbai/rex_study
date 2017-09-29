using UnityEngine;
using System.Collections;

namespace projectQ
{    
    public class PathHelper
    {
        /// <summary>
        /// 下载数据存储路径
        /// </summary>
        /// <value>The get data persistent path.</value>
        static public string PersistentPath {
            get { return UnityEngine.Application.persistentDataPath; }
        }

        /// <summary>
        /// Steaming Asset Path
        /// </summary>
        /// <value>The get data streaming asset path.</value>
        static public string StreamingAssetPath {
            get { return UnityEngine.Application.streamingAssetsPath; }
        }

        /// <summary>
        /// 带协议名称的路径
        /// </summary>
        /// <value>The protocal streaming asset path.</value>
        static public string ProtocalStreamingAssetPath
        {
            get {
                //编辑器路径
#if UNITY_EDITOR || UNITY_EDITOR_64 || UNITY_EDITOR_OSX
                return string.Format("file://{0}", StreamingAssetPath);
                //安卓真机路径，协议中自带了apk://协议名称
#elif UNITY_ANDROID
            return StreamingAssetPath;
            //其他平台路径 ios 走其他路径即可
#else
            return string.Format("file://{0}" , StreamingAssetPath);
#endif
            }
        }

        static public string GetProtocalDataPersiitentPath
        {
            get {
#if UNITY_ANDROID
                return PersistentPath;
#else
            return string.Format("file://{0}", PersistentPath);
#endif
            }
        }
    }
}

