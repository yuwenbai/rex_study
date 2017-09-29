/**
 * @Author Xin.Wang
 *
 *
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConstDefine
{

    #region Mahjong
    public const int MJ_PK_NULL = 0x00000000;

    public const int MJ_PK_PENG = 0x01 << 1;
    public const int MJ_PK_GANG = 0x01 << 2;
    public const int MJ_PK_TING = 0x01 << 3;
    public const int MJ_PK_HUPAI = 0x01 << 4;
    public const int MJ_PK_CHI = 0x01 << 5;
    public const int MJ_PK_DUANGANG = 0x01 << 6;
    public const int Mj_PK_MINGLOU = 0x01 << 7;
    public const int Mj_PK_TINGGANG = 0x01 << 8;
    public const int Mj_PK_XUANPIAO = 0x01 << 9;
    public const int Mj_Pk_CIHU = 0x01 << 10;


    #region Anim playTime all
    //public const float Mj_Anim_NoTime = 0.01f;
    //public const float Mj_Anim_NormalTime = 0.5f;

    //public const float Mj_Anim_GameStart = 2.0f;
    //public const float Mj_Anim_Roll = 2.5f;
    //public const float Mj_Anim_CreateCode = 4.5f;
    //public const float Mj_Anim_DealCard = 2.5f;
    //public const float Mj_Anim_OpenCard = 2.0f;

    //public const float Mj_Anim_Get = 0.35f;
    //public const float Mj_Anim_Put = 0.8f;
    //public const float Mj_Anim_Flip = 2.0f;
    //public const float Mj_Anim_ChangeThree = 3.0f;
    //public const float Mj_Anim_ChangeFlower = 5.0f;
    //public const float Mj_Anim_ControlCreate = 0.5f;
    //public const float Mj_Anim_Ting = 0.5f;
    //public const float Mj_Anim_Hu = 1.5f;
    //public const float Mj_Anim_Mao = 1.0f;

    //public const float Mj_Anim_BuyHorse = 5.0f;
    //public const float Mj_Anim_BalancePutdown = 0.5f;
    //public const float Mj_Anim_BalanceHuaJiao = 1.0f;
    //public const float Mj_Anim_BalanceTuishui = 1.0f;
    //public const float Mj_Anim_Balance = 8.0f;
    //public const float Mj_Anim_BalanceLiuju = 4.0f;
    //public const float Mj_Anim_SiJiaMaiMaResult = 4.0f;
    #endregion


    #region Anim playtime part
    //public static float[] Mj_Anim3d_DealCard = new float[] { 0.3f, 0.4f };
    //public const float Mj_Anim3d_OpenCard = 0.2f;
    //public const float Mj_Anim3d_OpenInDown = 0.3f;
    //public static float[] Mj_Anim3d_ChangeSort = new float[] { 0.2f, 0.2f, 0.2f };
    //public static float[] Mj_Anim3d_ChangeFlower = new float[] { 0.4f, 1.0f, 0.4f };
    //public static float[] Mj_Anim3d_ChangeThree = new float[] { 1.5f, 1.0f };
    //public static float[] Mj_Anim3d_FloatInDown = new float[] { 0.1f, 0.1f };
    //public static float[] Mj_Anim3d_MoveInDown = new float[] { 0.3f, 0.2f };
    //public static float[] Mj_Anim3d_FlipInDown = new float[] { 0.1f, 0.04f, 0.1f, 0.02f, 0.1f };
    //public const float Mj_Anim3d_MoveInPut = 0.5f;
    //public static float[] Mj_Anim3D_RotateInDown = new float[] { 0.2f, 0.4f };          //翻开时间，延迟时间
    //public static float[] Mj_Anim3D_AutoDown = new float[] { 0.35f, 0.5f };              //麻将在牌墙中自动落下: 落下时间，延迟时间
    //public static float[] Mj_Anim3D_ChangeFlower = new float[] { 1.5f, 0, 0f };          //麻将补花 
    #endregion


    #region Anim Playtime 2D
    public const float Mj_Anim2d_CloseWait = 1.5f;                                      //关闭场景等待时间
    public const float Mj_Anim2d_PutTips = 1.0f;                                        //出牌的2D展示时间
    public const float Mj_Anim2d_BaseTips = 1.5f;                                       //吃碰杠听等基本展示时间
    public const float Mj_Anim2d_BaseTipsDelay = 0.6f;                                  //基本展示延迟时间
    public static float[] Mj_Anim2d_HuTips = new float[] { 1.5f, 1.5f };                //展示胡牌提示的时间
    public static float[] Mj_Anim2d_BuyHorse = new float[] { 1.5f, 3.0f };              //买马的展示时间 ：延迟显示结果，显示结果之后等待多久
    public const float Mj_Anim2d_MaBaoAfterFly = 3.0f;                                  //抓鸟跟扎马爆开的显示时间
    public static float[] Mj_Anim2d_SiJiaMaiMa = new float[] { 0.5f, 3.5f };            //四家买马结果动画事件
    #endregion


    #region Anim Playtime FX
    public const float Mj_AnimFx_Gang = 1.5f;
    public const float Mj_AnimFx_Hu = 2.5f;
    public const float Mj_AnimFx_Hua = 1.0f;
    public const float Mj_AnimFx_Que = 1.0f;
    public const float Mj_AnimFx_PengGang = 0.5f;
    public const float Mj_AnimFx_MaFly = 1.0f;
    #endregion



    #region UIOpenMa
    public const float OpenMaFlippingTime = 0.3f;
    public const float OpenMaFlippingIntervalTime = 0.2f;
    public const float UICloseDlay = 2f;
    public const float OpenMaUIOpenTime = 0.2f;
    #endregion


    #region 局内界面参数

    #region 扎马动画界面PrefabEffectID
    public const int CatchHorseConfigID = 1;
    public const int CatchBirdConfigID = 2;
    public const float CatchHorseAnimTime = 0.35f;
    public const float CatchHorseAnimSmokeTime = 1f;
    #endregion

    #region 杠后拿吃
    public const int UIMahjongNaChi_PrefabConfigID = 3;
    public const int UIMahjongNaChiChoose_PrefabConfigID = 4;
    public const int UIMahjongNaChiChooseTips_PrefabConfigID = 5;
    public const int UIMahjongNaChiChooseItem_PrefabConfigID = 6;
    public const int UIBankerFirstAlertConfig = 7;
    #endregion

    #region 赌暗杠
    public const float DAG_HideDelayTime = 2f;
    public const float DAG_HideAlertInfoDelayTime = 2f;
    #endregion

    #endregion

    #region 临时状态开关
    public static bool ShowPlayBack = true;                            //是否显示回放按钮（大小结算）
    #endregion

    #endregion


}
