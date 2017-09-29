/**
* @Author GarFey
* OneKeyBuildView
*/
using System.IO;
using UnityEditor;
using UnityEngine;
namespace BuildUpdateEditor
{
    public class OneKeyBuildView
    {
        private static OneKeyBuildView Instan;
        public static OneKeyBuildView GetInstance
        {
            get { return Instan = Instan == null ? new OneKeyBuildView() : Instan; }
        }
        public static bool isUnity = false;

        private float SidebarWidth;
        Vector2 scrollPosition;

        public void Init()
        {
            SidebarWidth = CheckUpdateControl.GetInstance.SidebarWidth;
        }
        /// <summary>
        /// Build完成
        /// </summary>
        /// <param name="path">完成包路径</param>
        /// <param name="name">包名称</param>
        public void BuildFinish(string path,string name)
        {
            if (isUnity)
            {
                isUnity = false;
                string localDir = "";
                localDir = path.Replace(name, "");
                if (Directory.Exists(localDir))
                {
                    System.Diagnostics.Process.Start(localDir);
                }
            }
        }

        public void OnGUI()
        {
            try
            {
                using (new HorizontalBlock())
                {
                    leftUI();
                    right();
                }
            }
            catch{}
        }

        private void leftUI()
        {
            using (new VerticalBlock(GUI.skin.box, GUILayout.MaxWidth(SidebarWidth), GUILayout.ExpandHeight(true)))
            {
                if(GUILayout.Button("生成 Android 版本", GUILayout.Height(30)))
                {
                    BuildUpPlatformState.buildPlatformCurState = TBuildState.BuildAndroid;
                }
                new wordWrapped(GUILayout.Height(10));
                if (GUILayout.Button("生成 Ios 版本", GUILayout.Height(30)))
                {
                    BuildUpPlatformState.buildPlatformCurState = TBuildState.BuildIos;
                }
                new wordWrapped(GUILayout.Height(10));
                if (GUILayout.Button("生成 Windows 版本", GUILayout.Height(30)))
                {
                    BuildUpPlatformState.buildPlatformCurState = TBuildState.BuildWindows;
                }
                new wordWrapped(GUILayout.Height(10));
                if (GUILayout.Button("上传版本", GUILayout.Height(30)))
                {
                    UpBuildBagView.GetInstance.Init();
                    BuildUpPlatformState.buildPlatformCurState = TBuildState.UpBuildBag;
                }
            }
        }

        void right()
        {
            using (new VerticalBlock(GUI.skin.box, GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true)))
            {
                switch (BuildUpPlatformState.buildPlatformCurState)
                {
                    case TBuildState.BuildAndroid:
                        OneKeyBuildControl.GetInstance.BuildAndroidOnGUI();
                        break;
                    case TBuildState.BuildIos:
                        OneKeyBuildControl.GetInstance.BuildIosOnGUI();
                        break;
                    case TBuildState.BuildWindows:
                        OneKeyBuildControl.GetInstance.BuildWindowsOnGUI();
                        break;
                    case TBuildState.UpBuildBag:
                        UpBuildBagView.GetInstance.OnGUI();
                        break;
                }
            }
        }

        public void OnDestory()
        {
            Instan = null;
        }
    }
}