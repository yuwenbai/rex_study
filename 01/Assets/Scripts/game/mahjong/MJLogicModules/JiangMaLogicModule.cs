using System.Collections;
using System.Collections.Generic;
using projectQ;
using MahjongPlayType;

public class JiangMaLogicModule : SpecialLogicModule
{
    public override void AddEvents()
    {
        EventDispatcher.AddEvent(MJEnum.MjJiangMaEvent.MJJM_ShowLogicNotify.ToString(), ShowLogicNotify);
    }

    public void ShowLogicNotify(object[] pro)
    {
        if (pro.Length < 1)
        {
            return;
        }
        AnimPlayManager.Instance.waitSubStop = true;
        int nType = (int)pro[0];

        var jmData = MjDataManager.Instance.MjData.ProcessData.processJiangMa.GetJiangMaDataByType(nType);

        if (jmData == null)
            return;

        if (jmData.GetCards != null && jmData.GetCards.Count > 0)
        {
            float time = CardHelper.GetUITotalAimTime(jmData.GetCards[0].maCode.Count, ConstDefine.OpenMaFlippingTime, ConstDefine.OpenMaFlippingIntervalTime, ConstDefine.UICloseDlay);

            AnimPlayManager.Instance.PlayAnimCenter(time, MJEnum.MjJiangMaEvent.MJJM_AnimationOne.ToString(), nType, jmData.GetCards);
        }

        if (jmData.GetScore != null && jmData.GetScore.Count > 0)
        {
            List<int> showSeatID = new List<int>();
            for (int i = 0; i < jmData.GetScore.Count; i++)
            {
                if (jmData.GetScore[i].score < 0)
                {
                    showSeatID.Add(jmData.GetScore[i].seatID);
                }
            }

            if (showSeatID.Count > 0)
            {
                AnimPlayManager.Instance.PlayAnimCenter(ConstDefine.Mj_AnimFx_MaFly, MJEnum.MjJiangMaEvent.MJJM_AnimationTwo.ToString(), nType, showSeatID);
            }
        }
    }
}
