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
    public class UIMahjongXingPaiTipsState : MonoBehaviour
    {
        public UISprite[] spriteArray = null;

        public GameObject obj_DianP = null;

        public void IniState(EnumMjSpecialCheck stateType, bool isFirstStep)
        {
            obj_DianP.SetActive(isFirstStep);

            string[] nameArray = SetSpriteName(stateType, isFirstStep);

            for (int i = 0; i < nameArray.Length; i++)
            {
                spriteArray[i].spriteName = nameArray[i];
            }
        }


        private string[] SetSpriteName(EnumMjSpecialCheck stateType, bool isFirstStep)
        {
            string formatChid = "tip_icon_{0}";
            string[] nameArray = new string[3] { formatChid, formatChid, formatChid };
            string[] nameFormat = null;

            switch (stateType)
            {
                case EnumMjSpecialCheck.MjBaseCheckType_PaoTou:
                case EnumMjSpecialCheck.MjBaseCheckType_PaoKou:
                case EnumMjSpecialCheck.MjBaseCheckType_XiaPao:
                case EnumMjSpecialCheck.MjBaseCheckType_XinXiaPiao:
                    {
                        //下跑
                        if (isFirstStep)
                        {
                            nameFormat = new string[3] { "xia", "pao", "zhong" };
                        }
                        else
                        {
                            nameFormat = new string[3] { "yi", "xia", "pao" };
                        }
                    }
                    break;
                case EnumMjSpecialCheck.MjBaseCheckType_Lack:
                    {
                        //定缺
                        if (isFirstStep)
                        {
                            nameFormat = new string[3] { "ding", "que", "zhong" };
                        }
                        else
                        {
                            nameFormat = new string[3] { "yi", "ding", "que" };
                        }
                    }
                    break;
                case EnumMjSpecialCheck.MjBaseCheckType_LiangYi:
                case EnumMjSpecialCheck.MjBaseCheckType_ChangeThree:
                    {
                        //换三张
                        if (isFirstStep)
                        {
                            nameFormat = new string[3] { "xuan", "pai", "zhong" };
                        }
                        else
                        {
                            nameFormat = new string[3] { "yi", "xuan", "pai" };
                        }
                    }
                    break;
                case EnumMjSpecialCheck.MJBaseCheckType_XiaPaoZi:
                    {
                        //下炮子
                        if (isFirstStep)
                        {
                            nameFormat = new string[3] { "xia", "paozi", "zhong" };
                        }
                        else
                        {
                            nameFormat = new string[3] { "yi", "xia", "paozi" };
                        }
                    }
                    break;
                case EnumMjSpecialCheck.MjBaseCheckType_XiaYuNingXia:
                case EnumMjSpecialCheck.MjBaseCheckType_XiaYu:
                    {
                        //下鱼
                        if (isFirstStep)
                        {
                            nameFormat = new string[3] { "xia", "yu", "zhong" };
                        }
                        else
                        {
                            nameFormat = new string[3] { "yi", "xia", "yu" };
                        }
                    }
                    break;
                case EnumMjSpecialCheck.MjBaseCheckType_MingDa:
                case EnumMjSpecialCheck.MjBaseCheckType_KanNaoMo:
                    {
                        //坎牌 闹庄 末留
                        if (isFirstStep)
                        {
                            nameFormat = new string[3] { "xuan", "fen", "zhong" };
                        }
                        else
                        {
                            nameFormat = new string[3] { "yi", "xuan", "fen" };
                        }
                    }
                    break;
                case EnumMjSpecialCheck.MjBaseCheckType_PiaoSu:
                case EnumMjSpecialCheck.MjBaseCheckType_PiaoHu:
                case EnumMjSpecialCheck.MjBaseCheckType_PiaoGang:
                case EnumMjSpecialCheck.MjBaseCheckType_PiaoJin:
                    {
                        //选飘
                        if (isFirstStep)
                        {
                            nameFormat = new string[3] { "xuan", "fen", "zhong" };
                        }
                        else
                        {
                            nameFormat = new string[3] { "yi", "xuan", "fen" };
                        }

                    }
                    break;
                case EnumMjSpecialCheck.MjBaseCheckType_XiaPiao:
                    {
                        //下漂
                        if (isFirstStep)
                        {
                            nameFormat = new string[3] { "xia", "piao", "zhong" };
                        }
                        else
                        {
                            nameFormat = new string[3] { "yi", "xia", "piao" };
                        }
                    }
                    break;
                case EnumMjSpecialCheck.MjBaseCheckType_XiaBang:
                    {
                        //带绑
                        if (isFirstStep)
                        {
                            nameFormat = new string[3] { "xuan", "bang", "zhong" };
                        }
                        else
                        {
                            nameFormat = new string[3] { "yi", "xuan", "bang" };
                        }
                    }
                    break;
                case EnumMjSpecialCheck.MjBaseCheckType_XiaMa:
                    {
                        //下码
                        if (isFirstStep)
                        {
                            nameFormat = new string[3] { "xia", "ma", "zhong" };
                        }
                        else
                        {
                            nameFormat = new string[3] { "yi", "xia", "ma" };
                        }
                    }
                    break;
            }


            for (int i = 0; i < nameArray.Length; i++)
            {
                nameArray[i] = string.Format(nameArray[i], nameFormat[i]);
            }

            return nameArray;
        }




    }





}
