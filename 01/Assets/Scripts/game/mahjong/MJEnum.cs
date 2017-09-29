/**
 * @Author JEFF
 *
 *
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MJEnum
{
    #region 音乐相关
    public enum MusicEnum
    {
        ME_PlaySoundFX, //播放麻将音效
        ME_ActionFx,//碰杠听等行为音效
        ME_PutCard
    }
    #endregion

    #region 手部模型动画事件
    public enum HandModelEvent
    {
        //=====================手动画事件相关
        /// <summary>
        /// 动画手模型初始化
        /// </summary>
        HME_Init,
        /// <summary>
        /// 动画手模型销毁
        /// </summary>
        HME_Destroy,
        /// <summary>
        /// 调用动画播放
        /// </summary>
        HME_Play,
        /// <summary>
        /// 动画事件发出
        /// </summary>
        HME_Event,
    }
    #endregion

    #region 麻将功能模块

    #region Process 模块
    public enum ProcessBasicEnum
    {
        PROBASIC_Roll_DatatoLogic,                  //扔色子(Data -> logic
        PROBASIC_Roll_LogictoView,                  //扔色子(Logic -> View

        PROBASIC_SPECIAL_DataToLogic,               //翻开特殊牌（data ->logic
        PROBASIC_SPECIAL_LogicToView,               //翻开特殊牌（logic ->view
        PROBASIC_SPECIALShowCard_LogicToView,               //UI展示特殊牌（logic ->view


        PROBASIC_ANIM_PlayerTimeLimit,              //动画基本流程，玩家时间阻塞
    }


    #endregion

    #region PlayOprate 模块

    public enum GangHouNaChi
    {
        GHNC_LogicNotify,
        GHNC_LogicUploadResult,               //上传拿吃结果
        GHNC_LogicResponseResult,           //服务器消息返回
        GHNC_OpenUINaChi,            //打开ui
        GHNC_CloseUINaChi,        //关闭ui
        GHNC_OpenUINaChiChoose,            //打开ui
        GHNC_CloseUINaChiChoose,        //关闭ui
        GHNC_OpenUINaChiTips,            //打开其他玩家提示信息
        GHNC_CloseUINaChiTips,          //关闭其他玩家提示
        GHNC_ClearUI,
    }

    public enum DuAnGangEvents
    {
        DAG_LogicNotify,         //服务器通知消息
        DAG_ClearUIAlert,         //清楚提示信息
        DAG_UIAlert,          //其他玩家提示
        DAG_RspNotify,         //服务器消息返回
        DAG_OpenUIDuAnGang,    //显示UI
        DAG_CloseUIDuAnGang,      //关闭
        DAG_UIDuAnGangShowResult, //显示赌暗杠的结果
        DAG_UIAlertTipResult,       //其他玩家结果提示
        DAG_UIAlertTip,       //其他玩家提示赌暗杠
        DAG_LogicUploadData
    }

    public enum BankerPutAlert
    {
        BPA_BankerFirstTurn,//庄家第一次出牌提示
        BPA_OpenUIAlert,//出牌提示ui
        BPA_CloseUIAlert,//出牌提示ui
        BPA_BankerTurnOver
    }

    public enum MingDaEvents
    {
        MD_LogicNotify,             //服务器通知消息
        MD_RspNotify,               //玩家信息更新
        MD_ShowUI,                  //显示UI
        MD_CloseUI,                 //关闭UI
        MD_ShowRefresh,             //显示刷新
        MD_ShowResult,              //显示结果
        //MD_ClearUI,                 //清除显示相关
        //MD_ShowOtherPlayer,         //其他玩家信息显示
        MD_ClickBtn,                //点击选择
        MD_LogicUploadData,         //
    }

    public enum MjXingpaiChoose
    {
        XPC_EventClose,             //事件关闭
        XPC_AnimShowTingTip,        //动画开启听口
        XPC_AnimShow,               //动画开启显示

        XPC_EventShowMao,           //事件开启放毛
        XPC_AnimShowMao,            //动画开启放毛显示
    }

    /// <summary>
    /// 亮一
    /// </summary>
    public enum MjLiangyiEvent
    {
        MJLY_LogicShowUI,           //逻辑 显示UI
        MJLY_LogicUpdateUI,         //逻辑 更新选择状态
        MJLY_UIChangeState,         //UI   改变UI的状态
        MJLY_LogicSendData,         //UI  发送数据

        MJLY_UIShowUI,              //显示UI

        MJLY_AnimShowUI,            //动画 显示UI
        MJLY_AnimUpdateUI,          //动画 更新UI
    }
    /// <summary>
    /// 亮四打一 
    /// </summary>
    public enum MjLiangSiDaYiEvent
    {
        LSDY_UIShow3D,              //重连初始化3D牌
    }

    //胡漂 跟漂
    public enum MjHuPiaoGenPiaoEvent
    {
        HPGP_DataToLogicShowEffect,           //Data->逻辑 显示效果

        HPGP_LogicToUIEvent,            //逻辑->UI 事件    
        HPGP_LogicToUIAnim,             //逻辑->UI 动画

        HPGP_UIToLogicSendData,         //UI->逻辑 发送数据
    }

    public enum MjLiangGangTouEvent
    {
        MJLGT_LogicNotify,                      //收到数据
        MJLGT_ShowUi,                           //打开UI
        MJLGT_CloseUI,                          //关闭UI
    }

    public enum MjDuanMenEvent
    {
        MjDM_LogicNotify,                   //收到数据
        MjDM_ShowUI,                        //打开刷新UI
    }

    public enum MjOpenSepcial
    {
        OPENSPECIAL_DataToLogic,            // Data->Logic 显示
    }
    public enum MjJiangMaEvent
    {
        MJJM_ShowLogicNotify,                       //收到显示信息
        MJJM_LogicNotify,                           //收到数据
        MJJM_ShowUI,                                //显示UI
        MJJM_ShowUIResult,                          //显示UI结果
        MJJM_AnimationOne,                          //动画一段
        MJJM_AnimationTwo,                          //动画二段

    }
    #region 四家买马
    public enum MjSiJiaMaiMaEvent
    {
        MJSJMM_LogicNotify,                //服务器通知消息
        MJSJMM_FirstShowUI,                //第一阶段拿4张牌
        MJSJMM_KaiMaResultAnim             //开马结果动画
    }
    #endregion

    #region 风圈
    public enum MjFengQuanEvent
    {
        MJFQ_LogicNotify,                //服务器通知消息
        MJFQ_ShowUI,                     //UI表现入栈
        MJFQ_ClearUI
    }
    #endregion


    #region 扭牌(青岛)
    public enum NiuPaiEvent
    {
        NPE_LogicNiuPaiNotify,//逻辑通知消息
        NPE_LogicBuNiuNotify,//补扭通知
        NPE_LogicRepNiuPai,//结果通知
        NPE_LogicRepBuNiu,//补扭结果通知
        NPE_ShowNiuPaiUI,
        NPE_ShowBuNiuPaiUI,
        NPE_CloseNiuPaiUI,
        NPE_CloseBuNiuPaiUI,
        NPE_UILogicNiu,
        NPE_LogicResponse,//消息返回
        NPE_LogicReconnect,//重连消息
        NPE_UINiuPaiReconnect,//重连消息
        NPE_UINiuPaiShowFx,
        NPE_NiuPaiAnim,//动画
    }
    #endregion


    #endregion

    #endregion




}
