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
    public class TestAnim : MonoBehaviour
    {
        public MahJongCard cardArray = null;

        public int flipNewIndex = 0;

        public MahjongUI3D _ui3D = null;


        public int clockType = 1;



        private void Awake()
        {

        }


        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.A))
            {
                //for (int i = 0; i < cardArray.Length; i++)
                //{
                //    cardArray[i].needAutoDown = true;
                //}


                //for (int i = 0; i < cardArray.Length; i++)
                //{
                //    cardArray[i].SetCardUpAutoDown();
                //}

                cardArray.selfTransform.localPosition = Vector3.right * (13 * GameConst.mahJongWidth + GameConst.mahJongTempBase);


                flipNewIndex = Mathf.Min(flipNewIndex, 12);
                flipNewIndex = Mathf.Max(flipNewIndex, 0);

                cardArray.CurListIndex = 13;
                cardArray.NewListIndex = flipNewIndex;

                cardArray.CardFilpAutoInDown();
            }


            //if (Input.GetKeyDown(KeyCode.B))
            //{
            //    for (int i = 0; i < 4; i++)
            //    {
            //        _ui3D.SetChangeThree(i, true);
            //    }

            //    _ui3D.TurnChangeThree((EnumClockType)clockType, null
            //    , ConstDefine.Mj_Anim3d_ChangeThree);

            //}

        }


    }

}

