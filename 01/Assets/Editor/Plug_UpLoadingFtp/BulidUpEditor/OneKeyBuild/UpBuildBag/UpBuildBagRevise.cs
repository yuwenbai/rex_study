/*********************************
 * @Author GarFey
 * upBuildBag 编辑、修改类
 *********************************/
using UnityEditor;
using UnityEngine;

namespace BuildUpdateEditor
{
    public class UpBuildBagRevise : EditorWindow
    {
        private static UpBuildBagRevise Instace;
        public static UpBuildBagRevise GetInstance
        {
            get { return Instace == null ? Instace = new UpBuildBagRevise() : Instace; }
        }
        static UpBuildBagRevise window;

        private string localPath = "";
        private string ftpPath = "";
        private string infoName = "";
        private string resiveInfoName = "";
        private string tmpInfoName = "";

        public void ShowRevise(string _name, string _localpath, string _ftpPath)
        {
            infoName = _name;
            localPath = _localpath;
            ftpPath = _ftpPath;
            tmpInfoName = _ftpPath.Replace(UpBuildBagControl.GetInstacne.FtpPath,"");
            resiveInfoName = tmpInfoName;
            window = (UpBuildBagRevise)EditorWindow.GetWindow(typeof(UpBuildBagRevise));
            window.minSize = new Vector2(550, 300);
            window.maxSize = window.minSize;
            window.titleContent = new GUIContent("UpBuildBagRevise");
            window.Show();
        }

        private void OnGUI()
        {
            using (new VerticalBlock(GUILayout.ExpandHeight(true)))
            {
                GUIStyle gst = new GUIStyle();
                gst.normal.textColor = Color.black;
                gst.fontStyle = FontStyle.Bold;
                gst.fontSize = 14;
                gst.wordWrap = true;
                GUILayout.Label("文件名:", EditorStyles.boldLabel);
                GUILayout.Label(infoName, gst);
                GUILayout.Label("文件本地路径:", EditorStyles.boldLabel);
                GUILayout.Label(localPath, gst);
                GUILayout.Label("文件ftp路径:", EditorStyles.boldLabel);
                GUILayout.Label(ftpPath, gst);
                GUILayout.Label("修改上传文件名:  <注：文件名加后缀,要个本地上传的后缀一致>", EditorStyles.boldLabel);
                resiveInfoName = GUILayout.TextField(resiveInfoName, GUILayout.Width(200));
            }

            if (GUILayout.Button("确定", GUILayout.Height(20)))
            {
                if(resiveInfoName!=""&&resiveInfoName!= tmpInfoName)
                {
                    UpBuildBagControl.GetInstacne.ReviseUpBuildDic(infoName, resiveInfoName);
                }
                this.Close();
            }
        }


        public void IsClose()
        {
            if (window != null)
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