using UnityEngine;
using System.Collections;

namespace projectQ
{
    public class MessageBoxUI : MonoBehaviour
    {
        public GameObject OkBtn;
        public GameObject NoBtn;
        public UILabel TitleLab;
        public UILabel DescLab;

        public MessageBoxMgr.OnCancel mOnCancel;
        public MessageBoxMgr.OnOK mOnOK;

        void Start()
        {
            UIEventListener.Get(OkBtn).onClick = OnOkBtnClick;
            UIEventListener.Get(NoBtn).onClick = OnNoBtnClick;
        }

        public string Title
        {
            get
            {
                return TitleLab.text;
            }
            set
            {
                TitleLab.text = value;
            }
        }

        public string Message
        {
            get
            {
                return DescLab.text;
            }
            set
            {
                DescLab.text = value;
            }
        }


        public void OnOkBtnClick(GameObject go)
        {
            if (mOnOK != null)
            {
                mOnOK();
            }

            //GameObject.Destroy(gameObject);
        }

        public void OnNoBtnClick(GameObject go)
        {
            if (mOnCancel != null)
            {
                mOnCancel();
            }

            //GameObject.Destroy(gameObject);
        }
    }
}