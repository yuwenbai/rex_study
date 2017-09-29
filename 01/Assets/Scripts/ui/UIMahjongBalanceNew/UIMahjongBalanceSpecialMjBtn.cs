using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using projectQ;

public class UIMahjongBalanceSpecialMjBtn : MonoBehaviour
{
    private enum e_ShowPostionType
    {
        Left = 0,
        Right,
    }

    private const int m_LimitCount = 4;
    private UIMahjongBalanceNew m_UI;

    private List<MjBalanceNew.MjHorseInfo.BuyHorseItem> m_List_ShowMjs;
    private e_ShowType m_ShowType;

    private e_ShowPostionType m_ShowPostion;
    private EnumMjOpenMaType m_GameType;

    public GameObject m_Item_Mj;
    public UIGrid m_Grid_Mj;
    public UILabel m_Label_Title;
    public UILabel m_Label_WinNum;
    public GameObject m_Panel_MoreMark;
    public UISprite m_Sprite_Bg;
    public GameObject m_Obj_Click;

    public void InitData(UIMahjongBalanceNew ui)
    {
        m_UI = ui;
    }

    private enum e_ShowType
    {
        Normal,
        Special,
    }

    public void InitSetData(object[]  objs)
    {
        if (objs == null || objs.Length < 2)
        {
            Close();
            return;
        }

        if (m_List_ShowMjs == null)
            m_List_ShowMjs = new List<MjBalanceNew.MjHorseInfo.BuyHorseItem>();
        List<MjBalanceNew.MjHorseInfo.BuyHorseItem> mjList = (List<MjBalanceNew.MjHorseInfo.BuyHorseItem>)objs[0];
        m_GameType = (EnumMjOpenMaType)(int)objs[1];

        int showPos = 0;
        switch (m_GameType)
        {
            case EnumMjOpenMaType.ZhuaNiao:
                break;
            case EnumMjOpenMaType.ZhuaMa:
                showPos = 0;
                break;
            case EnumMjOpenMaType.JiangMa:
            case EnumMjOpenMaType.BaoZhaMa:
                showPos = 1;
                break;
            default:
                break;
        }
        m_ShowPostion = (e_ShowPostionType)showPos;

        if (mjList == null || mjList.Count < 1 || mjList[0].cardIDList == null || mjList[0].cardIDList.Count < 1)
        {
            Close();
            return;
        }

        m_List_ShowMjs = mjList;

        ShowMj();
    }

    private List<int> m_ShowMjs;

    private void ShowMj()
    {
        List<int> showmjs = new List<int>();
        for (int i = 0; i < m_List_ShowMjs[0].horseStateList.Count; i++)
        {
            if (m_List_ShowMjs[0].horseStateList[i] == (int)EnumMjBuyhorseStateType.BuyHorseWin || m_List_ShowMjs[0].horseStateList[i] == (int)EnumMjBuyhorseStateType.BuyHorseLose)
                showmjs.Add(i);
        }

        if (showmjs.Count < 1)
        {
            Close();
            return;
        }
        m_ShowMjs = showmjs;

        int count = 0;
        if (showmjs.Count <= m_LimitCount)
        {
            count = showmjs.Count;
            m_ShowType = e_ShowType.Normal;
        }
        else
        {
            count = m_LimitCount;
            m_ShowType = e_ShowType.Special;
        }

        //显示标题（奖马、爆炸马）
        string titName = MjDataManager.Instance.GetJiangMaNameByType(m_GameType);

        m_Label_Title.text = string.Format("{0}：", titName);

        if(m_Label_WinNum != null)
        {
            string showRes = "";
            if(m_List_ShowMjs[0].cardIDList.Count > 0)
            {
                showRes = string.Format("{0}x{1}   ", titName, m_List_ShowMjs[0].cardIDList.Count);
            }
            if(showmjs.Count > 0)
            {
                showRes = string.Format("{0}买中x{1}", showRes, showmjs.Count);
            }
            
            m_Label_WinNum.text = showRes;
        }

        bool isShowMore = m_ShowType == e_ShowType.Special;
        if (m_Panel_MoreMark.activeSelf != isShowMore)
            m_Panel_MoreMark.SetActive(isShowMore);

        for (int i = 0; i < count; i++)
        {
            GameObject addObj = NGUITools.AddChild(m_Grid_Mj.gameObject, m_Item_Mj);
            //显示麻将牌

            float al = 0;
            int index = showmjs[i];
            switch (m_List_ShowMjs[0].horseStateList[index])
            {
                case (int)EnumMjBuyhorseStateType.BuyHorseNull:
                    al = 0.75f;
                    break;
                case (int)EnumMjBuyhorseStateType.BuyHorseWin:
                    al = 1;
                    break;
                case (int)EnumMjBuyhorseStateType.BuyHorseLose:
                    al = 0.5f;
                    break;
            }
            UISprite mjSp = addObj.GetComponent<UISprite>();
            int showId = m_List_ShowMjs[0].cardIDList[index];// m_ShowType == e_ShowType.Normal ? showmjs[i] : m_List_ShowMjs[0].cardIDList[i];
            CardHelper.SetRecordUI(mjSp, showId, true, al);

            UISprite mjBg = addObj.transform.Find("HgihtLight").GetComponent<UISprite>();
            if (m_List_ShowMjs[0].horseStateList[index] == (int)EnumMjBuyhorseStateType.BuyHorseWin)
            {
                mjBg.spriteName = "desk_bj_kaimahuangguang";
                mjBg.gameObject.SetActive(true);
            }
            else if (m_List_ShowMjs[0].horseStateList[index] == (int)EnumMjBuyhorseStateType.BuyHorseLose)
            {
                mjBg.spriteName = "desk_bj_kaimalanguang";
                mjBg.gameObject.SetActive(true);
            }
        }

        ChangPostion(count);
    }

    private void ChangPostion(int count)
    {
        float gridWith = count > 1 ? m_Grid_Mj.cellWidth * (count - 1) : 0;
        int startWidth = 0;
        int pos = 0;
        switch (m_ShowType)
        {
            case e_ShowType.Normal:
                pos = -20;
                startWidth = pos;
                break;
            case e_ShowType.Special:
                pos = -132;
                startWidth = pos;

                UIEventListener.Get(m_Obj_Click).onClick = OnClickBtn;
                break;
        }
        m_Grid_Mj.transform.localPosition = new Vector3(pos, 0, 0);
        m_Label_Title.transform.localPosition = new Vector3(pos - Mathf.Abs(gridWith) - 6, 0, 0);

        m_Sprite_Bg.width = (int)(Mathf.Abs(startWidth) + m_Grid_Mj.cellWidth * count + m_Label_Title.width);
        m_Sprite_Bg.transform.localPosition = new Vector3(-m_Sprite_Bg.width / 2 + 20, 0, 0);

        if(m_ShowPostion == e_ShowPostionType.Right)
        {
            m_Label_WinNum.transform.localPosition = new Vector3(-m_Label_Title.transform.localPosition.x, 39, 0);
        }
        else
        {
            m_Label_WinNum.transform.localPosition = new Vector3(-(m_Label_Title.width - m_Label_WinNum.width), 39, 0);
        }

        switch (m_ShowPostion)
        {
            case e_ShowPostionType.Left:
                this.transform.localPosition = new Vector3(m_Sprite_Bg.width, 0, 0);
                break;
            case e_ShowPostionType.Right:
                this.transform.localPosition = Vector3.zero;
                break;
        }
    }

    private void Close()
    {
        Destroy(this.gameObject);
    }

    private void OnClickBtn(GameObject obj)
    {
        if(m_UI != null)
            m_UI.LoadUIMain("UIShowAllMa", m_List_ShowMjs, m_ShowMjs);
    }
}
