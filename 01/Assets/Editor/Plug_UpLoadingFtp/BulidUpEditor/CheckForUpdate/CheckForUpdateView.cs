/**
 * @Author GarFey
 * CheckForUpdateView
 */
using UnityEditor;
using UnityEngine;
using PlugProject;

namespace BuildUpdateEditor
{
    public class CheckForUpdateView
    {
        private static CheckForUpdateView Instan;
        public static CheckForUpdateView GetInstance
        {
            get { return Instan = Instan == null ? new CheckForUpdateView() : Instan; }
        } 

        private static string localPath= "";
        private static string ftpPath= "";
        private static string serverVersion="";

        private string userID = "369dev";
        private string UserID
        {
            get { return userID; }
            set { userID = value; }
        }
        private string passWord = "369dev";
        private string PassWord
        {
            get { return passWord; }
            set { passWord =value; }
        }
        private float SidebarWidth;
        /// <summary>
        /// 是否强制更新
        /// </summary>
        bool isOneKeyUpdate = false;
        /// <summary>
        /// 是否只上传version
        /// </summary>
        bool isOnlyUpVersion = false;
        Vector2 scrollPosition;

        public void Init()
        {
            CheckUpDate.GetInstance.ClearFileInfoDic();
            SidebarWidth = CheckUpdateControl.GetInstance.SidebarWidth;
            serverVersion = UpLoadingData.Version_Server;
            switch(CheckUpdateControl.mUpDateType)
            {
                case UpdateType.Xml:
                    localPath = UpLoadingData.UpXmlPath;
                    ftpPath = UpLoadingData.ServerSiteUrl;
                    break;
                case UpdateType.Texture:
                    localPath = UpLoadingData.UpTexturePath;
                    ftpPath = UpLoadingData.ServerSiteUrl;
                    break;
                case UpdateType.AssetBundle:
                    break;
            }
        }

        public void OnGUI()
        {
            CheckUpdateControl.GetInstance.UserID = userID;
            CheckUpdateControl.GetInstance.PassWord = passWord;
            using (new HorizontalBlock())
            {
                leftUI();
                rightUI();
            }
        }

        void leftUI()
        {
            using (new VerticalBlock(GUI.skin.box, GUILayout.MaxWidth(SidebarWidth), GUILayout.ExpandHeight(true)))
            {
                using (new VerticalBlock(GUI.skin.box, GUILayout.MaxWidth(SidebarWidth), GUILayout.ExpandHeight(true)))
                {
                    GUILayout.Label("用户名:", EditorStyles.boldLabel);
                    UserID = GUILayout.TextField(UserID, GUILayout.Width(200));

                    GUILayout.Label("密码:", EditorStyles.boldLabel);
                    PassWord = GUILayout.PasswordField(PassWord, '*', GUILayout.Width(200));
                }

                using (new VerticalBlock(GUI.skin.box, GUILayout.MaxWidth(SidebarWidth), GUILayout.ExpandHeight(true)))
                {
                    GUIStyle gst = new GUIStyle();
                    gst.normal.textColor = Color.blue;
                    gst.fontStyle = FontStyle.Bold;
                    gst.fontSize = 18;
                    gst.wordWrap = true;
                    GUILayout.Label("本地资源目录:",gst, GUILayout.Height(25));
                    gst.normal.textColor = Color.black;
                    gst.fontSize = 14;
                    GUILayout.Label(localPath, gst);

                    new wordWrapped(GUILayout.Height(10));

                    gst.normal.textColor = Color.blue;
                    gst.fontSize = 18;
                    GUILayout.Label("服务器资源目录:", gst, GUILayout.Height(25));
                    gst.normal.textColor = Color.black;
                    gst.fontSize = 14;
                    GUILayout.Label(ftpPath, gst);
                }

                using (new VerticalBlock(GUI.skin.box, GUILayout.MaxWidth(SidebarWidth), GUILayout.ExpandHeight(true)))
                {
                    GUIStyle gst = new GUIStyle();
                    gst.normal.textColor = Color.blue;
                    gst.fontStyle = FontStyle.Bold;
                    gst.fontSize = 18;
                    gst.wordWrap = true;
                    GUILayout.Label("服务器Version:", gst);
                    serverVersion = GUILayout.TextField(serverVersion, GUILayout.Width(200));
                    isOnlyUpVersion = GUILayout.Toggle(isOnlyUpVersion, "只上传Version");
                    if(isOnlyUpVersion)
                    {
                        if (GUILayout.Button("上传Version",GUILayout.Width(150), GUILayout.Height(30)))
                        {
                            CheckUpdateControl.GetInstance.UpVersion(serverVersion,false);
                        }
                    }
                }

                using (new VerticalBlock(GUI.skin.box, GUILayout.MaxWidth(SidebarWidth), GUILayout.ExpandHeight(true)))
                {
                    using (new HorizontalBlock())
                    {
                        if (GUILayout.Button("检查MD5生成上传列表", GUILayout.Width(150), GUILayout.Height(30)))
                        {
                            CheckUpdateControl.GetInstance.CheckUpdateMd5();
                        }
                    }  
                }
            }
        }

        void rightUI()
        {
            using (new VerticalBlock(GUI.skin.box, GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true)))
            {
                using (new ScrollviewBlock(ref scrollPosition))
                {
                    if (CheckUpDate.GetInstance.getFileInfoDic.Count > 0)
                    {
                        foreach (var sst in CheckUpDate.GetInstance.getFileInfoDic)
                        {
                            using (new HorizontalBlock(EditorStyles.helpBox))
                            {
                                sst.Value.InitListInfo();
                            }
                        }
                    }
                    if(CheckUpdateControl.GetInstance.removeDicNameQueue.Count>0)
                    {
                        string deFileName = "";
                        int queCount = CheckUpdateControl.GetInstance.removeDicNameQueue.Count;
                        for (int i = 0; i < queCount; i++)
                        {
                            deFileName = CheckUpdateControl.GetInstance.removeDicNameQueue.Dequeue();
                            if (CheckUpDate.GetInstance.getFileInfoDic.Count > 0 && CheckUpDate.GetInstance.getFileInfoDic.ContainsKey(deFileName))
                            {
                                CheckUpDate.GetInstance.DelFileInfoDic(deFileName);
                            }
                        }
                    }
                }
                new wordWrapped(GUILayout.Height(4));
                using (new HorizontalBlock())
                {
                    isOneKeyUpdate = GUILayout.Toggle(isOneKeyUpdate, "强更上传 (强更上传 所有本地文件的version都改为-1! 謹慎使用)");
                    if (GUILayout.Button("排序 ↑", GUILayout.Width(100), GUILayout.Height(30), GUILayout.ExpandWidth(false)))
                    {
                        CheckUpdateControl.GetInstance.OrderByFileInfoDic();
                    }
                    new wordWrapped(GUILayout.Width(30));
                    if (isOneKeyUpdate)
                    {
                        // 强更上传 所有本地文件的version都改为-1
                        if (GUILayout.Button("强更上传", GUILayout.Width(100),GUILayout.Height(30), GUILayout.ExpandWidth(false)))
                        {
                            if (CheckUpDate.GetInstance.getFileInfoDic.Count > 0)
                            {
                                CheckUpdateControl.GetInstance.UpdateFileInfoDic(serverVersion);
                            }
                            else
                            {
                                EditorUtility.DisplayDialog("提示", "请先生成上传列表，点击左侧【检查MD5生成上传列表】按钮！", "确定");
                            }
                        }
                    }
                    else
                    {
                        if (GUILayout.Button("上传", GUILayout.Width(100), GUILayout.Height(30), GUILayout.ExpandWidth(false)))
                        {
                            if(CheckUpDate.GetInstance.getFileInfoDic.Count>0)
                            {
                                CheckUpdateControl.GetInstance.UpdateFileInfoDic(serverVersion);
                            }
                            else
                            {
                                EditorUtility.DisplayDialog("提示", "请先生成上传列表，点击左侧【检查MD5生成上传列表】按钮！", "确定");
                            }
                        }
                    }
                }
            }
        }
    }
}
