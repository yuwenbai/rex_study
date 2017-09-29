
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace projectQ
{
    public class UIModificationAccount : UIViewBase
    {
        private UIModificationAccountModel Model
        {
            get { return _model as UIModificationAccountModel; }
        }
        #region UI
        public UIInput InputAccount;
        public GameObject ButtonOK;
        public GameObject ButtonClose;
        #endregion         

      
       
        #region Event
        //按钮点击
        private void OnButtonOKClick(GameObject go)
        {
            string value = InputAccount.value;
            value = value.Trim();
            
            if (value.Length == 0) {
                this.LoadTip("请输入要更改的ID");
               
            }if (value.Length > 20 || value.Length < 4)
            {
                this.LoadTip("请输入的字符长度不对");
            }
            else if (!IsNumAndEnCh(value[0]))//判断的首个字符是否是字母或者数字
            {
                this.LoadTip("请输入符合规则的字符");
            }
            else 
            {

                LoadPop(WindowUIType.SystemPopupWindow, "提示",
                "是否修改账号", new string[] { "取消", "确定" }, ModificationAccount);
            }

        }
        public  bool IsNumAndEnCh(char input)
        {
            bool bNumAndEn = (input >= 'a' && input <= 'z')
                 || (input >= '0' && input <= '9');
            return bNumAndEn;
        }
        //发送修改帐号
        private void ModificationAccount(int btnIndex)
        {
            if(btnIndex==1)
            {
                string acc = InputAccount.value;
                ModelNetWorker.Instance.ModifyAccountReq(acc);
            }
        }
        private void OnButtonClose(GameObject go)
        {
            this.Close();
            EventDispatcher.FireSysEvent(GEnum.NamedEvent.SysData_PlayerData_Update, MemoryData.UserID);
            
        }
        #endregion

        #region override
        public override void Init()
        {
            UIEventListener.Get(ButtonOK).onClick = OnButtonOKClick;
            UIEventListener.Get(ButtonClose).onClick = OnButtonClose;
        }

        public override void OnHide()
        {

        }

        public override void OnShow()
        {
            this.InputAccount.value = "";
        }
        #endregion
    }
}
