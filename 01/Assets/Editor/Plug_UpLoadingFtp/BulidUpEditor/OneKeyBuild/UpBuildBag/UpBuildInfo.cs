/*
* @Author GarFey
* 打包上传Info
 * */
using UnityEngine;

namespace BuildUpdateEditor
{
    public class UpBuildInfo
    {
        private string bagName = "";
        private string localPath = "";
        private string ftpPath = "";
        /// <summary>
        /// 文件名称
        /// </summary>
        public string BagName
        {
            get { return bagName; }            set { bagName = value; }
        }
        /// <summary>
        /// 本地路径
        /// </summary>
        public string LocalPath
        {
            get { return localPath; }            set { localPath = value; }
        }
        /// <summary>
        /// ftp路径
        /// </summary>
        public string FtpPath
        {
            get { return ftpPath; }            set { ftpPath = value; }
        }

        public void refUpBuildInfo()
        {
            using (new VerticalBlock())
            {
                GUILayout.Label(localPath);
                GUILayout.Label(ftpPath);
                using (new HorizontalBlock())
                {
                    new wordWrapped();
                    if (GUILayout.Button("编辑", GUILayout.Width(50), GUILayout.ExpandWidth(false)))
                    {
                        UpBuildBagRevise.GetInstance.ShowRevise(BagName, localPath, ftpPath);
                    }
                    if (GUILayout.Button("删除", GUILayout.Width(50), GUILayout.ExpandWidth(false)))
                    {
                        UpBuildBagControl.GetInstacne.AddRemoveBuildDicQue(BagName);
                    }
                }
            }
        }
    }
}
