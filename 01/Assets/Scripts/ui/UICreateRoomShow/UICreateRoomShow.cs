using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace projectQ
{
    public class UICreateRoomShow : UIViewBase
    {
        public UITable TableCenter;
        public UIScrollView scrollView;
        public GameObject CloseBtn;
        public UISprite CheckToggle;
        public GameObject RuleBtn;
        public UILabel TitleLabel;

        private MjDeskInfo m_MjData;

        public override void OnPushData(object[] data)
        {
            if (data != null && data.Length > 0)
            {
                MjDeskInfo deskInfo = data[0] as MjDeskInfo;

                if (deskInfo != null)
                {
                    m_MjData = deskInfo;
                    ShowContent();
                }
            }
        }

        public override void Init()
        {
            UIEventListener.Get(CloseBtn).onClick = OnClickClost;
            UIEventListener.Get(RuleBtn).onClick = OnClickRule;
            EventDispatcher.AddEvent(GEnum.NamedEvent.EMjControlCloseMaskUI, CloseUI);

        }

        protected override void OnClose()
        {
            EventDispatcher.RemoveEvent(GEnum.NamedEvent.EMjControlCloseMaskUI, CloseUI);
            base.OnClose();
        }

        public override void OnHide()
        {

        }

        public override void OnShow()
        {

        }

        private void CloseUI(object[] vars)
        {
            this.Close();
        }

        private void OnClickClost(GameObject obj)
        {
            this.Close();
        }

        private void OnClickRule(GameObject obj)
        {
            LoadUIMain("UIRuleSmall", m_MjData.mjGameConfigId);
        }

        private void ShowContent()
        {
            MahjongPlay data = MemoryData.MahjongPlayData.GetMahjongPlayByConfigId(m_MjData.mjGameConfigId);

            CheckToggle.alpha = m_MjData.viewScore ? 1 : 0;
            TitleLabel.text = data.Name;

            UITools.CreateChild<MahjongPlayRule>(TableCenter.transform, null, data.RuleList, (ruleGo, ruleData) =>
            {
                var tempScript = ruleGo.GetComponent<UICreateRoomRuleItem>();
                tempScript.Init(ruleData, null, true);
            });
            TableCenter.Reposition();

            Transform trans = TableCenter.transform.GetChild(0);
            if (trans != null)
            {
                Transform line = trans.Find("Line");
                if (line != null)
                {
                    UISprite linesp = line.GetComponent<UISprite>();
                    linesp.alpha = 0;
                }
            }

            StartCoroutine(ResetPos());
        }

        private IEnumerator ResetPos()
        {
            yield return null;
            scrollView.ResetPosition();
        }
    }
}
