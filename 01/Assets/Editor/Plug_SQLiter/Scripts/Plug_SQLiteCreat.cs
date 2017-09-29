/****************************************************
*
*  读取Sqlite数据库里的数据，生成对应的xml
*  作者: lyb
*  日期：2017年7月25日
*
*****************************************************/

using System;
using System.IO;
using UnityEngine;
using UnityEditor;
using Mono.Data.Sqlite;
using System.Collections.Generic;

namespace PlugProject
{
    public class Plug_SQLiteCreat
    {
        // 数据库数据表名字存储 
        private static List<string> tabelList = new List<string>();

        // 数据库数据表名字为key ， 字段名列表list为value 
        public static Dictionary<string, List<string>> Tabel_FieldDic = new Dictionary<string, List<string>>();

        // 特殊数据库数据存储字典，直接通过数据流生成文件
        private static List<string> Tabel_OutPutList = new List<string>();

        private static string CreatXmlPath = Application.dataPath + "/Resources/ClientConfig/XmlCreatFile";

        public static void SqLite_XmlCreat()
        {
            SQLiteHelper sql = new SQLiteHelper("data source=" + Application.dataPath + "/Editor/Plug_SQLiter/Databases/game_conf.db");

            // 查询获取数据库中的所有的表名----------------------------------------

            SqliteDataReader reader = sql.ExecuteQuery("SELECT name FROM sqlite_master WHERE type='table' ORDER BY name");

            string rName = reader.GetName(0);

            tabelList = new List<string>();

            while (reader.Read())
            {
                string tName = reader.GetString(reader.GetOrdinal(rName));

                tabelList.Add(tName);

                PlugMenuItemMain.PlugLog_Show(" #[Plug_SQLiteCreat]# - 读取出来的数据库里的表名 = " + tName);
            }

            // 查询获取表中的所有的字段名-------------------------------------------

            Tabel_FieldDic = new Dictionary<string, List<string>>();
            Tabel_OutPutList = new List<string>();

            foreach (string tabelName in tabelList)
            {
                SqliteDataReader rField = sql.ReadFullTable(tabelName);

                List<string> fieldList = new List<string>();
                for (int i = 0; i < rField.FieldCount; i++)
                {
                    string fieldName = rField.GetName(i);

                    fieldList.Add(fieldName);
                }

                if (tabelName.Equals("xml_output"))
                {
                    Tabel_OutPutList.Add(tabelName);
                }
                else
                {
                    Tabel_FieldDic.Add(tabelName, fieldList);
                }
            }

            // 根据字典里的数据获取每张表中每个字段的数据----------------------------

            if (!Directory.Exists(CreatXmlPath))
            {
                Directory.CreateDirectory(CreatXmlPath);
            }
            else
            {
                Tool_FileOperation.Files_DeleteAll(CreatXmlPath);

                AssetDatabase.Refresh();
            }

            foreach (KeyValuePair<string, List<string>> kv in Tabel_FieldDic)
            {
                int index = 1;

                string xPath = CreatXmlPath + "/" + kv.Key + ".xml";
                Tool_XmlOperation.Xml_Creat(xPath);

                SqliteDataReader sqlitData = sql.ReadFullTable(kv.Key);
                while (sqlitData.Read())
                {
                    Tool_XmlOperation.Xml_CreatElement(xPath, index.ToString());
                    foreach (string eStr in kv.Value)
                    {
                        string valueStr = sqlitData.GetString(sqlitData.GetOrdinal(eStr));
                        Tool_XmlOperation.Xml_Add(xPath, index.ToString(), eStr, valueStr, 1);
                    }
                    index++;

                    string doc = kv.Key + ".xml";
                    bool isCancel = EditorUtility.DisplayCancelableProgressBar("Xml生成中", doc, (float)index / (float)sqlitData.FieldCount);
                }
            }

            foreach (string outPutTable in Tabel_OutPutList)
            {
                SqliteDataReader sqlitData = sql.ReadFullTable(outPutTable);
                while (sqlitData.Read())
                {
                    string tableName = sqlitData.GetString(sqlitData.GetOrdinal("FileName"));
                    string tableValue = sqlitData.GetString(sqlitData.GetOrdinal("XML"));

                    string xPath = CreatXmlPath + "/" + tableName + ".xml";

                    if (!File.Exists(xPath))
                    {
                        FileStream fs = new FileStream(xPath, FileMode.OpenOrCreate);
                        StreamWriter sw = new StreamWriter(fs);
                        sw.WriteLine(tableValue);
                        sw.Close();
                        fs.Close();
                    }
                }
            }

            EditorUtility.ClearProgressBar();

            PlugMenuItemMain.PlugLog_Show(" #[Plug_SQLiteCreat]# - Xml创建完成 ");

            AssetDatabase.Refresh();

            Plug_XmlBuild.XmlBuild();
        }
    }
}