/**
* @Author lyb
* 获取更改客户端存储的版本号
*
*/

using UnityEditor;
using UnityEngine;
using System.Collections;

public class VersionClient
{
    public static string VersionClientPath = Application.dataPath + "/Resources/ClientConfig/Version_Client.xml";

    #region 配置文件版本号修改 ------------------------------------

    public static void Versionclient_Update(string newVersion)
    {
        Tool_XmlOperation.Xml_UpdateVersion(VersionClientPath, newVersion);
        TCSetting.SetVersion(newVersion);
        Tool_EditorCoroutineRunner.StartEditorCoroutine(AssectRefresh());        
    }

    private static IEnumerator AssectRefresh()
    {
        yield return 1.0f;

        Debug.Log(" #[VersionClient]# Refresh 一下");

        AssetDatabase.Refresh();
    }

    #endregion ----------------------------------------------------
}
