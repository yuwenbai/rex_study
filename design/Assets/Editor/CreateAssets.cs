using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

public class CreateAssets : Editor {

    [MenuItem("CreateAssets/TestAssets")]
    static void Create()
    {
        //
        ScriptableObject bullet = ScriptableObject.CreateInstance<TestAssets>();
        if (!bullet)
        {
            return;
        }
        string path = Application.dataPath + "/BulletAsset";
        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
        }
        path = string.Format("Assets/BulletAsset/{0}.asset", (typeof(CreateAssets).ToString()));
        AssetDatabase.CreateAsset(bullet, path);
    }
}
