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
    public class UIMahjongChangeThreeTips : UIViewBase
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

        public GameObject obj_ChangeShun = null;
        public GameObject obj_ChangeNi = null;
        public GameObject obj_ChangeDui = null;

        private EnumClockType clockType = EnumClockType.ClockType_OppoSite;


        public override void OnPushData(object[] data)
        {
            if (data.Length > 0)
            {
                clockType = (EnumClockType)data[0];
            }
        }


        private void IniData()
        {
            switch (clockType)
            {
                case EnumClockType.ClockType_OppoSite:
                    {
                        obj_ChangeDui.SetActive(true);
                    }
                    break;
                case EnumClockType.ClockType_ClockWise:
                    {
                        obj_ChangeShun.SetActive(true);
                    }
                    break;
                case EnumClockType.ClockType_CounterClockWise:
                    {
                        obj_ChangeNi.SetActive(true);
                    }
                    break;
            }
        }


    }

}

