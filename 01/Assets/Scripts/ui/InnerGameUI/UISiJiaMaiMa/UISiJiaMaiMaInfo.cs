/**
 * @Author Hailong.Zhang
 *
 *
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace projectQ
{

    public class UISiJiaMaiMaInfo : UIMahjongBuyHorseBaseInfo
    {
        public void IniDataBySeatID(bool isSelf, string headUrl, bool isZhuang, List<int> mjCode, List<EnumMjBuyhorseStateType> horseType)
        {
            mjCodeList = mjCode;
            horseTypeList = horseType;
            this.isSelf = isSelf;
            DownHeadTexture.Instance.WeChat_HeadTextureGet(headUrl, DownHeadCall);
            obj_Zhuang.SetActive(isZhuang);

            if(mjCode!=null&&mjCode.Count>0)
            {
                UITools.CreateChild(grid_Pai.transform,obj_Pai,mjCode.Count,CreateDetailCall,true);
                grid_Pai.Reposition();
            }
        }

        private void CreateDetailCall(GameObject obj, int index)
        {
            UIMahjongBuyHorsePai paiItem = obj.GetComponent<UIMahjongBuyHorsePai>();
            if (paiItem != null)
            {
                paiItem.IniData(mjCodeList[index], horseTypeList[index], isSelf,false);
                paiList.Add(paiItem);
            }
        }
    }

}
