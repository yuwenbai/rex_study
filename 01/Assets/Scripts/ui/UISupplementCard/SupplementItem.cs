/**
 * @Author 周腾
 *
 *
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace projectQ
{
    public class SupplementItem : MonoBehaviour
    {
        public GameObject obj_Zhuang;
        public UITexture playerIcon;
        public UILabel playerName;

        public UIGrid grid;
        private Transform _gridTrans = null;
        private Transform GridTrans
        {
            get
            {
                if (_gridTrans == null)
                {
                    _gridTrans = grid.transform;
                }
                return _gridTrans;
            }
        }

        public GameObject cardItem;


        public void InitSupplementItem(string headUrl, string nickName, List<int> cardIDList)
        {
            DebugPro.Log(DebugPro.EnumLog.HeadUrl, "SupplementItem__InitSupplementItem", headUrl);
            //头像
            DownHeadTexture.Instance.WeChat_HeadTextureGet(headUrl, HeadTexCallBack);

            if (playerName != null)
            {  //昵称
                playerName.text = nickName;
            }

            if (cardIDList == null || cardIDList.Count <= 0)
            {
                DebugPro.DebugInfo("cardIDList is null nickName:", nickName);
                return;
            }
            if (GridTrans == null || cardItem ==null)
            {
                DebugPro.LogError("GridTrans  GameObject is null or cardItem is null");
                return;
            }
            for (int i = 0; i < cardIDList.Count; i++)
            {
                SupplementCard itemControl = GameTools.InstantiatePrefabAndReturnComp<SupplementCard>(cardItem, GridTrans, true, true);
                if (itemControl != null)
                {
                    itemControl.InitCard(cardIDList[i]);
                    if (itemControl.selfObj != null)
                    {
                        itemControl.selfObj.SetActive(true);
                    }
                    else
                    {
                        DebugPro.LogError("SupplementCard selfObj  is null");
                    }
                }
                else
                {
                    DebugPro.LogError("SupplementCard instance  is null");
                }
            }
            if (grid != null)
            {
                grid.Reposition();
            }
            else
            {
                DebugPro.LogError("grid GameObject  is null");
            }


        }
        /// <summary>
        /// 头像回调
        /// </summary>
        /// <param name="HeadTexture"></param>
        void HeadTexCallBack(Texture2D HeadTexture, string headName)
        {
            if (HeadTexture == null)
            {
                DebugPro.LogError("HeadTexCallBack error:download HeadTexture is null");
                return;
            }
            if (playerIcon == null)
            {
                DebugPro.LogError("playerIcon gameObject is null");
                return;
            }
            playerIcon.mainTexture = HeadTexture;
        }

        public void SetZhuangActive()
        {
            if (obj_Zhuang != null)
            {
                obj_Zhuang.SetActive(true);
            }
            else
            {
                DebugPro.LogError("obj_Zhuang gameObject is null");
            }
        }

        public void ClearInfo()
        {
            if (obj_Zhuang != null)
            {
                obj_Zhuang.SetActive(false);
            }
            else
            {
                DebugPro.LogError("obj_Zhuang gameObject is null");
            }
            if (GridTrans != null)
            {
                GridTrans.DestroyChildren();
            }
            else
            {
                DebugPro.LogError("GridTrans gameObject is null");
            }
        }
    }

}
