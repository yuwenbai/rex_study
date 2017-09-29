/**
 * @Author lyb
 *
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace projectQ
{
    public class UIMahjongResultBestModel : UIModelBase
    {
        public UIMahjongResultBest UI
        {
            get { return this._ui as UIMahjongResultBest; }
        }

        #region override------------------------------------------------------------------

        protected override GEnum.NamedEvent[] FocusNetWorkData()
        {
            return new GEnum.NamedEvent[] { };
        }

        protected override void OnNetWorkDataCallBack(GEnum.NamedEvent msgEnum, object[] data)
        {
        }

        #endregion------------------------------------------------------------------------

        #region 实现方法------------------------------------------------------------------

        /// <summary>
        /// 初始化面板数据
        /// </summary>
        public void ResultBestInit()
        {
            //UI.BoutsMax.text = string.Format("{0}番", UI.MjRecordBest.oddsCount);

            int index = 0;
            foreach (int score in UI.MjRecordBest.scoreChange)
            {
                if (score > 0)
                {
                    UI.ScoreNum.text = string.Format("+{0}", score);
                    UI.PlayerName.text = UI.MjRecordBest.playerNameList[index];
                }

                index++;
            }
            
            int checkSeatID = UI.MjRecordBest.selfSeatID - 1;
            if (checkSeatID < UI.MjRecordBest.scoreChange.Count && checkSeatID >= 0)
            {
                if (UI.MjRecordBest.scoreChange[checkSeatID] > 0)
                {
                    UI.ScoreOwnLab.text = "";
                    UI.ScoreOwnNum.text = "";
                }
                else
                {
                    UI.ScoreOwnLab.text = "我";
                    UI.ScoreOwnNum.text = UI.MjRecordBest.scoreChange[checkSeatID].ToString();
                }                
            }
        }

        #endregion------------------------------------------------------------------------
    }
}
