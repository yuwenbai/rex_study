/****************************************************
*
*  控制文件相关的工具类
*  作者: lyb
*  日期：2017年7月25日
*
*****************************************************/

using System;
using System.IO;
using UnityEngine;
using System.Collections.Generic;

public class Tool_FileOperation
{
    #region lyb 删除指定文件夹下的资源-----------------------

    /// <summary>
    /// 删除指定文件夹下的资源
    /// </summary>
    public static void Files_DeleteAll(string filePath)
    {
        try
        {
            DirectoryInfo dir = new DirectoryInfo(filePath);

            //返回目录中所有文件和子目录
            FileSystemInfo[] fileinfo = dir.GetFileSystemInfos();

            foreach (FileSystemInfo i in fileinfo)
            {
                //判断是否文件夹
                if (i is DirectoryInfo)
                {
                    DirectoryInfo subdir = new DirectoryInfo(i.FullName);

                    //删除子目录和文件
                    subdir.Delete(true);
                }
                else
                {
                    //删除指定文件
                    File.Delete(i.FullName);
                }
            }
        }
        catch (Exception e)
        {
            Debug.Log(e);
            throw;
        }
    }

    #endregion ----------------------------------------------

    #region 获取一个路径下所有文件的名字列表-----------------

    /// <summary>
    /// 获取一个路径下所有文件的名字列表
    /// filePath 文件的全路径
    /// </summary>
    public static List<string> Files_NameGetAll(string filePath)
    {
        List<string> result = new List<string>();

        FileInfo info = new FileInfo(filePath);

        if (info.Exists)
        {
            // 如果是文件
            result.Add(info.FullName);
        }
        else
        {
            //如果不是文件 ， 就是文件夹
            //获取到其中的所有文件
            string[] files = Directory.GetFiles(filePath);

            //获取到所有文件夹
            string[] dirs = Directory.GetDirectories(filePath);

            if (files != null && files.Length > 0)
            {
                foreach (var f in files)
                {
                    //添加文件
                    result.Add(f);
                }
            }

            if (dirs != null && dirs.Length > 0)
            {
                //如果是文件夹 ， 遍历下面的文件
                foreach (var f in dirs)
                {
                    //扫描文件夹
                    List<string> all = Files_NameGetAll(f);

                    foreach (var subf in all)
                    {
                        //添加扫描到的文件
                        result.Add(subf);
                    }
                }
            }
        }

        return result;
    }

    #endregion-----------------------------------------------

    #region 计算文件的大小-----------------------------------

    /// <summary>
    /// 计算文件的大小
    /// </summary>
    public static string Files_Size(string path)
    {
        FileInfo fileInfo = null;
        try
        {
            fileInfo = new FileInfo(path);
        }
        catch (Exception e)
        {
            Debug.Log(e.Message);

        }

        // 如果文件存在
        if (fileInfo != null && fileInfo.Exists)
        {
            System.Diagnostics.FileVersionInfo info = System.Diagnostics.FileVersionInfo.GetVersionInfo(path);
            return fileInfo.Length + "字节";
        }
        else
        {
            Debug.Log(" 指定的文件路径不正确! ");
        }
        return "";
    }

    #endregion ----------------------------------------------
}