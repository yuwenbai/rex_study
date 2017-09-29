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

    public class UIMahjongXingPaiTipsScore : MonoBehaviour
    {
        public UISprite scoreType = null;
        public UISprite scoreFu = null;
        public UISprite scoreNum = null;

        public GameObject obj_fx = null;

        public Transform transParent = null;


        public void SetTypeValue(int value)
        {
            if (scoreNum != null)
            {
                scoreNum.spriteName = CardHelper.GetStringBuiderStr("mj_icon_pao{0}", value);
            }
        }


        public void SetTypeWithScore(int typeName, int score)
        {
            string scoreTypeName = CardHelper.GetScoreTypeName(typeName);

            if (scoreType != null)
            {
                scoreType.spriteName = scoreTypeName;
            }

            SetScore(score);
        }

        string fuName = string.Empty;
        string numTitle = string.Empty;

        public void SetScore(int score)
        {
            fuName = string.Empty;
            numTitle = string.Empty;

            if (score >= 0)
            {
                fuName = "desk_score_jiafenjia";
                numTitle = "desk_score_jiafen{0}";
            }
            else
            {
                fuName = "desk_score_jianfenjian";
                numTitle = "desk_score_jianfen{0}";
            }

            if (scoreFu != null)
            {
                scoreFu.spriteName = fuName;
            }


            string scoreName = score.ToString();
            if (score >= 0)
            {
                scoreName = "+" + scoreName;
            }
            char[] strChar = scoreName.ToCharArray();

            int length = strChar.Length;
            if (strChar != null && length > 0)
            {
                for (int i = 1; i < length; i++)
                {
                    CreateNumber(i - 1, strChar[i]);
                }
            }

            if (score > 0 && obj_fx != null)
            {
                obj_fx.SetActive(true);
            }
        }


        private void CreateNumber(int index, char charNum)
        {
            GameObject obj = GameTools.InstantiatePrefab(scoreNum.gameObject, transParent, true, true);
            obj.transform.localPosition = Vector3.right * (45f * index);
            UISprite sprite = obj.GetComponent<UISprite>();

            if (sprite != null)
            {
                string spriteName = string.Format(numTitle, charNum);
                sprite.spriteName = spriteName;
            }
            obj.SetActive(true);
        }


    }

}

