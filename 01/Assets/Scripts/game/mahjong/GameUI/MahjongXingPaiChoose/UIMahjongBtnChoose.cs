/**
* @Author Xin.Wang
*
*
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace projectQ
{
    public class UIMahjongBtnChoose : UIViewBase
    {
        private class BtnChooseDataTwo
        {
            public string[] chooseName = null;

            public BtnChooseDataTwo(params string[] names)
            {
                if (names != null)
                {
                    int length = names.Length;
                    chooseName = new string[length];
                    for (int i = 0; i < length; i++)
                    {
                        chooseName[i] = names[i];
                    }
                }
            }
        }


        public override void Init()
        {

        }

        public override void OnHide()
        {

        }

        public override void OnShow()
        {
            IniData();
        }

        public GameObject btnItem = null;
        public UIGrid btn_Grid = null;
        public GameObject tipsCommon = null;

        private UIButton obj_ChangeThree = null;
        private UILabel label_ChangeThree = null;

        private float cutDownTime = -1;
        private EnumMjSpecialCheck specialType = EnumMjSpecialCheck.Null;
        private bool haveSend = false;


        /// <summary>
        /// 多选按钮
        /// </summary>
        private List<int> btnNumList = null;
        private int defaultNum = -1;

        /// <summary>
        /// 换三张 
        /// </summary>
        private List<int> chooseThreeList = null;

        /// <summary>
        /// 死蹲死顶
        /// </summary>
        private BtnChooseDataTwo _processChoose = null;




        public override void OnPushData(object[] data)
        {
            if (data != null && data.Length > 0)
            {
                specialType = (EnumMjSpecialCheck)data[0];
                switch (specialType)
                {
                    case EnumMjSpecialCheck.MjBaseCheckType_ChangeThree:
                        {
                            cutDownTime = (float)data[1];
                            chooseThreeList = (List<int>)data[2];
                        }
                        break;
                    case EnumMjSpecialCheck.MjBaseCheckType_XiaYuNingXia:
                    case EnumMjSpecialCheck.MjBaseCheckType_XiaBang:
                    case EnumMjSpecialCheck.MjBaseCheckType_XiaYu:
                    case EnumMjSpecialCheck.MjBaseCheckType_XiaPao:
                    case EnumMjSpecialCheck.MjBaseCheckType_XiaPiao:
                    case EnumMjSpecialCheck.MjBaseCheckType_XiaMa:
                    case EnumMjSpecialCheck.MJBaseCheckType_XiaPaoZi:
                    case EnumMjSpecialCheck.MjBaseCheckType_XinXiaPiao:
                        {
                            btnNumList = (List<int>)data[1];
                            defaultNum = (int)data[2];
                        }
                        break;
                    case EnumMjSpecialCheck.MjBaseCheckType_SiDunSiDing:
                        {
                            int chooseType = (int)data[1];
                            switch (chooseType)
                            {
                                case 1:
                                    {
                                        _processChoose = new BtnChooseDataTwo("蹲", "不蹲");
                                    }
                                    break;
                                case 2:
                                    {
                                        _processChoose = new BtnChooseDataTwo("顶", "不顶");
                                    }
                                    break;
                            }
                        }
                        break;
                    case EnumMjSpecialCheck.MjBaseCheckType_LiangYi:
                        {

                        }
                        break;
                }
            }
        }


        private void IniData()
        {
            btn_Grid.gameObject.SetActive(false);
            btn_Grid.transform.DestroyChildren();
            SetTipsContent();
            tipsCommon.SetActive(false);

            switch (specialType)
            {
                case EnumMjSpecialCheck.MjBaseCheckType_ChangeThree:
                    {
                        SetDataChangeThree();
                    }
                    break;
                case EnumMjSpecialCheck.MjBaseCheckType_XiaYu:
                case EnumMjSpecialCheck.MjBaseCheckType_XiaPao:
                case EnumMjSpecialCheck.MjBaseCheckType_XiaPiao:
                case EnumMjSpecialCheck.MjBaseCheckType_XiaMa:
                case EnumMjSpecialCheck.MjBaseCheckType_XiaBang:
                case EnumMjSpecialCheck.MjBaseCheckType_XiaYuNingXia:
                case EnumMjSpecialCheck.MJBaseCheckType_XiaPaoZi:
                case EnumMjSpecialCheck.MjBaseCheckType_XinXiaPiao:
                    {
                        SetDataChoose();
                    }
                    break;
                case EnumMjSpecialCheck.MjBaseCheckType_SiDunSiDing:
                    {
                        //死蹲死顶
                        SetDataChooseTwo();
                    }
                    break;
                case EnumMjSpecialCheck.MjBaseCheckType_LiangYi:
                    {
                        SetDataLiangYi();
                    }
                    break;
            }

            NGUITools.SetActive(btn_Grid.gameObject, true);
            btn_Grid.Reposition();
        }

        /// <summary>
        /// 设置提示内容
        /// </summary>
        private void SetTipsContent()
        {
            UILabel label = tipsCommon.GetComponentInChildren<UILabel>(true);
            if (label != null)
            {
                label.text = CardHelper.GetBtnChooseTips(specialType);
            }
        }


        #region 下跑 下炮 下鱼

        private void SetDataChoose()
        {
            if (btnNumList != null && btnNumList.Count > 0)
            {
                Transform gridTransP = btn_Grid.transform;
                for (int i = 0; i < btnNumList.Count; i++)
                {
                    GameObject paoBtn = GameTools.InstantiatePrefab(btnItem, null, true, true);
                    paoBtn.name = i.ToString();
                    UIEventListener.Get(paoBtn).onClick = ClickBtnPao;
                    string forMat = CardHelper.GetBtnChooseStr(specialType, btnNumList[i]);

                    UILabel label = paoBtn.transform.FindChild("Label").GetComponent<UILabel>();
                    if (label)
                    {
                        label.text = forMat;
                    }

                    UIMahjongBtnChooseItem item = paoBtn.GetComponent<UIMahjongBtnChooseItem>();
                    if (item == null)
                    {
                        item = paoBtn.AddComponent<UIMahjongBtnChooseItem>();
                    }
                    item.SetBtnValue(btnNumList[i]);

                    paoBtn.transform.SetParent(gridTransP, false);
                    paoBtn.SetActive(true);
                }
            }
        }

        private void ClickBtnPao(GameObject obj)
        {
            if (haveSend)
            {
                return;
            }
            haveSend = true;

            //anim
            MahjongAnimEventItem eventItem = obj.GetComponent<MahjongAnimEventItem>();
            if (eventItem != null)
            {
                eventItem.IniAnimEvent(obj.name, null, AnimPaoCallBack);
                eventItem.PlayAnim(null);
            }
            else
            {
                AnimPaoCallBack(obj.name);
            }


            int paoNum = 0;
            UIMahjongBtnChooseItem item = obj.GetComponent<UIMahjongBtnChooseItem>();
            if (item != null)
            {
                paoNum = item.GetBtnValue();
            }

            EventDispatcher.FireEvent(GEnum.NamedEvent.EMjControlClickChooseBtn, specialType, paoNum);
        }

        private void AnimPaoCallBack(string vars)
        {
            Close();
        }


        #endregion


        #region 换三张
        private void SetDataChangeThree()
        {
            GameObject paoBtn = GameTools.InstantiatePrefab(btnItem, null, true, true);
            UIEventListener.Get(paoBtn).onClick = ClickBtnChooseThree;
            obj_ChangeThree = paoBtn.GetComponent<UIButton>();
            label_ChangeThree = paoBtn.transform.FindChild("Label").GetComponent<UILabel>();
            if (label_ChangeThree)
            {
                label_ChangeThree.text = "确定";
            }
            paoBtn.transform.SetParent(btn_Grid.transform, false);
            paoBtn.SetActive(true);
            RefreshBtnState(true, chooseThreeList);
            tipsCommon.SetActive(true);
        }



        public void RefreshBtnState(bool isShow, List<int> chooseList)
        {
            obj_ChangeThree.isEnabled = isShow;
            this.chooseThreeList = chooseList;
        }



        private void ClickBtnChooseThree(GameObject obj)
        {
            if (haveSend)
            {
                return;
            }
            haveSend = true;
            EventDispatcher.FireEvent(GEnum.NamedEvent.EMjControlSendChangeThree, chooseThreeList);

            MahjongAnimEventItem eventItem = obj.GetComponent<MahjongAnimEventItem>();
            if (eventItem != null)
            {
                eventItem.IniAnimEvent(null, null, AnimChangeThreeCallBack);
                eventItem.PlayAnim(null);
            }
            else
            {
                AnimChangeThreeCallBack(null);
            }
        }


        private void AnimChangeThreeCallBack(string para)
        {
            if (chooseThreeList != null && chooseThreeList.Count == 3)
            {
                chooseThreeList = null;
                obj_ChangeThree = null;
                label_ChangeThree = null;
            }

            Close();
        }

        #endregion


        #region 死蹲死顶

        private void SetDataChooseTwo()
        {
            if (_processChoose != null && _processChoose.chooseName != null)
            {
                Transform gridTransP = btn_Grid.transform;
                for (int i = 0; i < _processChoose.chooseName.Length; i++)
                {
                    GameObject paoBtn = GameTools.InstantiatePrefab(btnItem, null, true, true);
                    paoBtn.name = i.ToString();
                    UIEventListener.Get(paoBtn).onClick = ClickBtnChooseTwo;
                    string forMat = _processChoose.chooseName[i];

                    UILabel label = paoBtn.transform.FindChild("Label").GetComponent<UILabel>();
                    if (label)
                    {
                        label.text = forMat;
                    }

                    paoBtn.transform.SetParent(gridTransP, false);
                    paoBtn.SetActive(true);
                }
            }
        }

        private void ClickBtnChooseTwo(GameObject obj)
        {
            //anim
            MahjongAnimEventItem eventItem = obj.GetComponent<MahjongAnimEventItem>();
            if (eventItem != null)
            {
                eventItem.IniAnimEvent(obj.name, null, AnimPaoCallBack);
                eventItem.PlayAnim(null);
            }
            else
            {
                AnimPaoCallBack(obj.name);
            }


            int paoNum = 0;
            int.TryParse(obj.name, out paoNum);

            EventDispatcher.FireEvent(GEnum.NamedEvent.EMjControlClickChooseBtn, specialType, paoNum);
        }


        #endregion

        #region 亮一
        private void SetDataLiangYi()
        {
            GameObject paoBtn = GameTools.InstantiatePrefab(btnItem, null, true, true);
            UIEventListener.Get(paoBtn).onClick = ClickBtnLiangYi;
            obj_ChangeThree = paoBtn.GetComponent<UIButton>();
            label_ChangeThree = paoBtn.transform.FindChild("Label").GetComponent<UILabel>();
            if (label_ChangeThree)
            {
                label_ChangeThree.text = "确定";
            }
            paoBtn.transform.SetParent(btn_Grid.transform, false);
            paoBtn.SetActive(true);
            RefreshBtnStateLiangyi(false);
            tipsCommon.SetActive(true);
        }

        public void RefreshBtnStateLiangyi(bool isShow)
        {
            obj_ChangeThree.isEnabled = isShow;
        }

        public void ClickBtnLiangYi(GameObject obj)
        {
            if (haveSend)
            {
                return;
            }
            haveSend = true;
            MjDataManager.Instance.LiangyiUISendChooseResult();

            MahjongAnimEventItem eventItem = obj.GetComponent<MahjongAnimEventItem>();
            if (eventItem != null)
            {
                eventItem.IniAnimEvent(null, null, AnimChangeThreeCallBack);
                eventItem.PlayAnim(null);
            }
            else
            {
                AnimChangeThreeCallBack(null);
            }
        }

        #endregion


    }

}
