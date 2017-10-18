// C# Example
// Set Wrapmode of each imported texture to Clamp
//设置每个导入纹理的循环模式到Clamp
using UnityEngine;
using UnityEditor;
using System.Collections;

public class TexturePreprocessor : AssetPostprocessor
{
    //public void OnPreprocessTexture()
    //{
    //    TextureImporter textureImporter = assetImporter as TextureImporter;
    //    textureImporter.mipmapEnabled = false;
    //    textureImporter.textureType = TextureImporterType.Lightmap;
    //    textureImporter.wrapMode = TextureWrapMode.Repeat;
    //    string path = textureImporter.assetPath;

    //    Object asset = AssetDatabase.LoadAssetAtPath(path, typeof(Texture2D));
    //    Texture2D texture = asset as Texture2D;

    //    if (texture != null)
    //    {
    //        Debug.Log("Texture path: " + path);
    //        texture.wrapMode = TextureWrapMode.Clamp;
    //        EditorUtility.SetDirty(asset);
    //    }
    //    else
    //    {
    //        Debug.Log("error " + path);
    //    }
    //}
    //void OnPostprocessTexture(Texture2D texture)
    //{
    //}
    //[MenuItem("AssetDatabase/LoadAssetExample")]
    //static void ImportExample()
    //{
    //    Texture2D t = (Texture2D)AssetDatabase.LoadAssetAtPath("Assets/Tex_FriendAward.jpg", typeof(Texture2D));
    //}
}
