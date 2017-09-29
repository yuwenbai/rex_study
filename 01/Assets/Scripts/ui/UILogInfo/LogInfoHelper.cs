/**
* @Author lyb
* 读取本地的Log日志文件工具类
*
*/

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace projectQ
{
    public class LogInfoHelper
    {
        #region lyb 获取指定路径下的所有的文件--------------------------

        /// <summary>
        /// 获取指定路径下的所有的文件
        /// </summary>
        public static List<FileInfo> LogFile_AllGet(string filePath)
        {
            List<FileInfo> logList = new List<FileInfo>();

            DirectoryInfo dirInfo = new DirectoryInfo(filePath);

            FileInfo[] fileList = dirInfo.GetFiles();

            for (int i = 0; i < fileList.Length; i++)
            {
                if (fileList[i].Extension == ".log")
                {
                    logList.Add(fileList[i]);
                }
            }

            return logList;
            //FileLog_Cerat(logList);
        }

        #endregion------------------------------------------------------

        #region lyb 读取一个text文件的每一行数据------------------------

        /// <summary>
        /// 获取一个text文件的每一行数据
        /// </summary>
        /// <param name="path">读取文件的路径</param>
        /// <param name="name">读取文件的名称</param>
        /// <returns></returns>
        public static ArrayList LogFile_LoadData(string path, string name)
        {
            StreamReader sr = null;
            try
            {
                sr = File.OpenText(path + "//" + name);
            }
            catch (Exception e)
            {
                //路径与名称未找到文件则直接返回空
                return null;
            }

            string lineStr;
            ArrayList arrlist = new ArrayList();
            while ((lineStr = sr.ReadLine()) != null)
            {
                if (!string.IsNullOrEmpty(lineStr))
                {
                    arrlist.Add(lineStr);
                }
            }
            sr.Close();
            sr.Dispose();
            return arrlist;
        }

        #endregion------------------------------------------------------

        #region lyb 删除指定文件夹下的资源------------------------------

        /// <summary>
        /// 删除指定文件夹下的资源
        /// </summary>
        public static void LogFile_AllDelete(string filePath)
        {
            Tools_FileOperation.Files_DeleteAll(filePath);
        }

        #endregion -----------------------------------------------------
    }
}