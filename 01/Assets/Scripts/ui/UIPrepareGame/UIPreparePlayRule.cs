/**
 * @Author YQC
 *
 * 玩法规则展示
 */

using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace projectQ
{
    public class UIPreparePlayRule : MonoBehaviour {
        public UITable RuleRoot;

        //中间数据==================
        [Tooltip("房间名称")]
        public UILabel LabelRoomName;
        [Tooltip("房间号")]
        public UILabel LabelRoomNumber;
        [Tooltip("房间说明"),Obsolete,HideInInspector]
        public UILabel LabelRoomMemo;
        [Tooltip("玩法名称")]
        public UILabel LabelPlayName;
        [Tooltip("ScrollView")]
        public UIScrollView ScrollView;

        private Action<GameObject, MahjongPlayRule> onRefreshRuleItem;
        private UIPrepareGameData uiData;

        //刷新UI
        public void RefreshUI(UIPrepareGameData data)
        {
            uiData = data;
            if (uiData == null) return;

            //房间名称
            if (uiData.DeskOwnerPlayer == null)
            {
                if(uiData.DeskInfo != null && uiData.DeskInfo.deskID == 0)
                {
                    LabelRoomName.text = MemoryData.PlayerData.MyPlayerModel.PlayerDataBase.Name + " 的棋牌室";
                }
                else
                {
                    LabelRoomName.text = "";
                }
            }
            else
            {
                string nikeName = uiData.DeskOwnerPlayer.nickName;
                if (string.IsNullOrEmpty(nikeName))
                {
                    nikeName = "游客" + uiData.DeskOwnerPlayer.userID.ToString().Substring(0, 2);
                }
                LabelRoomName.text = nikeName + " 的房间";
            }


            //房间号 2017/06/19 加删N遍所以就仅注释掉 else 内容。如果加回来解除注释就好
            if(uiData.DeskInfo.deskID == 0)
            {
                LabelRoomNumber.gameObject.SetActive(false);
            }
            else
            {
                LabelRoomNumber.gameObject.SetActive(true);
                LabelRoomNumber.text = "桌号：" + uiData.DeskInfo.deskID;
            }
            //LabelRoomMemo.text = "房间说明：" + uiData.DeskInfo.DeskAdvert;

            this.RefreshPlay();
        }

        public void RefreshScrollViewPos()
        {
            StartCoroutine(UITools.ResetScrollView(this.ScrollView));
        }

        //刷新玩法
        private void RefreshPlay()
        {
            int configId = (int)uiData.DeskInfo.mjGameConfigId;
            //麻将玩法
            var mjPlay = MemoryData.MahjongPlayData.GetMahjongPlayByConfigId(configId);
            LabelPlayName.text = mjPlay.Name;
            UITools.CreateChild<MahjongPlayRule>(RuleRoot.transform, null, mjPlay.RuleList, onRefreshRuleItem);
            RuleRoot.Reposition();
            StartCoroutine(UITools.ResetScrollView(this.ScrollView));
        }

        //刷新规则
        private void RefreshRuleItem(GameObject go, MahjongPlayRule data)
        {
            StringBuilder sb = new StringBuilder();

            UILabel labelRule = go.transform.FindChild("LabelRuleName").GetComponent<UILabel>();
            UILabel labelOption = go.transform.FindChild("LabelOption").GetComponent<UILabel>();

            //规则名称
            sb.Append(data.Name).Append("：");
            labelRule.text = sb.ToString();

            sb.Length = 0;
            //List<int> optionIdList = uiData.DeskInfo.mjRulerList;
            var optionList = data.GetOptionByRulersStr(uiData.DeskInfo.mjRulerSelected, data.id);
            for (int i = 0; i < optionList.Count; i++)
            {
                if (i > 0)
                    sb.Append("、");

                sb.Append(optionList[i]);
            }
            labelOption.text = sb.Length == 0 ? "无" : sb.ToString();
        }


        #region 生命周期
        private void Awake()
        {
            this.onRefreshRuleItem = RefreshRuleItem;
        }
        #endregion
    }
}
