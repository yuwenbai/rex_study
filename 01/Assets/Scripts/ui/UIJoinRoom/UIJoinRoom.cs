using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace projectQ
{

    public class UIJoinRoom : UIViewBase
    {
        private UIJoinRoomModel Model {
            get {
                return base._model as UIJoinRoomModel;
            }
        } 
        #region Propert

        #region UI
        public Transform ButtonKeyRoot;
    
        public Transform PwdRoot;

        //关闭按钮
        public GameObject ButtonClose;
        #endregion

        //键盘List
        private List<GameObject> ButtonKeyList = new List<GameObject>();
        //密码
        private List<UILabel> PwdList = new List<UILabel>();
        //Data 密码
        private List<int> password = new List<int>();
        #endregion

        #region Func
        //发送密码
        public void SendPassword(string pwd)
        {
            StartCoroutine(UITools.WaitExcution(ClearPassword, 0.2f));

            ulong userID = (ulong)MemoryData.UserID;
            int roomID = Int32.Parse(pwd);
            MahjongLogicNew.Instance.SendMjJoinDesk(userID, roomID);
        }
        //发送密码的回调
        public void SendPasswordCallback(int resultCode)
        {
            if (resultCode == 0)
            {
                StartCoroutine(UITools.WaitExcution(()=> { this.Hide(); }, 0.2f));
            }
            else
            {
                //this.ClearPassword();
            }
        }

        #region 密码输入
        private void RefreshPwd()
        {
            for (int i = 0; i < this.PwdList.Count; ++i)
            {
                if (password.Count > i)
                {
                    PwdList[i].text = password[i].ToString();
                    //string spriteName = "joinDesk_icon_" + password[i];
                    //if (PwdList[i].spriteName != spriteName)
                    //    PwdList[i].spriteName = spriteName;
                    PwdList[i].enabled = true;
                }
                else
                {
                    PwdList[i].enabled = false;
                }
            }

            if (password.Count == PwdList.Count)
            {
                System.Text.StringBuilder str = new System.Text.StringBuilder();
                for (int i = 0; i < password.Count; ++i)
                {
                    str.Append(password[i]);
                }
                this.SendPassword(str.ToString());
            }
        }
        #endregion


        #region Password
        private void AddPassword(int num)
        {
            if (password.Count < 6)
            {
                password.Add(num);
                this.RefreshPwd();
            }
        }
        private void ClearPassword()
        {
            password.Clear();
            this.RefreshPwd();
        }
        private void RemovePassword()
        {
            if (password != null && password.Count > 0)
            {
                password.RemoveAt(password.Count - 1);
                this.RefreshPwd();
            }
        }
        #endregion

        #region 无奈总改只能写代码生成按钮了
        [Tooltip("普通按钮名字")]
        public string ButtonSpriteName;
        [Tooltip("特殊按钮名称 清空")]
        public string ButtonSpriteNameQingKong;
        [Tooltip("特殊按钮名称 回退")]
        public string ButtonSpriteNameHuiTui;
        [Tooltip("数字前缀")]
        public string ButtonNumSpriteNamePre;

        private void InitButton()
        {
            var list = new List<int>();
            for (int i = 1; i <= 12; i++)
            {
                list.Add(i);
            }
            //键盘
            UITools.CreateChild<int>(ButtonKeyRoot, null, list, (go, index) =>
            {
                string tempSpriteName = "";
                string tempNumSpriteName = "";
                if (index == 10)//清空
                {
                    tempSpriteName = ButtonSpriteNameQingKong;
                    tempNumSpriteName = "清空";
                    //tempNumSpriteName = ButtonNumSpriteNamePre + "qingkong";
                }
                else if(index == 11)//0
                {
                    tempSpriteName = ButtonSpriteName;
                    tempNumSpriteName = "0";
                    //tempNumSpriteName = ButtonNumSpriteNamePre + "0";
                }
                else if(index == 12)//回退
                {
                    tempSpriteName = ButtonSpriteNameHuiTui;
                    tempNumSpriteName = "回退";
                    //tempNumSpriteName = ButtonNumSpriteNamePre + "huitui";
                }
                else
                {
                    tempSpriteName = ButtonSpriteName;
                    tempNumSpriteName = index.ToString();
                    //tempNumSpriteName = ButtonNumSpriteNamePre + index;
                }

                go.GetComponent<UISprite>().spriteName = tempSpriteName;
                UILabel NumContent = go.transform.FindChild("NumContent").GetComponent<UILabel>();
                //NumContent.spriteName = tempNumSpriteName;
                if(tempNumSpriteName.Length > 1)
                {
                    NumContent.fontSize = 38;
                    NumContent.gradientBottom = Color.white;
                    NumContent.color = Color.white;
                }
                NumContent.text = tempNumSpriteName;
                NumContent.MakePixelPerfect();
            });
            ButtonKeyRoot.GetComponent<UIGrid>().Reposition();
        }
        #endregion

        #region Event
        private void OnKeyClick(GameObject go)
        {
            int index = this.ButtonKeyList.IndexOf(go);
            if (index < 0) return;

            switch (index)
            {
                case 0:
                case 1:
                case 2:
                case 3:
                case 4:
                case 5:
                case 6:
                case 7:
                case 8:
                    this.AddPassword(index + 1);
                    break;

                case 9:
                    this.ClearPassword();
                    break;

                case 10:
                    this.AddPassword(0);
                    break;

                case 11:
                    this.RemovePassword();
                    break;
                default:
                    QLoger.ERROR("键盘输入错误");
                    break;
            }

        }

        //关闭按钮点击
        private void OnCloseClick(GameObject go)
        {
            Hide();
        }
        #endregion


        #region override
        public override void Init()
        {
            InitButton();

            for (int i = 0; i < ButtonKeyRoot.childCount; ++i)
            {
                GameObject go = ButtonKeyRoot.GetChild(i).gameObject;
                UIEventListener.Get(go).onClick = OnKeyClick;
                ButtonKeyList.Add(go);
            }

            for (int i = 0; i < PwdRoot.childCount; ++i)
            {
                UILabel img = PwdRoot.GetChild(i).GetChild(0).GetComponent<UILabel>();
                PwdList.Add(img);
            }

            //关闭按钮
            UIEventListener.Get(ButtonClose).onClick = OnCloseClick;
        }

        public override void OnShow()
        {
            //this.ShowMask();
            ClearPassword();
        }

        public override void OnHide()
        {
        }

        #endregion

        #endregion
    }
}
