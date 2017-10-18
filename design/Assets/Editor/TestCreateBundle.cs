using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

public class TestCreateBundle : Editor
{

    //LoadFromFile

    [MenuItem("Test/Trunk_LZ4Bunle")]
    static void MakeLZ4Bunle()
    {
        BuildPipeline.BuildAssetBundles("Assets/AssetBundles/Trunk", BuildAssetBundleOptions.ChunkBasedCompression, BuildTarget.StandaloneWindows64);    
    }

}

