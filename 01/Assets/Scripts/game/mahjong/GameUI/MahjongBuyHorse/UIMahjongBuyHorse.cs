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
    public class UIMahjongBuyHorse : UIViewBase
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

        public UIMahjongBuyHorseInfo[] buyHorseInfoArray = null;

        private List<MjHorse> horseList = null;
        private int selfSeatID = -1;
        private int dealerSeatID = -1;

        private float delayTime = -1;
        private float showTime = -1;

        public override void OnPushData(object[] data)
        {
            if (data != null && data.Length > 0)
            {
                horseList = (List<MjHorse>)data[0];
                selfSeatID = (int)data[1];
                dealerSeatID = (int)data[2];

                delayTime = (float)data[3];
                showTime = (float)data[4];
            }
        }


        private void IniData()
        {
            if (horseList != null)
            {
                for (int i = 0; i < horseList.Count; i++)
                {
                    int curSeatID = horseList[i].seatID;
                    string headUrl = horseList[i].headUrl;

                    int uiSeat = CardHelper.GetMJUIPosByServerPos(curSeatID, selfSeatID);
                    buyHorseInfoArray[uiSeat].IniData(uiSeat == 0, headUrl, curSeatID == dealerSeatID, horseList[i].maCode, horseList[i].horseType);
                }
                EventDispatcher.FireEvent(MJEnum.MusicEnum.ME_PlaySoundFX.ToString(), GEnum.SoundEnum.desk_kaima);
            }

            StartCoroutine(IEShowResult());
        }


        private IEnumerator IEShowResult()
        {
            yield return new WaitForSeconds(delayTime);
            if (horseList != null && buyHorseInfoArray != null)
            {
                for (int i = 0; i < buyHorseInfoArray.Length; i++)
                {
                    buyHorseInfoArray[i].ShowMessage();
                }
                yield return new WaitForSeconds(showTime);
            }
            Close();
        }

    }

}
