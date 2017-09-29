/**
* @Author Xin.Wang
*
*
*/
using System.Collections;
using System.Collections.Generic;
using UI.ScrollViewTool;
using UnityEngine;


namespace projectQ
{
    public class CuratorListData
    {
        public MjDeskInfo deskInfo = null;
        public System.Action<int> clickBtnCall = null;
    }


    public class UICuratorListItem : ScrollViewItemBase<CuratorListData>
    {

        public UILabel Label_TableNum = null;       //桌子号
        public UILabel Label_GameType = null;       //游戏类型 
        public UILabel Label_OddMax = null;         //封顶番数

        public UILabel Label_Process = null;        //游戏进程 

        public UILabel[] Array_PlayerInfo = null;     //玩家信息列表 
        public UILabel[] Array_PlayerEmpty = null;      //玩家空位标志

        public GameObject btn_CheckBureau = null;       //查看战绩列表 

        private MjDeskInfo data = null;
        private System.Action<int> clickCall = null;

        public override void Refresh()
        {
            CuratorListData uiData = this.UIData;
            data = uiData.deskInfo;
            clickCall = uiData.clickBtnCall;

            Label_TableNum.text = CardHelper.GetGameDeskNum(data.deskID);
            Label_GameType.text = CardHelper.GetGameMessageByType(data.mjGameConfigId);
            Label_OddMax.text = CardHelper.GetGameOdd(data.maxDouble);
            Label_Process.text = CardHelper.GetGameDeskProcess(data.bouts, data.rounds);

            MjPlayerInfo[] playerInfo = MjDataManager.Instance.GetAllPlayerInfoByDeskID(data.deskID);
            if (playerInfo != null)
            {
                for (int i = 0; i < playerInfo.Length; i++)
                {
                    bool isEmpty = playerInfo[i] == null;
                    Array_PlayerInfo[i].gameObject.SetActive(!isEmpty);
                    Array_PlayerEmpty[i].gameObject.SetActive(isEmpty);

                    if (!isEmpty)
                    {
                        //Array_PlayerInfo[i].text = playerInfo[i].nickName;
                        Array_PlayerInfo[i].text = MemoryData.PlayerData.get(playerInfo[i].userID).PlayerDataBase.Name;
                    }
                }
            }
            
            SetBtnState();
        }


        private void SetBtnState()
        {
            btn_CheckBureau.SetActive(data.viewScore);
            if (data.viewScore)
            {
                UIEventListener.Get(btn_CheckBureau).onClick = OnClickBtnCheck;
            }

            var btn = btn_CheckBureau.GetComponent<UIDefinedButton>();
            if (btn != null)
            {
                btn.isEnabled = data.bouts >= 1;
            }
        }


        private void OnClickBtnCheck(GameObject obj)
        {
            if (clickCall != null)
            {
                if(data.bouts == 1)
                {
                    WindowUIManager.Instance.CreateTip("第一局未完成，无战绩记录");
                    return;
                }
                clickCall(data.deskID);
            }
        }

    }

}

