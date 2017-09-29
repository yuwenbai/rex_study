/****************************************************
*
*  下载一整个Apk包
*  作者: lyb
*  日期：2017年7月25日
*
*****************************************************/

using System;
using System.IO;
using System.Net;
using UnityEngine;
using System.ComponentModel;

namespace projectQ
{
    public class BundleDownApkUI : MonoBehaviour
    {
        public UILabel UrlLab;
        public UISlider DownSlider;
        public UILabel DownSliderValue;
        public UILabel DownInfoLab;

        private int sliderValue;
        private long bytesReceived;
        private long totalBytesToReceive;
        private bool isDownFinish = false;
        private bool isDownErr = false;

        void Update()
        {
            //进度条进度
            this.DownSlider.value = (float)sliderValue / 100.0f;
            //完成百分比
            this.DownSliderValue.text = sliderValue.ToString() + "%";
            //完成进度
            string receivedStr = CommonTools.FormatFileSize(bytesReceived);
            string receivedTotalStr = CommonTools.FormatFileSize(totalBytesToReceive);
            this.DownInfoLab.text = string.Format("正在下载{0}/{1}", receivedStr, receivedTotalStr);

            if (isDownFinish)
            {
                isDownFinish = false;

                WindowUIManager.Instance.CreateOrAddWindow(WindowUIType.SystemPopupWindow,
                    "提示", ResourceConfig.Bundle_Text503, new string[] { "确定" },
                    delegate (int index)
                    {
                        ResourceLoadMain.AppDownInstall();

                        gameObject.SetActive(false);
                    });
            }

            if (isDownErr)
            {
                WindowUIManager.Instance.CreateOrAddWindow(WindowUIType.SystemPopupWindow,
                    "提示", ResourceConfig.Bundle_Text504, new string[] { "确定" },
                    delegate (int index)
                    {
                        Application.Quit();
                    });

                isDownErr = false;
            }
        }

        #region 下载完毕弹框提示-------------------

        #endregion---------------------------------

        #region 下载apk接口------------------------

        /// <summary>
        /// 开始下载apk包
        /// </summary>
        public void DownApk_Begin(string downApkUrl)
        {
            if (string.IsNullOrEmpty(downApkUrl))
            {
                WindowUIManager.Instance.CreateOrAddWindow(WindowUIType.SystemPopupWindow,
                    "提示", ResourceConfig.Bundle_Text505, new string[] { "确定" },
                    delegate (int index) { });

                return;
            }

            string[] values = downApkUrl.Split(new char[] { '/' });

            //UrlLab.text = "../../" + values[values.Length - 1];

            WebClient webClient = new WebClient();

            //是否存在正在进行中的Web请求
            if (webClient.IsBusy)
            {
                webClient.CancelAsync();
            }

            webClient.DownloadProgressChanged += new DownloadProgressChangedEventHandler(webClient_DownloadProgressChanged);

            webClient.DownloadFileCompleted += new AsyncCompletedEventHandler(webClient_DownloadFileCompleted);

            GameConfig.Instance.ApkLocal_Name = GameConfig.Instance.Version_Server + ".apk";

            string apkPath = GameConfig.Instance.ApkLocal_Path + GameConfig.Instance.ApkLocal_Name;

            webClient.DownloadFileAsync(new Uri(downApkUrl), apkPath);
        }

        private void webClient_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            // 进度条进度
            sliderValue = e.ProgressPercentage;
            // 完成进度
            bytesReceived = e.BytesReceived;
            // 总进度
            totalBytesToReceive = e.TotalBytesToReceive;
        }

        private void webClient_DownloadFileCompleted(object sender, AsyncCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                //下载失败
                isDownErr = true;
            }

            if (e.Cancelled)
            {
                //用户下载被取消
            }
            else
            {
                string apkName = GameConfig.Instance.ApkLocal_Path + GameConfig.Instance.Version_Server + ".apk";

                if (File.Exists(apkName))
                {
                    string apkMd5 = Tools_Md5.MD5_File(apkName).ToLower();
                    string remoteMd5 = GameConfig.Instance.ApkMd5.ToLower();

                    if (apkMd5.Equals(remoteMd5))
                    {
                        //apk - Md5值相同提示安装操作
                        //用户下载完毕
                        isDownFinish = true;
                    }
                    else
                    {
                        //apk - Md5值不相同下载
                        //下载失败
                        isDownErr = true;
                    }
                }
            }
        }

        #endregion---------------------------------
    }
}