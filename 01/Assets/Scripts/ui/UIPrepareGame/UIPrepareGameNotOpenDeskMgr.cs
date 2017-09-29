/**
 * @Author YQC
 *
 *
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace projectQ
{
    public class UIPrepareGameNotOpenDeskMgr : MonoBehaviour
    {
        [Tooltip("规则说明Script")]
        public UIPreparePlayRule PlayRuleScript;

        [Tooltip("按钮组")]
        public UIGrid ButtonGrid;
        private List<GameObject> ButtonList;

        private UIPrepareGame ui;
        private UIPrepareGameData uiData;

        public GameObject Mask;

        private bool IsRoomMainPrepare = false;
        private bool IsShow = false;

        public void RefreshUI()
        {
            Mask.SetActive(false);
            uiData = ui.Model.data;
            PlayRuleScript.RefreshUI(uiData);
            bool flag = uiData.DeskInfo.deskID == 0 || (uiData.MyPlayer != null && uiData.MyPlayer.seatID == 1);
            RefreshButton(flag ? 0 : 1);

            if (ui.Model.data.MyPlayer != null && ui.Model.data.MyPlayer.seatID == 1 && !IsRoomMainPrepare)
            {
                MemoryData.PlayerData.MyPlayerModel.PlayerDataBase.OwnerDeskID = ui.Model.data.DeskInfo.deskID;
                IsRoomMainPrepare = true;
                ui.Model.OnPrepareGame();
            }
        }

        public void Show()
        {
            IsShow = true;
            gameObject.SetActive(true);
            Mask.SetActive(true);
            PlayRuleScript.RefreshScrollViewPos();
            RefreshButton(2);
        }
        #region Event
        /// <summary>
        /// 刷新按钮
        /// </summary>
        /// <param name="isDeskOwner">我是否是桌主</param>
        private void RefreshButton(int index)
        {
            ButtonGrid.gameObject.SetActive(true);
            ButtonList[0].SetActive(false);
            ButtonList[1].SetActive(false);
            if (index == 0)
            {
                if (ui.CurrState == UIPrepareGame.UIState.NotOpenDesk)
                {
                    ButtonList[0].SetActive(true);
                    ButtonList[1].SetActive(true);
                }
            }
            else if(index == 1)
            {
                if(uiData.MyPlayer != null)
                {
                    ButtonList[1].GetComponentInChildren<UILabel>().text = uiData.MyPlayer.hasReady ? "已准备" : "准备";
                    ButtonList[1].GetComponent<BoxCollider>().enabled = !uiData.MyPlayer.hasReady;

                    ButtonList[1].SetActive(true);
                    ButtonList[0].SetActive(false);
                }
            }
            else if(index == 2)
            {
                ButtonList[0].GetComponentInChildren<UILabel>().text = "关闭";
                ButtonList[0].GetComponent<BoxCollider>().enabled = true;

                ButtonList[0].SetActive(true);
                ButtonList[1].SetActive(false);
            }
            ButtonGrid.Reposition();
        }

        //按钮点击
        private void OnButtonPrepareClick(GameObject go)
        {
            int index = ButtonList.IndexOf(go);
            if(IsShow)
            {
                gameObject.SetActive(false);
                return;
            }

            //重置规则
            if (index == 0)
            {
                ui.LoadUIMain("UICreateRoomRule", ui.Model.data.DeskInfo.mjGameConfigId,true);
                ui.Hide();
            }
            else//确认规则
            {
                if(uiData.DeskInfo.deskID == 0)
                {
                    int roomId = MemoryData.PlayerData.MyPlayerModel.PlayerDataBase.OwnerRoomID;
                    if (roomId <= 0)
                        roomId = MemoryData.GameStateData.CurrMjRoomId;
                    //就是创建牌桌
                    ModelNetWorker.Instance.MjNewDeskReq(uiData.DeskInfo, roomId);
                }
                else
                {
                    ui.Model.OnPrepareGame();
                }
            }
        }
        #endregion

        public void Init(UIPrepareGame ui)
        {
            IsRoomMainPrepare = false;
            this.ui = ui;
        }

        private void Awake()
        {
            //按钮List
            ButtonList = new List<GameObject>();
            for (int i = 0; i < ButtonGrid.transform.childCount; i++)
            {
                Transform tf = ButtonGrid.transform.GetChild(i);
                ButtonList.Add(tf.gameObject);
                UIEventListener.Get(tf.gameObject).onClick = OnButtonPrepareClick;
            }
            ButtonGrid.gameObject.SetActive(false);
        }
    }
}