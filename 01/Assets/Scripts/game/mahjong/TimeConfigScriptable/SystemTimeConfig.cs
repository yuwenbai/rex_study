/**
 * @Author Xin.Wang
 *
 *
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SystemTimeConfigData", menuName = "GameScriptableData/SystemTimeConfigData")]
public class SystemTimeConfig : ScriptableObject
{
    public float Time_Notime = 0.01f;
    public float Time_NormalTime = 0.5f;

    public float Mj_Anim_GameStart = 2.0f;
    public float Mj_Anim_Roll = 1.5f;
    public float Mj_Anim_CreateCode = 3.5f;
    public float Mj_Anim_DealCard = 2.5f;
    public float Mj_Anim_OpenCard = 2.0f;

    public float Mj_Anim_Get = 0.35f;
    public float Mj_Anim_Put = 0.8f;
    public float Mj_Anim_Flip = 2.0f;
    public float Mj_Anim_ChangeThree = 3.0f;
    public float Mj_Anim_ChangeFlower = 5.0f;
    public float Mj_Anim_ControlCreate = 0.5f;
    public float Mj_Anim_Ting = 0.5f;
    public float Mj_Anim_Hu = 1.5f;
    public float Mj_Anim_Mao = 1.0f;

    public float Mj_Anim_BuyHorse = 5.0f;
    public float Mj_Anim_BalancePutdown = 0.5f;
    public float Mj_Anim_BalanceHuaJiao = 1.0f;
    public float Mj_Anim_BalanceTuishui = 1.0f;
    public float Mj_Anim_Balance = 8.0f;
    public float Mj_Anim_BalanceLiuju = 4.0f;
    public float Mj_Anim_SiJiaMaiMaResult = 4.0f;


    [System.Serializable]
    public class PartTimeConfig
    {
        public float[] Mj_Anim3d_DealCard = new float[] { 0.3f, 0.4f };
        public float Mj_Anim3d_OpenCard = 0.2f;
        public float Mj_Anim3d_OpenInDown = 0.3f;
        public float[] Mj_Anim3d_ChangeSort = new float[] { 0.2f, 0.2f, 0.2f };
        public float[] Mj_Anim3d_ChangeFlower = new float[] { 0.4f, 1.0f, 0.4f };
        public float[] Mj_Anim3d_ChangeThree = new float[] { 1.5f, 1.0f };
        public float[] Mj_Anim3d_FloatInDown = new float[] { 0.1f, 0.1f };
        public float[] Mj_Anim3d_MoveInDown = new float[] { 0.3f, 0.2f };
        public float[] Mj_Anim3d_FlipInDown = new float[] { 0.1f, 0.04f, 0.1f, 0.02f, 0.1f };
        public float Mj_Anim3d_MoveInPut = 0.5f;
        public float[] Mj_Anim3D_RotateInDown = new float[] { 0.2f, 0.4f };          //翻开时间，延迟时间
        public float[] Mj_Anim3D_AutoDown = new float[] { 0.35f, 0.5f };              //麻将在牌墙中自动落下: 落下时间，延迟时间
        public float[] Mj_Anim3D_ChangeFlower = new float[] { 1.5f, 0, 0f };          //麻将补花 
    }

    public PartTimeConfig PartTime;

    [System.Serializable]
    public class Part2DTimeConfig
    {
        public float Mj_Anim2d_CloseWait = 1.5f;                                      //关闭场景等待时间
        public float Mj_Anim2d_AskClose = 30f;                                        //关闭窗口的等待时间 暂时如此
        public float Mj_Anim2d_PutTips = 1.0f;                                        //出牌的2D展示时间
        public float Mj_Anim2d_BaseTips = 1.5f;                                       //吃碰杠听等基本展示时间
        public float Mj_Anim2d_BaseTipsDelay = 0.6f;                                  //基本展示延迟时间
        public float[] Mj_Anim2d_HuTips = new float[] { 1.5f, 1.5f };                //展示胡牌提示的时间
        public float[] Mj_Anim2d_BuyHorse = new float[] { 1.5f, 3.0f };              //买马的展示时间 ：延迟显示结果，显示结果之后等待多久
        public float Mj_Anim2d_MaBaoAfterFly = 3.0f;                                  //抓鸟跟扎马爆开的显示时间
        public float[] Mj_Anim2d_SiJiaMaiMa = new float[] { 0.5f, 3.5f };            //四家买马结果动画事件
    }

    public Part2DTimeConfig Part2DTime;



    [System.Serializable]
    public class FxTimeConfig
    {
        public float Mj_AnimFx_Gang = 1.5f;
        public float Mj_AnimFx_Hu = 2.5f;
        public float Mj_AnimFx_Hua = 1.0f;
        public float Mj_AnimFx_Que = 1.0f;
        public float Mj_AnimFx_PengGang = 0.5f;
        public float Mj_AnimFx_MaFly = 1.0f;
    }

    public FxTimeConfig FxTime;





}
