using UnityEngine;
using UnityEditor;

public class TestCreateLZ4Bundle : ScriptableObject
{
    //LoadFromCacheOrDownload
    //LoadFromMemory

    [MenuItem("Test/Patch_LZMABunle")]
    static void MakeLZMABunle()
    {
        BuildPipeline.BuildAssetBundles("Assets/AssetBundles/Patch", BuildAssetBundleOptions.None, BuildTarget.StandaloneWindows64);
    }
}