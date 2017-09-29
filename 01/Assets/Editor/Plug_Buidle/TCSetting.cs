/**
 * @Author YQC
 *
 *
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

/// <summary>
/// TC功能设置
/// </summary>
public class TCSetting
{
    private static string TcConfigPath = Application.dataPath + "/../teamcity.default.properties";
    public static void SetVersion(string version)
    {
        FileInfo fi = new FileInfo(TcConfigPath);
        using (StreamWriter sw = fi.CreateText())
        {
            sw.WriteLine("env.version_number=" + version);
        }
    }
}
