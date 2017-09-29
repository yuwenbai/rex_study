/**
* @Author GarFey
* BulidUp Editor
* 文件 配置 xml 更新 下载 读取 加密
*/
using UnityEditor;
using UnityEngine;

namespace BuildUpdateEditor
{
    public class BuildUpEditor : EditorWindow
    {
        private static BuildUpEditor window;
        public static BuildUpEditor GetInstance
        {
            get { return window; }
        }
        [MenuItem("Tools/BuildUpEditor")]
        public static void Init()
        {
            window = (BuildUpEditor)EditorWindow.GetWindow(typeof(BuildUpEditor));
            window.minSize = new Vector2(1200, 800);
            window.titleContent = new GUIContent("BuildUpEditor");
            window.Show();
        }

        public void OnGUI()
        {
            ToolsBar.GetInstance.OnGUI();
            switch (ToolsEditorState.toolsEidtorCurState)
            {
                case TEditorState.NULL:
                    break;
                case TEditorState.MakeDir:
                    MakeDirView.GetInstance.OnGUI();
                    break;
                case TEditorState.CHECKUPDATE:
                    CheckForUpdateView.GetInstance.OnGUI();
                    break;
                case TEditorState.ONEKEYBUILD:
                    OneKeyBuildView.GetInstance.OnGUI();
                    break;
                default:
                    break;
            }
        }

        void OnDestroy()
        {
            ToolsEditorState.toolsEidtorCurState = TEditorState.NULL;
            BuildUpPlatformState.buildPlatformCurState = TBuildState.NULL;
            CheckRevise.GetInstance.IsClose();
            CheckUpDate.GetInstance.OnDestroy();
            ToolsBar.GetInstance.OnDestory();
            CheckUpdateControl.GetInstance.OnDestory();
            OneKeyBuildView.GetInstance.OnDestory();
            UpBuildBagView.GetInstance.OnDestory();
        }
    }
}
