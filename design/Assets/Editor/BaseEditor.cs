using UnityEngine;
using System.Collections.Generic;
using UnityEditor;
using System.Text;
using System.IO;
using System;



namespace Daemo
{

    public class BaseEditor : Editor
    {

        /// <summary>
        /// 切割路径
        /// </summary>
        protected static string SplitPath(string path, string s)
        {
            string[] ss = path.Split('/');
            string result = "";
            bool flat = false;
            for (int i = 0; i < ss.Length; i++)
            {
                if (flat)
                {
                    result = result + "/" + ss[i];
                }
                if (!flat && ss[i] == s)
                {
                    flat = true;
                }
            }
            return result;
        }

        #region 遍历文件夹

        //遍历文件夹
        public static void HandlelDirections(string path, Action<string> action)
        {
            HandleLocalFiles(path, action);
            string[] childrenPaths = Directory.GetDirectories(path);
            foreach (string childPath in childrenPaths)
            {
                HandlelDirections(childPath, action);
            }
        }
        public static void HandleLocalFiles(string path, Action<string> action)
        {
            string[] files = Directory.GetFiles(path);
            for (int i = 0; i < files.Length; i++)
            {
                string file = files[i];
                file = file.Replace(@"\",@"/");
                action(file);
            }
        }

        #endregion

        #region 遍历子节点

        public static void CycleChild(Transform t, Action<Transform> action)
        {
            action(t);
            for (int i = 0; i < t.childCount; i++)
            {

                if (t.childCount > 0)
                    CycleChild(t.GetChild(i), action);
            }
        }

        #endregion

    }

}


