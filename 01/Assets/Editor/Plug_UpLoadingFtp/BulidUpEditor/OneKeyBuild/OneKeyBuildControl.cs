/**
* @Author GarFey
* 打包上传管理类
*/
using UnityEditor;
using UnityEngine;

namespace BuildUpdateEditor
{
    public class OneKeyBuildControl
    {
        private static OneKeyBuildControl _instance;
        public static OneKeyBuildControl GetInstance
        {
            get { return _instance =  _instance == null ? new OneKeyBuildControl() : _instance; }
        }

        public void BuildAndroidOnGUI()
        {
            if (GUILayout.Button("生成Android版本", GUILayout.Width(150), GUILayout.Height(30)))
            {
                BuildUpEditor.GetInstance.Close();
                AutoBuidle.BuildAndroid();
            }
        }

        public void BuildIosOnGUI()
        {
            if (GUILayout.Button("生成Ios版本", GUILayout.Width(150), GUILayout.Height(30)))
            {
                switch (Application.platform)
                {
                    case RuntimePlatform.WindowsEditor://WindowsPC端  
                        EditorUtility.DisplayDialog("提示", "当前平台不是OSXEditor！", "确定");
                        break;
                    case RuntimePlatform.OSXEditor://OS 
                        BuildUpEditor.GetInstance.Close();
                        AutoBuidle.BuildIOS();
                        break;
                }
            }
        }

        public void BuildWindowsOnGUI()
        {
            if (GUILayout.Button("生成Pc版本", GUILayout.Width(150), GUILayout.Height(30)))
            {
                BuildUpEditor.GetInstance.Close();
                AutoBuidle.BuildWin32();
            }
        }
    }
}
