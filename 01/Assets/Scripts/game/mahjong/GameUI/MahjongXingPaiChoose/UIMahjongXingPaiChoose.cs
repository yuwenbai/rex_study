/**
* @Author Xin.Wang
*
*
*/
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace projectQ
{
    public class UIMahjongXingPaiChoose : UIViewBase
    {
        public override void OnPushData(object[] data)
        {
            _paikouStruct = (MjPaiKou)data[0];
            if (data.Length > 1)
            {
                _needIni = (bool)data[1];
            }
        }

        public override void Init()
        {
            EventDispatcher.AddEvent(MJEnum.MjXingpaiChoose.XPC_EventClose.ToString(), EventClose);
            if (_needIni)
            {
                if (showBaseParent != null)
                {
                    showBaseParent.SetActive(false);
                }
                IniShowBase();
            }
        }

        public override void OnShow()
        {

        }

        public override void OnHide()
        {

        }

        protected override void OnClose()
        {
            EventDispatcher.RemoveEvent(MJEnum.MjXingpaiChoose.XPC_EventClose.ToString(), EventClose);
            if (showBaseParent != null)
            {
                showBaseParent.SetActive(true);
            }
            base.OnClose();
        }


        private void EventClose(object[] obj)
        {
            this.Close();
        }


        public void AnimShow()
        {
            if (showBaseParent != null)
            {
                showBaseParent.SetActive(true);
            }
        }


        //基本按钮
        public GameObject showBaseParent = null;

        public UITable grid_Btn = null;

        public GameObject obj_Cancel = null;
        public GameObject obj_Pass = null;

        //当前的牌口信息
        private MjPaiKou _paikouStruct;
        public bool _needIni = true;

        //gang
        public UIGrid grid_ChooseGang = null;
        public GameObject item_ChooseGang = null;

        //chi
        public UIGrid grid_ChooseChi = null;
        public GameObject item_ChooseChi = null;

        //ting
        public UIGrid grid_ShowTing = null;
        public Transform showTingParent = null;
        public GameObject item_ShowTing = null;
        public GameObject[] tingTips = null;


        private EnumMjOpAction curAnimAction = EnumMjOpAction.Null;

        private void SetCurAnim()
        {
            if (_paikouStruct.canZiMo)
            {
                curAnimAction = EnumMjOpAction.MjOp_Zimo;
            }
            else if (_paikouStruct.canHu)
            {
                curAnimAction = EnumMjOpAction.MjOp_HuPai;
            }
            else if (_paikouStruct.canPiao)
            {
                curAnimAction = EnumMjOpAction.MjOp_HuPiaoGenPiao;
            }
            else if (_paikouStruct.canCiHu)
            {
                curAnimAction = EnumMjOpAction.MjOp_CiHu;
            }
            else if (_paikouStruct.canTingGang)
            {
                curAnimAction = EnumMjOpAction.MjOp_TingGang;
            }
            else if (_paikouStruct.canGang)
            {
                curAnimAction = EnumMjOpAction.MjOp_Gang;
            }
            else if (_paikouStruct.canPeng)
            {
                curAnimAction = EnumMjOpAction.MjOp_Peng;
            }
            else if (_paikouStruct.canChi)
            {
                curAnimAction = EnumMjOpAction.MjOp_Chi;
            }
            else if (_paikouStruct.canMingLou)
            {
                curAnimAction = EnumMjOpAction.MjOp_Minglou;
            }
            else if (_paikouStruct.canTing)
            {
                curAnimAction = EnumMjOpAction.MjOp_Ting;
            }
        }


        private void SetChooseFX(GameObject obj, EnumMjOpAction action)
        {
            if (curAnimAction == action)
            {
                UIMahjongXingPaiChooseItem item = obj.GetComponent<UIMahjongXingPaiChooseItem>();
                item.IniXingPaiTexiao(true);
            }
        }


        private void IniShowBase()
        {
            obj_Cancel.SetActive(false);
            UIEventListener.Get(obj_Cancel).onClick = ClickCancelBtn;
            grid_Btn.gameObject.SetActive(true);

            SetCurAnim();

            int showCount = 999;
            ShowBaseBtn(obj_Pass, showCount, EnumMjOpAction.MjOp_Pass);
            showCount--;


            if (_paikouStruct.canTing)
            {
                GameObject obj_Ting = null;

                string path = null;
                string pathEnd = "Ting";
                string endTemp = MjDataManager.Instance.checkPaikouShowData(ConstDefine.MJ_PK_TING);
                if (endTemp != null)
                {
                    pathEnd = endTemp;
                }

                path = string.Format(GameConst.path_Anim_MjXingPaiChoose, pathEnd);

                obj_Ting = GameTools.InstantiatePrefab(path, grid_Btn.transform, true, true).gameObject;
                SetChooseFX(obj_Ting, EnumMjOpAction.MjOp_Ting);
                ShowBaseBtn(obj_Ting, showCount, EnumMjOpAction.MjOp_Ting);
                showCount--;
            }

            if (_paikouStruct.canMingLou)
            {
                GameObject obj_Ting = null;
                string path = null;
                string pathEnd = "Minglou";
                string endTemp = MjDataManager.Instance.checkPaikouShowData(ConstDefine.Mj_PK_MINGLOU);
                if (endTemp != null)
                {
                    pathEnd = endTemp;
                }
                path = string.Format(GameConst.path_Anim_MjXingPaiChoose, pathEnd);

                obj_Ting = GameTools.InstantiatePrefab(path, grid_Btn.transform, true, true).gameObject;
                SetChooseFX(obj_Ting, EnumMjOpAction.MjOp_Minglou);
                ShowBaseBtn(obj_Ting, showCount, EnumMjOpAction.MjOp_Minglou);
                showCount--;
            }


            if (_paikouStruct.canChi)
            {
                GameObject obj_Chi = null;
                string path = null;

                path = string.Format(GameConst.path_Anim_MjXingPaiChoose, "Chi");
                obj_Chi = GameTools.InstantiatePrefab(path, grid_Btn.transform, true, true).gameObject;
                SetChooseFX(obj_Chi, EnumMjOpAction.MjOp_Chi);
                ShowBaseBtn(obj_Chi, showCount, EnumMjOpAction.MjOp_Chi);
                showCount--;
            }

            if (_paikouStruct.canPeng)
            {
                GameObject obj_Peng = null;
                string path = null;

                path = string.Format(GameConst.path_Anim_MjXingPaiChoose, "Peng");

                obj_Peng = GameTools.InstantiatePrefab(path, grid_Btn.transform, true, true).gameObject;
                SetChooseFX(obj_Peng, EnumMjOpAction.MjOp_Peng);
                ShowBaseBtn(obj_Peng, showCount, EnumMjOpAction.MjOp_Peng);
                showCount--;
            }


            if (_paikouStruct.canGang)
            {
                GameObject obj_Gang = null;
                string path = null;

                path = string.Format(GameConst.path_Anim_MjXingPaiChoose, "Gang");

                obj_Gang = GameTools.InstantiatePrefab(path, grid_Btn.transform, true, true).gameObject;
                SetChooseFX(obj_Gang, EnumMjOpAction.MjOp_Gang);
                ShowBaseBtn(obj_Gang, showCount, EnumMjOpAction.MjOp_Gang);
                showCount--;
            }

            //听杠 回头放到杠下面
            if (_paikouStruct.canTingGang)
            {
                GameObject obj_Gang = null;
                string path = null;
                path = string.Format(GameConst.path_Anim_MjXingPaiChoose, "Tinggang");

                obj_Gang = GameTools.InstantiatePrefab(path, grid_Btn.transform, true, true).gameObject;
                SetChooseFX(obj_Gang, EnumMjOpAction.MjOp_TingGang);
                ShowBaseBtn(obj_Gang, showCount, EnumMjOpAction.MjOp_TingGang);
                showCount--;
            }

            if (_paikouStruct.canCiHu)
            {
                GameObject obj_Gang = null;
                string path = null;
                path = string.Format(GameConst.path_Anim_MjXingPaiChoose, "Ci");

                obj_Gang = GameTools.InstantiatePrefab(path, grid_Btn.transform, true, true).gameObject;
                SetChooseFX(obj_Gang, EnumMjOpAction.MjOp_CiHu);
                ShowBaseBtn(obj_Gang, showCount, EnumMjOpAction.MjOp_CiHu);
                showCount--;
            }

            if (_paikouStruct.canPiao)
            {
                GameObject obj_Gang = null;
                string path = null;
                path = string.Format(GameConst.path_Anim_MjXingPaiChoose, "Piao");

                obj_Gang = GameTools.InstantiatePrefab(path, grid_Btn.transform, true, true).gameObject;
                SetChooseFX(obj_Gang, EnumMjOpAction.MjOp_HuPiaoGenPiao);
                ShowBaseBtn(obj_Gang, showCount, EnumMjOpAction.MjOp_HuPiaoGenPiao);
                showCount--;
            }


            if (_paikouStruct.canZiMo)
            {
                GameObject obj_Zimo = null;
                string path = null;
                string pathEnd = "Zimo";
                string endTemp = MjDataManager.Instance.checkPaikouShowData(ConstDefine.MJ_PK_HUPAI);
                if (endTemp != null)
                {
                    pathEnd = endTemp;
                }

                path = string.Format(GameConst.path_Anim_MjXingPaiChoose, pathEnd);

                obj_Zimo = GameTools.InstantiatePrefab(path, grid_Btn.transform, true, true).gameObject;
                SetChooseFX(obj_Zimo, EnumMjOpAction.MjOp_Zimo);
                ShowBaseBtn(obj_Zimo, showCount, EnumMjOpAction.MjOp_Zimo);
                showCount--;
            }
            else
            {
                if (_paikouStruct.canHu)
                {
                    GameObject obj_Hu = null;
                    string path = null;
                    string pathEnd = "Hu";
                    string endTemp = MjDataManager.Instance.checkPaikouShowData(ConstDefine.MJ_PK_HUPAI);
                    if (endTemp != null)
                    {
                        pathEnd = endTemp;
                    }

                    path = string.Format(GameConst.path_Anim_MjXingPaiChoose, pathEnd);
                    obj_Hu = GameTools.InstantiatePrefab(path, grid_Btn.transform, true, true).gameObject;
                    SetChooseFX(obj_Hu, EnumMjOpAction.MjOp_HuPai);
                    ShowBaseBtn(obj_Hu, showCount, EnumMjOpAction.MjOp_HuPai);
                    showCount--;
                }
            }

            grid_Btn.Reposition();
        }


        private void ShowBaseBtn(GameObject obj, int showCount, EnumMjOpAction opaction)
        {
            obj.name = showCount.ToString();
            UIEventListener.Get(obj).onClick = delegate (GameObject go)
            {
                ClickBaseBtn(opaction);
            };

            obj.SetActive(true);
        }

        private void SetCancelBtnState(bool state)
        {
            if (state)
            {
                grid_Btn.gameObject.SetActive(false);
                obj_Cancel.SetActive(true);
                obj_Pass.SetActive(false);
            }
            else
            {
                obj_Cancel.SetActive(false);
                obj_Pass.SetActive(true);
                grid_Btn.gameObject.SetActive(true);

                grid_ChooseChi.gameObject.SetActive(false);
                grid_ChooseGang.gameObject.SetActive(false);
                grid_ShowTing.gameObject.SetActive(false);
                showTingParent.gameObject.SetActive(false);
            }

        }




        private void ClickCancelBtn(GameObject obj)
        {
            EventDispatcher.FireEvent(GEnum.NamedEvent.EMjControlClickOpaction, EnumMjOpAction.MjOp_Cancel);
            SetCancelBtnState(false);
        }


        private void ClickBaseBtn(EnumMjOpAction action)
        {
            EventDispatcher.FireEvent(MJEnum.MusicEnum.ME_PlaySoundFX.ToString(), GEnum.SoundEnum.desk_gang);
            switch (action)
            {
                case EnumMjOpAction.MjOp_Chi:
                    {
                        IniShowChi();
                    }
                    break;
                case EnumMjOpAction.MjOp_Gang:
                    {
                        IniShowGang(EnumMjOpAction.MjOp_Gang);
                    }
                    break;
                case EnumMjOpAction.MjOp_Minglou:
                    {
                        IniShowMingLou();
                    }
                    break;
                case EnumMjOpAction.MjOp_Ting:
                    {
                        IniShowTing();
                    }
                    break;
                case EnumMjOpAction.MjOp_HuPiaoGenPiao:
                case EnumMjOpAction.MjOp_Peng:
                case EnumMjOpAction.MjOp_HuPai:
                case EnumMjOpAction.MjOp_Zimo:
                case EnumMjOpAction.MjOp_Pass:
                    {
                        EventDispatcher.FireEvent(GEnum.NamedEvent.EMjControlClickOpaction, action);
                        Close();
                    }
                    break;
                case EnumMjOpAction.MjOp_TingGang:
                    {
                        IniShowGang(EnumMjOpAction.MjOp_TingGang);
                    }
                    break;
                case EnumMjOpAction.MjOp_CiHu:
                    {
                        IniShowGang(EnumMjOpAction.MjOp_CiHu);
                    }
                    break;
            }

        }


        //chi
        private void IniShowChi()
        {
            //HIDE
            SetCancelBtnState(true);

            List<MjActionCodeChi> chiList = MjDataManager.Instance.GetPaikouChi();
            if (chiList.Count == 1)
            {
                //只有一组
                ClickChooseChi(chiList[0]);
            }
            else
            {
                UITools.CreateChild<MjActionCodeChi>(grid_ChooseChi.transform, item_ChooseChi, chiList, CreateChiCallBack, true);
                grid_ChooseChi.gameObject.SetActive(true);
                grid_ChooseChi.Reposition();
            }
        }

        private void CreateChiCallBack(GameObject obj, MjActionCodeChi chiInfo)
        {
            UIMahjongChooseChi chooseChiInfo = obj.GetComponent<UIMahjongChooseChi>();
            chooseChiInfo.IniChooseChi(chiInfo, ClickChooseChi);
        }

        private void ClickChooseChi(MjActionCodeChi chiInfo)
        {
            EventDispatcher.FireEvent(GEnum.NamedEvent.EMjControlClickOpaction, EnumMjOpAction.MjOp_Chi, chiInfo);
            Close();
        }

        private EnumMjOpAction curGangType = EnumMjOpAction.MjOp_Gang;

        //gang
        private void IniShowGang(EnumMjOpAction gangType)
        {
            //HIDE
            curGangType = gangType;
            SetCancelBtnState(true);
            List<int> gangList = null;
            if (gangType == EnumMjOpAction.MjOp_CiHu)
            {
                gangList = MjDataManager.Instance.GetPaikouCiList();
            }
            else
            {
                gangList = MjDataManager.Instance.GetPaikouGangList();
            }
            int curGangCount = gangList.Count;

            if (curGangCount == 1)
            {
                //没有选择杠的余地
                int mjCode = 0;
                if (gangType == EnumMjOpAction.MjOp_CiHu)
                {
                    mjCode = MjDataManager.Instance.GetPaikouCi(0);
                }
                else
                {
                    mjCode = MjDataManager.Instance.GetPaikouGang(0);
                }

                EventDispatcher.FireEvent(GEnum.NamedEvent.EMjControlClickOpaction, gangType, mjCode);
                Close();
            }
            else
            {
                //显示选择杠哪个的UI
                UITools.CreateChild(grid_ChooseGang.transform, item_ChooseGang, gangList.Count, CreateGangCallBack, true);
                grid_ChooseGang.gameObject.SetActive(true);
                grid_ChooseGang.Reposition();
            }
        }

        private void CreateGangCallBack(GameObject obj, int index)
        {
            int gangID = 0;
            if (curGangType == EnumMjOpAction.MjOp_CiHu)
            {
                gangID = MjDataManager.Instance.GetPaikouCi(index);
            }
            else
            {
                gangID = MjDataManager.Instance.GetPaikouGang(index);
            }
            UIMahjongChooseGang chooseGang = obj.GetComponent<UIMahjongChooseGang>();
            chooseGang.IniChooseGang(gangID, ClickChooseGang);
        }

        private void ClickChooseGang(int gangID)
        {
            EventDispatcher.FireEvent(GEnum.NamedEvent.EMjControlClickOpaction, curGangType, gangID);
            Close();
        }

        //ting
        private void IniShowTing()
        {
            //HIDE
            SetCancelBtnState(true);
            EventDispatcher.FireEvent(GEnum.NamedEvent.EMjControlClickOpaction, EnumMjOpAction.MjOp_Ting);
        }

        private void IniShowMingLou()
        {
            SetCancelBtnState(true);
            EventDispatcher.FireEvent(GEnum.NamedEvent.EMjControlClickOpaction, EnumMjOpAction.MjOp_Minglou);
        }

        public void IniDefaultShowTing()
        {
            grid_Btn.gameObject.SetActive(false);
            obj_Cancel.SetActive(false);
            obj_Pass.SetActive(false);
        }


        public void RefreshShowTing(int chooseMj)
        {
            MjTingInfo info = MjDataManager.Instance.GetPaikouTing(chooseMj);
            if (info == null)
            {
                //close
                for (int i = 0; i < tingTips.Length; i++)
                {
                    tingTips[i].SetActive(false);
                    tingTips[i].transform.SetParent(showTingParent);
                }

                showTingParent.gameObject.SetActive(false);
                grid_ShowTing.transform.DestroyChildren();
                grid_ShowTing.gameObject.SetActive(false);
            }
            else
            {
                for (int i = 0; i < tingTips.Length; i++)
                {
                    tingTips[i].SetActive(false);
                    tingTips[i].transform.SetParent(showTingParent);
                }
                grid_ShowTing.transform.DestroyChildren();

                for (int i = 0; i < tingTips.Length; i++)
                {
                    tingTips[i].transform.SetParent(grid_ShowTing.transform);
                    tingTips[i].SetActive(true);
                }

                //show
                int huCount = info.huCodeNum.Count;
                List<MjTingInfoModel> modelList = new List<MjTingInfoModel>();
                for (int i = 0; i < huCount; i++)
                {
                    MjTingInfoModel item = new MjTingInfoModel(info.huCode[i], info.someNum[i], info.huCodeNum[i]);
                    modelList.Add(item);
                }

                MjDataManager.Instance.SortTingInfoModel(modelList, huCount <= 8);

                int showCount = Mathf.Min(huCount, 8);

                for (int i = 0; i < showCount; i++)
                {
                    SetObjToTing(modelList[i].mjCode, modelList[i].maxOdd, modelList[i].restCount);
                }


                showTingParent.gameObject.SetActive(true);
                grid_ShowTing.gameObject.SetActive(true);
                grid_ShowTing.Reposition();
            }
        }


        private void SetObjToTing(int mjCode, int someNum, int restNum)
        {
            GameObject obj = UITools.CloneObject(item_ShowTing, grid_ShowTing.gameObject);
            UIMahjongShowTing showTing = obj.GetComponent<UIMahjongShowTing>();
            showTing.IniShowTing(mjCode, someNum, restNum);
            obj.SetActive(true);

        }


    }

}

