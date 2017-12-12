using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

public class TestCreateBundle : Editor
{

    //LoadFromFile
    [MenuItem("Test/Export Bundle")]
    public static void MakeLZ4Bunle()
    {
        //BuildPipeline.BuildAssetBundles("Assets/AssetBundles/Trunk", BuildAssetBundleOptions.ChunkBasedCompression, BuildTarget.StandaloneWindows64);
        //CollectResourceAndBuilds(BuildTarget.Android);
        //CollectResourceAndBuilds(BuildTarget.iOS);
        BuildPipeline.BuildAssetBundles(Application.streamingAssetsPath,BuildAssetBundleOptions.ChunkBasedCompression,BuildTarget.Android);
    }
    static void CollectResourceAndBuilds(BuildTarget _target)
    {
        List<AssetBundleBuild> builds = new List<AssetBundleBuild>();
        DirectoryInfo tdirfolder = new DirectoryInfo(TestEditorConfig.CollectResourceLocation);
        FileInfo[] tfileinfos = tdirfolder.GetFiles("*.*", System.IO.SearchOption.AllDirectories);
        string tapppath = System.IO.Directory.GetCurrentDirectory() + "\\";
        foreach (FileInfo tfile in tfileinfos)
        {
            if (tfile.Name.EndsWith(".meta")) continue;
            AssetBundleBuild build = new AssetBundleBuild();
            build.assetBundleName = tfile.Name + ".rex";
            string tRelativePath = tfile.FullName;
            tRelativePath = tRelativePath.Replace(tapppath, "");
            tRelativePath = tRelativePath.Replace("\\", "/");
            build.assetNames = new string[] { tRelativePath };
            builds.Add(build);
        }
        DoExport(TestEditorConfig.ExportResourceLocation, builds.ToArray(), _target);
    }
    static void DoExport(string path, AssetBundleBuild[] _builds, BuildTarget _target)
    {
        BuildPipeline.BuildAssetBundles(path, _builds, BuildAssetBundleOptions.ChunkBasedCompression, _target);
        //string tmanifestname = sConfig.GetTartFolder(_target).Replace("/", "");
        //string tdespathname = _path + "/" + sConfig.sAppName.Replace("/", "") + LitEngine.Loader.BaseBundle.sSuffixName;
        //if (File.Exists(tdespathname))
        //    File.Delete(tdespathname);
        //File.Copy(_path + tmanifestname, tdespathname);
        Debug.Log("导出完成!");
    }

}

