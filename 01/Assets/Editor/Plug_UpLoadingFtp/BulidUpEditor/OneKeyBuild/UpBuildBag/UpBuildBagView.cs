/**
* @Author GarFey
* UpBuildBagView
*/

using System;
using UnityEditor;
using UnityEngine;

namespace BuildUpdateEditor
{
    public class UpBuildBagView
    {
        private static UpBuildBagView _instance;
        public static UpBuildBagView  GetInstance
        {
            get { return _instance = _instance == null ? new UpBuildBagView() : _instance; }
        }

        /// <summary>
        /// tfp用户名
        /// </summary>
        private string userID = "369dev";
        private string UserID
        {
            get { return userID; }
            set { userID = value; }
        }
        /// <summary>
        /// ftp密码
        /// </summary>
        private string passWord = "369dev";
        private string PassWord
        {
            get { return passWord; }
            set { passWord = value; }
        }

        /// <summary>
        /// 当前选择上传服务器类型
        /// </summary>
        int curSelectPlatForm = -1;
        int selectedPlatform = 0;
        string[] platformType = { "Android", "Ios" };
        int[] sizes = { 0, 1 };


        Vector2 scrollPosition;

        public void Init()
        {

        }

        public void OnGUI()
        {
            using (new VerticalBlock(GUI.skin.box, GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true)))
            {
                using (new HorizontalBlock())
                {
                    using (new VerticalBlock())
                    {
                        GUIStyle gst = new GUIStyle();
                        gst.normal.textColor = Color.blue;
                        gst.fontStyle = FontStyle.Bold;
                        gst.fontSize = 18;
                        gst.wordWrap = true;
                        GUILayout.Label("选择上传平台:", gst);
                        new wordWrapped(GUILayout.Height(10));
                        selectedPlatform = EditorGUILayout.IntPopup("选择上传平台: ", selectedPlatform, platformType, sizes);
                        switch (platformType[Array.IndexOf(sizes, selectedPlatform)])
                        {
                            case "Android":
                                if (curSelectPlatForm != selectedPlatform)
                                {
                                    curSelectPlatForm = selectedPlatform;
                                    PassWord = UserID = "369dev";
                                    UpBuildBagControl.GetInstacne.ClearUpBuildDic();
                                }
                                break;
                            case "Ios":
                                if (curSelectPlatForm != selectedPlatform)
                                {
                                    curSelectPlatForm = selectedPlatform;
                                    PassWord = UserID = "iosupload";
                                    UpBuildBagControl.GetInstacne.ClearUpBuildDic();
                                }
                                break;
                        }

                        GUILayout.Label("用户名:", EditorStyles.boldLabel);
                        UserID = GUILayout.TextField(UserID, GUILayout.Width(200));

                        GUILayout.Label("密码:", EditorStyles.boldLabel);
                        PassWord = GUILayout.PasswordField(PassWord, '*', GUILayout.Width(200));
                       
                        new wordWrapped(GUILayout.Height(10));
                        GUILayout.Label("本地版本目录:", gst, GUILayout.Height(25));
                        if (GUILayout.Button("选择", GUILayout.Width(150), GUILayout.Height(30)))
                        {
                            switch (platformType[Array.IndexOf(sizes, curSelectPlatForm)])
                            {
                                case "Android":
                                    UpBuildBagControl.GetInstacne.selectApk("apk");
                                    break;
                                case "Ios":
                                    UpBuildBagControl.GetInstacne.selectApk("ipa");
                                    break;
                            }
                        }

                        new wordWrapped(GUILayout.Height(10));

                        gst.normal.textColor = Color.blue;
                        gst.fontSize = 18;
                        GUILayout.Label("服务器版本根目录:", gst, GUILayout.Height(25));
                        gst.normal.textColor = Color.black;
                        gst.fontSize = 14;
                        GUILayout.Label(UpBuildBagControl.GetInstacne.FtpPath , gst);
                    }
                    using (new VerticalBlock())
                    {
                        using (new ScrollviewBlock(ref scrollPosition))
                        {
                            if(UpBuildBagControl.GetInstacne.GetupBuildDic.Count>0)
                            {
                                foreach(var builInfo in UpBuildBagControl.GetInstacne.GetupBuildDic)
                                {
                                    using (new HorizontalBlock(EditorStyles.helpBox))
                                    {
                                        builInfo.Value.refUpBuildInfo();
                                    }
                                }
                            }
                            if (UpBuildBagControl.GetInstacne.removeBuildInfoDicQue.Count > 0)
                            {
                                string deFileName = "";
                                int queCount = UpBuildBagControl.GetInstacne.removeBuildInfoDicQue.Count;
                                for (int i = 0; i < queCount; i++)
                                {
                                    deFileName = UpBuildBagControl.GetInstacne.removeBuildInfoDicQue.Dequeue();
                                    if(UpBuildBagControl.GetInstacne.GetupBuildDic.ContainsKey(deFileName))
                                    {
                                        UpBuildBagControl.GetInstacne.RemoveUpBuildDic(deFileName);
                                    }
                                }
                            }
                        }
                        using (new HorizontalBlock())
                        {
                            new wordWrapped();
                            if (GUILayout.Button("上传", GUILayout.Width(150), GUILayout.Height(30)))
                            {
                                if(UpBuildBagControl.GetInstacne.GetupBuildDic.Count<=0)
                                {
                                    EditorUtility.DisplayDialog("提示", "上传列表为空，请选择左侧【本地版本目录】", "确定");
                                }
                                else
                                {
                                    CheckUpdateControl.GetInstance.UserID = userID;
                                    CheckUpdateControl.GetInstance.PassWord = passWord;
                                    UpBuildBagControl.GetInstacne.UpLoadBuildBag();
                                }
                            }
                        }
                    }
                }
            }
        }
        public void OnDestory()
        {
            _instance = null;
        }
    }
}
