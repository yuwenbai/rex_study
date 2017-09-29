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

    public class UIMahjongResultPlayer : MonoBehaviour
    {
        public UILabel label_Nick = null;

        public UITexture texture_Head = null;

        public GameObject obj_Score = null;
        public UIGrid grid_Score = null;

        public GameObject obj_Title = null;
        public UIGrid grid_Title = null;
        public GameObject obj_Pos = null;
        public GameObject obj_Owner = null;

        public GameObject m_Obj_Time = null;
        public UILabel m_Label_Time = null;

        public UILabel m_Label_Cost = null;
        public GameObject m_Obj_ShowCost = null;
        public GameObject m_Obj_ClickRule = null;

        private List<int> titleList = null;
        private int m_ShowType = 0;
        private GameResultCostData m_CardData = null;
        private bool m_ShowTime = false;
        private System.Action m_ClickRuleCall = null;

        public void IniPlayerInfo(string nickName, int score, List<int> title, string headUrl, bool showPos, bool isOwner, 
            int showType, GameResultCostData cardData, bool showTime, System.Action ruleCall)
        {
            if (label_Nick != null)
            {
                label_Nick.text = nickName;
            }

            if (obj_Pos != null)
            {
                obj_Pos.SetActive(showPos);
            }

            if (obj_Owner != null)
            {
                obj_Owner.SetActive(isOwner);
            }

            m_ShowType = showType;
            m_CardData = cardData;
            m_ShowTime = showTime;
            m_ClickRuleCall = ruleCall;

            //set url
            DownHeadTexture.Instance.WeChat_HeadTextureGet(headUrl, HeadCall);
            //set score
            SetScore(score);

            //set title
            title.Sort();
            titleList = title;
            IniTitle();

            if (m_Obj_ClickRule != null)
                UIEventListener.Get(m_Obj_ClickRule).onClick = OnClickRule;
        }

        private void ShowTime()
        {
            #region 废弃
            /*if (m_Obj_Time && m_CardData != null && m_ShowTime)
            {
                //m_Obj_Time.gameObject.SetActive(m_CardData.joinRoomTime > 0);

                //System.DateTime startTime = System.TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1));
                //System.DateTime dt = startTime.AddSeconds(m_CardData.joinRoomTime);
                //if (m_CardData.joinRoomTime > 0 && m_Label_Time != null)
                //    m_Label_Time.text = string.Format("入桌 {0}", dt.ToString("H:mm:ss"));//.ToLongTimeString()
            }*/
            #endregion
        }

        private void HeadCall(Texture2D texture, string headName)
        {
            texture_Head.mainTexture = texture;
        }

        string fuName = string.Empty;
        string numTitle = string.Empty;

        private void SetScore(int score)
        {
            Transform tranP = grid_Score.transform;
            tranP.DestroyChildren();

            fuName = string.Empty;
            numTitle = string.Empty;

            if (score >= 0)
            {
                fuName = "desk_icon_jiafenjian";
                numTitle = "desk_icon_jiafen{0}";
            }
            else
            {
                fuName = "desk_icon_jianfenjian";
                numTitle = "desk_icon_jianfen{0}";
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
                for (int i = 0; i < length; i++)
                {
                    if (i == 0)
                    {
                        if (score < 0)
                        {
                            CreateNumber(i, strChar[i], fuName, tranP);
                        }
                    }
                    else
                    {
                        CreateNumber(i, strChar[i], numTitle, tranP);
                    }
                }
            }

            grid_Score.Reposition();
        }

        private void CreateNumber(int index, char charNum, string formatStr, Transform tranP)
        {
            GameObject obj = GameTools.InstantiatePrefab(obj_Score, tranP, true, true);
            obj.name = index.ToString();
            UISprite sprite = obj.GetComponent<UISprite>();

            if (sprite != null)
            {
                string spriteName = string.Format(formatStr, charNum);
                sprite.spriteName = spriteName;
                sprite.MakePixelPerfect();
            }
            obj.SetActive(true);
        }


        private void IniTitle()
        {
            if (m_Obj_ClickRule.gameObject.activeSelf)
                m_Obj_ClickRule.gameObject.SetActive(false);
            m_Label_Cost.text = "";

            Transform tranP = grid_Title.transform;
            tranP.DestroyChildren();
            if (titleList != null)
            {
                for (int i = 0; i < titleList.Count; i++)
                {
                    GameObject obj = GameTools.InstantiatePrefab(obj_Title, tranP, true, true);
                    obj.name = i.ToString();
                    UISprite sprite = obj.GetComponent<UISprite>();
                    CardHelper.GetTitleByID(sprite, titleList[i]);
                    sprite.MakePixelPerfect();
                    obj.SetActive(true);
                }
                grid_Title.Reposition();
            }

            if (m_ShowType == (int)UIMahjongResult.e_ShowType.OpenInGameOver || m_ShowType == (int)UIMahjongResult.e_ShowType.OpenInWithout)
            {
                if (m_CardData != null)
                {
                    if (m_Label_Cost != null && m_CardData.costCardNum != 0)
                    {
                        m_Label_Cost.text = string.Format("桌卡-{0}", m_CardData.costCardNum);
                        if (m_Obj_ClickRule.gameObject.activeSelf != m_ShowTime)
                            m_Obj_ClickRule.gameObject.SetActive(m_ShowTime);
                    }
                }
            }

            if (titleList != null && titleList.Count > 0)
            {
                m_Obj_ShowCost.transform.localPosition = new Vector3(100, -45, 0);
            }
            else
            {
                m_Obj_ShowCost.transform.localPosition = new Vector3(100, -20, 0);
                if (!m_Obj_ClickRule.gameObject.activeSelf && m_CardData.costCardNum > 0)
                    m_Obj_ClickRule.gameObject.SetActive(true);
            }

            ShowTime();
        }

        private void OnClickRule(GameObject obj)
        {
            if(m_ClickRuleCall != null)
            {
                m_ClickRuleCall();
            }
        }
    }
}


