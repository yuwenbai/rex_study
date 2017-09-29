/**
 * @Author YQC
 *
 *
 */

using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

namespace projectQ
{
    public class UIBindPhoneNo : MonoBehaviour {
        public UIInput InputPhoneNo;
        public UIInput InputCode;
        public GameObject ButtonGetCode;
        public GameObject buttonGetUpdateTime;
        public GameObject ButtonSubmit;
        public GameObject ButtonClose;
        public UILabel updateTimeLabel;
        private string phoneNumber;
        private bool updateTime;
        string phoneNo ="";
        string code ="";
        public void RefreshUI()
        {
            this.gameObject.SetActive(true);
            InputPhoneNo.value = "";
            InputCode.value = "";
        }

        //绑定回复
        public void BindRsp(bool isOk)
        {
            if (isOk)
            {
                MemoryData.PlayerData.MyPlayerModel.PlayerDataBase.PhoneNo = phoneNumber;
                WindowUIManager.Instance.CreateOrAddWindow(WindowUIType.SystemPopupWindow, "提示",
                    "恭喜您,手机绑定成功", new string[] { "确定" },
                    (index) =>
                    {
                        this.OnButtonCloseClick(null);
                    });
            }
            else
            {
                MemoryData.PlayerData.MyPlayerModel.PlayerDataBase.PhoneNo = "";
                WindowUIManager.Instance.CreateOrAddWindow(WindowUIType.SystemPopupWindow, "提示",
                    "验证码错误!", new string[] { "确定" },
                    (index) =>
                    {
                        InputCode.value = "";
                    });
            }
        }
        private void Update()
        {
            phoneNo = InputPhoneNo.value.Trim();
            code = InputCode.value.Trim();
            if (phoneNo.Length > 11)
            {
                WindowUIManager.Instance.CreateTip("手机号码上限11位");
                InputPhoneNo.value = phoneNo.Substring(0, 11);
            }
            if (code.Length > 6)
            {
                WindowUIManager.Instance.CreateTip("验证码上限6位");
                InputCode.value = code.Substring(0, 6);
            }
            if (updateTime)
            {
                //ButtonGetCode.GetComponent<UISprite>().spriteName = "userinfo_bindPhone_01";
                //ButtonGetCode.GetComponent<BoxCollider>().enabled = false;
                
                float time = waitTime - (Time.realtimeSinceStartup - lastClickTime);
             
                if (time < 0)
                {
                    time = 0;
                    updateTime = false;
                }
                updateTimeLabel.text = Mathf.RoundToInt(time).ToString();
            }
            else
            {
                ButtonGetCode.SetActive(true);
                buttonGetUpdateTime.SetActive(false);
                lastClickTime = Time.realtimeSinceStartup;
            }
        }
        private float lastClickTime;
        private float waitTime = 30;
        private bool CheckPhoneNo(string phoneNo)
        {
            if (phoneNo.Length != 11)
            {
                updateTime = false;
                ButtonGetCode.SetActive(true);
                buttonGetUpdateTime.SetActive(false);
                WindowUIManager.Instance.CreateOrAddWindow(WindowUIType.SystemPopupWindow,
                    "提示", "请正确填写11位手机号码", new string[] { "确定" },
                    (index) =>
                    {
                        
                    });
                return false;
            }
            else
            {
                ButtonGetCode.SetActive(false);
                buttonGetUpdateTime.SetActive(true);
                updateTime = true;
            }
            return true;
        }
        private bool CheckCode(string code)
        {
            if (code.Length != 4)
            {
                WindowUIManager.Instance.CreateOrAddWindow(WindowUIType.SystemPopupWindow,
                    "提示", "请正确填写验证码", new string[] { "确定" },
                    (index) =>
                    {
                    });
                return false;
            }
            return true;
        }
        #region Event
        private void OnButtonGetCodeClick(GameObject go)
        {
            phoneNumber = InputPhoneNo.value.Trim();

            if (!this.CheckPhoneNo(phoneNumber))
                return;

           
            ModelNetWorker.Instance.GetVerifyCodeReq(phoneNumber);
        }
        private void OnButtonSubmitClick(GameObject go)
        {
            phoneNo = InputPhoneNo.value.Trim();
            code = InputCode.value.Trim();

            if (!this.CheckPhoneNo(phoneNo) || !this.CheckCode(code))
            {
                return;
            }

            ModelNetWorker.Instance.BindPhoneReq(phoneNo, code);
        }
        private void OnButtonCloseClick(GameObject go)
        {
            this.gameObject.SetActive(false);
        }
        #endregion

        private void Awake()
        {
            UIEventListener.Get(ButtonGetCode).onClick = OnButtonGetCodeClick;
            UIEventListener.Get(ButtonSubmit).onClick = OnButtonSubmitClick;
            UIEventListener.Get(ButtonClose).onClick = OnButtonCloseClick;
        }
    }
}