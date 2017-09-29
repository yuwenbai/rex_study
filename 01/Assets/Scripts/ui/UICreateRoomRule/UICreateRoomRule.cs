using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace projectQ
{
    public class UICreateRoomRule : UIViewBase
    {

        private UICreateRoomRuleModel Model
        {
            get
            {
                return this._model as UICreateRoomRuleModel;
            }
        }

        //Button相关
        public GameObject BtnClose;
        public GameObject BtnRoomCardShop;
        public GameObject BtnPlayRule;
        public GameObject BtnCreateRoom;

        //Text相关
        public UILabel TxtRoomCardNum;
        public UILabel TxtGamePlay;

        public UITable TabRulePanel;

        public UIToggle TogCuratorIsCanSeeResult;  //是否允许馆长查看战绩

        public UIScrollView ScrollView;


        private MahjongPlay CurrentGamePlay; //当前玩法
        private List<UICreateRoomRuleItem> RuleItemList = new List<UICreateRoomRuleItem>(); //规则列表

        public override void Init()
        {
            UIEventListener.Get(BtnClose).onClick = OnClickClose;
            UIEventListener.Get(BtnRoomCardShop).onClick = OnClickRoomCardShop;
            UIEventListener.Get(BtnPlayRule).onClick = OnClickPlayRule;
            UIEventListener.Get(BtnCreateRoom).onClick = OnClickCreateRoom;
        }

        public override void OnPushData(object[] data)
        {
            if (data != null && data.Length >= 1)
            {
                //是否可返回
                bool isNotCanBack = false;
                if (data.Length >= 2)
                {
                    isNotCanBack = (bool)data[1];
                }
                //锁定规则 不能显示返回按钮
                BtnClose.SetActive(!isNotCanBack);
                BtnCreateRoom.GetComponentInChildren<UILabel>().text = isNotCanBack ? "确定" : "开桌";

                int configID = (int)data[0];
                CurrentGamePlay = Model.PlayData.GetMahjongPlayByConfigId(configID);

                CreateRulesItem();
            }
        }

        public override void OnShow()
        {
            if (MemoryData.DeskData.TempDeskData != null && CurrentGamePlay == null)
                CurrentGamePlay = MemoryData.MahjongPlayData.GetMahjongPlayByConfigId(MemoryData.DeskData.TempDeskData.mjGameConfigId);

            TxtGamePlay.text = CurrentGamePlay.Name;

            RefreshUI();
        }

        //创建每个规则面板
        private void CreateRulesItem()
        {
            CurrentGamePlay.OptionLogic.CreateOptionImpactStructure(); //创建前置互斥结构

            Model.PlayData.Selected(CurrentGamePlay);

            int index = 0;
            UITools.CreateChild<MahjongPlayRule>(TabRulePanel.transform, null, CurrentGamePlay.RuleList, (ruleGo, ruleData) =>
            {
                var tempScript = ruleGo.GetComponent<UICreateRoomRuleItem>();
                tempScript.Init(ruleData, this);

                RuleItemList.Add(tempScript);

                Transform line = ruleGo.transform.Find("Line");
                if (line != null)
                {
                    UISprite linesp = line.GetComponent<UISprite>();
                    linesp.alpha = index == 0 ? 0 : 1;
                }
                
                index++;
            });

            TabRulePanel.Reposition();
            StartCoroutine(ResetPosition());
        }

        private IEnumerator ResetPosition()
        {
            yield return null;
            ScrollView.ResetPosition();
        }

        //刷新界面
        public void RefreshUI()
        {
            TxtRoomCardNum.text = MemoryData.PlayerData.MyPlayerModel.PlayerDataBase.TotalTicket.ToString(); //房卡数量
        }

        //刷新所有规则选项
        public void RefreshAllRuleOptions()
        {
            int count = RuleItemList.Count;
            for (int i = 0; i < count; i++)
            {
                RuleItemList[i].RefreshOptions();
            }
        }

        //打开购房卡商店 界面
        private void OnClickRoomCardShop(GameObject go)
        {
            if (MemoryData.PlayerData.MyPlayerModel.PlayerDataBase.OwnerRoomID != 0)
            {
                // 馆主
                LoadUIMain("UIRoomCardMaster");
            }
            else
            {
                //非馆主
                LoadUIMain("UIRoomCard");
            }
        }

        //打开 玩法规则说明 界面
        private void OnClickPlayRule(GameObject go)
        {
            LoadUIMain("UIRuleSmall", CurrentGamePlay.ConfigId);
        }

        //创建房间
        private void OnClickCreateRoom(GameObject go)
        {

            SelectedPlay selected = Model.PlayData.GetSelected(CurrentGamePlay);

            if (selected != null)
            {
                selected.ViewScore = TogCuratorIsCanSeeResult.value;

                MemoryData.MahjongPlayData.SavePlaySet(selected.configID, selected);

                MjDeskInfo createDesk = new MjDeskInfo(CurrentGamePlay, 0);
                createDesk.MjType = CurrentGamePlay.MjType;
                createDesk.viewScore = selected.ViewScore;
                MemoryData.DeskData.TempDeskData = createDesk;
                this.Close();
                EventDispatcher.FireSysEvent(GEnum.NamedEvent.SysUI_OpenPrepareGame);
            }
        }

        //关闭界面
        private void OnClickClose(GameObject go)
        {
            if(MemoryData.MahjongPlayData.GetMahjongPlayOnlyConfigId() == 0)
            {
                LoadUIMain("UICreateRoom");
            }
            else
            {
                EventDispatcher.FireSysEvent(GEnum.NamedEvent.SysUI_OpenMain, EnumChangeSceneType.GamePrepare_To_Main);
            }
            this.Close();
        }

        public override void OnHide()
        {

        }
    }
}
