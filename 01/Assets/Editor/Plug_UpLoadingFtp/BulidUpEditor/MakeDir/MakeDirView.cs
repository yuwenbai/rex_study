/**
* @Author GarFey
* MakeDirView
*/
using UnityEngine;

namespace BuildUpdateEditor
{
    public class MakeDirView
    {
        private static MakeDirView Instan;
        public static MakeDirView GetInstance
        {
            get { return Instan = Instan == null ? new MakeDirView() : Instan; }
        }
        /// <summary>
        /// 输出日志
        /// </summary>
        public string dirLog = "";
        Vector2 scrollPosition;
        private float SidebarWidth;

        public void Init()
        {
            SidebarWidth = CheckUpdateControl.GetInstance.SidebarWidth;
        }
        /// <summary>
        /// 输出日志
        /// </summary>
        /// <param name="logs"></param>
        public void OnLog(string logs)
        {
            dirLog += logs+"\n\n";
        }

        public void OnGUI()
        {
            using (new HorizontalBlock())
            {
                leftUI();
                rightUI();
            }
        }

        void leftUI()
        {
            using (new VerticalBlock(GUI.skin.box,GUILayout.ExpandHeight(true)))
            {
                using (new VerticalBlock(GUI.skin.box, GUILayout.MaxWidth(SidebarWidth), GUILayout.ExpandHeight(true)))
                {
                    using (new HorizontalBlock())
                    {
                        if (GUILayout.Button("导入单个Excel", GUILayout.Width(150), GUILayout.Height(35)))
                        {
                            PlugMenuItemMain.ExcelReadOnce();
                        }
                        new wordWrapped(GUILayout.Width(80));
                        if (GUILayout.Button("导入全部Excel", GUILayout.Width(150), GUILayout.Height(35)))
                        {
                            PlugMenuItemMain.ExcelReadAll();
                        }
                    }
                }
                using (new VerticalBlock(GUI.skin.box, GUILayout.MaxWidth(SidebarWidth),GUILayout.ExpandHeight(true)))
                {
                    if (GUILayout.Button("生成XML", GUILayout.Width(150), GUILayout.Height(35)))
                    {
                        PlugMenuItemMain.SqLiteXmlCreat();
                    }
                }
                using (new VerticalBlock(GUI.skin.box, GUILayout.MaxWidth(SidebarWidth), GUILayout.ExpandHeight(true)))
                {
                    using (new HorizontalBlock())
                    {
                        if (GUILayout.Button("XML压缩", GUILayout.Width(150), GUILayout.Height(35)))
                        {
                            PlugMenuItemMain.EncryptZipCreat();
                        }
                        new wordWrapped(GUILayout.Width(80));
                        if (GUILayout.Button("XML加密", GUILayout.Width(150), GUILayout.Height(35)))
                        {
                            PlugMenuItemMain.EncryptDESCreat();
                        }
                    }
                }
            }
        }

        void rightUI()
        {
            using (new VerticalBlock(GUI.skin.box))
            {
                using (new HorizontalBlock())
                {
                    new wordWrapped();
                    if (GUILayout.Button("清空log", GUILayout.Width(100), GUILayout.Height(30), GUILayout.ExpandWidth(false)))
                    {
                        dirLog = "";
                    }
                }
                using (new ScrollviewBlock(ref scrollPosition))
                {
                    GUIStyle gst = new GUIStyle();
                    gst.normal.textColor = Color.black;
                    gst.fontSize = 14;
                    gst.wordWrap = true;
                    GUILayout.Label(dirLog, gst);
                }
            }
        }
    }
}
