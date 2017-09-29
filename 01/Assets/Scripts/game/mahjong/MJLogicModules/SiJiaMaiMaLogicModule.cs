/**
 * @Author HaiLong.Zhang
 *
 *
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using projectQ;
using MahjongPlayType;
public class SiJiaMaiMaLogicModule : SpecialLogicModule
{
    public override void AddEvents()
    {
        EventDispatcher.AddEvent(MJEnum.MjSiJiaMaiMaEvent.MJSJMM_LogicNotify.ToString(), NotifySiJiaMaiMa);
    }


    private void NotifySiJiaMaiMa(object[] obj)
    {
        int seatID = -1;
        List<int> mjPaiList = new List<int>();
        MahjongPlayType.SiJiaMaiMaData data = MjDataManager.Instance.MjData.ProcessData.processSiJiaMaiMa.SiJiaMaiMaData;
        if (NullHelper.IsObjectIsNull(data))
        {
            return;
        }
        int type = data.selectType;
        if (type < 0)
        {
            return;
        }
        MjDataManager.Instance.SiJiaMaiMaGetData(out mjPaiList, out seatID);
        if (type == 1)
        {
            //第一阶段展示4张牌
            AnimPlayManager.Instance.PlayAnimCenter(MahjongTimeConfig.Instance.MjSystemTime.Time_NormalTime, MJEnum.MjSiJiaMaiMaEvent.MJSJMM_FirstShowUI.ToString(), mjPaiList, seatID);
        }
        else
        {
            //第二阶段展示买马动画
            if (mjPaiList != null && mjPaiList.Count > 0)
            {
                AnimPlayManager.Instance.waitSubStop = true;
                AnimPlayManager.Instance.PlayAnimCenter(MahjongTimeConfig.Instance.MjSystemTime.Mj_Anim_SiJiaMaiMaResult, MJEnum.MjSiJiaMaiMaEvent.MJSJMM_KaiMaResultAnim.ToString(), mjPaiList, seatID);
            }
        }
    }

}
