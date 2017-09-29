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
    public class UIMahjongResultDetailItem : MonoBehaviour
    {
        public UILabel label_score = null;
        public UILabel label_scoreDown = null;

        public GameObject obj_hu = null;
        public GameObject obj_Zimo = null;
        public GameObject obj_Pao = null;


        public void IniDetailItem(int score, bool isHu, bool isZimo, bool isPao)
        {
            bool isAdd = score > 0;

            label_score.gameObject.SetActive(isAdd);
            label_scoreDown.gameObject.SetActive(!isAdd);
            string scoreStr = CardHelper.GetUIScore(score);

            if (isAdd)
            {
                label_score.text = scoreStr;
            }
            else
            {
                label_scoreDown.text = scoreStr;
            }

            if (isZimo)
            {
                isHu = false;
                isPao = false;
            }

            obj_hu.SetActive(isHu);
            obj_Zimo.SetActive(isZimo);
            obj_Pao.SetActive(isPao);

        }


    }


}
