/****************************************************
*
*  动态生成xml结构数据类
*  作者: lyb
*  日期：2017年7月25日
*
*****************************************************/

using System;
using System.IO;
using System.Text;
using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

namespace PlugProject
{
    public class Plug_XmlBuild
    {
        public static string BuildPath;

        public static void XmlBuild()
        {
            BuildPath = Application.dataPath + "/Scripts/modle" + "/XmlBuild.cs";

            if (File.Exists(BuildPath))
            {
                File.Delete(BuildPath);
            }

            XmlBuild_Creat();
        }

        /// <summary>
        /// 创建
        /// </summary>
        public static void XmlBuild_Creat()
        {
            if (!File.Exists(BuildPath))
            {
                FileStream fs = new FileStream(BuildPath, FileMode.OpenOrCreate);
                StreamWriter sw = new StreamWriter(fs);
                string str = "using UnityEngine;\n" + "using System.Collections;\n" + "using System.Collections.Generic;\n";
                sw.WriteLine(str);
                sw.Close();
            }

            string text1 = "namespace projectQ\n{\n    public abstract class BaseXmlBuild\n    {\n";
            string text2 = "        public Dictionary<string , string> XmlBuildDic = new Dictionary<string, string>();\n\n";
            string text3 = "        public static Dictionary<string, System.Type> XmlTypeMaper = new Dictionary<string, System.Type>();\n\n";
            string text4 = "        public static void Init()\n";
            string text5 = "        {\n";
            string textAll = text1 + text2 + text3 + text4 + text5;
            WriteBuildText(BuildPath, textAll);

            foreach (KeyValuePair<string, List<string>> kv in Plug_SQLiteCreat.Tabel_FieldDic)
            {
                string strDoc = "            XmlTypeMaper.Add(" + "\"{0}\"" + ", typeof({1}));\n";
                string str = string.Format(strDoc, kv.Key, kv.Key);

                WriteBuildText(BuildPath, str);
            }

            WriteBuildText(BuildPath, "        }\n");
            WriteBuildText(BuildPath, "    }\n\n");

            XmlBuild_Add();
        }

        /// <summary>
        /// 添加
        /// </summary>
        public static void XmlBuild_Add()
        {
            foreach (KeyValuePair<string, List<string>> kv in Plug_SQLiteCreat.Tabel_FieldDic)
            {
                string str1 = string.Format("    public class {0} : BaseXmlBuild", kv.Key);
                string str2 = "\n    {\n";

                string strText1 = str1 + str2;
                WriteBuildText(BuildPath, strText1);

                foreach (string eStr in kv.Value)
                {
                    string str3 = string.Format("        private string {0};", "_" + eStr);
                    string str4 = string.Format("\n        public string {0}", eStr);
                    string str5 = "\n        {";

                    string str6 = "\n            get {";
                    string str7 = string.Format(" return XmlBuildDic[\"{0}\"]; ", eStr);
                    string str8 = "}";

                    string str9 = "\n            set {";
                    string str10 = string.Format(" {0} = value; ", "_" + eStr);
                    string str11 = "}";

                    string str12 = "\n        }\n\n";

                    string strText2 = str3 + str4 + str5 + str6 + str7 + str8 + str9 + str10 + str11 + str12;
                    WriteBuildText(BuildPath, strText2);
                }

                string str13 = "    }\n\n";
                WriteBuildText(BuildPath, str13);
            }

            WriteBuildText(BuildPath, "}");

            PlugMenuItemMain.PlugLog_Show(" #[Plug_XmlBuild]# - 脚本创建完成 ");

            AssetDatabase.Refresh();
        }

        /// <summary>
        /// fPath - 写入文件的路径
        /// byte[] bytes = encoder.GetBytes(logStr);//将待写的入数据从字符串转换为字节数组
        /// fs.Position = fs.Length; //设定书写的开始位置为文件的末尾
        /// fs.Write(bytes, 0, bytes.Length); //将待写入内容追加到文件末尾
        /// </summary>
        private static void WriteBuildText(string fPath, string buildStr)
        {
            FileStream fs = null;
            Encoding encoder = Encoding.UTF8;
            byte[] bytes = encoder.GetBytes(buildStr);
            try
            {
                fs = File.OpenWrite(fPath);
                if (fs != null)
                {
                    fs.Position = fs.Length;
                    fs.Write(bytes, 0, bytes.Length);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Open File Error = {0}", ex.ToString());
            }
            finally
            {
                if (fs != null)
                {
                    fs.Close();
                }
            }
            Console.ReadLine();
        }
    }
}