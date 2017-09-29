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
    public class UIMahjongQueChoose : UIViewBase
    {
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

        public GameObject[] obj_Wan = null;
        public GameObject[] obj_Tiao = null;
        public GameObject[] obj_Tong = null;

        private int defaultType = 1;

        public override void OnPushData(object[] data)
        {
            if (data != null && data.Length > 0)
            {
                defaultType = (int)data[0];
            }
        }


        public void IniData()
        {
            if (defaultType > 0)
            {
                //show default
                switch (defaultType)
                {
                    case 1:
                        {
                            obj_Wan[1].SetActive(true);
                        }
                        break;
                    case 2:
                        {
                            obj_Tiao[1].SetActive(true);
                        }
                        break;
                    case 3:
                        {
                            obj_Tong[1].SetActive(true);
                        }
                        break;

                }
            }

            UIEventListener.Get(obj_Wan[0]).onClick = ClickQue_Wan;
            UIEventListener.Get(obj_Tiao[0]).onClick = ClickQue_Tiao;
            UIEventListener.Get(obj_Tong[0]).onClick = ClickQue_Tong;
        }

        private int chooseNum = -1;

        public void ClickQue_Wan(GameObject obj)
        {
            chooseNum = 1;
            //ClickBtn(1);
            PlayAnim();
        }

        public void ClickQue_Tiao(GameObject obj)
        {
            chooseNum = 2;
            //ClickBtn(2);
            PlayAnim();
        }

        public void ClickQue_Tong(GameObject obj)
        {
            chooseNum = 3;
            //ClickBtn(3);
            PlayAnim();
        }

        private void PlayAnim()
        {
            obj_Wan[1].SetActive(false);
            obj_Tiao[1].SetActive(false);
            obj_Tong[1].SetActive(false);

            DestroyAuto auto = null;

            switch (chooseNum)
            {
                case 1:
                    {
                        //obj_Wan[1].SetActive(true);
                        auto = obj_Wan[2].GetComponent<DestroyAuto>();
                    }
                    break;
                case 2:
                    {
                        //obj_Tiao[1].SetActive(true);
                        auto = obj_Tiao[2].GetComponent<DestroyAuto>();
                    }
                    break;
                case 3:
                    {
                        //obj_Tong[1].SetActive(true);
                        auto = obj_Tong[2].GetComponent<DestroyAuto>();
                    }
                    break;
            }


            EventDispatcher.FireEvent(GEnum.NamedEvent.EMjControlClickChooseQue, chooseNum);
            auto.IniCom(ConstDefine.Mj_AnimFx_Que, AnimCall, false, true);
            auto.gameObject.SetActive(true);
        }


        private void AnimCall()
        {
            Close();
        }

    }

}
