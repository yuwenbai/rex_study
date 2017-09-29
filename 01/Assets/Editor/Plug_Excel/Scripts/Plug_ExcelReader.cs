/****************************************************
*
*  读取Exl表格到SqLiter数据库
*  作者: lyb
*  日期：2017年7月25日
*
*****************************************************/

using Excel;
using System.IO;
using System.Data;
using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

namespace PlugProject
{
    public class Plug_ExcelReader : ExcelReaderHelper
    {
        /// <summary>
        /// 是否是导入的全部数据表
        /// </summary>
        public static bool isReadAll = false;
        /// <summary>
        /// 数据表路径
        /// </summary>
        public static string ExcelPath = @"E:\SVN\public\Config";
        /// <summary>
        /// 当前exl数据表路径列表
        /// </summary>
        public static List<FileInfo> ExcelPathList;
        /// <summary>
        /// 当前导入的exl数据表数量
        /// </summary>
        public static int ExcelReadCount = 0;

        #region 读取Excel -----------------------------------------

        /// <summary>
        /// 读取单张数据表，弹框选择
        /// </summary>
        public static void Excel_ReadOnce()
        {
            string path = EditorUtility.OpenFilePanel("Load Excel", ExcelPath, "xlsx");

            if (path.Length != 0)
            {
                Excel_ReadData(path);
            }
        }

        /// <summary>
        /// 读取所有数据表，筛选exl
        /// </summary>
        public static void Excel_ReadAll()
        {
            string path = EditorUtility.OpenFolderPanel("Load Excel All", ExcelPath, "");

            if (path.Length != 0)
            {
                ExcelPathList = new List<FileInfo>();

                ExcelReadCount = 0;

                DirectoryInfo dirInfo = new DirectoryInfo(path);

                FileInfo[] fileList = dirInfo.GetFiles();

                for (int i = 0; i < fileList.Length; i++)
                {
                    if (fileList[i].Extension == ".xlsx")
                    {
                        ExcelPathList.Add(fileList[i]);
                    }
                }

                Excel_ImportData();
            }
        }

        /// <summary>
        /// 开始导入数据表
        /// </summary>
        private static void Excel_ImportData()
        {
            if (ExcelReadCount < ExcelPathList.Count)
            {
                EditorUtility.DisplayProgressBar("Read All Excel", ExcelPathList[ExcelReadCount].Name,
                    (float)(ExcelReadCount + 1) / (float)ExcelPathList.Count);

                Excel_ReadData(ExcelPathList[ExcelReadCount].FullName);
            }
            else
            {
                EditorUtility.ClearProgressBar();
            }
        }

        #endregion --------------------------------------------------

        #region 获取Excel里的数据 -----------------------------------

        public static void Excel_ReadData(string exlPath)
        {
            //EG :  http://exceldatareader.codeplex.com

            FileStream stream = File.Open(exlPath, FileMode.Open, FileAccess.Read, FileShare.Read);

            // 97-2003 format *.xls
            //IExcelDataReader excelReader = ExcelReaderFactory.CreateBinaryReader (stream);
            //DataSet result = excelReader.AsDataSet();

            // 2007 format *.xlsx
            IExcelDataReader excelReader = ExcelReaderFactory.CreateOpenXmlReader(stream);
            excelReader.IsFirstRowAsColumnNames = true;
            DataSet result = excelReader.AsDataSet();

            List<string> ExlReaderList = new List<string>();
            while (excelReader.Read())
            {
                string readerStr = "";
                for (int i = 0; i < excelReader.FieldCount; i++)
                {
                    if (i <= excelReader.FieldCount - 2)
                    {
                        readerStr += excelReader.GetString(i) + "#";
                    }
                    else
                    {
                        readerStr += excelReader.GetString(i);
                    }
                }
                ExlReaderList.Add(readerStr);
            }

            excelReader.Close();
            stream.Close();

            Exlcel_DataWriteInSqlLite(excelReader.Name, ExlReaderList);
        }

        #endregion --------------------------------------------------

        #region 把Excel里的数据写入数据库 ---------------------------

        /// <summary>
        /// 数据表中的数据写入数据库
        /// isTabel 数据表是否存在
        /// </summary>
        public static void Exlcel_DataWriteInSqlLite(string tableName, List<string> ExlReaderList)
        {
            SQLiteHelper sql = new SQLiteHelper("data source=" + Application.dataPath + "/Editor/Plug_SQLiter/Databases/game_conf.db");

            string deleteStr = string.Format("DROP TABLE IF EXISTS '{0}';", tableName);

            sql.ExecuteQuery(deleteStr);

            //bool isTabel = IsTableExist(tableName, sql);
            bool isTabel = false;

            int index = 0;
            foreach (string strData in ExlReaderList)
            {
                string[] fieldArray = strData.Split(new char[] { '#' });

                if (isTabel)
                {
                    // 数据表存在
                    if (index >= 2)
                    {
                        //sql.InsertValues("table2", new string[] { "'10012'", "'你是谁'", "'112'", "'这是1段描述'" });

                        string[] fArray = new string[fieldArray.Length];

                        for (int i = 0; i < fieldArray.Length; i++)
                        {
                            fArray[i] = "'" + fieldArray[i] + "'";
                        }

                        sql.InsertValues(tableName, fArray);
                    }
                }
                else
                {
                    //数据表不存在
                    if (index == 1)
                    {
                        string[] typeArray = new string[fieldArray.Length];

                        for (int i = 0; i < typeArray.Length; i++)
                        {
                            typeArray[i] = "TEXT";
                        }

                        sql.CreateTable(tableName, fieldArray, typeArray);

                        //创建完数据表之后打开写入数据表开关
                        isTabel = true;
                    }
                }

                index++;
            }

            sql.CloseConnection();

            PlugMenuItemMain.PlugLog_Show(" #[Plug_ExcelReader]# - 写入数据库完毕 = " + tableName);

            if (isReadAll)
            {
                ExcelReadCount++;

                Excel_ImportData();
            }
        }

        #endregion --------------------------------------------------
    }
}