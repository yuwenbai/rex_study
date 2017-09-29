/**
* @Author Xin.Wang
* 基本流程逻辑类
*
*/
using System.Collections;
using System.Collections.Generic;
using projectQ;


public class MjProcessBasicLogic : BaseLogicModule
{
    public override void AddEvents()
    {
        EventDispatcher.AddEvent(MJEnum.ProcessBasicEnum.PROBASIC_Roll_DatatoLogic.ToString(), PROBASIC_Roll_DatatoLogic);
        EventDispatcher.AddEvent(MJEnum.ProcessBasicEnum.PROBASIC_SPECIAL_DataToLogic.ToString(), PROBASIC_SPECIAL_DataToLogic);


        EventDispatcher.AddEvent(MJEnum.ProcessBasicEnum.PROBASIC_ANIM_PlayerTimeLimit.ToString(), PROBASIC_ANIM_PlayerTimeLimit);
    }

    /// <summary>
    /// 扔色子 
    /// </summary>
    /// <param name="obj"></param>
    private void PROBASIC_Roll_DatatoLogic(object[] obj)
    {
        int seatID = -1;
        List<int> rolls = new List<int>();
        MjDataManager.Instance.ProcesBasic_GetRollNums(out rolls, out seatID);

        if (rolls != null && rolls.Count > 0)
        {
            AnimPlayManager.Instance.PlayAnimCenter(MahjongTimeConfig.Instance.MjSystemTime.Time_Notime, MJEnum.ProcessBasicEnum.PROBASIC_Roll_LogictoView.ToString(), rolls, seatID);
        }

    }

    /// <summary>
    /// 翻开特殊牌
    /// </summary>
    /// <param name="obj"></param>
    private void PROBASIC_SPECIAL_DataToLogic(object[] obj)
    {
        MjStandingPlateData standingData = MjDataManager.Instance.GetCurOpenSpecialStanding();
        if (standingData != null)
        {
            MjDataManager.Instance.ProcessBasic_RollNums(standingData.standingRolls, standingData.standingSeatID);
            AnimPlayManager.Instance.PlayAnimCenter(MahjongTimeConfig.Instance.MjSystemTime.Mj_Anim_OpenCard, MJEnum.ProcessBasicEnum.PROBASIC_SPECIAL_LogicToView.ToString());
            AnimPlayManager.Instance.PlayAnimCenter(MahjongTimeConfig.Instance.MjSystemTime.Time_Notime, MJEnum.ProcessBasicEnum.PROBASIC_SPECIALShowCard_LogicToView.ToString(), true);
            MjDataManager.Instance.ProcessBasic_AnimPlayerTime(true, MahjongTimeConfig.Instance.MjSystemTime.Mj_Anim_OpenCard);                 //压入时间
        }
    }


    /// <summary>
    /// 个人动画空白时常压栈
    /// </summary>
    /// <param name="obj"></param>
    private void PROBASIC_ANIM_PlayerTimeLimit(object[] obj)
    {
        List<int> seatIDList = new List<int>();
        float limitTime = 0f;

        MjDataManager.Instance.ProcessBasic_GetAnimPlayerTime(out seatIDList, out limitTime);

        if (seatIDList != null && seatIDList.Count > 0)
        {
            for (int i = 0; i < seatIDList.Count; i++)
            {
                AnimPlayManager.Instance.PlayAnim(seatIDList[i], limitTime, GEnum.NameAnim.EMjAnimJiange);
            }
        }
    }


}
