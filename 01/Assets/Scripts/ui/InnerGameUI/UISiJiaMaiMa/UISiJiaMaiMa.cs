/**
 * @Author HaiLong.Zhang
 *
 *
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using projectQ;
public class UISiJiaMaiMa : UIViewBase
{

    #region 重写基类方法
    public override void Init()
    {

    }
    public override void OnShow()
    {
        InitData();
    }
    public override void OnHide()
    {

    }

    public UISiJiaMaiMaInfo[] siJiaMaiMaInfoArray = null;
    private List<MahjongPlayType.SiJiaMaiMaCellData> horseList = null;
    private int selfSeatID = -1;
    private int dealerSeatID = -1;

    private float delayTime = -1;
    private float showTime = -1;


    public override void OnPushData(object[] data)
    {
        if (data != null && data.Length > 0)
        {
            horseList = (List<MahjongPlayType.SiJiaMaiMaCellData>)data[0];
            selfSeatID = (int)data[1];
            dealerSeatID = (int)data[2];

            delayTime = (float)data[3];
            showTime = (float)data[4];
        }
    }
    #endregion

    private int firstTime = 0;
    private int seconedTime = 0;

    private void InitData()
    {
        if (horseList != null)
        {
            for (int i = 0; i < horseList.Count; i++)
            {
                int curSeatID = horseList[i].seatID;
                string headUrl = horseList[i].headUrl;
                int buyInSeatID = CardHelper.GetMJUIPosByServerPos(curSeatID, selfSeatID);
                siJiaMaiMaInfoArray[buyInSeatID].IniDataBySeatID(buyInSeatID == 0, headUrl, curSeatID == dealerSeatID, horseList[i].mjCode, horseList[i].buyInType);
            }
            EventDispatcher.FireEvent(MJEnum.MusicEnum.ME_PlaySoundFX.ToString(), GEnum.SoundEnum.desk_kaima);
        }

        //不再使用协程，而由TimeTick代替
        //TimeTick.Instance.SetAction(ShowResult, delayTime, 1);
        //TimeTick.Instance.SetAction(ShowDelayResult, showTime + delayTime, 1);
        StartCoroutine("ShowResult");
    }


    IEnumerator ShowResult()
    {
        yield return new WaitForSeconds(delayTime);
        if (horseList != null && siJiaMaiMaInfoArray != null)
        {
            for (int i = 0; i < siJiaMaiMaInfoArray.Length; i++)
            {
                siJiaMaiMaInfoArray[i].ShowMessage();
            }
            yield return new WaitForSeconds(showTime);
           
        }
        Close();
    }

    //private IEnumerator IECloseUI()
    //{
    //    yield return new WaitForSeconds(showTime);
    //    ShowDelayResult();
    //}
    //private void ShowDelayResult()
    //{
    //    Close();
    //}
}
