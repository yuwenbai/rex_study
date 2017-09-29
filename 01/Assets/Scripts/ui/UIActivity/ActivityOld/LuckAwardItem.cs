/**
 * 作者：周腾
 * 作用：
 * 日期：
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LuckAwardItem : MonoBehaviour {
    [HideInInspector]
    public int awardId;
    [HideInInspector]
    public string awardName;
    [HideInInspector]
    public int awardNum;
    public UILabel awardNameLab;
    public UILabel awardNumLab;
    public UISprite itemIconSp;
    //public bool isShowSelfGuangDian;
    //private float showSelfGuangDianTimer;
    #region 光点
    //public GameObject firstShow1;
    //public GameObject firstShow2;
    //public GameObject secondShow1;
    //public GameObject secondShow2;
    //public GameObject thirdShow;
    #endregion
    /// <summary>
    /// 初始化Item
    /// </summary>
    /// <param name="awardNum"></param>
    /// <param name="awardName"></param>
    /// <param name="resUrl"></param>
    /// <param name="awardId"></param>
    public void InitAwardItem(int awardNum,string awardName,string resUrl,int awardId)
    {
        itemIconSp.spriteName = resUrl;
        awardNameLab.text = awardName;
       
        if (awardNum > 0)
        {
            awardNumLab.gameObject.SetActive(true);
            awardNumLab.text = "x" + awardNum.ToString();
        }
        else
        {
            awardNumLab.gameObject.SetActive(false);
        }
        this.awardId = awardId;
        this.awardName = awardName;
        this.awardNum = awardNum;
    }

    public void ShowSelfGuangDian()
    {
        //isShowSelfGuangDian = true;
    }

    public void HideSelfGuangDian()
    {
        //isShowSelfGuangDian = false;
    }
    private void Update()
    {
        //if (isShowSelfGuangDian)
        //{
        //    showSelfGuangDianTimer += Time.deltaTime;
        //    if (showSelfGuangDianTimer > 0 && showSelfGuangDianTimer <= 0.5)
        //    {
        //        firstShow1.SetActive(true);
        //        firstShow2.SetActive(true);
        //    }
        //    else if (showSelfGuangDianTimer > 0.5 && showSelfGuangDianTimer <= 1)
        //    {
        //        secondShow1.SetActive(true);
        //        secondShow2.SetActive(true);
        //    }
        //    else if (showSelfGuangDianTimer > 1 && showSelfGuangDianTimer <= 1.5)
        //    {
        //        thirdShow.SetActive(true);
        //    }
        //    else
        //    {
        //        firstShow1.SetActive(false);
        //        firstShow2.SetActive(false);
        //        secondShow1.SetActive(false);
        //        secondShow2.SetActive(false);
        //        thirdShow.SetActive(false);
        //        showSelfGuangDianTimer = 0;

        //    }
        //}
    }

}
