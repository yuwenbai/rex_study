/**
* @Author GarFey
* 
*/
using UnityEditor;
using UnityEngine;

public class S369Editor : MonoBehaviour
{
    [MenuItem("369Editor/打开缓存目录")]
    public static void OpenPersistentData()
    {
        System.Diagnostics.Process.Start(Application.persistentDataPath);
    }
    [MenuItem("369Editor/Scene/Main")]
    public static void OpenMainScene()
    {
       
        EditorApplication.OpenScene(Application.dataPath + "/Scene/Main.unity");
    }
    [MenuItem("369Editor/Scene/Empty")]
    public static void OpenEmptyScene()
    {

        EditorApplication.OpenScene(Application.dataPath + "/Scene/Empty.unity");
    }
    [MenuItem("369Editor/Scene/MahJong")]
    public static void OpenMahJongScene()
    {

        EditorApplication.OpenScene(Application.dataPath + "/Scene/MahJong.unity");
    }
    [MenuItem("369Editor/Scene/Animator")]
    public static void OpenAnimatorScene()
    {

        EditorApplication.OpenScene(Application.dataPath + "/Scene/Animator.unity");
    }
    [MenuItem("369Editor/Scene/MyMahJongTable")]
    public static void OpenMyMahJongTableScene()
    {

        EditorApplication.OpenScene(Application.dataPath + "/Scene/MyMahJongTable.unity");
    }
    [MenuItem("369Editor/Scene/TestAnimMahjong")]
    public static void OpenTestAnimMahjongScene()
    {

        EditorApplication.OpenScene(Application.dataPath + "/Scene/TestAnimMahjong.unity");
    }
    [MenuItem("369Editor/Scene/testMahjong")]
    public static void OpentestMahjongScene()
    {

        EditorApplication.OpenScene(Application.dataPath + "/Scene/testMahjong.unity");
    }
}
