/**
 * 作者：周腾
 * 作用：
 * 日期：
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;
using System;

namespace projectQ
{
    public class UIDeskRule : UIViewBase
    {
        public UIPreparePlayRule Script;

        public GameObject closeBtn;
        private Action<GameObject, MahjongPlayRule> onRefreshRuleItem;
        public UIPrepareGameData data;

        public override void OnPushData(object[] data)
        {
        }

        public override void Init()
        {
            onRefreshRuleItem = RefreshRuleItem;
            UIEventListener.Get(closeBtn).onClick = OnCloseClick;
        }

        private void OnCloseClick(GameObject go)
        {
            this.Hide();
        }

        private void RefreshRuleItem(GameObject go, MahjongPlayRule data)
        {
            UILabel labelRule = go.transform.FindChild("LabelRuleName").GetComponent<UILabel>();
            UILabel labelOption = go.transform.FindChild("LabelOption").GetComponent<UILabel>();
            labelRule.text = data.Name + "：";

            StringBuilder sb = new StringBuilder();
            //List<int> optionList = this.data.DeskInfo.mjRulerList;
            //var optionMap = data.GetAllOption();
            //for (int i = 0; i < optionList.Count; i++)
            //{
            //    int optionId = (int)optionList[i];
            //    if (optionMap.ContainsKey(optionId))
            //    {
            //        if (sb.Length > 0)
            //            sb.Append("\r\n");
            //        sb.Append(optionMap[optionId].Name);
            //    }
            //}
            labelOption.text = sb.ToString();
        }
        public void RefreshUI()
        {
            data = new UIPrepareGameData();
            data.DeskInfo = MemoryData.DeskData.GetOneDeskInfo(MjDataManager.Instance.MjData.curUserData.selfDeskID);
            Script.RefreshUI(data);
        }
        public override void OnShow()
        {
            this.MaskClickClose = true;
            //RefreshPlay();
            RefreshUI();

            EventDispatcher.AddEvent(GEnum.NamedEvent.EMjControlCloseMaskUI, CloseUI);
        }

        public override void OnHide()
        {

            EventDispatcher.RemoveEvent(GEnum.NamedEvent.EMjControlCloseMaskUI, CloseUI);
        }

        private void CloseUI(object[] values)
        {
            this.Hide();
        }
    }

}
