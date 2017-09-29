/**
* @Author GarFey
* CheckInfo 信息条类
*/
using UnityEngine;

namespace BuildUpdateEditor
{
    public class CheckListInfo
    {
        private string mFileName = "";
        private string mLocalFilePath = "";
        private string mFileMd5 = "";
        private string mVersionTitle = "";
        private string mFileVersion ="";
        /// <summary>
        /// 文件Version
        /// </summary>
        public string _FileVersion
        {
            get { return mFileVersion; }    set { mFileVersion = value; }
        }
        /// <summary>
        /// 文件本地路径
        /// </summary>
        public string _LocalFilePath
        {
            get{return mLocalFilePath;}    set{mLocalFilePath = value;}
        }
        /// <summary>
        /// 文件名称
        /// </summary>
        public string _FileName
        {
            get{return mFileName;}       set{mFileName = value;}
        }
        /// <summary>
        /// 文件MD5
        /// </summary>
        public string _FileMd5
        {
            get { return mFileMd5; }      set { mFileMd5 = value; }
        }
        /// <summary>
        /// 显示version title
        /// </summary>
        public string _VersionTitle
        {
            get{return mVersionTitle; }     set{mVersionTitle = value;}
        }

        /// <summary>
        /// 文件操作状态
        /// </summary>
        public enum FileState
        {
            /// <summary>
            /// 修改
            /// </summary>
            Revise,
            /// <summary>
            /// 不同
            /// </summary>
            Difc,
            /// <summary>
            /// 添加
            /// </summary>
            Add,
            /// <summary>
            /// 相同
            /// </summary>
            Null
        }
        public FileState mfileState = FileState.Null;
        GUIStyle fileColorStyle = new GUIStyle();

        public void InitListInfo()
        {
            using (new VerticalBlock())
            {
                using (new HorizontalBlock())
                {
                    setColorStyle();
                    GUILayout.Label(mFileName, fileColorStyle);
                }

                new wordWrapped(GUILayout.Height(2));
                
                using (new HorizontalBlock())
                {
                    GUILayout.Label(mVersionTitle);

                    if (GUILayout.Button("编辑", GUILayout.Width(50), GUILayout.ExpandWidth(false)))
                    {
                        CheckRevise.GetInstance.ShowRevise(mFileName, mLocalFilePath, mFileMd5, mFileVersion);
                    }

                    if (GUILayout.Button("删除", GUILayout.Width(50), GUILayout.ExpandWidth(false)))
                    {
                        CheckUpdateControl.GetInstance.RemoveFileInfo(mFileName);
                    }
                } 
            }
        }

        private void setColorStyle()
        {
            switch(mfileState)
            {
                case FileState.Null:
                    fileColorStyle.normal.textColor = Color.black;
                    break;
                case FileState.Add:
                    fileColorStyle.normal.textColor = Color.blue;
                    break;
                case FileState.Difc:
                    fileColorStyle.normal.textColor = Color.red;
                    break;
                case FileState.Revise:
                    fileColorStyle.normal.textColor = Color.magenta;
                    break;
                default:
                    fileColorStyle.normal.textColor = Color.black;
                    break;
            }
            
        }
    }
}