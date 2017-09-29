/**
* @Author YQC
*
*
*/
using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;

namespace projectQ
{
    [Serializable,XmlRoot("SysLoginData")]
    public class SysLoginData
    {
        public enum EnumLoginType
        {
            /// <summary>
            /// 普通的账号密码登录
            /// </summary>
            NormalLogin = 0,

            /// <summary>
            /// 微信登录
            /// </summary>
            WeiXinLogin = 1,

            /// <summary>
            /// 游客登录
            /// </summary>
            GuestLogin = 2,

            /// <summary>
            /// 自动登录
            /// </summary>
            AutoLogin = 3,
        }

        private string _userName;
        private string _passWord;
        private EnumLoginType _loginType;
        private string _openId;

        /// <summary>
        /// 用户名
        /// </summary>
        public string UserName
        {
            private set { _userName = value; }
            get {  return _userName; }
        }


        /// <summary>
        /// 密码
        /// </summary>
        public string PassWord
        {
            private set { _passWord = value; }
            get { return _passWord; }
        }

        /// <summary>
        /// 登录类型 
        /// 0: 游戏内登录
        /// 1: 微信登录 
        /// 2: 游客登录 
        /// 3: 自动登录
        /// </summary>
        public EnumLoginType LoginType
        {
            private set { _loginType = value; }
            get { return _loginType; }
        }
        public string OpenId
        {
             private set { _openId = value; }
             get { return _openId; }
        }

        /// <summary>
        /// 自动登录时候使用
        /// </summary>
        public void SetOpenId(string value)
        {
            if(string.IsNullOrEmpty(value))
            {
                this.OpenId = null;
            }
            else
            {
                OpenId = this.Encrypt(value);
            }
        }
        public string GetOpenId()
        {
            if (string.IsNullOrEmpty(OpenId)) return null;
            return Decrypt(OpenId);
        }


        #region API
        /// <summary>
        /// 设置值
        /// </summary>
        public void SetData(EnumLoginType type,string userName,string password = null,string optionId = null)
        {
            switch(type)
            {
                case EnumLoginType.NormalLogin:
                    this.LoginType = type;
                    this.UserName = userName;
                    this.PassWord = password;
                    this.OpenId = null;
                    break;
                case EnumLoginType.WeiXinLogin:
                    this.LoginType = type;
                    this.UserName = userName;
                    this.PassWord = string.Empty;
                    this.OpenId = null;
                    break;
                case EnumLoginType.GuestLogin:
                    this.LoginType = type;
                    this.UserName = string.Empty;
                    this.PassWord = string.Empty;
                    this.OpenId = null;
                    break;
                case EnumLoginType.AutoLogin:
                    this.SetOpenId(optionId);
                    break;
            }
        }

        /// <summary>
        /// 取得值
        /// </summary>
        public Msg.LoginReq GeLoginReqData(EnumLoginType type)
        {
            //先检查OpenID 如果没有 则改成普通登录
            if(type == EnumLoginType.AutoLogin)
            {
                var OpenId =this.GetOpenId();
                if(string.IsNullOrEmpty(OpenId))
                {
                    type = this.LoginType;
                }
            }


            var result = new Msg.LoginReq();
            result.logintype = (int)type;
            string verion = Tools_VersionClient.Versionclient_Find();
            result.verion = string.IsNullOrEmpty(verion) ? Application.version : verion;


            switch (type)
            {
                case EnumLoginType.NormalLogin:
                case EnumLoginType.WeiXinLogin:
                case EnumLoginType.GuestLogin:
                    {
                        result.account = this.UserName;
                        result.password = this.PassWord;
                    }
                    break;
                case EnumLoginType.AutoLogin:
                    {
                        result.account = this.GetOpenId();
                        result.password = this.PassWord;
                        if(this.LoginType == EnumLoginType.NormalLogin)
                        {
                            result.logintype = (int)EnumLoginType.NormalLogin;
                        }
                    }
                    break;
            }


            /**
             * 王大将军 onz4zxDyotc5dvm_Dcq-xdg-bvYw 
             * 风满楼   onz4zxLQ6V8WWZfQJ-23nQzJPv7A 
             */
            //测试用 使用OpenId把这里解开解开解开
            //result.account = "onz4zxLQ6V8WWZfQJ-23nQzJPv7A"; //填写OpenID
            //result.password = "";     //不改
            //123456result.logintype = 3;     //不改


            return result;
        }
        #endregion
        
        /// <summary>
        /// 加密
        /// </summary>
        private string Encrypt(string str)
        {
            if (string.IsNullOrEmpty(str)) return null;
            return DESPassword.Encrypt(str, "pFV78VlX");
        }

        /// <summary>
        /// 解密
        /// </summary>
        private string Decrypt(string str)
        {
            if (string.IsNullOrEmpty(str)) return null;
            return DESPassword.Decrypt(str, "pFV78VlX");
        }
    }

    
    public class SysLoginDataMgr
    {
        private SysLoginData _loginData;

        public SysLoginData LoginData
        {
            private set { 
                _loginData = value;

            }
            get {
                if(_loginData == null)
                    this.Reset();

                if (_loginData == null)
                    _loginData = new SysLoginData();

                return _loginData;
            }
        }

        public void SetDataByNormalLogin(string userName,string password)
        {
            LoginData.SetData(SysLoginData.EnumLoginType.NormalLogin, userName, password);
        }
        public void SetDataByWeiXinLogin(string userName)
        {
            LoginData.SetData(SysLoginData.EnumLoginType.WeiXinLogin, userName);
        }
        public void SetDataByAutoLogin(string openId)
        {
            LoginData.SetData(SysLoginData.EnumLoginType.AutoLogin, null, null ,openId);
        }

        public Msg.LoginReq GeLoginReqData(SysLoginData.EnumLoginType type)
        {
            if(this.LoginData == null)
            {
                Reset();
            }
            
            if(this.LoginData != null)
            {
                return this.LoginData.GeLoginReqData(type);
            }
            return null;
        }

        public void Reset()
        {
            LoginData = PlayerPrefsTools.GetObject<SysLoginData>(MKey.SYS_LOGIN_DATA);
        }
        /// <summary>
        /// 保存到本地存储
        /// </summary>
        public void Save()
        {
            if(LoginData != null)
            {
                PlayerPrefsTools.SetObject<SysLoginData>(MKey.SYS_LOGIN_DATA, LoginData);
            }
        }


        public void ClearAutoLoginData()
        {
            LoginData.SetOpenId(null);
            Save();
        }
    }
    #region 内存数据
    public partial class MKey
    {
        public const string SYS_LOGIN_DATA = "SYS_LOGIN_DATA";
    }

    public partial class MemoryData
    {
        static public SysLoginDataMgr LoginDataMgr
        {
            get
            {
                SysLoginDataMgr itemData = MemoryData.Get<SysLoginDataMgr>(MKey.SYS_LOGIN_DATA);

                if (itemData == null)
                {
                    itemData = new SysLoginDataMgr();
                    MemoryData.Set(MKey.SYS_LOGIN_DATA, itemData);
                }
                return itemData;
            }
        }
    }
    #endregion
}
 
 
 
 