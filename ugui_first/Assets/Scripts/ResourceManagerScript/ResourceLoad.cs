//using UnityEngine;
//using System.Collections;
//using UnityEditor;
//public class ResourceLoad : MonoBehaviour
//{

//    // Use this for initialization
//    void Start()
//    {
//        string strMaterialName = "Assets/Materials/prop_battleBus_lights_fx.mat";
//        Material mat = AssetDatabase.LoadAssetAtPath(strMaterialName, typeof(Material)) as Material;
//        Debug.Log("ResourceLoad material'maintexture name is " + mat.mainTexture.name);
//        StartCoroutine(LoadAssetsBoundle());

//        //unload resource 
//        //Resources.UnloadAsset(mat);
//        // Debug.Log("ResourceLoad material'maintexture name is " + mat.mainTexture.name);
//        // Material newMat = Object.Instantiate<Material>(mat);
//        //AssetDatabase.CreateAsset(newMat, "Assets/selfAssets/newprop_battleBus_lights_fx.mat");
//    }

//    //// Update is called once per frame
//    //void Update () {

//    //}
//    IEnumerator LoadAssetsBoundle()
//    {
//        WWW www = new WWW("file://D:/GitHub_Repositories/rex_study/ugui_first/Assets/BoundleDir/test2");
//        yield return www;
//        AssetBundle aBoudle = www.assetBundle;
//        string[] assetNames = aBoudle.GetAllAssetNames();
//        for (int i = 0; i < assetNames.Length; ++i)
//        {
//            Debug.Log("ffffffffffff is " + assetNames.GetValue(i));
//        }
//        AssetBundleRequest request = aBoudle.LoadAssetAsync(assetNames.GetValue(0).ToString());
//        yield return request;
//        GameObject obj = request.asset as GameObject;
//        Instantiate(obj);
//        www.Dispose();
//    }
//}
