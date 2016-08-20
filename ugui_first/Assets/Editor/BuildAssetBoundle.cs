using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using System.Diagnostics;
public class BuildAssetBoundle
{

    // Use this for initialization
    [MenuItem("Publish/build")]
   public static void BuildBoundles()
    {
        UnityEngine.Debug.Log("execute calculate");
        List<AssetBundleBuild> buildList = new List<AssetBundleBuild>();
        AssetBundleBuild build = new AssetBundleBuild();
        build.assetBundleName = "test2";
        build.assetNames = new string[] { "Assets/Models/char_ethan.fbx" };
        buildList.Add(build);
        BuildPipeline.BuildAssetBundles("D:/GitHub_Repositories/rex_study/ugui_first/Assets/BoundleDir", buildList.ToArray(), BuildAssetBundleOptions.CompleteAssets | BuildAssetBundleOptions.CollectDependencies, BuildTarget.StandaloneWindows64);
        string strPath = FileUtil.GetProjectRelativePath("D:/GitHub_Repositories/rex_study/ugui_first/BoundleDir/test1.manifest");
        UnityEngine.Debug.Log("strPath is " + strPath);
        strPath = FileUtil.GetUniqueTempPathInProject();
        strPath = Application.dataPath + "../BoundleDir";
        UnityEngine.Debug.Log("strPath is " + strPath);
       // BuildPipeline.BuildPlayer(new string[] { "Assets/scene/ugui.unity" }, "D:/GitHub_Repositories/rex_study/ugui_first/Assets/BoundleDir/ugui.unity3d", BuildTarget.StandaloneWindows64, BuildOptions.None);
    }
}
