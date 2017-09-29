using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace projectQ
{

    public enum WindowMaskEffcType
    {
        None,
        Close,
        Hide
    }

    public class WindowUIBase : MonoBehaviour
    {

        #region Process func
        public WindowUIData SelfData;
        private System.Action<int> clickCloseBack = null;
        //初始化窗体控制 
        public virtual void Init(System.Action<int> clickClose)
        {
            clickCloseBack = clickClose;
            if (selfObj == null)
            {
                selfObj = this.gameObject;
            }
            if (selfTrans == null)
            {
                selfTrans = this.transform;
            }
            this.onInit();
        }

        //初始化窗体数据
        public virtual void SetData(WindowUIData windowData)
        {
            SelfData = windowData;
            this.onSetData();
        }

        //显示窗体
        public virtual void Open(GameObject parent = null)
        {
            if (parent != null)
            { //更换父节点的是
                if (selfTrans.parent != parent)
                {
                    //父节点调整的时候才重设
                    selfTrans.parent = parent.transform;
                    selfTrans.localPosition = Vector3.zero;
                    this.rootPanel = UITools.GetNearestPanel(this.gameObject);//重设RootPanel
                }
            }
            else
            { //挂载到默认挂载点
                if (selfTrans.parent == null)
                {
                    //没有挂在过挂载点
                    //this.transform.parent = UIManager.Instance.LoadPoint.transform;
                    selfTrans.localPosition = Vector3.zero;
                    this.rootPanel = UITools.GetNearestPanel(selfObj);//重设RootPanel
                }
            }

            isWindowShow = true;
            this.onOpen();
        }

        //重置窗体状态
        public virtual void Reset()
        {
            this.onReset();
        }

        //隐藏窗体
        public virtual void Hide()
        {
            this.onHide();
            isWindowShow = false;
            UITools.SetActive(selfObj, false);
        }

        public virtual void Show()
        {
            UITools.SetActive(selfObj, true);
            isWindowShow = true;
            this.onShow();
        }



        //关闭窗体
        public virtual void Close()
        {
            if (this.clickCloseBack != null)
            {
                clickCloseBack(SelfData.sortNum);
            }
            CloseCurWindow();
        }

        public virtual void PlaySound(bool isOpenUI)
        {
            if (isOpenUI)
            {
                //MusicCtrl.Instance.Music_SoundPlay(GEnum.SoundEnum.btn_select1);
            }
        }

        public void CloseCurWindow()
        {
            isWindowShow = false;
            this.onClose();
            GameObject.Destroy(selfObj);
        }

        #endregion

        #region Process Event
        public virtual void onInit()
        {

        }

        public virtual void onSetData()
        {

        }


        public virtual void onOpen()
        {

        }

        public virtual void onReset()
        {

        }

        public virtual void onHide()
        {

        }

        public virtual void onShow()
        {

        }

        public virtual void onClose()
        {

        }


        #endregion


        #region Propertys
        [HideInInspector]
        public GameObject selfObj = null;
        [HideInInspector]
        public Transform selfTrans = null;

        [HideInInspector]
        public UIPanel rootPanel = null;

        [HideInInspector]
        public UITexture windowMask = null; // the window's mask back

        [HideInInspector]
        public WindowMaskEffcType curMaskEffcType = WindowMaskEffcType.None; // click mask effect

        [HideInInspector]
        public bool isWindowShow = false;   // the window has showed

        #endregion

        #region other func
        UIWidget Mask = null;
        public void CreateMask(WindowMaskEffcType maskType = WindowMaskEffcType.None)
        {
            if (selfObj == null)
            {
                QLoger.ERROR("Create mask should init first", this);
            }
            UITools.ShowMask(selfObj, Mask);
            //UITools.MakeMask(selfObj);
        }


        #endregion




        #region Script Lifeline
        void Awake()
        {
        }

        void Start()
        { }

        void Update()
        { }

        void FixedUpdate()
        { }

        void LateUpdate()
        { }

        void OnDestroy()
        { }

        #endregion


    }


}
