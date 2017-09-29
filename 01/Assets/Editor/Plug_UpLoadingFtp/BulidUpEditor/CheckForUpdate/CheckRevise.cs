/**
 * @Author GarFey
 * Check 编辑、修改类
 */
using UnityEditor;
using UnityEngine;

namespace BuildUpdateEditor
{
    public class CheckRevise : EditorWindow
    {
        private static CheckRevise Instace;
        public static CheckRevise GetInstance
        {
            get { return Instace == null ? Instace = new CheckRevise() : Instace; }
        }
        static CheckRevise window;

        private  string mfileName = "";
        private  string mfilePath = "";
        private  string mfileMd5 = "";
        private  string mfileVersion = "";

        public void ShowRevise(string _name, string _path, string _md5, string _ver)
        {
            mfileName = _name;
            mfilePath = _path;
            mfileMd5 = _md5;
            mfileVersion = _ver;
            window = (CheckRevise)EditorWindow.GetWindow(typeof(CheckRevise));
            window.minSize = new Vector2(550, 300);
            window.maxSize = window.minSize;
            window.titleContent = new GUIContent("CheckRevise");
            window.Show();
        }

        private void OnGUI()
        {
            using (new VerticalBlock(GUILayout.ExpandHeight(true)))
            {
                GUILayout.Label("文件名:", EditorStyles.boldLabel);
                GUILayout.Label(mfileName, EditorStyles.boldLabel);
                GUILayout.Label("文件本地路径:", EditorStyles.boldLabel);
                GUIStyle gst = new GUIStyle();
                gst.normal.textColor = Color.black;
                gst.fontStyle = FontStyle.Bold;
                gst.fontSize = 14;
                gst.wordWrap = true;
                GUILayout.Label(mfilePath, gst);
                GUILayout.Label("文件MD5:", EditorStyles.boldLabel);
                GUILayout.Label(mfileMd5, EditorStyles.boldLabel);
                GUILayout.Label("文件Version:", EditorStyles.boldLabel);
                mfileVersion = GUILayout.TextField(mfileVersion, GUILayout.Width(100));
            }

            if (GUILayout.Button("确定",GUILayout.Height(20)))
            {
                CheckUpdateControl.GetInstance.CheckUpReviseVersion(mfileName,mfileVersion);
                this.Close();
            }
        }

        public void IsClose()
        {
            if(window!=null)
            {
                this.Close();
            }
        }

        void OnDestroy()
        {
            window = null;
        }
    }
}
