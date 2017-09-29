using System.Collections;
using System.Collections.Generic;
using Msg;
using UnityEngine;

namespace projectQ
{
    public class UIBanner: UIViewBase
    {
        private UIBannerModel _mode;
        public UIBannerModel Model
        {
            get { return _model as UIBannerModel; }
        }
        public UITexture noticeTexture;
        public GameObject closeBtn;
        // Use this for initialization

        public override void Init()
        {
            if (noticeTexture == null)
            {
                noticeTexture = transform.FindChild("Anim/BannerTexture").GetComponent<UITexture>();
            }
            if (closeBtn == null)
            {
                closeBtn = transform.FindChild("Anim/BtnClose").gameObject;
            }
            UIEventListener.Get(closeBtn).onClick = OnCloseBtn;
        }
        public override void OnPushData(object[] data)
        {
            if (data == null || data.Length == 0)
            {
                this.Close();
                return;
            }
            List<ActivityNotify> activ = (List<ActivityNotify>)data[0];
            if (activ.Count == 0)
            {
                this.Close();
                DebugPro.Log("======================UIBanner  activ.Count == 0");
                return;
            }
            string resUrl = activ[0].ResUrl;//"Tex_Notice"
            if (string.IsNullOrEmpty(resUrl))
            {
                this.Close();
                DebugPro.Log("======================UIBanner  resUrl 为空");
                return;
            }
               
            DownHeadTexture.Instance.Activity_TextureGet(resUrl, TextureSetCallBack);
        }

        public override void OnShow()
        {
            
        }

        public override void OnHide()
        {
           
        }
        /// <summary>
        /// 获取活动图片资源回调
        /// </summary>
        void TextureSetCallBack(Texture2D tex2D, string headName)
        {
            noticeTexture.mainTexture = tex2D;
        }
        private void OnCloseBtn(GameObject _go)
        {
            this.Close();
        }
    }
}

