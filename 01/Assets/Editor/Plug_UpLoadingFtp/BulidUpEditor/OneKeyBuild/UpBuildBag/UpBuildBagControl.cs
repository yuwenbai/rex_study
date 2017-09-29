/*
* @Author GarFey
* 打包上传Info
 * */
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace BuildUpdateEditor
{
    public class UpBuildBagControl
    {
        private static UpBuildBagControl _instance;
        public static UpBuildBagControl GetInstacne
        {
            get { return _instance = _instance == null ? new UpBuildBagControl() : _instance; }
        }

        private Dictionary<string, UpBuildInfo> upBuildInfoDic = new Dictionary<string, UpBuildInfo>();
        /// <summary>
        /// 上传版本列表
        /// </summary>
        public Dictionary<string ,UpBuildInfo> GetupBuildDic
        {
            get { return upBuildInfoDic; }
        }
        /// <summary>
        /// buildInfoDic 待删除队列
        /// </summary>
        public Queue<string> removeBuildInfoDicQue = new Queue<string>();


        /// <summary>
        /// ftp路径
        /// </summary>
        private string ftpBuildPath = "ftp://192.168.221.48/";
        public string FtpPath
        {
            get { return ftpBuildPath; }
        }

        /// <summary>
        /// 添加到待删除队列
        /// </summary>
        /// <param name="fileName"></param>
        public void AddRemoveBuildDicQue(string fileName)
        {
            if(!removeBuildInfoDicQue.Contains(fileName))
            {
                removeBuildInfoDicQue.Enqueue(fileName);
            }
        }

        /// <summary>
        /// 添加数据到 上传列表中
        /// </summary>
        /// <param name="bagName"></param>
        /// <param name="info"></param>
        public void AddUpBuildDic(string bagName,UpBuildInfo  info)
        {
            if(upBuildInfoDic!=null&&upBuildInfoDic.ContainsKey(bagName))
            {
                upBuildInfoDic.Remove(bagName);
            }
            upBuildInfoDic.Add(bagName, info);
        }
        /// <summary>
        /// 删除上传列表中的 数据
        /// </summary>
        /// <param name="bagName"></param>
        public void RemoveUpBuildDic(string bagName)
        {
            if (upBuildInfoDic != null && upBuildInfoDic.ContainsKey(bagName))
            {
                upBuildInfoDic.Remove(bagName);
            }
        }
        /// <summary>
        /// 修改UpBuildDic数据 
        /// </summary>
        /// <param name="bagoldName"></param>
        /// <param name="bagnewName"></param>
        public void ReviseUpBuildDic(string bagoldName,string bagnewName)
        {
            if(upBuildInfoDic!=null&& upBuildInfoDic.ContainsKey(bagoldName))
            {
                UpBuildInfo tmpInfo = upBuildInfoDic[bagoldName];
                tmpInfo.FtpPath = ftpBuildPath+bagnewName;
            }
        }
        /// <summary>
        /// 清空上传列表
        /// </summary>
        public void ClearUpBuildDic()
        {
            if (upBuildInfoDic != null)
            {
                upBuildInfoDic.Clear();
            }
        }

        /// <summary>
        /// 弹框选择
        /// </summary>
        public void selectApk(string target)
        {
            string path = EditorUtility.OpenFilePanel("Select BuildBag", Application.dataPath, target);

            if (path.Length != 0)
            {
                string bagName = getFileName(path);
                UpBuildInfo info = new UpBuildInfo();
                info.BagName = bagName;
                info.LocalPath = path;
                info.FtpPath = ftpBuildPath+bagName;
                AddUpBuildDic(bagName, info);
            }
        }
        /// <summary>
        /// 根据资源路径截取资源名称  例 f/ass/d/c.txt  最后得到 c.txt
        /// </summary>
        /// <param name="path"></param>
        private string getFileName(string path)
        {
            string fileName = "";
            if (path != "" && path.Contains("/"))
            {
                int idex = path.LastIndexOf('/');
                fileName = path.Substring(idex + 1);
            }
            return fileName;
        }

        /// <summary>
        /// 上传版本
        /// </summary>
        public void UpLoadBuildBag()
        {
            foreach (var tmp in upBuildInfoDic)
            {
                CheckUpdateControl.GetInstance.upLoadFiles(tmp.Value.LocalPath, tmp.Value.FtpPath, upLoadFilesCallBack, false, true);
            }
            if (removeBuildInfoDicQue.Count == upBuildInfoDic.Count)
            {
                EditorUtility.DisplayDialog("提示", "所有文件上传成功。", "确定");
            }
            else
            {
                EditorUtility.DisplayDialog("提示", "有文件上传失败 ！", "确定");
            }
        }
        void upLoadFilesCallBack(string fileName, bool isFinish)
        {
            if (isFinish)
            {
                AddRemoveBuildDicQue(fileName);
                Debug.Log("文件【" + fileName + "】上传成功！<br/>");
            }
            else
            {
                Debug.Log("文件【" + fileName + "】上传失败！<br/>");
            }
        }
    }
}