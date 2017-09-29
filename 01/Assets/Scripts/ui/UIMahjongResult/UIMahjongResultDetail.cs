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

    public class UIMahjongResultDetail : MonoBehaviour
    {
        public UILabel label_Bereau = null;
        public GameObject m_Button_PlayBack = null;
        public UIMahjongResultDetailItem[] itemArray = null;


        private int _index = -1;
        private System.Action<int> clickCall = null;
        private System.Action<int> clickPlayCall = null;

        //rexzhao
        public void IniDetailInfo(string bereau, int[] scoreList, List<int> winIndex, int zimoIndex, int paoIndex, bool isGaming, int index, int showType, System.Action<int> call, System.Action<int> playCall, bool bShowReplay)
        {
            _index = index;
            clickCall = call;
            clickPlayCall = playCall;
            UIEventListener.Get(this.gameObject).onClick = ClickItem;
           // UIEventListener.Get(this.replayBtn.gameObject).onClick = ClickReplayBtn;

            if (label_Bereau != null)
            {
                label_Bereau.text = bereau;
            }
            //没有回放记录 关闭播放按钮显示
            if (!bShowReplay)
            {
               // replayBtn.gameObject.SetActive(false);
            }
            for (int i = 0; i < scoreList.Length; i++)
            {
                bool isHu = winIndex.Contains(i);
                bool isZimo = zimoIndex == i;
                bool isPao = paoIndex == i;

                itemArray[i].IniDetailItem(scoreList[i], isHu, isZimo, isPao);
            }

            if (m_Button_PlayBack != null)
            {
                m_Button_PlayBack.gameObject.SetActive(showType == 1 && ConstDefine.ShowPlayBack && bShowReplay);// && FakeReplayManager.Instance.ReplayState
                UIEventListener.Get(m_Button_PlayBack.gameObject).onClick = ClickPlayButtion;
            }
        }

        private void ClickItem(GameObject obj)
        {
            if (clickCall != null)
            {
                clickCall(_index);
            }
        }
        private void ClickPlayButtion(GameObject obj)
        {
            if (clickPlayCall != null)
            {
                clickPlayCall(_index);
            }
        }
    }
}
