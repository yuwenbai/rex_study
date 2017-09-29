using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using projectQ.animation;

namespace projectQ
{
    public class SystemPopupWindow : WindowUIBase
    {
        #region component handle

        //标题
        public UILabel labelTitle = null;
        //内容文本
        public UILabel labelContent = null;
        //按钮列表
        public UIGrid btnsGrid = null;

        public GameObject ButtonClose = null;

        public AnimationItem item = null;

        //按钮文本
        [HideInInspector]
        public List<UILabel> btnsLabel = null;
        //按钮点击事件
        private System.Action<int> onbtnClick = null;

        public UIScrollView ScrollView = null;

        #endregion

        private IEnumerator ScrollViewResetPosition()
        {
            yield return null;
            if (this.ScrollView != null)
            {
                this.ScrollView.ResetPosition();
            }
        }

        private void OnCloseClick(GameObject go)
        {
            OnBtnClick(-1);
        }

        #region overide process fun

        //窗体初始化
        public override void Init(Action<int> clickClose)
        {
            base.Init(clickClose);
        }

        public override void SetData(WindowUIData windowData)
        {
            labelTitle.text = windowData.titleText;
            NGUITools.SetActive(labelTitle.gameObject, windowData.titleText != null);

            labelContent.text = windowData.contentText;
            NGUITools.SetActive(labelContent.gameObject, windowData.contentText != null);

            if (windowData.buttonTexts != null)
            {
                for (int i = 0; i < windowData.buttonTexts.Length || i < btnsLabel.Count; i++)
                {
                    if (i >= windowData.buttonTexts.Length)
                    {
                        UITools.SetActive(btnsLabel[i].transform.parent.gameObject, false);
                        continue;
                    }

                    if (i >= btnsLabel.Count)
                    {
                        GameObject addBtn = UITools.CloneObject(btnsLabel[0].transform.parent.gameObject, btnsLabel[0].transform.parent.parent.gameObject);
                        btnsLabel.Add(addBtn.GetComponentInChildren<UILabel>());
                    }

                    btnsLabel[i].text = windowData.buttonTexts[i];

                    //设置按钮图片
                    if (windowData.buttonTexts.Length == 1)
                    {
                        btnsLabel[i].transform.parent.GetComponent<UISprite>().spriteName = "public_button_01";
                    }
                    else
                    {
                        btnsLabel[i].transform.parent.GetComponent<UISprite>().spriteName = i == 0 ? "public_button_02" : "public_button_01";
                    }

                    UITools.SetActive(btnsLabel[i].transform.parent.gameObject, true);
                }
                UITools.SetActive(btnsGrid.gameObject);
                btnsGrid.Reposition();
            }

            onbtnClick = windowData.btnCall;
            if (this.ButtonClose != null)
            {
                if (this.ButtonClose.activeSelf != windowData.isCloseButton)
                {
                    ButtonClose.SetActive(windowData.isCloseButton);
                }
            }

            base.SetData(windowData);

            StartCoroutine(ScrollViewResetPosition());
        }

        public override void Open(GameObject parent = null)
        {
            if(item == null)
            {
                NGUITools.SetActive(selfObj, true);
                base.Open(parent);
            }
            else
            {
                _R.ui.PlayAnimation(item, true, (isOk) =>
                {
                    if(isOk)
                    {
                        NGUITools.SetActive(selfObj, true);
                        base.Open(parent);
                    }
                });
            }
        }

        public override void Close()
        {
            base.Close();
        }

        #endregion

        #region overide event func

        private void OnBtnClick(int index)
        {
            if(item == null)
            {
                if (onbtnClick != null)
                {
                    onbtnClick(index);
                }
                this.Close();
            }
            else
            {
                _R.ui.PlayAnimation(item, false, (isOk) =>
                {
                    if (isOk)
                    {
                        if (onbtnClick != null)
                        {
                            onbtnClick(index);
                        }
                        this.Close();
                    }
                });
            }
        }

        public override void onInit()
        {
            if (btnsGrid != null)
            {
                btnsLabel = new List<UILabel>(btnsGrid.transform.childCount);
                int childCount = btnsGrid.transform.childCount;
                for (int i = 0; i < childCount; ++i)
                {
                    GameObject childOBJ = btnsGrid.transform.GetChild(i).gameObject;
                    if (childOBJ != null)
                    {
                        UIEventListener.Get(childOBJ).onClick = delegate (GameObject go)
                        {
                            for (int j = 0; j < childCount; ++j)
                            {
                                if (btnsGrid.gameObject.activeInHierarchy && go == btnsGrid.transform.GetChild(j).gameObject)
                                {
                                    OnBtnClick(j);
                                }
                            }

                        };
                        btnsLabel.Add(childOBJ.transform.FindChild("Label_Button").GetComponent<UILabel>());
                    }
                }
                base.CreateMask(WindowMaskEffcType.None);
            }
            else
            {
                QLoger.ERROR("SystemPopupWindow : btnsGrid  is null !", this);
            }
            if (ButtonClose != null)
            {
                UIEventListener.Get(ButtonClose).onClick = OnCloseClick;
            }
            base.onInit();
        }

        public override void onSetData()
        {
            base.onSetData();
        }

        public override void onOpen()
        {
            base.onOpen();
        }

        public override void onClose()
        {
            base.onClose();
        }

        #endregion
    }
}