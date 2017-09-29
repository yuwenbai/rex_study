/**
 * @Author YQC
 *
 *
 */

using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using System;

namespace projectQ
{
    public class UICertification : MonoBehaviour {
        public UIInput InputName;
        public UIInput InputIDNumber;
        public GameObject ButtonOK;
        public GameObject ButtonClose;
        private string realName;
        private string cardNumber;
        private bool isMan;
        private bool cardIsMan;
        public void IsActive()
        {
            if (UIToggle.current.value)
            {
                isMan = true;
                QLoger.LOG("toggle true 男");
            }
            else
            {
                isMan = false;
                QLoger.LOG("toggle false 女");
            }
        }
        public void RefreshUI()
        {
            this.gameObject.SetActive(true);
            InputName.value = "";
            InputIDNumber.value = "";
        }

        //绑定回复    
        public void BindRsp(bool isOk)
        {
            if(isOk)
            {
                MemoryData.PlayerData.MyPlayerModel.PlayerDataBase.RealName = realName;
                MemoryData.PlayerData.MyPlayerModel.PlayerDataBase.IDCard = cardNumber;
                WindowUIManager.Instance.CreateOrAddWindow(WindowUIType.SystemPopupWindow, "",
                    "恭喜您,实名认证成功", new string[] { "确定" },
                    (index) =>
                    {
                        this.OnButtonCloseClick(null);
                    });
            }
            else
            {
                MemoryData.PlayerData.MyPlayerModel.PlayerDataBase.RealName = "";
                MemoryData.PlayerData.MyPlayerModel.PlayerDataBase.IDCard = "";
                WindowUIManager.Instance.CreateOrAddWindow(WindowUIType.SystemPopupWindow, "",
                    "实名认证失败,请检查填写内容", new string[] { "确定" },
                    (index) =>
                    {

                    });
            }
        }

        void Update()
        {
            realName = InputName.value.Trim();
            cardNumber = InputIDNumber.value.Trim();
            if (realName.Length>8)
            {
                WindowUIManager.Instance.CreateTip("名称上限8位");
                InputName.value = realName.Substring(0, 8);
            }
            if(cardNumber.Length>18)
            {
                WindowUIManager.Instance.CreateTip("身份证号上限18位");
                InputIDNumber.value = cardNumber.Substring(0, 18);
            }
        }

        #region Event
        private void OnButtonOKClick(GameObject go)
        {
            realName = InputName.value.Trim();
            cardNumber = InputIDNumber.value.Trim();
            if (realName.Length < 2)
            {
                WindowUIManager.Instance.CreateOrAddWindow( WindowUIType.SystemPopupWindow,
                    "姓名填写错误", "姓名长度不小于2位", new string[] { "确定" },
                    (index) => {
                    });

                return;
            }
            Regex Connote = new Regex("^[\\d]{18}|[\\d]{17}[a-zA-Z]$");
            if (!Connote.IsMatch(cardNumber))
            {
                WindowUIManager.Instance.CreateOrAddWindow(WindowUIType.SystemPopupWindow,
                    "身份证填写错误", "身份证号应该是18位数字，或17位数字+1位英文字母", new string[] { "确定" },
                   (index) =>
                   {
                       InputName.value = "";
                       realName = "";
                   });

                return;
            }
            if (!CheckNumber())//验证身份证信息
            {
                WindowUIManager.Instance.CreateOrAddWindow(WindowUIType.SystemPopupWindow,
                    "身份证填写错误", "身份证信息错误，请重新填写", new string[] { "确定" },
                   (index) =>
                   {
                       InputIDNumber.value = "";
                       cardNumber = "";
                   });
                return;
            }
           
            ModelNetWorker.Instance.CertificationReq(realName, cardNumber);
        }
        /// <summary>
        /// 身份证规则检查
        /// </summary>
        /// <returns></returns>
        bool CheckNumber()
        {
            string regionID = cardNumber.Substring(0,2);//区号，前两位
            string timeDate = cardNumber.Substring(6,8);//时间，中间八位
            string sex = cardNumber.Substring(16, 1);//性别，倒数第二位
            if (CheckRegion(regionID) && CheckTime(timeDate) && CheckSex(sex))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        /// <summary>
        /// 检查区号
        /// </summary>
        /// <returns></returns>
        bool CheckRegion(string regionID)
        {
            List<AreaCode> codeList = new List<AreaCode>();
            List<string> idList = new List<string>();
            List<BaseXmlBuild> buildList = MemoryData.XmlData.XmlBuildDataDic["AreaCode"];
            foreach (BaseXmlBuild build in buildList)
            {
                AreaCode info = (AreaCode)build;
                idList.Add(info.AreaID);
            }
            if (idList.Contains(regionID))
            {
                return true;
            }
            else
            {
                return false;
            }
            
        }
        /// <summary>
        /// 检查日期
        /// </summary>
        /// <returns></returns>
        bool CheckTime(string timeStr)
        {
            string yearStr = timeStr.Substring(0, 4);
            string monthStr = timeStr.Substring(4, 2);
            string dayStr = timeStr.Substring(6, 2);
            int year = int.Parse(yearStr);
            int month = int.Parse(monthStr);
            int day = int.Parse(dayStr);
            if (year < (DateTime.Now.Year - 120) || year > DateTime.Now.Year)//最小120年之前，最大当前年
            {
                return false;
            }
            else
            {
                if (month < 1 || month > 12)//最小1月，最大12月
                {
                    return false;
                }
                else
                {
                    if (day < 1 || day > DateTime.DaysInMonth(year,month))//最小1号，最大，那年那月的天数
                    {
                        return false;
                    }
                    else
                    {
                        return true;
                    }
                }
            }
           
        }
        /// <summary>
        /// 检查性别
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        bool CheckSex(string number)
        {
            int cardSex = int.Parse(number);
            if (cardSex % 2 == 0)
            {
                cardIsMan = false;
            }
            else
            {
                cardIsMan = true;
            }
            if (cardIsMan == isMan)
            {
                return true;
            }
            else
            {
                return false;
               
            }
           
        }

        private void OnButtonCloseClick(GameObject go)
        {
            this.gameObject.SetActive(false);
        }
        #endregion

        private void Awake()
        {
            UIEventListener.Get(ButtonOK).onClick = OnButtonOKClick;
            UIEventListener.Get(ButtonClose).onClick = OnButtonCloseClick;
        }
    }
}