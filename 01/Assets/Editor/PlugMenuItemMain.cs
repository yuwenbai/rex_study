/****************************************************
*
*  Editor调用总控制类
*  作者: lyb
*  日期：2017年7月25日
*
*****************************************************/

using UnityEditor;
using UnityEngine;

public class PlugMenuItemMain
{
    #region Exl相关方法调用 --------------------------

    [MenuItem("Tools/ConfigTools/Plug_Excel/ExcelReadOnce")]
    public static void ExcelReadOnce()
    {
        PlugProject.Plug_ExcelReader.isReadAll = false;
        PlugProject.Plug_ExcelReader.Excel_ReadOnce();
    }

    [MenuItem("Tools/ConfigTools/Plug_Excel/ExcelReadAll")]
    public static void ExcelReadAll()
    {
        PlugProject.Plug_ExcelReader.isReadAll = true;
        PlugProject.Plug_ExcelReader.Excel_ReadAll();
    }

    [MenuItem("Tools/ConfigTools/Plug_Excel/ExcelReadClear")]
    public static void ExcelReadClear()
    {
        EditorUtility.ClearProgressBar();
    }

    #endregion ---------------------------------------

    #region SqitLite生成Xml --------------------------

    [MenuItem("Tools/ConfigTools/Plug_SqLite/SqLiteXmlCreat")]
    public static void SqLiteXmlCreat()
    {
        PlugProject.Plug_SQLiteCreat.SqLite_XmlCreat();
    }

    #endregion ---------------------------------------

    #region 压缩，加密相关 ---------------------------

    [MenuItem("Tools/ConfigTools/Plug_Encrypt/Zip")]
    public static void EncryptZipCreat()
    {
        PlugProject.Plug_EncryptCreat.Encrypt_ZipCreat();
    }

    [MenuItem("Tools/ConfigTools/Plug_Encrypt/DESEncrypt")]
    public static void EncryptDESCreat()
    {
        PlugProject.Plug_EncryptCreat.Encrypt_DESCreat();
    }

    #endregion ---------------------------------------

    #region 创建音乐Bundle配置文件 -------------------

    [MenuItem("Tools/ConfigTools/Plug_MusicXml/MusicXmlCreat")]
    public static void MusicXmlCreat()
    {
        PlugProject.Plug_MusicSettingXml.MusicXmlCreat();
    }

    #endregion ---------------------------------------

    #region 调用面板，显示Log日志 --------------------

    public static void PlugLog_Show(string logStr)
    {
        Debug.Log(logStr);

        BuildUpdateEditor.MakeDirView.GetInstance.OnLog(logStr);
    }

    #endregion ---------------------------------------

    [MenuItem("Tools/ConfigTools/ApkMd5")]
    public static void ApkMd5()
    {
        string path = EditorUtility.OpenFilePanel("Load Excel", "", "apk");

        if (path.Length != 0)
        {
            string apkMd5 = Tools_Md5.MD5_File(path);
            Debug.Log("Md5值 = " + apkMd5);
        }
    }
}
