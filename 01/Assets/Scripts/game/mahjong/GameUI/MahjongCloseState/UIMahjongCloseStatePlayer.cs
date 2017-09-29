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
    public class UIMahjongCloseStatePlayer : MonoBehaviour
    {
        public UITexture text_Head = null;
        public GameObject obj_Agree = null;
        public GameObject obj_Disagree = null;
        public GameObject obj_Choosing = null;

        public void IniState(int state, string headUrl)
        {
            DebugPro.Log(DebugPro.EnumLog.HeadUrl, "UIMahjongCloseState__SetHead", headUrl);
            DownHeadTexture.Instance.WeChat_HeadTextureGet(headUrl, DownHeadCallBack);
            UpdateState(state);
        }

        private void DownHeadCallBack(Texture2D text, string headName)
        {
            if (text_Head != null)
            {
                text_Head.mainTexture = text;
            }
        }


        public void UpdateState(int state)
        {
            bool agree = false;
            bool disAgree = false;
            bool choosing = false;
            switch (state)
            {
                case 0:
                    {
                        //选择中
                        choosing = true;
                    }
                    break;
                case 1:
                    {
                        //同意
                        agree = true;
                    }
                    break;
                case 2:
                    {
                        //不同意
                        disAgree = true;
                    }
                    break;
            }

            obj_Agree.SetActive(agree);
            obj_Disagree.SetActive(disAgree);
            obj_Choosing.SetActive(choosing);
        }


    }

}


