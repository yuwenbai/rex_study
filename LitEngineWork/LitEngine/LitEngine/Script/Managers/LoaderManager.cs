using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
namespace LitEngine
{
    namespace Loader
    {
        public class LoaderManager : MonoManagerBase
        {
            private static string DataPath = "Data/";
            #region 属性
            private string mAppName = "";
            private BundleVector mBundleList = null;
            private LoadTaskVector mBundleTaskList = null;
            private Dictionary<string, BaseBundle> mWaitLoadBundleList = null;
            #region PATH_LOADER
            private static string StreamingDataPath = null;
            private static string ResDataPath = null;
            private static string PersistentDataPath = null;
            #endregion
            #endregion

            static public string GetDataPath()
            {
                if (ResDataPath == null)
                    ResDataPath = GetPersistentDataPath() + DataPath;
                return ResDataPath;
            }

            static public string GetResourcesDataPath(string _filename)
            {
                string tfullpathname = Path.Combine(DataPath, _filename);
                return tfullpathname;
            }

            static public string GetFullPath(string _filename)
            {
                string tfullpathname = Path.Combine(GetDataPath(), _filename);
                if (!File.Exists(tfullpathname))
                    tfullpathname = Path.Combine(LoaderManager.GetStreamingDataPath(), _filename);
                return tfullpathname;
            }
            static public string GetStreamingDataPath()
            {
                if (StreamingDataPath == null)
                    StreamingDataPath = Path.Combine(Application.streamingAssetsPath + "/", DataPath);
                return StreamingDataPath;
            }
            static public string GetPersistentDataPath()
            {
                if (PersistentDataPath == null)
                {
                    if (Application.platform != RuntimePlatform.WindowsEditor && Application.platform != RuntimePlatform.WindowsPlayer)
                    {
                        PersistentDataPath = Application.persistentDataPath + "/";
                    }
                    else
                    {
                        PersistentDataPath = Application.dataPath + "/../";
                    }
                }
                return PersistentDataPath;
            }
            static public bool IsFileExistsSD(string _filename)
            {
                string tpath = GetDataPath();
                if (!File.Exists(Path.Combine(tpath, _filename)))
                    return false;
                return true;
            }
            public LoaderManager()
            {
                mWaitLoadBundleList = new Dictionary<string, BaseBundle>();
                mBundleList = new BundleVector();
                mBundleTaskList = new LoadTaskVector();
            }

            override public void DestroyManager()
            {
                if (mWaitLoadBundleList.Count != 0 || mBundleTaskList.TaskList.Count != 0)
                    throw new System.NotSupportedException("删除App的LoaderManager时,发现仍然有未完成的加载动作.请确保加载完成后正确调用.");
                mBundleTaskList.Clear();
                RemoveAllBundle(true);
                base.DestroyManager();
            }

            #region update
            void Update()
            {
                if (mWaitLoadBundleList.Count > 0)
                {
                    List<string> tlist = new List<string>(mWaitLoadBundleList.Keys);
                    foreach (string tkey in tlist)
                    {
                        if (!mWaitLoadBundleList.ContainsKey(tkey)) continue;
                        if (mWaitLoadBundleList[tkey].IsDone())
                            mWaitLoadBundleList.Remove(tkey);
                    }
                }

                if (mBundleTaskList.Count > 0)
                {
                    List<long> ttasklist = new List<long>(mBundleTaskList.Keys);
                    foreach (long tkey in ttasklist)
                    {
                        if (!mBundleTaskList.ContainsKey(tkey)) continue;
                        mBundleTaskList[tkey].IsDone();
                    }
                }


                if (mWaitLoadBundleList.Count == 0 && mBundleTaskList.TaskList.Count == 0)
                    ActiveLoader(false);
            }
            #endregion

            #region fun
            void ActiveLoader(bool _active)
            {
                if (gameObject.activeInHierarchy != _active)
                    gameObject.SetActive(_active);
            }
            #endregion

            #region 资源管理

            public BaseBundle GetBundle(string _key)
            {
                if (mBundleList.Contains(_key))
                    return mBundleList[_key];
                return null;
            }

            public void ReleaseBundle(string _key)
            {
                mBundleList.ReleaseBundle(_key);
            }

            public void RemoveTask(LoadTask _obj)
            {
                mBundleTaskList.Remove(_obj);
            }
            public void RemoveAllBundle(bool _clear)
            {
                mBundleList.Clear(_clear);
            }

            public void RemoveBundle(string _key, bool _clear)
            {
                mBundleList.Remove(_key, _clear);
            }

            public void RemoveBundleAndClear(string _key, bool _Clear)
            {
                mBundleList.Remove(_key, _Clear);
            }
            public void ClearAllGC1(bool _clear)
            {
                mBundleList.ClearAllGC1(_clear);
            }
            public void ClearGC1(string _key, bool _clear)
            {
                mBundleList.ClearGC1(_key, _clear);
            }

            #endregion
            private LoadTask CreatTaskAndStart(string _key, BaseBundle _bundle, System.Action<string, object, object> _callback, System.Action<string, float> _progress, object _Targetobj)
            {
                LoadTask ret = new LoadTask(_key, _bundle, _callback, _progress, _Targetobj);
                mBundleTaskList.Add(ret);
                return ret;
            }

            #region 资源载入

            #region 同步载入
            #region Res资源
            /// <summary>
            /// 载入Resources资源
            /// </summary>
            /// <param name="_relativePathName">_curPathname 是相对于path/Date/下的路径 例如目录结构Assets/Resources/Date/ +_curPathname</param>
            /// <returns></returns>
            public Object LoadResourcesBundleByRelativePathName(string _relativePathName)
            {
                if (_relativePathName == null || _relativePathName.Equals("")) return null;
                if (mBundleList.Contains(_relativePathName))
                {
                    mBundleList[_relativePathName].Retain();
                    return (Object)mBundleList[_relativePathName].Asset;
                }

                ResourcesBundle tbundle = new ResourcesBundle(_relativePathName, "");
                tbundle.Load();
                if (tbundle.Asset != null)
                {
                    tbundle.Retain();
                    mBundleList.Add(tbundle);
                    return (Object)tbundle.Asset;
                }
                return null;
            }
            #endregion
            //使用前需要设置datapath 默认为 Data _assetname 
            public AssetsBundleFromFile LoadAssetsBundleByNameNoCache(string _relativePathName)
            {
                AssetsBundleFromFile tbundle = new AssetsBundleFromFile(_relativePathName, "");
                tbundle.Load();
                if (tbundle.Asset != null)
                    return tbundle;
                return null;
            }
            public Object LoadAssetsBundleByName(string _relativePathName)
            {
                return LoadAssetsBundleByPathName(_relativePathName);
            }
            public Object LoadAssetsBundleByPathName(string _relativePathName)
            {
                if (_relativePathName == null || _relativePathName.Equals("")) return null;

                if (mBundleList.Contains(_relativePathName))
                {
                    mBundleList[_relativePathName].Retain();
                    return (Object)mBundleList[_relativePathName].Asset;
                }

                AssetsBundleFromFile tbundle = new AssetsBundleFromFile(_relativePathName, "");
                tbundle.Load();
                //Debug.Log("loader---|"+ _assetname);
                if (tbundle.Asset != null)
                {
                    tbundle.Retain();
                    mBundleList.Add(tbundle);
                    return (Object)tbundle.Asset;
                }
                return null;
            }
            #endregion
            #region 异步载入
            /// <summary>
            /// 异步加载AssetsBundle
            /// </summary>
            ///  <param name="_type">资源类型<param>
            ///  <param name="_key">任务建立者标识<param>
            ///  <param name="_relativePathName">例:Application.streamingAssetsPath/Data/&(_relativePathName)<param>
            ///  <param name="_callback">回调函数<param>
            ///  <param name="_progress">进度回调<param>
            ///  <param name="_target">载入的资源是准备给谁用的 Target <param>
            public void LoadResourcesBundleByRelativePathNameAsync(string _key, string _relativePathName, string _resname, System.Action<string, object, object> _callback, System.Action<string, float> _progress, object _target)
            {
                if (_relativePathName.Length == 0)
                {
                    DLog.LOG(DLogType.Error,"LoadResourcesBundleByRelativePathNameAsync -- _relativePathName 的长度不能为空");
                }
                if (_callback == null)
                {
                    DLog.LOG(DLogType.Error,"LoadResourcesBundleByRelativePathNameAsync -- CallBack Fun can not be null");
                    return;
                }

                if (mBundleList.Contains(_relativePathName))
                {
                    if (mBundleList[_relativePathName].Loaded)
                    {
                        if (mBundleList[_relativePathName].Asset == null)
                            DLog.LOG(DLogType.Error,"ResourcesBundleAsync-erro in vector。文件载入失败,请检查文件名:" + _relativePathName);
                        mBundleList[_relativePathName].Retain();
                        _callback(_key, mBundleList[_relativePathName].Asset, _target);
                    }
                    else
                    {
                        CreatTaskAndStart(_key, mBundleList[_relativePathName], _callback, _progress, _target);
                        ActiveLoader(true);
                    }

                }
                else
                {
                    ResourcesBundleAsync tbund = new ResourcesBundleAsync(_relativePathName, _resname);
                    tbund.Load();
                    mBundleList.Add(tbund);
                    mWaitLoadBundleList.Add(tbund.AssetName, tbund);
                    CreatTaskAndStart(_key, tbund, _callback, _progress, _target);
                    ActiveLoader(true);
                }
            }

            /// <summary>
            /// 异步加载AssetsBundle
            /// </summary>
            ///  <param name="_type">资源类型<param>
            ///  <param name="_key">任务建立者标识<param>
            ///  <param name="_relativePathName">例:Application.streamingAssetsPath/Data/&(_relativePathName)<param>
            ///  <param name="_callback">回调函数<param>
            ///  <param name="_progress">进度回调<param>
            ///  <param name="_target">载入的资源是准备给谁用的 Target <param>
            public void LoadAssetsBundleByRelativePathNameAsync(string _key, string _relativePathName, string _resname, System.Action<string, object, object> _callback, System.Action<string, float> _progress, object _target)
            {
                if (_relativePathName.Length == 0)
                {
                    DLog.LOG(DLogType.Error,"LoadAssetsBundleByFullNameAsync -- _relativePathName 的长度不能为空");
                }
                if (_callback == null)
                {
                    DLog.LOG(DLogType.Error,"LoadAssetsBundleByFullNameAsync -- CallBack Fun can not be null");
                    return;
                }

                if (mBundleList.Contains(_relativePathName))
                {
                    if (mBundleList[_relativePathName].Loaded)
                    {
                        if (mBundleList[_relativePathName].Asset == null)
                            DLog.LOG(DLogType.Error,"AssetsBundleAsyncFromFile-erro in vector。文件载入失败,请检查文件名:" + _relativePathName);
                        mBundleList[_relativePathName].Retain();
                        _callback(_key, mBundleList[_relativePathName].Asset, _target);
                    }
                    else
                    {
                        CreatTaskAndStart(_key, mBundleList[_relativePathName], _callback, _progress, _target);
                        ActiveLoader(true);
                    }

                }
                else
                {
                    AssetsBundleAsyncFromFile tbund = new AssetsBundleAsyncFromFile(_relativePathName, _resname);
                    tbund.Load();
                    mBundleList.Add(tbund);
                    mWaitLoadBundleList.Add(tbund.AssetName, tbund);
                    CreatTaskAndStart(_key, tbund, _callback, _progress, _target);
                    ActiveLoader(true);
                }
            }
            #endregion
            #region WWW载入
            public void WWWLoad(string _key, string _FullName, System.Action<string, object, object> _callback, System.Action<string, float> _progress, object _target)
            {
                if (_callback == null)
                {
                    DLog.LOG(DLogType.Error,"assetsbundle -- CallBack Fun can not be null");
                    return;
                }

                if (mBundleList.Contains(_FullName))
                {
                    if (mBundleList[_FullName].Loaded)
                    {
                        if (mBundleList[_FullName].Asset == null)
                            DLog.LOG(DLogType.Error,"WWWLoad-erro in vector。文件载入失败,请检查文件名:" + _FullName);
                        mBundleList[_FullName].Retain();
                        _callback(_key, mBundleList[_FullName].Asset, _target);
                    }
                    else
                    {
                        CreatTaskAndStart(_key, mBundleList[_FullName], _callback, _progress, _target);
                        ActiveLoader(true);
                    }

                }
                else
                {
                    WWWBundle tbund = new WWWBundle(_FullName, "");
                    tbund.Load();
                    mBundleList.Add(tbund);
                    mWaitLoadBundleList.Add(tbund.AssetName, tbund);
                    CreatTaskAndStart(_key, tbund, _callback, _progress, _target);
                    ActiveLoader(true);
                }
            }
            #endregion
            #endregion
        }

    }
}

