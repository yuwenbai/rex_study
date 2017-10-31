using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.IO;
public class ExportBundle
{
    static ExportConfig sConfig = new ExportConfig();
    #region 入口
    [UnityEditor.MenuItem("Export/Export Bundle/For Android")]
    static void ExportAllBundleForAndroid()
    {
        ExportAllBundle(BuildTarget.Android);
    }

    [UnityEditor.MenuItem("Export/Export Bundle/For IPhone")]
    static void ExportAllBundleForIOS()
    {
        ExportAllBundle(BuildTarget.iOS);
    }
    #endregion

    #region 导出逻辑
    static void ExportAllBundle(BuildTarget _target)
    {
        sConfig.LoadConfig();
        string tpath = sConfig.sDefaultFolder + sConfig.sAppName + sConfig.GetTartFolder(_target);

        List<AssetBundleBuild> builds = new List<AssetBundleBuild>();

        DirectoryInfo tdirfolder = new DirectoryInfo(sConfig.sResourcesPath);
        FileInfo[] tfileinfos = tdirfolder.GetFiles("*.*", System.IO.SearchOption.AllDirectories);
        string tapppath = System.IO.Directory.GetCurrentDirectory() + "\\";

        foreach(FileInfo tfile in tfileinfos)
        {
            if (tfile.Name.EndsWith(".meta")) continue;
            AssetBundleBuild tbuild = new AssetBundleBuild();
            tbuild.assetBundleName = tfile.Name + ".bytes";
            string tRelativePath = tfile.FullName;
            tRelativePath = tRelativePath.Replace(tapppath,"");
            tRelativePath = tRelativePath.Replace("\\","/");
            tbuild.assetNames = new string[] { tRelativePath };
            builds.Add(tbuild);
        }
       
        GoExport(tpath,builds.ToArray(),_target);
    }

    static void GoExport(string _path,AssetBundleBuild[] _builds, BuildTarget _target)
    {
        AssetBundleManifest tmainfest = BuildPipeline.BuildAssetBundles(_path, _builds, sConfig.sBuildOption, _target);
        Debug.Log("导出完成!");
    }
    #endregion

    #region 移动资源

    [UnityEditor.MenuItem("Export/Move Bundle/Move Android to StreamingPath")]
    static void MoveAndroidBundleToStreamPath()
    {
        MoveBUndleToStreamingPath(BuildTarget.Android);
    }

    [UnityEditor.MenuItem("Export/Move Bundle/Move IOS to StreamingPath")]
    static void MoveIOSBundleToStreamPath()
    {
        MoveBUndleToStreamingPath(BuildTarget.iOS);
    }

    [UnityEditor.MenuItem("Export/Move Bundle/Move Android to SidePath")]
    static void MoveAndroidBundleToSide()
    {
        MoveBundleToSideDate(BuildTarget.Android);
    }

    [UnityEditor.MenuItem("Export/Move Bundle/Move IOS to SidePath")]
    static void MoveIOSBundleToStreamSide()
    {
        MoveBundleToSideDate(BuildTarget.iOS);
    }

    static void MoveBUndleToStreamingPath(BuildTarget _target)
    {
        sConfig.LoadConfig();
        string tpath = sConfig.sDefaultFolder + sConfig.sAppName + sConfig.GetTartFolder(_target);
        string tfullpath = System.IO.Directory.GetCurrentDirectory() + "\\" + sConfig.sStreamingBundleFolder + sConfig.sAppName;
        tfullpath = tfullpath.Replace("\\", "/");
        MoveToPath(tpath, tfullpath, sConfig.GetTartFolder(_target));
    }

    static void MoveBundleToSideDate(BuildTarget _target)
    {
        sConfig.LoadConfig();
        string tpath = sConfig.sDefaultFolder + sConfig.sAppName + sConfig.GetTartFolder(_target);
        string tfullpath = System.IO.Directory.GetCurrentDirectory() + "\\" + sConfig.sEditorBundleFolder + sConfig.sAppName;
        tfullpath = tfullpath.Replace("\\", "/");
        MoveToPath(tpath, tfullpath, sConfig.GetTartFolder(_target));
    }

    static void DeleteAllFile(string _path)
    {
        DirectoryInfo tdirfolder = new DirectoryInfo(_path);
        FileInfo[] tfileinfos = tdirfolder.GetFiles("*.*", System.IO.SearchOption.AllDirectories);
        foreach (FileInfo tfile in tfileinfos)
            File.Delete(tfile.FullName);
        Debug.Log("清除目录结束.");
    }

    static void MoveToPath(string _socPath,string _desPath,string _targetname)
    {
        DeleteAllFile(_desPath);

        string tmainfest = _targetname.Replace("/", "") + ".manifest";
        File.Copy(_socPath + tmainfest, _desPath + "/" + "appmanifest.manifest");
       

        DirectoryInfo tdirfolder = new DirectoryInfo(_socPath);

        FileInfo[] tfileinfos = tdirfolder.GetFiles("*.*", System.IO.SearchOption.AllDirectories);

        if (!Directory.Exists(_desPath))
            Directory.CreateDirectory(_desPath);

        foreach (FileInfo tfile in tfileinfos)
        {
            if (tfile.Name.EndsWith(".meta") || tfile.Name.EndsWith(".manifest") || !tfile.Name.Contains(".")) continue;
            File.Copy(tfile.FullName, _desPath + "/" + tfile.Name);
        }

        Debug.Log("移动完成.");
    }
    #endregion
}
